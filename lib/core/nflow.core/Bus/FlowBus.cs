namespace nflow.core
{
	using System;
	using System.Linq;

	internal sealed class FlowBus : IBus
	{

		ICommandsDSL IBus.Commands => _commands;

		IOraclesDSL IBus.Oracles => _oracles;

		IWhispersDSL IBus.Whispers => _whispers;

		Registry IBus.Registry { get; } = new FlowRegistry();

		public FlowBus(
			IStream[] streams,
			Func<IStream[], IOraclesDSL> oracles,
			Func<IStream[], IWhispersDSL> whispers,
			Func<IStream[], ICommandsDSL> commands)
		{

			var @public = streams.Where(stream => stream.IsPublic);
			_public = @public.ToArray();

			TStream[] filter_all<TStream>()
						where TStream : IStream
						=> _public
						.Where(stream => stream is TStream)
						.Cast<TStream>()
						.ToArray();

			_oracles = oracles(filter_all<IOracle>());
			_whispers = whispers(filter_all<IWhisper>());
			_commands = commands(filter_all<ICommand>());
		}

		private readonly IStream[] _public;
		private readonly IWhispersDSL _whispers;
		private readonly ICommandsDSL _commands;
		private readonly IOraclesDSL _oracles;

		class FlowRegistry : Registry { }
	}
}
