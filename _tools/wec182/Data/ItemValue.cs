using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEngine;

namespace Data;

public class ItemValue
{
	public IPrefabValue Prefab = DataValue.Prefab(data.prefab);

	public float Chance = data.chance;

	public IIntValue? Stack = ((data.stack == null) ? null : DataValue.Int(data.stack));

	public IFloatValue? Durability = ((data.durability == null) ? null : DataValue.Float(data.durability));

	public string Position = data.pos;

	private Vector2i RolledPosition = Parse.Vector2Int(data.pos);

	public IBoolValue? Equipped = ((data.equipped == null) ? null : DataValue.Bool(data.equipped));

	public IIntValue? Quality = ((data.quality == null) ? null : DataValue.Int(data.quality));

	public IIntValue? Variant = ((data.variant == null) ? null : DataValue.Int(data.variant));

	public ILongValue? CrafterID = ((data.crafterID == null) ? null : DataValue.Long(data.crafterID));

	public IStringValue? CrafterName = ((data.crafterName == null) ? null : DataValue.String(data.crafterName));

	public Dictionary<string, IStringValue>? CustomData = data.customData?.ToDictionary((KeyValuePair<string, string> kvp) => kvp.Key, (KeyValuePair<string, string> kvp) => DataValue.String(kvp.Value));

	public IIntValue? WorldLevel = ((data.worldLevel == null) ? null : DataValue.Int(data.worldLevel));

	public IBoolValue? PickedUp = ((data.pickedUp == null) ? null : DataValue.Bool(data.pickedUp));

	public IBoolValue? Cheated = ((data.cheated == null) ? null : DataValue.Bool(data.cheated));

	private int RolledPrefab;

	private int RolledStack;

	public ItemValue(ItemYaml data)
	{
	}//IL_0068: Unknown result type (might be due to invalid IL or missing references)
	//IL_006d: Unknown result type (might be due to invalid IL or missing references)


	public static bool Match(Functions f, List<ItemValue> data, ZDO zdo, IIntValue? amount)
	{
		Functions f2 = f;
		List<ItemRecord> records = ItemDataHelper.Load(zdo);
		int num = data.Count((ItemValue item) => item.Match(f2, records));
		if (amount == null)
		{
			if (num == data.Count)
			{
				return records.Count == 0;
			}
			return false;
		}
		return amount.Match(f2, num).GetValueOrDefault();
	}

	public static bool Match(Functions f, ZDO zdo, IIntValue amount)
	{
		List<ItemRecord> list = ItemDataHelper.Load(zdo);
		return amount.Match(f, list.Count).GetValueOrDefault();
	}

	public bool MatchSingle(Functions f, ZDO zdo)
	{
		ItemRecord itemRecord = ItemDataHelper.LoadSingle(zdo);
		if (itemRecord == null)
		{
			return false;
		}
		IIntValue? stack = Stack;
		if (stack != null && stack.Match(f, itemRecord.Stack) == false)
		{
			return false;
		}
		return MatchProperties(f, itemRecord);
	}

	public byte[]? Create(Functions f, ZDO zdo)
	{
		GameObject prefab = ZNetScene.instance.GetPrefab(zdo.m_prefab);
		ItemDrop val = default(ItemDrop);
		if ((Object)(object)prefab == (Object)null || !prefab.TryGetComponent<ItemDrop>(ref val))
		{
			return null;
		}
		RolledStack = (Stack?.Get(f)).GetValueOrDefault(1);
		return ItemDataHelper.Serialize(CreateItemData(f, prefab));
	}

	public static string LoadItems(Functions f, List<ItemValue> items, Vector2i size, int amount)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return Convert.ToBase64String(LoadItemBytes(f, items, size, amount));
	}

	internal static byte[] LoadItemBytes(Functions f, List<ItemValue> items, Vector2i size, int amount)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		ZPackage val = new ZPackage();
		val.Write(109);
		items = Generate(f, items, size, amount);
		List<ItemValue> list = new List<ItemValue>();
		list.AddRange(items.Where((ItemValue item) => item.CanWrite()));
		items = list;
		val.Write((ushort)items.Count);
		foreach (ItemValue item in items)
		{
			item.Write(f, val);
		}
		return val.GetArray();
	}

	public static List<ItemValue> Generate(Functions f, List<ItemValue> data, Vector2i size, int amount)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		List<ItemValue> list = data.Where((ItemValue item) => item.Position != "").ToList();
		List<ItemValue> items = data.Where((ItemValue item) => item.Position == "").ToList();
		Dictionary<Vector2i, ItemValue> dictionary = new Dictionary<Vector2i, ItemValue>();
		foreach (ItemValue item in list)
		{
			if (item.Roll(f))
			{
				dictionary[item.RolledPosition] = item;
			}
		}
		if (amount == 0)
		{
			GenerateEach(f, dictionary, size, items);
		}
		else
		{
			GenerateAmount(f, dictionary, size, items, amount);
		}
		Dictionary<Vector2i, ItemValue>.ValueCollection values = dictionary.Values;
		List<ItemValue> list2 = new List<ItemValue>(values.Count);
		list2.AddRange(values);
		return list2;
	}

	private static void GenerateEach(Functions f, Dictionary<Vector2i, ItemValue> inventory, Vector2i size, List<ItemValue> items)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		foreach (ItemValue item in items)
		{
			if (item.Roll(f))
			{
				Vector2i? val = FindNextFreeSlot(inventory, size);
				if (!val.HasValue)
				{
					break;
				}
				item.RolledPosition = val.Value;
				inventory[val.Value] = item;
			}
		}
	}

	private static void GenerateAmount(Functions f, Dictionary<Vector2i, ItemValue> inventory, Vector2i size, List<ItemValue> items, int amount)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		float num = items.Sum((ItemValue item) => item.Chance);
		for (int i = 0; i < amount; i++)
		{
			if (items.Count <= 0)
			{
				break;
			}
			Vector2i? val = FindNextFreeSlot(inventory, size);
			if (val.HasValue)
			{
				ItemValue itemValue = RollItem(items, num);
				itemValue.RolledPosition = val.Value;
				if (itemValue.RollPrefab(f))
				{
					inventory[val.Value] = itemValue;
				}
				num -= itemValue.Chance;
				items.Remove(itemValue);
				continue;
			}
			break;
		}
	}

	private static ItemValue RollItem(List<ItemValue> items, float maxWeight)
	{
		float num = Random.Range(0f, maxWeight);
		foreach (ItemValue item in items)
		{
			if (num < item.Chance)
			{
				return item;
			}
			num -= item.Chance;
		}
		return items.Last();
	}

	private static Vector2i? FindNextFreeSlot(Dictionary<Vector2i, ItemValue> inventory, Vector2i size)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		int x = size.x;
		int y = size.y;
		Vector2i val = default(Vector2i);
		for (int i = 0; i < y; i++)
		{
			for (int j = 0; j < x; j++)
			{
				((Vector2i)(ref val))._002Ector(j, i);
				if (!inventory.ContainsKey(val))
				{
					return val;
				}
			}
		}
		return null;
	}

	public bool RollPrefab(Functions f)
	{
		RolledPrefab = Prefab.Get(f).GetValueOrDefault();
		RolledStack = (Stack?.Get(f)).GetValueOrDefault(1);
		if (RolledPrefab != 0)
		{
			return RolledStack != 0;
		}
		return false;
	}

	public bool RollChance()
	{
		if (!(Chance >= 1f))
		{
			return Random.value <= Chance;
		}
		return true;
	}

	public bool Roll(Functions f)
	{
		if (RollChance())
		{
			return RollPrefab(f);
		}
		return false;
	}

	private bool CanWrite()
	{
		GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(RolledPrefab);
		ItemDrop val = default(ItemDrop);
		if ((Object)(object)itemPrefab != (Object)null)
		{
			return itemPrefab.TryGetComponent<ItemDrop>(ref val);
		}
		return false;
	}

	public void Write(Functions f, ZPackage pkg)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(RolledPrefab);
		ItemDrop val = default(ItemDrop);
		if (!((Object)(object)itemPrefab == (Object)null) && itemPrefab.TryGetComponent<ItemDrop>(ref val))
		{
			ItemData obj = CreateItemData(f, itemPrefab);
			obj.m_gridPos = RolledPosition;
			obj.Save(pkg);
		}
	}

	private ItemData CreateItemData(Functions f, GameObject prefab)
	{
		Functions f2 = f;
		Dictionary<string, string> customData = CustomData?.ToDictionary((KeyValuePair<string, IStringValue> x) => x.Key, (KeyValuePair<string, IStringValue> x) => x.Value.Get(f2) ?? "");
		return ItemDataHelper.Create(prefab, RolledStack, Durability?.Get(f2), (Quality?.Get(f2)).GetValueOrDefault(1), (Variant?.Get(f2)).GetValueOrDefault(), (CrafterID?.Get(f2)).GetValueOrDefault(), CrafterName?.Get(f2) ?? "", (WorldLevel?.Get(f2)).GetValueOrDefault(), (PickedUp?.GetBool(f2)).GetValueOrDefault(), (Cheated?.GetBool(f2)).GetValueOrDefault(), (Equipped?.GetBool(f2)).GetValueOrDefault(), customData);
	}

	public bool Match(Functions f, List<ItemRecord> records)
	{
		ItemRecord itemRecord = FindMatch(f, records);
		if (itemRecord == null)
		{
			return false;
		}
		records.Remove(itemRecord);
		return true;
	}

	private ItemRecord? FindMatch(Functions f, List<ItemRecord> records)
	{
		if (Position != "")
		{
			ItemRecord itemRecord = records.FirstOrDefault((ItemRecord r) => r.GridPos == RolledPosition);
			if (itemRecord == null)
			{
				return null;
			}
			IIntValue? stack = Stack;
			if (stack != null && stack.Match(f, itemRecord.Stack) == false)
			{
				return null;
			}
			if (MatchItem(f, itemRecord))
			{
				return itemRecord;
			}
			return null;
		}
		foreach (ItemRecord record in records)
		{
			IIntValue? stack2 = Stack;
			if ((stack2 == null || stack2.Match(f, record.Stack) != false) && MatchItem(f, record))
			{
				return record;
			}
		}
		return null;
	}

	private bool MatchItem(Functions f, ItemRecord item)
	{
		if (Prefab.Match(f, item.PrefabHash) == false)
		{
			return false;
		}
		return MatchProperties(f, item);
	}

	private bool MatchProperties(Functions f, ItemRecord item)
	{
		IFloatValue? durability = Durability;
		bool flag2;
		if (durability != null)
		{
			bool? flag = durability.Match(f, item.Durability);
			flag2 = false;
			if (flag == flag2)
			{
				return false;
			}
		}
		IBoolValue? equipped = Equipped;
		if (equipped != null)
		{
			bool? flag = equipped.Match(f, item.Equipped);
			flag2 = false;
			if (flag == flag2)
			{
				return false;
			}
		}
		IIntValue? quality = Quality;
		if (quality != null)
		{
			bool? flag = quality.Match(f, item.Quality);
			flag2 = false;
			if (flag == flag2)
			{
				return false;
			}
		}
		IIntValue? variant = Variant;
		if (variant != null)
		{
			bool? flag = variant.Match(f, item.Variant);
			flag2 = false;
			if (flag == flag2)
			{
				return false;
			}
		}
		ILongValue? crafterID = CrafterID;
		if (crafterID != null)
		{
			bool? flag = crafterID.Match(f, item.CrafterID);
			flag2 = false;
			if (flag == flag2)
			{
				return false;
			}
		}
		IStringValue? crafterName = CrafterName;
		if (crafterName != null)
		{
			bool? flag = crafterName.Match(f, item.CrafterName);
			flag2 = false;
			if (flag == flag2)
			{
				return false;
			}
		}
		IIntValue? worldLevel = WorldLevel;
		if (worldLevel != null)
		{
			bool? flag = worldLevel.Match(f, item.WorldLevel);
			flag2 = false;
			if (flag == flag2)
			{
				return false;
			}
		}
		IBoolValue? pickedUp = PickedUp;
		if (pickedUp != null)
		{
			bool? flag = pickedUp.Match(f, item.PickedUp);
			flag2 = false;
			if (flag == flag2)
			{
				return false;
			}
		}
		IBoolValue? cheated = Cheated;
		if (cheated != null)
		{
			bool? flag = cheated.Match(f, item.Cheated);
			flag2 = false;
			if (flag == flag2)
			{
				return false;
			}
		}
		if (CustomData == null)
		{
			return true;
		}
		foreach (KeyValuePair<string, IStringValue> customDatum in CustomData)
		{
			if (!item.CustomData.TryGetValue(customDatum.Key, out string value))
			{
				flag2 = false;
			}
			else
			{
				if (customDatum.Value.Match(f, value) != false)
				{
					continue;
				}
				flag2 = false;
			}
			goto IL_0243;
		}
		return true;
		IL_0243:
		return flag2;
	}
}
