namespace nflow.core
{
	using System;

	public interface IWhispersDSL
	{
		void Send<TWhisper>(TWhisper whisper)
		where TWhisper : IWhisper;

		IObservable<TWhisper> Listen<TWhisper>()
		where TWhisper : IWhisper;
	}


}
