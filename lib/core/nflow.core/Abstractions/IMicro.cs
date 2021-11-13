

namespace nflow.core
{
    using System.Collections.Generic;
    using System.Linq;

    internal interface IMicro
    {
        string Name => Registry.GetType().Name;
        string Namespace => Registry.Namespace;
        Registry Registry { get; }
        IBus Bus { get; }
        IEnumerable<IStream> PublicStreams => Streams.Where(stream => stream.IsPublic);
        IEnumerable<IStream> InternalStreams => Streams.Except(PublicStreams);
        IEnumerable<IStream> Streams { get; }
        IEnumerable<IHook> Hooks => Oracles.Concat(Whispers).Concat(Instructions);
        IEnumerable<IHook> Oracles { get; }
        IEnumerable<IHook> Whispers { get; }
        IEnumerable<IHook> Instructions { get; }

    }

    internal class Micro : IMicro
    {
        IMicro Self => this as IMicro;
        IBus IMicro.Bus => _bus;

        IEnumerable<IStream> IMicro.Streams => _mStreams;
        IEnumerable<IHook> IMicro.Hooks => _oHooks.Concat(_wHooks).Concat(_iHooks);

        Registry IMicro.Registry => _mRegistry;

        IEnumerable<IHook> IMicro.Oracles => _oHooks;

        IEnumerable<IHook> IMicro.Whispers => _wHooks;

        IEnumerable<IHook> IMicro.Instructions => _iHooks;

        public Micro(Registry registry, IStreamsResolver streams, INanosResolver nanos)
        {
            _mRegistry = registry;
            _mStreams = streams.Types.Of(registry.Namespace).ToArray();
            _oHooks = streams.Hooks.OraclesOf(registry.Namespace).ToArray();
            _wHooks = streams.Hooks.WhispersOf(registry.Namespace).ToArray();
            _iHooks = streams.Hooks.InstructionsOf(registry.Namespace).ToArray();
            _mNanos = nanos.Of(registry.Namespace).ToArray();
            _bus = new MicroBus(registry, streams);

        }
        private readonly Registry _mRegistry;
        private readonly IStream[] _mStreams;
        private readonly IHook[] _oHooks;
        private readonly IHook[] _wHooks;
        private readonly IHook[] _iHooks;
        private readonly INano[] _mNanos;
        private readonly IBus _bus;
    }
}