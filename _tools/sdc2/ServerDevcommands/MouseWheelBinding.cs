using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace ServerDevcommands;

public class MouseWheelBinding
{
	public static void Execute(float ticks)
	{
		if (ticks == 0f || !Object.op_Implicit((Object)(object)Chat.instance))
		{
			return;
		}
		List<CommandBind> bestWheelCommands = BindManager.GetBestWheelCommands();
		if (bestWheelCommands.Count == 0)
		{
			return;
		}
		string command = ticks.ToString(CultureInfo.InvariantCulture);
		foreach (CommandBind item in bestWheelCommands)
		{
			((Terminal)Chat.instance).TryRunCommand(TerminalUtils.Substitute(item.Command, command), true, true);
		}
	}

	public static bool CouldExecute()
	{
		if (ZInput.GetMouseScrollWheel() == 0f)
		{
			return false;
		}
		return BindManager.CouldWheelExecute();
	}

	public static int ExecuteCount()
	{
		return BindManager.GetBestWheelCount();
	}
}
