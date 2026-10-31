using System;
using System.Runtime.CompilerServices;
using BepInEx.Bootstrap;
using UnityEngine;

namespace MobBattle;

// Compatibility with Creature Level & Loot Control (CLLC). CLLC is an optional dependency: nothing here touches its
// types unless the plugin is actually loaded, so the mod works the same without it.
//
// What CLLC does that matters for a staged fight:
//  - Every level change rolls a random colored star effect and elemental infusion. That would decide fights by luck
//    instead of by the creatures that were picked, so battle creatures get those removed (unless the config says to keep them).
//  - The "Splitting" effect spawns children on death that are aimed at the local player, which would break the
//    spectator rule and leave untracked creatures behind. It is always removed.
//  - Boss affixes (Twin, Summoner, ...) spawn extra bosses. They are always removed.
internal static class ClllcCompat
{
	internal const string Guid = "org.bepinex.plugins.creaturelevelcontrol";

	internal static bool Present => Chainloader.PluginInfos.ContainsKey(Guid);

	// Call right after the creature's level has been set, so CLLC's own reaction to the level change is already done.
	internal static void Prepare(Character character)
	{
		if (!Present)
		{
			return;
		}

		try
		{
			Apply(character, MobBattle.keepClllcEffects.Value);
		}
		catch (Exception e)
		{
			// Never let a CLLC problem stop a battle from starting.
			Debug.LogWarning($"[MobBattle] Could not adjust CLLC effects: {e.Message}");
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Apply(Character character, bool keepEffects)
	{
		if (character.IsBoss())
		{
			if (CreatureLevelControl.API.GetAffixBoss(character) != CreatureLevelControl.BossAffix.None)
			{
				CreatureLevelControl.API.SetAffixBoss(character, CreatureLevelControl.BossAffix.None);
			}

			return;
		}

		CreatureLevelControl.CreatureExtraEffect effect = CreatureLevelControl.API.GetExtraEffectCreature(character);
		if (!keepEffects || effect == CreatureLevelControl.CreatureExtraEffect.Splitting)
		{
			CreatureLevelControl.API.SetExtraEffectCreature(character, CreatureLevelControl.CreatureExtraEffect.None);
		}

		if (!keepEffects)
		{
			CreatureLevelControl.API.SetInfusionCreature(character, CreatureLevelControl.CreatureInfusion.None);
			// CLLC itself removes the flame visuals this way when a creature ends up without an infusion.
			Transform? flames = character.transform.Find("infusion flames");
			if (flames != null)
			{
				UnityEngine.Object.Destroy(flames.gameObject);
			}
		}
	}
}
