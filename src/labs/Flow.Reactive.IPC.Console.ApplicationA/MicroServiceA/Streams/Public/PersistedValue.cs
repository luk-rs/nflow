﻿namespace Flow.Reactive.IPC.Console.ApplicationA.MicroServiceA.Streams.Public
{
    using Flow.Reactive.Streams.Persisted;

    public class PersistedValueStream : PublicPersistedStream<PersistedValue>
    {
        public override PersistedValue InitialState { get; } = new PersistedValue() { Value = 1 };
    }

    public class PersistedValue : PersistedStreamData
    {
        public int Value { get; set; }
    }
}
