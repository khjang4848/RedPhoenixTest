namespace RedPhoenix.Test;

using AutoFixture;
using System.Reflection;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class NoAutoPropertiesAttribute : CustomizeAttribute
{
    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
        if (parameter == null) throw new ArgumentNullException(nameof(parameter));
        var targetType = parameter.ParameterType;
        return new NoAutoPropertiesCustomization(targetType);
    }
}

