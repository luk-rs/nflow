namespace Flow.Reactive.Streams.Middleware
{

    public interface IMiddleware
    {

        TStreamData Intercept<TStreamData>(TStreamData data)
                where TStreamData : IStreamData;

    }

}