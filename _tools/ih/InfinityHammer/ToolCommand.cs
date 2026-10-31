using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class ToolCommand
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static ConsoleEvent _003C0_003E__Execute;
	}

	public ToolCommand()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		object obj = _003C_003EO._003C0_003E__Execute;
		if (obj == null)
		{
			ConsoleEvent val = Execute;
			_003C_003EO._003C0_003E__Execute = val;
			obj = (object)val;
		}
		Helper.Command("tool", "[tool] - Selects a tool to be used when placed.", (ConsoleEvent)obj);
		AutoComplete.Register("tool", (Func<int, int, List<string>>)((int index, int subIndex) => (from tool in ToolManager.GetAll()
			select tool.Name).ToList()));
	}

	protected static void Execute(ConsoleEventArgs args)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		Helper.ArgsCheck(args, 2, "Missing the tool name.");
		if (!Hammer.HasAny())
		{
			Hammer.Equip();
		}
		string args2 = HammerHelper.GetArgs("tool", args);
		if (!ToolManager.TryGetTool(Hammer.Get(), args2, out Tool tool))
		{
			tool = new Tool(new ToolData
			{
				name = "Command",
				command = args2,
				description = args2
			});
		}
		Hammer.SelectRepair();
		Selection.CreateGhost(new ToolSelection(tool));
		PlaceRotation.Set(Quaternion.identity);
		HammerHelper.Message(args.Context, "Selected tool " + tool.Name + ".");
	}
}
