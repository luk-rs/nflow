namespace nflow.core
{
	using System;
	using System.Linq;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;

	internal class WhispersDSL : IWhispersDSL

	{

		IObservable<TWhisper> IWhispersDSL.Listen<TWhisper>()
		=> OnValidatedCarrier(carrier => carrier.Hook<TWhisper>());
		void IWhispersDSL.Send<TWhisper>(TWhisper whisper)
		=> OnValidatedCarrier<TWhisper>(carrier => carrier.Route(whisper))
		.ObserveOn(Scheduler.Default)
		.Subscribe();


		public WhispersDSL(IStream[] streams, IStreamCarrier[] whispers)
		{
			_whispers = whispers
			.Where(carrier => streams.Any(stream => carrier.Carrying(stream)))
			.ToArray();
		}

		private readonly IStreamCarrier[] _whispers;

		private IObservable<TWhisper> OnValidatedCarrier<TWhisper>(Func<IStreamCarrier, IObservable<TWhisper>> selector)
		where TWhisper : IWhisper
		{
			var whisper = _whispers.SingleOrDefault(carrier => carrier.Carrying<TWhisper>());

			return whisper == default
			? throw new ArgumentOutOfRangeException($"There is no instance of scanned whisper carrier that matches {typeof(TWhisper)}")
			: selector(whisper);
		}
		private IObservable<TWhisper> OnValidatedCarrier<TWhisper>(Action<IStreamCarrier> selector)
		where TWhisper : IWhisper
		=> OnValidatedCarrier(carrier =>
			 {
				 selector(carrier);
				 return Observable.Empty<TWhisper>();
			 });
	}
}
