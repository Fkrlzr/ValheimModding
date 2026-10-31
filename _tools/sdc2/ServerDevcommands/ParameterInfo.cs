using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ServerDevcommands;

public class ParameterInfo
{
	private static List<string> components = new List<string>();

	private static readonly List<string> enumGlobalKeys;

	private static readonly TimedCache globalKeys;

	private static readonly TimedCache ids;

	private static readonly TimedCache environments;

	private static readonly List<string> colors;

	private static readonly TimedCache objectIds;

	private static readonly TimedCache roomIds;

	private static List<ZoneLocation>? LocationDataCache;

	private static List<string>? LocationIdsCache;

	private static List<string>? ServerLocationIdsCache;

	private static List<ZoneVegetation>? VegetationDataCache;

	private static List<string>? VegetationIdsCache;

	private static List<string>? ServerVegetationIdsCache;

	private static readonly TimedCache statusEffects;

	private static readonly TimedCache itemIds;

	private static readonly TimedCache hairs;

	private static readonly TimedCache beards;

	private static List<string> keyCodes;

	private static List<string> keyCodesWithNegative;

	public static List<string> Origin;

	public static List<string> None;

	public static List<string> Missing;

	public static List<string> Components
	{
		get
		{
			if (components.Count == 0)
			{
				List<string> list = new List<string>();
				list.AddRange(ComponentInfo.Types.Select((Type t) => t.Name));
				components = list;
			}
			return components;
		}
	}

	public static List<string> GlobalKeys => globalKeys.Get();

	private static IEnumerable<KeySlider> WorldModifierSliders => ServerOptionsGUI.m_modifiers?.OfType<KeySlider>() ?? Array.Empty<KeySlider>();

	public static List<string> WorldModifiers
	{
		get
		{
			List<string> list = new List<string>();
			list.AddRange(WorldModifierSliders.Select((KeySlider slider) => ((object)(WorldModifiers)(ref slider.m_modifier)).ToString()));
			return list;
		}
	}

	public static List<string> ActiveGlobalKeys
	{
		get
		{
			if (!Object.op_Implicit((Object)(object)ZoneSystem.instance))
			{
				return new List<string>();
			}
			return ZoneSystem.instance.GetGlobalKeys();
		}
	}

	public static List<string> Ids => ids.Get();

	public static List<string> Environments => environments.Get();

	public static List<string> Colors => colors;

	public static List<string> ObjectIds => objectIds.Get();

	public static List<string> RoomIds => roomIds.Get();

	public static List<string> LocationIds
	{
		get
		{
			if (ServerLocationIdsCache != null)
			{
				return ServerLocationIdsCache;
			}
			if (!Object.op_Implicit((Object)(object)ZoneSystem.instance))
			{
				return new List<string>();
			}
			if (LocationDataCache != ZoneSystem.instance.m_locations)
			{
				LocationIdsCache = null;
			}
			if (LocationIdsCache == null)
			{
				List<string> list = new List<string>();
				list.AddRange((from l in ZoneSystem.instance.m_locations.Where(Helper.IsValid)
					select l.m_prefab.Name).Distinct());
				LocationIdsCache = list;
			}
			LocationDataCache = ZoneSystem.instance.m_locations;
			return LocationIdsCache;
		}
	}

	public static List<string> VegetationIds
	{
		get
		{
			if (ServerVegetationIdsCache != null)
			{
				return ServerVegetationIdsCache;
			}
			if (!Object.op_Implicit((Object)(object)ZoneSystem.instance))
			{
				return new List<string>();
			}
			if (VegetationDataCache != ZoneSystem.instance.m_vegetation)
			{
				VegetationIdsCache = null;
			}
			if (VegetationIdsCache == null)
			{
				List<string> list = new List<string>();
				list.AddRange((from veg in ZoneSystem.instance.m_vegetation
					where veg != null && (Object)(object)veg.m_prefab != (Object)null
					select veg into l
					select l.m_name).Distinct());
				VegetationIdsCache = list;
			}
			VegetationDataCache = ZoneSystem.instance.m_vegetation;
			return VegetationIdsCache;
		}
	}

	public static List<string> Events
	{
		get
		{
			List<string> list = new List<string>();
			list.AddRange(RandEventSystem.instance.m_events.Select((RandomEvent ev) => ev.m_name));
			return list;
		}
	}

	public static List<string> StatusEffects => statusEffects.Get();

	public static List<string> EffectAreas
	{
		get
		{
			string[] names = Enum.GetNames(typeof(Type));
			List<string> list = new List<string>(names.Length);
			list.AddRange(names);
			return list;
		}
	}

	public static List<string> ItemIds => itemIds.Get();

	public static List<string> PlayerNames => ZNet.instance?.m_players.Select((PlayerInfo player) => (!player.m_name.Contains(" ")) ? player.m_name : ("\"" + player.m_name + "\"")).ToList() ?? new List<string>(1) { "No players found!" };

	public static List<string> PublicPlayerNames => (from player in ZNet.instance?.m_players
		where player.m_publicPosition
		select (!player.m_name.Contains(" ")) ? player.m_name : ("\"" + player.m_name + "\"")).ToList() ?? new List<string>(1) { "No public players found!" };

	public static List<string> Hairs => hairs.Get();

	public static List<string> Beards => beards.Get();

	public static List<string> KeyCodes
	{
		get
		{
			if (keyCodes.Count == 0)
			{
				string[] names = Enum.GetNames(typeof(KeyCode));
				List<string> list = new List<string>();
				list.AddRange(names.Select((string value) => value.ToLower()));
				keyCodes = list;
				keyCodes.Add("wheel");
			}
			return keyCodes;
		}
	}

	public static List<string> KeyCodesWithNegative
	{
		get
		{
			if (keyCodesWithNegative.Count == 0)
			{
				string[] names = Enum.GetNames(typeof(KeyCode));
				List<string> list = new List<string>();
				list.AddRange(names.Select((string value) => value.ToLower()));
				keyCodesWithNegative = list;
				keyCodesWithNegative.AddRange(keyCodesWithNegative.Select((string value) => "-" + value).ToList());
			}
			return keyCodesWithNegative;
		}
	}

	public static List<string> CommandNames
	{
		get
		{
			Dictionary<string, ConsoleCommand>.KeyCollection keys = Terminal.commands.Keys;
			List<string> list = new List<string>(keys.Count);
			list.AddRange(keys);
			return list;
		}
	}

	public static List<string> WorldModifierOptions(string modifier)
	{
		string modifier2 = modifier;
		List<string> list = new List<string>();
		list.AddRange(WorldModifierSliders.Where((KeySlider slider) => ((object)(WorldModifiers)(ref slider.m_modifier)).ToString().Equals(modifier2, StringComparison.OrdinalIgnoreCase)).SelectMany((KeySlider slider) => slider.m_settings.Select((SliderSetting setting) => ((object)(WorldModifierOption)(ref setting.m_modifierValue)).ToString())));
		return list;
	}

	public static void SetServerLocationIds(List<string>? data)
	{
		ServerLocationIdsCache = data;
	}

	public static void SetServerVegetationIds(List<string>? data)
	{
		ServerVegetationIdsCache = data;
	}

	public static List<string> Create(string value)
	{
		return new List<string>(1) { "?" + value };
	}

	public static List<string> Error(string value)
	{
		return new List<string>(1) { "?<color=red>Error:</color> " + value };
	}

	public static List<string> Create(string name, string value, string description)
	{
		return Create(name + "=<color=yellow>" + value + "</color> | " + description);
	}

	public static List<string> CreateWithMinMax(string name, string value, string description)
	{
		return Create(name + "=<color=yellow>" + value + "</color> or " + name + "=<color=yellow>min-max</color> | " + description);
	}

	public static List<string> Create(string values, string description)
	{
		return Create(values + " | " + description);
	}

	public static List<string> InvalidNamed(string name)
	{
		return Error("Invalid named parameter " + name + "!");
	}

	public static List<string> Flag(string name)
	{
		return Error(name + " is a flag so it doesn't have any arguments!");
	}

	public static List<string> Flag(string name, int subIndex)
	{
		if (subIndex != 0)
		{
			return Flag(name);
		}
		return new List<string>(1) { name };
	}

	public static List<string> Flag(string name, string description)
	{
		return Error(name + " is a flag so it doesn't have any arguments! | " + description);
	}

	public static List<string> Flag(string name, string description, int subIndex)
	{
		if (subIndex != 0)
		{
			return Flag(name, description);
		}
		return new List<string>(1) { name };
	}

	public static List<string> XZY(string name, string description, int index)
	{
		return index switch
		{
			0 => Create(name + "=<color=yellow>X</color>,Z,Y | " + description + "."), 
			1 => Create(name + "=X,<color=yellow>Z</color>,Y | " + description + "."), 
			2 => Create(name + "=X,Z,<color=yellow>Y</color> | " + description + "."), 
			_ => None, 
		};
	}

	public static List<string> XZYR(string description, int index)
	{
		return index switch
		{
			0 => Create("<color=yellow>X</color>,Z,Y,R | " + description + "."), 
			1 => Create("X,<color=yellow>Z</color>,Y,R | " + description + "."), 
			2 => Create("X,Z,<color=yellow>Y</color>,R | " + description + "."), 
			4 => Create("X,Z,Y,<color=yellow>R</color> | " + description + "."), 
			_ => None, 
		};
	}

	public static List<string> XZY(string description, int index)
	{
		return index switch
		{
			0 => Create("<color=yellow>X</color>,Z,Y | " + description + "."), 
			1 => Create("X,<color=yellow>Z</color>,Y | " + description + "."), 
			2 => Create("X,Z,<color=yellow>Y</color> | " + description + "."), 
			_ => None, 
		};
	}

	public static List<string> XYZ(string name, string description, int index)
	{
		return index switch
		{
			0 => Create(name + "=<color=yellow>X</color>,Z,Y | " + description + "."), 
			1 => Create(name + "=X,<color=yellow>Y</color>,Z | " + description + "."), 
			2 => Create(name + "=X,Y,<color=yellow>Z</color> | " + description + "."), 
			_ => None, 
		};
	}

	public static List<string> XYZ(string description, int index)
	{
		return index switch
		{
			0 => Create("<color=yellow>X</color>,Z,Y | " + description + "."), 
			1 => Create("X,<color=yellow>Y</color>,Z | " + description + "."), 
			2 => Create("X,Y,<color=yellow>Z</color> | " + description + "."), 
			_ => None, 
		};
	}

	public static List<string> YXZ(string name, string description, int index)
	{
		return index switch
		{
			0 => Create(name + "=<color=yellow>Y</color>,X,Z | " + description + "."), 
			1 => Create(name + "=Y,<color=yellow>X</color>,Z | " + description + "."), 
			2 => Create(name + "=Y,X,<color=yellow>Z</color> | " + description + "."), 
			_ => None, 
		};
	}

	public static List<string> YXZ(string description, int index)
	{
		return index switch
		{
			0 => Create("<color=yellow>Y</color>,X,Z | " + description + "."), 
			1 => Create("Y,<color=yellow>X</color>,Z | " + description + "."), 
			2 => Create("Y,X,<color=yellow>Z</color> | " + description + "."), 
			_ => None, 
		};
	}

	public static List<string> XZ(string name, string description, int index)
	{
		return index switch
		{
			0 => Create(name + "=<color=yellow>X</color>,Z | " + description + "."), 
			1 => Create(name + "=X,<color=yellow>Z</color> | " + description + "."), 
			_ => None, 
		};
	}

	public static List<string> XZ(string description, int index)
	{
		return index switch
		{
			0 => Create("<color=yellow>X</color>,Z | " + description + "."), 
			1 => Create("X,<color=yellow>Z</color> | " + description + "."), 
			_ => None, 
		};
	}

	public static List<string> FRU(string name, string description, int index)
	{
		return index switch
		{
			0 => Create(name + "=<color=yellow>forward</color>,right,up | " + description + "."), 
			1 => Create(name + "=forward,<color=yellow>right</color>,up | " + description + "."), 
			2 => Create(name + "=forward,right,<color=yellow>up</color> | " + description + "."), 
			_ => None, 
		};
	}

	public static List<string> FRU(string description, int index)
	{
		return index switch
		{
			0 => Create("<color=yellow>forward</color>,right,up | " + description + "."), 
			1 => Create("forward,<color=yellow>right</color>,up | " + description + "."), 
			2 => Create("forward,right,<color=yellow>up</color> | " + description + "."), 
			_ => None, 
		};
	}

	public static List<string> Scale(string name, string description, int index)
	{
		return index switch
		{
			0 => Create(name + "=<color=yellow>number</color> or " + name + "=<color=yellow>X</color>,Z,Y,free | " + description), 
			1 => Create(name + "=X,<color=yellow>Y</color>,Z,free  | " + description + "."), 
			2 => Create(name + "=X,Y,<color=yellow>Z</color>,free  | " + description + "."), 
			3 => Create(name + "=X,Y,Z<color=yellow>free</color>  | If given, each axis is randomized separately."), 
			_ => None, 
		};
	}

	public static List<string> Scale(string description, int index)
	{
		if (index == 0)
		{
			return Create("<color=yellow>number</color> or " + XYZ(description, index)[0].Substring(0));
		}
		return XYZ(description, index);
	}

	public static List<string> RollPitchYaw(string name, string description, int index)
	{
		return index switch
		{
			0 => Create(name + "=<color=yellow>roll</color>,pitch,yaw | " + description + "."), 
			1 => Create(name + "=roll,<color=yellow>pitch</color>,yaw | " + description + "."), 
			2 => Create(name + "=roll,pitch,<color=yellow>yaw</color> | " + description + "."), 
			_ => None, 
		};
	}

	public static List<string> RollPitchYaw(string description, int index)
	{
		return index switch
		{
			0 => Create("<color=yellow>roll</color>,pitch,yaw | " + description + "."), 
			1 => Create("roll,<color=yellow>pitch</color>,yaw | " + description + "."), 
			2 => Create("roll,pitch,<color=yellow>yaw</color> | " + description + "."), 
			_ => None, 
		};
	}

	public static List<string> YawRollPitch(string name, string description, int index)
	{
		return index switch
		{
			0 => Create(name + "=<color=yellow>yaw</color>,roll,pitch | " + description + "."), 
			1 => Create(name + "=yaw,<color=yellow>roll</color>,pitch | " + description + "."), 
			2 => Create(name + "=yaw,roll,<color=yellow>pitch</color> | " + description + "."), 
			_ => None, 
		};
	}

	public static List<string> YawRollPitch(string description, int index)
	{
		return index switch
		{
			0 => Create("<color=yellow>yaw</color>,roll,pitch | " + description + "."), 
			1 => Create("yaw,<color=yellow>roll</color>,pitch | " + description + "."), 
			2 => Create("yaw,roll,<color=yellow>pitch</color> | " + description + "."), 
			_ => None, 
		};
	}

	static ParameterInfo()
	{
		List<string> list = new List<string>();
		list.AddRange(from s in Enum.GetNames(typeof(GlobalKeys))
			select s.ToLowerInvariant());
		enumGlobalKeys = list;
		globalKeys = new TimedCache(delegate
		{
			if (Object.op_Implicit((Object)(object)ZNetScene.instance))
			{
				IEnumerable<string> second = from prefab in ZNetScene.instance.m_namedPrefabs.Values
					select prefab.GetComponent<Character>()?.m_defeatSetGlobalKey into key
					where !string.IsNullOrEmpty(key)
					select key.ToLowerInvariant();
				List<string> list4 = enumGlobalKeys.Union<string>(second).ToList();
				list4.Sort();
				return list4;
			}
			return (List<string>?)null;
		}, enumGlobalKeys);
		ids = new TimedCache(delegate
		{
			List<string> list3 = ObjectIds.Concat<string>(LocationIds).ToList();
			list3.Sort();
			return list3;
		});
		environments = new TimedCache(delegate
		{
			if (Object.op_Implicit((Object)(object)EnvMan.instance))
			{
				List<string> list2 = EnvMan.instance.m_environments.Select((EnvSetup env) => env.m_name.Replace(" ", "_")).ToList();
				list2.Sort();
				return list2;
			}
			return (List<string>?)null;
		});
		colors = new List<string>(23)
		{
			"#rrggbbaa", "aqua", "black", "blue", "brown", "cyan", "darkblue", "fuchsia", "green", "grey",
			"lightblue", "lime", "magenta", "maroon", "navy", "olive", "orange", "purple", "red", "silver",
			"teal", "white", "yellow"
		};
		objectIds = new TimedCache(() => (!Object.op_Implicit((Object)(object)ZNetScene.instance)) ? null : ZNetScene.instance.GetPrefabNames());
		roomIds = new TimedCache(() => (!Object.op_Implicit((Object)(object)DungeonDB.instance)) ? null : DungeonDB.instance.m_rooms.Select((RoomData room) => room.m_prefab.Name).ToList());
		LocationDataCache = null;
		LocationIdsCache = null;
		ServerLocationIdsCache = null;
		VegetationDataCache = null;
		VegetationIdsCache = null;
		ServerVegetationIdsCache = null;
		statusEffects = new TimedCache(() => (!Object.op_Implicit((Object)(object)ObjectDB.instance)) ? null : ObjectDB.instance.m_StatusEffects.Select((StatusEffect se) => ((Object)se).name).ToList());
		itemIds = new TimedCache(() => (!Object.op_Implicit((Object)(object)ObjectDB.instance)) ? null : ObjectDB.instance.m_items.Select((GameObject item) => ((Object)item).name).ToList());
		hairs = new TimedCache(() => (!Object.op_Implicit((Object)(object)ObjectDB.instance)) ? null : (from item in ObjectDB.instance.GetAllItems((ItemType)10, "Hair")
			select ((Object)item).name).ToList());
		beards = new TimedCache(() => (!Object.op_Implicit((Object)(object)ObjectDB.instance)) ? null : (from item in ObjectDB.instance.GetAllItems((ItemType)10, "Beard")
			select ((Object)item).name).ToList());
		keyCodes = new List<string>();
		keyCodesWithNegative = new List<string>();
		Origin = new List<string>(3) { "player", "object", "world" };
		None = Error("Too many parameters!");
		Missing = Create("No autocomplete available.");
	}
}
