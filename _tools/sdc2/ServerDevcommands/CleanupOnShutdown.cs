using HarmonyLib;
using Service;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ZNet), "Shutdown")]
public class CleanupOnShutdown
{
	private static void Postfix()
	{
		MultiCommands.ClearGroups();
		ServerChat.Clear();
		CommandInputResolver.Clear();
		UndoManager.Clear();
	}
}
