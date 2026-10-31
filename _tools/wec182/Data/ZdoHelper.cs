using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Common;
using UnityEngine;

namespace Data;

public static class ZdoHelper
{
	private static readonly Dictionary<string, int> HashCache = new Dictionary<string, int>();

	private static readonly Dictionary<int, string> ReverseHashCache = new Dictionary<int, string>();

	private static void FindKnownKey(int hash, ref string? key)
	{
		string text = ZDOKeys.Convert(hash);
		if (text != hash.ToString())
		{
			key = text;
		}
	}

	public static int Hash(string key)
	{
		if (HashCache.TryGetValue(key, out var value))
		{
			return value;
		}
		value = HashKey(key);
		HashCache[key] = value;
		ReverseHashCache[value] = key;
		return value;
	}

	private static int HashKey(string key)
	{
		if (Parse.TryInt(key, out var result))
		{
			return result;
		}
		if (key.StartsWith("$", StringComparison.InvariantCultureIgnoreCase))
		{
			int hash = ZSyncAnimation.GetHash(key.Substring(1));
			if (key == "$anim_speed")
			{
				return hash;
			}
			return 438569 + hash;
		}
		return StringExtensionMethods.GetStableHashCode(key);
	}

	public static string ReverseHash(int hash)
	{
		if (ReverseHashCache.TryGetValue(hash, out string value))
		{
			return value;
		}
		string key = null;
		FindKnownKey(hash, ref key);
		return key ?? hash.ToString(CultureInfo.InvariantCulture);
	}

	public static string GetString(ZDO zdo, string key, string defaultValue)
	{
		int num = Hash(key);
		if (num == ZDOVars.s_items)
		{
			return GetBytes(zdo, key, defaultValue);
		}
		return TryGetString(zdo, num) ?? defaultValue;
	}

	public static string GetBytes(ZDO zdo, string key, string defaultValue)
	{
		byte[] array = TryGetBytes(zdo, Hash(key));
		if (array != null)
		{
			return Convert.ToBase64String(array);
		}
		return defaultValue;
	}

	public static float GetFloat(ZDO zdo, string key, string defaultValue)
	{
		return TryGetFloat(zdo, Hash(key)) ?? Parse.Float(defaultValue);
	}

	public static int GetInt(ZDO zdo, string key, string defaultValue)
	{
		return TryGetInt(zdo, Hash(key)) ?? Parse.Int(defaultValue);
	}

	public static long GetLong(ZDO zdo, string key, string defaultValue)
	{
		return TryGetLong(zdo, Hash(key)) ?? Parse.Long(defaultValue, 0L);
	}

	public static bool GetBool(ZDO zdo, string key, string defaultValue)
	{
		return TryGetBool(zdo, Hash(key)) ?? Parse.Boolean(defaultValue);
	}

	public static Vector3 GetVec(ZDO zdo, string key, string defaultValue)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		return (Vector3)(((_003F?)TryGetVec(zdo, Hash(key))) ?? Parse.VectorXZY(defaultValue));
	}

	public static Quaternion GetQuaternion(ZDO zdo, string key, string defaultValue)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		return (Quaternion)(((_003F?)TryGetQuaternion(zdo, Hash(key))) ?? Parse.AngleYXZ(defaultValue));
	}

	public static string? TryGetStringField(int prefabHash, int hash)
	{
		if (!(GetField(prefabHash, ReverseHash(hash)) is string result))
		{
			return null;
		}
		return result;
	}

	public static float? TryGetFloatField(int prefabHash, int hash)
	{
		object field = GetField(prefabHash, ReverseHash(hash));
		if (!(field is float))
		{
			return null;
		}
		return (float)field;
	}

	public static int? TryGetIntField(int prefabHash, int hash)
	{
		object field = GetField(prefabHash, ReverseHash(hash));
		if (!(field is int))
		{
			return null;
		}
		return (int)field;
	}

	public static bool? TryGetBoolField(int prefabHash, int hash)
	{
		object field = GetField(prefabHash, ReverseHash(hash));
		if (!(field is bool))
		{
			return null;
		}
		return (bool)field;
	}

	public static long? TryGetLongField(int prefabHash, int hash)
	{
		object field = GetField(prefabHash, ReverseHash(hash));
		if (!(field is long))
		{
			return null;
		}
		return (long)field;
	}

	public static Vector3? TryGetVecField(int prefabHash, int hash)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		object field = GetField(prefabHash, ReverseHash(hash));
		if (!(field is Vector3))
		{
			return null;
		}
		return (Vector3)field;
	}

	public static Quaternion? TryGetQuatField(int prefabHash, int hash)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		object field = GetField(prefabHash, ReverseHash(hash));
		if (!(field is Quaternion))
		{
			return null;
		}
		return (Quaternion)field;
	}

	private static object? GetField(int prefabHash, string value)
	{
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, '.');
		if (keyValuePair.Value == "")
		{
			return null;
		}
		GameObject prefab = ZNetScene.instance.GetPrefab(prefabHash);
		if ((Object)(object)prefab == (Object)null)
		{
			return null;
		}
		Component val = FindComponent(prefab, keyValuePair.Key);
		if ((Object)(object)val == (Object)null)
		{
			return null;
		}
		string[] array = keyValuePair.Value.Split('.');
		object obj = val;
		string[] array2 = array;
		foreach (string name in array2)
		{
			FieldInfo field = obj.GetType().GetField(name);
			if (field == null)
			{
				return null;
			}
			obj = field.GetValue(obj);
			if (obj == null)
			{
				return null;
			}
		}
		GameObject val2 = (GameObject)((obj is GameObject) ? obj : null);
		if (val2 != null)
		{
			return ((Object)val2).name;
		}
		ItemDrop val3 = (ItemDrop)((obj is ItemDrop) ? obj : null);
		if (val3 != null)
		{
			return ((Object)((Component)val3).gameObject).name;
		}
		return obj;
	}

	private static Component? FindComponent(GameObject obj, string name)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		obj.GetComponentsInChildren<MonoBehaviour>(ZNetView.m_tempComponents);
		foreach (MonoBehaviour tempComponent in ZNetView.m_tempComponents)
		{
			if (((object)tempComponent).GetType().Name == name)
			{
				return (Component?)(object)tempComponent;
			}
		}
		foreach (Transform item in obj.transform)
		{
			Component val = FindComponent(((Component)item).gameObject, name);
			if ((Object)(object)val != (Object)null)
			{
				return val;
			}
		}
		return null;
	}

	public static Vector2i GetInventorySize(DataEntry entry, Functions f, ZDO? zdo)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		int num = Hash("Container.m_width");
		int num2 = Hash("Container.m_height");
		int num3 = 0;
		int num4 = 0;
		Dictionary<int, IIntValue>? ints = entry.Ints;
		if (ints != null && ints.TryGetValue(num, out IIntValue value))
		{
			num3 = value.Get(f).GetValueOrDefault();
		}
		Dictionary<int, IIntValue>? ints2 = entry.Ints;
		if (ints2 != null && ints2.TryGetValue(num2, out IIntValue value2))
		{
			num4 = value2.Get(f).GetValueOrDefault();
		}
		if (num3 > 0 && num4 > 0)
		{
			return new Vector2i(num3, num4);
		}
		if (zdo == null)
		{
			return new Vector2i((num3 > 0) ? num3 : 4, (num4 > 0) ? num4 : 2);
		}
		if (num3 <= 0)
		{
			num3 = zdo.GetInt(num, 0);
		}
		if (num4 <= 0)
		{
			num4 = zdo.GetInt(num2, 0);
		}
		if (num3 > 0 && num4 > 0)
		{
			return new Vector2i(num3, num4);
		}
		Container componentInChildren = ZNetScene.instance.GetPrefab(zdo.GetPrefab()).GetComponentInChildren<Container>();
		if (Object.op_Implicit((Object)(object)componentInChildren))
		{
			if (num3 <= 0)
			{
				num3 = componentInChildren.m_width;
			}
			if (num4 <= 0)
			{
				num4 = componentInChildren.m_height;
			}
		}
		if (num3 <= 0)
		{
			num3 = 4;
		}
		if (num4 <= 0)
		{
			num4 = 2;
		}
		return new Vector2i(num3, num4);
	}

	public static byte[]? TryGetBytes(ZDO zdo, int hash)
	{
		return zdo.GetByteArray(hash, (byte[])null);
	}

	public static string? TryGetString(ZDO zdo, int hash)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (ItemDataHelper.TryGetString(zdo, hash, out string value))
		{
			return value;
		}
		string result = default(string);
		if (!ZDOExtraData.s_strings.TryGetValue(zdo.m_uid, out var value2) || !value2.TryGetValue(hash, ref result))
		{
			return TryGetStringField(zdo.m_prefab, hash);
		}
		return result;
	}

	public static float? TryGetFloat(ZDO zdo, int hash)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (ItemDataHelper.TryGetFloat(zdo, hash, out var value))
		{
			return value;
		}
		float value3 = default(float);
		if (!ZDOExtraData.s_floats.TryGetValue(zdo.m_uid, out var value2) || !value2.TryGetValue(hash, ref value3))
		{
			return TryGetFloatField(zdo.m_prefab, hash);
		}
		return value3;
	}

	public static int? TryGetInt(ZDO zdo, int hash)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (ItemDataHelper.TryGetInt(zdo, hash, out var value))
		{
			return value;
		}
		int value3 = default(int);
		if (!ZDOExtraData.s_ints.TryGetValue(zdo.m_uid, out var value2) || !value2.TryGetValue(hash, ref value3))
		{
			return TryGetIntField(zdo.m_prefab, hash);
		}
		return value3;
	}

	public static long? TryGetLong(ZDO zdo, int hash)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (ItemDataHelper.TryGetLong(zdo, hash, out var value))
		{
			return value;
		}
		long value3 = default(long);
		if (!ZDOExtraData.s_longs.TryGetValue(zdo.m_uid, out var value2) || !value2.TryGetValue(hash, ref value3))
		{
			return TryGetLongField(zdo.m_prefab, hash);
		}
		return value3;
	}

	public static bool? TryGetBool(ZDO zdo, int hash)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (ItemDataHelper.TryGetInt(zdo, hash, out var value))
		{
			return value > 0;
		}
		int num = default(int);
		if (!ZDOExtraData.s_ints.TryGetValue(zdo.m_uid, out var value2) || !value2.TryGetValue(hash, ref num))
		{
			return TryGetBoolField(zdo.m_prefab, hash);
		}
		return num > 0;
	}

	public static Vector3? TryGetVec(ZDO zdo, int hash)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		Vector3 value2 = default(Vector3);
		if (!ZDOExtraData.s_vec3.TryGetValue(zdo.m_uid, out var value) || !value.TryGetValue(hash, ref value2))
		{
			return TryGetVecField(zdo.m_prefab, hash);
		}
		return value2;
	}

	public static Quaternion? TryGetQuaternion(ZDO zdo, int hash)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		Quaternion value2 = default(Quaternion);
		if (!ZDOExtraData.s_quats.TryGetValue(zdo.m_uid, out var value) || !value.TryGetValue(hash, ref value2))
		{
			return TryGetQuatField(zdo.m_prefab, hash);
		}
		return value2;
	}
}
