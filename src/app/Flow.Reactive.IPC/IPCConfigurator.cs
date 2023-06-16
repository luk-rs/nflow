namespace Flow.Reactive.IPC
{
    using Flow.Reactive.IPC.Middleware;
    using Flow.Reactive.Streams.Ephemeral;
    using Flow.Reactive.Streams.Ephemeral.Commands;
    using Flow.Reactive.Streams.Middleware;
    using Flow.Reactive.Streams.Persisted;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class IPCConfigurator
    { 
        internal static List<Type> CommandTypes { get; }
        internal static List<Type> StreamTypes { get; }

        static IPCConfigurator()
        {
            CommandTypes = Assembly
                .GetEntryAssembly()
                .ExportedTypes
                .Where(type => typeof(Command).IsAssignableFrom(type))
                .ToList();

            StreamTypes = Assembly
                .GetEntryAssembly()
                .ExportedTypes
                .Where(type => typeof(StreamData).IsAssignableFrom(type) ||
                               typeof(PersistedStreamData).IsAssignableFrom(type))
                .ToList();
        }

        internal static string AssemblyName { get; } = Assembly.GetEntryAssembly().GetName().Name;

        internal static List<string> RemoteApps { get; private set; } = new();

        public static void SetCommunicationWith(params string[] remoteApps) => RemoteApps.AddRange(remoteApps);

        public static MicroRegistry Registry
        {
            get
            {
                var serviceCollection = new ServiceCollection();

                serviceCollection.AddSingleton<IMiddleware, DispatcherMiddleware>();
                serviceCollection.AddSingleton<IMediator, Mediator>();

                return new("IPCMicroService", typeof(IPCConfigurator).Assembly, serviceCollection);
            }
        }

        internal static bool IsPersistedStreamData(Type streamDataType) => 
            typeof(PersistedStreamData).IsAssignableFrom(streamDataType);

        internal static Type GetStreamType(string typeName) =>
            StreamTypes.First(type => type.Name == typeName);
    }
}
