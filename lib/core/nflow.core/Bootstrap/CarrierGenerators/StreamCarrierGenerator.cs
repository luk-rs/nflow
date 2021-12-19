namespace nflow.core
{
	using System;

	internal class StreamCarrierGenerator : IStreamCarrierGenerator
	{

		IStreamCarrier IStreamCarrierGenerator.New<TStream>(TStream stream)
		{
			var arg = stream.GetType();

			var carrier = typeof(StreamCarrier<>);
			//	stream switch
			//{
			//	IOracle => typeof(OracleCarrier<>),
			//	ICommand => typeof(CommandCarrier<>),
			//	IWhisper => typeof(WhisperCarrier<>),
			//	_ => throw new ArgumentOutOfRangeException(nameof(stream))
			//};

			var activator = carrier.MakeGenericType(arg);

			var instance = Activator.CreateInstance(activator, stream);

			return instance as IStreamCarrier;
		}
	}
}

