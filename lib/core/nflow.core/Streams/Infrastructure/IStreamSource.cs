namespace nflow.core
{
    using System;
    using System.Collections.Generic;


    public interface IStreamSource
    {
        StreamDictionary LocalStreams { get; }

        StreamDictionary AllStreams { get; }
    }
}
