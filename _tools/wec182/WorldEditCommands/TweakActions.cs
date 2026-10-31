using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using ServerDevcommands;

namespace WorldEditCommands;

public static class TweakActions
{
	private const string DEFAULT = "default";

	private static int BiomeToInt(string value)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected I4, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		string[] array = value.Split(',');
		Biome val = (Biome)0;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			if (Enum.TryParse<Biome>(array2[i], ignoreCase: true, out Biome result))
			{
				val |= result;
			}
		}
		return (int)val;
	}

	private static string Print<T>(T? value)
	{
		if (value != null)
		{
			return value.ToString();
		}
		return "default";
	}

	private static bool? Reverse(bool? value)
	{
		if (value.HasValue)
		{
			return !value;
		}
		return null;
	}

	public static string Render(ZNetView view, bool? value)
	{
		value = !Actions.ToggleBool(view, Reverse(value), Hash.NoRender);
		return "¤ render set to " + Print(value) + ".";
	}

	public static string Interact(ZNetView view, bool? value)
	{
		value = !Actions.ToggleBool(view, Reverse(value), Hash.NoInteract);
		return "¤ interact set to " + Print(value) + ".";
	}

	public static string Collision(ZNetView view, bool? value)
	{
		value = !Actions.ToggleBool(view, Reverse(value), Hash.NoCollision);
		return "¤ collision set to " + Print(value) + ".";
	}

	public static string Wear(ZNetView view, string? value)
	{
		Actions.SetInt(view, WearNumber(value), Hash.Wear);
		return "¤ wear set to " + Print(value) + ".";
	}

	private static int WearNumber(string? wear)
	{
		return wear switch
		{
			"broken" => 3, 
			"damaged" => 1, 
			"healthy" => 2, 
			_ => 0, 
		};
	}

	public static string Status(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.Status);
		return "¤ status set to " + Print(value) + ".";
	}

	public static string Event(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.Event);
		return "¤ event set to " + Print(value) + ".";
	}

	public static string Effect(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.Effect);
		return "¤ effect set to " + Print(value) + ".";
	}

	public static string Weather(ZNetView view, int hash, string? value)
	{
		string value2 = value;
		if (!EnvMan.instance.m_environments.Any((EnvSetup env) => env.m_name == value2))
		{
			value2 = value2?.Replace("_", " ");
		}
		Actions.SetString(view, value2, hash);
		return "¤ weather set to " + Print(value2) + ".";
	}

	public static string Water(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.Water);
		return "¤ water set to " + Print(value) + ".";
	}

	public static string EnterText(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.DungeonEnterText);
		return "¤ enter text set to " + Print(value) + ".";
	}

	public static string ExitText(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.DungeonExitText);
		return "¤ exit text set to " + Print(value) + ".";
	}

	public static string EnterHover(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.DungeonEnterHover);
		return "¤ enter hover set to " + Print(value) + ".";
	}

	public static string ExitHover(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.DungeonExitHover);
		return "¤ exit hover set to " + Print(value) + ".";
	}

	public static string Fall(ZNetView view, string? value)
	{
		Actions.SetInt(view, FallNumber(value), Hash.Fall);
		return "¤ fall set to " + Print(value) + ".";
	}

	private static int FallNumber(string? fall)
	{
		if (fall == null)
		{
			return 0;
		}
		return fall switch
		{
			"off" => 3, 
			"terrain" => 1, 
			"solid" => 2, 
			_ => 0, 
		};
	}

	public static string Growth(ZNetView view, string? value)
	{
		int num = GrowthNumber(value);
		Actions.SetInt(view, num, Hash.Growth);
		long num2 = ((num < 0) ? ZNet.instance.GetTime().Ticks : (DateTime.MaxValue.Ticks / 2));
		view.GetZDO().Set(Hash.PlantTime, num2);
		return "¤ growth set to " + Print(value) + ".";
	}

	private static int GrowthNumber(string? growth)
	{
		return growth switch
		{
			"big" => 4, 
			"big_bad" => 1, 
			"small" => 2, 
			"small_bad" => 3, 
			_ => 0, 
		};
	}

	public static string Component(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.Component);
		return "¤ component set to " + Print(value) + ".";
	}

	public static string AddComponent(ZNetView view, string value)
	{
		HashSet<string> hashSet = Parse.Split(view.GetZDO().GetString(Hash.Component, ""), ',').ToHashSet();
		hashSet.Add(value);
		Actions.SetString(view, string.Join(",", hashSet), Hash.Component);
		return "¤ component set to " + Print(value) + ".";
	}

	public static string Restrict(ZNetView view, bool? value)
	{
		value = !Actions.ToggleBool(view, Reverse(value), Hash.NoRestrict);
		return "¤ restrict set to " + Print(value) + ".";
	}

	public static string Boss(ZNetView view, bool? value)
	{
		value = Actions.ToggleBool(view, value, Hash.Boss);
		return "¤ boss set to " + Print(value) + ".";
	}

	public static string Hunt(ZNetView view, bool? value)
	{
		value = Actions.ToggleBool(view, value, Hash.HuntPlayer);
		return "¤ hunt set to " + Print(value) + ".";
	}

	public static string Tame(ZNetView view, bool? value)
	{
		value = Actions.ToggleBool(view, value, Hash.Tamed);
		return "¤ tamed set to " + Print(value) + ".";
	}

	public static string Unlock(ZNetView view, bool? value)
	{
		value = Actions.ToggleBool(view, value, Hash.Unlock);
		return "¤ unlock set to " + Print(value) + ".";
	}

	public static string Consume(ZNetView view, bool? value)
	{
		value = Actions.ToggleBool(view, value, Hash.DoorConsume);
		return "¤ consume set to " + Print(value) + ".";
	}

	public static string NoClose(ZNetView view, bool? value)
	{
		value = Actions.ToggleBool(view, value, Hash.DoorNoClose);
		return "¤ no close set to " + Print(value) + ".";
	}

	public static string DoorKey(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.DoorKey);
		return "¤ door key set to " + Print(value) + ".";
	}

	public static string Creator(ZNetView view, long? value)
	{
		Actions.SetLong(view, value.GetValueOrDefault(), Hash.Creator);
		return "¤ creator set to " + Print(value) + ".";
	}

	public static string Smoke(ZNetView view, string? value)
	{
		int value2 = 0;
		if (value == "off")
		{
			value2 = 1;
		}
		if (value == "ignore")
		{
			value2 = 2;
		}
		Actions.SetInt(view, value2, Hash.Smoke);
		return "¤ smoke set to " + Print(value) + ".";
	}

	public static string CLLC_BossEffect(ZNetView view, string[] values)
	{
		Enum_CLLC_Boss enum_CLLC_Boss = Enum_CLLC_Boss.None;
		for (int i = 0; i < values.Length; i++)
		{
			if (Enum.TryParse<Enum_CLLC_Boss>(values[i], ignoreCase: true, out var result))
			{
				enum_CLLC_Boss |= result;
			}
		}
		Actions.SetInt(view, (int)enum_CLLC_Boss, Hash.CLLC_BossEffect);
		return "¤ CLLC affix set to " + Print(enum_CLLC_Boss) + ".";
	}

	public static string CLLC_Effect(ZNetView view, string[] values)
	{
		Enum_CLLC_Effect enum_CLLC_Effect = Enum_CLLC_Effect.None;
		for (int i = 0; i < values.Length; i++)
		{
			if (Enum.TryParse<Enum_CLLC_Effect>(values[i], ignoreCase: true, out var result))
			{
				enum_CLLC_Effect |= result;
			}
		}
		Actions.SetInt(view, (int)enum_CLLC_Effect, Hash.CLLC_Effect);
		return "¤ CLLC effect set to " + Print(enum_CLLC_Effect) + ".";
	}

	public static string CLLC_Infusion(ZNetView view, string[] values)
	{
		Enum_CLLC_Infusion enum_CLLC_Infusion = Enum_CLLC_Infusion.None;
		for (int i = 0; i < values.Length; i++)
		{
			if (Enum.TryParse<Enum_CLLC_Infusion>(values[i], ignoreCase: true, out var result))
			{
				enum_CLLC_Infusion |= result;
			}
		}
		Actions.SetInt(view, (int)enum_CLLC_Infusion, Hash.CLLC_Infusion);
		return "¤ CLLC infusion set to " + Print(enum_CLLC_Infusion) + ".";
	}

	public static string SpawnData(ZNetView view, string? value)
	{
		Actions.SetString(view, (value == null) ? null : DataHelper.Base64(new Dictionary<string, string>(), value), Hash.Data);
		return "¤ spawn data set to " + Print(value) + ".";
	}

	public static string Spawn(ZNetView view, int hash, string? value)
	{
		Actions.SetPrefab(view, value, hash);
		return "¤ spawn prefab set to " + Print(value) + ".";
	}

	public static string Biome(ZNetView view, string? value)
	{
		Actions.SetInt(view, BiomeToInt(value ?? ""), Hash.Biome);
		return "¤ biome set to " + Print(value) + ".";
	}

	public static string Fuel(ZNetView view, string? value)
	{
		Actions.SetPrefab(view, value, Hash.OverrideFuel);
		return "¤ fuel prefab set to " + Print(value) + ".";
	}

	public static string ItemOffset(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.ItemOffset);
		return "¤ item offset set to " + Print(value) + ".";
	}

	public static string CoverOffset(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.CoverOffset);
		return "¤ cover offset set to " + Print(value) + ".";
	}

	public static string SpawnOffset(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.SpawnOffset);
		return "¤ spawn offset set to " + Print(value) + ".";
	}

	public static string ItemStandPrefix(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.ItemStandPrefix);
		return "¤ item stand prefix set to " + Print(value) + ".";
	}

	public static string GlobalKey(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.GlobalKey);
		return "¤ global key set to " + Print(value) + ".";
	}

	public static string Name(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.OverrideName);
		return "¤ name set to " + Print(value) + ".";
	}

	public static string Text(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.OverrideText);
		return "¤ text set to " + Print(value) + ".";
	}

	public static string TextBiome(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.TextBiome);
		return "¤ biome text set to " + Print(value) + ".";
	}

	public static string TextSpace(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.TextSpace);
		return "¤ space text set to " + Print(value) + ".";
	}

	public static string TextSleep(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.TextSleep);
		return "¤ sleep text set to " + Print(value) + ".";
	}

	public static string TextHappy(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.TextHappy);
		return "¤ happy text set to " + Print(value) + ".";
	}

	public static string TextCheck(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.TextCheck);
		return "¤ check text set to " + Print(value) + ".";
	}

	public static string TextExtract(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.TextExtract);
		return "¤ extract text set to " + Print(value) + ".";
	}

	public static string Compendium(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.Compendium);
		return "¤ compendium set to " + Print(value) + ".";
	}

	public static string Topic(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.Topic);
		return "¤ topic set to " + Print(value) + ".";
	}

	public static string Discover(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.Discover);
		return "¤ discover set to " + Print(value) + ".";
	}

	public static string Spawns(ZNetView view, string[] value)
	{
		float num = default(float);
		if (value.Length > 4 && !Parse.TryFloat(value[4], ref num))
		{
			value[4] = DataHelper.Base64(new Dictionary<string, string>(), value[4]);
		}
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.SpawnSpawnArea);
		return "¤ spawn prefabs set to " + Print(value2) + ".";
	}

	public static string Conversions(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.Conversion);
		return "¤ conversions set to " + Print(value2) + ".";
	}

	public static string Items(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.OverrideItems);
		return "¤ items set to " + Print(value2) + ".";
	}

	public static string SpawnEffect(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.SpawnEffect);
		return "¤ spawn effect set to " + Print(value2) + ".";
	}

	public static string DestroyEffect(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.DestroyEffect);
		return "¤ destroy effect set to " + Print(value2) + ".";
	}

	public static string UseEffect(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.UseEffect);
		return "¤ use effect set to " + Print(value2) + ".";
	}

	public static string InputEffect(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.InputEffect);
		return "¤ input effect set to " + Print(value2) + ".";
	}

	public static string OpenEffect(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.OpenEffect);
		return "¤ open effect set to " + Print(value2) + ".";
	}

	public static string CloseEffect(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.CloseEffect);
		return "¤ close effect set to " + Print(value2) + ".";
	}

	public static string LockedEffect(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.LockedEffect);
		return "¤ locked effect set to " + Print(value2) + ".";
	}

	public static string OutputEffect(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.OutputEffect);
		return "¤ output effect set to " + Print(value2) + ".";
	}

	public static string FuelEffect(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.FuelEffect);
		return "¤ fuel effect set to " + Print(value2) + ".";
	}

	public static string StartEffect(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.StartEffect);
		return "¤ start effect set to " + Print(value2) + ".";
	}

	public static string SpawnItem(ZNetView view, string? value)
	{
		Actions.SetPrefab(view, value, Hash.SpawnItem);
		return "¤ spawn item prefab set to " + Print(value) + ".";
	}

	public static string Item(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.OverrideItem);
		return "¤ item set to " + Print(value) + ".";
	}

	public static string Faction(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.Faction);
		return "¤ faction set to " + Print(value) + ".";
	}

	public static string Command(ZNetView view, string? value)
	{
		Actions.SetString(view, value, Hash.Command);
		return "¤ command set to " + Print(value) + ".";
	}

	private static string Resistance(string value)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected I4, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected I4, but got Unknown
		string[] array = value.Split(',');
		if (array.Length < 2)
		{
			return "";
		}
		if (!Enum.TryParse<DamageType>(array[0], ignoreCase: true, out DamageType result))
		{
			return "";
		}
		if (!Enum.TryParse<DamageModifier>(array[1], ignoreCase: true, out DamageModifier result2))
		{
			return "";
		}
		return (int)result + "," + (int)result2;
	}

	public static string Resistances(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", from s in value.Select(Resistance)
			where s != ""
			select s));
		Actions.SetString(view, value2, Hash.Resistances);
		return "¤ resistances set to " + Print(string.Join("|", value)) + ".";
	}

	public static string Attacks(ZNetView view, string[] value)
	{
		string value2 = ((value.Length == 0) ? null : string.Join("|", value));
		Actions.SetString(view, value2, Hash.Attacks);
		return "¤ attacks set to " + Print(string.Join("|", value)) + ".";
	}

	public static string MinLevel(ZNetView view, int? value)
	{
		Actions.SetInt(view, value, Hash.MinLevel);
		return "¤ minimum level set to " + Print(value) + ".";
	}

	public static string MaxLevel(ZNetView view, int? value)
	{
		Actions.SetInt(view, value, Hash.MaxLevel);
		return "¤ maximum level set to " + Print(value) + ".";
	}

	public static string MinAmount(ZNetView view, int? value)
	{
		Actions.SetInt(view, value, Hash.MinAmount);
		return "¤ minimum amount set to " + Print(value) + ".";
	}

	public static string Level(ZNetView view, int? value)
	{
		Actions.SetInt(view, value, Hash.Level);
		return "¤ level set to " + Print(value) + ".";
	}

	public static string Health(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.Health);
		return "¤ health set to " + Print(value) + ".";
	}

	public static string MaxAmount(ZNetView view, int? value)
	{
		Actions.SetInt(view, value, Hash.MaxAmount);
		return "¤ maximum amount set to " + Print(value) + ".";
	}

	public static string Amount(ZNetView view, int? value)
	{
		Actions.SetInt(view, value, Hash.Amount);
		return "¤ amount set to " + Print(value) + ".";
	}

	public static string MaxNear(ZNetView view, int? value)
	{
		Actions.SetInt(view, value, Hash.MaxNear);
		return "¤ max near set to " + Print(value) + ".";
	}

	public static string MaxTotal(ZNetView view, int? value)
	{
		Actions.SetInt(view, value, Hash.MaxTotal);
		return "¤ max total set to " + Print(value) + ".";
	}

	public static string SpawnCondition(ZNetView view, int? value)
	{
		Actions.SetInt(view, value, Hash.SpawnCondition);
		return "¤ spawn condition set to " + Print(value) + ".";
	}

	public static string MaxFuel(ZNetView view, int? value)
	{
		Actions.SetInt(view, value, Hash.MaxFuel);
		return "¤ maximum fuel set to " + Print(value) + ".";
	}

	public static string FuelUsage(ZNetView view, int? value)
	{
		Actions.SetInt(view, value, Hash.FuelUsage);
		return "¤ fuel usage set to " + Print(value) + ".";
	}

	public static string Speed(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.Speed);
		return "¤ speed set to " + Print(value) + " seconds.";
	}

	public static string MaxCover(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.MaxCover);
		return "¤ max cover set to " + Print(value) + " seconds.";
	}

	public static string Respawn(ZNetView view, int hash, float? value)
	{
		Actions.SetFloat(view, value, hash);
		return "¤ respawn time set to " + Print(value) + " minutes.";
	}

	public static string Delay(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.Delay);
		return "¤ delay set to " + Print(value) + " seconds.";
	}

	public static string ItemStandRange(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.ItemStandRange);
		return "¤ item stand range set to " + Print(value) + " meters.";
	}

	public static string SpawnMaxY(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.SpawnMaxY);
		return "¤ spawn max y set to " + Print(value) + " meters.";
	}

	public static string RespawnSeconds(ZNetView view, int hash, float? value)
	{
		Actions.SetFloat(view, value, hash);
		return "¤ respawn time set to " + Print(value) + " seconds.";
	}

	public static string SpawnHealth(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.SpawnHealth);
		return "¤ spawn health set to " + Print(value) + ".";
	}

	public static string TriggerDistance(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.TriggerDistance);
		return "¤ trigger distance set to " + Print(value) + ".";
	}

	public static string TriggerNoise(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.TriggerNoise);
		return "¤ trigger noise set to " + Print(value) + " meters.";
	}

	public static string LevelChance(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.LevelChance);
		return "¤ level up chance set to " + Print(value) + " %.";
	}

	public static string SpawnRadius(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.SpawnRadius);
		return "¤ spawn radius set to " + Print(value) + " meters.";
	}

	public static string NearRadius(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.NearRadius);
		return "¤ near radius set to " + Print(value) + " meters.";
	}

	public static string FarRadius(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.FarRadius);
		return "¤ far radius set to " + Print(value) + " meters.";
	}

	public static string SpawnOffset(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.SpawnOffset);
		return "¤ spawn offset to " + Print(value) + " meters.";
	}

	public static string Destroy(ZNetView view, float? value)
	{
		Actions.SetFloat(view, value, Hash.Destroy);
		return "¤ timed destroy set to " + Print(value) + " seconds.";
	}
}
