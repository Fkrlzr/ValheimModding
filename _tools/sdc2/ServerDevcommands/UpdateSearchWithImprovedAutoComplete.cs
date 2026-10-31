using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Terminal), "updateSearch")]
public class UpdateSearchWithImprovedAutoComplete
{
	private static void Prefix(Terminal __instance, ref string word)
	{
		if (Settings.ImprovedAutoComplete && Object.op_Implicit((Object)(object)__instance.m_search) && !((Object)(object)__instance == (Object)(object)Chat.instance))
		{
			word = TerminalUtils.GetLastWord(__instance);
		}
	}

	private static void Postfix(Terminal __instance, List<string> options, ref string word, bool usePrefix)
	{
		if (usePrefix)
		{
			options = ((!word.StartsWith("_", StringComparison.OrdinalIgnoreCase)) ? options.Where((string cmd) => !cmd.StartsWith("_", StringComparison.OrdinalIgnoreCase)).ToList() : options.Where((string cmd) => cmd.StartsWith("_", StringComparison.OrdinalIgnoreCase)).ToList());
		}
		if (!Settings.ImprovedAutoComplete || options == null || !Object.op_Implicit((Object)(object)__instance.m_search) || (Object)(object)__instance == (Object)(object)Chat.instance)
		{
			return;
		}
		if (Settings.CommandDescriptions && options == __instance.m_commandList && Terminal.commands.TryGetValue(word, out var value) && value.ShowCommand(__instance))
		{
			options = ParameterInfo.Create(value.Description);
		}
		if (options.All((string option) => option.StartsWith("?")))
		{
			__instance.m_search.text = "<color=white>" + string.Join(", ", options.Select((string option) => option.Substring(1))) + "</color>";
		}
	}
}
