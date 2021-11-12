namespace nflow.core
{
    using System;
    using System.Linq;

    public interface IOracles
    {
        IObservable<TStream> Query<TStream>() where TStream : IPersistedStream;

    }

    internal sealed class Oracles : IOracles
    {

        IObservable<TStream> IOracles.Query<TStream>() => throw new NotImplementedException();

        public Oracles(IMicrosResolver micros)
        {
            _persisted = micros.All
                .SelectMany(micro => micro.PublicStreams)
                .Distinct()
                .Where(stream => stream.GetType().IsSubclassOf(typeof(IPersistedStream)))
                .ToArray();
        }

        private IStream[] _persisted;



    }
}
