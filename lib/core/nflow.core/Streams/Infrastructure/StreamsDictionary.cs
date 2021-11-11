namespace nflow.core.Flow
{
    using System;
    using System.Collections.Generic;


    public class StreamDictionary : Dictionary<Type, object>
    {

        public StreamDictionary AddOracle<TStream>(Func<TStream> construction)
            where TStream : IPersistedStream, new()
        {
            base.Add(typeof(TStream), new Oracle<TStream>());
            return this;
        }
    }


}