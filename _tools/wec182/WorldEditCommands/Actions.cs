using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public static class Actions
{
	private static Dictionary<string, int> IdToHash = new Dictionary<string, int>();

	private static Dictionary<string, int> LowerIdToHash = new Dictionary<string, int>();

	private const string DEFAULT = "default";

	public static bool? ToggleBool(ZNetView obj, bool? value, int hash)
	{
		bool @bool = obj.GetZDO().GetBool(hash, false);
		bool flag = value ?? (!@bool);
		obj.GetZDO().Set(hash, flag);
		return flag;
	}

	public static GameObject? Refresh(ZDO zdo)
	{
		GameObject gameObject = ((Component)ZNetScene.instance.m_instances[zdo]).gameObject;
		if (!Object.op_Implicit((Object)(object)gameObject) || zdo.GetPrefab() == Hash.Player)
		{
			return gameObject;
		}
		Object.Destroy((Object)(object)gameObject);
		GameObject val = ZNetScene.instance.CreateObject(zdo);
		ZNetScene.instance.m_instances[zdo] = val.GetComponent<ZNetView>();
		return val;
	}

	public static GameObject? Refresh(ZNetView view)
	{
		return Refresh(view.GetZDO());
	}

	public static void SetFloat(ZNetView obj, float? value, int hash, bool refresh = false)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			obj.GetZDO().Set(hash, value.GetValueOrDefault(-1f));
			if (refresh)
			{
				Refresh(obj);
			}
		}
	}

	public static void SetInt(ZNetView obj, int? value, int hash, bool refresh = false)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			obj.GetZDO().Set(hash, value.GetValueOrDefault(-1), false);
			if (refresh)
			{
				Refresh(obj);
			}
		}
	}

	public static void SetLong(ZNetView obj, long? value, int hash, bool refresh = false)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			obj.GetZDO().Set(hash, value.GetValueOrDefault(-1L));
			if (refresh)
			{
				Refresh(obj);
			}
		}
	}

	public static int GetId(string id)
	{
		if (IdToHash.Count == 0)
		{
			IdToHash = ZNetScene.instance.m_namedPrefabs.ToDictionary((KeyValuePair<int, GameObject> kvp) => ((Object)kvp.Value).name, (KeyValuePair<int, GameObject> kvp) => kvp.Key);
		}
		if (LowerIdToHash.Count == 0)
		{
			LowerIdToHash = ZNetScene.instance.m_namedPrefabs.ToLookup((KeyValuePair<int, GameObject> kvp) => ((Object)kvp.Value).name.ToLower(), (KeyValuePair<int, GameObject> kvp) => kvp.Key).ToDictionary((IGrouping<string, int> kvp) => kvp.Key, (IGrouping<string, int> kvp) => kvp.FirstOrDefault());
		}
		if (IdToHash.TryGetValue(id, out var value))
		{
			return value;
		}
		if (LowerIdToHash.TryGetValue(id.ToLower(), out var value2))
		{
			return value2;
		}
		return ZDOKeys.Hash(id);
	}

	public static void SetPrefab(ZNetView obj, string? value, int hash, bool refresh = false)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			obj.GetZDO().Set(hash, value ?? "");
			if (refresh)
			{
				Refresh(obj);
			}
		}
	}

	public static void SetString(ZNetView obj, string? value, int hash, bool refresh = false)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			obj.GetZDO().Set(hash, value ?? "");
			if (refresh)
			{
				Refresh(obj);
			}
		}
	}

	public static void SetTame(GameObject obj, bool tame)
	{
		SetTame(obj.GetComponent<Character>(), tame);
	}

	public static void SetTame(Character obj, bool tame)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		if (!Object.op_Implicit((Object)(object)obj))
		{
			return;
		}
		obj.SetTamed(tame);
		BaseAI component = ((Component)obj).GetComponent<BaseAI>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return;
		}
		component.SetAlerted(false);
		if (tame)
		{
			SetHunt(component, hunt: false);
			component.SetPatrolPoint();
		}
		component.SetTargetInfo(ZDOID.None);
		MonsterAI component2 = ((Component)obj).GetComponent<MonsterAI>();
		if (Object.op_Implicit((Object)(object)component2))
		{
			component2.m_targetCreature = null;
			component2.m_targetStatic = null;
			if (tame)
			{
				component2.SetDespawnInDay(false);
				component2.SetEventCreature(false);
			}
		}
		AnimalAI component3 = ((Component)obj).GetComponent<AnimalAI>();
		if (Object.op_Implicit((Object)(object)component3))
		{
			component3.m_target = null;
		}
	}

	public static void SetHunt(GameObject obj, bool hunt)
	{
		SetHunt(obj.GetComponent<BaseAI>(), hunt);
	}

	public static bool SetPrefab(ZNetView obj, string prefab)
	{
		if (!Object.op_Implicit((Object)(object)obj))
		{
			return false;
		}
		ZDO zDO = obj.GetZDO();
		if (zDO == null || !zDO.IsValid())
		{
			return false;
		}
		int prefab2 = zDO.GetPrefab();
		zDO.SetPrefab(ZDOKeys.Hash(prefab));
		GameObject val = ZNetScene.instance.CreateObject(zDO);
		if (!Object.op_Implicit((Object)(object)val))
		{
			zDO.SetPrefab(prefab2);
			return false;
		}
		Object.Destroy((Object)(object)((Component)obj).gameObject);
		ZNetScene.instance.m_instances[zDO] = val.GetComponent<ZNetView>();
		return true;
	}

	public static void SetHunt(BaseAI obj, bool hunt)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			obj.m_huntPlayer = hunt;
			obj.m_nview.GetZDO().Set(Hash.HuntPlayer, hunt);
		}
	}

	public static void SetFuel(ZNetView view, float amount)
	{
		if (Object.op_Implicit((Object)(object)view))
		{
			view.GetZDO().Set(Hash.Fuel, amount);
		}
	}

	public static void SetSleeping(GameObject obj, bool sleep)
	{
		SetSleeping(obj.GetComponent<MonsterAI>(), sleep);
	}

	public static void SetSleeping(MonsterAI obj, bool sleep)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			obj.m_sleeping = sleep;
			((BaseAI)obj).m_nview.GetZDO().Set(Hash.Sleeping, sleep);
			((BaseAI)obj).m_animator.SetBool("sleeping", sleep);
		}
	}

	public static void SetBaby(GameObject obj)
	{
		SetBaby(obj.GetComponent<Growup>());
	}

	public static void SetBaby(Growup obj)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			ZNetView component = ((Component)obj).GetComponent<ZNetView>();
			if (component != null)
			{
				component.GetZDO().Set(Hash.SpawnTime, DateTime.MaxValue.Ticks);
			}
		}
	}

	public static void SetLevel(GameObject obj, int level)
	{
		if (level >= 1)
		{
			SetLevel(obj.GetComponent<ItemDrop>(), level);
			SetLevel(obj.GetComponent<Character>(), level);
		}
	}

	public static void SetLevel(Character obj, int level)
	{
		if (Object.op_Implicit((Object)(object)obj) && obj.GetLevel() != level)
		{
			obj.SetLevel(level);
		}
	}

	public static void SetLevel(ItemDrop obj, int level)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			obj.m_itemData.m_quality = level;
			obj.m_nview.GetZDO().Set(Hash.Quality, level, false);
			SaveItem(obj);
		}
	}

	private static void SaveItem(ItemDrop obj)
	{
		ZDO zDO = obj.m_nview.GetZDO();
		ItemDrop.SaveToZDO(obj.m_itemData, zDO, -1);
		obj.m_loadedRevision = zDO.DataRevision;
	}

	public static float GetHealth(ZNetView obj)
	{
		ZDO zDO = obj.GetZDO();
		ItemDrop component = ((Component)obj).GetComponent<ItemDrop>();
		if (Object.op_Implicit((Object)(object)component))
		{
			return component.m_itemData.m_durability;
		}
		Character component2 = ((Component)obj).GetComponent<Character>();
		if (Object.op_Implicit((Object)(object)component2))
		{
			return component2.GetHealth();
		}
		WearNTear component3 = ((Component)obj).GetComponent<WearNTear>();
		if (Object.op_Implicit((Object)(object)component3))
		{
			return zDO.GetFloat(Hash.Health, component3.m_health);
		}
		Destructible component4 = ((Component)obj).GetComponent<Destructible>();
		if (Object.op_Implicit((Object)(object)component4))
		{
			return zDO.GetFloat(Hash.Health, component4.m_health);
		}
		TreeLog component5 = ((Component)obj).GetComponent<TreeLog>();
		if (Object.op_Implicit((Object)(object)component5))
		{
			return zDO.GetFloat(Hash.Health, component5.m_health);
		}
		TreeBase component6 = ((Component)obj).GetComponent<TreeBase>();
		if (Object.op_Implicit((Object)(object)component6))
		{
			return zDO.GetFloat(Hash.Health, component6.m_health);
		}
		return -1f;
	}

	public static float SetHealth(GameObject obj, float health, bool isPercentage)
	{
		ItemDrop component = obj.GetComponent<ItemDrop>();
		if (Object.op_Implicit((Object)(object)component))
		{
			return SetHealth(component, health, isPercentage);
		}
		Character component2 = obj.GetComponent<Character>();
		if (Object.op_Implicit((Object)(object)component2))
		{
			return SetHealth(component2, health, isPercentage);
		}
		WearNTear component3 = obj.GetComponent<WearNTear>();
		if (Object.op_Implicit((Object)(object)component3))
		{
			return SetHealth(component3, health, isPercentage);
		}
		TreeLog component4 = obj.GetComponent<TreeLog>();
		if (Object.op_Implicit((Object)(object)component4))
		{
			return SetHealth(component4, health, isPercentage);
		}
		TreeBase component5 = obj.GetComponent<TreeBase>();
		if (Object.op_Implicit((Object)(object)component5))
		{
			return SetHealth(component5, health, isPercentage);
		}
		Destructible component6 = obj.GetComponent<Destructible>();
		if (Object.op_Implicit((Object)(object)component6))
		{
			return SetHealth(component6, health, isPercentage);
		}
		return 0f;
	}

	public static float SetHealth(Character obj, float health, bool isPercentage)
	{
		if (!Object.op_Implicit((Object)(object)obj))
		{
			return 0f;
		}
		if (isPercentage)
		{
			health *= obj.GetMaxHealth();
		}
		float maxHealth = obj.GetMaxHealth();
		if (health == 0f)
		{
			obj.SetupMaxHealth();
			return maxHealth;
		}
		obj.SetMaxHealth(health);
		obj.SetHealth(health * 1.000001f);
		return maxHealth;
	}

	public static float SetHealth(WearNTear obj, float health, bool isPercentage)
	{
		if (!Object.op_Implicit((Object)(object)obj))
		{
			return 0f;
		}
		if (isPercentage)
		{
			health *= obj.m_health;
		}
		if (health == 0f)
		{
			health = obj.m_health;
		}
		float @float = obj.m_nview.GetZDO().GetFloat(Hash.Health, obj.m_health);
		obj.m_nview.GetZDO().Set(Hash.Health, health);
		return @float;
	}

	public static float SetHealth(TreeLog obj, float health, bool isPercentage)
	{
		if (!Object.op_Implicit((Object)(object)obj))
		{
			return 0f;
		}
		if (isPercentage)
		{
			health *= obj.m_health;
		}
		if (health == 0f)
		{
			health = obj.m_health;
		}
		float @float = obj.m_nview.GetZDO().GetFloat(Hash.Health, obj.m_health);
		obj.m_nview.GetZDO().Set(Hash.Health, health);
		return @float;
	}

	public static float SetHealth(Destructible obj, float health, bool isPercentage)
	{
		if (!Object.op_Implicit((Object)(object)obj))
		{
			return 0f;
		}
		if (isPercentage)
		{
			health *= obj.m_health;
		}
		if (health == 0f)
		{
			health = obj.m_health;
		}
		float @float = obj.m_nview.GetZDO().GetFloat(Hash.Health, obj.m_health);
		obj.m_nview.GetZDO().Set(Hash.Health, health);
		return @float;
	}

	public static float SetHealth(TreeBase obj, float health, bool isPercentage)
	{
		if (!Object.op_Implicit((Object)(object)obj))
		{
			return 0f;
		}
		if (isPercentage)
		{
			health *= obj.m_health;
		}
		if (health == 0f)
		{
			health = obj.m_health;
		}
		float @float = obj.m_nview.GetZDO().GetFloat(Hash.Health, obj.m_health);
		obj.m_nview.GetZDO().Set(Hash.Health, health);
		return @float;
	}

	public static float SetHealth(ItemDrop obj, float health, bool isPercentage)
	{
		if (!Object.op_Implicit((Object)(object)obj))
		{
			return 0f;
		}
		float durability = obj.m_itemData.m_durability;
		if (isPercentage)
		{
			health *= obj.m_itemData.GetMaxDurability();
		}
		if (health == 0f)
		{
			health = obj.m_itemData.GetMaxDurability();
		}
		obj.m_itemData.m_durability = health;
		obj.m_nview.GetZDO().Set(Hash.Durability, obj.m_itemData.m_durability);
		SaveItem(obj);
		return durability;
	}

	public static float SetCreator(ZNetView obj, long creator)
	{
		Piece obj2 = default(Piece);
		if (((Component)obj).TryGetComponent<Piece>(ref obj2))
		{
			return SetCreator(obj2, creator);
		}
		return 0f;
	}

	public static long SetCreator(Piece obj, long creator)
	{
		if (!Object.op_Implicit((Object)(object)obj))
		{
			return 0L;
		}
		long creator2 = obj.GetCreator();
		obj.m_creator = creator;
		obj.m_nview.GetZDO().Set(Hash.Creator, creator);
		return creator2;
	}

	public static void SetVariant(GameObject obj, int variant)
	{
		SetVariant(obj.GetComponent<ItemDrop>(), variant);
	}

	public static void SetVariant(ItemDrop obj, int variant)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			obj.m_itemData.m_variant = variant;
			obj.m_nview.GetZDO().Set(Hash.Variant, variant, false);
			SaveItem(obj);
		}
	}

	public static void SetName(GameObject obj, string name)
	{
		SetName(obj.GetComponent<Tameable>(), name);
		SetName(obj.GetComponent<ItemDrop>(), name);
	}

	public static void SetName(Tameable obj, string name)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			obj.m_nview.GetZDO().Set(Hash.TamedName, name);
		}
	}

	public static void SetCrafterId(ItemDrop obj, long id)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			obj.m_itemData.m_crafterID = id;
			obj.m_nview.GetZDO().Set(Hash.CrafterID, obj.m_itemData.m_crafterID);
			SaveItem(obj);
		}
	}

	public static void SetName(ItemDrop obj, string name)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			obj.m_itemData.m_crafterID = ((!(name == "")) ? (-1) : 0);
			obj.m_nview.GetZDO().Set(Hash.CrafterID, obj.m_itemData.m_crafterID);
			obj.m_itemData.m_crafterName = name;
			obj.m_nview.GetZDO().Set(Hash.CrafterName, name);
			SaveItem(obj);
		}
	}

	public static int SetStack(GameObject obj, int remaining)
	{
		if (remaining <= 0)
		{
			return 0;
		}
		ItemDrop component = obj.GetComponent<ItemDrop>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return 0;
		}
		int num = Math.Min(remaining, component.m_itemData.m_shared.m_maxStackSize);
		component.m_itemData.m_stack = num;
		component.m_nview.GetZDO().Set(Hash.Stack, num, false);
		SaveItem(component);
		return num;
	}

	public static void SetRotation(GameObject obj, Quaternion rotation)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		ZNetView component = obj.GetComponent<ZNetView>();
		if (!((Object)(object)component == (Object)null) && rotation != Quaternion.identity)
		{
			component.GetZDO().SetRotation(rotation);
			obj.transform.rotation = rotation;
		}
	}

	public static void Respawn(GameObject obj)
	{
		Respawn(obj.GetComponent<Container>());
		Respawn(obj.GetComponent<Pickable>());
		Respawn(obj.GetComponent<CreatureSpawner>());
	}

	public static void Respawn(Container obj)
	{
		if (Object.op_Implicit((Object)(object)obj) && obj.m_defaultItems.m_drops.Count != 0)
		{
			Inventory inventory = obj.m_inventory;
			if (inventory != null)
			{
				inventory.RemoveAll();
			}
			obj.AddDefaultItems();
		}
	}

	public static bool CanRespawn(GameObject obj)
	{
		if (!Object.op_Implicit((Object)(object)obj.GetComponent<Pickable>()) && !Object.op_Implicit((Object)(object)obj.GetComponent<CreatureSpawner>()))
		{
			Container component = obj.GetComponent<Container>();
			if (component == null)
			{
				return false;
			}
			return component.m_defaultItems.m_drops.Count > 0;
		}
		return true;
	}

	public static void Respawn(Pickable obj)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			obj.SetPicked(false);
		}
	}

	public static void Respawn(CreatureSpawner obj)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		if (!Object.op_Implicit((Object)(object)obj))
		{
			return;
		}
		ZNetView nview = obj.m_nview;
		ZDO val = ((nview != null) ? nview.GetZDO() : null);
		if (val != null)
		{
			FakeZDO fakeZDO = new FakeZDO(val);
			ZDOID connectionZDOID = val.GetConnectionZDOID((ConnectionType)3);
			if (ZDOMan.instance.m_objectsByID.TryGetValue(connectionZDOID, out var value))
			{
				UndoHelper.AddRemoveAction(value);
				RemoveZDO(value);
			}
			UndoHelper.AddRemoveAction(val);
			RemoveZDO(val);
			fakeZDO.Data.ConnectionType = (ConnectionType)0;
			fakeZDO.Data.ConnectionHash = 0;
			fakeZDO.Data.TargetConnectionId = ZDOID.None;
			fakeZDO.Data.Longs?.Remove(ZDOVars.s_aliveTime);
			UndoHelper.AddSpawnAction(fakeZDO.Create());
		}
	}

	public static void SetModel(GameObject obj, int index)
	{
		SetModel(obj.GetComponent<Character>(), index);
	}

	public static void SetModel(Character obj, int index)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			VisEquipment component = ((Component)obj).GetComponent<VisEquipment>();
			if (Object.op_Implicit((Object)(object)component))
			{
				component.SetModel(index);
			}
		}
	}

	public static void SetVisual(GameObject obj, Item item)
	{
		SetVisual(obj.GetComponent<ItemStand>(), item);
	}

	public static void SetVisual(GameObject obj, VisSlot slot, Item? item)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		SetVisual(obj.GetComponent<Character>(), slot, item);
	}

	public static void SetDistant(GameObject obj, bool distant)
	{
		obj.GetComponent<ZNetView>().GetZDO().Distant = distant;
	}

	public static void SetPersistent(GameObject obj, bool persistent)
	{
		obj.GetComponent<ZNetView>().GetZDO().Persistent = persistent;
	}

	public static int SetFields(GameObject obj, Dictionary<string, object> fields)
	{
		return SetFields(obj.GetComponent<ZNetView>(), fields);
	}

	public static int SetFields(ZNetView view, Dictionary<string, object> fields)
	{
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		ZDO val = ((view != null) ? view.GetZDO() : null);
		if ((Object)(object)view == (Object)null || val == null)
		{
			return 0;
		}
		HashSet<string> hashSet = ComponentInfo.Get(view).ToHashSet();
		int num = 0;
		bool flag = false;
		foreach (KeyValuePair<string, object> field in fields)
		{
			if (field.Key.Split('.').Length > 1)
			{
				string text = field.Key.Split('.')[0];
				if (!hashSet.Contains(text))
				{
					continue;
				}
				val.Set(Hash.HasFields, true);
				val.Set("HasFields" + text, true);
			}
			else
			{
				flag = true;
			}
			int num2 = ZDOKeys.Hash(field.Key);
			if (field.Value is int num3)
			{
				val.Set(num2, num3, false);
			}
			else if (field.Value is float num4)
			{
				val.Set(num2, num4);
			}
			else if (field.Value is string text2)
			{
				val.Set(num2, text2);
			}
			else if (field.Value is bool flag2)
			{
				val.Set(num2, flag2);
			}
			else if (field.Value is long num5)
			{
				val.Set(num2, num5);
			}
			else if (field.Value is Vector3 pos)
			{
				val.Set(num2, TransformPos(val, field.Key, pos));
			}
			else if (field.Value is Quaternion rot)
			{
				val.Set(num2, TransformRot(val, field.Key, rot));
			}
			num++;
		}
		if (flag)
		{
			Refresh(view);
		}
		else
		{
			view.LoadFields();
			WearNTear val2 = default(WearNTear);
			if (((Component)view).TryGetComponent<WearNTear>(ref val2))
			{
				val2.m_healthPercentage = val.GetFloat(ZDOVars.s_health, val2.m_health) / val2.m_health;
				val2.UpdateVisual(false);
			}
		}
		return num;
	}

	private static Vector3 TransformPos(ZDO zdo, string key, Vector3 pos)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (ZDOKeys.IsRoomPos(key))
		{
			return zdo.GetPosition() + zdo.GetRotation() * pos;
		}
		return pos;
	}

	private static Quaternion TransformRot(ZDO zdo, string key, Quaternion rot)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (ZDOKeys.IsRoomRot(key))
		{
			return zdo.GetRotation() * rot;
		}
		return rot;
	}

	public static void SetVisual(ItemStand obj, Item? item)
	{
		if (Object.op_Implicit((Object)(object)obj) && item != null)
		{
			obj.m_nview.GetZDO().Set(Hash.Item, item.Name);
			obj.m_nview.GetZDO().Set(Hash.Variant, item.Variant, false);
			obj.UpdateVisual();
		}
	}

	public static void SetVisual(Character obj, VisSlot slot, Item? item)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)obj) && item != null)
		{
			VisEquipment component = ((Component)obj).GetComponent<VisEquipment>();
			if (Object.op_Implicit((Object)(object)component))
			{
				component.SetItem(slot, ZDOKeys.Hash(item.Name), item.Variant, 0);
			}
		}
	}

	public static void Move(ZNetView obj, Vector3 offset, string origin)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		ZDO zDO = obj.GetZDO();
		Vector3 position = ((Component)obj).transform.position;
		Quaternion val = ((Component)Player.m_localPlayer).transform.rotation;
		if (origin == "world")
		{
			val = Quaternion.identity;
		}
		if (origin == "object")
		{
			val = ((Component)obj).transform.rotation;
		}
		position += val * Vector3.right * offset.x;
		position += val * Vector3.up * offset.y;
		position += val * Vector3.forward * offset.z;
		zDO.SetPosition(position);
		((Component)obj).transform.position = position;
	}

	public static void Rotate(ZNetView obj, Vector3 relative, string origin, Vector3? center = null)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		ZDO zDO = obj.GetZDO();
		Quaternion val = ((Component)Player.m_localPlayer).transform.rotation;
		if (origin == "world")
		{
			val = Quaternion.identity;
		}
		if (origin == "object")
		{
			val = ((Component)obj).transform.rotation;
		}
		Transform transform = ((Component)obj).transform;
		Vector3 val2 = (Vector3)(((_003F?)center) ?? transform.position);
		transform.RotateAround(val2, val * Vector3.up, relative.y);
		transform.RotateAround(val2, val * Vector3.forward, relative.x);
		transform.RotateAround(val2, val * Vector3.right, relative.z);
		zDO.SetRotation(((Component)obj).transform.rotation);
	}

	public static void Mirror(ZNetView obj, Vector3 center)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		Transform transform = ((Component)obj).transform;
		transform.position = new Vector3(2f * center.x - transform.position.x, transform.position.y, transform.position.z);
		Vector3 eulerAngles = transform.eulerAngles;
		transform.rotation = Quaternion.Euler(eulerAngles.x, 0f - eulerAngles.y, eulerAngles.z);
		ZDO zDO = obj.GetZDO();
		zDO.SetPosition(transform.position);
		zDO.SetRotation(transform.rotation);
	}

	public static void ResetRotation(ZNetView obj)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		obj.GetZDO().SetRotation(Quaternion.identity);
		((Component)obj).transform.rotation = Quaternion.identity;
	}

	public static bool Scale(ZNetView obj, Vector3 scale)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		bool result = false;
		if (!obj.m_syncInitialScale && WorldEditCommands.IsTweaks)
		{
			SetFields(obj, new Dictionary<string, object> { { "ZNetView.m_syncInitialScale", true } });
			result = true;
		}
		if (obj.m_syncInitialScale)
		{
			obj.SetLocalScale(scale);
		}
		return result;
	}

	public static void RemoveZDO(ZDO zdo)
	{
		if (zdo != null && zdo.IsValid())
		{
			zdo.SetOwnerInternal(ZDOMan.GetSessionID());
			if (ZNetScene.instance.m_instances.TryGetValue(zdo, out var value))
			{
				ZNetScene.instance.Destroy(((Component)value).gameObject);
				Object.DestroyImmediate((Object)(object)value);
			}
			else
			{
				ZDOMan.instance.DestroyZDO(zdo);
			}
		}
	}

	private static string Print<T>(T? value)
	{
		if (value != null)
		{
			return value.ToString();
		}
		return "default";
	}

	public static string Damage(ZNetView view, float? value)
	{
		if (!Object.op_Implicit((Object)(object)((Component)view).GetComponent<Character>()))
		{
			return "Skipped: ¤ is not a creature.";
		}
		SetFloat(view, value.GetValueOrDefault(1f), Hash.Damage);
		return "¤ damage set to " + Print(value) + ".";
	}

	public static string Ammo(ZNetView view, int? value)
	{
		if (!Object.op_Implicit((Object)(object)((Component)view).GetComponent<Turret>()))
		{
			return "Skipped: ¤ is not a turret.";
		}
		SetInt(view, value.GetValueOrDefault(), Hash.Ammo);
		return "¤ ammo set to " + Print(value) + ".";
	}

	public static string AmmoType(ZNetView view, string? value)
	{
		if (!Object.op_Implicit((Object)(object)((Component)view).GetComponent<Turret>()))
		{
			return "Skipped: ¤ is not a turret.";
		}
		SetString(view, value ?? "", Hash.AmmoType);
		return "¤ ammo type set to " + Print(value) + ".";
	}
}
