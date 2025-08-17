namespace RedPhoenix.Test;

using System.Collections.Immutable;
using AutoFixture;
using AutoFixture.Kernel;
using System.Reflection;

public class ImmutableArrayCustomization : ICustomization
{
    public void Customize(IFixture fixture)
        => fixture.Customizations.Add(new ImmutableArrayBuilder(fixture));

    private class ImmutableArrayBuilder(IFixture builder) : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            return request switch
            {
                Type type when IsImmutableArrayType(type)
                    => GenerateImmutableArrayInstance(type),
                _ => new NoSpecimen()
            };
        }

        private static bool IsImmutableArrayType(Type type)
            => type is { IsValueType: true, IsGenericType: true }
               && type.GetGenericTypeDefinition() == typeof(ImmutableArray<>);

        private object GenerateImmutableArrayInstance(Type type)
        {
            var elemType = type.GenericTypeArguments[0];
            var template = typeof(ImmutableArrayBuilder).GetMethod(
                nameof(GenerateImmutableArray),
                BindingFlags.NonPublic | BindingFlags.Instance);

            return template!.MakeGenericMethod(elemType).Invoke(this, null)!;
        }

        private ImmutableArray<T> GenerateImmutableArray<T>()
            => [.. builder.CreateMany<T>()];


    }
}