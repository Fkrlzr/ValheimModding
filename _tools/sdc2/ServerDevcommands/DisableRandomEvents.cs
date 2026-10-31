using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(RandEventSystem), "StartRandomEvent")]
public class DisableRandomEvents
{
	private static bool Prefix()
	{
		return !Settings.DisableEvents;
	}
}
