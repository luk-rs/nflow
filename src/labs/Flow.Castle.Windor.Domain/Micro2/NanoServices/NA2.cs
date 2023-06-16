namespace Flow.Castle.Windsor.Domain.Micro2.NanoServices
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Reactive.Services;
    using Services;
    using Streams;


    public class NA2 : TriggerNano<int>
    {

        private readonly IFoo _foo;

        public NA2(IFoo foo) => _foo = foo;

        public override IObservable<Unit> Connect() => Trigger.Update<int, B2>(this, (nextInt, b) => b.Control = nextInt);

        protected override IObservable<int> Trigger => Observable.Return(_foo.Next);

    }

}