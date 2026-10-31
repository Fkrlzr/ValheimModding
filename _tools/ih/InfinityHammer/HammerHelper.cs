using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Data;
using ServerDevcommands;
using Service;
using UnityEngine;

namespace InfinityHammer;

public static class HammerHelper
{
	public static string GetArgs(string cmd, ConsoleEventArgs args)
	{
		return args.FullLine.Substring(cmd.Length + 1);
	}

	public static string Format(float value)
	{
		return value.ToString("0.###", CultureInfo.InvariantCulture);
	}

	public static string PrintXZY(Vector3 vec)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		return Format(vec.x) + "," + Format(vec.z) + "," + Format(vec.y);
	}

	public static string PrintYXZ(Vector3 vec)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		return Format(vec.y) + "," + Format(vec.x) + "," + Format(vec.z);
	}

	public static GameObject GetPlacementGhost()
	{
		Player player = Helper.GetPlayer();
		if (!Object.op_Implicit((Object)(object)player.m_placementGhost))
		{
			throw new InvalidOperationException("Not currently placing anything.");
		}
		return player.m_placementGhost;
	}

	public static string GetTool()
	{
		ItemData rightItem = ((Humanoid)Helper.GetPlayer()).GetRightItem();
		if (Hammer.IsInfinityHammer(rightItem))
		{
			return "infinity_hammer";
		}
		object obj;
		if (rightItem == null)
		{
			obj = null;
		}
		else
		{
			GameObject dropPrefab = rightItem.m_dropPrefab;
			obj = ((dropPrefab != null) ? ((Object)dropPrefab).name : null);
		}
		if (obj == null)
		{
			obj = "";
		}
		return (string)obj;
	}

	public static void RemoveZDO(ZDO zdo)
	{
		if (Selector.IsValid(zdo))
		{
			if (!zdo.IsOwner())
			{
				zdo.SetOwner(ZDOMan.instance.m_sessionID);
			}
			if (ZNetScene.instance.m_instances.TryGetValue(zdo, out var value))
			{
				ZNetScene.instance.Destroy(((Component)value).gameObject);
			}
			else
			{
				ZDOMan.instance.DestroyZDO(zdo);
			}
		}
	}

	public static Vector3 ParseSize(GameObject ghost, string value)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		float num = Parse.Multiplier(value);
		Vector3 value2 = Vector3.one;
		if (value.Contains("auto"))
		{
			if (!Object.op_Implicit((Object)(object)ghost))
			{
				throw new InvalidOperationException("No placement ghost.");
			}
			if (!Configuration.Dimensions.TryGetValue(Utils.GetPrefabName(ghost).ToLower(), out value2))
			{
				throw new InvalidOperationException("Missing object dimensions. Try placing the object to fix the issue.");
			}
		}
		return num * value2;
	}

	public static Vector3 TryParseSize(GameObject ghost, string[] args, int index, string defaltValue = "auto")
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		string value = defaltValue;
		if (args.Length > index)
		{
			value = args[index];
		}
		return ParseSize(ghost, value);
	}

	public static Vector3 TryParseSizesZYX(GameObject ghost, string[] args, int index, string defaltValue = "auto")
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		string text = defaltValue;
		if (args.Length > index)
		{
			text = args[index];
		}
		string[] args2 = text.Split(',');
		return new Vector3(TryParseSize(ghost, args2, 2, defaltValue).x, TryParseSize(ghost, args2, 1, defaltValue).y, TryParseSize(ghost, args2, 0, defaltValue).z);
	}

	public static bool IsSquareX(GameObject ghost)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (!Object.op_Implicit((Object)(object)ghost))
		{
			return false;
		}
		Vector3 val = Configuration.Dimensions[Utils.GetPrefabName(ghost).ToLower()];
		return val.y - val.z < 0.01f;
	}

	public static bool IsSquareY(GameObject ghost)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (!Object.op_Implicit((Object)(object)ghost))
		{
			return false;
		}
		Vector3 val = Configuration.Dimensions[Utils.GetPrefabName(ghost).ToLower()];
		return val.x - val.z < 0.01f;
	}

	public static bool IsSquareZ(GameObject ghost)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (!Object.op_Implicit((Object)(object)ghost))
		{
			return false;
		}
		Vector3 val = Configuration.Dimensions[Utils.GetPrefabName(ghost).ToLower()];
		return val.x - val.y < 0.01f;
	}

	public static void Message(Terminal instance, string message)
	{
		if (!Configuration.DisableMessages)
		{
			Helper.AddMessage(instance, message, true);
		}
	}

	public static GameObject SafeInstantiate(ZNetView view, GameObject parent)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		GameObject obj = ((Component)view).gameObject;
		if ((from collider in ((Component)view).GetComponentsInChildren<Collider>()
			where collider.enabled && !collider.isTrigger
			select collider).ToArray().Length == 0)
		{
			int num = ((view.GetZDO() == null) ? StringExtensionMethods.GetStableHashCode(view.GetPrefabName()) : view.GetZDO().GetPrefab());
			obj = ZNetScene.instance.GetPrefab(num);
		}
		return SafeInstantiate(obj, ((Component)view).transform.rotation, parent.transform);
	}

	public static GameObject ChildInstantiate(ZNetView view, GameObject parent)
	{
		GameObject gameObject = ((Component)view).gameObject;
		ResetHighlight(gameObject);
		GameObject obj = Object.Instantiate<GameObject>(gameObject, parent.transform);
		CleanObject(obj);
		return obj;
	}

	public static GameObject SafeInstantiateRoom(RoomData room, bool emptyRoom, GameObject parent)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		room.m_prefab.Load();
		ZNetView[] enabledComponentsInChildren = Utils.GetEnabledComponentsInChildren<ZNetView>(room.m_prefab.Asset);
		if (emptyRoom)
		{
			ZNetView[] array = enabledComponentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				((Component)array[i]).gameObject.SetActive(false);
			}
		}
		GameObject result = SafeInstantiate(room.m_prefab.Asset, room.m_prefab.Asset.transform.rotation, parent.transform);
		if (emptyRoom)
		{
			ZNetView[] array = enabledComponentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				((Component)array[i]).gameObject.SetActive(true);
			}
		}
		room.m_prefab.Release();
		return result;
	}

	public static GameObject SafeInstantiateLocation(ZoneLocation location, int? seed, GameObject parent)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		if (!location.m_prefab.IsValid)
		{
			GameObject val = new GameObject();
			EnsurePiece(val);
			((Object)val).name = location.m_prefab.Name;
			return val;
		}
		location.m_prefab.Load();
		ZNetView[] enabledComponentsInChildren = Utils.GetEnabledComponentsInChildren<ZNetView>(location.m_prefab.Asset);
		RandomSpawn[] enabledComponentsInChildren2 = Utils.GetEnabledComponentsInChildren<RandomSpawn>(location.m_prefab.Asset);
		ZNetView[] array = enabledComponentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			((Component)array[i]).gameObject.SetActive(true);
		}
		if (seed.HasValue)
		{
			State state = Random.state;
			Random.InitState(seed.Value);
			RandomSpawn[] array2 = enabledComponentsInChildren2;
			foreach (RandomSpawn obj in array2)
			{
				obj.Prepare();
				obj.Randomize(Vector3.zero, (Location)null, (DungeonGenerator)null);
			}
			Random.state = state;
		}
		GameObject result = SafeInstantiate(location.m_prefab.Asset, location.m_prefab.Asset.transform.rotation, parent.transform);
		location.m_prefab.Release();
		return result;
	}

	private static GameObject SafeInstantiate(GameObject obj, Quaternion rot, Transform parent)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		ResetHighlight(obj);
		GameObject obj2 = Object.Instantiate<GameObject>(obj, Vector3.zero, rot, parent);
		CleanObject(obj2);
		EnsurePiece(obj2);
		((Object)obj2).name = ((Object)obj).name;
		return obj2;
	}

	private static void ResetHighlight(GameObject obj)
	{
		WearNTear val = default(WearNTear);
		if (obj.TryGetComponent<WearNTear>(ref val))
		{
			val.ResetHighlight();
		}
	}

	public static void DisableRandomMaterialValues(GameObject obj)
	{
		RandomMaterialValues[] componentsInChildren = obj.GetComponentsInChildren<RandomMaterialValues>(true);
		foreach (RandomMaterialValues obj2 in componentsInChildren)
		{
			((MonoBehaviour)obj2).CancelInvoke();
			((Behaviour)obj2).enabled = false;
		}
	}

	private static void CleanObject(GameObject obj)
	{
		DisableRandomMaterialValues(obj);
		DestroyComponents<RandomFlyingBird>(obj);
		DestroyComponents<DungeonGenerator>(obj);
		DestroyComponents<MusicLocation>(obj);
		DisableComponents<Fish>(obj);
		DestroyComponents<Ragdoll>(obj);
		DestroyComponents<LocationProxy>(obj);
		DestroyComponents<MapTable>(obj);
		DestroyComponents<ArcheryTarget>(obj);
		DestroyComponents<FootStep>(obj);
		DestroyComponents<Growup>(obj);
		DestroyComponents<Windmill>(obj);
		DestroyComponents<MineRock>(obj);
		DestroyComponents<MineRock5>(obj);
		DestroyComponents<CharacterAnimEvent>(obj);
		DestroyComponents<TreeLog>(obj);
		DestroyComponents<TreeBase>(obj);
		DestroyComponents<CreatureSpawner>(obj);
		DestroyComponents<TombStone>(obj);
		DestroyComponents<Aoe>(obj);
		DestroyComponents<SpawnArea>(obj);
		DestroyComponents<Procreation>(obj);
		DestroyComponents<StaticPhysics>(obj);
		DestroyComponents<Tameable>(obj);
		DestroyComponents<Catapult>(obj);
		DestroyComponents<TimedDestruction>(obj);
		DestroyComponents<Vine>(obj);
		DestroyComponents<Plant>(obj);
		Character component = obj.GetComponent<Character>();
		if (Object.op_Implicit((Object)(object)component))
		{
			Character.s_characters.Remove(component);
		}
	}

	private static void DisableComponents<T>(GameObject obj) where T : MonoBehaviour
	{
		T[] componentsInChildren = obj.GetComponentsInChildren<T>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			((Behaviour)(object)componentsInChildren[i]).enabled = false;
		}
	}

	private static void DestroyComponents<T>(GameObject obj) where T : MonoBehaviour
	{
		T[] componentsInChildren = obj.GetComponentsInChildren<T>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.DestroyImmediate((Object)(object)componentsInChildren[i]);
		}
	}

	public static bool IsBuildPiece(GameObject obj)
	{
		GameObject obj2 = obj;
		return Player.m_localPlayer.m_buildPieces.m_pieces.Any((GameObject piece) => Utils.GetPrefabName(obj2) == Utils.GetPrefabName(piece));
	}

	public static void EnsurePiece(GameObject obj)
	{
		if (Object.op_Implicit((Object)(object)obj.GetComponent<Piece>()))
		{
			if ((from collider in obj.GetComponentsInChildren<Collider>()
				where collider.enabled && !collider.isTrigger
				select collider).ToArray().Length == 0)
			{
				obj.GetComponent<Piece>().m_clipEverything = true;
			}
			return;
		}
		Piece val = obj.AddComponent<Piece>();
		LocationProxy component = obj.GetComponent<LocationProxy>();
		if (Object.op_Implicit((Object)(object)component) && Object.op_Implicit((Object)(object)component.m_instance))
		{
			val.m_name = Utils.GetPrefabName(component.m_instance);
		}
		else
		{
			val.m_name = Utils.GetPrefabName(obj);
		}
		val.m_clipEverything = true;
		((StaticTarget)val).m_randomTarget = false;
		((StaticTarget)val).m_primaryTarget = false;
	}

	public static void EnabledCheck()
	{
		if (!Configuration.Enabled)
		{
			throw new InvalidOperationException("Infinity Hammer is disabled.");
		}
	}

	public static void Init()
	{
		EnabledCheck();
		Hammer.Equip();
	}

	public static string ConvertHashToName(int hash)
	{
		GameObject prefab = ZNetScene.instance.GetPrefab(hash);
		return ((prefab != null) ? ((Object)prefab).name : null) ?? hash.ToString(CultureInfo.InvariantCulture);
	}

	public static bool TryGetName(DataEntry data, int key, out string name)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		int hash = default(int);
		if (DataEntryExtensions.TryGetHash(data, dictionary, key, ref hash))
		{
			name = ConvertHashToName(hash);
			return true;
		}
		string text = default(string);
		if (DataEntryExtensions.TryGetString(data, dictionary, key, ref text))
		{
			name = text;
			return true;
		}
		name = "";
		return false;
	}

	public static bool TryGetNameHash(DataEntry data, int key, out int hash)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		int num = default(int);
		if (DataEntryExtensions.TryGetHash(data, dictionary, key, ref num))
		{
			hash = num;
			return true;
		}
		int num2 = default(int);
		if (DataEntryExtensions.TryGetInt(data, dictionary, key, ref num2))
		{
			hash = num2;
			return true;
		}
		string text = default(string);
		if (DataEntryExtensions.TryGetString(data, dictionary, key, ref text))
		{
			hash = ZDOKeys.Hash(text);
			return true;
		}
		hash = 0;
		return false;
	}

	public static bool IsDown(string key)
	{
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		bool checkDown = true;
		if (key.StartsWith("-<"))
		{
			flag = true;
			checkDown = false;
			key = key.Substring(2, key.Length - 3);
		}
		else if (key.StartsWith("<"))
		{
			flag = true;
			key = key.Substring(1, key.Length - 2);
		}
		else if (key.StartsWith("-"))
		{
			checkDown = false;
			key = key.Substring(1, key.Length - 1);
		}
		if (flag)
		{
			switch (key)
			{
			case "mod1":
				key = Configuration.ModifierKey1();
				flag = false;
				break;
			case "mod2":
				key = Configuration.ModifierKey2();
				flag = false;
				break;
			case "alt":
				key = "AltPlace";
				break;
			}
		}
		if (flag)
		{
			return ZInput.instance.TryGetButtonState(key, (Func<ButtonDef, bool>)((ButtonDef b) => b.Held == checkDown));
		}
		if (Enum.TryParse<KeyCode>(key, ignoreCase: true, out KeyCode result))
		{
			return Input.GetKey(result) == checkDown;
		}
		return false;
	}

	public static Sprite? FindSprite(string name)
	{
		return SpriteHelper.FindSprite(name);
	}
}
