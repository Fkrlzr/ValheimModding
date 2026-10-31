using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace UnlimitedCarryWeight;

[BepInPlugin(ModGUID, ModName, ModVersion)]
public class UnlimitedCarryWeight : BaseUnityPlugin
{
	private const string ModName = "Unlimited Carry Weight";
	private const string ModVersion = "1.1.0";
	private const string ModGUID = "org.bepinex.plugins.unlimitedcarryweight";

	private static ConfigEntry<bool> modEnabled = null!;
	private static ConfigEntry<float> maxCarryWeight = null!;
	private static ConfigEntry<KeyboardShortcut> toggleKey = null!;

	public void Awake()
	{
		modEnabled = Config.Bind("1 - General", "Enabled", true, "If off, the vanilla carry weight limit is used.");
		maxCarryWeight = Config.Bind("1 - General", "Max carry weight", 5000f,
			new ConfigDescription("Maximum carry weight while the mod is enabled. Vanilla is 300 plus bonuses. A value below your vanilla limit never lowers it.",
				new AcceptableValueRange<float>(0f, CarryWeightLogic.UnlimitedLimit)));
		toggleKey = Config.Bind("1 - General", "Toggle key", new KeyboardShortcut(KeyCode.None), "Key to toggle unlimited carry weight while playing. Unset by default.");

		Harmony harmony = new(ModGUID);
		harmony.PatchAll(Assembly.GetExecutingAssembly());
	}

	public void Update()
	{
		if (toggleKey.Value.IsDown() && Player.m_localPlayer is not null)
		{
			modEnabled.Value = !modEnabled.Value;
			Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, $"Unlimited carry weight {(modEnabled.Value ? "on" : "off")}");
		}
	}

	[HarmonyPatch(typeof(Player), nameof(Player.GetMaxCarryWeight))]
	private static class UnlimitedWeight
	{
		[UsedImplicitly]
		private static void Postfix(Player __instance, ref float __result)
		{
			__result = CarryWeightLogic.ApplyLimit(modEnabled.Value, __instance == Player.m_localPlayer, __result, maxCarryWeight.Value);
		}
	}
}
