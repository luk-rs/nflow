namespace nflow.core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	internal sealed class MicroBus : IBus
	{

		ICommandsDSL IBus.Commands => _commands;

		IOraclesDSL IBus.Oracles => _oracles;

		IWhispersDSL IBus.Whispers => _whispers;

		Registry IBus.Registry => _registry;


		public MicroBus(
			Registry registry,
			IStream[] streams,
			Func<IStream[], IOraclesDSL> oracles,
			Func<IStream[], IWhispersDSL> whispers,
			Func<IStream[], ICommandsDSL> commands
			)
		{
			_registry = registry;

			var @internal = new List<IStream>();
			var @public = new List<IStream>();

			foreach (var stream in streams)
			{
				Action retain = stream.IsPublic switch
				{
					true => () => @public.Add(stream),
					_ => stream.GetType().Namespace.StartsWith(registry.Namespace) switch
					{
						true => () => @internal.Add(stream),
						_ => () => { }
					}
				};

				retain();
			}

			_public = @public.ToArray();
			_internal = @internal.ToArray();
			_allStreams = _internal.Concat(_public).ToArray();


			TStream[] filter_all<TStream>()
			where TStream : IStream
			=> _allStreams
			.Where(stream => stream is TStream)
			.Cast<TStream>()
			.ToArray();

			_oracles = oracles(filter_all<IOracle>());
			_whispers = whispers(filter_all<IWhisper>());
			_commands = commands(filter_all<ICommand>());
		}

		private readonly Registry _registry;
		private readonly IStream[] _public;
		private readonly IStream[] _internal;
		private readonly IStream[] _allStreams;

		private readonly IOraclesDSL _oracles;
		private readonly IWhispersDSL _whispers;
		private readonly ICommandsDSL _commands;
	}
}
