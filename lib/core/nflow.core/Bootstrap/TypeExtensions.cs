namespace nflow.core
{
	using System;
	using System.Linq;

	internal static class TypeExtensions
	{
		private static object get_default_value(this Type type)
		{
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}
			return null;
		}

		public static IStream generate_sample(this Type type)
		{
			var @default = type.GetConstructors()
			.Select(ctor => (ctor, @params: ctor.GetParameters()))
			.OrderBy(construction => construction.@params)
			.Select(construction =>
			{
				var args = construction
					 .@params
					 .Select(param => param.GetType())
					 .Select(pType => pType.get_default_value())
					 .ToArray();

				return construction.ctor.Invoke(args);
			})
			.Cast<IStream>()
			.First();

			return @default as IStream;
		}

	}

}