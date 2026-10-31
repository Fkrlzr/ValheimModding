using System.Collections.Generic;
using UnityEngine;

namespace Data;

public class PrefabValue : DynamicValue, IPrefabValue
{
	private List<int>? Cache;

	private Functions? LastFunctions;

	public PrefabValue(string[] values)
		: base(values)
	{
	}

	public int? Get(Functions f)
	{
		if (f != LastFunctions)
		{
			List<string> allValues = GetAllValues(f);
			Cache = PrefabHelper.GetPrefabs(allValues, null);
			LastFunctions = f;
		}
		if (Cache == null || Cache.Count == 0)
		{
			return null;
		}
		if (Cache.Count == 1)
		{
			return Cache[0];
		}
		return Cache[Random.Range(0, Cache.Count)];
	}

	public bool? Match(Functions f, int value)
	{
		if (f != LastFunctions)
		{
			List<string> allValues = GetAllValues(f);
			Cache = PrefabHelper.GetPrefabs(allValues, null);
			LastFunctions = f;
		}
		if (Cache == null || Cache.Count == 0)
		{
			return null;
		}
		return Cache.Contains(value);
	}
}
