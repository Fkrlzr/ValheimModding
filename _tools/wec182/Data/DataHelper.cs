using System;
using System.Collections.Generic;
using System.Linq;
using ServerDevcommands;
using UnityEngine;
using WorldEditCommands;

namespace Data;

public class DataHelper
{
	public static string Name(int hash)
	{
		return Utils.GetPrefabName(ZNetScene.instance.GetPrefab(hash));
	}

	public static ZDO Regen(ZDO existing, ZDO zdo)
	{
		ZNetScene.instance.CreateObject(zdo);
		Destroy(existing);
		return zdo;
	}

	public static ZDO Regen(ZDO existing, PlainDataEntry data)
	{
		ZDO val = CloneBase(existing);
		((ResolvedDataEntry)data).Write(val);
		ZNetScene.instance.CreateObject(val);
		Destroy(existing);
		return val;
	}

	public static void Destroy(ZDO zdo)
	{
		zdo.SetOwner(ZDOMan.instance.m_sessionID);
		if (ZNetScene.instance.m_instances.TryGetValue(zdo, out var value) && Object.op_Implicit((Object)(object)value))
		{
			ZNetScene.instance.Destroy(((Component)value).gameObject);
		}
		else
		{
			ZDOMan.instance.DestroyZDO(zdo);
		}
	}

	public static ZDO CloneBase(ZDO zdo)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		ZDO obj = ZDOMan.instance.CreateNewZDO(zdo.m_position, zdo.m_prefab);
		obj.m_prefab = zdo.m_prefab;
		obj.m_rotation = zdo.m_rotation;
		obj.Type = zdo.Type;
		obj.Distant = zdo.Distant;
		obj.Persistent = zdo.Persistent;
		obj.IncreaseDataRevision();
		return obj;
	}

	public static ZDO CloneWithKeys(ZDO zdo, string[] keys)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		HashSet<int> hashSet = keys.Select(ZDOKeys.Hash).ToHashSet();
		ZDO val = CloneBase(zdo);
		ZDOID uid = zdo.m_uid;
		ZDOID uid2 = val.m_uid;
		float num = default(float);
		Vector3 val2 = default(Vector3);
		Quaternion val3 = default(Quaternion);
		int num2 = default(int);
		long num3 = default(long);
		string text = default(string);
		byte[] array = default(byte[]);
		foreach (int item in hashSet)
		{
			if (ZDOExtraData.GetFloat(uid, item, ref num))
			{
				ZDOExtraData.Set(uid2, item, num);
			}
			if (ZDOExtraData.GetVec3(uid, item, ref val2))
			{
				ZDOExtraData.Set(uid2, item, val2);
			}
			if (ZDOExtraData.GetQuaternion(uid, item, ref val3))
			{
				ZDOExtraData.Set(uid2, item, val3);
			}
			if (ZDOExtraData.GetInt(uid, item, ref num2))
			{
				ZDOExtraData.Set(uid2, item, num2);
			}
			if (ZDOExtraData.GetLong(uid, item, ref num3))
			{
				ZDOExtraData.Set(uid2, item, num3);
			}
			if (ZDOExtraData.GetString(uid, item, ref text))
			{
				ZDOExtraData.Set(uid2, item, text);
			}
			if (ZDOExtraData.GetByteArray(uid, item, ref array))
			{
				ZDOExtraData.Set(uid2, item, array);
			}
		}
		return val;
	}

	private static IEnumerable<KeyValuePair<int, T>> GetValues<T>(Dictionary<ZDOID, BinarySearchDictionary<int, T>> values, ZDOID id)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		if (!values.TryGetValue(id, out BinarySearchDictionary<int, T> value))
		{
			return Enumerable.Empty<KeyValuePair<int, T>>();
		}
		return (IEnumerable<KeyValuePair<int, T>>)value;
	}

	public static bool HasKey(ZDO zdo, string[] keys)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		HashSet<int> hashSet = keys.Select(ZDOKeys.Hash).ToHashSet();
		ZDOID uid = zdo.m_uid;
		IEnumerable<int> first = from kvp in GetValues(ZDOExtraData.s_floats, uid)
			select kvp.Key;
		IEnumerable<int> second = from kvp in GetValues(ZDOExtraData.s_vec3, uid)
			select kvp.Key;
		IEnumerable<int> second2 = from kvp in GetValues(ZDOExtraData.s_quats, uid)
			select kvp.Key;
		IEnumerable<int> second3 = from kvp in GetValues(ZDOExtraData.s_ints, uid)
			select kvp.Key;
		IEnumerable<int> second4 = from kvp in GetValues(ZDOExtraData.s_longs, uid)
			select kvp.Key;
		IEnumerable<int> second5 = from kvp in GetValues(ZDOExtraData.s_strings, uid)
			select kvp.Key;
		return Enumerable.Concat(second: from kvp in GetValues(ZDOExtraData.s_byteArrays, uid)
			select kvp.Key, first: first.Concat(second).Concat(second2).Concat(second3)
			.Concat(second4)
			.Concat(second5)).Any(hashSet.Contains);
	}

	public static ZDO CloneWithoutKeys(ZDO zdo, string[] keys)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		HashSet<int> hashed = keys.Select(ZDOKeys.Hash).ToHashSet();
		ZDO val = CloneBase(zdo);
		ZDOID uid = zdo.m_uid;
		ZDOID uid2 = val.m_uid;
		Dictionary<int, float> dictionary = (from kvp in GetValues(ZDOExtraData.s_floats, uid)
			where !hashed.Contains(kvp.Key)
			select kvp).ToDictionary((KeyValuePair<int, float> kvp) => kvp.Key, (KeyValuePair<int, float> kvp) => kvp.Value);
		Dictionary<int, Vector3> dictionary2 = (from kvp in GetValues(ZDOExtraData.s_vec3, uid)
			where !hashed.Contains(kvp.Key)
			select kvp).ToDictionary((KeyValuePair<int, Vector3> kvp) => kvp.Key, (KeyValuePair<int, Vector3> kvp) => kvp.Value);
		Dictionary<int, Quaternion> dictionary3 = (from kvp in GetValues(ZDOExtraData.s_quats, uid)
			where !hashed.Contains(kvp.Key)
			select kvp).ToDictionary((KeyValuePair<int, Quaternion> kvp) => kvp.Key, (KeyValuePair<int, Quaternion> kvp) => kvp.Value);
		Dictionary<int, int> dictionary4 = (from kvp in GetValues(ZDOExtraData.s_ints, uid)
			where !hashed.Contains(kvp.Key)
			select kvp).ToDictionary((KeyValuePair<int, int> kvp) => kvp.Key, (KeyValuePair<int, int> kvp) => kvp.Value);
		Dictionary<int, long> dictionary5 = (from kvp in GetValues(ZDOExtraData.s_longs, uid)
			where !hashed.Contains(kvp.Key)
			select kvp).ToDictionary((KeyValuePair<int, long> kvp) => kvp.Key, (KeyValuePair<int, long> kvp) => kvp.Value);
		Dictionary<int, string> dictionary6 = (from kvp in GetValues(ZDOExtraData.s_strings, uid)
			where !hashed.Contains(kvp.Key)
			select kvp).ToDictionary((KeyValuePair<int, string> kvp) => kvp.Key, (KeyValuePair<int, string> kvp) => kvp.Value);
		Dictionary<int, byte[]> dictionary7 = (from kvp in GetValues(ZDOExtraData.s_byteArrays, uid)
			where !hashed.Contains(kvp.Key)
			select kvp).ToDictionary((KeyValuePair<int, byte[]> kvp) => kvp.Key, (KeyValuePair<int, byte[]> kvp) => kvp.Value);
		foreach (KeyValuePair<int, float> item in dictionary)
		{
			ZDOExtraData.Set(uid2, item.Key, item.Value);
		}
		foreach (KeyValuePair<int, Vector3> item2 in dictionary2)
		{
			ZDOExtraData.Set(uid2, item2.Key, item2.Value);
		}
		foreach (KeyValuePair<int, Quaternion> item3 in dictionary3)
		{
			ZDOExtraData.Set(uid2, item3.Key, item3.Value);
		}
		foreach (KeyValuePair<int, int> item4 in dictionary4)
		{
			ZDOExtraData.Set(uid2, item4.Key, item4.Value);
		}
		foreach (KeyValuePair<int, long> item5 in dictionary5)
		{
			ZDOExtraData.Set(uid2, item5.Key, item5.Value);
		}
		foreach (KeyValuePair<int, string> item6 in dictionary6)
		{
			ZDOExtraData.Set(uid2, item6.Key, item6.Value);
		}
		foreach (KeyValuePair<int, byte[]> item7 in dictionary7)
		{
			ZDOExtraData.Set(uid2, item7.Key, item7.Value);
		}
		return val;
	}

	public static List<string> Print(ZDO zdo)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		GameObject prefab = ZNetScene.instance.GetPrefab(zdo.m_prefab);
		ZDOID uid = zdo.m_uid;
		List<string> collection = new List<string>(6)
		{
			$"Id: {uid}",
			"Prefab: " + (Object.op_Implicit((Object)(object)prefab) ? ((Object)prefab).name : "Unknown"),
			$"Owner: {zdo.GetOwner()}",
			"Position: " + Helper.PrintVectorXZY(zdo.m_position) + " (vec x,z,y)",
			"Rotation: " + Helper.PrintVectorYXZ(zdo.m_rotation) + " (quat y,x,z)",
			$"Revision: {zdo.DataRevision} + {zdo.OwnerRevision}"
		};
		IEnumerable<string> collection2 = from kvp in GetValues(ZDOExtraData.s_vec3, uid)
			select ZDOKeys.Convert(kvp.Key) + ": " + Helper.PrintVectorXZY(kvp.Value) + " (vec x,z,y)";
		IEnumerable<string> collection3 = from kvp in GetValues(ZDOExtraData.s_ints, uid)
			select $"{ZDOKeys.Convert(kvp.Key)}: {kvp.Value} (int)";
		IEnumerable<string> collection4 = from kvp in GetValues(ZDOExtraData.s_floats, uid)
			select $"{ZDOKeys.Convert(kvp.Key)}: {kvp.Value} (float)";
		IEnumerable<string> collection5 = from kvp in GetValues(ZDOExtraData.s_quats, uid)
			select ZDOKeys.Convert(kvp.Key) + ": " + Helper.PrintAngleYXZ(kvp.Value) + " (quat y,x,z)";
		IEnumerable<string> collection6 = from kvp in GetValues(ZDOExtraData.s_strings, uid)
			select ZDOKeys.Convert(kvp.Key) + ": " + kvp.Value + " (string)";
		IEnumerable<string> collection7 = from kvp in GetValues(ZDOExtraData.s_longs, uid)
			select $"{ZDOKeys.Convert(kvp.Key)}: {kvp.Value} (long)";
		IEnumerable<string> collection8 = from kvp in GetValues(ZDOExtraData.s_byteArrays, uid)
			select ZDOKeys.Convert(kvp.Key) + ": " + Convert.ToBase64String(kvp.Value) + " (byte array)";
		List<string> list = new List<string>();
		list.AddRange(collection);
		list.AddRange(collection2);
		list.AddRange(collection3);
		list.AddRange(collection4);
		list.AddRange(collection5);
		list.AddRange(collection6);
		list.AddRange(collection7);
		list.AddRange(collection8);
		return list;
	}

	public static ZDO? Init(int prefab, Transform tr, DataEntry? data)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return Init(prefab, tr.position, tr.rotation, tr.localScale, data);
	}

	public static ZDO? Init(int prefab, Vector3 pos, Quaternion rot, Vector3? scale, DataEntry? data)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		CleanUp();
		GameObject prefab2 = ZNetScene.instance.GetPrefab(prefab);
		if (!Object.op_Implicit((Object)(object)prefab2))
		{
			return null;
		}
		return Init(prefab2, pos, rot, scale, data, new Dictionary<string, string>());
	}

	public static ZDO? Init(GameObject obj, Vector3 pos, Quaternion rot, Vector3? scale, DataEntry? data, Dictionary<string, string> pars)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		CleanUp();
		if (data == null && !scale.HasValue)
		{
			return null;
		}
		ZNetView val = default(ZNetView);
		if (!obj.TryGetComponent<ZNetView>(ref val))
		{
			return null;
		}
		int stableHashCode = StringExtensionMethods.GetStableHashCode(Utils.GetPrefabName(obj));
		ZNetView.m_initZDO = ZDOMan.instance.CreateNewZDO(pos, stableHashCode);
		ZNetView.m_initZDO.Type = val.m_type;
		ZNetView.m_initZDO.Distant = val.m_distant;
		ZNetView.m_initZDO.Persistent = val.m_persistent;
		ZNetView.m_initZDO.m_prefab = stableHashCode;
		if (data != null)
		{
			Write(data, pars, ZNetView.m_initZDO);
		}
		ZNetView.m_initZDO.m_rotation = ((Quaternion)(ref rot)).eulerAngles;
		ZNetView.m_initZDO.m_prefab = stableHashCode;
		if (!val.m_syncInitialScale && scale.HasValue && global::WorldEditCommands.WorldEditCommands.IsTweaks)
		{
			ZNetView.m_initZDO.Set(Hash.HasFields, true);
			ZNetView.m_initZDO.Set("HasFieldsZNetView", true);
			ZNetView.m_initZDO.Set("ZNetView.m_syncInitialScale", true);
			val.m_syncInitialScale = true;
			((Terminal)Console.instance).AddString("Note: Scaling set to true.");
		}
		if (val.m_syncInitialScale && scale.HasValue)
		{
			ZNetView.m_initZDO.Set(ZDOVars.s_scaleHash, scale.Value);
		}
		ZNetView.m_initZDO.DataRevision = 0u;
		ZNetView.m_initZDO.IncreaseDataRevision();
		return ZNetView.m_initZDO;
	}

	public static void CleanUp()
	{
		ZNetView.m_initZDO = null;
	}

	public static DataEntry? Merge(params DataEntry?[] datas)
	{
		DataEntry[] array = datas.Where((DataEntry d) => d != null).ToArray();
		if (array.Length == 0)
		{
			return null;
		}
		if (array.Length == 1)
		{
			return array[0];
		}
		DataEntry dataEntry = new DataEntry();
		DataEntry[] array2 = array;
		foreach (DataEntry data in array2)
		{
			dataEntry.Load(data);
		}
		return dataEntry;
	}

	public static DataEntry Get(string name)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		int stableHashCode = StringExtensionMethods.GetStableHashCode(name);
		if (!DataLoading.Data.ContainsKey(stableHashCode))
		{
			try
			{
				DataLoading.Data[stableHashCode] = new DataEntry(new ZPackage(name));
			}
			catch (Exception innerException)
			{
				if (name.Contains("=") || name.Length > 32)
				{
					throw new InvalidOperationException("Can't load data value: " + name, innerException);
				}
				throw new InvalidOperationException("Can't find data entry: " + name, innerException);
			}
		}
		return DataLoading.Data[stableHashCode];
	}

	public static PlainDataEntry Resolve(DataEntry data, Dictionary<string, string> pars, ZDO? zdo)
	{
		PlainDataEntry plainDataEntry = new PlainDataEntry();
		plainDataEntry.Load(data, new ParFunctions(pars, zdo), zdo);
		return plainDataEntry;
	}

	public static void Write(DataEntry data, Dictionary<string, string> pars, ZDO zdo)
	{
		((ResolvedDataEntry)Resolve(data, pars, zdo)).Write(zdo);
	}

	public static string Base64(Dictionary<string, string> pars, string data)
	{
		if (!DataLoading.Data.TryGetValue(StringExtensionMethods.GetStableHashCode(data), out DataEntry value))
		{
			return data;
		}
		return Resolve(value, pars, null).GetBase64();
	}
}
