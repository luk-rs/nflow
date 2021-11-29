namespace streams.MicroServiceA.NanoServices
{
	using System;
	using System.Reactive;
	using System.Reactive.Linq;
	using nflow.core;
	using streams.MicroServiceA.Streams;

	public sealed class FooEventListener : INanoService
	{
		public IObservable<Unit> Connect(IBus bus) =>
			 bus
			 	.Whispers
				 .Listen<FooEvent>()
				 .Do(@event => Console.WriteLine($"Event {@event.Value}"))
				 .Each();
		//   .Handle<UpdateSomethingCommand>()
		//   .AndUpdate<Bar>(bus, (command, stream) => stream.SomeValue++);
	}
}

