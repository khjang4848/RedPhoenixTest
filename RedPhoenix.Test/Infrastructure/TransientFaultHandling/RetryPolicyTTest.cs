namespace RedPhoenix.Test.Infrastructure.TransientFaultHandling;

using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture;
using RedPhoenix.Infrastructure.TransientFaultHandling;

[TestClass]
public class RetryPolicyTTest
{
    public interface IFunctionProvider
    {
        TResult Func<T, TResult>(T arg);

        TResult Func<T1, T2, TResult>(T1 arg1, T2 arg2);
    }

    [TestMethod]
    public void sut_has_guard_clauses()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        var assertion = new GuardClauseAssertion(fixture);
        assertion.Verify(typeof(RetryPolicy<>));
    }
}
