using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;

namespace InfinityHammer;

public class ToolImportCommand
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static ConsoleEvent _003C0_003E__Execute;
	}

	public ToolImportCommand()
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
		Helper.Command("tool_import", "[hammer/hoe/etc[data] - Imports a new tool.", (ConsoleEvent)obj);
		AutoComplete.Register("tool_import", (Func<int, int, List<string>>)delegate(int index, int subIndex)
		{
			if (index == 0)
			{
				Dictionary<string, List<Tool>>.KeyCollection keys = ToolManager.Tools.Keys;
				List<string> list = new List<string>(keys.Count);
				list.AddRange(keys);
				return list;
			}
			return ParameterInfo.Create("Tool data");
		});
	}

	protected static void Execute(ConsoleEventArgs args)
	{
		Helper.ArgsCheck(args, 2, "Missing the equipment.");
		Helper.ArgsCheck(args, 3, "Missing the tool data.");
		string text = args[1];
		string args2 = HammerHelper.GetArgs("tool_import " + text, args);
		ToolData toolData = ToolManager.Import(text, args2);
		HammerHelper.Message(args.Context, "Imported tool " + toolData.name + " to " + text + ".");
	}
}
