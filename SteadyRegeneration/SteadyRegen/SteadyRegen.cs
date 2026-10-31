using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;

namespace SteadyRegen;

[BepInPlugin(ModGUID, ModName, ModVersion)]
[BepInIncompatibility("org.bepinex.plugins.valheim_plus")]
public class SteadyRegen : BaseUnityPlugin
{
	private const string ModName = "Steady Regeneration";
	private const string ModVersion = "1.0.6";
	private const string ModGUID = "org.bepinex.plugins.steadyregeneration";

	private static ConfigEntry<float> regenMultiplier = null!;
	private static ConfigEntry<float> staminaRegenMultiplier = null!;
	private static ConfigEntry<float> staminaUsageMultiplier = null!;

	// Vanilla heals once per 10s; this mod spreads that over 100 ticks of 0.1s (hence 0.01).
	// The multiplier scales the total regen rate on top of that.
	public static float GetHealFactor() => 0.01f * regenMultiplier.Value;

	public void Awake()
	{
		regenMultiplier = Config.Bind("1 - Regeneration", "Regeneration multiplier", 1f,
			new ConfigDescription("Multiplier for the food based health regeneration rate. 1 is the vanilla rate, 2 is double, 0.5 is half, 0 disables regeneration.",
				new AcceptableValueRange<float>(0f, 100f)));

		staminaRegenMultiplier = Config.Bind("2 - Stamina", "Stamina regeneration multiplier", 1f,
			new ConfigDescription("Multiplier for the stamina regeneration rate. 1 is the vanilla rate, 2 is double, 0.5 is half, 0 disables stamina regeneration.",
				new AcceptableValueRange<float>(0f, 100f)));

		staminaUsageMultiplier = Config.Bind("2 - Stamina", "Stamina usage multiplier", 1f,
			new ConfigDescription("Multiplier for how much stamina is used by sprinting, jumping, attacking, blocking, building and everything else. 1 is vanilla, 0.5 uses half as much, 2 uses double, 0 makes all actions free.",
				new AcceptableValueRange<float>(0f, 100f)));

		Assembly assembly = Assembly.GetExecutingAssembly();
		Harmony harmony = new(ModGUID);
		harmony.PatchAll(assembly);
	}

	[HarmonyPatch(typeof(Player), nameof(Player.UpdateFood))]
	private static class MakeRegenSmooth
	{
		private static readonly MethodInfo Heal = AccessTools.DeclaredMethod(typeof(Character), nameof(Player.Heal));
		private static readonly FieldInfo RegenTimer = AccessTools.DeclaredField(typeof(Player), nameof(Player.m_foodRegenTimer));

		[UsedImplicitly]
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			List<CodeInstruction> instructionList = instructions.ToList();
			for (int i = 0; i < instructionList.Count; ++i)
			{
				if (instructionList[i].opcode == OpCodes.Ldc_R4 && instructionList[i].OperandIs(10f))
				{
					yield return new CodeInstruction(OpCodes.Ldc_R4, 0.1f);
				}
				else if (instructionList[i].opcode == OpCodes.Ldc_R4 && instructionList[i].OperandIs(0f) && instructionList[i + 1].opcode == OpCodes.Stfld && instructionList[i + 1].OperandIs(RegenTimer))
				{
					yield return new CodeInstruction(OpCodes.Ldarg_0);
					yield return new CodeInstruction(OpCodes.Ldfld, RegenTimer);
					yield return new CodeInstruction(OpCodes.Ldc_R4, 0.1f);
					yield return new CodeInstruction(OpCodes.Sub);
				}
				else if (instructionList[i].opcode == OpCodes.Ldc_I4_1 && instructionList[i + 1].opcode == OpCodes.Call && instructionList[i + 1].OperandIs(Heal))
				{
					yield return new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(SteadyRegen), nameof(GetHealFactor)));
					yield return new CodeInstruction(OpCodes.Mul);
					yield return new CodeInstruction(OpCodes.Ldc_I4_0);
				}
				else
				{
					yield return instructionList[i];
				}
			}
		}
	}

	// Player.UpdateStats asks the status effect manager for a stamina regen multiplier every tick,
	// so scaling it here scales the whole stamina regeneration rate without touching the delay logic.
	[HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyStaminaRegen))]
	private static class ScaleStaminaRegen
	{
		[UsedImplicitly]
		private static void Postfix(SEMan __instance, ref float staminaMultiplier)
		{
			if (__instance.m_character == Player.m_localPlayer)
			{
				staminaMultiplier *= staminaRegenMultiplier.Value;
			}
		}
	}

	// Every stamina cost (run, jump, dodge, attack, block, build, swim, ...) goes through Player.UseStamina.
	// Only positive values are scaled, so stamina that is given back is left alone.
	[HarmonyPatch(typeof(Player), nameof(Player.UseStamina))]
	private static class ScaleStaminaUsage
	{
		[UsedImplicitly]
		private static void Prefix(Player __instance, ref float v)
		{
			if (v > 0f && __instance == Player.m_localPlayer)
			{
				v *= staminaUsageMultiplier.Value;
			}
		}
	}

	[HarmonyPatch(typeof(GuiBar), nameof(GuiBar.SetValue))]
	private static class DisplaySmoothRegenOnHealthbar
	{
		private static readonly FieldInfo BarValue = AccessTools.DeclaredField(typeof(GuiBar), nameof(GuiBar.m_value));
		private static readonly FieldInfo SmoothFill = AccessTools.DeclaredField(typeof(GuiBar), nameof(GuiBar.m_smoothFill));

		[UsedImplicitly]
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			List<CodeInstruction> instructionList = instructions.ToList();
			for (int i = 0; i < instructionList.Count; ++i)
			{
				if (instructionList[i].opcode == OpCodes.Ldfld && instructionList[i].OperandIs(SmoothFill))
				{
					for (; i > 0; --i)
					{
						if (instructionList[i].opcode == OpCodes.Ldfld && instructionList[i].OperandIs(BarValue))
						{
							instructionList.Insert(i + 1, new CodeInstruction(OpCodes.Add));
							instructionList.Insert(i + 1, new CodeInstruction(OpCodes.Ldc_R4, 0.5f));
							break;
						}
					}

					break;
				}
			}

			return instructionList;
		}
	}

	[HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetTooltip), typeof(ItemDrop.ItemData), typeof(int), typeof(bool), typeof(float), typeof(int), typeof(bool))]
	private static class ChangeFoodTooltip
	{
		[UsedImplicitly]
		private static void Postfix(ItemDrop.ItemData __instance, ref string __result)
		{
			__result = __result.Replace("hp/tick", "hp/10s");
		}
	}
}