using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch]
public static class SelectionPatches
{
	public static bool SetItemHack;

	private static void SetItem(VisEquipment obj, VisSlot slot, int hash, int variant, int quality)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected I4, but got Unknown
		switch ((int)slot)
		{
		case 0:
			obj.m_leftItem = hash;
			obj.m_leftItemQuality = quality;
			obj.m_leftItemVariant = variant;
			break;
		case 1:
			obj.m_rightItem = hash;
			obj.m_rightItemQuality = quality;
			break;
		case 2:
			obj.m_leftBackItem = hash;
			obj.m_leftBackItemQuality = quality;
			obj.m_leftBackItemVariant = variant;
			break;
		case 3:
			obj.m_rightBackItem = hash;
			obj.m_rightBackItemQuality = quality;
			break;
		case 4:
			obj.m_chestItem = hash;
			break;
		case 5:
			obj.m_legItem = hash;
			break;
		case 6:
			obj.m_helmetItem = hash;
			break;
		case 7:
			obj.m_shoulderItem = hash;
			obj.m_shoulderItemQuality = quality;
			obj.m_shoulderItemVariant = variant;
			break;
		case 8:
			obj.m_utilityItem = hash;
			break;
		case 9:
			obj.m_beardItem = hash;
			break;
		case 10:
			obj.m_hairItem = hash;
			break;
		}
	}

	[HarmonyPatch(typeof(VisEquipment), "SetItem")]
	[HarmonyPrefix]
	private static bool SetItemPrefix(VisEquipment __instance, VisSlot slot, int itemHash, int variant, int quality)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		if (SetItemHack)
		{
			SetItem(__instance, slot, itemHash, variant, quality);
		}
		return !SetItemHack;
	}

	private static bool IsOwner(ZNetView view)
	{
		if (Object.op_Implicit((Object)(object)view))
		{
			return view.IsOwner();
		}
		return false;
	}

	[HarmonyPatch(typeof(ItemStand), "SetVisualItem")]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> ItemStandSetVisualItemNullSafe(IEnumerable<CodeInstruction> instructions)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		return new CodeMatcher(instructions, (ILGenerator)null).MatchForward(false, (CodeMatch[])(object)new CodeMatch[1]
		{
			new CodeMatch((OpCode?)OpCodes.Callvirt, (object)AccessTools.Method(typeof(ZNetView), "IsOwner", (Type[])null, (Type[])null), (string)null)
		}).Set(OpCodes.Call, (object)AccessTools.Method(typeof(SelectionPatches), "IsOwner", (Type[])null, (Type[])null)).InstructionEnumeration();
	}

	[HarmonyPatch(typeof(PieceTable), "GetSelectedPiece")]
	[HarmonyPostfix]
	private static Piece GetSelectedPiece(Piece result, PieceTable __instance)
	{
		if (!Configuration.Enabled || !Object.op_Implicit((Object)(object)Player.m_localPlayer) || (Object)(object)Player.m_localPlayer.m_buildPieces != (Object)(object)__instance)
		{
			return result;
		}
		Piece selectedPiece = Selection.Get().GetSelectedPiece();
		if (Object.op_Implicit((Object)(object)selectedPiece))
		{
			return selectedPiece;
		}
		BuildMenuTool buildMenuTool = default(BuildMenuTool);
		if (Object.op_Implicit((Object)(object)result) && ((Component)result).TryGetComponent<BuildMenuTool>(ref buildMenuTool))
		{
			Tool? tool = buildMenuTool.tool;
			if (tool != null && tool.Instant)
			{
				return null;
			}
		}
		return result;
	}

	[HarmonyPatch(typeof(Player), "SetPlaceMode")]
	[HarmonyPostfix]
	private static void SelectionActivate(Player __instance)
	{
		if ((Object)(object)__instance == (Object)(object)Player.m_localPlayer)
		{
			Selection.Get().Activate();
		}
	}

	[HarmonyPatch(typeof(Player), "OnDestroy")]
	[HarmonyPrefix]
	private static void PlayerOnDestroy(Player __instance)
	{
		if ((Object)(object)__instance == (Object)(object)Player.m_localPlayer)
		{
			Player_ManualUpdate.Projector = null;
			Selection.Destroy();
			TrackPreviewMaterials.Clear();
		}
	}
}
