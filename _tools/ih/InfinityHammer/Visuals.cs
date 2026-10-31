using HarmonyLib;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch]
public static class Visuals
{
	public static bool HideEffectsActive;

	[HarmonyPatch(typeof(EffectList), "Create")]
	[HarmonyPrefix]
	private static bool HideEffects()
	{
		if (HideEffectsActive)
		{
			return !Configuration.HideEffects;
		}
		return true;
	}

	[HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
	[HarmonyPostfix]
	private static void HidePlacementMarker(Player __instance)
	{
		GameObject placementMarkerInstance = __instance.m_placementMarkerInstance;
		if (Object.op_Implicit((Object)(object)placementMarkerInstance))
		{
			for (int i = 0; i < placementMarkerInstance.transform.childCount && i < 2; i++)
			{
				((Component)placementMarkerInstance.transform.GetChild(i)).gameObject.SetActive(!Configuration.HidePlacementMarker);
			}
		}
	}

	[HarmonyPatch(typeof(WearNTear), "Highlight")]
	[HarmonyPrefix]
	private static bool HideSupportColor()
	{
		if (Configuration.HideSupportColor && HammerMark.MarkedPieces.Count <= 0)
		{
			return Ruler.IsActive;
		}
		return true;
	}

	[HarmonyPatch(typeof(Hud), "UpdateCrosshair")]
	[HarmonyPostfix]
	private static void HidePieceHealth(Hud __instance)
	{
		if (Configuration.HidePieceHealth)
		{
			((Component)__instance.m_pieceHealthRoot).gameObject.SetActive(false);
		}
	}
}
