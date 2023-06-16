namespace Flow.Rx.Extensions
{

    using System;
    using System.Reactive.Subjects;


    /// <summary>
    ///    Encapsulation of a BehaviorSubject
    /// </summary>
    public class RxProperty<T> : IObservable<T>, IDisposable
    {
        private readonly BehaviorSubject<T> _subject;

        public RxProperty(T initialValue) => _subject = new BehaviorSubject<T>(initialValue);

        public T Value
        {
            get => _subject.Value;
            set => _subject.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<T> observer) => _subject.Subscribe(observer);

        public void Dispose()
        {
            _subject.OnCompleted();
            _subject.Dispose();
        }
    }
}
