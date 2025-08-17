namespace RedPhoenix.Test;

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using System.Reflection;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AutoDataAttribute : Attribute, ITestDataSource
{
    private readonly IFixture _builder = CreateBuilder();

    private static IFixture CreateBuilder()
    {
        var customization = new CompositeCustomization(
            new AutoMoqCustomization(),
            new ImmutableArrayCustomization());

        return new Fixture().Customize(customization);
    }

    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        yield return methodInfo.GetParameters().Select(Resolve).ToArray();
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        if (data == null) return null;

        var args = methodInfo
            .GetParameters()
            .Zip(data, (param, arg) => $"{param.Name}: {arg}");

        return $"{methodInfo.Name}({string.Join(", ", args)})";

    }

    private object Resolve(ParameterInfo parameter)
    {
        foreach (var customAttribute in parameter.GetCustomAttributes())
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (customAttribute is IParameterCustomizationSource attribute)
            {
                attribute.GetCustomization(parameter).Customize(_builder);
            }
        }

        return new SpecimenContext(_builder).Resolve(request: parameter);
    }
}
