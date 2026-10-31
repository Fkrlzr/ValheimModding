using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ValueDeserializers;

internal sealed class MaximumRecursionValueDeserializer : IValueDeserializer
{
	private readonly IValueDeserializer innerDeserializer;

	private readonly int maximumRecursion;

	public MaximumRecursionValueDeserializer(IValueDeserializer innerDeserializer, int maximumRecursion)
	{
		this.innerDeserializer = innerDeserializer ?? throw new ArgumentNullException("innerDeserializer");
		this.maximumRecursion = maximumRecursion;
	}

	public object? DeserializeValue(IParser parser, Type expectedType, SerializerState state, IValueDeserializer nestedObjectDeserializer)
	{
		RecursionLevel recursionLevel = state.Get(() => new RecursionLevel(maximumRecursion));
		Mark start = parser.Current.Start;
		Mark end = parser.Current.End;
		recursionLevel.Increment(in start, in end);
		object result = innerDeserializer.DeserializeValue(parser, expectedType, state, nestedObjectDeserializer);
		recursionLevel.Decrement();
		return result;
	}
}
