using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Player), "TeleportTo")]
public class FasterTeleport1
{
	private static void Postfix(Player __instance, bool __result)
	{
		if (Settings.DebugModeFastTeleport && __result && Player.m_debugMode)
		{
			__instance.m_teleportTimer = 15f;
		}
	}
}
