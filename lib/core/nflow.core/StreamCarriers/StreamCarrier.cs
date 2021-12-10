namespace nflow.core
{
	using System;
	using System.Diagnostics;
	using System.Reactive.Linq;
	using System.Reactive.Subjects;

	internal abstract class StreamCarrier<TStream>
	 : IStreamCarrier
	 where TStream : IStream
	{

		//IStreamCarrier
		bool IStreamCarrier.Carrying<TOtherStream>()
		=> typeof(TOtherStream).IsAssignableFrom(typeof(TStream));
		bool IStreamCarrier.Carrying<TOtherStream>(TOtherStream stream)
		=> stream is TStream;
		IObservable<TTargetStream> IStreamCarrier.Hook<TTargetStream>()
		=> _subject
		.Select(x => (TTargetStream)(object)x)
		.Catch<TTargetStream, Exception>(ex =>
		 {
			 Debug.WriteLine($"Could not resolve hook for {typeof(TTargetStream)} on carrier of {typeof(TStream)} with exception {ex}");
			 return Observable.Empty<TTargetStream>();
		 });
		void IStreamCarrier.Route(object payload)
		{
			Action route = typeof(TStream).IsAssignableFrom(payload.GetType())
			? () => _subject.OnNext((TStream)payload)
			: () => Debug.WriteLine($"Cannot route payload of type {payload.GetType()} through carrier of type {typeof(TStream)}");

			route();
		}

		TTargetStream IStreamCarrier.Value<TTargetStream>() => _subject switch
		{
			BehaviorSubject<TTargetStream> subj => subj.Value,
			_ => default
		};

		public StreamCarrier()
		{
			_subject = typeof(TStream) switch
			{

				{ } type when typeof(IPersistedStream).IsAssignableFrom(type) => new BehaviorSubject<TStream>(default(TStream)),
				{ } type when typeof(IStream).IsAssignableFrom(type) => new Subject<TStream>(),
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		private readonly SubjectBase<TStream> _subject;
	}
}

