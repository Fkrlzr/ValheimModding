using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Player), "UpdateTeleport")]
public class FasterTeleport2
{
	private static void Postfix(Player __instance)
	{
		if (Settings.DebugModeFastTeleport && Player.m_debugMode)
		{
			__instance.m_teleportCooldown = Mathf.Max(__instance.m_teleportCooldown, 1.5f);
		}
	}
}
