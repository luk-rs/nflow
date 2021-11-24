

namespace nflow.core
{
	using System;
	using System.Reactive;

	internal interface IMicro
	{
		string Name => Registry.GetType().Name;
		string Namespace => Registry.Namespace;
		internal Registry Registry { get; }
		IBus Bus { get; }

		IObservable<Unit> Connect();
	}
}