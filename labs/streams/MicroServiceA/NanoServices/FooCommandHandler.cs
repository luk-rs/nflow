namespace streams.MicroServiceA.NanoServices
{
	using System;
	using System.Reactive;
	using System.Reactive.Linq;
	using nflow.core;
	using streams.MicroServiceA.Commands;
	using streams.MicroServiceA.Streams;

	public sealed class FooCommandHandler : INanoService
	{
		private int _eventIdx = 0;

		public IObservable<Unit> Connect(IBus bus)
		=> bus
		.Commands
		.Handle<FooCommand>()
		.Do(_ => bus.Whispers.Send(new FooEvent(_eventIdx++)))
		.Each();
	}
}

