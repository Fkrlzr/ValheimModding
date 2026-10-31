using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class ToolExportCommand
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static ConsoleEvent _003C0_003E__Execute;
	}

	public ToolExportCommand()
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
		Helper.Command("tool_export", "[name] - Exports a tool", (ConsoleEvent)obj);
		AutoComplete.Register("tool_export", (Func<int, int, List<string>>)((int index, int subIndex) => (from tool in ToolManager.GetAll()
			select tool.Name).ToList()));
	}

	protected static void Execute(ConsoleEventArgs args)
	{
		Helper.ArgsCheck(args, 2, "Missing the tool name.");
		string args2 = HammerHelper.GetArgs("tool_export", args);
		string text = ToolManager.Export(Hammer.Get(), args2);
		if (string.IsNullOrEmpty(text))
		{
			HammerHelper.Message(args.Context, "Unable to find the tool " + args2 + ".");
			return;
		}
		GUIUtility.systemCopyBuffer = text;
		HammerHelper.Message(args.Context, "Exported tool " + args2 + " to the clipboard.");
	}
}
