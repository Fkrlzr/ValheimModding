using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Data;
using HarmonyLib;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch]
public static class Placing
{
	public static bool? SpawnRandomDamage;

	public static bool SpawnAllViews;

	[HarmonyPatch(typeof(Player), "PlacePiece")]
	[HarmonyPrefix]
	private static void PlacePiecePrefix()
	{
		Visuals.HideEffectsActive = true;
	}

	[HarmonyPatch(typeof(Player), "PlacePiece")]
	[HarmonyFinalizer]
	private static void PlacePieceFinalizer()
	{
		Visuals.HideEffectsActive = false;
		DataHelper.CleanUp();
		if (Selection.Get().SingleUse)
		{
			Hammer.SelectRepair();
		}
	}

	private static GameObject GetPrefab(GameObject obj, Vector3 pos, Quaternion rot)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (!Configuration.Enabled)
		{
			return obj;
		}
		return Selection.Get().GetPrefab(obj, pos, rot);
	}

	private static void Postprocess(GameObject obj)
	{
		if (Configuration.Enabled)
		{
			Selection.Get().AfterPlace(obj);
		}
	}

	[HarmonyPatch(typeof(Player), "PlacePiece")]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> PlacePieceTranspiler(IEnumerable<CodeInstruction> instructions)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		return new CodeMatcher(instructions, (ILGenerator)null).MatchForward(false, (CodeMatch[])(object)new CodeMatch[1]
		{
			new CodeMatch((OpCode?)OpCodes.Callvirt, (object)AccessTools.PropertyGetter(typeof(Component), "gameObject"), (string)null)
		}).Advance(1).Insert((CodeInstruction[])(object)new CodeInstruction[3]
		{
			new CodeInstruction(OpCodes.Ldarg_2, (object)null),
			new CodeInstruction(OpCodes.Ldarg_3, (object)null),
			new CodeInstruction(OpCodes.Call, Transpilers.EmitDelegate<Func<GameObject, Vector3, Quaternion, GameObject>>((Func<GameObject, Vector3, Quaternion, GameObject>)GetPrefab).operand)
		})
			.MatchForward(false, (CodeMatch[])(object)new CodeMatch[1]
			{
				new CodeMatch((OpCode?)OpCodes.Ldfld, (object)AccessTools.Field(typeof(Piece), "m_placeEffect"), (string)null)
			})
			.InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
			{
				new CodeInstruction(OpCodes.Ldloc_0, (object)null)
			})
			.InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
			{
				new CodeInstruction(OpCodes.Call, (object)AccessTools.Method(typeof(Placing), "Postprocess", (Type[])null, (Type[])null))
			})
			.InstructionEnumeration();
	}

	[HarmonyPatch(typeof(Player), "UpdatePlacement")]
	[HarmonyPrefix]
	private static void HoldUsePrefix(Player __instance, ref ItemData __state)
	{
		__state = ((Humanoid)__instance).GetRightItem();
		Hammer.RemoveToolCosts(__state);
		Hammer.AddRemoveAnything(__state);
	}

	private static Transform RemoveFailSafe(Piece piece)
	{
		if (!((Object)(object)piece == (Object)null))
		{
			return ((Component)piece).transform;
		}
		return ((Component)Player.m_localPlayer).transform;
	}

	[HarmonyPatch(typeof(Player), "UpdatePlacement")]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> HoldUseTranspiler(IEnumerable<CodeInstruction> instructions)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		return new CodeMatcher(instructions, (ILGenerator)null).MatchForward(false, (CodeMatch[])(object)new CodeMatch[1]
		{
			new CodeMatch((OpCode?)OpCodes.Ldfld, (object)AccessTools.Field(typeof(SharedData), "m_destroyEffect"), (string)null)
		}).Advance(2).Set(OpCodes.Call, (object)AccessTools.Method(typeof(Placing), "RemoveFailSafe", (Type[])null, (Type[])null))
			.InstructionEnumeration();
	}

	[HarmonyPatch(typeof(Player), "UpdatePlacement")]
	[HarmonyPostfix]
	private static void HoldUsePostfix(Player __instance)
	{
		if (Selection.Get().Continuous && __instance.m_placePressedTime == -9999f)
		{
			if (!ZInput.GetButton("Attack") && !ZInput.GetButton("JoyPlace"))
			{
				__instance.m_placePressedTime = -9998f;
			}
			else if (!(Time.time - __instance.m_lastToolUseTime < __instance.m_placeDelay / 4f))
			{
				__instance.m_placePressedTime = Time.time;
				__instance.m_lastToolUseTime = 0f;
			}
		}
	}

	[HarmonyPatch(typeof(Player), "UpdatePlacement")]
	[HarmonyFinalizer]
	private static void HoldUseFinalizer(ItemData __state)
	{
		Hammer.RestoreToolCosts(__state);
		Hammer.RestoreRemoveAnything(__state);
	}

	[HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
	[HarmonyPrefix]
	private static void UnlockBuildDistancePrefix(Player __instance, ref float __state)
	{
		__state = __instance.m_maxPlaceDistance;
		BaseSelection baseSelection = Selection.Get();
		if (baseSelection != null)
		{
			__instance.m_maxPlaceDistance = baseSelection.MaxPlaceDistance(__instance.m_maxPlaceDistance);
		}
	}

	[HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
	[HarmonyPostfix]
	private static void UnlockBuildDistancePostfix(Player __instance, float __state)
	{
		__instance.m_maxPlaceDistance = __state;
	}

	private static void Customize(ZNetView[] views)
	{
		if (SpawnRandomDamage.HasValue)
		{
			WearNTear.m_randomInitialDamage = SpawnRandomDamage.Value;
		}
		if (SpawnAllViews)
		{
			for (int i = 0; i < views.Length; i++)
			{
				((Component)views[i]).gameObject.SetActive(true);
			}
		}
	}

	[HarmonyPatch(typeof(ZoneSystem), "SpawnLocation")]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> SpawnLocationTranspiler(IEnumerable<CodeInstruction> instructions)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		return new CodeMatcher(instructions, (ILGenerator)null).MatchForward(false, (CodeMatch[])(object)new CodeMatch[1]
		{
			new CodeMatch((OpCode?)OpCodes.Stsfld, (object)AccessTools.Field(typeof(WearNTear), "m_randomInitialDamage"), (string)null)
		}).Advance(1).InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
		{
			new CodeInstruction(OpCodes.Ldloc_0, (object)null)
		})
			.InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
			{
				new CodeInstruction(OpCodes.Call, Transpilers.EmitDelegate<Action<ZNetView[]>>((Action<ZNetView[]>)Customize).operand)
			})
			.InstructionEnumeration();
	}
}
