using Newtonsoft.Json;

namespace Flow.Reactive.Extensions
{
    internal static class ObjectExtensions
    {
        public static T Clone<T>(this T streamPayload) => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(streamPayload));

    }
}
