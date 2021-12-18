namespace nflow.core
{
	using System;

	class WhisperCarrierGenerator : StreamCarrierGenerator<IWhisper>
	{

		public override IStreamCarrier CreateInstance<TStream>(object stream)
		{
			var arg = stream.GetType();

			var carrier = typeof(WhisperCarrier<>);

			var activator = carrier.MakeGenericType(arg);

			var instance = Activator.CreateInstance(activator);

			return instance as IStreamCarrier;
		}
	}

}