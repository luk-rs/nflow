namespace Flow.Reactive.Json
{
    using Persistence;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using Streams;


    public class JsonRepository : IRepository
    {
        private readonly string _baseFolder;

        public JsonRepository(string baseFolder) => _baseFolder = baseFolder;

        public TStreamData Load<TStreamData>(TStreamData defaultData) where TStreamData : IStreamData
        {
            var fullPath = JsonFullPath(typeof(TStreamData).GetFriendlyName());
            var fileInfo = new FileInfo(fullPath);

            return fileInfo.Exists ? 
                    JsonConvert.DeserializeObject<TStreamData>(File.ReadAllText(fileInfo.FullName)) : 
                    defaultData;
        }

        public TStreamData Save<TStreamData>(TStreamData streamData) where TStreamData : IStreamData
        {
            File.WriteAllText(JsonFullPath(typeof(TStreamData).GetFriendlyName()), JsonConvert.SerializeObject(streamData));
            return streamData;
        }

        private string JsonFullPath(string fileNameWithoutExtension) => 
            Path.Combine(_baseFolder, $"{fileNameWithoutExtension}.json");
    }

    public static class TypeNameExtensions
    {
        public static string GetFriendlyName(this Type type)
        {
            string friendlyName = type.Name;
            if (type.IsGenericType)
            {
                int iBacktick = friendlyName.IndexOf('`');
                if (iBacktick > 0)
                {
                    friendlyName = friendlyName.Remove(iBacktick);
                }
                friendlyName += "_";
                Type[] typeParameters = type.GetGenericArguments();
                for (int i = 0; i < typeParameters.Length; ++i)
                {
                    string typeParamName = GetFriendlyName(typeParameters[i]);
                    friendlyName += (i == 0 ? typeParamName : "." + typeParamName);
                }
                friendlyName += "_";
            }

            return friendlyName;
        }
    }
}
