namespace nflow.core
{
	using System;
	using System.Reflection;
	using Microsoft.Extensions.DependencyInjection;


	public static class InstallationDSL
	{
		private interface IBootstrapContainer : IServiceProvider { }

		public static IServiceCollection WithFlow(this IServiceCollection services, Assembly origin = default)
		{
			var bootstrap = new BootstrapCollection(origin).BuildServiceProvider();

			var flow = bootstrap.GetRequiredService<IFlow>();

			flow.AttachTo(services, bootstrap);

			return services;
		}


		//* Bootstrap helpers
		private static IServiceProvider GenerateBootstrapContainer(this IServiceCollection collection, Assembly origin)
		{
			var bootstrapCollection = new BootstrapCollection(origin);
			return bootstrapCollection.BuildServiceProvider();
		}


	}
}