namespace Flow.Reactive.Playground.Sagas
{
    using Flow.Reactive.Playground.MicroServices.Processing.Streams;
    using Flow.Reactive.Sagas;
    using Flow.Rx.Extensions;
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    public class ConsoleWriterSaga : ISaga, IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public ConsoleWriterSaga(IFlow flow) =>
            flow
                .Query<LastIntegerAdded>()
                .Do(x => Console.WriteLine($"Last integer added: {x.Value}"))
                .Subscribe(_disposables);

        public void Dispose() => _disposables.Dispose();
    }
}
