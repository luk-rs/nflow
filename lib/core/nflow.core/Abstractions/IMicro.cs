

namespace nflow.core
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IMicro
    {
        string Name => Registry.GetType().Name;
        Registry Registry { get; }
        IBus Bus { get; }
        IEnumerable<IStream> PublicStreams => Streams.Where(stream => stream.IsPublic);
        IEnumerable<IStream> InternalStreams => Streams.Except(PublicStreams);
        IEnumerable<IStream> Streams { get; }

    }

    internal class Micro : IMicro
    {

        IBus IMicro.Bus => throw new System.NotImplementedException();

        IEnumerable<IStream> IMicro.Streams => _mStreams;

        Registry IMicro.Registry => _mRegistry;

        public Micro(Registry registry, IStreamsResolver streams, INanosResolver nanos)
        {
            _mRegistry = registry;
            _mStreams = streams.Of(registry.Namespace).ToArray();
            _mNanos = nanos.Of(registry.Namespace).ToArray();

        }
        private readonly Registry _mRegistry;
        private readonly IStream[] _mStreams;
        private readonly INano[] _mNanos;
    }
}