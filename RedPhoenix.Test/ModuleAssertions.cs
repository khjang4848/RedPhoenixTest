namespace RedPhoenix.Test;

using Autofac;
using FluentAssertions;

public static class ModuleAssertions
{
    public static void AssertServiceRegistered<T>(this IContainer container)
        where T : class
    {
        var service = container.ResolveOptional<T>();
        service.Should().NotBeNull();
    }

    public static void AssertServiceRegistered<T>(
        this IContainer container, T expected)
        where T : class
    {
        var service = container.ResolveOptional<T>();
        service.Should().NotBeNull().And.Be(expected);
    }

    public static void AssertServiceRegistered<TService, TImplementation>(
        this IContainer container)
        where TService : class
        where TImplementation : TService
    {
        var service = container.ResolveOptional<TService>();
        service.Should().BeOfType<TImplementation>();
    }
}

