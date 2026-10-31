using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Terminal), "tabCycle")]
public class TabCycleForceSearchRefresh
{
	private static void Postfix(Terminal __instance)
	{
		__instance.m_lastSearchLength = -1;
	}
}
