namespace streams.Test.Nanos
{
	using System;
	using System.Reactive;
	using System.Reactive.Linq;
	using nflow.core;
	using streams.Test.Commands;
	using streams.Test.Services;
	using streams.Test.Streams;

	internal class NanoZ : INanoService
	{

		public IObservable<Unit> Connect(IBus bus)
		{

			var handler = bus
			.Commands
			.Handle<CommandZ>()
			.Do(_ => bus.Whispers.Send<StreamZ>(new StreamZ($"Hello NANO whisper [ CMD | {_.Name} ]", 2)))
			.Select(_ii => Unit.Default);

			IObservable<Unit> send(string msg)
			=> Observable
			.Return(Unit.Default)
			.Delay(TimeSpan.FromSeconds(5))
			.Do(_ => SendViaBus(bus, msg + ""));



			return new[] { handler, send("Hello Commands"), send("Hello Santos") }.Merge();
		}

		private static void SendViaBus(IBus bus, string msg)
		=> bus.Commands.Send<CommandZ>(new CommandZ(msg));

		private readonly Ii _ii;

		public NanoZ(Ii ii)
		{
			_ii = ii;
		}

	}
}