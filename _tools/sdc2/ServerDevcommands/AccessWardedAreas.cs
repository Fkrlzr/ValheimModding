using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(PrivateArea), "HaveLocalAccess")]
public class AccessWardedAreas
{
	private static void Postfix(ref bool __result)
	{
		__result |= Settings.AccessWardedAreas;
	}
}
