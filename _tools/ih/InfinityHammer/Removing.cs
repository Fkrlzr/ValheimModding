using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using Service;
using UnityEngine;
using WorldEditCommands;

namespace InfinityHammer;

[HarmonyPatch]
public static class Removing
{
	public static bool DisableDrops;

	[HarmonyPatch(typeof(Player), "RemovePiece")]
	[HarmonyPrefix]
	private static bool RemovePiecePrefix(Player __instance, ref bool __result, ref float __state)
	{
		__state = __instance.m_maxPlaceDistance;
		if (Configuration.Range > 0f)
		{
			__instance.m_maxPlaceDistance = Configuration.Range;
		}
		UndoHelper.BeginAction();
		Visuals.HideEffectsActive = true;
		DisableDrops = Configuration.DisableLoot;
		if (Configuration.RemoveAnything)
		{
			__result = RemoveAnything(__instance);
			return false;
		}
		return true;
	}

	[HarmonyPatch(typeof(Player), "RemovePiece")]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> RemovePieceTranspiler(IEnumerable<CodeInstruction> instructions)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		return new CodeMatcher(instructions, (ILGenerator)null).MatchForward(false, (CodeMatch[])(object)new CodeMatch[1]
		{
			new CodeMatch((OpCode?)OpCodes.Call, (object)AccessTools.PropertyGetter(typeof(ZNetScene), "instance"), (string)null)
		}).InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
		{
			new CodeInstruction(OpCodes.Ldloc_1, (object)null)
		}).InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
		{
			new CodeInstruction(OpCodes.Call, (object)AccessTools.Method(typeof(Removing), "HandleRemoved", (Type[])null, (Type[])null))
		})
			.InstructionEnumeration();
	}

	[HarmonyPatch(typeof(Player), "RemovePiece")]
	[HarmonyFinalizer]
	private static void RemovePieceFinalizer(Player __instance, float __state)
	{
		__instance.m_maxPlaceDistance = __state;
		UndoHelper.EndAction();
		Visuals.HideEffectsActive = false;
		DisableDrops = false;
	}

	private static void HandleRemoved(Piece piece)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		ZDO zDO = piece.m_nview.GetZDO();
		Vector3 position = ((Component)piece).transform.position;
		int prefab = zDO.m_prefab;
		ZDOID uid = zDO.m_uid;
		UndoHelper.AddRemoveAction(zDO);
		RemoveInArea(uid, prefab, position, Configuration.RemoveArea);
	}

	private static bool RemoveAnything(Player obj)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		Hovered hovered = Selector.GetHovered(obj, obj.m_maxPlaceDistance, Array.Empty<string>(), Configuration.RemoveIds, false);
		if (hovered == null)
		{
			return false;
		}
		obj.m_removeEffects.Create(((Component)hovered.Obj).transform.position, Quaternion.identity, (Transform)null, 1f, -1, default(ZDOID));
		Vector3 position = ((Component)hovered.Obj).transform.position;
		int prefab = hovered.Obj.GetZDO().m_prefab;
		ZDOID uid = hovered.Obj.GetZDO().m_uid;
		if (Remove(hovered))
		{
			RemoveInArea(uid, prefab, position, Configuration.RemoveArea);
		}
		ItemData rightItem = ((Humanoid)obj).GetRightItem();
		if (rightItem != null)
		{
			obj.FaceLookDirection();
			((Character)obj).m_zanim.SetTrigger(rightItem.m_shared.m_attack.m_attackAnimation);
		}
		return true;
	}

	private static bool Remove(Hovered hovered)
	{
		ZNetView obj = hovered.Obj;
		if (Object.op_Implicit((Object)(object)((Component)obj).GetComponent<DungeonGenerator>()) && hovered.Index > -1)
		{
			if (((Component)obj).transform.childCount > 1)
			{
				UndoHelper.AddEditAction(obj.GetZDO());
			}
			else
			{
				UndoHelper.AddRemoveAction(obj.GetZDO());
			}
			DungeonRooms.RemoveRoom(obj, hovered.Index);
			if (((Component)obj).transform.childCount == 0)
			{
				HammerHelper.RemoveZDO(obj.GetZDO());
				return true;
			}
			return false;
		}
		Remove(obj);
		return true;
	}

	private static void Remove(ZNetView obj)
	{
		CharacterDrop component = ((Component)obj).GetComponent<CharacterDrop>();
		if (component != null)
		{
			component.OnDeath();
		}
		Piece component2 = ((Component)obj).GetComponent<Piece>();
		if (component2 != null)
		{
			component2.DropResources((HitData)null);
		}
		if (!Configuration.RemoveContents)
		{
			ArmorStand component3 = ((Component)obj).GetComponent<ArmorStand>();
			if (component3 != null)
			{
				component3.OnDestroyed();
			}
			ItemStand component4 = ((Component)obj).GetComponent<ItemStand>();
			if (component4 != null)
			{
				component4.OnDestroyed();
			}
			Container component5 = ((Component)obj).GetComponent<Container>();
			if (component5 != null)
			{
				component5.OnDestroyed();
			}
		}
		UndoHelper.AddRemoveAction(obj.GetZDO());
		HammerHelper.RemoveZDO(obj.GetZDO());
	}

	private static void RemoveInArea(ZDOID id, int prefab, Vector3 position, float radius)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (radius != 0f)
		{
			ZNetView[] array = ZNetScene.instance.m_instances.Values.Where((ZNetView view) => Object.op_Implicit((Object)(object)view) && view.IsValid() && view.GetZDO().m_prefab == prefab && Vector3.Distance(position, view.GetZDO().m_position) < radius && view.GetZDO().m_uid != id).ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				Remove(array[i]);
			}
		}
	}

	[HarmonyPatch(typeof(WearNTear), "Remove")]
	[HarmonyPrefix]
	private static void WearNTearRemove(WearNTear __instance)
	{
		if (__instance.m_nview.IsValid())
		{
			UndoHelper.AddRemoveAction(__instance.m_nview.GetZDO());
		}
	}

	[HarmonyPatch(typeof(Destructible), "CreateFragments")]
	[HarmonyPrefix]
	private static bool PreventDestructibleFragments()
	{
		return !Configuration.HideEffects;
	}

	[HarmonyPatch(typeof(Piece), "DropResources")]
	[HarmonyPrefix]
	private static bool PreventPieceDrops()
	{
		return !DisableDrops;
	}

	[HarmonyPatch(typeof(CharacterDrop), "OnDeath")]
	[HarmonyPrefix]
	private static bool PreventCreatureDrops()
	{
		return !DisableDrops;
	}
}
