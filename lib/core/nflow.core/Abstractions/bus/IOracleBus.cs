namespace nflow.core
{
    using System;

    public interface IOracleBus<TOracle> where TOracle : IOracle
    {
        void Inform(Action<TOracle> change);
        IObservable<TOracle> Query { get; }


    }
}

