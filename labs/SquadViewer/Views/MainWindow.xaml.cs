namespace SquadViewer.Views
{
	using System;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using System.Windows;
	using Microsoft.Extensions.DependencyInjection;
	using nflow.core;
	using Reactive.Bindings;
	using SquadViewer.MicroServices.DataContexts.Streams;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var services = new ServiceCollection();

			using var container = services
				 .WithFlow()
				 .BuildServiceProvider();

			var bus = container.GetRequiredService<IBus>();

			bus
				.Oracles
				.Query<CurrentPage>()
				.ObserveOn(Scheduler.Default)
				.Delay(TimeSpan.FromSeconds(5))
				.ObserveOn(ReactivePropertyScheduler.Default)
				.Subscribe(currentPage => DataContext = currentPage.Page);
		}
	}

	public class AppRegistry : Registry
	{
		public AppRegistry()
		{

		}
	}
}
