using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace PickupRange;

[BepInPlugin(ModGUID, ModName, ModVersion)]
public class PickupRange : BaseUnityPlugin
{
	private const string ModName = "Pickup Range";
	private const string ModVersion = "1.0.0";
	private const string ModGUID = "org.bepinex.plugins.pickuprange";

	private static ConfigEntry<bool> extendedEnabled = null!;
	private static ConfigEntry<float> pickupRange = null!;
	private static ConfigEntry<KeyboardShortcut> toggleKey = null!;

	public void Awake()
	{
		extendedEnabled = Config.Bind("1 - General", "Enabled", true, "If off, the vanilla auto pickup range is used.");
		pickupRange = Config.Bind("1 - General", "Pickup range", 6f,
			new ConfigDescription("Auto pickup range in meters. Vanilla is 2. High values can make item pickup lag on servers with high latency.",
				new AcceptableValueRange<float>(1f, 30f)));
		toggleKey = Config.Bind("1 - General", "Toggle key", new KeyboardShortcut(KeyCode.None), "Key to toggle the extended pickup range while playing. Unset by default.");

		Harmony harmony = new(ModGUID);
		harmony.PatchAll(Assembly.GetExecutingAssembly());
	}

	public void Update()
	{
		if (toggleKey.Value.IsDown() && Player.m_localPlayer is not null)
		{
			extendedEnabled.Value = !extendedEnabled.Value;
			Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, $"Extended pickup range {(extendedEnabled.Value ? "on" : "off")}");
		}
	}

	[HarmonyPatch(typeof(Player), nameof(Player.AutoPickup))]
	private static class ApplyPickupRange
	{
		private static float? vanillaRange;

		[UsedImplicitly]
		private static void Prefix(Player __instance)
		{
			if (__instance != Player.m_localPlayer)
			{
				return;
			}

			// Remember the vanilla value once, so toggling off restores it.
			vanillaRange ??= __instance.m_autoPickupRange;
			__instance.m_autoPickupRange = extendedEnabled.Value ? pickupRange.Value : vanillaRange.Value;
		}
	}
}

