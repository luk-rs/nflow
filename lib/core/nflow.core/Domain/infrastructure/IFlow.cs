namespace nflow.core
{
	using Microsoft.Extensions.DependencyInjection;
	using System;

	internal interface IFlow
	{

		internal void AttachTo(IServiceCollection services, IServiceProvider bootstrap);
		IBus DSL
		{
			get;
		}
	}

}