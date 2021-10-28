namespace streams.Core
{
    public interface IPCCommand : ICommand { }


    //public sealed class EventStream<TEvent> : IDisposable where TEvent : IEvent
    //{
    //    private readonly Subject<TEvent> _events = new();

    //    public EventStream() => _events.Publish().RefCount();

    //    public IObservable<TEvent> Events => _events;

    //    public void Publish(TEvent @event) => _events.OnNext(@event);

    //    public void Dispose() => _events.OnCompleted();
    //}

    //public sealed class EventStream<TEvent> where TEvent : IEvent
    //{
    //    private readonly IEventChannel<TEvent> _eventBus;

    //    public EventStream(IEventChannel<TEvent> eventBus) =>
    //        _eventBus = eventBus;

    //    public void Notify(TEvent @event) => _eventBus.Publish(@event);

    //    public IObservable<TEvent> Subscribe() => _eventBus.Events;
    //}

    //public interface IEvent { }

    //public interface ICommand { }

    //public interface IPCCommand
    //{
    //    string RemoteApp { get; }
    //}

    //public interface IEventChannel<T> where T : IEvent
    //{
    //    void Publish(T @event);

    //    IObservable<T> Events { get; }
    //}

    //public interface IMicroCommandStream
    //{
    //    IObservable<ICommand> Commands { get; }
    //}

    //public sealed class MicroCommandBus : IMicroCommandStream, IDisposable
    //{
    //    private readonly Subject<ICommand> _commands = new();

    //    public MicroCommandBus() => _commands.Publish().RefCount();

    //    public IObservable<ICommand> Commands => _commands;

    //    public void Dispose() => _commands.OnCompleted();
    //}

    //public sealed class EventChannel<TEvent> :
    //    IEventChannel<TEvent>,
    //    IDisposable
    //    where TEvent : IEvent
    //{
    //    private readonly Subject<TEvent> _events = new();

    //    public EventChannel() => _events.Publish().RefCount();

    //    public IObservable<TEvent> Events => _events;

    //    public void Publish(TEvent @event) => _events.OnNext(@event);

    //    public void Dispose() => _events.OnCompleted();
    //}

    //public sealed class IPCEventChannel<TEvent> :
    //    IEventChannel<TEvent>
    //    where TEvent : IEvent
    //{
    //    private readonly IBus _bus;
    //    private readonly List<string> _remoteApps;

    //    public IPCEventChannel(IBus bus, IEnumerable<string> remoteApps)
    //    {
    //        _bus = bus;
    //        _remoteApps = new List<string>(remoteApps);
    //    }

    //    public IObservable<TEvent> Events =>
    //        _bus
    //            .NewMessage
    //            .Where(message => message.GetType().Name == typeof(TEvent).Name)
    //            .Cast<TEvent>();

    //    public void Publish(TEvent @event) =>
    //        _remoteApps
    //            .ForEach(remoteApp => _bus.SendMessage(remoteApp, @event));
    //}

    //public interface IBus
    //{
    //    IObservable<object> NewMessage { get; }

    //    void SendMessage(string clientName, object message);
    //}

    //public class JsonMessage
    //{
    //    public JsonMessage(string sender, MessageType type, string typeName, string content)
    //    {
    //        Sender = sender;
    //        Type = type;
    //        TypeName = typeName;
    //        Content = content;
    //    }

    //    public string Sender { get; }

    //    public MessageType Type { get; }

    //    public string TypeName { get; }

    //    public string Content { get; }
    //}

    //public enum MessageType
    //{
    //    Command,
    //    Subscription,
    //    StreamData
    //}

    //public interface IFlow
    //{
    //    IObservable<TEvent> Listen<TEvent>() where TEvent : IEvent;

    //    void Notify<TEvent>(TEvent @event) where TEvent : IEvent;
    //}

    //public class Flow : IFlow
    //{
    //    private Dictionary<Type, dynamic> _streams = new();

    //    public Flow()
    //    {
    //        //Simulate registrations
    //        _streams.Add(typeof(FooEvent), new Subject<FooEvent>());
    //    }

    //    public IObservable<TEvent> Listen<TEvent>() where TEvent : IEvent =>
    //        _streams[typeof(TEvent)];

    //    public void Notify<TEvent>(TEvent @event) where TEvent : IEvent =>
    //        _streams[typeof(TEvent)].OnNext(@event);
    //}

    //public record FooEvent(int SomeValue) : IEvent;

    //internal class EventStream<T> : IDisposable,
    //                                         IStream where T : class
    //{
    //    private readonly Subject<T> _events = new();

    //    public bool IsPublic { get; } = true;

    //    public IObservable<T> Events { get; }

    //    public void Dispose() =>
    //        _events.OnCompleted();

    //    public void Notify(T @event) =>
    //        _events.OnNext(@event);
}

//internal class FooEventStream : EventStream<Foo>

