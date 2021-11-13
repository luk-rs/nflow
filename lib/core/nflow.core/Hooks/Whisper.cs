namespace nflow.core
{
    using System;

    public interface IWhispersBus<TWhisper> where TWhisper : IWhisper
    {
        void Gossip(Action<TWhisper> whisp);
        IObservable<TWhisper> Listen { get; }
    }

    internal sealed class Whisper<TWhisper> : Hook<TWhisper>, IWhispersBus<TWhisper>
    where TWhisper : IWhisper, new()
    {
        IObservable<TWhisper> IWhispersBus<TWhisper>.Listen => Self.Socket;

        void IWhispersBus<TWhisper>.Gossip(Action<TWhisper> whisp)
        {
            var gossip = new TWhisper();
            whisp(gossip);

            Self.Route(gossip);
        }
    }
}

