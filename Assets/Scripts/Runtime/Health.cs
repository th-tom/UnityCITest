using System;

namespace UnityCITest
{
    /// <summary>
    /// 纯逻辑血量类(不依赖 UnityEngine),由 EditMode 测试覆盖。
    /// </summary>
    public sealed class Health
    {
        public int Max { get; }

        public int Current { get; private set; }

        public bool IsDead => Current <= 0;

        /// <param name="max">最大血量,必须为正。</param>
        /// <param name="initial">初始血量;负数表示默认满血;超过 max 时钳制到 max。</param>
        public Health(int max, int initial = -1)
        {
            if (max <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "Max must be positive.");
            }

            Max = max;

            if (initial < 0)
            {
                Current = max;
            }
            else
            {
                Current = initial > max ? max : initial;
            }
        }

        /// <summary>受到伤害,血量钳制到 0,不会出现负值。</summary>
        public void TakeDamage(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Damage must not be negative.");
            }

            Current = Math.Max(0, Current - amount);
        }

        /// <summary>恢复血量,钳制到 Max,不会溢出。</summary>
        public void Heal(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Heal must not be negative.");
            }

            Current = Math.Min(Max, Current + amount);
        }
    }
}
