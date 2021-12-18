namespace nflow.core
{
	internal interface IStreamCarrierGenerator<TTargetStream> : IStreamCarrierGenerator
	 where TTargetStream : IStream
	{
		bool Handling(IStream stream) => stream is TTargetStream;
		IStreamCarrier CreateInstance<TStream>(object stream)
		where TStream : TTargetStream;

	}

	public interface IStreamCarrierGenerator
	{

		bool For(IStream stream);
		IStreamCarrier New<TStream>(TStream stream)
		where TStream : class, IStream;

	}

}