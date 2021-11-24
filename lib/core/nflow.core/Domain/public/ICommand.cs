namespace nflow.core
{
	public interface ICommand : IStream
	{
		bool IStream.IsPublic => true;

	}
}

