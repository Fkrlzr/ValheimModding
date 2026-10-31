using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Game), "Start")]
public class AdminReset
{
	private static void Postfix()
	{
		Admin.Reset();
	}
}
