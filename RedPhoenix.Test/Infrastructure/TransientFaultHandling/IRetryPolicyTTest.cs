namespace RedPhoenix.Test.Infrastructure.TransientFaultHandling;

using FluentAssertions;
using RedPhoenix.Infrastructure.TransientFaultHandling;

[TestClass]
public class IRetryPolicyTTest
{
    [TestMethod]
    public void sut_inherits_IRetryPolicy()
        => typeof(IRetryPolicy<>).Should().Implement<IRetryPolicy>();
}