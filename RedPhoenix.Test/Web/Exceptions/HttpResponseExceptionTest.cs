namespace RedPhoenix.Test.Web.Exceptions;

using AutoFixture.Idioms;
using AutoFixture;
using System.Reflection;
using FluentAssertions;

using RedPhoenix.Web.Exceptions;

[TestClass]
public class HttpResponseExceptionTest
{
    [TestMethod]
    public void sut_inherits_ApplicationException()
        => typeof(HttpResponseException).BaseType.Should().Be(typeof(ApplicationException));

    [TestMethod]
    public void sut_has_guard_clauses()
    {
        var sut = typeof(HttpResponseException);
        new GuardClauseAssertion(new Fixture())
            .Verify(sut.GetConstructors(BindingFlags.Public));
    }
}