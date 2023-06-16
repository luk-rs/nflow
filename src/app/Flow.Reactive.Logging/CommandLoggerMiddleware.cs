namespace Flow.Reactive.Logging
{
    using System;
    using Flow.Reactive.Streams;
    using Flow.Reactive.Streams.Ephemeral.Commands;
    using Flow.Reactive.Streams.Middleware;

    public class CommandLoggerMiddleware : IMiddleware
    {
        private readonly string _prefix;

        public CommandLoggerMiddleware(string prefix = "Command") =>
            _prefix = prefix;

        public TStreamData Intercept<TStreamData>(TStreamData data) where TStreamData : IStreamData
        {
            if (data is Command command && command.Trace)
            {
                Console.WriteLine($"[{data.GetType()}] {_prefix}: {command.ShortFormat}");
                //Log.For(data.GetType()).Info($"{_prefix}: {command.ShortFormat}");
            }

            return data;
        }
    }
}
