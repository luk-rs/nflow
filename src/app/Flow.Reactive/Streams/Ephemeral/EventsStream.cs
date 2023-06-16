namespace Flow.Reactive.Streams.Ephemeral
{

    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;


    public abstract class EventsStream<TStreamData> : IEventsStream<TStreamData>
            where TStreamData : StreamData, IStreamData
    {

        public IObservable<TStreamData> Data => UpdatesReporter.Publish().RefCount();

        public TStreamData Notify(TStreamData newData)
        {
            UpdatesReporter.OnNext(newData);
            return newData;
        }

        public virtual bool Public => false;

        public void Dispose() { }

        protected SubjectBase<TStreamData> UpdatesReporter { get; } = new Subject<TStreamData>();

    }

    public abstract class PublicEventsStream<TStreamData> : EventsStream<TStreamData>
          where TStreamData : StreamData, IStreamData
    {
        public override bool Public => true;
    }
}