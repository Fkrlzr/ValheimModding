using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(/*Could not decode attribute arguments.*/)]
public class CheatTracking
{
	private static bool Prefix(ref bool __result)
	{
		if (!Settings.DisableCheatTracking || !PermissionManager.Instance.CanCheat)
		{
			return true;
		}
		__result = true;
		return false;
	}
}
