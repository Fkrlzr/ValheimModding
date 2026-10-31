using System.Collections.Generic;
using System.Linq;
using Common;
using Service;
using UnityEngine;

namespace Data;

public static class PrefabHelper
{
	private static readonly Dictionary<string, List<int>> ResultCache = new Dictionary<string, List<int>>();

	private static Dictionary<string, int> PrefabCache = new Dictionary<string, int>();

	public static void ClearCache()
	{
		ResultCache.Clear();
		PrefabCache.Clear();
	}

	public static List<int> GetPrefabs(string include, string exclude)
	{
		string key = include + "|" + exclude;
		if (ResultCache.TryGetValue(key, out List<int> value))
		{
			return value;
		}
		List<string> includes = Parse.ToList(include);
		List<string> excludes = ((exclude == "") ? null : Parse.ToList(exclude));
		List<int> prefabs = GetPrefabs(includes, excludes);
		if (prefabs == null)
		{
			return new List<int>();
		}
		ResultCache[key] = prefabs;
		return prefabs;
	}

	public static List<int>? GetPrefabs(List<string> includes, List<string>? excludes)
	{
		if (includes.Count == 0)
		{
			return null;
		}
		HashSet<int> excludedPrefabs = null;
		if (excludes != null && excludes.Count > 0)
		{
			HashSet<int> hashSet = new HashSet<int>();
			foreach (int item in (from s in excludes.Select(ParsePrefabs)
				where s != null
				select s).SelectMany((List<int> s) => s))
			{
				hashSet.Add(item);
			}
			excludedPrefabs = hashSet;
		}
		if (includes.Count == 1)
		{
			List<int> list = ParsePrefabs(includes[0]);
			if (list == null)
			{
				return null;
			}
			if (excludedPrefabs != null)
			{
				List<int> list2 = new List<int>();
				list2.AddRange(list.Where((int i) => !excludedPrefabs.Contains(i)));
				list = list2;
			}
			return list;
		}
		HashSet<int> hashSet2 = new HashSet<int>();
		foreach (List<int> item2 in includes.Select(ParsePrefabs))
		{
			if (item2 == null)
			{
				continue;
			}
			foreach (int item3 in item2)
			{
				if (excludedPrefabs == null || !excludedPrefabs.Contains(item3))
				{
					hashSet2.Add(item3);
				}
			}
		}
		if (hashSet2.Count != 0)
		{
			HashSet<int> hashSet = hashSet2;
			List<int> list3 = new List<int>(hashSet.Count);
			list3.AddRange(hashSet);
			return list3;
		}
		return null;
	}

	private static void EnsurePrefabCache()
	{
		if (PrefabCache.Count == 0)
		{
			PrefabCache = ZNetScene.instance.m_namedPrefabs.ToDictionary((KeyValuePair<int, GameObject> pair) => ((Object)pair.Value).name, (KeyValuePair<int, GameObject> pair) => pair.Key);
		}
	}

	private static List<int>? ParsePrefabs(string prefab)
	{
		string prefab2 = prefab;
		EnsurePrefabCache();
		if (Wildcard.IsPattern(prefab2))
		{
			List<int> list = new List<int>();
			list.AddRange(from pair in PrefabCache
				where Wildcard.Match(pair.Key, prefab2, ignoreCase: true)
				select pair.Value);
			return list;
		}
		if (PrefabCache.TryGetValue(prefab2, out var value))
		{
			return new List<int>(1) { value };
		}
		if (ValueGroups.TryGet(prefab2, out List<string> values))
		{
			List<int> list2 = new List<int>();
			list2.AddRange(values.Select((string s) => StringExtensionMethods.GetStableHashCode(s)));
			return list2;
		}
		Log.Warning("Failed to resolve prefab: " + prefab2);
		return null;
	}
}
