using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data;

public static class ItemDataHelper
{
	public const long SyntheticCrafterId = 1L;

	private static readonly Dictionary<int, (int Index, bool IsValue)> CustomDataKeys = BuildCustomDataKeys();

	private static readonly int[] HashKeys;

	public static long EnsureVisibleCrafterId(long crafterId, string crafterName)
	{
		if (crafterId != 0L || string.IsNullOrWhiteSpace(crafterName))
		{
			return crafterId;
		}
		return 1L;
	}

	private static bool TryLoadDrop(ZDO zdo, out ItemData itemData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		itemData = new ItemData();
		byte[] byteArray = zdo.GetByteArray(ZDOVars.s_itemData, (byte[])null);
		if (byteArray == null || byteArray.Length <= 2)
		{
			return false;
		}
		ZPackage val = new ZPackage(byteArray);
		Item val2 = (Item)val.ReadByte();
		itemData = ItemData.Load(val, val2).Item2;
		return true;
	}

	public static ItemRecord? LoadSingle(ZDO zdo)
	{
		if (!TryLoadDrop(zdo, out ItemData itemData))
		{
			return null;
		}
		return ToRecord(zdo.m_prefab, itemData);
	}

	public static ItemRecord? LoadItem(byte[] bytes)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (bytes.Length <= 2)
		{
			return null;
		}
		try
		{
			ZPackage val = new ZPackage(bytes);
			Item val2 = (Item)val.ReadByte();
			var (hash, item) = ItemData.Load(val, val2);
			return ToRecord(hash, item);
		}
		catch
		{
			return null;
		}
	}

	private static ItemRecord ToRecord(int hash, ItemData item)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		ItemRecord obj = new ItemRecord
		{
			PrefabHash = hash
		};
		object prefabName;
		if (hash == 0)
		{
			prefabName = "";
		}
		else
		{
			GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(hash);
			prefabName = ((itemPrefab != null) ? ((Object)itemPrefab).name : null) ?? "";
		}
		obj.PrefabName = (string)prefabName;
		obj.Stack = item.m_stack;
		obj.Durability = item.m_durability;
		obj.GridPos = item.m_gridPos;
		obj.Equipped = item.m_equipped;
		obj.Quality = item.m_quality;
		obj.Variant = item.m_variant;
		obj.CrafterID = item.m_crafterID;
		obj.CrafterName = item.m_crafterName;
		obj.CustomData = item.m_customData;
		obj.WorldLevel = item.m_worldLevel;
		obj.PickedUp = item.m_pickedUp;
		obj.Cheated = item.m_cheated;
		return obj;
	}

	public static byte[] Serialize(ItemData itemData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ZPackage val = new ZPackage();
		val.Write((byte)109);
		itemData.Save(val);
		return val.GetArray();
	}

	private static Dictionary<int, (int, bool)> BuildCustomDataKeys()
	{
		Dictionary<int, (int, bool)> dictionary = new Dictionary<int, (int, bool)>();
		for (int i = 0; i < 32; i++)
		{
			dictionary[StringExtensionMethods.GetStableHashCode($"data_{i}")] = (i, false);
			dictionary[StringExtensionMethods.GetStableHashCode($"data__{i}")] = (i, true);
		}
		return dictionary;
	}

	public static bool TryGetString(ZDO zdo, int hash, out string value)
	{
		value = "";
		bool flag = hash == ZDOVars.s_crafterName;
		(int, bool) value2;
		bool flag2 = CustomDataKeys.TryGetValue(hash, out value2);
		if (!flag && !flag2)
		{
			return false;
		}
		if (!TryLoadDrop(zdo, out ItemData itemData))
		{
			return false;
		}
		if (flag)
		{
			value = itemData.m_crafterName;
			return true;
		}
		if (value2.Item1 >= itemData.m_customData.Count)
		{
			return false;
		}
		KeyValuePair<string, string> keyValuePair = itemData.m_customData.ElementAt(value2.Item1);
		value = (value2.Item2 ? keyValuePair.Value : keyValuePair.Key);
		return true;
	}

	public static bool TryGetFloat(ZDO zdo, int hash, out float value)
	{
		value = 0f;
		if (hash != ZDOVars.s_durability)
		{
			return false;
		}
		if (!TryLoadDrop(zdo, out ItemData itemData))
		{
			return false;
		}
		value = itemData.m_durability;
		return true;
	}

	public static bool TryGetInt(ZDO zdo, int hash, out int value)
	{
		value = 0;
		if (hash != ZDOVars.s_stack && hash != ZDOVars.s_quality && hash != ZDOVars.s_variant && hash != ZDOVars.s_worldLevel && hash != ZDOVars.s_pickedUp && hash != ZDOVars.s_dataCount)
		{
			return false;
		}
		if (!TryLoadDrop(zdo, out ItemData itemData))
		{
			return false;
		}
		if (hash == ZDOVars.s_stack)
		{
			value = itemData.m_stack;
		}
		else if (hash == ZDOVars.s_quality)
		{
			value = itemData.m_quality;
		}
		else if (hash == ZDOVars.s_variant)
		{
			value = itemData.m_variant;
		}
		else if (hash == ZDOVars.s_worldLevel)
		{
			value = itemData.m_worldLevel;
		}
		else if (hash == ZDOVars.s_pickedUp)
		{
			value = (itemData.m_pickedUp ? 1 : 0);
		}
		else
		{
			if (hash != ZDOVars.s_dataCount)
			{
				return false;
			}
			value = itemData.m_customData.Count;
		}
		return true;
	}

	public static bool TryGetLong(ZDO zdo, int hash, out long value)
	{
		value = 0L;
		if (hash != ZDOVars.s_crafterID)
		{
			return false;
		}
		if (!TryLoadDrop(zdo, out ItemData itemData))
		{
			return false;
		}
		value = itemData.m_crafterID;
		return true;
	}

	public static ItemData Create(GameObject prefab, int stack, float? durability, int quality, int variant, long crafterId, string crafterName, int worldLevel, bool pickedUp, bool cheated, bool equipped, Dictionary<string, string>? customData)
	{
		ItemData val = prefab.GetComponent<ItemDrop>().m_itemData.Clone();
		val.m_dropPrefab = prefab;
		val.m_stack = stack;
		val.m_quality = quality;
		val.m_variant = variant;
		val.m_crafterID = EnsureVisibleCrafterId(crafterId, crafterName);
		val.m_crafterName = crafterName;
		val.m_worldLevel = worldLevel;
		val.m_durability = durability ?? val.GetMaxDurability(quality);
		val.m_equipped = equipped;
		val.m_pickedUp = pickedUp;
		val.m_cheated = cheated;
		if (customData != null)
		{
			val.m_customData = new Dictionary<string, string>(customData);
		}
		return val;
	}

	public static ZPackage? GetPackage(ZDO zdo)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		byte[] byteArray = zdo.GetByteArray(ZDOVars.s_items, (byte[])null);
		if (byteArray != null && byteArray.Length != 0)
		{
			return new ZPackage(byteArray);
		}
		string @string = zdo.GetString(ZDOVars.s_items, "");
		if (!(@string != ""))
		{
			return null;
		}
		return new ZPackage(@string);
	}

	public static List<ItemRecord> Load(ZDO zdo)
	{
		ZPackage package = GetPackage(zdo);
		if (package != null)
		{
			return Load(package);
		}
		return new List<ItemRecord>();
	}

	public static List<ItemRecord> Load(ZPackage pkg)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		List<ItemRecord> list = new List<ItemRecord>();
		try
		{
			Item val = (Item)pkg.ReadInt();
			if ((int)val == 0)
			{
				return list;
			}
			if ((int)val >= 108)
			{
				LoadNew(pkg, val, list);
			}
			else
			{
				LoadOld(pkg, val, list);
			}
		}
		catch
		{
		}
		return list;
	}

	private static void LoadNew(ZPackage pkg, Item version, List<ItemRecord> records)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		ushort num = pkg.ReadUShort();
		for (int i = 0; i < num; i++)
		{
			var (hash, item) = ItemData.Load(pkg, version);
			records.Add(ToRecord(hash, item));
		}
	}

	private static void LoadOld(ZPackage pkg, Item version, List<ItemRecord> records)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Invalid comparison between Unknown and I4
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Invalid comparison between Unknown and I4
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Invalid comparison between Unknown and I4
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Invalid comparison between Unknown and I4
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		int num = pkg.ReadInt();
		for (int i = 0; i < num; i++)
		{
			string text = pkg.ReadString();
			int stack = pkg.ReadInt();
			float durability = pkg.ReadSingle();
			Vector2i gridPos = pkg.ReadVector2i();
			bool equipped = pkg.ReadBool();
			int quality = (((int)version < 101) ? 1 : pkg.ReadInt());
			int variant = (((int)version >= 102) ? pkg.ReadInt() : 0);
			long crafterID = 0L;
			string crafterName = "";
			if ((int)version >= 103)
			{
				crafterID = pkg.ReadLong();
				crafterName = pkg.ReadString();
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if ((int)version >= 104)
			{
				int num2 = pkg.ReadInt();
				for (int j = 0; j < num2; j++)
				{
					dictionary[pkg.ReadString()] = pkg.ReadString();
				}
			}
			int worldLevel = (((int)version >= 105) ? pkg.ReadInt() : 0);
			bool pickedUp = (int)version >= 106 && pkg.ReadBool();
			bool cheated = (int)version == 107 && pkg.ReadBool();
			int num3 = ((text != "") ? StringExtensionMethods.GetStableHashCode(text) : 0);
			object obj;
			if (num3 == 0)
			{
				obj = "";
			}
			else
			{
				GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(num3);
				obj = ((itemPrefab != null) ? ((Object)itemPrefab).name : null) ?? "";
			}
			string prefabName = (string)obj;
			records.Add(new ItemRecord
			{
				PrefabHash = num3,
				PrefabName = prefabName,
				Stack = stack,
				Durability = durability,
				GridPos = gridPos,
				Equipped = equipped,
				Quality = quality,
				Variant = variant,
				CrafterID = crafterID,
				CrafterName = crafterName,
				CustomData = dictionary,
				WorldLevel = worldLevel,
				PickedUp = pickedUp,
				Cheated = cheated
			});
		}
	}

	public static int CountInvalid(List<ItemRecord> records)
	{
		return records.Count((ItemRecord r) => r.PrefabName == "");
	}

	public static List<ItemRecord> RemoveInvalid(List<ItemRecord> records)
	{
		List<ItemRecord> list = new List<ItemRecord>();
		list.AddRange(records.Where((ItemRecord r) => r.PrefabName != ""));
		return list;
	}

	public static byte[] Save(List<ItemRecord> records)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ZPackage val = new ZPackage();
		Save(records, val);
		return val.GetArray();
	}

	public static void SaveTo(ZDO zdo, List<ItemRecord> records)
	{
		zdo.Set(ZDOVars.s_items, Save(records));
	}

	public static Vector2i? FindFreeSlot(List<ItemRecord> records, Vector2i size)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < size.y; i++)
		{
			for (int j = 0; j < size.x; j++)
			{
				Vector2i pos = new Vector2i(j, i);
				if (!records.Any((ItemRecord r) => r.GridPos == pos))
				{
					return pos;
				}
			}
		}
		return null;
	}

	public static int GetMaxStackSize(int prefabHash)
	{
		GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(prefabHash);
		return ((itemPrefab == null) ? null : itemPrefab.GetComponent<ItemDrop>()?.m_itemData.m_shared.m_maxStackSize).GetValueOrDefault(1);
	}

	public static void Save(List<ItemRecord> records, ZPackage pkg)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		pkg.Write(109);
		pkg.Write((ushort)records.Count);
		foreach (ItemRecord record in records)
		{
			new ItemData
			{
				m_dropPrefab = ObjectDB.instance.GetItemPrefab(record.PrefabHash),
				m_stack = record.Stack,
				m_durability = record.Durability,
				m_gridPos = record.GridPos,
				m_equipped = record.Equipped,
				m_quality = record.Quality,
				m_variant = record.Variant,
				m_crafterID = record.CrafterID,
				m_crafterName = record.CrafterName,
				m_customData = record.CustomData,
				m_worldLevel = record.WorldLevel,
				m_pickedUp = record.PickedUp,
				m_cheated = record.Cheated
			}.Save(pkg);
		}
	}

	public static void ConvertAll(ResolvedDataEntry data)
	{
		ConvertInventory(data);
		ConvertItemNames(data);
		ConvertItemData(data);
	}

	public static void ConvertInventory(ResolvedDataEntry data)
	{
		if (data.Strings == null || !data.Strings.TryGetValue(ZDOVars.s_items, out string value))
		{
			return;
		}
		if (!string.IsNullOrEmpty(value))
		{
			if (data.ByteArrays == null)
			{
				data.ByteArrays = new Dictionary<int, byte[]>();
			}
			data.ByteArrays[ZDOVars.s_items] = Convert.FromBase64String(value);
		}
		data.Strings.Remove(ZDOVars.s_items);
		if (data.Strings.Count == 0)
		{
			data.Strings = null;
		}
	}

	public static void ConvertItemNames(ResolvedDataEntry data)
	{
		if (data.Strings == null)
		{
			return;
		}
		int[] hashKeys = HashKeys;
		foreach (int key in hashKeys)
		{
			if (!data.Strings.TryGetValue(key, out string value))
			{
				continue;
			}
			if (!string.IsNullOrEmpty(value))
			{
				if (data.Ints == null)
				{
					data.Ints = new Dictionary<int, int>();
				}
				data.Ints[key] = StringExtensionMethods.GetStableHashCode(value);
			}
			data.Strings.Remove(key);
		}
		if (data.Strings.Count == 0)
		{
			data.Strings = null;
		}
	}

	private static bool HasLegacyItemData(ResolvedDataEntry data)
	{
		Dictionary<int, float>? floats = data.Floats;
		if (floats == null || !floats.ContainsKey(ZDOVars.s_durability))
		{
			Dictionary<int, int>? ints = data.Ints;
			if (ints == null || !ints.ContainsKey(ZDOVars.s_stack))
			{
				Dictionary<int, int>? ints2 = data.Ints;
				if (ints2 == null || !ints2.ContainsKey(ZDOVars.s_quality))
				{
					Dictionary<int, int>? ints3 = data.Ints;
					if (ints3 == null || !ints3.ContainsKey(ZDOVars.s_variant))
					{
						Dictionary<int, long>? longs = data.Longs;
						if (longs == null || !longs.ContainsKey(ZDOVars.s_crafterID))
						{
							Dictionary<int, string>? strings = data.Strings;
							if (strings == null || !strings.ContainsKey(ZDOVars.s_crafterName))
							{
								Dictionary<int, int>? ints4 = data.Ints;
								if (ints4 == null || !ints4.ContainsKey(ZDOVars.s_dataCount))
								{
									Dictionary<int, int>? ints5 = data.Ints;
									if (ints5 == null || !ints5.ContainsKey(ZDOVars.s_worldLevel))
									{
										Dictionary<int, int>? ints6 = data.Ints;
										if (ints6 == null || !ints6.ContainsKey(ZDOVars.s_pickedUp))
										{
											return data.Ints?.ContainsKey(ZDOVars.s_cheated) ?? false;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return true;
	}

	public static void ConvertItemData(ResolvedDataEntry data)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		if (HasLegacyItemData(data))
		{
			byte[] value = null;
			data.ByteArrays?.TryGetValue(ZDOVars.s_itemData, out value);
			ItemData val = ParseLegacyItemData(data, value);
			ZPackage val2 = new ZPackage();
			val2.Write((byte)109);
			val.Save(val2);
			if (data.ByteArrays == null)
			{
				data.ByteArrays = new Dictionary<int, byte[]>();
			}
			data.ByteArrays[ZDOVars.s_itemData] = val2.GetArray();
			RemoveLegacyItemData(data, val);
		}
	}

	private static ItemData ParseLegacyItemData(ResolvedDataEntry data, byte[]? existing)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		ItemData val = new ItemData();
		if (existing != null && existing.Length != 0)
		{
			ZPackage val2 = new ZPackage(existing);
			Item val3 = (Item)val2.ReadByte();
			val = ItemData.Load(val2, val3).Item2;
		}
		if (data.Floats != null && data.Floats.TryGetValue(ZDOVars.s_durability, out var value))
		{
			val.m_durability = value;
		}
		if (data.Ints != null && data.Ints.TryGetValue(ZDOVars.s_stack, out var value2))
		{
			val.m_stack = value2;
		}
		if (data.Ints != null && data.Ints.TryGetValue(ZDOVars.s_quality, out var value3))
		{
			val.m_quality = value3;
		}
		if (data.Ints != null && data.Ints.TryGetValue(ZDOVars.s_variant, out var value4))
		{
			val.m_variant = value4;
		}
		if (data.Longs != null && data.Longs.TryGetValue(ZDOVars.s_crafterID, out var value5))
		{
			val.m_crafterID = value5;
		}
		if (data.Strings != null && data.Strings.TryGetValue(ZDOVars.s_crafterName, out string value6))
		{
			val.m_crafterName = value6;
		}
		val.m_crafterID = EnsureVisibleCrafterId(val.m_crafterID, val.m_crafterName);
		if (data.Ints != null && data.Ints.TryGetValue(ZDOVars.s_dataCount, out var value7))
		{
			for (int i = 0; i < value7; i++)
			{
				int stableHashCode = StringExtensionMethods.GetStableHashCode($"data_{i}");
				int stableHashCode2 = StringExtensionMethods.GetStableHashCode($"data__{i}");
				string value8;
				string text = ((data.Strings != null && data.Strings.TryGetValue(stableHashCode, out value8)) ? value8 : null);
				string value9;
				string text2 = ((data.Strings != null && data.Strings.TryGetValue(stableHashCode2, out value9)) ? value9 : null);
				if (!string.IsNullOrEmpty(text))
				{
					val.m_customData[text] = text2 ?? "";
				}
				data.Strings?.Remove(stableHashCode);
				data.Strings?.Remove(stableHashCode2);
			}
		}
		if (data.Ints != null && data.Ints.TryGetValue(ZDOVars.s_worldLevel, out var value10))
		{
			val.m_worldLevel = (byte)value10;
		}
		if (data.Ints != null && data.Ints.TryGetValue(ZDOVars.s_pickedUp, out var value11))
		{
			val.m_pickedUp = value11 != 0;
		}
		if (data.Ints != null && data.Ints.TryGetValue(ZDOVars.s_cheated, out var value12))
		{
			val.m_cheated = value12 != 0;
		}
		return val;
	}

	private static void RemoveLegacyItemData(ResolvedDataEntry data, ItemData itemData)
	{
		data.Floats?.Remove(ZDOVars.s_durability);
		data.Ints?.Remove(ZDOVars.s_stack);
		if (itemData.m_quality == 1)
		{
			data.Ints?.Remove(ZDOVars.s_quality);
		}
		if (itemData.m_variant == 0)
		{
			data.Ints?.Remove(ZDOVars.s_variant);
		}
		data.Longs?.Remove(ZDOVars.s_crafterID);
		data.Strings?.Remove(ZDOVars.s_crafterName);
		data.Ints?.Remove(ZDOVars.s_dataCount);
		data.Ints?.Remove(ZDOVars.s_worldLevel);
		data.Ints?.Remove(ZDOVars.s_pickedUp);
		data.Ints?.Remove(ZDOVars.s_cheated);
		Dictionary<int, float>? floats = data.Floats;
		if (floats != null && floats.Count == 0)
		{
			data.Floats = null;
		}
		Dictionary<int, int>? ints = data.Ints;
		if (ints != null && ints.Count == 0)
		{
			data.Ints = null;
		}
		Dictionary<int, long>? longs = data.Longs;
		if (longs != null && longs.Count == 0)
		{
			data.Longs = null;
		}
		Dictionary<int, string>? strings = data.Strings;
		if (strings != null && strings.Count == 0)
		{
			data.Strings = null;
		}
	}

	static ItemDataHelper()
	{
		List<int> list = new List<int>();
		list.Add(ZDOVars.s_content);
		list.Add(ZDOVars.s_item);
		list.AddRange(from i in Enumerable.Range(0, 15)
			select StringExtensionMethods.GetStableHashCode($"{i}_item"));
		HashKeys = list.ToArray();
	}
}
