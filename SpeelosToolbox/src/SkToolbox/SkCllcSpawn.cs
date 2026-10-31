using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using SkToolbox.Utility;
using UnityEngine;

namespace SkToolbox;

// Lets the Spawn menu work with Creature Level & Loot Control (CLLC) without linking to it: everything goes through
// reflection, so the toolbox runs the same without CLLC and across its versions.
//
// What CLLC changes for a spawned creature: stars go up to its own "Maximum stars" setting rather than the vanilla 2, it rolls
// a random colored star effect and an elemental infusion on every level change, and bosses can roll an affix. The Spawn menu
// uses its level range and lets you pick the effect, infusion and boss affix instead of leaving them to chance.
internal static class SkCllcSpawn
{
	internal const string Guid = "org.bepinex.plugins.creaturelevelcontrol";

	internal const string Random = "Random";

	private static bool resolved;

	private static bool usable;

	private static Type effectType;

	private static Type infusionType;

	private static Type affixType;

	private static MethodInfo setEffect;

	private static MethodInfo setInfusion;

	private static MethodInfo setAffix;

	internal static bool Present
	{
		get
		{
			try
			{
				return Chainloader.PluginInfos != null && Chainloader.PluginInfos.ContainsKey(Guid);
			}
			catch (Exception)
			{
				return false;
			}
		}
	}

	private static bool Resolve()
	{
		if (resolved)
		{
			return usable;
		}
		resolved = true;
		try
		{
			if (!Present || Chainloader.PluginInfos[Guid].Instance == null)
			{
				return false;
			}
			Assembly assembly = Chainloader.PluginInfos[Guid].Instance.GetType().Assembly;
			Type api = assembly.GetType("CreatureLevelControl.API");
			effectType = assembly.GetType("CreatureLevelControl.CreatureExtraEffect");
			infusionType = assembly.GetType("CreatureLevelControl.CreatureInfusion");
			affixType = assembly.GetType("CreatureLevelControl.BossAffix");
			if (api == null || effectType == null || infusionType == null || affixType == null)
			{
				return false;
			}
			setEffect = api.GetMethod("SetExtraEffectCreature", new Type[2] { typeof(Character), effectType });
			setInfusion = api.GetMethod("SetInfusionCreature", new Type[2] { typeof(Character), infusionType });
			setAffix = api.GetMethod("SetAffixBoss", new Type[2] { typeof(Character), affixType });
			usable = setEffect != null && setInfusion != null && setAffix != null;
		}
		catch (Exception ex)
		{
			SkUtilities.Logz(new string[2] { "CLLC", "ERROR" }, new string[1] { ex.Message }, (LogType)2);
			usable = false;
		}
		return usable;
	}

	internal static bool Available => Resolve();

	// The highest level CLLC will give creatures: its "Maximum stars" setting plus the base level. Falls back to six (five stars).
	internal static int MaxLevel()
	{
		try
		{
			if (Present && Chainloader.PluginInfos[Guid].Instance != null)
			{
				ConfigFile config = Chainloader.PluginInfos[Guid].Instance.Config;
				foreach (ConfigDefinition key in config.Keys)
				{
					if (string.Equals(key.Key, "Maximum stars", StringComparison.OrdinalIgnoreCase))
					{
						return Math.Max(3, Convert.ToInt32(config[key].BoxedValue) + 1);
					}
				}
			}
		}
		catch (Exception)
		{
		}
		return 6;
	}

	private static List<string> WithRandom(Type enumType)
	{
		List<string> list = new List<string> { Random };
		if (enumType != null)
		{
			list.AddRange(Enum.GetNames(enumType));
		}
		return list;
	}

	internal static List<string> Effects()
	{
		Resolve();
		return WithRandom(effectType);
	}

	internal static List<string> Infusions()
	{
		Resolve();
		return WithRandom(infusionType);
	}

	internal static List<string> Affixes()
	{
		Resolve();
		return WithRandom(affixType);
	}

	// Sets what the player picked on a creature that has just been spawned and levelled. "Random" leaves CLLC's own roll alone.
	internal static void Apply(Character character, string effect, string infusion, string affix)
	{
		if (character == null || !Resolve())
		{
			return;
		}
		try
		{
			if (character.IsBoss())
			{
				if (!string.IsNullOrEmpty(affix) && affix != Random)
				{
					setAffix.Invoke(null, new object[2]
					{
						character,
						Enum.Parse(affixType, affix, ignoreCase: true)
					});
				}
				return;
			}
			if (!string.IsNullOrEmpty(effect) && effect != Random)
			{
				setEffect.Invoke(null, new object[2]
				{
					character,
					Enum.Parse(effectType, effect, ignoreCase: true)
				});
			}
			if (!string.IsNullOrEmpty(infusion) && infusion != Random)
			{
				setInfusion.Invoke(null, new object[2]
				{
					character,
					Enum.Parse(infusionType, infusion, ignoreCase: true)
				});
			}
		}
		catch (Exception ex)
		{
			SkUtilities.Logz(new string[2] { "CLLC", "APPLY" }, new string[1] { ex.InnerException?.Message ?? ex.Message }, (LogType)2);
		}
	}
}
