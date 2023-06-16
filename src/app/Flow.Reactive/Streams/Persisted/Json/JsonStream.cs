namespace Flow.Reactive.Streams.Persisted.Json
{

    using System;
    using System.IO;
    using System.Reactive.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using System.Diagnostics;


    public abstract class JsonStream<TStreamData> : PersistedStream<TStreamData>
            where TStreamData : JsonStreamData
    {


        public sealed override IObservable<TStreamData> Data => base.Data.Do(Save);
        public sealed override TStreamData InitialState => Load();

        protected virtual string BaseDirectory => Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        protected virtual DirectoryInfo Directory => new(Path.Combine(BaseDirectory, Assembly.GetEntryAssembly()!.GetName().Name));

        private FileInfo JsonFile => new(Path.Combine(Directory.FullName, $"{typeof(TStreamData).Name}.json"));

        private void Save(TStreamData data) => File.WriteAllText(JsonFile.FullName, 
                                        JsonConvert.SerializeObject(data, Formatting.Indented, 
                                    new Newtonsoft.Json.Converters.StringEnumConverter()));

        private TStreamData Load()
        {
            var streamDefaultValue = Activator.CreateInstance<TStreamData>();

            if (!JsonFile.Directory!.Exists) 
                JsonFile.Directory.Create();
            if (!JsonFile.Exists) 
                File.WriteAllText(JsonFile.FullName, JsonConvert.SerializeObject(streamDefaultValue, 
                    Formatting.Indented, 
                    new Newtonsoft.Json.Converters.StringEnumConverter()));

            TStreamData storedStreamContent;
            try
            {
                storedStreamContent = JsonConvert.DeserializeObject<TStreamData>(File.ReadAllText(JsonFile.FullName));
            }
            catch (Exception e)
            {
                storedStreamContent = streamDefaultValue;
                Debug.WriteLine($"Failed to load '{GetType().Name}' stream data: {e}");
            }

            if (storedStreamContent != null || streamDefaultValue == null)
                return storedStreamContent;

            Debug.WriteLine($"Failed to load '{GetType().Name}' stream data, null value is not acceptable.");
            return streamDefaultValue;
        }
    }
}