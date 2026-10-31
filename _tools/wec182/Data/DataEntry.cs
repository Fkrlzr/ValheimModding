using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Service;
using UnityEngine;

namespace Data;

public class DataEntry
{
	private static readonly int HasFieldsHash = ZdoHelper.Hash("HasFields");

	public bool CanBeInjected = true;

	public Dictionary<int, IStringValue>? Strings;

	public Dictionary<int, IFloatValue>? Floats;

	public Dictionary<int, IIntValue>? Ints;

	public Dictionary<int, IIntValue>? Components;

	public Dictionary<int, IBoolValue>? Bools;

	public Dictionary<int, IHashValue>? Hashes;

	public Dictionary<int, ILongValue>? Longs;

	public Dictionary<int, IVector3Value>? Vecs;

	public Dictionary<int, IQuaternionValue>? Quats;

	public Dictionary<int, IBytesValue>? ByteArrays;

	public List<ItemValue>? Items;

	public ItemValue? Item;

	public Vector2i? ContainerSize;

	public IIntValue? ItemAmount;

	public ConnectionType? ConnectionType;

	public int ConnectionHash;

	public IZdoIdValue? OriginalId;

	public IZdoIdValue? TargetConnectionId;

	public IBoolValue? Persistent;

	public IBoolValue? Distant;

	public ObjectType? Priority;

	public IVector3Value? Position;

	public IQuaternionValue? Rotation;

	public static HashSet<string> SupportedTypes = new HashSet<string> { "float", "int", "bool", "hash", "long", "string", "vec", "vec3", "quat", "bytes" };

	public DataEntry()
	{
	}

	public DataEntry(string[] typeKeyValues)
	{
		Load(typeKeyValues);
	}

	public DataEntry(DataYaml data)
	{
		Load(data);
	}

	public DataEntry(ZDO zdo)
	{
		Load(zdo);
	}

	public DataEntry(ZPackage pkg)
	{
		Load(pkg);
	}

	public void Load(DataEntry data)
	{
		if (data.Floats != null)
		{
			if (Floats == null)
			{
				Floats = new Dictionary<int, IFloatValue>();
			}
			foreach (KeyValuePair<int, IFloatValue> @float in data.Floats)
			{
				Floats[@float.Key] = @float.Value;
			}
		}
		if (data.Vecs != null)
		{
			if (Vecs == null)
			{
				Vecs = new Dictionary<int, IVector3Value>();
			}
			foreach (KeyValuePair<int, IVector3Value> vec in data.Vecs)
			{
				Vecs[vec.Key] = vec.Value;
			}
		}
		if (data.Quats != null)
		{
			if (Quats == null)
			{
				Quats = new Dictionary<int, IQuaternionValue>();
			}
			foreach (KeyValuePair<int, IQuaternionValue> quat in data.Quats)
			{
				Quats[quat.Key] = quat.Value;
			}
		}
		if (data.Ints != null)
		{
			if (Ints == null)
			{
				Ints = new Dictionary<int, IIntValue>();
			}
			foreach (KeyValuePair<int, IIntValue> @int in data.Ints)
			{
				Ints[@int.Key] = @int.Value;
			}
		}
		if (data.Strings != null)
		{
			if (Strings == null)
			{
				Strings = new Dictionary<int, IStringValue>();
			}
			foreach (KeyValuePair<int, IStringValue> @string in data.Strings)
			{
				Strings[@string.Key] = @string.Value;
			}
		}
		if (data.ByteArrays != null)
		{
			if (ByteArrays == null)
			{
				ByteArrays = new Dictionary<int, IBytesValue>();
			}
			foreach (KeyValuePair<int, IBytesValue> byteArray in data.ByteArrays)
			{
				ByteArrays[byteArray.Key] = byteArray.Value;
			}
		}
		if (data.Longs != null)
		{
			if (Longs == null)
			{
				Longs = new Dictionary<int, ILongValue>();
			}
			foreach (KeyValuePair<int, ILongValue> @long in data.Longs)
			{
				Longs[@long.Key] = @long.Value;
			}
		}
		if (data.Bools != null)
		{
			if (Bools == null)
			{
				Bools = new Dictionary<int, IBoolValue>();
			}
			foreach (KeyValuePair<int, IBoolValue> @bool in data.Bools)
			{
				Bools[@bool.Key] = @bool.Value;
			}
		}
		if (data.Hashes != null)
		{
			if (Hashes == null)
			{
				Hashes = new Dictionary<int, IHashValue>();
			}
			foreach (KeyValuePair<int, IHashValue> hash in data.Hashes)
			{
				Hashes[hash.Key] = hash.Value;
			}
		}
		if (data.Components != null)
		{
			if (Components == null)
			{
				Components = new Dictionary<int, IIntValue>();
			}
			foreach (KeyValuePair<int, IIntValue> component in data.Components)
			{
				Components[component.Key] = component.Value;
			}
		}
		if (data.Items != null)
		{
			if (Items == null)
			{
				Items = new List<ItemValue>();
			}
			foreach (ItemValue item in data.Items)
			{
				Items.Add(item);
			}
		}
		if (data.Item != null)
		{
			Item = data.Item;
		}
		if (data.ContainerSize.HasValue)
		{
			ContainerSize = data.ContainerSize;
		}
		if (data.ItemAmount != null)
		{
			ItemAmount = data.ItemAmount;
		}
		if (data.ConnectionType.HasValue)
		{
			ConnectionType = data.ConnectionType;
			ConnectionHash = data.ConnectionHash;
		}
		if (data.OriginalId != null)
		{
			OriginalId = data.OriginalId;
		}
		if (data.TargetConnectionId != null)
		{
			TargetConnectionId = data.TargetConnectionId;
		}
		if (data.Persistent != null)
		{
			Persistent = data.Persistent;
		}
		if (data.Distant != null)
		{
			Distant = data.Distant;
		}
		if (data.Priority.HasValue)
		{
			Priority = data.Priority;
		}
		if (data.Position != null)
		{
			Position = data.Position;
		}
		if (data.Rotation != null)
		{
			Rotation = data.Rotation;
		}
		CanBeInjected &= data.CanBeInjected;
	}

	public DataEntry Reset(DataYaml data)
	{
		CanBeInjected = true;
		Floats = null;
		Vecs = null;
		Quats = null;
		Ints = null;
		Strings = null;
		ByteArrays = null;
		Longs = null;
		Bools = null;
		Hashes = null;
		Items = null;
		Item = null;
		Components = null;
		ContainerSize = null;
		ItemAmount = null;
		ConnectionType = null;
		ConnectionHash = 0;
		OriginalId = null;
		TargetConnectionId = null;
		Position = null;
		Rotation = null;
		Distant = null;
		Persistent = null;
		Priority = null;
		Load(data);
		return this;
	}

	private bool CheckCanBeInjected()
	{
		if ((Ints == null || (!Ints.ContainsKey(HasFieldsHash) && !Ints.ContainsKey(ZDOVars.s_level))) && Components == null && Position == null)
		{
			return Rotation == null;
		}
		return false;
	}

	public byte[]? CreateItemData(Functions f, ZDO? zdo)
	{
		if (zdo != null)
		{
			return Item?.Create(f, zdo);
		}
		return null;
	}

	public byte[]? CreateInventory(Functions f, ZDO? zdo)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		if (Items == null || Items.Count == 0)
		{
			return null;
		}
		Vector2i size = (Vector2i)(((_003F?)ContainerSize) ?? ZdoHelper.GetInventorySize(this, f, zdo));
		return ItemValue.LoadItemBytes(f, Items, size, (ItemAmount?.Get(f)).GetValueOrDefault());
	}

	public void Load(ZDO zdo)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		ZDOID uid = zdo.m_uid;
		Floats = (ZDOExtraData.s_floats.TryGetValue(uid, out var value) ? ((IEnumerable<KeyValuePair<int, float>>)value).ToDictionary((KeyValuePair<int, float> kvp) => kvp.Key, (KeyValuePair<int, float> kvp) => DataValue.Constant(kvp.Value)) : null);
		Ints = (ZDOExtraData.s_ints.TryGetValue(uid, out var value2) ? ((IEnumerable<KeyValuePair<int, int>>)value2).ToDictionary((KeyValuePair<int, int> kvp) => kvp.Key, (KeyValuePair<int, int> kvp) => DataValue.Constant(kvp.Value)) : null);
		Longs = (ZDOExtraData.s_longs.TryGetValue(uid, out var value3) ? ((IEnumerable<KeyValuePair<int, long>>)value3).ToDictionary((KeyValuePair<int, long> kvp) => kvp.Key, (KeyValuePair<int, long> kvp) => DataValue.Constant(kvp.Value)) : null);
		Strings = (ZDOExtraData.s_strings.TryGetValue(uid, out var value4) ? ((IEnumerable<KeyValuePair<int, string>>)value4).ToDictionary((KeyValuePair<int, string> kvp) => kvp.Key, (KeyValuePair<int, string> kvp) => DataValue.Constant(kvp.Value)) : null);
		Vecs = (ZDOExtraData.s_vec3.TryGetValue(uid, out var value5) ? ((IEnumerable<KeyValuePair<int, Vector3>>)value5).ToDictionary((KeyValuePair<int, Vector3> kvp) => kvp.Key, (KeyValuePair<int, Vector3> kvp) => DataValue.Constant(kvp.Value)) : null);
		Quats = (ZDOExtraData.s_quats.TryGetValue(uid, out var value6) ? ((IEnumerable<KeyValuePair<int, Quaternion>>)value6).ToDictionary((KeyValuePair<int, Quaternion> kvp) => kvp.Key, (KeyValuePair<int, Quaternion> kvp) => DataValue.Constant(kvp.Value)) : null);
		ByteArrays = (ZDOExtraData.s_byteArrays.TryGetValue(uid, out var value7) ? ((IEnumerable<KeyValuePair<int, byte[]>>)value7).ToDictionary((KeyValuePair<int, byte[]> kvp) => kvp.Key, (KeyValuePair<int, byte[]> kvp) => DataValue.Constant(kvp.Value)) : null);
		if (ZDOExtraData.s_connectionsHashData.TryGetValue(uid, out var value8))
		{
			ConnectionType = value8.m_type;
			ConnectionHash = value8.m_hash;
		}
		OriginalId = new ConstantZdoIdValue(uid);
		if (ZDOExtraData.s_connections.TryGetValue(uid, out var value9) && value9.m_target != ZDOID.None)
		{
			TargetConnectionId = new ConstantZdoIdValue(value9.m_target);
			ConnectionType = value9.m_type;
		}
		Persistent = null;
		Distant = null;
		Priority = null;
		CanBeInjected = CheckCanBeInjected();
	}

	public void Load(DataYaml data)
	{
		//IL_0ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5c: Unknown result type (might be due to invalid IL or missing references)
		HashSet<string> hashSet = new HashSet<string>();
		if (data.floats != null)
		{
			if (Floats == null)
			{
				Floats = new Dictionary<int, IFloatValue>();
			}
			string[] floats = data.floats;
			foreach (string text in floats)
			{
				KeyValuePair<string, string> keyValuePair = Parse.Kvp(text);
				if (keyValuePair.Key == "")
				{
					throw new InvalidOperationException("Failed to parse float " + text + ".");
				}
				if (keyValuePair.Key.Contains("."))
				{
					hashSet.Add(keyValuePair.Key.Split('.')[0]);
				}
				int key = ZdoHelper.Hash(keyValuePair.Key);
				if (Floats.ContainsKey(key))
				{
					Log.Warning("Data " + data.name + ": Duplicate float key " + keyValuePair.Key + ".");
				}
				Floats[key] = DataValue.Float(keyValuePair.Value);
			}
		}
		if (data.ints != null)
		{
			if (Ints == null)
			{
				Ints = new Dictionary<int, IIntValue>();
			}
			string[] floats = data.ints;
			foreach (string text2 in floats)
			{
				KeyValuePair<string, string> keyValuePair2 = Parse.Kvp(text2);
				if (keyValuePair2.Key == "")
				{
					throw new InvalidOperationException("Failed to parse int " + text2 + ".");
				}
				if (keyValuePair2.Key.Contains("."))
				{
					hashSet.Add(keyValuePair2.Key.Split('.')[0]);
				}
				int key2 = ZdoHelper.Hash(keyValuePair2.Key);
				if (Ints.ContainsKey(key2))
				{
					Log.Warning("Data " + data.name + ": Duplicate int key " + keyValuePair2.Key + ".");
				}
				Ints[key2] = DataValue.Int(keyValuePair2.Value);
			}
		}
		if (data.bools != null)
		{
			if (Bools == null)
			{
				Bools = new Dictionary<int, IBoolValue>();
			}
			string[] floats = data.bools;
			foreach (string text3 in floats)
			{
				KeyValuePair<string, string> keyValuePair3 = Parse.Kvp(text3);
				if (keyValuePair3.Key == "")
				{
					throw new InvalidOperationException("Failed to parse bool " + text3 + ".");
				}
				if (keyValuePair3.Key.Contains("."))
				{
					hashSet.Add(keyValuePair3.Key.Split('.')[0]);
				}
				int key3 = ZdoHelper.Hash(keyValuePair3.Key);
				if (Bools.ContainsKey(key3))
				{
					Log.Warning("Data " + data.name + ": Duplicate bool key " + keyValuePair3.Key + ".");
				}
				Bools[key3] = DataValue.Bool(keyValuePair3.Value);
			}
		}
		if (data.hashes != null)
		{
			if (Hashes == null)
			{
				Hashes = new Dictionary<int, IHashValue>();
			}
			string[] floats = data.hashes;
			foreach (string text4 in floats)
			{
				KeyValuePair<string, string> keyValuePair4 = Parse.Kvp(text4);
				if (keyValuePair4.Key == "")
				{
					throw new InvalidOperationException("Failed to parse hash " + text4 + ".");
				}
				if (keyValuePair4.Key.Contains("."))
				{
					hashSet.Add(keyValuePair4.Key.Split('.')[0]);
				}
				int key4 = ZdoHelper.Hash(keyValuePair4.Key);
				if (Hashes.ContainsKey(key4))
				{
					Log.Warning("Data " + data.name + ": Duplicate hash key " + keyValuePair4.Key + ".");
				}
				Hashes[key4] = DataValue.Hash(keyValuePair4.Value);
			}
		}
		if (data.longs != null)
		{
			if (Longs == null)
			{
				Longs = new Dictionary<int, ILongValue>();
			}
			string[] floats = data.longs;
			foreach (string text5 in floats)
			{
				KeyValuePair<string, string> keyValuePair5 = Parse.Kvp(text5);
				if (keyValuePair5.Key == "")
				{
					throw new InvalidOperationException("Failed to parse long " + text5 + ".");
				}
				if (keyValuePair5.Key.Contains("."))
				{
					hashSet.Add(keyValuePair5.Key.Split('.')[0]);
				}
				int key5 = ZdoHelper.Hash(keyValuePair5.Key);
				if (Longs.ContainsKey(key5))
				{
					Log.Warning("Data " + data.name + ": Duplicate long key " + keyValuePair5.Key + ".");
				}
				Longs[key5] = DataValue.Long(keyValuePair5.Value);
			}
		}
		if (data.strings != null)
		{
			if (Strings == null)
			{
				Strings = new Dictionary<int, IStringValue>();
			}
			string[] floats = data.strings;
			foreach (string text6 in floats)
			{
				KeyValuePair<string, string> keyValuePair6 = Parse.Kvp(text6);
				if (keyValuePair6.Key == "")
				{
					throw new InvalidOperationException("Failed to parse string " + text6 + ".");
				}
				if (keyValuePair6.Key.Contains("."))
				{
					hashSet.Add(keyValuePair6.Key.Split('.')[0]);
				}
				int num = ZdoHelper.Hash(keyValuePair6.Key);
				if (num == ZDOVars.s_items)
				{
					if (ByteArrays == null)
					{
						ByteArrays = new Dictionary<int, IBytesValue>();
					}
					if (ByteArrays.ContainsKey(num))
					{
						Log.Warning("Data " + data.name + ": Duplicate string key " + keyValuePair6.Key + ".");
					}
					ByteArrays[num] = DataValue.Bytes(keyValuePair6.Value);
				}
				else
				{
					if (Strings.ContainsKey(num))
					{
						Log.Warning("Data " + data.name + ": Duplicate string key " + keyValuePair6.Key + ".");
					}
					Strings[num] = DataValue.String(keyValuePair6.Value);
				}
			}
		}
		if (data.vecs != null)
		{
			if (Vecs == null)
			{
				Vecs = new Dictionary<int, IVector3Value>();
			}
			string[] floats = data.vecs;
			foreach (string text7 in floats)
			{
				KeyValuePair<string, string> keyValuePair7 = Parse.Kvp(text7);
				if (keyValuePair7.Key == "")
				{
					throw new InvalidOperationException("Failed to parse vector " + text7 + ".");
				}
				if (keyValuePair7.Key.Contains("."))
				{
					hashSet.Add(keyValuePair7.Key.Split('.')[0]);
				}
				int key6 = ZdoHelper.Hash(keyValuePair7.Key);
				if (Vecs.ContainsKey(key6))
				{
					Log.Warning("Data " + data.name + ": Duplicate vector key " + keyValuePair7.Key + ".");
				}
				Vecs[key6] = DataValue.Vector3(keyValuePair7.Value);
			}
		}
		if (data.quats != null)
		{
			if (Quats == null)
			{
				Quats = new Dictionary<int, IQuaternionValue>();
			}
			string[] floats = data.quats;
			foreach (string text8 in floats)
			{
				KeyValuePair<string, string> keyValuePair8 = Parse.Kvp(text8);
				if (keyValuePair8.Key == "")
				{
					throw new InvalidOperationException("Failed to parse quaternion " + text8 + ".");
				}
				if (keyValuePair8.Key.Contains("."))
				{
					hashSet.Add(keyValuePair8.Key.Split('.')[0]);
				}
				int key7 = ZdoHelper.Hash(keyValuePair8.Key);
				if (Quats.ContainsKey(key7))
				{
					Log.Warning("Data " + data.name + ": Duplicate quaternion key " + keyValuePair8.Key + ".");
				}
				Quats[key7] = DataValue.Quaternion(keyValuePair8.Value);
			}
		}
		if (data.bytes != null)
		{
			if (ByteArrays == null)
			{
				ByteArrays = new Dictionary<int, IBytesValue>();
			}
			string[] floats = data.bytes;
			foreach (string text9 in floats)
			{
				KeyValuePair<string, string> keyValuePair9 = Parse.Kvp(text9);
				if (keyValuePair9.Key == "")
				{
					throw new InvalidOperationException("Failed to parse byte array " + text9 + ".");
				}
				if (keyValuePair9.Key.Contains("."))
				{
					hashSet.Add(keyValuePair9.Key.Split('.')[0]);
				}
				int key8 = ZdoHelper.Hash(keyValuePair9.Key);
				if (ByteArrays.ContainsKey(key8))
				{
					Log.Warning("Data " + data.name + ": Duplicate byte array key " + keyValuePair9.Key + ".");
				}
				ByteArrays[key8] = DataValue.Bytes(keyValuePair9.Value);
			}
		}
		if (data.items != null)
		{
			List<ItemValue> list = new List<ItemValue>();
			list.AddRange(data.items.Select((ItemYaml item) => new ItemValue(item)));
			Items = list;
		}
		if (data.item != null)
		{
			Item = new ItemValue(data.item);
		}
		if (!string.IsNullOrWhiteSpace(data.containerSize))
		{
			ContainerSize = Parse.Vector2Int(data.containerSize);
		}
		if (!string.IsNullOrWhiteSpace(data.itemAmount))
		{
			ItemAmount = DataValue.Int(data.itemAmount);
		}
		CanBeInjected = hashSet.Count == 0;
		if (hashSet.Count > 0)
		{
			if (Components == null)
			{
				Components = new Dictionary<int, IIntValue>();
			}
			Components[ZdoHelper.Hash("HasFields")] = DataValue.Constant(1);
			foreach (string item in hashSet)
			{
				Components[ZdoHelper.Hash("HasFields" + item)] = DataValue.Constant(1);
			}
		}
		if (!string.IsNullOrWhiteSpace(data.position))
		{
			Position = DataValue.Vector3(data.position);
		}
		if (!string.IsNullOrWhiteSpace(data.rotation))
		{
			Rotation = DataValue.Quaternion(data.rotation);
		}
		if (data.persistent != null)
		{
			Persistent = DataValue.Bool(data.persistent);
		}
		if (data.distant != null)
		{
			Distant = DataValue.Bool(data.distant);
		}
		if (data.priority != null)
		{
			Priority = (Enum.TryParse<ObjectType>(data.priority, ignoreCase: true, out ObjectType result) ? new ObjectType?(result) : null);
		}
		if (string.IsNullOrWhiteSpace(data.connection))
		{
			return;
		}
		string[] array = Parse.SplitWithEmpty(data.connection);
		if (array.Length == 1)
		{
			string[] floats = array;
			List<string> list2 = new List<string>(floats.Length);
			list2.AddRange(floats);
			ConnectionType = ToByteEnum<ConnectionType>(list2);
			return;
		}
		List<string> list3 = array.Take(array.Length - 1).ToList();
		string text10 = array[array.Length - 1];
		ConnectionType = ToByteEnum<ConnectionType>(list3);
		if (text10.Contains(":") || text10.Contains("<"))
		{
			TargetConnectionId = DataValue.ZdoId(text10);
			OriginalId = TargetConnectionId;
			return;
		}
		ConnectionHash = Parse.Int(text10);
		if (ConnectionHash == 0)
		{
			ConnectionHash = StringExtensionMethods.GetStableHashCode(text10);
		}
	}

	public void Load(string[] typeKeyValues)
	{
		if (typeKeyValues.Length != 3)
		{
			throw new InvalidOperationException("Failed to parse type, field, value.");
		}
		string text = typeKeyValues[0].ToLowerInvariant();
		string text2 = typeKeyValues[1];
		string values = typeKeyValues[2];
		if (text2.Contains("."))
		{
			CanBeInjected = false;
			string text3 = text2.Split('.')[0];
			if (Ints == null)
			{
				Ints = new Dictionary<int, IIntValue>();
			}
			Ints[ZdoHelper.Hash("HasFields")] = DataValue.Constant(1);
			Ints[ZdoHelper.Hash("HasFields" + text3)] = DataValue.Constant(1);
		}
		switch (text)
		{
		case "float":
			if (Floats == null)
			{
				Floats = new Dictionary<int, IFloatValue>();
			}
			Floats[ZdoHelper.Hash(text2)] = DataValue.Float(values);
			break;
		case "int":
			if (Ints == null)
			{
				Ints = new Dictionary<int, IIntValue>();
			}
			Ints[ZdoHelper.Hash(text2)] = DataValue.Int(values);
			break;
		case "bool":
			if (Bools == null)
			{
				Bools = new Dictionary<int, IBoolValue>();
			}
			Bools[ZdoHelper.Hash(text2)] = DataValue.Bool(values);
			break;
		case "hash":
			if (Hashes == null)
			{
				Hashes = new Dictionary<int, IHashValue>();
			}
			Hashes[ZdoHelper.Hash(text2)] = DataValue.Hash(values);
			break;
		case "long":
			if (Longs == null)
			{
				Longs = new Dictionary<int, ILongValue>();
			}
			Longs[ZdoHelper.Hash(text2)] = DataValue.Long(values);
			break;
		case "string":
			if (Strings == null)
			{
				Strings = new Dictionary<int, IStringValue>();
			}
			Strings[ZdoHelper.Hash(text2)] = DataValue.String(values);
			break;
		case "vec":
		case "vec3":
			if (Vecs == null)
			{
				Vecs = new Dictionary<int, IVector3Value>();
			}
			Vecs[ZdoHelper.Hash(text2)] = DataValue.Vector3(values);
			break;
		case "quat":
			if (Quats == null)
			{
				Quats = new Dictionary<int, IQuaternionValue>();
			}
			Quats[ZdoHelper.Hash(text2)] = DataValue.Quaternion(values);
			break;
		case "bytes":
			if (ByteArrays == null)
			{
				ByteArrays = new Dictionary<int, IBytesValue>();
			}
			ByteArrays[ZdoHelper.Hash(text2)] = DataValue.Bytes(values);
			break;
		default:
			throw new InvalidOperationException("Unknown type " + text + ".");
		}
	}

	public void Load(ZPackage pkg)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		pkg.SetPos(0);
		int num = pkg.ReadInt();
		if (((uint)num & (true ? 1u : 0u)) != 0)
		{
			if (Floats == null)
			{
				Floats = new Dictionary<int, IFloatValue>();
			}
			byte b = pkg.ReadByte();
			for (int i = 0; i < b; i++)
			{
				Floats[pkg.ReadInt()] = new ConstantFloatValue(pkg.ReadSingle());
			}
		}
		if (((uint)num & 2u) != 0)
		{
			if (Vecs == null)
			{
				Vecs = new Dictionary<int, IVector3Value>();
			}
			byte b2 = pkg.ReadByte();
			for (int j = 0; j < b2; j++)
			{
				Vecs[pkg.ReadInt()] = new ConstantVector3Value(pkg.ReadVector3());
			}
		}
		if (((uint)num & 4u) != 0)
		{
			if (Quats == null)
			{
				Quats = new Dictionary<int, IQuaternionValue>();
			}
			byte b3 = pkg.ReadByte();
			for (int k = 0; k < b3; k++)
			{
				Quats[pkg.ReadInt()] = new ConstantQuaternionValue(pkg.ReadQuaternion());
			}
		}
		if (((uint)num & 8u) != 0)
		{
			if (Ints == null)
			{
				Ints = new Dictionary<int, IIntValue>();
			}
			byte b4 = pkg.ReadByte();
			for (int l = 0; l < b4; l++)
			{
				Ints[pkg.ReadInt()] = new ConstantIntValue(pkg.ReadInt());
			}
		}
		if (((uint)num & 0x40u) != 0)
		{
			if (Longs == null)
			{
				Longs = new Dictionary<int, ILongValue>();
			}
			byte b5 = pkg.ReadByte();
			for (int m = 0; m < b5; m++)
			{
				Longs[pkg.ReadInt()] = new ConstantLongValue(pkg.ReadLong());
			}
		}
		if (((uint)num & 0x10u) != 0)
		{
			if (Strings == null)
			{
				Strings = new Dictionary<int, IStringValue>();
			}
			byte b6 = pkg.ReadByte();
			for (int n = 0; n < b6; n++)
			{
				Strings[pkg.ReadInt()] = new ConstantStringValue(pkg.ReadString());
			}
		}
		if (((uint)num & 0x80u) != 0)
		{
			if (ByteArrays == null)
			{
				ByteArrays = new Dictionary<int, IBytesValue>();
			}
			byte b7 = pkg.ReadByte();
			for (int num2 = 0; num2 < b7; num2++)
			{
				ByteArrays[pkg.ReadInt()] = new ConstantBytesValue(pkg.ReadByteArray());
			}
		}
		if (((uint)num & 0x100u) != 0)
		{
			ConnectionType = (ConnectionType)pkg.ReadByte();
			ConnectionHash = pkg.ReadInt();
		}
		if (((uint)num & 0x200u) != 0)
		{
			Persistent = new ConstantBoolValue(pkg.ReadBool());
		}
		if (((uint)num & 0x400u) != 0)
		{
			Distant = new ConstantBoolValue(pkg.ReadBool());
		}
		if (((uint)num & 0x800u) != 0)
		{
			Priority = (ObjectType)pkg.ReadByte();
		}
		CanBeInjected = CheckCanBeInjected();
	}

	private static T ToByteEnum<T>(List<string> list) where T : struct, Enum
	{
		byte b = 0;
		foreach (string item in list)
		{
			string text = item.Trim();
			if (Enum.TryParse<T>(text, ignoreCase: true, out var result))
			{
				b += (byte)(object)result;
			}
			else
			{
				Log.Warning("Failed to parse value " + text + " as T.");
			}
		}
		return (T)(object)b;
	}

	public bool Match(Functions f, ZDO zdo)
	{
		return Matches(f, zdo, failOn: false);
	}

	public bool MatchNone(Functions f, ZDO zdo)
	{
		return Matches(f, zdo, failOn: true);
	}

	private bool Matches(Functions f, ZDO zdo, bool failOn)
	{
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		if (Fails(Strings, f, zdo, failOn, GetString, (IStringValue v, Functions ctx, string data) => v.Match(ctx, data)))
		{
			return false;
		}
		if (Fails(Floats, f, zdo, failOn, GetFloat, (IFloatValue v, Functions ctx, float data) => v.Match(ctx, data)))
		{
			return false;
		}
		if (Fails(Ints, f, zdo, failOn, GetInt, (IIntValue v, Functions ctx, int data) => v.Match(ctx, data)))
		{
			return false;
		}
		if (Fails(Longs, f, zdo, failOn, GetLong, (ILongValue v, Functions ctx, long data) => v.Match(ctx, data)))
		{
			return false;
		}
		if (Fails(Bools, f, zdo, failOn, GetBool, (IBoolValue v, Functions ctx, bool data) => v.Match(ctx, data)))
		{
			return false;
		}
		if (Fails(Hashes, f, zdo, failOn, GetInt, (IHashValue v, Functions ctx, int data) => v.Match(ctx, data)))
		{
			return false;
		}
		if (Fails(Vecs, f, zdo, failOn, GetVec, (IVector3Value v, Functions ctx, Vector3 data) => v.Match(ctx, data)))
		{
			return false;
		}
		if (Fails(Quats, f, zdo, failOn, GetQuaternion, (IQuaternionValue v, Functions ctx, Quaternion data) => v.Match(ctx, data)))
		{
			return false;
		}
		if (Fails(ByteArrays, f, zdo, failOn, (ZDO z, int key) => z.GetByteArray(key, (byte[])null), (IBytesValue v, Functions ctx, byte[] data) => v.Match(ctx, data)))
		{
			return false;
		}
		if (Persistent != null && Persistent.Match(f, zdo.Persistent) == failOn)
		{
			return false;
		}
		if (Distant != null && Distant.Match(f, zdo.Distant) == failOn)
		{
			return false;
		}
		if (Priority.HasValue && Priority.Value == zdo.Type == failOn)
		{
			return false;
		}
		if (Item != null && Item.MatchSingle(f, zdo) == failOn)
		{
			return false;
		}
		if (Items != null)
		{
			return ItemValue.Match(f, Items, zdo, ItemAmount) != failOn;
		}
		if (ItemAmount != null)
		{
			return ItemValue.Match(f, zdo, ItemAmount) != failOn;
		}
		if (ConnectionType.HasValue)
		{
			if ((int)ConnectionType.Value == 0)
			{
				ZDOConnection connection = zdo.GetConnection();
				if ((connection == null || connection.m_target == ZDOID.None) == failOn)
				{
					return false;
				}
			}
			else
			{
				ZDOID connectionZDOID = zdo.GetConnectionZDOID(ConnectionType.Value);
				if (TargetConnectionId == null)
				{
					if (connectionZDOID != ZDOID.None == failOn)
					{
						return false;
					}
				}
				else
				{
					ZDOID? val = TargetConnectionId.Get(f);
					if (val.HasValue)
					{
						ZDOID val2 = connectionZDOID;
						ZDOID? val3 = val;
						if ((val3.HasValue && val2 == val3.GetValueOrDefault()) == failOn)
						{
							return false;
						}
					}
				}
			}
		}
		return true;
	}

	private static bool Fails<TValue, TData>(Dictionary<int, TValue>? values, Functions f, ZDO zdo, bool failOn, Func<ZDO, int, TData> read, Func<TValue, Functions, TData, bool?> match)
	{
		if (values == null)
		{
			return false;
		}
		foreach (KeyValuePair<int, TValue> value in values)
		{
			if (match(value.Value, f, read(zdo, value.Key)) == failOn)
			{
				return true;
			}
		}
		return false;
	}

	private static string GetString(ZDO zdo, int key)
	{
		return ZdoHelper.TryGetString(zdo, key) ?? "";
	}

	private static float GetFloat(ZDO zdo, int key)
	{
		return ZdoHelper.TryGetFloat(zdo, key).GetValueOrDefault();
	}

	private static int GetInt(ZDO zdo, int key)
	{
		return ZdoHelper.TryGetInt(zdo, key).GetValueOrDefault();
	}

	private static long GetLong(ZDO zdo, int key)
	{
		return ZdoHelper.TryGetLong(zdo, key).GetValueOrDefault();
	}

	private static bool GetBool(ZDO zdo, int key)
	{
		return ZdoHelper.TryGetBool(zdo, key).GetValueOrDefault();
	}

	private static Vector3 GetVec(ZDO zdo, int key)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		return (Vector3)(((_003F?)ZdoHelper.TryGetVec(zdo, key)) ?? Vector3.zero);
	}

	private static Quaternion GetQuaternion(ZDO zdo, int key)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		return (Quaternion)(((_003F?)ZdoHelper.TryGetQuaternion(zdo, key)) ?? Quaternion.identity);
	}
}
