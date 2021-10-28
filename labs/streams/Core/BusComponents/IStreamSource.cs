namespace streams.Core.BusComponents
{
    using System;
    using System.Collections.Generic;

    public interface IStreamSource
    {
        Dictionary<Type, object> LocalStreams { get; }

        Dictionary<Type, object> AllStreams { get; }
    }
}
