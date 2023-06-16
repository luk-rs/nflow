namespace Flow.Reactive.Playground
{
    using Flow.Reactive.Streams;
    using Flow.Reactive.Streams.Middleware;

    public class PlaygroundCustomMiddleware : IMiddleware
    {
        public TStreamData Intercept<TStreamData>(TStreamData data) where TStreamData : IStreamData
        {
            //Do Whatever you want

            return data;
        }
    }
}