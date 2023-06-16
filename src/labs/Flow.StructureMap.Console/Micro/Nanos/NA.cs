namespace Flow.StructureMap.Console.Micro.Nanos
{

    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Reactive.Services;
    using Services;
    using Streams;


    internal class NA : TriggerNano<int>
    {

        private readonly IFoo _foo;

        public NA(IFoo foo) => _foo = foo;

        public override IObservable<Unit> Connect() => Trigger.Update<int, B>(this, (nextInt, b) => b.Control = nextInt);

        protected override IObservable<int> Trigger => Observable.Return(_foo.Next);

    }

}