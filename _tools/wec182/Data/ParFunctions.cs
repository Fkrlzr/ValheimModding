using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Data;

public class ParFunctions : Functions
{
	[CompilerGenerated]
	private Dictionary<string, string> _003Cpars_003EP;

	private readonly ObjectFunctions? objectFunctions;

	public ParFunctions(Dictionary<string, string> pars, ZDO? zdo)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		_003Cpars_003EP = pars;
		objectFunctions = ((zdo == null) ? null : new ObjectFunctions(GetPrefabName(zdo), Array.Empty<string>(), zdo));
		base._002Ector(GetPrefabName(zdo), Array.Empty<string>(), zdo?.m_position ?? Vector3.zero);
	}

	private static string GetPrefabName(ZDO? zdo)
	{
		object obj;
		if (zdo != null)
		{
			GameObject prefab = ZNetScene.instance.GetPrefab(zdo.m_prefab);
			obj = ((prefab != null) ? ((Object)prefab).name : null);
			if (obj == null)
			{
				return "";
			}
		}
		else
		{
			obj = "";
		}
		return (string)obj;
	}

	protected override string? GetFunction(string key, string defaultValue)
	{
		if (_003Cpars_003EP.TryGetValue("<" + key + ">", out string value))
		{
			return value;
		}
		if (ValueGroups.TryGetRandom(key, out value))
		{
			return value;
		}
		if (objectFunctions == null)
		{
			return base.GetFunction(key, defaultValue);
		}
		return objectFunctions.Resolve(key, defaultValue);
	}
}
