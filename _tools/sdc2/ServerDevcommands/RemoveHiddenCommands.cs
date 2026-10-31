using System;
using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Terminal), "updateCommandList")]
public class RemoveHiddenCommands
{
	private static void Postfix(Terminal __instance)
	{
		__instance.m_commandList.RemoveAll((string command) => command.StartsWith("_", StringComparison.OrdinalIgnoreCase));
	}
}
