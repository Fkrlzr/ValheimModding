using System;
using HarmonyLib;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch]
public static class DeepNorthBuildMenu
{
	[HarmonyPatch(typeof(BuildUi), "Awake")]
	[HarmonyPostfix]
	private static void InstallPieceList(BuildUi __instance)
	{
		for (int i = 0; i < __instance.m_pieceLists.Count; i++)
		{
			if (!(__instance.m_pieceLists[i] is PinnedPieceList))
			{
				IPieceList original = __instance.m_pieceLists[i];
				if (i == 0)
				{
					original = (IPieceList)(object)new HammerPieceList(original);
				}
				__instance.m_pieceLists[i] = (IPieceList)(object)new PinnedPieceList(original);
			}
		}
	}

	[HarmonyPatch(typeof(BuildUi), "OnSelectPiece")]
	[HarmonyPrefix]
	private static bool SelectTool(BuildUi __instance, Piece piece)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Player.m_localPlayer;
		BuildMenuTool buildMenuTool = default(BuildMenuTool);
		if (!Object.op_Implicit((Object)(object)localPlayer) || !Object.op_Implicit((Object)(object)localPlayer.m_buildPieces) || !Object.op_Implicit((Object)(object)piece) || !((Component)piece).TryGetComponent<BuildMenuTool>(ref buildMenuTool) || buildMenuTool.tool == null)
		{
			return true;
		}
		PieceTable buildPieces = localPlayer.m_buildPieces;
		if (!TryGetIndex(buildPieces, piece, out var index, out var category))
		{
			return false;
		}
		Tool tool = buildMenuTool.tool;
		int menuRevision = Hammer.MenuRevision;
		BaseSelection baseSelection = Selection.Get();
		localPlayer.m_placePressedTime = -9998f;
		buildPieces.m_selectedCategory = category;
		((Terminal)Console.instance).TryRunCommand(tool.Instant ? TakeOverBuildMenu.GetInstantCommand(tool, localPlayer) : ("tool " + tool.Name), false, false);
		if (menuRevision != Hammer.MenuRevision)
		{
			return false;
		}
		if (!tool.Instant || baseSelection != Selection.Get())
		{
			buildPieces.m_selectedPiece[category] = index;
		}
		if (!ZInput.IsTouchActive() || ZInput.HasDoubleTapped())
		{
			localPlayer.PlayButtonSound();
			Hud.CloseBuildUi();
		}
		return false;
	}

	[HarmonyPatch(typeof(Player), "SetSelectedPiece", new Type[] { typeof(Piece) })]
	[HarmonyPrefix]
	private static void ClearCommandSelection(Player __instance, Piece __0)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)__instance != (Object)(object)Player.m_localPlayer) && Object.op_Implicit((Object)(object)__0) && !Object.op_Implicit((Object)(object)((Component)__0).GetComponent<BuildMenuTool>()) && Object.op_Implicit((Object)(object)__instance.m_buildPieces) && TryGetIndex(__instance.m_buildPieces, __0, out var index, out var category) && !((Object)(object)Selection.Get().GetSelectedPiece() == (Object)null))
		{
			Selection.Clear();
			__instance.m_buildPieces.m_selectedCategory = category;
			__instance.m_buildPieces.m_selectedPiece[category] = index;
			__instance.SetupPlacementGhost();
		}
	}

	internal static bool TryGetIndex(PieceTable table, Piece piece, out Vector2Int index, out PieceCategory category)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected I4, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected I4, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		foreach (PieceCategory category2 in table.m_categories)
		{
			if ((int)category2 >= 0 && (int)category2 < table.m_availablePiecesByCategory.Count)
			{
				int num = table.m_availablePiecesByCategory[(int)category2].IndexOf(piece);
				if (num >= 0)
				{
					index = new Vector2Int(num % 15, num / 15);
					category = (PieceCategory)(int)category2;
					return true;
				}
			}
		}
		for (int i = 0; i < table.m_availablePiecesByCategory.Count; i++)
		{
			if (!table.m_categories.Contains((PieceCategory)i))
			{
				int num2 = table.m_availablePiecesByCategory[i].IndexOf(piece);
				if (num2 >= 0)
				{
					index = new Vector2Int(num2 % 15, num2 / 15);
					category = (PieceCategory)i;
					return true;
				}
			}
		}
		index = Vector2Int.zero;
		category = (PieceCategory)0;
		return false;
	}

	[HarmonyPatch(typeof(PieceTable), "GetPieceIndex")]
	[HarmonyPrefix]
	private static bool FindSyntheticPiece(PieceTable __instance, Piece __0, ref Vector2Int index, ref PieceCategory category, ref bool __result)
	{
		if (!Object.op_Implicit((Object)(object)__0) || (!Hammer.IsInfinityHammer(__instance) && !Object.op_Implicit((Object)(object)((Component)__0).GetComponent<BuildMenuTool>())))
		{
			return true;
		}
		__result = TryGetIndex(__instance, __0, out index, out category);
		return false;
	}
}
