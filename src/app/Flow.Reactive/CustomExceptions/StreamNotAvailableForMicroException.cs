namespace Flow.Reactive.CustomExceptions
{
    using System;

    public class StreamNotAvailableForMicroException : Exception
    {
        public StreamNotAvailableForMicroException(Type streamType, string microId)
            : base($"Stream data type {streamType.Name} is not accessible for MicroService {microId}.{Environment.NewLine} " +
                   $"MicroService {microId} has only access to its Private streams and all Public streams")
        {
        }
    }
}
