namespace nflow.core
{
    using System;

    internal partial class BootstrapRegistry
    {
        sealed class OracleCarrierGenerator : StreamCarrierGenerator<IOracle>
        {
            public override IStreamCarrier CreateInstance<TStream>(object stream)
            {
                var arg = stream.GetType();

                var carrier = typeof(OracleCarrier<>);

                var activator = carrier.MakeGenericType(arg);

                var instance = Activator.CreateInstance(activator);

                return instance as IStreamCarrier;
            }
        }
    }

}