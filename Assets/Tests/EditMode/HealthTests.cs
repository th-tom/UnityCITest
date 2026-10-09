using System;
using NUnit.Framework;
using UnityCITest;

/// <summary>
/// EditMode 纯逻辑测试:运行快、不进 Play 模式,适合血量/数值类规则。
/// </summary>
public class HealthTests
{
    [Test]
    public void NewHealth_DefaultsToFull()
    {
        var health = new Health(100);

        Assert.AreEqual(100, health.Current);
        Assert.IsFalse(health.IsDead);
    }

    [Test]
    public void NewHealth_InitialValue_IsApplied()
    {
        var health = new Health(100, 60);

        Assert.AreEqual(60, health.Current);
    }

    [Test]
    public void NewHealth_InitialGreaterThanMax_ClampsToMax()
    {
        var health = new Health(50, 80);

        Assert.AreEqual(50, health.Current);
    }

    [Test]
    public void NewHealth_NonPositiveMax_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Health(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Health(-5));
    }

    [Test]
    public void TakeDamage_ReducesCurrent()
    {
        var health = new Health(100);

        health.TakeDamage(30);

        Assert.AreEqual(70, health.Current);
        Assert.IsFalse(health.IsDead);
    }

    [Test]
    public void TakeDamage_Overkill_ClampsToZeroAndDies()
    {
        var health = new Health(100);

        health.TakeDamage(999);

        Assert.AreEqual(0, health.Current);
        Assert.IsTrue(health.IsDead);
    }

    [Test]
    public void TakeDamage_NegativeAmount_Throws()
    {
        var health = new Health(100);

        Assert.Throws<ArgumentOutOfRangeException>(() => health.TakeDamage(-1));
    }

    [Test]
    public void Heal_RestoresCurrent()
    {
        var health = new Health(100, 40);

        health.Heal(30);

        Assert.AreEqual(70, health.Current);
    }

    [Test]
    public void Heal_ClampsAtMax()
    {
        var health = new Health(100, 90);

        health.Heal(50);

        Assert.AreEqual(100, health.Current);
    }

    [Test]
    public void Heal_NegativeAmount_Throws()
    {
        var health = new Health(100);

        Assert.Throws<ArgumentOutOfRangeException>(() => health.Heal(-1));
    }

    [Test]
    public void Dead_ThenHeal_Revives()
    {
        var health = new Health(100);

        health.TakeDamage(100);
        Assert.IsTrue(health.IsDead);

        health.Heal(10);
        Assert.AreEqual(10, health.Current);
        Assert.IsFalse(health.IsDead);
    }
}
