

namespace nflow.core
{
	using System;
	using System.Linq;
	using System.Reactive;
	using System.Reactive.Linq;

	internal class Micro : IMicro
	{

		IObservable<Unit> IMicro.Connect()
		=> _nanos
		.Select(nano => nano.Connect(_bus))
		.Merge();
		Registry IMicro.Registry => _mRegistry;

		IBus IMicro.Bus => _bus;

		public Micro(Registry registry, INanoService[] nanos, IBus[] buses)
		{
			_mRegistry = registry;

			_bus = buses.Single(bus => bus.Namespace == registry.Namespace);

			_nanos = nanos
			.Where(
				nano => nano
				.GetType()
				.Namespace
				.StartsWith(registry.Namespace))
			.ToArray();

		}
		private readonly Registry _mRegistry;
		private readonly INanoService[] _nanos;
		private readonly IBus _bus;

	}
}