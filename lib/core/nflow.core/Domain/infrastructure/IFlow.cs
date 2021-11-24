namespace nflow.core
{
	using System.Collections.Generic;
	using Microsoft.Extensions.DependencyInjection;
	using System;
	using System.Reactive;
	using System.Diagnostics;
	using System.Linq;
	using System.Reactive.Linq;
	using System.Reactive.Subjects;
	using System.Reactive.Concurrency;

	internal interface IFlow
	{

		internal void AttachTo(IServiceCollection services, IServiceProvider bootstrap);
		IBus DSL
		{
			get;
		}
	}

}