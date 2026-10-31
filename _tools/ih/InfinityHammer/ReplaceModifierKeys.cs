using System;
using HarmonyLib;

namespace InfinityHammer;

[HarmonyPatch(typeof(Terminal), "TryRunCommand")]
public class ReplaceModifierKeys
{
	[HarmonyPriority(600)]
	private static void Prefix(ref string text)
	{
		if (!text.StartsWith("bind ", StringComparison.OrdinalIgnoreCase) && !text.StartsWith("alias ", StringComparison.OrdinalIgnoreCase))
		{
			text = text.Replace("<mod1>", Configuration.ModifierKey1());
			text = text.Replace("<mod2>", Configuration.ModifierKey2());
		}
	}
}
