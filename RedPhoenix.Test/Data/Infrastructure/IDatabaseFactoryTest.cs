namespace RedPhoenix.Test.Data.Infrastructure;

using FluentAssertions;
using Moq;
using RedPhoenix.Data.Infrastructure;

[TestClass]
// ReSharper disable once InconsistentNaming
public class IDatabaseFactoryTest
{
    [TestMethod]
    public void sut_implement_IDisposable()
    {
        var sut = new Mock<IDatabaseFactory>();
        sut.Object.Should().BeAssignableTo<IDisposable>();
    }
}

