using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch]
public class GhostFeatures
{
	public static readonly int HashIgnoreSleep = "DEV_GhostIgnoreSleep".GetHashCode();

	public static readonly int HashGhost = "DEV_Ghost".GetHashCode();

	private static float? PreviousGhostY = null;

	private static bool? PreviousPublicReferencePosition = null;

	public static bool IsGhost(Player player)
	{
		if (!((Character)player).InGhostMode())
		{
			ZNetView nview = ((Character)player).m_nview;
			if (nview == null)
			{
				return false;
			}
			ZDO zDO = nview.GetZDO();
			return ((zDO != null) ? new bool?(zDO.GetBool(HashGhost, false)) : null).GetValueOrDefault();
		}
		return true;
	}

	[HarmonyPatch(typeof(Player), "InGhostMode")]
	[HarmonyPostfix]
	private static bool InGhostModePostfix(bool result, Player __instance)
	{
		if (!((Object)(object)__instance == (Object)(object)Player.m_localPlayer))
		{
			return result;
		}
		return Settings.IsEnabled(PermissionHash.Ghost, result);
	}

	private static List<Player> Players()
	{
		if (!Settings.GhostNoSpawns)
		{
			return Player.s_players;
		}
		return Player.s_players.Where((Player player) => !IsGhost(player)).ToList();
	}

	private static IEnumerable<CodeInstruction> ReplacePlayers(IEnumerable<CodeInstruction> instructions)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		return new CodeMatcher(instructions, (ILGenerator)null).MatchStartForward((CodeMatch[])(object)new CodeMatch[1]
		{
			new CodeMatch((OpCode?)OpCodes.Ldsfld, (object)AccessTools.Field(typeof(Player), "s_players"), (string)null)
		}).SetAndAdvance(OpCodes.Call, Transpilers.EmitDelegate<Func<List<Player>>>((Func<List<Player>>)Players).operand).InstructionEnumeration();
	}

	[HarmonyPatch(typeof(Game), "EverybodyIsTryingToSleep")]
	[HarmonyPostfix]
	private static bool EverybodyIsTryingToSleepPostfix(bool result)
	{
		if (result)
		{
			return result;
		}
		List<ZDO> allCharacterZDOS = ZNet.instance.GetAllCharacterZDOS();
		if (allCharacterZDOS.Count == 0)
		{
			return false;
		}
		if (!allCharacterZDOS.Any((ZDO zdo) => zdo.GetBool(ZDOVars.s_inBed, false)))
		{
			return false;
		}
		return allCharacterZDOS.All((ZDO zdo) => zdo.GetBool(ZDOVars.s_inBed, false) || zdo.GetBool(HashIgnoreSleep, false));
	}

	[HarmonyPatch(typeof(Player), "SetGhostMode")]
	[HarmonyPostfix]
	private static void SetGhostModePostfix(Player __instance)
	{
		if ((Object)(object)__instance != (Object)(object)Player.m_localPlayer)
		{
			return;
		}
		ZNetView nview = ((Character)__instance).m_nview;
		if (nview != null)
		{
			ZDO zDO = nview.GetZDO();
			if (zDO != null)
			{
				zDO.Set(HashGhost, ((Character)__instance).InGhostMode());
			}
		}
		if (!Settings.GhostIgnoreSleep)
		{
			return;
		}
		ZNetView nview2 = ((Character)__instance).m_nview;
		if (nview2 != null)
		{
			ZDO zDO2 = nview2.GetZDO();
			if (zDO2 != null)
			{
				zDO2.Set(HashIgnoreSleep, ((Character)__instance).InGhostMode());
			}
		}
	}

	[HarmonyPatch(typeof(ZDOMan), "SendZDOs")]
	[HarmonyPrefix]
	private static void SendZDOsPrefix()
	{
		if (!Object.op_Implicit((Object)(object)Player.m_localPlayer) || !Object.op_Implicit((Object)(object)((Character)Player.m_localPlayer).m_nview) || !((Character)Player.m_localPlayer).InGhostMode() || !Settings.GhostInvisibility)
		{
			return;
		}
		ZDO zDO = ((Character)Player.m_localPlayer).m_nview.GetZDO();
		if (zDO == null)
		{
			return;
		}
		PreviousGhostY = zDO.m_position.y;
		zDO.m_position.y = 10000f;
		SEMan sEMan = ((Character)Player.m_localPlayer).GetSEMan();
		if (sEMan == null)
		{
			return;
		}
		foreach (StatusEffect statusEffect in sEMan.GetStatusEffects())
		{
			statusEffect.RemoveStartEffects();
		}
	}

	[HarmonyPatch(typeof(ZDOMan), "SendZDOs")]
	[HarmonyPostfix]
	private static void SendZDOsPostfix()
	{
		if (Object.op_Implicit((Object)(object)Player.m_localPlayer) && Object.op_Implicit((Object)(object)((Character)Player.m_localPlayer).m_nview) && PreviousGhostY.HasValue)
		{
			((Character)Player.m_localPlayer).m_nview.GetZDO().m_position.y = PreviousGhostY.Value;
			PreviousGhostY = null;
		}
	}

	[HarmonyPatch(typeof(ZNet), "SendPeriodicData")]
	[HarmonyPrefix]
	private static void SendPeriodicDataPrefix(ZNet __instance)
	{
		if (Object.op_Implicit((Object)(object)Player.m_localPlayer) && Object.op_Implicit((Object)(object)((Character)Player.m_localPlayer).m_nview) && ((Character)Player.m_localPlayer).InGhostMode() && Settings.GhostInvisibility)
		{
			PreviousPublicReferencePosition = __instance.m_publicReferencePosition;
			__instance.m_publicReferencePosition = false;
		}
	}

	[HarmonyPatch(typeof(ZNet), "SendPeriodicData")]
	[HarmonyPostfix]
	private static void SendPeriodicDataPostfix(ZNet __instance)
	{
		if (PreviousPublicReferencePosition.HasValue)
		{
			__instance.m_publicReferencePosition = PreviousPublicReferencePosition.Value;
			PreviousPublicReferencePosition = null;
		}
	}

	[HarmonyPatch(typeof(SpawnSystem), "GetPlayersInZone")]
	[HarmonyPostfix]
	private static void GetPlayersInZonePostfix(List<Player> players)
	{
		if (!Settings.GhostNoSpawns)
		{
			return;
		}
		foreach (Player item in players.Where(IsGhost).ToList())
		{
			players.Remove(item);
		}
	}

	[HarmonyPatch(typeof(Player), "GetClosestPlayer")]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> GetClosestPlayerTranspiler(IEnumerable<CodeInstruction> instructions)
	{
		return ReplacePlayers(instructions);
	}

	[HarmonyPatch(typeof(Player), "IsPlayerInRange", new Type[]
	{
		typeof(Vector3),
		typeof(float)
	})]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> IsPlayerInRangeTranspiler(IEnumerable<CodeInstruction> instructions)
	{
		return ReplacePlayers(instructions);
	}

	[HarmonyPatch(typeof(Player), "IsPlayerInRange", new Type[]
	{
		typeof(Vector3),
		typeof(float),
		typeof(float)
	})]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> IsPlayerInRange2Transpiler(IEnumerable<CodeInstruction> instructions)
	{
		return ReplacePlayers(instructions);
	}
}
