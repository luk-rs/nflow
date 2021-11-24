namespace nflow.core
{
	public partial interface IBus
	{
		ICommandsDSL Commands { get; }
		IOraclesDSL Oracles { get; }
		IWhispersDSL Whispers { get; }

		internal Registry Registry { get; }
		internal string Namespace => Registry.Namespace;
	}
}
