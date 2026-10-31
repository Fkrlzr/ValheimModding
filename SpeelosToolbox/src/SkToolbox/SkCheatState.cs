using HarmonyLib;

namespace SkToolbox;

// Server Devcommands (and some other cheat mods) patch Player.InGodMode, Player.IsDebugFlying, Player.InDebugFlyMode and
// Player.InGhostMode and force them to false unless they consider you allowed to cheat (admin plus devcommands on).
// The toolbox sets Player.m_godMode / m_debugFly / m_ghostMode directly, so with such a mod installed the game never
// saw those states: no damage immunity, no flying, and the toolbox's keep-alive loop kept toggling fly on and off.
// These postfixes run last and report the real state, but only for states the toolbox itself switched on.
internal static class SkCheatState
{
	[HarmonyPatch(typeof(Player), "InGodMode")]
	private static class PatchInGodMode
	{
		[HarmonyPriority(0)]
		private static void Postfix(Player __instance, ref bool __result)
		{
			if (!__result && SkCommandProcessor.godEnabled && __instance.m_godMode && __instance == Player.m_localPlayer)
			{
				__result = true;
			}
		}
	}

	[HarmonyPatch(typeof(Player), "IsDebugFlying")]
	private static class PatchIsDebugFlying
	{
		[HarmonyPriority(0)]
		private static void Postfix(Player __instance, ref bool __result)
		{
			if (!__result && SkCommandProcessor.flyEnabled && __instance.m_debugFly && __instance == Player.m_localPlayer)
			{
				__result = true;
			}
		}
	}

	[HarmonyPatch(typeof(Player), "InDebugFlyMode")]
	private static class PatchInDebugFlyMode
	{
		[HarmonyPriority(0)]
		private static void Postfix(Player __instance, ref bool __result)
		{
			if (!__result && SkCommandProcessor.flyEnabled && __instance.m_debugFly && __instance == Player.m_localPlayer)
			{
				__result = true;
			}
		}
	}

	// The postfixes above did not stick on a server where Server Devcommands has no rule for you: the log showed the toolbox
	// state on (flyEnabled, m_debugFly and the network flag all true) while IsDebugFlying() still read false. So the answer is
	// also fixed at its source, in the permission lookup Server Devcommands itself makes, which is the same hook the Infinity
	// Hammer access option uses and which is known to take effect. Only for a state the toolbox switched on.
	[HarmonyPatch]
	private static class PatchFeaturePermission
	{
		private const string Section = "serverdevcommands";

		private static readonly System.Type managerType = AccessTools.TypeByName("ServerDevcommands.PermissionManager");

		private static readonly int FlyHash = "fly".GetStableHashCode();

		private static readonly int GodHash = "god".GetStableHashCode();

		private static readonly int GhostHash = "ghost".GetStableHashCode();

		private static bool Prepare()
		{
			return managerType != null && AccessTools.Method(managerType, "IsFeatureEnabledByHash", (System.Type[])null, (System.Type[])null) != null;
		}

		private static System.Reflection.MethodBase TargetMethod()
		{
			return AccessTools.Method(managerType, "IsFeatureEnabledByHash", (System.Type[])null, (System.Type[])null);
		}

		private static void Postfix(string section, int featureHash, ref bool __result)
		{
			if (__result || !string.Equals(section, Section, System.StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			Player localPlayer = Player.m_localPlayer;
			if (localPlayer == null)
			{
				return;
			}
			if ((featureHash == FlyHash && SkCommandProcessor.flyEnabled && localPlayer.m_debugFly) || (featureHash == GodHash && SkCommandProcessor.godEnabled && localPlayer.m_godMode) || (featureHash == GhostHash && SkCommandProcessor.ghostEnabled && localPlayer.m_ghostMode))
			{
				__result = true;
			}
		}
	}

	[HarmonyPatch(typeof(Player), "InGhostMode")]
	private static class PatchInGhostMode
	{
		[HarmonyPriority(0)]
		private static void Postfix(Player __instance, ref bool __result)
		{
			if (!__result && SkCommandProcessor.ghostEnabled && __instance.m_ghostMode && __instance == Player.m_localPlayer)
			{
				__result = true;
			}
		}
	}
}
