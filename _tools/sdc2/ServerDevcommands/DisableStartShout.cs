using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Game), "SpawnPlayer")]
public class DisableStartShout
{
	private static void Postfix(Game __instance)
	{
		if (Settings.DisableStartShout)
		{
			__instance.m_firstSpawn = false;
		}
	}
}
