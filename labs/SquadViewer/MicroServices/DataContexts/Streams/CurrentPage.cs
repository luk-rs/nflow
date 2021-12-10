namespace SquadViewer.MicroServices.DataContexts.Streams
{
	using nflow.core;
	using SquadViewer.Core;
	using SquadViewer.Pages;

	public class CurrentPage : IOracle
	{
		bool IStream.IsPublic => true;

		public IPage Page { get; set; } = new InitialPage();
	}
}
