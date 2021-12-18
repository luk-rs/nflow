namespace nflow.core
{
	internal sealed class OracleCarrier<TOracle> : StreamCarrier<TOracle>
	 where TOracle : IOracle
	{
		public OracleCarrier(TOracle @default) : base(@default)
		{
		}
	}
}

