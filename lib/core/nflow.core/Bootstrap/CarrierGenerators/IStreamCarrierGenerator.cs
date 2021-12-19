namespace nflow.core
{

	internal interface IStreamCarrierGenerator
	{
		IStreamCarrier New<TStream>(TStream stream)
		where TStream : class, IStream;

	}

}