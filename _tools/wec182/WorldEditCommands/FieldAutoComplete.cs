using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Data;
using ServerDevcommands;
using TMPro;
using UnityEngine;

namespace WorldEditCommands;

public class FieldAutoComplete
{
	public static Dictionary<string, Dictionary<string, Type>> Fields = new Dictionary<string, Dictionary<string, Type>>();

	public static Dictionary<string, List<string>> Keys = new Dictionary<string, List<string>>();

	private static List<string> Components = new List<string>();

	private static readonly HashSet<Type> ValidTypes = new HashSet<Type>
	{
		typeof(string),
		typeof(int),
		typeof(float),
		typeof(bool),
		typeof(Vector3),
		typeof(GameObject),
		typeof(ItemDrop)
	};

	private static readonly HashSet<Type> ValidTweakTypes;

	private static readonly Dictionary<string, Dictionary<string, Type>> ZdoFields;

	public static void Init()
	{
		Fields = LoadFields();
		InitZdoFields();
		foreach (KeyValuePair<string, Dictionary<string, Type>> field in Fields)
		{
			if (!Keys.ContainsKey(field.Key))
			{
				Keys[field.Key] = new List<string>();
			}
			foreach (KeyValuePair<string, Type> item2 in field.Value)
			{
				string item = item2.Key.Replace("m_", "");
				if (!Keys[field.Key].Contains(item))
				{
					Keys[field.Key].Add(item);
				}
			}
		}
		foreach (KeyValuePair<string, Dictionary<string, Type>> zdoField in ZdoFields)
		{
			if (!Keys.ContainsKey(zdoField.Key))
			{
				Keys[zdoField.Key] = new List<string>();
			}
			foreach (KeyValuePair<string, Type> item3 in zdoField.Value)
			{
				if (!Keys[zdoField.Key].Contains(item3.Key))
				{
					Keys[zdoField.Key].Add(item3.Key);
				}
			}
		}
		Dictionary<string, List<string>>.KeyCollection keys = Keys.Keys;
		List<string> list = new List<string>(keys.Count);
		list.AddRange(keys);
		Components = list;
		Components = Components.Distinct().ToList();
	}

	private static Dictionary<string, Dictionary<string, Type>> LoadFields()
	{
		Dictionary<string, Dictionary<string, Type>> dictionary = new Dictionary<string, Dictionary<string, Type>>();
		Type[] types = ComponentInfo.Types;
		HashSet<Type> hashSet = (WorldEditCommands.IsTweaks ? ValidTweakTypes : ValidTypes);
		Type[] array = types;
		foreach (Type type in array)
		{
			if (!dictionary.ContainsKey(type.Name))
			{
				dictionary[type.Name] = new Dictionary<string, Type>();
			}
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (hashSet.Contains(fieldInfo.FieldType))
				{
					dictionary[type.Name][fieldInfo.Name] = fieldInfo.FieldType;
					PreventStripping.AddKey(type, fieldInfo);
				}
			}
			if (dictionary[type.Name].Count == 0)
			{
				dictionary.Remove(type.Name);
			}
		}
		return dictionary;
	}

	private static Dictionary<string, Dictionary<string, Type>> LoadTweakFields()
	{
		Dictionary<string, Dictionary<string, Type>> dictionary = new Dictionary<string, Dictionary<string, Type>>();
		Type[] types = ComponentInfo.Types;
		HashSet<Type> validTweakTypes = ValidTweakTypes;
		Type[] array = types;
		foreach (Type type in array)
		{
			if (!dictionary.ContainsKey(type.Name))
			{
				dictionary[type.Name] = new Dictionary<string, Type>();
			}
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.FieldType.IsEnum || validTweakTypes.Contains(fieldInfo.FieldType))
				{
					dictionary[type.Name][fieldInfo.Name] = fieldInfo.FieldType;
					PreventStripping.AddKey(type, fieldInfo);
				}
			}
			if (dictionary[type.Name].Count == 0)
			{
				dictionary.Remove(type.Name);
			}
		}
		return dictionary;
	}

	public static List<string> GetComponents()
	{
		return GetComponents(PrefabFromCommand(GetInput()));
	}

	private static string GetInput()
	{
		Aliasing.RestoreAlias(((Terminal)Console.m_instance).m_input);
		string result = Aliasing.Plain(((TMP_InputField)((Terminal)Console.m_instance).m_input).text);
		Aliasing.RemoveAlias(((Terminal)Console.m_instance).m_input);
		return result;
	}

	public static string PrefabFromCommand(string command)
	{
		string[] array = command.Split(' ');
		string text = array.LastOrDefault((string s) => s.StartsWith("id=", StringComparison.Ordinal));
		string result = "";
		if (!string.IsNullOrEmpty(text))
		{
			result = text.Split('=')[1];
		}
		else if (array.Length > 1 && ZNetScene.instance.m_namedPrefabs.ContainsKey(ZDOKeys.Hash(array[1])))
		{
			result = array[1];
		}
		else
		{
			Player localPlayer = Player.m_localPlayer;
			GameObject val = ((localPlayer != null) ? ((Humanoid)localPlayer).GetHoverObject() : null);
			if (Object.op_Implicit((Object)(object)val))
			{
				result = Utils.GetPrefabName(val);
			}
		}
		return result;
	}

	public static List<string> GetComponents(string prefab)
	{
		return GetComponents(ZDOKeys.Hash(prefab));
	}

	public static List<string> GetComponents(int prefab)
	{
		if (ZNetScene.instance.m_namedPrefabs.TryGetValue(prefab, out var value))
		{
			List<string> list = new List<string>();
			list.AddRange(from c in value.GetComponentsInChildren<MonoBehaviour>()
				select ((object)c).GetType().Name);
			list.Add("zdo");
			return list;
		}
		return Components;
	}

	public static List<string> GetFields()
	{
		string input = GetInput();
		return GetFields(ComponentFromCommand(PrefabFromCommand(input), input));
	}

	private static string ComponentFromCommand(string prefab, string command)
	{
		string text = command.Split(' ').LastOrDefault((string s) => s.StartsWith("field=", StringComparison.Ordinal) || s.StartsWith("f=", StringComparison.Ordinal));
		if (text == null)
		{
			return "";
		}
		string component = text.Split('=')[1].Split(',')[0];
		return RealComponent(prefab, component);
	}

	private static List<string> GetFields(string component)
	{
		if (!Keys.TryGetValue(component, out List<string> value))
		{
			return new List<string>();
		}
		return value;
	}

	public static List<string> GetTypes(int index)
	{
		bool isTweaks = WorldEditCommands.IsTweaks;
		string input = GetInput();
		string prefab = PrefabFromCommand(input);
		string component = ComponentFromCommand(prefab, input);
		string field = FieldFromCommand(component, input);
		bool zdoField;
		string field2 = RealField(component, field, out zdoField);
		Type type = GetType(component, field2, zdoField);
		if (type == typeof(string))
		{
			return new List<string>(1) { "Text" };
		}
		if (type == typeof(int))
		{
			return new List<string>(1) { "Number" };
		}
		if (type == typeof(float))
		{
			return new List<string>(1) { "Decimal" };
		}
		if (type == typeof(long))
		{
			return new List<string>(1) { "Timestamp" };
		}
		if (type == typeof(bool))
		{
			return new List<string>(2) { "true", "false" };
		}
		if (type == typeof(Vector3))
		{
			return ParameterInfo.XZY("Field", index);
		}
		if (type == typeof(Quaternion))
		{
			return ParameterInfo.YXZ("Field", index);
		}
		if (type == typeof(GameObject))
		{
			if (!isTweaks)
			{
				return ParameterInfo.Ids;
			}
			return GetIdsOrTransforms(prefab, component, field);
		}
		if (type == typeof(ObjectHash))
		{
			return ParameterInfo.ObjectIds;
		}
		if (type == typeof(LocationHash))
		{
			return ParameterInfo.LocationIds;
		}
		if (type == typeof(RoomHash))
		{
			return ParameterInfo.RoomIds;
		}
		if (type == typeof(ItemDrop))
		{
			return ParameterInfo.ItemIds;
		}
		if (isTweaks && type.IsEnum)
		{
			string[] names = Enum.GetNames(type);
			List<string> list = new List<string>(names.Length);
			list.AddRange(names);
			return list;
		}
		if (isTweaks && type == typeof(EffectList))
		{
			return ParameterInfo.Ids;
		}
		return new List<string>();
	}

	private static List<string> GetIdsOrTransforms(string prefab, string component, string field)
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		string component2 = component;
		GameObject prefab2 = ZNetScene.instance.GetPrefab(prefab);
		if (!Object.op_Implicit((Object)(object)prefab2))
		{
			return ParameterInfo.Ids;
		}
		MonoBehaviour val = prefab2.GetComponents<MonoBehaviour>().FirstOrDefault((MonoBehaviour c) => ((object)c).GetType().Name == component2);
		if ((Object)(object)val == (Object)null)
		{
			return ParameterInfo.Ids;
		}
		bool zdoField;
		FieldInfo field2 = ((object)val).GetType().GetField(RealField(component2, field, out zdoField));
		if (field2 == null || field2.FieldType != typeof(GameObject))
		{
			return ParameterInfo.Ids;
		}
		GameObject val2 = (GameObject)field2.GetValue(val);
		if (Object.op_Implicit((Object)(object)val2) && (Object)(object)ZNetScene.instance.GetPrefab(((Object)val2).name) == (Object)(object)val2)
		{
			return ParameterInfo.Ids;
		}
		List<string> list = new List<string>();
		GetChild("/", prefab2.transform, list);
		return list;
	}

	private static void GetChild(string prefix, Transform tr, List<string> trs)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (Transform item in tr)
		{
			Transform val = item;
			trs.Add(prefix + ((Object)val).name);
			GetChild(prefix + ((Object)val).name + "/", val, trs);
		}
	}

	private static string FieldFromCommand(string component, string command)
	{
		string[] source = command.Split(' ');
		string text = source.LastOrDefault((string s) => s.StartsWith("field=", StringComparison.Ordinal) || s.StartsWith("f=", StringComparison.Ordinal));
		if (text == null)
		{
			return "";
		}
		source = text.Split('=')[1].Split(',');
		bool zdoField;
		if (source.Length >= 2)
		{
			return RealField(component, source[1], out zdoField);
		}
		return "";
	}

	public static Type GetType(string component, string field, bool zdoField)
	{
		if (zdoField)
		{
			if (!ZdoFields.TryGetValue("zdo", out Dictionary<string, Type> value))
			{
				return typeof(void);
			}
			if (!value.TryGetValue(field, out var value2))
			{
				return typeof(void);
			}
			return value2;
		}
		if (!Fields.TryGetValue(component, out Dictionary<string, Type> value3))
		{
			return typeof(void);
		}
		if (!value3.TryGetValue(field, out var value4))
		{
			return typeof(void);
		}
		return value4;
	}

	public static string RealComponent(string prefab, string component)
	{
		string component2 = component;
		List<string> components = GetComponents(prefab);
		return components.FirstOrDefault((string s) => s.Equals(component2, StringComparison.OrdinalIgnoreCase)) ?? components.FirstOrDefault((string s) => s.StartsWith(component2, StringComparison.OrdinalIgnoreCase)) ?? component2;
	}

	public static string RealField(string component, string field, out bool zdoField)
	{
		string field2 = field;
		zdoField = false;
		string f = "m_" + field2;
		Dictionary<string, Type> value;
		List<string> source = (Fields.TryGetValue(component, out value) ? value.Keys.ToList() : new List<string>());
		string text = source.FirstOrDefault((string s) => s.Equals(f, StringComparison.OrdinalIgnoreCase));
		if (text != null)
		{
			return text;
		}
		zdoField = true;
		Dictionary<string, Type> value2;
		List<string> source2 = (ZdoFields.TryGetValue(component, out value2) ? value2.Keys.ToList() : new List<string>());
		string text2 = source2.FirstOrDefault((string s) => s.Equals(field2, StringComparison.OrdinalIgnoreCase));
		if (text2 != null)
		{
			return text2;
		}
		zdoField = false;
		string text3 = source.FirstOrDefault((string s) => s.StartsWith(f, StringComparison.OrdinalIgnoreCase) || s.StartsWith(field2, StringComparison.OrdinalIgnoreCase));
		if (text3 != null)
		{
			return text3;
		}
		zdoField = true;
		string text4 = source2.FirstOrDefault((string s) => s.StartsWith(field2, StringComparison.OrdinalIgnoreCase));
		if (text4 != null)
		{
			return text4;
		}
		return field2;
	}

	private static void InitZdoFields()
	{
		if (ZdoFields.ContainsKey("zdo"))
		{
			return;
		}
		if (WorldEditCommands.IsCLLC)
		{
			Dictionary<string, Type> dictionary = ZdoFields["MonsterAI"];
			dictionary["CL&LC effect"] = typeof(Enum_CLLC_Effect);
			dictionary["CL&LC infusion"] = typeof(Enum_CLLC_Infusion);
		}
		Dictionary<string, Type> dictionary2 = new Dictionary<string, Type>();
		foreach (KeyValuePair<string, Dictionary<string, Type>> zdoField in ZdoFields)
		{
			foreach (KeyValuePair<string, Type> item in zdoField.Value)
			{
				dictionary2[item.Key] = item.Value;
			}
		}
		ZdoFields["zdo"] = dictionary2;
	}

	static FieldAutoComplete()
	{
		HashSet<Type> hashSet = new HashSet<Type>();
		foreach (Type validType in ValidTypes)
		{
			hashSet.Add(validType);
		}
		hashSet.Add(typeof(EffectList));
		ValidTweakTypes = hashSet;
		ZdoFields = new Dictionary<string, Dictionary<string, Type>>
		{
			{
				"ArmorStand",
				new Dictionary<string, Type>
				{
					{
						"pose",
						typeof(int)
					},
					{
						"0_item",
						typeof(GameObject)
					},
					{
						"1_item",
						typeof(GameObject)
					},
					{
						"2_item",
						typeof(GameObject)
					},
					{
						"0_variant",
						typeof(int)
					},
					{
						"1_variant",
						typeof(int)
					},
					{
						"2_variant",
						typeof(int)
					}
				}
			},
			{
				"AnimalAI",
				new Dictionary<string, Type>
				{
					{
						"aggravated",
						typeof(bool)
					},
					{
						"alert",
						typeof(bool)
					},
					{
						"bosscount",
						typeof(bool)
					},
					{
						"spawntime",
						typeof(long)
					},
					{
						"ShownAlertMessage",
						typeof(bool)
					},
					{
						"huntplayer",
						typeof(bool)
					},
					{
						"lastWorldTime",
						typeof(long)
					},
					{
						"spawnpoint",
						typeof(Vector3)
					},
					{
						"patrol",
						typeof(bool)
					},
					{
						"patrolPoint",
						typeof(Vector3)
					}
				}
			},
			{
				"Bed",
				new Dictionary<string, Type>
				{
					{
						"owner",
						typeof(long)
					},
					{
						"ownerName",
						typeof(string)
					}
				}
			},
			{
				"Beehive",
				new Dictionary<string, Type>
				{
					{
						"lastTime",
						typeof(long)
					},
					{
						"level",
						typeof(int)
					},
					{
						"product",
						typeof(float)
					}
				}
			},
			{
				"Catapult",
				new Dictionary<string, Type>
				{
					{
						"Locked",
						typeof(bool)
					},
					{
						"visual",
						typeof(GameObject)
					}
				}
			},
			{
				"CharacterAnimEvent",
				new Dictionary<string, Type> { 
				{
					"LookTarget",
					typeof(Vector3)
				} }
			},
			{
				"Cinder",
				new Dictionary<string, Type> { 
				{
					"spread",
					typeof(bool)
				} }
			},
			{
				"CinderSpawner",
				new Dictionary<string, Type> { 
				{
					"spread",
					typeof(bool)
				} }
			},
			{
				"Container",
				new Dictionary<string, Type>
				{
					{
						"addedDefaultItems",
						typeof(bool)
					},
					{
						"InUse",
						typeof(bool)
					},
					{
						"items",
						typeof(string)
					}
				}
			},
			{
				"CookingStation",
				new Dictionary<string, Type>
				{
					{
						"fuel",
						typeof(float)
					},
					{
						"StartTime",
						typeof(long)
					},
					{
						"slot0",
						typeof(GameObject)
					},
					{
						"slot1",
						typeof(GameObject)
					},
					{
						"slot2",
						typeof(GameObject)
					},
					{
						"slotstatus0",
						typeof(int)
					},
					{
						"slotstatus1",
						typeof(int)
					},
					{
						"slotstatus2",
						typeof(int)
					}
				}
			},
			{
				"Corpse",
				new Dictionary<string, Type> { 
				{
					"timeOfDeath",
					typeof(long)
				} }
			},
			{
				"CreatureSpawner",
				new Dictionary<string, Type> { 
				{
					"alive_time",
					typeof(long)
				} }
			},
			{
				"DungeonGenerator",
				new Dictionary<string, Type>
				{
					{
						"rooms",
						typeof(int)
					},
					{
						"room0",
						typeof(RoomHash)
					},
					{
						"room1",
						typeof(RoomHash)
					},
					{
						"room2",
						typeof(RoomHash)
					},
					{
						"room0_pos",
						typeof(Vector3)
					},
					{
						"room1_pos",
						typeof(Vector3)
					},
					{
						"room2_pos",
						typeof(Vector3)
					},
					{
						"room0_rot",
						typeof(Quaternion)
					},
					{
						"room1_rot",
						typeof(Quaternion)
					},
					{
						"room2_rot",
						typeof(Quaternion)
					}
				}
			},
			{
				"Destructible",
				new Dictionary<string, Type> { 
				{
					"health",
					typeof(float)
				} }
			},
			{
				"Door",
				new Dictionary<string, Type> { 
				{
					"state",
					typeof(int)
				} }
			},
			{
				"EggGrow",
				new Dictionary<string, Type> { 
				{
					"GrowStart",
					typeof(float)
				} }
			},
			{
				"Fermenter",
				new Dictionary<string, Type>
				{
					{
						"Content",
						typeof(GameObject)
					},
					{
						"StartTime",
						typeof(long)
					}
				}
			},
			{
				"Fireplace",
				new Dictionary<string, Type>
				{
					{
						"fuel",
						typeof(float)
					},
					{
						"lastTime",
						typeof(long)
					},
					{
						"state",
						typeof(bool)
					}
				}
			},
			{
				"Fish",
				new Dictionary<string, Type>
				{
					{
						"escape",
						typeof(float)
					},
					{
						"hooked",
						typeof(bool)
					},
					{
						"spawnpoint",
						typeof(Vector3)
					}
				}
			},
			{
				"FishingFloat",
				new Dictionary<string, Type>
				{
					{
						"Bait",
						typeof(GameObject)
					},
					{
						"rodOwner",
						typeof(long)
					}
				}
			},
			{
				"Gibber",
				new Dictionary<string, Type>
				{
					{
						"HitPoint",
						typeof(Vector3)
					},
					{
						"HitDir",
						typeof(Vector3)
					}
				}
			},
			{
				"Humanoid",
				new Dictionary<string, Type>
				{
					{
						"BodyVelocity",
						typeof(Vector3)
					},
					{
						"bosscount",
						typeof(bool)
					},
					{
						"health",
						typeof(float)
					},
					{
						"IsBlocking",
						typeof(bool)
					},
					{
						"level",
						typeof(int)
					},
					{
						"max_health",
						typeof(float)
					},
					{
						"noise",
						typeof(float)
					},
					{
						"RandomSkillFactor",
						typeof(float)
					},
					{
						"seed",
						typeof(int)
					},
					{
						"tamed",
						typeof(bool)
					}
				}
			},
			{
				"ItemDrop",
				new Dictionary<string, Type>
				{
					{
						"crafterID",
						typeof(long)
					},
					{
						"crafterName",
						typeof(string)
					},
					{
						"data_0",
						typeof(string)
					},
					{
						"data_1",
						typeof(string)
					},
					{
						"data_2",
						typeof(string)
					},
					{
						"data__0",
						typeof(string)
					},
					{
						"data__1",
						typeof(string)
					},
					{
						"data__2",
						typeof(string)
					},
					{
						"durability",
						typeof(float)
					},
					{
						"pickedUp",
						typeof(bool)
					},
					{
						"piece",
						typeof(bool)
					},
					{
						"quality",
						typeof(int)
					},
					{
						"spawntime",
						typeof(long)
					},
					{
						"stack",
						typeof(int)
					},
					{
						"variant",
						typeof(int)
					},
					{
						"worldLevel",
						typeof(int)
					}
				}
			},
			{
				"ItemStand",
				new Dictionary<string, Type>
				{
					{
						"quality",
						typeof(int)
					},
					{
						"item",
						typeof(GameObject)
					},
					{
						"variant",
						typeof(int)
					}
				}
			},
			{
				"Leviathan",
				new Dictionary<string, Type> { 
				{
					"dead",
					typeof(bool)
				} }
			},
			{
				"LineConnect",
				new Dictionary<string, Type> { 
				{
					"line_slack",
					typeof(float)
				} }
			},
			{
				"LocationProxy",
				new Dictionary<string, Type>
				{
					{
						"location",
						typeof(LocationHash)
					},
					{
						"seed",
						typeof(int)
					}
				}
			},
			{
				"LootSpawner",
				new Dictionary<string, Type> { 
				{
					"spawntime",
					typeof(long)
				} }
			},
			{
				"MaterialVariation",
				new Dictionary<string, Type>
				{
					{
						"MatVar0",
						typeof(int)
					},
					{
						"MatVar1",
						typeof(int)
					},
					{
						"MatVar2",
						typeof(int)
					}
				}
			},
			{
				"MineRock",
				new Dictionary<string, Type>
				{
					{
						"Health0",
						typeof(float)
					},
					{
						"Health1",
						typeof(float)
					},
					{
						"Health2",
						typeof(float)
					}
				}
			},
			{
				"MineRock5",
				new Dictionary<string, Type> { 
				{
					"health",
					typeof(float)
				} }
			},
			{
				"MonsterAI",
				new Dictionary<string, Type>
				{
					{
						"aggravated",
						typeof(bool)
					},
					{
						"alert",
						typeof(bool)
					},
					{
						"bosscount",
						typeof(bool)
					},
					{
						"DespawnInDay",
						typeof(bool)
					},
					{
						"EventCreature",
						typeof(bool)
					},
					{
						"huntplayer",
						typeof(bool)
					},
					{
						"lastWorldTime",
						typeof(long)
					},
					{
						"patrol",
						typeof(bool)
					},
					{
						"patrolPoint",
						typeof(Vector3)
					},
					{
						"ShownAlertMessage",
						typeof(bool)
					},
					{
						"sleeping",
						typeof(bool)
					},
					{
						"spawnpoint",
						typeof(Vector3)
					},
					{
						"spawntime",
						typeof(long)
					}
				}
			},
			{
				"MusicLocation",
				new Dictionary<string, Type> { 
				{
					"played",
					typeof(bool)
				} }
			},
			{
				"Pickable",
				new Dictionary<string, Type>
				{
					{
						"picked",
						typeof(bool)
					},
					{
						"picked_time",
						typeof(long)
					}
				}
			},
			{
				"PickableItem",
				new Dictionary<string, Type>
				{
					{
						"itemPrefab",
						typeof(ObjectHash)
					},
					{
						"itemStack",
						typeof(int)
					}
				}
			},
			{
				"Piece",
				new Dictionary<string, Type> { 
				{
					"creator",
					typeof(long)
				} }
			},
			{
				"Plant",
				new Dictionary<string, Type>
				{
					{
						"seed",
						typeof(int)
					},
					{
						"plantTime",
						typeof(long)
					}
				}
			},
			{
				"Player",
				new Dictionary<string, Type>
				{
					{
						"baseValue",
						typeof(int)
					},
					{
						"BodyVelocity",
						typeof(Vector3)
					},
					{
						"DebugFly",
						typeof(bool)
					},
					{
						"dodgeinv",
						typeof(bool)
					},
					{
						"dead",
						typeof(bool)
					},
					{
						"health",
						typeof(float)
					},
					{
						"IsBlocking",
						typeof(bool)
					},
					{
						"eitr",
						typeof(float)
					},
					{
						"emote",
						typeof(string)
					},
					{
						"emoteID",
						typeof(int)
					},
					{
						"emote_oneshot",
						typeof(bool)
					},
					{
						"inBed",
						typeof(bool)
					},
					{
						"noise",
						typeof(float)
					},
					{
						"playerID",
						typeof(long)
					},
					{
						"playerName",
						typeof(string)
					},
					{
						"pvp",
						typeof(bool)
					},
					{
						"stamina",
						typeof(float)
					},
					{
						"Stealth",
						typeof(float)
					},
					{
						"wakeup",
						typeof(bool)
					},
					{
						"WeaponLoaded",
						typeof(bool)
					}
				}
			},
			{
				"PrivateArea",
				new Dictionary<string, Type>
				{
					{
						"creatorName",
						typeof(string)
					},
					{
						"enabled",
						typeof(bool)
					},
					{
						"permitted",
						typeof(int)
					},
					{
						"pu_id0",
						typeof(long)
					},
					{
						"pu_id1",
						typeof(long)
					},
					{
						"pu_id2",
						typeof(long)
					},
					{
						"pu_name0",
						typeof(string)
					},
					{
						"pu_name1",
						typeof(string)
					},
					{
						"pu_name2",
						typeof(string)
					}
				}
			},
			{
				"Procreation",
				new Dictionary<string, Type>
				{
					{
						"lovePoints",
						typeof(int)
					},
					{
						"pregnant",
						typeof(long)
					}
				}
			},
			{
				"Projectile",
				new Dictionary<string, Type> { 
				{
					"visual",
					typeof(string)
				} }
			},
			{
				"Ragdoll",
				new Dictionary<string, Type>
				{
					{
						"drops",
						typeof(int)
					},
					{
						"drop_hash0",
						typeof(ObjectHash)
					},
					{
						"drop_hash1",
						typeof(ObjectHash)
					},
					{
						"drop_hash2",
						typeof(ObjectHash)
					},
					{
						"drop_amount0",
						typeof(float)
					},
					{
						"drop_amount1",
						typeof(float)
					},
					{
						"drop_amount2",
						typeof(float)
					},
					{
						"Hue",
						typeof(float)
					},
					{
						"InitVel",
						typeof(Vector3)
					},
					{
						"Saturation",
						typeof(float)
					},
					{
						"Value",
						typeof(float)
					}
				}
			},
			{
				"RandomFlyingBird",
				new Dictionary<string, Type>
				{
					{
						"landed",
						typeof(bool)
					},
					{
						"spawnpoint",
						typeof(Vector3)
					}
				}
			},
			{
				"ResourceRoot",
				new Dictionary<string, Type>
				{
					{
						"lastTime",
						typeof(long)
					},
					{
						"level",
						typeof(float)
					}
				}
			},
			{
				"Sadle",
				new Dictionary<string, Type> { 
				{
					"user",
					typeof(long)
				} }
			},
			{
				"SapCollector",
				new Dictionary<string, Type>
				{
					{
						"lastTime",
						typeof(long)
					},
					{
						"level",
						typeof(int)
					},
					{
						"product",
						typeof(float)
					}
				}
			},
			{
				"SEMan",
				new Dictionary<string, Type> { 
				{
					"seAttrib",
					typeof(int)
				} }
			},
			{
				"ShieldGenerator",
				new Dictionary<string, Type>
				{
					{
						"fuel",
						typeof(float)
					},
					{
						"StartTime",
						typeof(long)
					}
				}
			},
			{
				"Ship",
				new Dictionary<string, Type>
				{
					{
						"forward",
						typeof(int)
					},
					{
						"rudder",
						typeof(float)
					}
				}
			},
			{
				"ShipConstructor",
				new Dictionary<string, Type>
				{
					{
						"done",
						typeof(bool)
					},
					{
						"spawntime",
						typeof(long)
					}
				}
			},
			{
				"Sign",
				new Dictionary<string, Type>
				{
					{
						"author",
						typeof(string)
					},
					{
						"text",
						typeof(string)
					}
				}
			},
			{
				"Smelter",
				new Dictionary<string, Type>
				{
					{
						"accTime",
						typeof(float)
					},
					{
						"bakeTimer",
						typeof(float)
					},
					{
						"fuel",
						typeof(float)
					},
					{
						"item0",
						typeof(GameObject)
					},
					{
						"item1",
						typeof(GameObject)
					},
					{
						"item2",
						typeof(GameObject)
					},
					{
						"queued",
						typeof(int)
					},
					{
						"SpawnOre",
						typeof(GameObject)
					},
					{
						"SpawnAmount",
						typeof(int)
					},
					{
						"StartTime",
						typeof(long)
					}
				}
			},
			{
				"Tameable",
				new Dictionary<string, Type>
				{
					{
						"HaveSaddle",
						typeof(bool)
					},
					{
						"TamedName",
						typeof(string)
					},
					{
						"TamedNameAuthor",
						typeof(string)
					}
				}
			},
			{
				"TeleportWorld",
				new Dictionary<string, Type>
				{
					{
						"tag",
						typeof(string)
					},
					{
						"tagauthor",
						typeof(string)
					}
				}
			},
			{
				"Trap",
				new Dictionary<string, Type>
				{
					{
						"state",
						typeof(int)
					},
					{
						"triggered",
						typeof(float)
					}
				}
			},
			{
				"TreeBase",
				new Dictionary<string, Type> { 
				{
					"health",
					typeof(float)
				} }
			},
			{
				"TreeLog",
				new Dictionary<string, Type> { 
				{
					"health",
					typeof(float)
				} }
			},
			{
				"TriggerSpawner",
				new Dictionary<string, Type> { 
				{
					"spawntime",
					typeof(long)
				} }
			},
			{
				"Turret",
				new Dictionary<string, Type>
				{
					{
						"ammo",
						typeof(int)
					},
					{
						"ammoType",
						typeof(GameObject)
					},
					{
						"lastAttack",
						typeof(float)
					},
					{
						"targets",
						typeof(int)
					},
					{
						"target0",
						typeof(string)
					},
					{
						"target1",
						typeof(string)
					},
					{
						"target2",
						typeof(string)
					}
				}
			},
			{
				"Vagon",
				new Dictionary<string, Type> { 
				{
					"attachJoint",
					typeof(bool)
				} }
			},
			{
				"WearNTear",
				new Dictionary<string, Type>
				{
					{
						"health",
						typeof(float)
					},
					{
						"support",
						typeof(float)
					}
				}
			},
			{
				"WispSpawner",
				new Dictionary<string, Type> { 
				{
					"LastSpawn",
					typeof(long)
				} }
			}
		};
	}
}
