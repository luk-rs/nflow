namespace nflow.core
{
	using System;

	internal sealed class OracleCarrierGenerator : StreamCarrierGenerator<IOracle>
	{

		public override IStreamCarrier CreateInstance<TStream>(object stream)
		{
			var arg = stream.GetType();

			var carrier = typeof(OracleCarrier<>);

			var activator = carrier.MakeGenericType(arg);

			var instance = Activator.CreateInstance(activator, stream);

			return instance as IStreamCarrier;
		}

	}
}

