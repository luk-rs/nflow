namespace nflow.core
{
    public interface IStreamSource
    {
        StreamDictionary LocalStreams { get; }

        StreamDictionary AllStreams { get; }
    }
}
