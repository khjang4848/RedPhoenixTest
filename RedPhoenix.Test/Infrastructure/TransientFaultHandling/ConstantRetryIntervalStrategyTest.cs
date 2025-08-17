﻿namespace RedPhoenix.Test.Infrastructure.TransientFaultHandling;

using AutoFixture;
using FluentAssertions;

using RedPhoenix.Infrastructure.TransientFaultHandling;

[TestClass]
public class ConstantRetryIntervalStrategyTest
{
    [TestMethod]
    public void sut_inherits_RetryIntervalStrategy()
        => typeof(ConstantRetryIntervalStrategy).BaseType.Should()
            .Be(typeof(RetryIntervalStrategy));

    [TestMethod]
    public void sut_has_Interval_property()
        => typeof(ConstantRetryIntervalStrategy)
            .Should().HaveProperty<TimeSpan>("Interval")
            .Which.Should().NotBeWritable();

    [TestMethod]
    public void constructor_sets_Interval_correctly()
    {
        var fixture = new Fixture();
        var interval = fixture.Create<TimeSpan>();

        var sut = new ConstantRetryIntervalStrategy(interval, false);

        sut.Interval.Should().Be(interval);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void constructor_sets_ImmediateFirstRetry_correctly(bool immediateFirstRetry)
    {
        var fixture = new Fixture();
        var interval = fixture.Create<TimeSpan>();

        var sut = new ConstantRetryIntervalStrategy(interval, immediateFirstRetry);

        sut.ImmediateFirstRetry.Should().Be(immediateFirstRetry);
    }

    [TestMethod]
    public void modest_constructor_sets_ImmediateFirstRetry_to_false()
    {
        var sut = new ConstantRetryIntervalStrategy(TimeSpan.Zero);
        sut.ImmediateFirstRetry.Should().BeFalse();
    }

    [TestMethod]
    public void modest_constructor_sets_Interval_correctly()
    {
        var fixture = new Fixture();
        var interval = fixture.Create<TimeSpan>();

        var sut = new ConstantRetryIntervalStrategy(interval);

        sut.Interval.Should().Be(interval);
    }


    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void GetIntervalFromZeroBasedTick_returns_Interval_always(bool zeroRetried)
    {
        var fixture = new Fixture();
        var interval = fixture.Create<TimeSpan>();
        var sut = new ConstantRetryIntervalStrategy(interval, immediateFirstRetry: false);
        var retried = zeroRetried ? 0 : fixture.Create<int>();

        var actual = sut.GetInterval(retried);

        actual.Should().Be(interval);
    }
}