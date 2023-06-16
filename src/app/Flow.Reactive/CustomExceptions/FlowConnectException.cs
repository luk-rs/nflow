namespace Flow.Reactive.CustomExceptions
{
    using System;

    public class FlowConnectException : Exception
    {
        public FlowConnectException(Exception innerException) 
            : base("Unable to start Flow", innerException)
        {
        }
    }
}
