namespace Flow.Reactive.DependencyInjection
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;


    public class Workspace
    {

        public static IEnumerable<Type> TypesFor<T>(Predicate<Type> typeFilter = default)
            => FilterAssemblies<T>(typeFilter)
              .Select(@group => @group.ToList())
              .SelectMany(types => types)
              .AsEnumerable();

        public static IEnumerable<Assembly> AssembliesWithTypes(Predicate<Type> typeFilter = default)
            => FilterAssemblies<object>(typeFilter)
              .Select(@group => @group.Key)
              .AsEnumerable();

        private static IEnumerable<IGrouping<Assembly, Type>> FilterAssemblies<T>(Predicate<Type> typeFilter = default)
            => Assemblies
              .AsParallel()
              .Select(assembly => assembly
                                 .GetTypes()
                                 .Where(type => !type.Attributes.HasFlag(TypeAttributes.Abstract))
                                 .Where(type => typeof(T).IsAssignableFrom(type))
                                 .Where(type => typeFilter?.Invoke(type) ?? true)
                                 .Select(type => (type, assembly)))
              .SelectMany(types => types)
              .AsEnumerable()
              .GroupBy(filter => filter.assembly, filter => filter.type);

        static Workspace()
        {
            Files.Select(path => new FileInfo(path))
                 .ToList()
                 .ForEach(file =>
                  {
                      var assemblies = DomainAssemblies.Select(assembly => assembly.GetName().Name);
                      if (!assemblies.Any(assembly => file.Name.StartsWith(assembly))) Assembly.LoadFrom(file.FullName);
                  });
        }

        static DirectoryInfo Location => new FileInfo(Assembly.GetEntryAssembly().Location).Directory;
        static IEnumerable<string> Dlls => Directory.EnumerateFiles(Location.FullName, "*.dll", SearchOption.AllDirectories);
        static IEnumerable<string> Exes => Directory.EnumerateFiles(Location.FullName, "*.exe", SearchOption.AllDirectories);
        static IEnumerable<string> Files => Dlls.Concat(Exes);

        static Assembly[] DomainAssemblies => AppDomain.CurrentDomain
                                                       .GetAssemblies();

        static IEnumerable<Assembly> Assemblies
            => DomainAssemblies
              .Where(assembly => !assembly.IsDynamic)
              .Where(assembly => Files.Any(file => file == assembly.Location))
              .Select(ass => new[] {ass}.AsEnumerable())
              .Aggregate((prev, cur) => prev.Concat(cur))
              .Select(x => x)
              .ToArray();

    }

}