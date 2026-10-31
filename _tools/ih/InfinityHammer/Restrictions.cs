using System;
using HarmonyLib;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch]
public static class Restrictions
{
	[HarmonyPatch(typeof(Player), "HaveRequirements", new Type[]
	{
		typeof(Piece),
		typeof(RequirementMode)
	})]
	[HarmonyPrefix]
	private static bool HaveRequirements(ref bool __result)
	{
		if (Configuration.NoResourceCost)
		{
			__result = true;
			return false;
		}
		return true;
	}

	[HarmonyPatch(typeof(Player), "ConsumeResources")]
	[HarmonyPrefix]
	private static bool ConsumeResources()
	{
		return !Configuration.NoResourceCost;
	}

	[HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
	[HarmonyPrefix]
	private static void UnlockPlacementPrefix(Player __instance)
	{
		if (Object.op_Implicit((Object)(object)__instance.m_placementGhost))
		{
			Piece component = __instance.m_placementGhost.GetComponent<Piece>();
			if (Configuration.AllowInDungeons)
			{
				component.m_allowedInDungeons = true;
			}
		}
	}

	[HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
	[HarmonyFinalizer]
	private static void UnlockPlacementFinalizer(Player __instance)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)__instance.m_placementGhost) && __instance.m_placementGhost.activeSelf)
		{
			PlacementStatus placementStatus = __instance.m_placementStatus;
			if ((int)placementStatus != 3 && (int)placementStatus != 11 && (int)placementStatus != 4 && Configuration.IgnoreOtherRestrictions)
			{
				__instance.m_placementStatus = (PlacementStatus)0;
				__instance.SetPlacementGhostValid(true);
			}
		}
	}

	[HarmonyPatch(typeof(Location), "IsInsideNoBuildLocation")]
	[HarmonyPostfix]
	[HarmonyPriority(0)]
	private static bool IsInsideNoBuildLocation(bool result)
	{
		return !Configuration.IgnoreNoBuild && result;
	}

	[HarmonyPatch(typeof(Location), "IsInsideActiveBossDungeon")]
	[HarmonyPostfix]
	[HarmonyPriority(0)]
	private static bool IsInsideActiveBossDungeon(bool result)
	{
		return !Configuration.IgnoreNoBuild && result;
	}

	[HarmonyPatch(typeof(PrivateArea), "CheckAccess")]
	[HarmonyPrefix]
	[HarmonyPriority(0)]
	private static bool CheckAccess(ref bool __result)
	{
		if (Configuration.IgnoreWards)
		{
			__result = true;
		}
		return !Configuration.IgnoreWards;
	}

	[HarmonyPatch(typeof(Player), "CheckCanRemovePiece")]
	[HarmonyPrefix]
	[HarmonyPriority(0)]
	private static bool CheckCanRemovePiece(ref bool __result)
	{
		if (Configuration.NoResourceCost)
		{
			__result = true;
		}
		return !Configuration.NoResourceCost;
	}
}
