namespace nflow.core
{
    using System;

    public interface IOraclesDSL
    {
        void Update<TOracle>(Action<TOracle> whisper)
        where TOracle : IOracle;

        IObservable<TOracle> Query<TOracle>()
        where TOracle : IOracle;
    }
}
