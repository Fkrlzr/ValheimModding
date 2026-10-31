using YamlDotNet.Core;

namespace YamlDotNet.Serialization;

public class ObjectEventInfo : EventInfo
{
	public AnchorName Anchor { get; set; }

	public TagName Tag { get; set; }

	protected ObjectEventInfo(IObjectDescriptor source)
		: base(source)
	{
	}
}
