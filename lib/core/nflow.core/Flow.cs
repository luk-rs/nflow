namespace nflow.core
{
	using System.Collections.Generic;
	using Microsoft.Extensions.DependencyInjection;
	using System;
	using System.Reactive;
	using System.Diagnostics;
	using System.Linq;
	using System.Reactive.Subjects;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;

	internal class Flow : IFlow, IDisposable
	{
		IBus IFlow.DSL => _bus;
		void IFlow.AttachTo(IServiceCollection services, IServiceProvider bootstrap)
		{
			services.AddSingleton<IBus>(_ =>
			{
				Connection = _mergedConnections.Connect();
				return this._bus;
			});
		}

		public void Dispose() => Connection?.Dispose();

		private IDisposable Connection { get; set; }


		public Flow(IEnumerable<IStreamCarrier> carriers, FlowBus bus, IMicro[] micros)
		{
			Debug.WriteLine("Flow constructed");

			_bus = bus;
			_micros = micros;

			_mergedConnections = _micros.AsEnumerable()
			.Select(micro => micro.Connect())
			.Merge()
			.ObserveOn(Scheduler.Default)
			.Publish();
		}

		private FlowBus _bus;
		private readonly IMicro[] _micros;
		private IConnectableObservable<Unit> _mergedConnections;

	}
}