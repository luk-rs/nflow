namespace nflow.core
{
    using System;

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

