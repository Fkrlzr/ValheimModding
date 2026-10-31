using HarmonyLib;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch]
public static class Fixes
{
	public static string ReplacedMessage = "";

	[HarmonyPatch(typeof(Player), "Message")]
	[HarmonyPrefix]
	private static void ReplaceMessage(ref string msg)
	{
		if (ReplacedMessage != "")
		{
			msg = ReplacedMessage;
			ReplacedMessage = "";
		}
	}

	[HarmonyPatch(typeof(MaterialMan), "UnregisterRenderers")]
	[HarmonyPrefix]
	private static bool UnregisterRenderers(MaterialMan __instance, GameObject gameObject)
	{
		return __instance.m_blocks.ContainsKey(((Object)gameObject).GetInstanceID());
	}

	[HarmonyPatch(typeof(ItemDrop), "MakePiece")]
	[HarmonyPrefix]
	private static bool PreventFishBecomingPieces(ItemDrop __instance)
	{
		return !Object.op_Implicit((Object)(object)((Component)__instance).GetComponent<Fish>());
	}

	[HarmonyPatch(typeof(Fish), "Awake")]
	[HarmonyPostfix]
	private static void PreventFishBeingPiece(Fish __instance)
	{
		ZNetView nview = __instance.m_nview;
		if (Object.op_Implicit((Object)(object)nview) && nview.IsValid() && nview.GetZDO().GetBool(ZDOVars.s_piece, false))
		{
			nview.GetZDO().Set(ZDOVars.s_piece, false);
		}
	}

	[HarmonyPatch(typeof(MineRock5), "Awake")]
	[HarmonyPrefix]
	private static void MineRock5NameFixPrefix(MineRock5 __instance, ref string __state)
	{
		__state = ((Object)__instance).name;
	}

	[HarmonyPatch(typeof(MineRock5), "Awake")]
	[HarmonyPostfix]
	private static void MineRock5NameFixPostfix(MineRock5 __instance, string __state)
	{
		((Object)__instance).name = __state;
	}
}
