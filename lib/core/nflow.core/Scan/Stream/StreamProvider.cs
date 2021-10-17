using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using nflow.core.Abstractions;

namespace nflow.core.Scan.Stream
{
    public class StreamProvider
    {
        private readonly IServiceProvider _privateStreamsProvider;
        private readonly IServiceProvider _publicStreamsProvider;

        public StreamProvider(IServiceCollection streams)
        {
            var privateStreams = new ServiceCollection();
            var publicStreams = new ServiceCollection();

            var streamsProvider = streams.BuildServiceProvider();
            var allStreams = streamsProvider.GetServices<IStream>();

            allStreams
                .ToList()
                .ForEach(stream =>
                {
                    var collection = stream.IsPublic
                        ? publicStreams
                        : privateStreams;

                    collection.AddSingleton(stream);
                });

            _privateStreamsProvider = privateStreams.BuildServiceProvider();
            _publicStreamsProvider = publicStreams.BuildServiceProvider();
        }

        public T Public<T>() where T : class, IStream
        {
            var publicStreams = _publicStreamsProvider.GetServices<IStream>();
            return Filter<T>(publicStreams)
                .SingleOrDefault();
        }

        public T Private<T>(string microNamespace) where T : class, IStream
        {
            var privateStreams = _privateStreamsProvider.GetServices<IStream>();
            return Filter<T>(privateStreams, microNamespace)
                .SingleOrDefault();
        }

        public IEnumerable<IStream> Micro(string microNamespace)
        {
            var privateStreams = _privateStreamsProvider.GetServices<IStream>();
            var publicStreams = _publicStreamsProvider.GetServices<IStream>();
            var allStreams = privateStreams.Concat(publicStreams);

            return Filter<IStream>(allStreams, microNamespace);
        }

        private static IEnumerable<T> Filter<T>(IEnumerable<IStream> streams, string microNamespace = default)
            where T : class, IStream
        {
            var filtered = streams
                .Where(s => s is T)
                .Where(s => microNamespace == default || s.GetType().Namespace!.Contains(microNamespace))
                .Cast<T>();

            return filtered;
        }
    }
}