namespace nflow.core
{
    using System;

    internal sealed class WhisperCarrier<TWhisper> : StreamCarrier<TWhisper>
    where TWhisper : IWhisper
    {
    }
}

