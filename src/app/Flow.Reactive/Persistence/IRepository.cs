namespace Flow.Reactive.Persistence
{

    using System.IO;
    using System.Reflection;
    using Streams;


    public interface IRepository
    {

        TStreamData Load<TStreamData>(TStreamData defaultData) where TStreamData : IStreamData;
        TStreamData Save<TStreamData>(TStreamData streamData) where TStreamData : IStreamData;

    }


    public abstract class Repository : IRepository
    {

        public DirectoryInfo Location { get; }

        public string Name { get; }

        protected Repository(string name, DirectoryInfo location = default)
        {
            Location = location ?? new FileInfo(Assembly.GetEntryAssembly().Location).Directory;
            Name = name;
        }

        public abstract TStreamData Load<TStreamData>(TStreamData defaultData) where TStreamData : IStreamData;
        public abstract TStreamData Save<TStreamData>(TStreamData streamData) where TStreamData : IStreamData;

    }

}