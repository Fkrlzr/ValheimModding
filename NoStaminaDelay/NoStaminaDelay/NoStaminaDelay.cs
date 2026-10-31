using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace NoStaminaDelay;

[BepInPlugin(ModGUID, ModName, ModVersion)]
public class NoStaminaDelay : BaseUnityPlugin
{
	private const string ModName = "No Stamina Delay";
	private const string ModVersion = "1.0.1";
	private const string ModGUID = "org.bepinex.plugins.nostaminadelay";

	private static ConfigEntry<bool> modEnabled = null!;
	private static ConfigEntry<float> regenDelay = null!;
	private static ConfigEntry<KeyboardShortcut> toggleKey = null!;

	public void Awake()
	{
		modEnabled = Config.Bind("1 - General", "Enabled", true, "If off, the vanilla stamina regeneration delay is used.");
		regenDelay = Config.Bind("1 - General", "Regen delay", 0.1f,
			new ConfigDescription("Seconds without stamina use before regeneration starts. Vanilla is 1. Keep this above 0 so stamina does not regenerate while sprinting, jumping or building.",
				new AcceptableValueRange<float>(0.05f, 1f)));
		toggleKey = Config.Bind("1 - General", "Toggle key", new KeyboardShortcut(KeyCode.None), "Key to toggle the removed stamina regeneration delay while playing. Unset by default.");

		Harmony harmony = new(ModGUID);
		harmony.PatchAll(Assembly.GetExecutingAssembly());
	}

	public void Update()
	{
		if (toggleKey.Value.IsDown() && Player.m_localPlayer is not null)
		{
			modEnabled.Value = !modEnabled.Value;
			Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, $"No stamina delay {(modEnabled.Value ? "on" : "off")}");
		}
	}

	// Vanilla sets m_staminaRegenTimer to m_staminaRegenDelay (1s) every time stamina is used. Continuous
	// drains (sprinting, swimming, drawing a bow) use stamina every tick, so the timer must never reach zero
	// while they are active. Capping it at a short delay keeps regen blocked during use and starts it
	// almost immediately afterwards.
	[HarmonyPatch(typeof(Player), nameof(Player.RPC_UseStamina))]
	private static class ShortenStaminaDelay
	{
		[UsedImplicitly]
		private static void Postfix(Player __instance, float v)
		{
			if (modEnabled.Value && v != 0f && __instance == Player.m_localPlayer)
			{
				__instance.m_staminaRegenTimer = Mathf.Min(__instance.m_staminaRegenTimer, regenDelay.Value);
			}
		}
	}
}
