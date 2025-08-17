namespace RedPhoenix.Test.Data.Infrastructure;

using FluentAssertions;
using RedPhoenix.Data.Infrastructure;
using RedPhoenix.Data.Models;


[TestClass]
public class DatabaseFactoryTest
{
    [TestMethod]
    public void sut_implement_IDatabaseFactory()
    {
        typeof(DatabaseFactory).Should().Implement<IDatabaseFactory>();
        typeof(DatabaseFactory).Should().BeAssignableTo<Disposable>();
    }

    [TestMethod]
    public void VerifyAppDomainHasConfigurationSettings()
    {
        var sut = new DatabaseFactory();
        var dataContext = sut.Get();

        dataContext.Should().NotBeNull();
        dataContext.Should().BeOfType<RedPhoenixContext>();
    }
}
