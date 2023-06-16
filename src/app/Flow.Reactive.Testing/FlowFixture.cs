namespace Flow.Reactive.TestingTools
{

    //public class FlowFixture<TRegistry> : IDisposable where TRegistry : Registry
    //{

    //    private readonly IFlowMicro _flow;

    //    private readonly Dictionary<Type, object> _observers = new Dictionary<Type, object>();

    //    public FlowFixture(TestScheduler testScheduler)
    //    {
    //        TestScheduler = testScheduler;

    //        _flow = (IFlowMicro)FlowBuilder.Create().AddRegistry<TRegistry>().Build();

    //        var publicStreams = _flow
    //                             .Streams
    //                             .Where(stream => stream.Public && !IsSubclassOfRawGeneric(typeof(CollectionStream<>), stream.GetType()))
    //                             .Select(stream => (IsCollectionStream: false, Stream: stream));

    //        var publicCollectionStreams = _flow
    //                             .Streams
    //                             .Where(stream => stream.Public && IsSubclassOfRawGeneric(typeof(CollectionStream<>), stream.GetType()))
    //                             .Select(stream => (IsCollectionStream: true, Stream: stream));

    //        Observable
    //             .Merge(publicStreams, publicCollectionStreams)
    //             .Do(x =>
    //             {
    //                 var streamType = x.Stream.GetType();
    //                 var streamDataType = x.Stream.GetType().BaseType.GetGenericArguments()[0];

    //                 var createObserverMethod = typeof(TestScheduler).GetMethod(nameof(TestScheduler.CreateObserver));

    //                 MethodInfo createObserverGenericMethod;
    //                 Type setDataType = null;

    //                 if (x.IsCollectionStream)
    //                 {
    //                     setDataType = typeof(Set<>).MakeGenericType(streamDataType);
    //                     createObserverGenericMethod = createObserverMethod.MakeGenericMethod(setDataType);
    //                 }
    //                 else
    //                 {
    //                     createObserverGenericMethod = createObserverMethod.MakeGenericMethod(streamDataType);
    //                 }

    //                 var observer = createObserverGenericMethod.Invoke(TestScheduler, new object[] { });

    //                 if (x.IsCollectionStream)
    //                     _observers.Add(setDataType, observer);
    //                 else
    //                     _observers.Add(streamDataType, observer);

    //                 var streamDynamic = (dynamic)x.Stream;
    //                 var observerDynamic = (dynamic)observer;

    //                 streamDynamic.Subscribe(observerDynamic);
    //             })
    //            .Subscribe(x => { }, () => { });
    //    }

    //    public TestScheduler TestScheduler { get; }

    //    public void Dispose() => _flow.Dispose();

    //    public IObservable<Unit> SendCommand<TCommand>(TCommand command) where TCommand : Command => _flow.Send(command);

    //    public IObservable<TStream> Stream<TStream>() where TStream : IStream => _flow.Stream<TStream>();

    //    public ITestableObserver<TNotification> Observer<TNotification>() where TNotification : IStreamData => (ITestableObserver<TNotification>)_observers[typeof(TNotification)];

    //    public void ShouldRaise<TNotification>(params (long Time, TNotification Message)[] messages)
    //        where TNotification : IStreamData
    //    {
    //        var receivedMessages = Observer<TNotification>()
    //                              .Messages
    //                              .Select(x => (x.Time, x.Value.Value));

    //        receivedMessages.Should()
    //                        .BeEquivalentTo(
    //                                        messages,
    //                                        options => options
    //                                                  .ComparingByMembers<(long, TNotification)>()
    //                                                  .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.0001))
    //                                                  .WhenTypeIs<double>()
    //                                       );
    //    }

    //    private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
    //    {
    //        while (toCheck != null && toCheck != typeof(object))
    //        {
    //            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;

    //            if (generic == cur)
    //                return true;

    //            toCheck = toCheck.BaseType;
    //        }
    //        return false;
    //    }
    //}

}
