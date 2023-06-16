namespace Flow.Reactive.Streams.Persisted
{

    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;


    public abstract class PersistedStream<TStreamData> : IPersistedStream<TStreamData>
            where TStreamData : PersistedStreamData
    {

        public virtual IObservable<TStreamData> Data => UpdatesReporter;

        public abstract TStreamData InitialState { get; }

        public TStreamData Update(Action<TStreamData> update)
        {
            TStreamData data = default;
            UpdateRequests.OnNext(state =>
            {
                update(state);
                data = state;
                return state;
            });
            return data;
        }

        public virtual bool Public => false;


        #region construction

        protected PersistedStream()
        {
            UpdatesReporter = new BehaviorSubject<TStreamData>(InitialState);

            UpdateSubscription = UpdateRequests.WithLatestFrom(Data, (request, state) => (request, state))
                                               .Select(update => (update.request, clonedState: (TStreamData)update.state.Clone()))
                                               .Select(update => update.request(update.clonedState))
                                               .Do(newState => UpdatesReporter.OnNext(newState))
                                               .Subscribe();
        }

        private IDisposable UpdateSubscription { get; }

        public void Reset() => UpdatesReporter.OnNext(InitialState);

        public void Dispose() => UpdateSubscription.Dispose();

        #endregion


        private Subject<Func<TStreamData, TStreamData>> UpdateRequests { get; } = new Subject<Func<TStreamData, TStreamData>>();
        private BehaviorSubject<TStreamData> UpdatesReporter { get; }

    }

    public abstract class PublicPersistedStream<TStreamData> : PersistedStream<TStreamData>
           where TStreamData : PersistedStreamData
    {
        public override bool Public => true;
    }
}