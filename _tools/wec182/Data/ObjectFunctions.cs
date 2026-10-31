using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Common;
using UnityEngine;

namespace Data;

public class ObjectFunctions : Functions
{
	[CompilerGenerated]
	private ZDO _003Czdo_003EP;

	private List<ItemRecord>? inventory;

	public ObjectFunctions(string prefab, string[] args, ZDO zdo)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		_003Czdo_003EP = zdo;
		base._002Ector(prefab, args, _003Czdo_003EP.m_position);
	}

	public string? Resolve(string key, string defaultValue)
	{
		return GetFunction(key, defaultValue);
	}

	protected override string? GetFunction(string key, string defaultValue)
	{
		string function = base.GetFunction(key, defaultValue);
		if (function != null)
		{
			return function;
		}
		return GetObjectFunction(key);
	}

	protected override string? GetValueFunction(string key, string value, string defaultValue)
	{
		string objectValueFunction = GetObjectValueFunction(key, value, defaultValue);
		if (objectValueFunction != null)
		{
			return objectValueFunction;
		}
		return objectValueFunction ?? base.GetValueFunction(key, value, defaultValue);
	}

	private string? GetObjectFunction(string key)
	{
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		switch (key)
		{
		case "zdo":
			return ((object)(ZDOID)(ref _003Czdo_003EP.m_uid)).ToString();
		case "pos":
			return Formatting.FormatPos(_003Czdo_003EP.m_position);
		case "i":
			return ZoneSystem.GetZone(_003Czdo_003EP.m_position).x.ToString();
		case "j":
			return ZoneSystem.GetZone(_003Czdo_003EP.m_position).y.ToString();
		case "a":
			return Formatting.Format(_003Czdo_003EP.m_rotation.y);
		case "rad":
			return Formatting.Format(_003Czdo_003EP.m_rotation.y * ((float)Math.PI / 180f));
		case "deg":
			return Formatting.Format(_003Czdo_003EP.m_rotation.y);
		case "rot":
			return Formatting.FormatRot(_003Czdo_003EP.m_rotation);
		case "owner":
			return _003Czdo_003EP.GetOwner().ToString();
		case "connected":
		{
			ZDOID val = _003Czdo_003EP.GetConnection()?.m_target ?? ZDOID.None;
			return ((object)(ZDOID)(ref val)).ToString();
		}
		case "biome":
		{
			Biome biome = WorldGenerator.instance.GetBiome(_003Czdo_003EP.m_position);
			return ((object)(Biome)(ref biome)).ToString();
		}
		case "altbiome":
			return GetAltBiome();
		case "joints":
			return GetJoints();
		default:
			return null;
		}
	}

	private string? GetObjectValueFunction(string key, string value, string defaultValue)
	{
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		switch (key)
		{
		case "string":
			return ZdoHelper.GetString(_003Czdo_003EP, value, defaultValue);
		case "float":
			return ZdoHelper.GetFloat(_003Czdo_003EP, value, defaultValue).ToString(CultureInfo.InvariantCulture);
		case "int":
			return ZdoHelper.GetInt(_003Czdo_003EP, value, defaultValue).ToString(CultureInfo.InvariantCulture);
		case "long":
			return ZdoHelper.GetLong(_003Czdo_003EP, value, defaultValue).ToString(CultureInfo.InvariantCulture);
		case "bool":
			return ZdoHelper.GetBool(_003Czdo_003EP, value, defaultValue) ? "true" : "false";
		case "hash":
			return GetHash(value, defaultValue);
		case "vec":
			return Formatting.FormatPos(ZdoHelper.GetVec(_003Czdo_003EP, value, defaultValue));
		case "quat":
		{
			Quaternion quaternion = ZdoHelper.GetQuaternion(_003Czdo_003EP, value, defaultValue);
			return Formatting.FormatRot(((Quaternion)(ref quaternion)).eulerAngles);
		}
		case "byte":
			return ZdoHelper.GetBytes(_003Czdo_003EP, value, defaultValue);
		case "zdo":
		{
			ZDOID zDOID = _003Czdo_003EP.GetZDOID(value);
			return ((object)(ZDOID)(ref zDOID)).ToString();
		}
		case "amount":
			return GetAmount(value, defaultValue);
		case "quality":
			return GetQuality(value, defaultValue);
		case "durability":
			return GetDurability(value, defaultValue);
		case "item":
			return GetItem(value, defaultValue);
		case "pos":
			return Formatting.FormatPos(GetPos(value));
		default:
			return null;
		}
	}

	private string GetAltBiome()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		WorldGenerator instance = WorldGenerator.instance;
		return GetBiomeName(instance.GetBiome(_003Czdo_003EP.m_position), instance.GetBiomeSector(_003Czdo_003EP.m_position, false).AltBiomes);
	}

	private static string GetBiomeName(Biome biome, List<AltBiome> altBiomes)
	{
		if (altBiomes.Count == 0)
		{
			return ((object)(Biome)(ref biome)).ToString();
		}
		string[] array = (from alt in altBiomes
			select alt.m_name into name
			where !string.IsNullOrWhiteSpace(name)
			select name).Distinct().OrderBy((string name) => name, StringComparer.Ordinal).ToArray();
		if (array.Length == 0)
		{
			return ((object)(Biome)(ref biome)).ToString();
		}
		return string.Join(", ", array);
	}

	private string GetJoints()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		GameObject prefab = ZNetScene.instance.GetPrefab(_003Czdo_003EP.m_prefab);
		if ((Object)(object)prefab == (Object)null)
		{
			return "";
		}
		Queue<Transform> queue = new Queue<Transform>();
		queue.Enqueue(prefab.transform);
		List<string> list = new List<string>();
		while (queue.Count > 0)
		{
			foreach (Transform item in queue.Dequeue())
			{
				Transform val = item;
				queue.Enqueue(val);
				list.Add(((Object)val).name);
			}
		}
		return string.Join(", ", list);
	}

	private string GetHash(string value, string defaultValue)
	{
		if (value == "")
		{
			return defaultValue;
		}
		int @int = _003Czdo_003EP.GetInt(value, 0);
		GameObject prefab = ZNetScene.instance.GetPrefab(@int);
		return ((prefab != null) ? ((Object)prefab).name : null) ?? ZoneSystem.instance.GetLocation(@int)?.m_prefabName ?? defaultValue;
	}

	private Vector3 GetPos(string value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = Parse.VectorXZY(value);
		return _003Czdo_003EP.GetPosition() + _003Czdo_003EP.GetRotation() * val;
	}

	private string GetItem(string value, string defaultValue)
	{
		if (value == "")
		{
			return defaultValue;
		}
		if (!TryGetCoordinates(value, out var x, out var y))
		{
			return GetAmountOfItems(value).ToString();
		}
		return GetItemAt(x, y)?.PrefabName ?? defaultValue;
	}

	private string GetAmount(string value, string defaultValue)
	{
		if (value == "")
		{
			return defaultValue;
		}
		if (!TryGetCoordinates(value, out var x, out var y))
		{
			return GetAmountOfItems(value).ToString();
		}
		return GetItemAt(x, y)?.Stack.ToString() ?? defaultValue;
	}

	private string GetDurability(string value, string defaultValue)
	{
		if (!TryGetCoordinates(value, out var x, out var y))
		{
			return defaultValue;
		}
		return GetItemAt(x, y)?.Durability.ToString() ?? defaultValue;
	}

	private string GetQuality(string value, string defaultValue)
	{
		if (!TryGetCoordinates(value, out var x, out var y))
		{
			return defaultValue;
		}
		return GetItemAt(x, y)?.Quality.ToString() ?? defaultValue;
	}

	private static bool TryGetCoordinates(string value, out int x, out int y)
	{
		x = 0;
		y = 0;
		if (value == "")
		{
			return false;
		}
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, '_');
		if (Parse.TryInt(keyValuePair.Key, out x))
		{
			return Parse.TryInt(keyValuePair.Value, out y);
		}
		return false;
	}

	private int GetAmountOfItems(string name)
	{
		string name2 = name;
		LoadInventory();
		if (inventory == null)
		{
			return 0;
		}
		if (name2 == "" || name2 == "*")
		{
			return inventory.Sum((ItemRecord i) => i.Stack);
		}
		if (!Wildcard.IsPattern(name2))
		{
			return inventory.Where((ItemRecord i) => i.PrefabName == name2).Sum((ItemRecord i) => i.Stack);
		}
		return inventory.Where((ItemRecord i) => Wildcard.Match(i.PrefabName, name2, ignoreCase: true)).Sum((ItemRecord i) => i.Stack);
	}

	private ItemRecord? GetItemAt(int x, int y)
	{
		LoadInventory();
		if (inventory == null || x < 0 || y < 0)
		{
			return null;
		}
		return inventory.FirstOrDefault((ItemRecord i) => i.GridPos.x == x && i.GridPos.y == y);
	}

	private void LoadInventory()
	{
		if (inventory == null)
		{
			inventory = ItemDataHelper.Load(_003Czdo_003EP);
		}
	}
}
