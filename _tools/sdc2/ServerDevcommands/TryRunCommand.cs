using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Terminal), "TryRunCommand")]
public class TryRunCommand
{
	private static bool Prefix(Terminal __instance, ref string text)
	{
		if (TerminalUtils.IsBlockedHelp(text))
		{
			return false;
		}
		if (!Settings.ImprovedChat && (Object)(object)__instance == (Object)(object)Chat.instance)
		{
			return true;
		}
		RemoveWhitespacedFromHistory(__instance);
		if (TerminalUtils.SkipProcessing(text))
		{
			return true;
		}
		if (TerminalUtils.TryResolveInputPlaceholders(__instance, text))
		{
			return false;
		}
		string[] array = MultiCommands.Split(text).Select(Aliasing.Plain).SelectMany(MultiCommands.Split)
			.ToArray();
		if (array.Length > 1)
		{
			MultiCommands.Handle(__instance, array);
			return false;
		}
		text = array[0];
		text = CheckLogic(text);
		RunCommand(__instance, text);
		return false;
	}

	private static void RemoveWhitespacedFromHistory(Terminal terminal)
	{
		List<string> history = terminal.m_history;
		if (history.Count != 0)
		{
			string text = history[history.Count - 1];
			if (text[0] == ' ' || text[0] == '\t')
			{
				history.RemoveAt(history.Count - 1);
			}
		}
	}

	private static string CheckLogic(string text)
	{
		IEnumerable<string> values = text.Split(' ').Select(delegate(string arg)
		{
			string[] array = arg.Split('=');
			if (array.Length == 1)
			{
				return Parse.Logic(arg);
			}
			return (array.Length > 2) ? arg : (array[0] + "=" + Parse.Logic(array[1]));
		});
		return string.Join(" ", values);
	}

	private static void RunCommand(Terminal t, string text)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		string text2 = text.Split(' ')[0];
		if (!Terminal.commands.TryGetValue(text2, out var value))
		{
			t.AddString("Unknown command '" + text2 + "'. Type 'help' to see a list of valid commands");
		}
		else if (!PermissionManager.Instance.IsCommandAllowed(value, text, remote: false))
		{
			t.AddString("Unauthorized to use command '" + text2 + "'.");
		}
		else if (value.RemoteCommand && Object.op_Implicit((Object)(object)ZNet.instance) && !ZNet.instance.IsServer())
		{
			t.AddString("Sending command: " + text);
			ZNet.instance.RemoteCommand(text);
		}
		else
		{
			value.RunAction(new ConsoleEventArgs(text, t, value));
		}
	}
}
