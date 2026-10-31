using System;
using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace ServerDevcommands;

public class FloatConverter : IYamlTypeConverter
{
	public bool Accepts(Type type)
	{
		return type == typeof(float);
	}

	public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
	{
		float num = float.Parse(((Scalar)parser.Current).Value, NumberStyles.Float, CultureInfo.InvariantCulture);
		parser.MoveNext();
		return num;
	}

	public void WriteYaml(IEmitter emitter, object value, Type type, ObjectSerializer serializer)
	{
		emitter.Emit(new Scalar(((float)value).ToString("0.###", CultureInfo.InvariantCulture)));
	}
}
