namespace nflow.core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;

	internal class CommandsDSL : ICommandsDSL
	{
		IObservable<TCommand> ICommandsDSL.Handle<TCommand>()
		=> OnValidatedCarrier(carrier => carrier.Hook<TCommand>());
		void ICommandsDSL.Send<TCommand>(TCommand command)
		=> OnValidatedCarrier<TCommand>(carrier => carrier.Route(command))
		.ObserveOn(Scheduler.Default)
		.Subscribe();


		public CommandsDSL(IStream[] streams, IStreamCarrier[] commands)
		{
			_commands = commands
			.Where(carrier => streams.Any(stream => carrier.Carrying(stream)))
			.ToArray();
		}

		private readonly IStreamCarrier[] _commands;

		private IObservable<TCommand> OnValidatedCarrier<TCommand>(Func<IStreamCarrier, IObservable<TCommand>> selector)
		where TCommand : ICommand
		{
			var command = _commands.SingleOrDefault(carrier => carrier.Carrying<TCommand>());

			return command == default
			? throw new ArgumentOutOfRangeException($"There is no instance of scanned command carrier that matches {typeof(TCommand)}")
			: selector(command);
		}
		private IObservable<TCommand> OnValidatedCarrier<TCommand>(Action<IStreamCarrier> selector)
		where TCommand : ICommand
		=> OnValidatedCarrier(carrier =>
			 {
				 selector(carrier);
				 return Observable.Empty<TCommand>();
			 });
	}
}
