using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Terminal), "tabCycle")]
public class TabCycleWithImprovedAutoComplete
{
	private static void Prefix(Terminal __instance, ref List<string> options, ref string word, bool usePrefix)
	{
		if (options == null)
		{
			return;
		}
		List<string> list = new List<string>();
		list.AddRange(options.Where((string option) => option != null && !option.Contains("?")));
		options = list;
		if (usePrefix)
		{
			if (word != null && word.StartsWith("_", StringComparison.OrdinalIgnoreCase))
			{
				List<string> list2 = new List<string>();
				list2.AddRange(options.Where((string cmd) => cmd.StartsWith("_", StringComparison.OrdinalIgnoreCase)));
				options = list2;
			}
			else
			{
				List<string> list3 = new List<string>();
				list3.AddRange(options.Where((string cmd) => !cmd.StartsWith("_", StringComparison.OrdinalIgnoreCase)));
				options = list3;
			}
		}
		if (Settings.ImprovedAutoComplete && !((Object)(object)__instance == (Object)(object)Chat.instance))
		{
			word = TerminalUtils.GetLastWord(__instance);
			if (word.EndsWith(" "))
			{
				word = "";
			}
		}
	}
}
