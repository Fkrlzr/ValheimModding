using System;
using System.Collections.Generic;
using ServerDevcommands;

namespace WorldEditCommands;

public class TweakParameters : BaseParameters
{
	public long Creator;

	public bool Force;

	public TweakParameters(Dictionary<string, Type> supportedOperations)
		: base(supportedOperations)
	{
	}

	protected override void ParseArg(string arg)
	{
		if (arg == "force")
		{
			Force = true;
		}
	}

	protected override void ParseArg(string arg, string value)
	{
		if (arg == "creator")
		{
			Creator = Parse.Long(value, 0L);
		}
	}
}
