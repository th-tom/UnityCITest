using System.Collections;
using NUnit.Framework;
using UnityCITest;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// PlayMode 行为测试:进 Play 模式验证时间驱动逻辑(回血节奏、边界行为)。
/// </summary>
public class HealthRegenTests
{
    [UnityTest]
    public IEnumerator Regen_HealsOverTime()
    {
        var go = new GameObject();
        var regen = go.AddComponent<HealthRegen>();
        regen.Configure(maxHealth: 100, regenPerTick: 10, tickInterval: 0.1f);

        regen.Health.TakeDamage(30);
        Assert.AreEqual(70, regen.Health.Current);

        yield return new WaitForSeconds(0.35f); // 约 3 个 tick

        Assert.Greater(regen.Health.Current, 70, "经过若干 tick 后血量应已回升");
        Object.Destroy(go);
    }

    [UnityTest]
    public IEnumerator Regen_StopsAtMax()
    {
        var go = new GameObject();
        var regen = go.AddComponent<HealthRegen>();
        regen.Configure(maxHealth: 100, regenPerTick: 50, tickInterval: 0.05f);

        regen.Health.TakeDamage(10);
        yield return new WaitForSeconds(0.5f);

        Assert.AreEqual(100, regen.Health.Current, "回血不应溢出上限");
        Object.Destroy(go);
    }

    [UnityTest]
    public IEnumerator Dead_DoesNotRegen()
    {
        var go = new GameObject();
        var regen = go.AddComponent<HealthRegen>();
        regen.Configure(maxHealth: 100, regenPerTick: 10, tickInterval: 0.05f);

        regen.Health.TakeDamage(100);
        Assert.IsTrue(regen.Health.IsDead);

        yield return new WaitForSeconds(0.2f);

        Assert.AreEqual(0, regen.Health.Current, "死亡后不应自行回血");
        Object.Destroy(go);
    }

    [UnityTest]
    public IEnumerator Configure_ResetsHealthToFull()
    {
        var go = new GameObject();
        var regen = go.AddComponent<HealthRegen>();
        regen.Configure(maxHealth: 100, regenPerTick: 10, tickInterval: 0.05f);

        regen.Health.TakeDamage(40);
        Assert.AreEqual(60, regen.Health.Current);

        regen.Configure(maxHealth: 50, regenPerTick: 1, tickInterval: 1f);

        Assert.AreEqual(50, regen.Health.Current, "重新配置后应重建为满血");
        Object.Destroy(go);

        yield return null;
    }
}
