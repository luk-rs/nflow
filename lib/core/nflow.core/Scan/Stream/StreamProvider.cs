using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using nflow.core.Abstractions;

namespace nflow.core.Scan.Stream
{
    public class StreamProvider
    {
        private IServiceProvider _privateStreamsProvider;
        private IServiceProvider _publicStreamsProvider;

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
            var stream = publicStreams.SingleOrDefault(s => s.GetType().IsAssignableFrom(typeof(T)));

            return stream as T;
        }
        
        public T Private<T>() where T : class, IStream
        {
            var privateStreams = _privateStreamsProvider.GetServices<IStream>();
            var stream = privateStreams.SingleOrDefault(s => s.GetType().IsAssignableFrom(typeof(T)));

            return stream as T;
        }
    }
}