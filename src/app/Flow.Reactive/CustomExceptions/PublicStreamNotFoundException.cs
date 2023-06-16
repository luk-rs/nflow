namespace Flow.Reactive.CustomExceptions
{
    using System;

    public class PublicStreamNotFoundException : Exception
    {
        public PublicStreamNotFoundException(Type streamType)
            : base($"Stream data type {streamType.Name} not found. Make sure the stream exists and it is defined as Public.")
        {
        }
    }
}
