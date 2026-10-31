using System;
using System.Linq;
using HarmonyLib;
using Service;
using UnityEngine;
using WorldEditCommands;

namespace InfinityHammer;

[HarmonyPatch]
public static class Repairing
{
	[HarmonyPatch(typeof(Player), "Repair")]
	[HarmonyPrefix]
	private static void RepairPrefix(Player __instance)
	{
		Visuals.HideEffectsActive = true;
		UndoHelper.BeginAction();
		if (Configuration.RepairAnything && ((Character)__instance).InPlaceMode() && !Object.op_Implicit((Object)(object)__instance.GetHoveringPiece()))
		{
			RepairAnything(__instance);
		}
	}

	[HarmonyPatch(typeof(Player), "Repair")]
	[HarmonyFinalizer]
	private static void RepairFinalizer()
	{
		UndoHelper.EndAction();
		Visuals.HideEffectsActive = false;
	}

	private static bool RepairPlayer(ZNetView obj)
	{
		Player component = ((Component)obj).GetComponent<Player>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return false;
		}
		float num = ((Character)component).GetMaxHealth() - ((Character)component).GetHealth();
		if (num == 0f)
		{
			return false;
		}
		((Character)component).Heal(num, true);
		return true;
	}

	public static bool RepairStructure(ZNetView obj)
	{
		WearNTear component = ((Component)obj).GetComponent<WearNTear>();
		if (!Object.op_Implicit((Object)(object)component) || Time.time - component.m_lastRepair < 1f)
		{
			return false;
		}
		bool flag = RepairShared(obj);
		if (flag)
		{
			component.m_lastRepair = Time.time;
			obj.InvokeRPC(ZNetView.Everybody, "RPC_HealthChanged", new object[1] { obj.GetZDO().GetFloat(ZDOVars.s_health, component.m_health) });
		}
		return flag;
	}

	private static bool RepairMineRock(ZNetView obj, MineRock mineRock, int index)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		ZDO zDO = obj.GetZDO();
		Collider hitArea = mineRock.GetHitArea(index);
		Vector3 val;
		Bounds bounds;
		if (hitArea == null)
		{
			val = ((Component)Player.m_localPlayer).transform.position;
		}
		else
		{
			bounds = hitArea.bounds;
			val = ((Bounds)(ref bounds)).center;
		}
		Vector3 val2 = val;
		int? num = null;
		float num2 = float.MaxValue;
		for (int i = 0; i < mineRock.m_hitAreas.Length; i++)
		{
			int stableHashCode = StringExtensionMethods.GetStableHashCode("Health" + i);
			if (!(zDO.GetFloat(stableHashCode, 1f) > 0f))
			{
				bounds = mineRock.m_hitAreas[i].bounds;
				float num3 = Vector3.Distance(((Bounds)(ref bounds)).center, val2);
				if (num3 < num2)
				{
					num = i;
					num2 = num3;
				}
			}
		}
		if (num.HasValue)
		{
			int? num4 = num;
			int stableHashCode2 = StringExtensionMethods.GetStableHashCode("Health" + num4);
			zDO.RemoveFloat(stableHashCode2);
			DamageText instance = DamageText.instance;
			bounds = mineRock.m_hitAreas[num.Value].bounds;
			instance.ShowText((TextType)4, ((Bounds)(ref bounds)).center, GetMaxHealth(mineRock.m_health), false);
			mineRock.UpdateVisability();
			return true;
		}
		return false;
	}

	private static bool RepairMineRock(MineRock5 mineRock, int index)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		HitArea hitArea = mineRock.GetHitArea(index);
		Vector3 val;
		Bounds bounds;
		if (hitArea == null)
		{
			val = ((Component)Player.m_localPlayer).transform.position;
		}
		else
		{
			bounds = hitArea.m_collider.bounds;
			val = ((Bounds)(ref bounds)).center;
		}
		Vector3 val2 = val;
		float health = ((Configuration.OverwriteHealth > 0f) ? Configuration.OverwriteHealth : mineRock.m_health);
		HitArea val3 = null;
		float num = float.MaxValue;
		foreach (HitArea hitArea2 in mineRock.m_hitAreas)
		{
			if (!(hitArea2.m_health > 0f))
			{
				bounds = hitArea2.m_collider.bounds;
				float num2 = Vector3.Distance(((Bounds)(ref bounds)).center, val2);
				if (num2 < num)
				{
					val3 = hitArea2;
					num = num2;
				}
			}
		}
		if (val3 != null)
		{
			val3.m_health = health;
			mineRock.SaveHealth();
			mineRock.UpdateMesh();
			DamageText instance = DamageText.instance;
			bounds = val3.m_collider.bounds;
			instance.ShowText((TextType)4, ((Bounds)(ref bounds)).center, GetMaxHealth(mineRock.m_health), false);
			return true;
		}
		return false;
	}

	private static float GetMaxHealth(float defaultValue)
	{
		if (!(Configuration.Invulnerability == "Off"))
		{
			return float.PositiveInfinity;
		}
		if (Configuration.OverwriteHealth != 0f)
		{
			return Configuration.OverwriteHealth;
		}
		return defaultValue;
	}

	private static bool RepairShared(ZNetView obj)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		obj.ClaimOwnership();
		float num = CustomHealth.SetHealth(obj, isRepair: true);
		if (num == 0f)
		{
			return false;
		}
		DamageText.instance.ShowText((TextType)4, ((Component)obj).transform.position, num, false);
		return true;
	}

	private static bool RepairInArea(ZDO zdo, float radius)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		if (radius == 0f)
		{
			return false;
		}
		Vector3 position = zdo.m_position;
		int prefab = zdo.m_prefab;
		ZNetView[] array = ZNetScene.instance.m_instances.Values.Where((ZNetView view) => Object.op_Implicit((Object)(object)view) && view.IsValid() && view.GetZDO().m_prefab == prefab && Vector3.Distance(position, view.GetZDO().m_position) < radius).ToArray();
		bool flag = false;
		ZNetView[] array2 = array;
		foreach (ZNetView obj in array2)
		{
			flag |= RepairObject(obj, 0);
		}
		return flag;
	}

	private static bool RepairObject(ZNetView obj, int index)
	{
		if (Object.op_Implicit((Object)(object)((Component)obj).GetComponent<Player>()))
		{
			return RepairPlayer(obj);
		}
		if (RepairShared(obj))
		{
			return true;
		}
		MineRock5 mineRock = default(MineRock5);
		if (((Component)obj).TryGetComponent<MineRock5>(ref mineRock))
		{
			return RepairMineRock(mineRock, index);
		}
		MineRock mineRock2 = default(MineRock);
		if (((Component)obj).TryGetComponent<MineRock>(ref mineRock2))
		{
			return RepairMineRock(obj, mineRock2, index);
		}
		return false;
	}

	private static bool RepairAnything(Player player)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		float num = ((Configuration.Range > 0f) ? Configuration.Range : player.m_maxPlaceDistance);
		Hovered hovered = Selector.GetHovered(player, num, Array.Empty<string>(), Array.Empty<string>(), true);
		if (hovered == null)
		{
			return false;
		}
		ZNetView obj = hovered.Obj;
		UndoHelper.AddEditAction(obj.GetZDO());
		if (!RepairObject(obj, hovered.Index))
		{
			return false;
		}
		Piece component = ((Component)obj).GetComponent<Piece>();
		string text = (Object.op_Implicit((Object)(object)component) ? component.m_name : Utils.GetPrefabName(((Component)obj).gameObject));
		if (component != null)
		{
			component.m_placeEffect.Create(((Component)obj).transform.position, ((Component)obj).transform.rotation, (Transform)null, 1f, -1, default(ZDOID));
		}
		((Character)player).Message((MessageType)1, Localization.instance.Localize("$msg_repaired", new string[1] { text }), 0, (Sprite)null, false);
		ItemData rightItem = ((Humanoid)player).GetRightItem();
		if (rightItem != null)
		{
			player.FaceLookDirection();
			((Character)player).m_zanim.SetTrigger(rightItem.m_shared.m_attack.m_attackAnimation);
			((Character)player).UseStamina(rightItem.m_shared.m_attack.m_attackStamina);
			((Character)player).UseEitr(rightItem.m_shared.m_attack.m_attackEitr);
			if (rightItem.m_shared.m_useDurability)
			{
				rightItem.m_durability -= rightItem.m_shared.m_useDurabilityDrain;
			}
		}
		return true;
	}

	[HarmonyPatch(typeof(Player), "UpdateWearNTearHover")]
	[HarmonyPrefix]
	private static void UnlockRepairDistancePrefix(Player __instance, ref float __state)
	{
		__state = __instance.m_maxPlaceDistance;
		if (Configuration.Range > 0f)
		{
			__instance.m_maxPlaceDistance = Configuration.Range;
		}
	}

	[HarmonyPatch(typeof(Player), "UpdateWearNTearHover")]
	[HarmonyPostfix]
	private static void UnlockRepairDistancePostfix(Player __instance, float __state)
	{
		__instance.m_maxPlaceDistance = __state;
	}

	[HarmonyPatch(typeof(WearNTear), "Repair")]
	[HarmonyPrefix]
	private static bool AdvancedRepair(WearNTear __instance, ref bool __result)
	{
		if (!Configuration.Enabled || !Object.op_Implicit((Object)(object)__instance.m_nview))
		{
			return true;
		}
		ZDO zDO = __instance.m_nview.GetZDO();
		bool num = Configuration.OverwriteHealth > 0f || Configuration.Invulnerability != "Off";
		bool flag = zDO.GetFloat(Hashes.MaxHealth, 0f) != 0f;
		if (!num && !flag)
		{
			return true;
		}
		__result = RepairStructure(__instance.m_nview);
		return false;
	}
}
