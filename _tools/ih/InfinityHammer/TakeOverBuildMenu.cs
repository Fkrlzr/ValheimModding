using System;
using System.Collections.Generic;
using System.Globalization;
using HarmonyLib;
using ServerDevcommands;
using Service;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch]
public class TakeOverBuildMenu
{
	[HarmonyPatch(typeof(PieceTable), "UpdateAvailable")]
	[HarmonyPrefix]
	private static bool UpdateAvailablePrefix(PieceTable __instance)
	{
		if (Hammer.IsInfinityHammer(__instance))
		{
			CustomBuildMenu.HandleCustomMenuMode(__instance);
			return false;
		}
		return true;
	}

	[HarmonyPatch(typeof(PieceTable), "UpdateAvailable")]
	[HarmonyPostfix]
	[HarmonyPriority(200)]
	private static void UpdateAvailablePostfix(PieceTable __instance)
	{
		if (!Configuration.ToolsEnabled || !Object.op_Implicit((Object)(object)Player.m_localPlayer) || (Object)(object)__instance != (Object)(object)Player.m_localPlayer.m_buildPieces)
		{
			return;
		}
		if (Hammer.IsInfinityHammer(__instance))
		{
			CustomMenu.AddPipettes(__instance);
			if (HammerMenuCommand.CurrentMode != 0)
			{
				return;
			}
		}
		CustomMenu.AddTools(__instance);
		if (HammerMenuCommand.CurrentMode != MenuMode.Builds || !(HammerMenuCommand.CurrentFilter != "") || !Hammer.IsInfinityHammer(((Humanoid)Helper.GetPlayer()).GetRightItem()))
		{
			return;
		}
		Piece item = CustomMenu.BackButton();
		foreach (List<Piece> item2 in __instance.m_availablePiecesByCategory)
		{
			item2.Insert(0, item);
			__instance.m_availablePieces.Add(item);
			__instance.m_enabledPieces.Add(item);
		}
	}

	[HarmonyPatch(typeof(Player), "SetSelectedPiece", new Type[] { typeof(Vector2Int) })]
	[HarmonyPrefix]
	[HarmonyPriority(200)]
	public static bool HandleSetSelectedPiece(Player __instance, Vector2Int p)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		PieceTable buildPieces = __instance.m_buildPieces;
		if ((Object)(object)__instance != (Object)(object)Player.m_localPlayer || !Object.op_Implicit((Object)(object)buildPieces))
		{
			return true;
		}
		Piece piece = buildPieces.GetPiece(p);
		BuildMenuTool buildMenuTool = default(BuildMenuTool);
		if (Object.op_Implicit((Object)(object)piece) && ((Component)piece).TryGetComponent<BuildMenuTool>(ref buildMenuTool) && buildMenuTool.tool != null)
		{
			__instance.m_placePressedTime = -9998f;
			Tool tool = buildMenuTool.tool;
			if (!tool.Instant)
			{
				return true;
			}
			BaseSelection baseSelection = Selection.Get();
			((Terminal)Console.instance).TryRunCommand(GetInstantCommand(tool, __instance), false, false);
			if (baseSelection != Selection.Get())
			{
				buildPieces.m_selectedPiece[buildPieces.GetSelectedCategory()] = p;
			}
			return false;
		}
		return true;
	}

	[HarmonyPatch(typeof(PieceTable), "LeftPiece")]
	[HarmonyPostfix]
	private static void HandleLeftPiece(PieceTable __instance)
	{
		ActivatePiece(__instance);
	}

	[HarmonyPatch(typeof(PieceTable), "RightPiece")]
	[HarmonyPostfix]
	private static void HandleRightPiece(PieceTable __instance)
	{
		ActivatePiece(__instance);
	}

	[HarmonyPatch(typeof(PieceTable), "UpPiece")]
	[HarmonyPostfix]
	private static void HandleUpPiece(PieceTable __instance)
	{
		ActivatePiece(__instance);
	}

	[HarmonyPatch(typeof(PieceTable), "DownPiece")]
	[HarmonyPostfix]
	private static void HandleDownPiece(PieceTable __instance)
	{
		ActivatePiece(__instance);
	}

	[HarmonyPatch(typeof(PieceTable), "SetCategory", new Type[] { typeof(int) })]
	[HarmonyPostfix]
	private static void HandleSetCategoryInt(PieceTable __instance)
	{
		ActivatePiece(__instance);
	}

	[HarmonyPatch(typeof(PieceTable), "SetCategory", new Type[] { typeof(PieceCategory) })]
	[HarmonyPostfix]
	private static void HandleSetCategoryPieceCategory(PieceTable __instance)
	{
		ActivatePiece(__instance);
	}

	[HarmonyPatch(typeof(PieceTable), "PrevCategory")]
	[HarmonyPostfix]
	private static void HandlePrevCategory(PieceTable __instance)
	{
		ActivatePiece(__instance);
	}

	[HarmonyPatch(typeof(PieceTable), "NextCategory")]
	[HarmonyPostfix]
	private static void HandleNextCategory(PieceTable __instance)
	{
		ActivatePiece(__instance);
	}

	[HarmonyPatch(typeof(PieceTable), "SetSelected")]
	[HarmonyPostfix]
	private static void HandleSetSelected(PieceTable __instance)
	{
		ActivatePiece(__instance);
	}

	private static void ActivatePiece(PieceTable pt)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		if (!Object.op_Implicit((Object)(object)Player.m_localPlayer) || (Object)(object)Player.m_localPlayer.m_buildPieces != (Object)(object)pt)
		{
			return;
		}
		PieceCategory selectedCategory = pt.GetSelectedCategory();
		Vector2Int selectedIndex = pt.GetSelectedIndex();
		Piece piece = pt.GetPiece(selectedCategory, selectedIndex);
		BuildMenuTool buildMenuTool = default(BuildMenuTool);
		if (!Object.op_Implicit((Object)(object)piece))
		{
			Selection.Clear();
		}
		else if (((Component)piece).TryGetComponent<BuildMenuTool>(ref buildMenuTool) && buildMenuTool.tool != null)
		{
			Tool tool = buildMenuTool.tool;
			if (!tool.Instant)
			{
				((Terminal)Console.instance).TryRunCommand("tool " + tool.Name, false, false);
			}
			pt.m_selectedPiece[selectedCategory] = selectedIndex;
		}
		else if (Object.op_Implicit((Object)(object)((Component)piece).GetComponent<ZNetView>()))
		{
			Selection.CreateGhost(new ObjectSelection(piece, singleUse: false));
		}
		else
		{
			Selection.Clear();
		}
	}

	internal static string GetInstantCommand(Tool tool, Player player)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		string command = tool.GetCommand();
		Vector3 hoveredPosition = GetHoveredPosition(player);
		command = command.Replace("<x>", hoveredPosition.x.ToString(CultureInfo.InvariantCulture));
		command = command.Replace("<y>", hoveredPosition.y.ToString(CultureInfo.InvariantCulture));
		command = command.Replace("<z>", hoveredPosition.z.ToString(CultureInfo.InvariantCulture));
		if (command.Contains("<id>"))
		{
			ZNetView hovered = Selector.GetHovered(Configuration.Range, Array.Empty<string>(), Configuration.IgnoredIds);
			if ((Object)(object)hovered != (Object)null)
			{
				command = command.Replace("<id>", Utils.GetPrefabName(((Component)hovered).gameObject));
			}
		}
		return command;
	}

	private static Vector3 GetHoveredPosition(Player player)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		float num = ((Configuration.Range > 0f) ? Configuration.Range : 50f);
		RaycastHit val = default(RaycastHit);
		if (Physics.Raycast(new Ray(((Component)GameCamera.instance).transform.position, ((Component)GameCamera.instance).transform.forward), ref val, num, player.m_placeRayMask))
		{
			return ((RaycastHit)(ref val)).point;
		}
		return ((Component)player).transform.position;
	}
}
