using HarmonyLib;
using TMPro;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Minimap), "Awake")]
public class Minimap_Alignment
{
	private static void Postfix(Minimap __instance)
	{
		__instance.m_biomeNameSmall.alignment = (TextAlignmentOptions)260;
		__instance.m_biomeNameLarge.alignment = (TextAlignmentOptions)260;
	}
}
