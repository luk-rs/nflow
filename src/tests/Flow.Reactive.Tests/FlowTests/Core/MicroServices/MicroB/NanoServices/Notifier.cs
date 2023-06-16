namespace Flow.Reactive.Tests.FlowTests.Core.MicroServices.MicroB.NanoServices
{
    using Flow.Reactive.Services;
    using Flow.Reactive.Tests.FlowTests.Core.MicroServices.MicroA.Streams.Public;
    using System;
    using System.Reactive;

    public class Notifier : QueryNano<PersistedStream1Data>
    {
        public override IObservable<Unit> Connect()
        {
            return Query.Notify(this, (query) => new EventStream1Data(query.Count));
        }
    }
}
