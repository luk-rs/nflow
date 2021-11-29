namespace streams.MicroServiceA.Streams
{
	using nflow.core;


	internal record FooEvent(int Value) : IWhisper;
}
