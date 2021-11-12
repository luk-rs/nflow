// namespace streams.MicroServiceB.NanoServices
// {
//     using System;
//     using System.Reactive;
//     using System.Reactive.Linq;
//     using nflow.core.Flow;
//     using streams.Core;
//     using streams.MicroServiceA.Streams;

//     public sealed class MyNanoServiceB : INano
//     {
//         public IObservable<Unit> Connect(IMicroBus bus) =>
//             bus
//                 .Listen<FooEvent>()
//                 .Select(x => Unit.Default);
//     }
// }
