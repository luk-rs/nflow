// namespace streams.MicroServiceA.NanoServices
// {
//     using System;
//     using System.Reactive;
//     using nflow.core.Flow;
//     using streams.Core;
//     using streams.MicroServiceA.Commands;
//     using streams.MicroServiceA.Streams;

//     public sealed class MyNanoServiceA2 : INano
//     {
//         public IObservable<Unit> Connect(IMicroBus bus) =>
//             bus
//                 .Handle<UpdateSomethingCommand>()
//                 .AndUpdate<Bar>(bus, (command, stream) => stream.SomeValue++);
//     }
// }

