using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Console), "IsConsoleEnabled")]
public class IsConsoleEnabled
{
	private static void Postfix(ref bool __result)
	{
		__result = true;
	}
}
