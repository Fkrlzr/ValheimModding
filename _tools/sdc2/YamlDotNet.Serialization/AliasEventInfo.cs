using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization;

public class AliasEventInfo : EventInfo
{
	public AnchorName Alias { get; }

	public bool NeedsExpansion { get; set; }

	public AliasEventInfo(IObjectDescriptor source, AnchorName alias)
		: base(source)
	{
		if (alias.IsEmpty)
		{
			throw new ArgumentNullException("alias");
		}
		Alias = alias;
	}
}
