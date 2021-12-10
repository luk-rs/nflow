namespace SquadViewer.Views
{
	using System;
	using System.Windows;
	using Microsoft.Extensions.DependencyInjection;
	using nflow.core;
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
