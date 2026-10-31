using System.Runtime.CompilerServices;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class ToolCmdCommand
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static ConsoleEvent _003C0_003E__Execute;
	}

	public ToolCmdCommand()
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
		Helper.Command("tool_cmd", "[command] - Selects a command to be used when placed.", (ConsoleEvent)obj);
		AutoComplete.RegisterEmpty("tool_cmd");
		AutoComplete.Offsets["tool_cmd"] = 0;
	}

	protected static void Execute(ConsoleEventArgs args)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Helper.ArgsCheck(args, 2, "Missing the command.");
		if (!Hammer.HasAny())
		{
			Hammer.Equip();
		}
		string args2 = HammerHelper.GetArgs("tool_cmd", args);
		Tool tool = new Tool(new ToolData
		{
			name = "Command",
			command = args2,
			description = args2
		});
		Selection.CreateGhost(new ToolSelection(tool));
		Hammer.SelectEmpty();
		PlaceRotation.Set(Quaternion.identity);
		HammerHelper.Message(args.Context, "Selected command " + tool.Name + ".");
	}
}
