namespace Flow.Castle.Windsor.Domain.Micro.NanoServices
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Flow.Castle.Windsor.Domain.Micro2.Streams;
    using Reactive.Services;
    using Services;
    using Streams;


    public class NA : TriggerNano<int>
    {

        private readonly IFoo _foo;

        public NA(IFoo foo) => _foo = foo;

        public override IObservable<Unit> Connect() => Trigger.Update<int, B>(this, (nextInt, b) => b.Control = nextInt);

        protected override IObservable<int> Trigger => Observable.Return(_foo.Next);

    }

}