using System.Collections.Generic;
using UnityEngine;

namespace SkToolbox.Utility;

internal static class SkIcons
{
	private static readonly Dictionary<string, Sprite> cache = new Dictionary<string, Sprite>();

	private static int cachedFor = 0;

	internal static Sprite Item(string prefabName)
	{
		if (string.IsNullOrEmpty(prefabName))
		{
			return null;
		}
		ObjectDB instance = ObjectDB.instance;
		if ((Object)(object)instance == (Object)null)
		{
			return null;
		}
		int instanceID = ((Object)instance).GetInstanceID();
		if (cachedFor != instanceID)
		{
			cache.Clear();
			cachedFor = instanceID;
		}
		if (cache.TryGetValue(prefabName, out var value))
		{
			return value;
		}
		Sprite val = null;
		GameObject val2 = default(GameObject);
		if (instance.TryGetItemPrefab(prefabName, out val2) && (Object)(object)val2 != (Object)null)
		{
			ItemDrop component = val2.GetComponent<ItemDrop>();
			if ((Object)(object)component != (Object)null && component.m_itemData != null && component.m_itemData.m_shared != null)
			{
				Sprite[] icons = component.m_itemData.m_shared.m_icons;
				if (icons != null && icons.Length != 0)
				{
					val = icons[0];
				}
			}
		}
		cache[prefabName] = val;
		return val;
	}

	internal static Sprite First(params string[] prefabNames)
	{
		if (prefabNames == null)
		{
			return null;
		}
		foreach (string prefabName in prefabNames)
		{
			Sprite val = Item(prefabName);
			if ((Object)(object)val != (Object)null)
			{
				return val;
			}
		}
		return null;
	}
}
