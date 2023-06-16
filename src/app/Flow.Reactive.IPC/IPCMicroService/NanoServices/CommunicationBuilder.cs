namespace Flow.Reactive.IPC.IPCMicroService.NanoServices
{
    using Flow.Reactive.Services;
    using Flow.Rx.Extensions;
    using System;
    using System.IO.Pipes;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using System.Reflection;
    using System.Threading.Tasks;

    public class CommunicationBuilder : TriggerNano<Unit>
    {
        private readonly IMediator _mediator;

        public CommunicationBuilder(IMediator mediator) => _mediator = mediator;

        protected override IObservable<Unit> Trigger =>
            Observable
                .Return(IPCConfigurator.RemoteApps)
                .SelectMany(remoteApp => remoteApp)
                .MergeSelect(remoteApp => Observable
                                              .Merge(CreateServerPipe(remoteApp).ToObservable(), 
                                                     CreateClientPipe(remoteApp).ToObservable()));


        public override IObservable<Unit> Connect() =>
            Trigger
                .Select(_ => Unit.Default);

        public async Task CreateServerPipe(string clientName)
        {
            var serverName = Assembly.GetEntryAssembly().GetName().Name;
            var pipe = new NamedPipeServerStream(_mediator.GetPipeName(serverName, clientName));
            await pipe.WaitForConnectionAsync();
            _mediator.AddOutputPipe(clientName, pipe);
        }

        public async Task CreateClientPipe(string serverName)
        {
            var clientName = Assembly.GetEntryAssembly().GetName().Name;
            var pipe = new NamedPipeClientStream(_mediator.GetPipeName(serverName, clientName));
            await pipe.ConnectAsync(10000);
            _mediator.AddInputPipe(serverName, pipe);
        }
    }
}
