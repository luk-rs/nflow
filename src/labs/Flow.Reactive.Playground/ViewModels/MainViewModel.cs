namespace Flow.Reactive.Playground.ViewModels
{

    using global::Reactive.Bindings;
    using System;
    using System.Reactive.Linq;
    using Reactive;
    using System.Collections.Generic;
    using System.Linq;
    using MicroServices.Processing.Commands;
    using Flow.Reactive.Playground.MicroServices.Processing.Streams;
    using System.Reactive.Disposables;
    using Flow.Reactive.Playground.MicroServices.Reporting.Streams;
    using ReactiveProperty;
    using Rx.Extensions;


    public class MainViewModel : IDisposable
    {

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public MainViewModel(IFlow flow)
        {
            Integers = flow.Bind<IEnumerable<int>, Integers>(set => set.Items.Select(x => x.Value), _disposables);

            LastInteger = flow.Bind<string, LastIntegerAdded>(integer => integer.PrintValue, _disposables);

            Sum = flow.Bind<int, Sum>(sum => sum.Total, _disposables);

            IntegerToAdd = new ReactiveProperty<string>("2").AddToDisposables(_disposables);

            AddIntegerCommand = flow.BindCommand(_ => new AddInteger(int.Parse(IntegerToAdd.Value)),
                                                 IntegerToAdd.Select(integer => int.TryParse(integer, out _)),
                                                 _disposables);
        }

        public ReactiveCommand AddIntegerSyncCommand { get; }

        public ReactiveCommand AddIntegerCommand { get; }

        public ReactiveProperty<string> IntegerToAdd { get; }

        public ReactiveProperty<IEnumerable<int>> Integers { get; }

        public ReactiveProperty<string> LastInteger { get; }

        public ReactiveProperty<int> Sum { get; }

        public void Dispose() => _disposables.Dispose();
    }
}