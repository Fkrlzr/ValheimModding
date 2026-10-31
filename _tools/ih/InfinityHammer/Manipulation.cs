using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch]
public static class Manipulation
{
	private static Vector3 CurrentNormal = Vector3.up;

	[HarmonyPatch(typeof(Player), "FindClosestSnapPoints")]
	[HarmonyPrefix]
	private static bool DisableSnapsWhenFrozen()
	{
		return !Position.Override.HasValue;
	}

	[HarmonyPatch(typeof(Player), "PieceRayTest")]
	[HarmonyPostfix]
	private static void FreezePlacementMarker(ref Vector3 point, ref Vector3 normal, ref Piece piece, ref Heightmap heightmap, ref Collider waterSurface, ref bool __result)
	{
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		if (__result && Grid.Enabled)
		{
			point = Grid.Apply(point, Object.op_Implicit((Object)(object)heightmap) ? Vector3.up : normal);
			if (Object.op_Implicit((Object)(object)heightmap))
			{
				point.y = ZoneSystem.instance.GetGroundHeight(point) + 2f;
				RaycastHit val = default(RaycastHit);
				if (Physics.Raycast(point, Vector3.down, ref val, 50f, Player.m_localPlayer.m_placeRayMask))
				{
					point = ((RaycastHit)(ref val)).point;
					normal = ((RaycastHit)(ref val)).normal;
				}
			}
		}
		if (Position.Override.HasValue)
		{
			point = Position.Override.Value;
			normal = CurrentNormal;
			__result = true;
			piece = null;
			heightmap = null;
			waterSurface = null;
		}
		else
		{
			CurrentNormal = normal;
		}
	}

	[HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> OverridePlacementGhost(IEnumerable<CodeInstruction> instructions)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		return new CodeMatcher(instructions, (ILGenerator)null).MatchForward(false, (CodeMatch[])(object)new CodeMatch[1]
		{
			new CodeMatch((OpCode?)OpCodes.Call, (object)AccessTools.Method(typeof(Location), "IsInsideNoBuildLocation", (Type[])null, (Type[])null), (string)null)
		}).Advance(-2).Insert((CodeInstruction[])(object)new CodeInstruction[3]
		{
			new CodeInstruction(OpCodes.Call, Transpilers.EmitDelegate<Action<GameObject>>((Action<GameObject>)Position.Apply).operand),
			new CodeInstruction(OpCodes.Ldarg_0, (object)null),
			new CodeInstruction(OpCodes.Ldfld, (object)AccessTools.Field(typeof(Player), "m_placementGhost"))
		})
			.InstructionEnumeration();
	}

	[HarmonyPatch(typeof(Humanoid), "UnequipItem")]
	[HarmonyPrefix]
	private static void UnfreezeOnUnequip(Humanoid __instance, ItemData item)
	{
		if (!((Object)(object)__instance != (Object)(object)Player.m_localPlayer) && item != null && Configuration.UnfreezeOnUnequip)
		{
			Position.Unfreeze();
		}
	}

	[HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
	[HarmonyPrefix]
	private static void DisableSnapCycleWhenTypingPrefix(Player __instance, ref int __state)
	{
		__state = __instance.m_manualSnapPoint;
	}

	[HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
	[HarmonyPostfix]
	private static void DisableSnapCycleWhenTypingPostfix(Player __instance, int __state)
	{
		if (!((Character)__instance).TakeInput())
		{
			__instance.m_manualSnapPoint = __state;
		}
	}
}
