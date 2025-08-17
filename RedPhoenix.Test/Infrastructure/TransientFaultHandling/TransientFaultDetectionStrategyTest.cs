namespace RedPhoenix.Test.Infrastructure.TransientFaultHandling;

using AutoFixture.Idioms;
using AutoFixture;
using FluentAssertions;
using RedPhoenix.Infrastructure.TransientFaultHandling;

[TestClass]
public class TransientFaultDetectionStrategyTest
{
    [TestMethod]
    public void sut_has_IsTransientException_method()
    {
        var sut = typeof(TransientFaultDetectionStrategy);
        const string methodName = "IsTransientException";
        sut.Should().HaveMethod(methodName, [typeof(Exception)]);
        var mut = sut.GetMethod(methodName);
        mut.ReturnType.Should().Be(typeof(bool), "return type of the method should be bool");
        mut.Should().BeVirtual();
    }

    [TestMethod]
    public void IsTransientException_returns_true()
    {
        var sut = new TransientFaultDetectionStrategy();
        var fixture = new Fixture();
        var exception = fixture.Create<Exception>();

        var actual = sut.IsTransientException(exception);

        actual.Should().BeTrue();
    }

    [TestMethod]
    public void IsTransientException_has_guard_clause()
    {
        var mut = typeof(TransientFaultDetectionStrategy).GetMethod("IsTransientException");
        new GuardClauseAssertion(new Fixture()).Verify(mut);
    }
}