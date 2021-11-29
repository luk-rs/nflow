namespace nflow.core
{
	using System;
	using System.Reactive;
	using System.Reactive.Linq;

	public static class FlowObservableDSL
	{
		public static IObservable<Unit> Each<T>(this IObservable<T> source) => source.Select(_ => Unit.Default);
	}
}