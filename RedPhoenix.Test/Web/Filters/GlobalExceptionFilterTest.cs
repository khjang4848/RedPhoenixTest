namespace RedPhoenix.Test.Web.Filters
{
    using AutoFixture.Idioms;
    using AutoFixture;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System.Reflection;

    using RedPhoenix.Web.Filters;

    [TestClass]
    public class GlobalExceptionFilterTest
    {
        [TestMethod]
        public void sut_implement_IExceptionFilters()
        {
            var sut = new GlobalExceptionFilter(Mock.Of<ILoggerFactory>());
            sut.Should().BeAssignableTo<IExceptionFilter>();
        }

        [TestMethod]
        public void sut_implement_IDisposable()
        {
            var sut = new GlobalExceptionFilter(Mock.Of<ILoggerFactory>());
            sut.Should().BeAssignableTo<IDisposable>();
        }

        [TestMethod]
        public void sut_has_guard_clauses()
        {
            var sut = typeof(GlobalExceptionFilter);
            new GuardClauseAssertion(new Fixture())
                .Verify(sut.GetConstructors(BindingFlags.Public));
        }
    }
}
