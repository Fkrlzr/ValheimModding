using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Container))]
public class AccessPrivateChests
{
	[HarmonyPatch("CheckAccess")]
	[HarmonyPostfix]
	private static bool CheckAccess(bool result)
	{
		if (!result)
		{
			return Settings.AccessPrivateChests;
		}
		return true;
	}

	[HarmonyPatch("RPC_OpenResponse")]
	[HarmonyPrefix]
	private static void RPC_OpenResponse(ref bool granted)
	{
		granted |= Settings.AccessPrivateChests;
	}
}
