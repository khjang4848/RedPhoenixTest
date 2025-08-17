namespace RedPhoenix.Test;

using AutoFixture;
using System.Reflection;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
public abstract class CustomizeAttribute : Attribute, IParameterCustomizationSource
{
    public abstract ICustomization GetCustomization(ParameterInfo parameter);

}
