using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace AutoParry;

[BepInPlugin(ModGUID, ModName, ModVersion)]
public class AutoParry : BaseUnityPlugin
{
	private const string ModName = "Auto Parry";
	private const string ModVersion = "1.0.0";
	private const string ModGUID = "org.bepinex.plugins.autoparry";

	private static ConfigEntry<bool> autoParryEnabled = null!;
	private static ConfigEntry<bool> parryRanged = null!;
	private static ConfigEntry<KeyboardShortcut> toggleKey = null!;

	public void Awake()
	{
		autoParryEnabled = Config.Bind("1 - General", "Enabled", true, "If on, blockable hits against you are parried automatically while you have a shield or a blocking weapon equipped.");
		parryRanged = Config.Bind("1 - General", "Parry ranged attacks", true, "If off, only melee hits are parried automatically.");
		toggleKey = Config.Bind("1 - General", "Toggle key", new KeyboardShortcut(KeyCode.None), "Key to toggle auto parry while playing. Unset by default.");

		Harmony harmony = new(ModGUID);
		harmony.PatchAll(Assembly.GetExecutingAssembly());
	}

	public void Update()
	{
		if (toggleKey.Value.IsDown() && Player.m_localPlayer is not null)
		{
			autoParryEnabled.Value = !autoParryEnabled.Value;
			Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, $"Auto parry {(autoParryEnabled.Value ? "on" : "off")}");
		}
	}

	// Damage is resolved on the owner's client, so this works on servers without the mod.
	[HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
	private static class ParryOnHit
	{
		private struct State
		{
			public bool Forced;
			public bool WasBlocking;
			public float BlockTimer;
		}

		[UsedImplicitly]
		private static void Prefix(Character __instance, HitData hit, out State __state)
		{
			__state = default;

			if (!autoParryEnabled.Value || __instance != Player.m_localPlayer || __instance is not Player player)
			{
				return;
			}

			if (!hit.m_blockable || (hit.m_ranged && !parryRanged.Value) || player.IsDead() || player.GetCurrentBlocker() is null)
			{
				return;
			}

			// Already holding block: leave vanilla behaviour (manual parry timing) alone.
			if (player.m_blocking)
			{
				return;
			}

			__state.Forced = true;
			__state.WasBlocking = player.m_blocking;
			__state.BlockTimer = player.m_blockTimer;

			// IsBlocking() still rejects attacking, dodging, staggering and encumbered states,
			// and BlockAttack() still requires the hit to come from the front.
			player.m_blocking = true;
			player.m_blockTimer = 0f;
		}

		[UsedImplicitly]
		private static void Postfix(Character __instance, State __state)
		{
			if (__state.Forced && __instance is Player player)
			{
				player.m_blocking = __state.WasBlocking;
				player.m_blockTimer = __state.BlockTimer;
			}
		}
	}
}
