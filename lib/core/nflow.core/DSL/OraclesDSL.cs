namespace nflow.core
{
	using System;
	using System.Linq;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;

	internal class OraclesDSL : IOraclesDSL
	{
		IObservable<TOracle> IOraclesDSL.Query<TOracle>()
		=> OnValidatedCarrier(carrier => carrier.Hook<TOracle>());
		void IOraclesDSL.Update<TOracle>(Action<TOracle> update)
		=> OnValidatedCarrier<TOracle>(carrier =>
		{
			var value = carrier.Value<TOracle>();
			update(value);
			carrier.Route(value);
		})
		.ObserveOn(Scheduler.Default)
		.Subscribe();


		public OraclesDSL(IStream[] streams, IStreamCarrier[] commands)
		{
			_commands = commands
			.Where(carrier => streams.Any(stream => carrier.Carrying(stream)))
			.ToArray();
		}

		private readonly IStreamCarrier[] _commands;

		private IObservable<TOracle> OnValidatedCarrier<TOracle>(Func<IStreamCarrier, IObservable<TOracle>> selector)
		where TOracle : IOracle
		{
			var oracle = _commands.SingleOrDefault(carrier => carrier.Carrying<TOracle>());

			return oracle == default
			? throw new ArgumentOutOfRangeException($"There is no instance of scanned oracle carrier that matches {typeof(TOracle)}")
			: selector(oracle);
		}
		private IObservable<TOracle> OnValidatedCarrier<TOracle>(Action<IStreamCarrier> selector)
		where TOracle : IOracle
		=> OnValidatedCarrier(carrier =>
			 {
				 selector(carrier);
				 return Observable.Empty<TOracle>();
			 });
	}
}
