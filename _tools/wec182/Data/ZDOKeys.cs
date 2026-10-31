using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using UnityEngine;

namespace Data;

public class ZDOKeys
{
	private static string[]? knownKeys;

	private static readonly string[] StaticKeys = new string[153]
	{
		"alive_time", "attachJoint", "body_avel", "body_vel", "bosscount", "emote_oneshot", "HaveSaddle", "haveTarget", "IsBlocking", "max_health",
		"picked_time", "relPos", "relRot", "scale", "scaleScalar", "vel", "lastWorldTime", "spawntime", "spawnpoint", "HasFields",
		"user_u", "user_i", "RodOwner_u", "RodOwner_i", "CatchID_u", "CatchID_i", "target_u", "target_i", "rooms", "spawn_id_u",
		"spawn_id_i", "parent_id_u", "parent_id_i", "CL&LC effect", "CL&LC infusion", "override_amount", "override_attacks", "override_biome", "override_boss", "override_collision",
		"override_compendium", "override_component", "override_conversion", "override_cover_offset", "override_data", "override_delay", "override_destroy", "override_discover", "override_dungeon_enter_hover", "override_dungeon_enter_text",
		"override_dungeon_exit_hover", "override_dungeon_exit_text", "override_dungeon_weather", "override_effect", "override_event", "override_faction", "override_fall", "override_far_radius", "override_fuel", "override_fuel_effect",
		"override_globalkey", "override_growth", "override_health", "override_input_effect", "override_interact", "override_item", "override_item_offset", "override_item_stand_prefix", "override_item_stand_range", "override_items",
		"override_level_chance", "override_maximum_amount", "override_maximum_cover", "override_maximum_fuel", "override_maximum_level", "override_max_near", "override_max_total", "override_minimum_amount", "override_minimum_level", "override_name",
		"override_near_radius", "override_output_effect", "override_pickable_spawn", "override_pickable_respawn", "override_render", "override_resistances", "override_respawn", "override_restrict", "override_smoke", "override_spawn",
		"override_spawn_condition", "override_spawn_effect", "override_spawn_max_y", "override_spawn_offset", "override_spawn_radius", "override_spawnarea_spawn", "override_spawnarea_respawn", "override_spawn_item", "override_start_effect", "override_text",
		"override_text_biome", "override_text_check", "override_text_extract", "override_text_happy", "override_text_sleep", "override_text_space", "override_topic", "override_trigger_distance", "override_trigger_noise", "override_unlock",
		"override_use_effect", "override_water", "override_wear", "override_weather", "KGmarketNPC", "KGnpcProfile", "KGnpcModelOverride", "KGnpcNameOverride", "KGnpcDialogue", "KGleftItem",
		"KGrightItem", "KGhelmetItem", "KGchestItem", "KGlegsItem", "KGcapeItem", "KGhairItem", "KGhairItemColor", "KGLeftItemBack", "KGRightItemBack", "KGinteractAnimation",
		"KGgreetingAnimation", "KGbyeAnimation", "KGgreetingText", "KGbyeText", "KGskinColor", "KGcraftingAnimation", "KGbeardItem", "KGbeardColor", "KGinteractSound", "KGtextSize",
		"KGtextHeight", "KGperiodicAnimation", "KGperiodicAnimationTime", "KGperiodicSound", "KGperiodicSoundTime", "KGnpcScale", "itemstand_hide", "itemstand_offset", "itemstand_rotation", "itemstand_scale",
		"steamName", "steamID", "xray_created"
	};

	private static readonly string[] AnimationKeys = new string[36]
	{
		"alert", "footstep", "forward_speed", "sideway_speed", "anim_speed", "statef", "statei", "blocking", "attack", "flapping",
		"falling", "onGround", "intro", "crouching", "encumbered", "equipping", "attach_bed", "attach_chair", "attach_throne", "attach_sitship",
		"attach_mast", "attach_dragon", "attach_lox", "bow_aim", "reload_crossbow", "crafting", "visible", "turn_speed", "idle", "flying",
		"body_forward_speed", "inWater", "onGround", "minoraction", "minoraction_fast", "emote"
	};

	private static Dictionary<string, int> keyToHash = new Dictionary<string, int>();

	private static Dictionary<int, string>? hashToKey;

	private static string[] KnownKeys => knownKeys ?? (knownKeys = GenerateKeys());

	private static Dictionary<int, string> HashToKey => hashToKey ?? (hashToKey = KnownKeys.ToDictionary(CalculateHash, (string x) => x));

	private static string[] GenerateKeys()
	{
		string[] staticKeys = StaticKeys;
		List<string> list = new List<string>(staticKeys.Length);
		list.AddRange(staticKeys);
		List<string> list2 = list;
		List<Assembly> list3 = new List<Assembly>();
		list3.Add(Assembly.GetAssembly(typeof(ZNetView)));
		list3.AddRange(from p in Chainloader.PluginInfos.Values
			where (Object)(object)p.Instance != (Object)null
			select ((object)p.Instance).GetType().Assembly);
		Type baseType = typeof(MonoBehaviour);
		Type[] source = list3.SelectMany(delegate(Assembly s)
		{
			try
			{
				return s.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				return ex.Types.Where((Type t) => t != null);
			}
		}).Where(delegate(Type t)
		{
			try
			{
				return baseType.IsAssignableFrom(t);
			}
			catch
			{
				return false;
			}
		}).ToArray();
		list2.AddRange(source.Select((Type t) => "HasFields" + t.Name));
		list2.AddRange(source.SelectMany((Type t) => from f in t.GetFields(BindingFlags.Instance | BindingFlags.Public)
			select t.Name + "." + f.Name));
		list2.AddRange(from f in typeof(ZDOVars).GetFields(BindingFlags.Static | BindingFlags.Public)
			select f.Name.Replace("s_", ""));
		list2.AddRange(from f in typeof(ZDOVars).GetFields(BindingFlags.Static | BindingFlags.Public)
			select FirstLetterUpper(f.Name.Replace("s_", "")));
		for (int i = 0; i < 20; i++)
		{
			list2.Add($"MatVar{i}");
			list2.Add($"item{i}");
			list2.Add($"quality{i}");
			list2.Add($"variant{i}");
			list2.Add($"data_{i}");
			list2.Add($"data__{i}");
			list2.Add($"drop_hash{i}");
			list2.Add($"drop_amount{i}");
			list2.Add($"slot{i}");
			list2.Add($"slotstatus{i}");
			list2.Add($"{i}_item");
			list2.Add($"{i}_durability");
			list2.Add($"{i}_stack");
			list2.Add($"{i}_quality");
			list2.Add($"{i}_variant");
			list2.Add($"{i}_crafterID");
			list2.Add($"{i}_crafterName");
			list2.Add($"{i}_dataCount");
			for (int j = 0; j < 20; j++)
			{
				list2.Add($"{i}_data_{j}");
				list2.Add($"{i}_data__{j}");
			}
			list2.Add($"{i}_worldLevel");
			list2.Add($"{i}_pickedUp");
			list2.Add($"pu_id{i}");
			list2.Add($"pu_name{i}");
			list2.Add($"Health{i}");
			list2.Add($"room{i}");
			list2.Add($"room{i}_pos");
			list2.Add($"room{i}_rot");
			list2.Add($"target{i}");
		}
		list2.AddRange(AnimationKeys.Select((string k) => "$" + k));
		List<string> list4 = new List<string>();
		list4.AddRange(list2.Distinct());
		return list4.ToArray();
	}

	public static bool IsRoomPos(string key)
	{
		if (key.EndsWith("_pos"))
		{
			return key.StartsWith("room");
		}
		return false;
	}

	public static bool IsRoomRot(string key)
	{
		if (key.EndsWith("_rot"))
		{
			return key.StartsWith("room");
		}
		return false;
	}

	private static string FirstLetterUpper(string s)
	{
		return char.ToUpper(s[0]) + s.Substring(1);
	}

	public static string Convert(int hash)
	{
		if (!HashToKey.TryGetValue(hash, out string value))
		{
			return hash.ToString();
		}
		return value;
	}

	public static int Hash(string key)
	{
		if (keyToHash.TryGetValue(key, out var value))
		{
			return value;
		}
		value = CalculateHash(key);
		keyToHash[key] = value;
		HashToKey[value] = key;
		return value;
	}

	private static int CalculateHash(string key)
	{
		if (key.StartsWith("$", StringComparison.InvariantCultureIgnoreCase))
		{
			int hash = ZSyncAnimation.GetHash(key.Substring(1));
			if (key == "$anim_speed")
			{
				return hash;
			}
			return hash + 438569;
		}
		return StringExtensionMethods.GetStableHashCode(key);
	}

	public static bool Exists(int hash)
	{
		if (hashToKey != null)
		{
			return HashToKey.ContainsKey(hash);
		}
		return true;
	}
}
