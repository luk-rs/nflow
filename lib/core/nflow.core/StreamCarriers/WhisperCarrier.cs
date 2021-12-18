namespace nflow.core
{
	internal sealed class WhisperCarrier<TWhisper> : StreamCarrier<TWhisper>
	 where TWhisper : IWhisper
	{
		public WhisperCarrier(TWhisper @default) : base(@default)
		{
		}
	}
}

