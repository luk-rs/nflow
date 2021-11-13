namespace nflow.core
{
    using System;
    using System.Reactive.Subjects;

    internal interface IHook<TStream> : IHook
    where TStream : IStream
    {
        IHook<TStream> Self { get; }

        IObservable<TStream> Socket { get; }

        void Route(TStream payload);

    }

    internal interface IHook
    {
        bool Holding<TOtherStream>() where TOtherStream : IStream;
        bool Holding(Type type);
    }

    internal abstract class Hook<TStream> : IHook<TStream>
    where TStream : IStream
    {
        IObservable<TStream> IHook<TStream>.Socket => _subject;

        public IHook<TStream> Self => this;

        void IHook<TStream>.Route(TStream payload) => _subject.OnNext(payload);
        bool IHook.Holding<TOtherStream>() => typeof(TOtherStream).IsInstanceOfType(typeof(TStream));
        bool IHook.Holding(Type type) => type.IsAssignableFrom(typeof(TStream));

        public Hook()
        {
            _subject = typeof(TStream) switch
            {
                { } type when typeof(ICommand).IsAssignableFrom(type) => new Subject<TStream>(),
                { } type when typeof(IWhisper).IsAssignableFrom(type) => new Subject<TStream>(),
                { } type when typeof(IOracle).IsAssignableFrom(type) => new BehaviorSubject<TStream>(default(TStream)),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private readonly SubjectBase<TStream> _subject;
    }
}

