using System;
using System.Collections.Generic;
using Common;
using Service;
using UnityEngine;

namespace Data;

public static class ValueGroups
{
	private static readonly Dictionary<int, List<string>> Groups = new Dictionary<int, List<string>>();

	private static readonly Dictionary<int, List<string>> DefaultGroups = new Dictionary<int, List<string>>();

	private static readonly Dictionary<string, List<string>> ComponentsByPrefab = new Dictionary<string, List<string>>();

	public static int Count => Groups.Count;

	public static int Hash(string group)
	{
		return StringExtensionMethods.GetStableHashCode(group.ToLowerInvariant());
	}

	public static bool Contains(int hash)
	{
		return Groups.ContainsKey(hash);
	}

	public static bool TryGet(int hash, out List<string> values)
	{
		return Groups.TryGetValue(hash, out values);
	}

	public static bool TryGet(string group, out List<string> values)
	{
		return Groups.TryGetValue(Hash(group), out values);
	}

	public static bool IsComponentGroup(string group)
	{
		return DefaultGroups.ContainsKey(Hash(group));
	}

	public static List<string> GetComponents(string prefabName)
	{
		if (!ComponentsByPrefab.TryGetValue(prefabName, out List<string> value))
		{
			return new List<string>();
		}
		return value;
	}

	public static bool TryGetRandom(string group, out string value)
	{
		if (!TryGet(group, out List<string> values) || values.Count == 0)
		{
			value = group;
			return false;
		}
		value = values[Random.Range(0, values.Count)];
		return true;
	}

	public static void Clear()
	{
		Groups.Clear();
	}

	public static void Add(DataYaml data, string? file = null)
	{
		if (data.value != null)
		{
			KeyValuePair<string, string> keyValuePair = Parse.Kvp(data.value);
			Add(keyValuePair.Key, new _003C_003Ez__ReadOnlySingleElementList<string>(keyValuePair.Value), file);
		}
		if (data.valueGroup != null && data.values != null)
		{
			Add(data.valueGroup, data.values, file);
		}
	}

	private static void Add(string group, IEnumerable<string> values, string? file)
	{
		int key = Hash(group);
		if (!Groups.TryGetValue(key, out List<string> value))
		{
			value = (Groups[key] = new List<string>());
		}
		else
		{
			Log.Warning((file == null) ? ("Duplicate value group entry: " + group) : ("Duplicate value group entry: " + group + " at " + file));
		}
		value.AddRange(values);
	}

	public static void Resolve()
	{
		LoadDefaults();
		foreach (List<string> value in Groups.Values)
		{
			ResolveNested(value);
		}
		foreach (KeyValuePair<int, List<string>> defaultGroup in DefaultGroups)
		{
			if (!Groups.ContainsKey(defaultGroup.Key))
			{
				Groups[defaultGroup.Key] = defaultGroup.Value;
			}
		}
	}

	private static void ResolveNested(List<string> values)
	{
		for (int i = 0; i < values.Count; i++)
		{
			string text = values[i];
			if (text.StartsWith("<", StringComparison.Ordinal) && text.EndsWith(">", StringComparison.Ordinal))
			{
				int key = Hash(text.Substring(1, text.Length - 2));
				List<string> value2;
				if (Groups.TryGetValue(key, out List<string> value))
				{
					values.RemoveAt(i);
					values.InsertRange(i, value);
					i--;
				}
				else if (DefaultGroups.TryGetValue(key, out value2))
				{
					values.RemoveAt(i);
					values.InsertRange(i, value2);
					i += value2.Count - 1;
				}
			}
		}
	}

	private static void LoadDefaults()
	{
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		if (DefaultGroups.Count > 0 || !Object.op_Implicit((Object)(object)ZNetScene.instance))
		{
			return;
		}
		ComponentsByPrefab.Clear();
		foreach (GameObject value2 in ZNetScene.instance.m_namedPrefabs.Values)
		{
			if (!Object.op_Implicit((Object)(object)value2))
			{
				continue;
			}
			string name = ((Object)value2).name;
			if (!ComponentsByPrefab.TryGetValue(name, out List<string> value))
			{
				value = (ComponentsByPrefab[name] = new List<string>());
			}
			HashSet<int> added = new HashSet<int>();
			value2.GetComponentsInChildren<MonoBehaviour>(ZNetView.m_tempComponents);
			foreach (MonoBehaviour tempComponent in ZNetView.m_tempComponents)
			{
				if (Object.op_Implicit((Object)(object)tempComponent))
				{
					string name2 = ((object)tempComponent).GetType().Name;
					AddDefault(added, name2, name);
					string text = name2.ToLowerInvariant();
					AddName(value, text);
					if (text == "humanoid")
					{
						AddName(value, "character");
					}
					if (text == "wearntear")
					{
						AddName(value, "structure");
					}
					WearNTear val = (WearNTear)(object)((tempComponent is WearNTear) ? tempComponent : null);
					if (val != null)
					{
						AddDefault(added, $"material_{val.m_materialType}", name);
					}
					ItemDrop val2 = (ItemDrop)(object)((tempComponent is ItemDrop) ? tempComponent : null);
					if (val2 != null)
					{
						AddDefault(added, $"itemtype_{val2.m_itemData.m_shared.m_itemType}", name);
					}
				}
			}
		}
		Alias("creature", "humanoid");
		Alias("structure", "wearntear");
	}

	private static void AddName(List<string> names, string name)
	{
		if (!names.Contains(name))
		{
			names.Add(name);
		}
	}

	private static void AddDefault(HashSet<int> added, string group, string prefabName)
	{
		int num = Hash(group.Replace(" ", "_"));
		if (added.Add(num))
		{
			if (!DefaultGroups.TryGetValue(num, out List<string> value))
			{
				value = (DefaultGroups[num] = new List<string>());
			}
			value.Add(prefabName);
		}
	}

	private static void Alias(string alias, string group)
	{
		if (DefaultGroups.TryGetValue(Hash(group), out List<string> value))
		{
			DefaultGroups[Hash(alias)] = value;
		}
	}
}
