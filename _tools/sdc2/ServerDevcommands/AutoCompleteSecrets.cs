using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Terminal), "Awake")]
public class AutoCompleteSecrets
{
	private static void Postfix(Terminal __instance)
	{
		__instance.m_autoCompleteSecrets = true;
	}
}
