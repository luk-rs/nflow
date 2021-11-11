namespace nflow.core.Flow
{
    using System;
    using System.Collections.Generic;
    using nflow.core.Flow;

    public interface IStreamSource
    {
        StreamDictionary LocalStreams { get; }

        StreamDictionary AllStreams { get; }
    }
}
