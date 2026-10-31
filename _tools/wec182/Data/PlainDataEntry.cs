using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Data;

public class PlainDataEntry : ResolvedDataEntry
{
	private static readonly HashSet<int> HashKeys;

	private bool HasConnection
	{
		get
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (ConnectionType.HasValue && ConnectionType != (ConnectionType?)0)
			{
				return ConnectionHash != 0;
			}
			return false;
		}
	}

	public PlainDataEntry()
	{
	}

	public PlainDataEntry(ZDO zdo)
	{
		Load(zdo);
	}

	public void Load(ZDO zdo)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		ZDOID uid = zdo.m_uid;
		Floats = (ZDOExtraData.s_floats.ContainsKey(uid) ? ((IEnumerable<KeyValuePair<int, float>>)ZDOExtraData.s_floats[uid]).ToDictionary((KeyValuePair<int, float> kvp) => kvp.Key, (KeyValuePair<int, float> kvp) => kvp.Value) : null);
		Ints = (ZDOExtraData.s_ints.ContainsKey(uid) ? ((IEnumerable<KeyValuePair<int, int>>)ZDOExtraData.s_ints[uid]).ToDictionary((KeyValuePair<int, int> kvp) => kvp.Key, (KeyValuePair<int, int> kvp) => kvp.Value) : null);
		Longs = (ZDOExtraData.s_longs.ContainsKey(uid) ? ((IEnumerable<KeyValuePair<int, long>>)ZDOExtraData.s_longs[uid]).ToDictionary((KeyValuePair<int, long> kvp) => kvp.Key, (KeyValuePair<int, long> kvp) => kvp.Value) : null);
		Strings = (ZDOExtraData.s_strings.ContainsKey(uid) ? ((IEnumerable<KeyValuePair<int, string>>)ZDOExtraData.s_strings[uid]).ToDictionary((KeyValuePair<int, string> kvp) => kvp.Key, (KeyValuePair<int, string> kvp) => kvp.Value) : null);
		Vecs = (ZDOExtraData.s_vec3.ContainsKey(uid) ? ((IEnumerable<KeyValuePair<int, Vector3>>)ZDOExtraData.s_vec3[uid]).ToDictionary((KeyValuePair<int, Vector3> kvp) => kvp.Key, (KeyValuePair<int, Vector3> kvp) => kvp.Value) : null);
		Quats = (ZDOExtraData.s_quats.ContainsKey(uid) ? ((IEnumerable<KeyValuePair<int, Quaternion>>)ZDOExtraData.s_quats[uid]).ToDictionary((KeyValuePair<int, Quaternion> kvp) => kvp.Key, (KeyValuePair<int, Quaternion> kvp) => kvp.Value) : null);
		ByteArrays = (ZDOExtraData.s_byteArrays.ContainsKey(uid) ? ((IEnumerable<KeyValuePair<int, byte[]>>)ZDOExtraData.s_byteArrays[uid]).ToDictionary((KeyValuePair<int, byte[]> kvp) => kvp.Key, (KeyValuePair<int, byte[]> kvp) => kvp.Value) : null);
		if (ZDOExtraData.s_connectionsHashData.TryGetValue(uid, out var value))
		{
			ConnectionType = value.m_type;
			ConnectionHash = value.m_hash;
		}
		OriginalId = uid;
		if (ZDOExtraData.s_connections.TryGetValue(uid, out var value2) && value2.m_target != ZDOID.None)
		{
			TargetConnectionId = value2.m_target;
			ConnectionType = value2.m_type;
		}
		Distant = zdo.Distant;
		Persistent = zdo.Persistent;
		Priority = zdo.Type;
	}

	public void Write(DataYaml data, bool all)
	{
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		data.floats = Floats?.Select((KeyValuePair<int, float> pair) => $"{ZDOKeys.Convert(pair.Key)}, {pair.Value}").ToArray();
		data.ints = (from pair in Ints?.Where((KeyValuePair<int, int> kvp) => !HashKeys.Contains(kvp.Key))
			select $"{ZDOKeys.Convert(pair.Key)}, {pair.Value}").ToArray();
		string[]? ints = data.ints;
		if (ints != null && ints.Length == 0)
		{
			data.ints = null;
		}
		data.hashes = Ints?.Where((KeyValuePair<int, int> kvp) => HashKeys.Contains(kvp.Key)).Select(delegate(KeyValuePair<int, int> pair)
		{
			string text = ZDOKeys.Convert(pair.Key);
			GameObject prefab2 = ZNetScene.instance.GetPrefab(pair.Value);
			return text + ", " + (((prefab2 != null) ? ((Object)prefab2).name : null) ?? pair.Value.ToString());
		}).ToArray();
		string[]? hashes = data.hashes;
		if (hashes != null && hashes.Length == 0)
		{
			data.hashes = null;
		}
		data.longs = Longs?.Select((KeyValuePair<int, long> pair) => $"{ZDOKeys.Convert(pair.Key)}, {pair.Value}").ToArray();
		data.strings = Strings?.Select((KeyValuePair<int, string> pair) => ZDOKeys.Convert(pair.Key) + ", " + Serialize(pair.Value)).ToArray();
		data.vecs = Vecs?.Select((KeyValuePair<int, Vector3> pair) => ZDOKeys.Convert(pair.Key) + ", " + Serialize(pair.Value)).ToArray();
		data.quats = Quats?.Select((KeyValuePair<int, Quaternion> pair) => ZDOKeys.Convert(pair.Key) + ", " + Serialize(pair.Value)).ToArray();
		Dictionary<int, byte[]> dictionary = ((ByteArrays == null) ? null : new Dictionary<int, byte[]>(ByteArrays));
		if (dictionary != null && dictionary.TryGetValue(ZDOVars.s_items, out var value))
		{
			List<ItemRecord> list = ItemDataHelper.Load(new ZPackage(value));
			if (list.Count > 0 && ItemDataHelper.CountInvalid(list) == 0)
			{
				List<ItemYaml> list2 = new List<ItemYaml>();
				list2.AddRange(list.Select((ItemRecord r) => ToData(r, includePos: true, all)));
				data.items = list2.ToArray();
				dictionary.Remove(ZDOVars.s_items);
			}
		}
		if (dictionary != null && dictionary.TryGetValue(ZDOVars.s_itemData, out var value2))
		{
			ItemRecord itemRecord = ItemDataHelper.LoadItem(value2);
			if (itemRecord != null)
			{
				data.item = ToData(itemRecord, all, all);
				dictionary.Remove(ZDOVars.s_itemData);
			}
		}
		data.bytes = ((dictionary != null && dictionary.Count > 0) ? dictionary.Select((KeyValuePair<int, byte[]> pair) => ZDOKeys.Convert(pair.Key) + ", " + Convert.ToBase64String(pair.Value)).ToArray() : null);
		if (HasConnection)
		{
			data.connection = $"{ConnectionType}, {ConnectionHash}";
		}
		bool valueOrDefault = Persistent.GetValueOrDefault(true);
		bool valueOrDefault2 = Distant.GetValueOrDefault();
		ObjectType valueOrDefault3 = Priority.GetValueOrDefault();
		if (all)
		{
			data.persistent = (valueOrDefault ? "true" : "false");
			data.distant = (valueOrDefault2 ? "true" : "false");
			data.priority = ((object)(ObjectType)(ref valueOrDefault3)).ToString();
			return;
		}
		bool flag = false;
		bool flag2 = true;
		ObjectType val = (ObjectType)0;
		if (OriginalId.HasValue)
		{
			ZDOID? originalId = OriginalId;
			ZDOID none = ZDOID.None;
			if (!originalId.HasValue || originalId.GetValueOrDefault() != none)
			{
				ZDO zDO = ZDOMan.instance.GetZDO(OriginalId.Value);
				GameObject prefab = ZNetScene.instance.GetPrefab(zDO.m_prefab);
				ZNetView val2 = ((prefab != null) ? prefab.GetComponent<ZNetView>() : null);
				if ((Object)(object)val2 != (Object)null)
				{
					flag = val2.m_distant;
					flag2 = val2.m_persistent;
					val = val2.m_type;
				}
			}
		}
		if (valueOrDefault != flag2)
		{
			data.persistent = (valueOrDefault ? "true" : "false");
		}
		if (valueOrDefault2 != flag)
		{
			data.distant = (valueOrDefault2 ? "true" : "false");
		}
		if (valueOrDefault3 != val)
		{
			data.priority = ((object)(ObjectType)(ref valueOrDefault3)).ToString();
		}
	}

	private static ItemYaml ToData(ItemRecord record, bool includePos, bool all)
	{
		return new ItemYaml
		{
			pos = (includePos ? $"{record.GridPos.x}, {record.GridPos.y}" : ""),
			prefab = record.PrefabName,
			stack = record.Stack.ToString(),
			quality = ((all || record.Quality != 1) ? record.Quality.ToString() : null),
			variant = ((all || record.Variant != 0) ? record.Variant.ToString() : null),
			customData = ((record.CustomData.Count > 0) ? record.CustomData : null),
			equipped = ((!all) ? (record.Equipped ? "true" : null) : (record.Equipped ? "true" : "false")),
			durability = Serialize(record.Durability),
			crafterID = ((all || record.CrafterID != 0L) ? record.CrafterID.ToString() : null),
			crafterName = ((all || record.CrafterName != "") ? record.CrafterName : null),
			pickedUp = ((!all) ? (record.PickedUp ? "true" : null) : (record.PickedUp ? "true" : "false")),
			cheated = ((!all) ? (record.Cheated ? "true" : null) : (record.Cheated ? "true" : "false")),
			worldLevel = ((all || record.WorldLevel != 0) ? record.WorldLevel.ToString() : null)
		};
	}

	private static string Serialize(string? str)
	{
		if (str != null && !(str == ""))
		{
			if (!str.Contains(","))
			{
				return str;
			}
			return "\"" + str + "\"";
		}
		return "\"\"";
	}

	private static string Serialize(Quaternion quat)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		Vector3 eulerAngles = ((Quaternion)(ref quat)).eulerAngles;
		if (eulerAngles.x == 0f && eulerAngles.z == 0f)
		{
			return Serialize(eulerAngles.y);
		}
		return Serialize(eulerAngles.y) + "," + Serialize(eulerAngles.x) + "," + Serialize(eulerAngles.z);
	}

	private static string Serialize(float value)
	{
		return value.ToString("0.#####", NumberFormatInfo.InvariantInfo);
	}

	private static string Serialize(Vector3 vec)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		return Serialize(vec.x) + "," + Serialize(vec.z) + "," + Serialize(vec.y);
	}

	public string GetBase64()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ZPackage val = new ZPackage();
		Write(val);
		return val.GetBase64();
	}

	public void Write(ZPackage pkg)
	{
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Expected I4, but got Unknown
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e9: Expected I4, but got Unknown
		int num = 0;
		Dictionary<int, float>? floats = Floats;
		if (floats != null && floats.Count > 0)
		{
			num |= 1;
		}
		Dictionary<int, Vector3>? vecs = Vecs;
		if (vecs != null && vecs.Count > 0)
		{
			num |= 2;
		}
		Dictionary<int, Quaternion>? quats = Quats;
		if (quats != null && quats.Count > 0)
		{
			num |= 4;
		}
		Dictionary<int, int>? ints = Ints;
		if (ints != null && ints.Count > 0)
		{
			num |= 8;
		}
		Dictionary<int, string>? strings = Strings;
		if (strings != null && strings.Count > 0)
		{
			num |= 0x10;
		}
		Dictionary<int, long>? longs = Longs;
		if (longs != null && longs.Count > 0)
		{
			num |= 0x40;
		}
		Dictionary<int, byte[]>? byteArrays = ByteArrays;
		if (byteArrays != null && byteArrays.Count > 0)
		{
			num |= 0x80;
		}
		if (HasConnection)
		{
			num |= 0x100;
		}
		if (Persistent == false)
		{
			num |= 0x200;
		}
		if (Distant.GetValueOrDefault())
		{
			num |= 0x400;
		}
		if (Priority.HasValue && Priority != (ObjectType?)0)
		{
			num |= 0x800;
		}
		pkg.Write(num);
		Dictionary<int, float>? floats2 = Floats;
		if (floats2 != null && floats2.Count > 0)
		{
			pkg.Write((byte)Floats.Count);
			foreach (KeyValuePair<int, float> @float in Floats)
			{
				pkg.Write(@float.Key);
				pkg.Write(@float.Value);
			}
		}
		Dictionary<int, Vector3>? vecs2 = Vecs;
		if (vecs2 != null && vecs2.Count > 0)
		{
			pkg.Write((byte)Vecs.Count);
			foreach (KeyValuePair<int, Vector3> vec in Vecs)
			{
				pkg.Write(vec.Key);
				pkg.Write(vec.Value);
			}
		}
		Dictionary<int, Quaternion>? quats2 = Quats;
		if (quats2 != null && quats2.Count > 0)
		{
			pkg.Write((byte)Quats.Count);
			foreach (KeyValuePair<int, Quaternion> quat in Quats)
			{
				pkg.Write(quat.Key);
				pkg.Write(quat.Value);
			}
		}
		Dictionary<int, int>? ints2 = Ints;
		if (ints2 != null && ints2.Count > 0)
		{
			pkg.Write((byte)Ints.Count);
			foreach (KeyValuePair<int, int> @int in Ints)
			{
				pkg.Write(@int.Key);
				pkg.Write(@int.Value);
			}
		}
		Dictionary<int, long>? longs2 = Longs;
		if (longs2 != null && longs2.Count > 0)
		{
			pkg.Write((byte)Longs.Count);
			foreach (KeyValuePair<int, long> @long in Longs)
			{
				pkg.Write(@long.Key);
				pkg.Write(@long.Value);
			}
		}
		Dictionary<int, string>? strings2 = Strings;
		if (strings2 != null && strings2.Count > 0)
		{
			pkg.Write((byte)Strings.Count);
			foreach (KeyValuePair<int, string> @string in Strings)
			{
				pkg.Write(@string.Key);
				pkg.Write(@string.Value);
			}
		}
		Dictionary<int, byte[]>? byteArrays2 = ByteArrays;
		if (byteArrays2 != null && byteArrays2.Count > 0)
		{
			pkg.Write((byte)ByteArrays.Count);
			foreach (KeyValuePair<int, byte[]> byteArray in ByteArrays)
			{
				pkg.Write(byteArray.Key);
				pkg.Write(byteArray.Value);
			}
		}
		if (HasConnection)
		{
			pkg.Write((byte)(int)ConnectionType.Value);
			pkg.Write(ConnectionHash);
		}
		if (((uint)num & 0x200u) != 0)
		{
			pkg.Write(Persistent.Value);
		}
		if (((uint)num & 0x400u) != 0)
		{
			pkg.Write(Distant.Value);
		}
		if (((uint)num & 0x800u) != 0)
		{
			pkg.Write((byte)(int)Priority.Value);
		}
	}

	static PlainDataEntry()
	{
		HashSet<int> hashSet = new HashSet<int>
		{
			ZDOVars.s_helmetItem,
			ZDOVars.s_chestItem,
			ZDOVars.s_legItem,
			ZDOVars.s_shoulderItem,
			ZDOVars.s_utilityItem,
			ZDOVars.s_leftItem,
			ZDOVars.s_rightItem,
			ZDOVars.s_content,
			ZDOVars.s_item
		};
		foreach (int item in from i in Enumerable.Range(0, 11)
			select ZDOKeys.Hash($"{i}_item"))
		{
			hashSet.Add(item);
		}
		HashKeys = hashSet;
	}
}
