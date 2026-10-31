using System;
using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch]
public class GodFeatures
{
	private static bool IsAttacking;

	[HarmonyPatch(typeof(Player), "InGodMode")]
	[HarmonyPostfix]
	private static bool InGodModePostfix(bool result, Player __instance)
	{
		if (!((Object)(object)__instance == (Object)(object)Player.m_localPlayer))
		{
			return result;
		}
		return Settings.IsEnabled(PermissionHash.God, result);
	}

	[HarmonyPatch(typeof(Player), "IsDodgeInvincible")]
	[HarmonyPostfix]
	private static void IsDodgeInvinciblePostfix(Player __instance, ref bool __result)
	{
		bool flag = Settings.GodModeAlwaysDodge && ((Character)__instance).InGodMode();
		__result |= flag;
	}

	[HarmonyPatch(typeof(Character), "ApplyDamage")]
	[HarmonyPostfix]
	private static void ApplyDamagePostfix(Character __instance, HitData hit)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		if (Settings.GodModeAlwaysParry && !((Object)(object)__instance != (Object)(object)Player.m_localPlayer) && __instance.InGodMode())
		{
			Character attacker = hit.GetAttacker();
			if (Object.op_Implicit((Object)(object)attacker) && !((Object)(object)attacker == (Object)(object)Player.m_localPlayer) && ((Humanoid)Player.m_localPlayer).m_blockTimer < 0f)
			{
				((Humanoid)Player.m_localPlayer).m_perfectBlockEffect.Create(hit.m_point, Quaternion.identity, (Transform)null, 1f, -1, default(ZDOID));
				attacker.Stagger(-hit.m_dir);
			}
		}
	}

	[HarmonyPatch(typeof(Player), "EdgeOfWorldKill")]
	[HarmonyPrefix]
	private static bool EdgeOfWorldKillPrefix(Player __instance)
	{
		if (Settings.GodModeNoEdgeOfWorld)
		{
			return !((Character)__instance).InGodMode();
		}
		return true;
	}

	[HarmonyPatch(typeof(Player), "HaveEitr")]
	[HarmonyPostfix]
	private static void HaveEitrPostfix(Player __instance, ref bool __result)
	{
		bool flag = Settings.GodModeNoEitr && ((Character)__instance).InGodMode();
		__result |= flag;
	}

	[HarmonyPatch(typeof(Attack), "Start")]
	[HarmonyPrefix]
	private static void AttackStartPrefix(Humanoid character)
	{
		if ((Object)(object)character == (Object)(object)Player.m_localPlayer)
		{
			IsAttacking = true;
		}
	}

	[HarmonyPatch(typeof(Attack), "Start")]
	[HarmonyPostfix]
	private static void AttackStartPostfix(Humanoid character)
	{
		if ((Object)(object)character == (Object)(object)Player.m_localPlayer)
		{
			IsAttacking = false;
		}
	}

	[HarmonyPatch(typeof(Player), "GetMaxEitr")]
	[HarmonyPostfix]
	private static void GetMaxEitrPostfix(Player __instance, ref float __result)
	{
		if (Settings.GodModeNoEitr && ((Character)__instance).InGodMode() && IsAttacking)
		{
			__result = Math.Max(__result, 0.1f);
		}
	}

	[HarmonyPatch(typeof(Player), "UseEitr")]
	[HarmonyPrefix]
	private static bool UseEitrPrefix(Player __instance)
	{
		return !Settings.GodModeNoEitr || !((Character)__instance).InGodMode();
	}

	[HarmonyPatch(typeof(Character), "ApplyPushback", new Type[]
	{
		typeof(Vector3),
		typeof(float)
	})]
	[HarmonyPrefix]
	private static bool ApplyPushbackPrefix(Character __instance)
	{
		return !Settings.GodModeNoKnockback || !__instance.InGodMode() || !__instance.IsPlayer();
	}

	[HarmonyPatch(typeof(ParticleMist), "Update")]
	[HarmonyPrefix]
	private static bool ParticleMistUpdatePrefix()
	{
		Player localPlayer = Player.m_localPlayer;
		return !Settings.GodModeNoMist || !Object.op_Implicit((Object)(object)localPlayer) || !((Character)localPlayer).InGodMode();
	}

	[HarmonyPatch(typeof(Character), "AddStaggerDamage")]
	[HarmonyPrefix]
	private static bool AddStaggerDamagePrefix(Character __instance)
	{
		return !Settings.GodModeNoStagger || !__instance.InGodMode() || !__instance.IsPlayer();
	}

	[HarmonyPatch(typeof(Player), "HaveStamina")]
	[HarmonyPostfix]
	private static void HaveStaminaPostfix(Player __instance, ref bool __result)
	{
		bool flag = Settings.GodModeNoStamina && ((Character)__instance).InGodMode();
		__result |= flag;
	}

	[HarmonyPatch(typeof(Player), "UseStamina")]
	[HarmonyPrefix]
	private static bool UseStaminaPrefix(Player __instance)
	{
		return !Settings.GodModeNoStamina || !((Character)__instance).InGodMode();
	}

	[HarmonyPatch(typeof(Inventory), "RemoveOneItem")]
	[HarmonyPrefix]
	private static bool RemoveOneItemPrefix(Inventory __instance, ref bool __result)
	{
		if (((Humanoid)(Player.m_localPlayer?)).m_inventory != __instance)
		{
			return true;
		}
		int num;
		if (Settings.GodModeNoUsage)
		{
			num = (((Character)Player.m_localPlayer).InGodMode() ? 1 : 0);
			if (num != 0)
			{
				__result = true;
			}
		}
		else
		{
			num = 0;
		}
		return num == 0;
	}

	[HarmonyPatch(typeof(Inventory), "RemoveItem", new Type[]
	{
		typeof(ItemData),
		typeof(int)
	})]
	[HarmonyPrefix]
	private static bool RemoveItemByDataPrefix(Inventory __instance, int amount, ref bool __result)
	{
		if (amount > 1)
		{
			return true;
		}
		if (((Humanoid)(Player.m_localPlayer?)).m_inventory != __instance)
		{
			return true;
		}
		int num;
		if (Settings.GodModeNoUsage)
		{
			num = (((Character)Player.m_localPlayer).InGodMode() ? 1 : 0);
			if (num != 0)
			{
				__result = true;
			}
		}
		else
		{
			num = 0;
		}
		return num == 0;
	}

	[HarmonyPatch(typeof(Inventory), "RemoveItem", new Type[]
	{
		typeof(string),
		typeof(int),
		typeof(int),
		typeof(bool)
	})]
	[HarmonyPrefix]
	private static bool RemoveItemByNamePrefix(Inventory __instance, int amount)
	{
		if (amount > 1)
		{
			return true;
		}
		if (((Humanoid)(Player.m_localPlayer?)).m_inventory != __instance)
		{
			return true;
		}
		return !Settings.GodModeNoUsage || !((Character)Player.m_localPlayer).InGodMode();
	}

	[HarmonyPatch(typeof(Player), "GetMaxCarryWeight")]
	[HarmonyPostfix]
	private static void GetMaxCarryWeightPostfix(Player __instance, ref float __result)
	{
		if (Settings.GodModeNoWeightLimit && (Object)(object)__instance == (Object)(object)Player.m_localPlayer && ((Character)__instance).InGodMode())
		{
			__result = 1E+10f;
		}
	}

	[HarmonyPatch(typeof(InventoryGui), "UpdateInventoryWeight")]
	[HarmonyPrefix]
	private static bool UpdateInventoryWeightPrefix(Player player, InventoryGui __instance)
	{
		int num;
		if (Settings.GodModeNoWeightLimit && (Object)(object)player == (Object)(object)Player.m_localPlayer)
		{
			num = (((Character)player).InGodMode() ? 1 : 0);
			if (num != 0)
			{
				int num2 = Mathf.CeilToInt(((Humanoid)player).GetInventory().GetTotalWeight());
				__instance.m_weight.text = $"{num2}/-";
			}
		}
		else
		{
			num = 0;
		}
		return num == 0;
	}
}
