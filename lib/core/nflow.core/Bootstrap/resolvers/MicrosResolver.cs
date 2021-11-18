namespace nflow.core
{
    using System.Collections.Generic;
    using System.Linq;

    internal interface IMicrosResolver
    {
        IEnumerable<IMicro> All { get; }
    }

    internal class MicrosResolver : IMicrosResolver
    {
        IEnumerable<IMicro> IMicrosResolver.All => _micros;

        private readonly IMicro[] _micros;
        public MicrosResolver(IEnumerable<Registry> registries, IStreamsResolver streams, INanosResolver nanos)
        {

            var regs = registries.ToList();

            _micros = regs.Select(registry => new Micro(registry, streams, nanos))
                        .ToArray();
        }


    }

}