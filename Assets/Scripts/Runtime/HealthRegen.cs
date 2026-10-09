using System;
using UnityEngine;

namespace UnityCITest
{
    /// <summary>
    /// 按固定间隔回血的行为组件,由 PlayMode 测试覆盖(时间驱动逻辑)。
    /// 死亡(血量为 0)后不再回血;满血时停止回血。
    /// </summary>
    public sealed class HealthRegen : MonoBehaviour
    {
        [SerializeField] private int m_MaxHealth = 100;
        [SerializeField] private int m_RegenPerTick = 5;
        [SerializeField] private float m_TickInterval = 1f;

        private Health m_Health;
        private float m_Elapsed;

        public Health Health => m_Health;

        private void Awake()
        {
            m_Health = new Health(m_MaxHealth);
        }

        /// <summary>测试注入点:重建内部状态并配置回血参数。</summary>
        public void Configure(int maxHealth, int regenPerTick, float tickInterval)
        {
            if (tickInterval <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(tickInterval), "Tick interval must be positive.");
            }

            m_MaxHealth = maxHealth;
            m_RegenPerTick = regenPerTick;
            m_TickInterval = tickInterval;
            m_Health = new Health(maxHealth);
            m_Elapsed = 0f;
        }

        private void Update()
        {
            if (m_Health == null || m_Health.IsDead || m_Health.Current >= m_Health.Max)
            {
                return;
            }

            m_Elapsed += Time.deltaTime;
            while (m_Elapsed >= m_TickInterval)
            {
                m_Elapsed -= m_TickInterval;
                m_Health.Heal(m_RegenPerTick);
            }
        }
    }
}
