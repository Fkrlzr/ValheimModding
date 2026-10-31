using System;
using System.Collections.Generic;
using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ConsoleCommand), "RunAction")]
public class RunAction
{
	private static void Prefix(ConsoleCommand __instance, ConsoleEventArgs args)
	{
		TerminalUtils.IsExecuting = true;
		if (Settings.DisableCheatTracking)
		{
			__instance.IsCheat = false;
		}
		string command = args.Args[0];
		for (int i = 1; i < args.Args.Length; i++)
		{
			List<string> options = AutoComplete.GetOptions(command, i - 1);
			if (options == null)
			{
				continue;
			}
			foreach (string item in options)
			{
				if (item != null && item.Equals(args.Args[i], StringComparison.OrdinalIgnoreCase))
				{
					args.Args[i] = item;
					break;
				}
			}
		}
	}

	private static void Finalizer()
	{
		TerminalUtils.IsExecuting = false;
	}
}
