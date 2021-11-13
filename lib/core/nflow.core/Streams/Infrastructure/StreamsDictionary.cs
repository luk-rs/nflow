namespace nflow.core
{
    using System;
    using System.Collections.Generic;


    public class StreamDictionary : Dictionary<Type, object>
    {

        // public StreamDictionary AddOracle<TStream>(Func<TStream> construction)
        //     where TStream : IWhisper, new()
        // {
        //     base.Add(typeof(TStream), new Oracle<TStream>());
        //     return this;
        // }
    }


}