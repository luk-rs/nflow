
using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using nflow.core;
using streams.MicroServiceA.Commands;
using streams.Test.Commands;
using streams.Test.Streams;


FactoryPatternInjection();

NonNanoConsumer();

void FactoryPatternInjection()
{
	var services = new ServiceCollection()
					.AddSingleton<Func<int, Foo>>(sp => @int => new Foo(@int, sp.GetRequiredService<IBar>()))
					.AddSingleton<IBar, Bar>();

	var provider = services
	.BuildServiceProvider();

	var foo1 = provider
	.GetService<Func<int, Foo>>()
	(1);

	var foo2 = provider
	.GetService<Func<int, Foo>>()
	(2);


	Console.WriteLine($"c1.Ref = {foo1.Ref}\tc1.Name = {foo2.ConstrInt.Name}\nc2.Ref = {foo2.Ref}\tc2.Name = {foo2.ConstrInt.Name}\n");
}
void NonNanoConsumer()
{
	var services = new ServiceCollection();

	using var container = ChronF(
		 () => services
		 .WithFlow()
		 .BuildServiceProvider()
		 , "Bootstrap");

	IBus bus = ChronF(
		 () => container
		 .GetRequiredService<IBus>()
		 , "Bus instantiation"
	);

	var whisper = bus
	.Whispers
	.Listen<StreamZ>()
	.Do(z => Console.WriteLine(z.Name))
	.Select(_ => Unit.Default)
	.Take(50)
	.ToTask();

	var instruction = bus
	.Commands
	.Handle<CommandZ>()
	.Do(z => Console.WriteLine(z.Name))
	.Select(_ => Unit.Default)
	.Take(2)
	.ToTask();

	ChronA(
			 () => bus
			.Whispers
			.Send<StreamZ>(new StreamZ("DSL Baby", 0)),
			"send whisper streamZ");

	var whispers = Observable
	.Range(0, 50)
	.Select(
		 it => Observable
		 .Return(Unit.Default)
		 .Do(_ => ChronA(
			 () => bus
			.Whispers
			.Send<StreamZ>(new StreamZ($"Hello Whispers {it}", 1)),
			"send whisper streamZ")))
	.Merge()
	.LastAsync()
	.ToTask();

	bus.Commands.Send(new FooCommand());
	bus.Commands.Send(new FooCommand());
	bus.Commands.Send(new FooCommand());
	bus.Commands.Send(new FooCommand());

	Task.WaitAll(whispers, instruction, whisper);
}

T ChronF<T>(Func<T> selector, string msg)
{
	var watch = Stopwatch.StartNew();
	var result = selector();
	watch.Stop();
	Console.WriteLine($"[{msg}] took {watch.Elapsed}");

	return result;
}
void ChronA(Action action, string msg)
=> ChronF(
	() =>
	{
		action();
		return true;
	}, msg);

class Foo
{

	public int Ref { get; }
	public IBar ConstrInt { get; }

	public Foo(int @ref, IBar constrInt)
	{
		Ref = @ref;
		ConstrInt = constrInt;
	}
}

interface IBar
{
	string Name => "Ola Inj";
}

class Bar : IBar { }


