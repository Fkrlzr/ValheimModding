using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BepInEx.Bootstrap;
using UnityEngine;

namespace MobBattle;

// What one side of a battle gets from Creature Level & Loot Control. Each value is "Default", "Random", "None" or the name of
// one of CLLC's effects, infusions or boss affixes.
//   Default: follow the "Keep CLLC effects" setting (effects and infusions removed unless it is on, boss affixes removed).
//   Random:  keep whatever CLLC rolled.
//   None:    remove it.
//   a name:  force that one.
public sealed class ClllcChoice
{
	public string Effect = "Default";

	public string Infusion = "Default";

	public string Affix = "Default";
}

// Compatibility with Creature Level & Loot Control (CLLC). CLLC is an optional dependency: nothing here touches its
// types unless the plugin is actually loaded, so the mod works the same without it.
//
// What CLLC does that matters for a staged fight:
//  - Every level change rolls a random colored star effect and elemental infusion. That would decide fights by luck
//    instead of by the creatures that were picked, so by default battle creatures get those removed (unless the config says
//    to keep them), and each side can pick its own effect and infusion instead.
//  - The "Splitting" effect spawns children on death that are aimed at the local player, which would break the
//    spectator rule and leave untracked creatures behind. It is always removed and cannot be picked.
//  - Boss affixes (Twin, Summoner, ...) spawn extra bosses. By default they are removed; a side can pick one on purpose.
internal static class ClllcCompat
{
	internal const string Guid = "org.bepinex.plugins.creaturelevelcontrol";

	internal static bool Present => Chainloader.PluginInfos.ContainsKey(Guid);

	// The values a cycle button steps through for each kind.
	internal static List<string> EffectOptions() => Present ? EffectNames() : new List<string> { "Default" };

	internal static List<string> InfusionOptions() => Present ? InfusionNames() : new List<string> { "Default" };

	internal static List<string> AffixOptions() => Present ? AffixNames() : new List<string> { "Default" };

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static List<string> EffectNames()
	{
		List<string> list = new List<string> { "Default", "Random" };
		list.AddRange(Enum.GetNames(typeof(CreatureLevelControl.CreatureExtraEffect)).Where(n => n != nameof(CreatureLevelControl.CreatureExtraEffect.Splitting)));
		return list;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static List<string> InfusionNames()
	{
		List<string> list = new List<string> { "Default", "Random" };
		list.AddRange(Enum.GetNames(typeof(CreatureLevelControl.CreatureInfusion)));
		return list;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static List<string> AffixNames()
	{
		List<string> list = new List<string> { "Default", "Random" };
		list.AddRange(Enum.GetNames(typeof(CreatureLevelControl.BossAffix)));
		return list;
	}

	// Call right after the creature's level has been set, so CLLC's own reaction to the level change is already done.
	internal static void Prepare(Character character, ClllcChoice choice)
	{
		if (!Present)
		{
			return;
		}

		try
		{
			Apply(character, MobBattle.keepClllcEffects.Value, choice);
		}
		catch (Exception e)
		{
			// Never let a CLLC problem stop a battle from starting.
			Debug.LogWarning($"[MobBattle] Could not adjust CLLC effects: {e.Message}");
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Apply(Character character, bool keepEffects, ClllcChoice choice)
	{
		if (character.IsBoss())
		{
			ApplyAffix(character, choice.Affix);
			return;
		}

		ApplyEffect(character, keepEffects, choice.Effect);
		ApplyInfusion(character, keepEffects, choice.Infusion);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ApplyAffix(Character character, string choice)
	{
		CreatureLevelControl.BossAffix current = CreatureLevelControl.API.GetAffixBoss(character);
		switch (choice)
		{
			case "Random":
				return;
			case "Default":
			case "None":
				if (current != CreatureLevelControl.BossAffix.None)
				{
					CreatureLevelControl.API.SetAffixBoss(character, CreatureLevelControl.BossAffix.None);
				}

				return;
			default:
				if (Enum.TryParse(choice, true, out CreatureLevelControl.BossAffix affix) && current != affix)
				{
					CreatureLevelControl.API.SetAffixBoss(character, affix);
				}

				return;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ApplyEffect(Character character, bool keepEffects, string choice)
	{
		CreatureLevelControl.CreatureExtraEffect current = CreatureLevelControl.API.GetExtraEffectCreature(character);
		CreatureLevelControl.CreatureExtraEffect wanted = current;
		switch (choice)
		{
			case "Default":
				if (!keepEffects)
				{
					wanted = CreatureLevelControl.CreatureExtraEffect.None;
				}

				break;
			case "Random":
				break;
			case "None":
				wanted = CreatureLevelControl.CreatureExtraEffect.None;
				break;
			default:
				if (Enum.TryParse(choice, true, out CreatureLevelControl.CreatureExtraEffect effect))
				{
					wanted = effect;
				}

				break;
		}

		// Splitting children would attack the spectator and stay behind after the fight, so it never stays.
		if (wanted == CreatureLevelControl.CreatureExtraEffect.Splitting)
		{
			wanted = CreatureLevelControl.CreatureExtraEffect.None;
		}

		if (wanted != current || choice == "Default" && !keepEffects)
		{
			CreatureLevelControl.API.SetExtraEffectCreature(character, wanted);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ApplyInfusion(Character character, bool keepEffects, string choice)
	{
		CreatureLevelControl.CreatureInfusion? wanted = null;
		switch (choice)
		{
			case "Default":
				if (!keepEffects)
				{
					wanted = CreatureLevelControl.CreatureInfusion.None;
				}

				break;
			case "Random":
				break;
			case "None":
				wanted = CreatureLevelControl.CreatureInfusion.None;
				break;
			default:
				if (Enum.TryParse(choice, true, out CreatureLevelControl.CreatureInfusion infusion))
				{
					wanted = infusion;
				}

				break;
		}

		if (!wanted.HasValue)
		{
			return;
		}

		CreatureLevelControl.API.SetInfusionCreature(character, wanted.Value);
		if (wanted.Value == CreatureLevelControl.CreatureInfusion.None)
		{
			// CLLC itself removes the flame visuals this way when a creature ends up without an infusion.
			Transform? flames = character.transform.Find("infusion flames");
			if (flames != null)
			{
				UnityEngine.Object.Destroy(flames.gameObject);
			}
		}
	}
}
