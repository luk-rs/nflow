using System;
using System.Linq;

namespace Flow.Reactive.Extensions
{

    internal static class ReflectionExtensions
    {

        public static object CreateParameterless(this Type type)
        {
            var constructors = type.GetConstructors()
                                   .Select(constructor => (constructor, parameters: constructor.GetParameters(), isGeneric: type.IsGenericType, typeParams: type.GetGenericArguments()))
                                   .ToList();

            if (constructors.None(constructor => constructor.parameters.Length == 0)) throw new Exception($"Extension {nameof(CreateParameterless)} only supports instances with parameterless constructor. Thus {type.Name} cannot be created");

            return constructors
                  .Where(construction => construction.parameters.Length == 0)
                  .Select(_ => Activator.CreateInstance(type))
                  .Single();
        }

    }

}
