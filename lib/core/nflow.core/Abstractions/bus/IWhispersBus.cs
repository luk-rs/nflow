namespace nflow.core
{
    using System;

    public interface IWhispersBus<TWhisper> where TWhisper : IWhisper
    {
        void Gossip(Action<TWhisper> whisp);
        IObservable<TWhisper> Listen { get; }
    }
}

