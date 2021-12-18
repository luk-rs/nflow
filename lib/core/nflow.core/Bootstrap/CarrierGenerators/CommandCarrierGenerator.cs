namespace nflow.core
{
	using System;

	class CommandCarrierGenerator : StreamCarrierGenerator<ICommand>
	{
		public override IStreamCarrier CreateInstance<TStream>(object stream)
		{
			var arg = stream.GetType();

			var carrier = typeof(CommandCarrier<>);

			var activator = carrier.MakeGenericType(arg);

			var instance = Activator.CreateInstance(activator, stream);

			return instance as IStreamCarrier;
		}
	}


}