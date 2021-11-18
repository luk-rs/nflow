namespace nflow.core
{
    using System;
    using System.Reactive.Subjects;

    internal sealed class Oracle<TOracle> : Hook<TOracle>, IOracleBus<TOracle>
    where TOracle : IOracle
    {
        IObservable<TOracle> IOracleBus<TOracle>.Query => Self.Socket;

        void IOracleBus<TOracle>.Inform(Action<TOracle> change)
        {
            var subj = Self.Socket as BehaviorSubject<TOracle>;
            var value = subj.Value;
            change(value);
            Self.Route(value);
        }
    }
}

