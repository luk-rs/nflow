namespace nflow.core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Microsoft.Extensions.DependencyInjection;

	internal partial class BootstrapRegistry : ServiceCollection
	{

		public BootstrapRegistry(Assembly origin)
		{
			var hook = RegisterHookFrom(origin);

			ScanRegistries(hook.Assembly);

			GenerateStreamCarriers(hook.Assembly);

			//? could become a way for injecting dsl's dynamically
			this.AddSingleton<Func<IStream[], IWhispersDSL>>(
				bootstrap =>
				streams => new WhispersDSL(streams, bootstrap.GetRequiredService<IStreamCarrier[]>()));
			this.AddSingleton<Func<IStream[], IOraclesDSL>>(
				bootstrap =>
				streams => new OraclesDSL(streams, bootstrap.GetRequiredService<IStreamCarrier[]>()));
			this.AddSingleton<Func<IStream[], ICommandsDSL>>(
				bootstrap =>
				streams => new CommandsDSL(streams, bootstrap.GetRequiredService<IStreamCarrier[]>()));

			ScanBuses(hook.Assembly);

			ScanServices();

			ScanNanoServices(hook.Assembly);

			ScanMicros();
			var micros = this.BuildServiceProvider().GetRequiredService<IMicro[]>();

			this.AddSingleton<IFlow, Flow>();
		}

		private void ScanMicros()
		{
			this.AddSingleton<Func<Registry, IMicro>>(
				 bootstrap =>
				 registry => new Micro(registry, bootstrap.GetRequiredService<INanoService[]>(), bootstrap.GetRequiredService<IBus[]>()));

			this.AddSingleton<IMicro[]>(bootstrap =>
			{
				var registries = bootstrap
					.GetServices<Registry>()
					.ToArray();

				var factory = bootstrap.GetRequiredService<Func<Registry, IMicro>>();

				return registries
				.Select(registry => factory(registry))
				.ToArray();
			});
		}

		private void ScanNanoServices(Assembly origin)
		{
			this.Scan(
				 scanner => scanner
				 .FromAssemblyDependencies(origin)
				 .AddClasses(classes => classes.Where(type => typeof(INanoService).IsAssignableFrom(type)))
				 .As<INanoService>()
				 .WithLifetime(ServiceLifetime.Singleton)
			);

			this.AddSingleton<INanoService[]>(bootstrap => bootstrap.GetServices<INanoService>()?.ToArray() ?? Array.Empty<INanoService>());
		}

		private Hook RegisterHookFrom(Assembly origin)
		{
			var hook = new Hook(origin == default ? Assembly.GetEntryAssembly() : origin);

			this.AddSingleton<Hook>(_ => hook);

			return hook;
		}

		private void ScanRegistries(Assembly origin)
		{
			this.Scan(
			  scanner => scanner
			  .FromAssemblyDependencies(origin)
			  .AddClasses(classes => classes.Where(type => type.IsSubclassOf(typeof(Registry))))
			  .As<Registry>()
				 .WithLifetime(ServiceLifetime.Singleton)
		 );

			using var regContainer = this.BuildServiceProvider();

			var registries = regContainer.GetServices<Registry>();

			registries
			.ToList()
			.ForEach(
				 registry => registry
				 .ToList()
				 .ForEach(descriptor => ((ICollection<ServiceDescriptor>)this).Add(descriptor))
			);
		}

		private void GenerateStreamCarriers(Assembly origin)
		{



			var streams = origin
				 .GetTypes()
				 .Where(type => !type.IsAbstract && type.IsClass)
				 .Where(type => typeof(IStream).IsAssignableFrom(type))
				 .Select(type => type.generate_sample())
				 .ToArray();

			this.AddSingleton<IStream[]>(_ => streams);

			this.AddSingleton<IStreamCarrierGenerator, OracleCarrierGenerator>();
			this.AddSingleton<IStreamCarrierGenerator, WhisperCarrierGenerator>();
			this.AddSingleton<IStreamCarrierGenerator, CommandCarrierGenerator>();

			this.AddSingleton<IStreamCarrier[]>(bootstrap =>
			{
				var streams = bootstrap.GetService<IStream[]>();
				var generators = bootstrap.GetServices<IStreamCarrierGenerator>();

				var carrierCtors = streams
					.Select(stream =>
					{
						var generator = generators
							.SingleOrDefault(g => g.For(stream));

						return () => generator?.New(stream);
					});

				IStreamCarrier[] build_carriers() => carrierCtors
					.Select(ctor => ctor())
					.Where(ctor => ctor != default)
					.ToArray();

				return build_carriers();
			});
		}

		private void ScanBuses(Assembly origin)
		{
			bool is_flow_bus(Type type) => typeof(FlowBus).IsAssignableFrom(type);

			this.Scan(
							scanner => scanner
							.FromAssemblyDependencies(origin)
							.AddClasses(classes => classes.Where(type => is_flow_bus(type)))
							.AsSelf()
							.WithLifetime(ServiceLifetime.Singleton)
			);

			this.AddSingleton<Func<Registry, MicroBus>>(
				 bootstrap =>
				 registry => new MicroBus(
					 registry,
					 bootstrap.GetRequiredService<IStream[]>(),
					 bootstrap.GetRequiredService<Func<IStream[], IOraclesDSL>>(),
					 bootstrap.GetRequiredService<Func<IStream[], IWhispersDSL>>(),
					 bootstrap.GetRequiredService<Func<IStream[], ICommandsDSL>>()));

			this.AddSingleton<IBus[]>(bootstrap =>
			{
				var registries = bootstrap
					.GetServices<Registry>()
					.ToArray();

				var factory = bootstrap.GetService<Func<Registry, MicroBus>>();

				return registries
					.Select(registry => factory(registry))
					.ToArray();
			});
		}


		private void ScanServices()
		{

		}
	}

}