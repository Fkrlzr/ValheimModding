using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;

namespace ServerDevcommands;

public class MultilineScalarFlowStyleEmitter : ChainedEventEmitter
{
	public MultilineScalarFlowStyleEmitter(IEventEmitter nextEmitter)
		: base(nextEmitter)
	{
	}

	public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
	{
		if (typeof(string).IsAssignableFrom(eventInfo.Source.Type))
		{
			string text = eventInfo.Source.Value as string;
			if (!string.IsNullOrEmpty(text) && text.IndexOfAny(new char[5] { '\r', '\n', '\u0085', '\u2028', '\u2029' }) >= 0)
			{
				eventInfo = new ScalarEventInfo(eventInfo.Source)
				{
					Style = ScalarStyle.Literal
				};
			}
		}
		nextEmitter.Emit(eventInfo, emitter);
	}
}
