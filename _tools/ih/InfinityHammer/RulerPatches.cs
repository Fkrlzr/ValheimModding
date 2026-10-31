using System;
using System.Linq;
using HarmonyLib;
using Service;
using TMPro;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch]
public static class RulerPatches
{
	private static Vector2? DefaultOffset;

	private static Vector2? ToolOffset;

	[HarmonyPatch(typeof(Player), "UpdateWearNTearHover")]
	[HarmonyPrefix]
	private static bool UpdateWearNTearHover()
	{
		return !Ruler.IsActive;
	}

	private static string DescriptionHover()
	{
		if (Object.op_Implicit((Object)(object)Ruler.Projector))
		{
			return "";
		}
		ZNetView hovered = Selector.GetHovered(Configuration.Range, Array.Empty<string>(), Configuration.IgnoredIds);
		string text = (((Object)(object)hovered == (Object)null) ? "" : Utils.GetPrefabName(((Component)hovered).gameObject));
		return "id: " + text;
	}

	private static string Description(ToolSelection selection)
	{
		if (Hud.IsPieceSelectionVisible())
		{
			return "";
		}
		string[] source = new string[4]
		{
			DescriptionHover(),
			Ruler.DescriptionScale(selection),
			Ruler.DescriptionPosition(),
			selection.DescriptionPoints()
		};
		return string.Join("\n", source.Where((string s) => s != ""));
	}

	[HarmonyPatch(typeof(Hud), "SetupPieceInfo")]
	[HarmonyPrefix]
	private static void SetupPieceInfoPrefix(Hud __instance)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		RectTransform rectTransform = __instance.m_pieceDescription.rectTransform;
		if (!DefaultOffset.HasValue)
		{
			DefaultOffset = rectTransform.offsetMin;
		}
		if (!ToolOffset.HasValue)
		{
			ToolOffset = new Vector2(rectTransform.offsetMin.x, -160f);
		}
		if (rectTransform.offsetMin != DefaultOffset.Value)
		{
			rectTransform.offsetMin = DefaultOffset.Value;
		}
	}

	[HarmonyPatch(typeof(Hud), "SetupPieceInfo")]
	[HarmonyPostfix]
	private static void SetupPieceInfoPostfix(Hud __instance, Piece piece)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if (!Object.op_Implicit((Object)(object)piece) || !(Selection.Get() is ToolSelection selection) || !ToolOffset.HasValue)
		{
			return;
		}
		RectTransform rectTransform = __instance.m_pieceDescription.rectTransform;
		if (rectTransform.offsetMin != ToolOffset.Value)
		{
			rectTransform.offsetMin = ToolOffset.Value;
		}
		if (!Hud.IsPieceSelectionVisible())
		{
			string text = Description(selection);
			if (__instance.m_pieceDescription.text != "")
			{
				text = "\n" + text;
			}
			TMP_Text pieceDescription = __instance.m_pieceDescription;
			pieceDescription.text += text;
		}
	}

	[HarmonyPatch(typeof(Player), "LateUpdate")]
	[HarmonyPostfix]
	private static void UpdateToolRuler(Player __instance)
	{
		if ((Object)(object)__instance == (Object)(object)Player.m_localPlayer)
		{
			Ruler.Update();
		}
	}
}
