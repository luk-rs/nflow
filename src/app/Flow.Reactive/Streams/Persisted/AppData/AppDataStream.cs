namespace Flow.Reactive.Streams.Persisted.AppData
{

    using System;
    using Json;


    public class AppDataStream<TStreamData> : JsonStream<TStreamData>
            where TStreamData : JsonStreamData
    {

        protected sealed override string BaseDirectory => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    }

}