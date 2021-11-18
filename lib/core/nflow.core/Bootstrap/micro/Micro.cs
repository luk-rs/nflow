

namespace nflow.core
{
    using System.Collections.Generic;
    using System.Linq;

    internal class Micro : IMicro
    {
        IMicro Self => this as IMicro;
        IBus IMicro.Bus => _bus;
        Registry IMicro.Registry => _mRegistry;
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