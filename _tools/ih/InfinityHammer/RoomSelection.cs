using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Service;
using SoftReferenceableAssets;
using UnityEngine;
using WorldEditCommands;

namespace InfinityHammer;

public class RoomSelection : BaseSelection
{
	private readonly GameObject Wrapper;

	private readonly SelectedObject Object;

	private readonly bool PlaceEmptyRoom;

	public override float DungeonRoomSnapMultiplier => 100f;

	public override void Destroy()
	{
		base.Destroy();
		Object.Destroy((Object)(object)Wrapper);
	}

	public RoomSelection(int hash, bool emptyRoom)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		PlaceEmptyRoom = emptyRoom;
		RoomData room = DungeonDB.m_instance.GetRoom(hash);
		if (room == null)
		{
			throw new InvalidOperationException("Room not found.");
		}
		Wrapper = new GameObject();
		Wrapper.SetActive(false);
		SelectedPrefab = HammerHelper.SafeInstantiateRoom(room, emptyRoom, Wrapper);
		Object = new SelectedObject(hash, scalable: false, null);
		List<Vector3> points = (from c in SelectedPrefab.GetComponentsInChildren<RoomConnection>(false).ToList()
			select ((Component)c).transform.position).ToList();
		Snapping.CreateSnapPoints(SelectedPrefab, points);
	}

	public override DataEntry? GetData(int index = 0)
	{
		return Object.Data;
	}

	public override int GetPrefab(int index = 0)
	{
		return Object.Prefab;
	}

	public override GameObject GetPrefab(GameObject obj, Vector3 pos, Quaternion rot)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		GameObject val = new GameObject
		{
			name = "Blueprint"
		};
		val.AddComponent<Piece>();
		return val;
	}

	public override void AfterPlace(GameObject obj)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = obj.transform.position;
		Quaternion rotation = obj.transform.rotation;
		DungeonGenerator val = DungeonRooms.FindClosestDg(position);
		if ((Object)(object)val == (Object)null)
		{
			Log.Error("DungeonGenerator not found.");
			return;
		}
		RoomData room = DungeonDB.m_instance.GetRoom(Object.Prefab);
		if (room == null)
		{
			Log.Error("Room not found.");
			return;
		}
		room.m_prefab.Load();
		Room component = room.m_prefab.Asset.GetComponent<Room>();
		ZNetView[] enabledComponentsInChildren = Utils.GetEnabledComponentsInChildren<ZNetView>(room.m_prefab.Asset);
		RandomSpawn[] enabledComponentsInChildren2 = Utils.GetEnabledComponentsInChildren<RandomSpawn>(room.m_prefab.Asset);
		for (int i = 0; i < enabledComponentsInChildren2.Length; i++)
		{
			enabledComponentsInChildren2[i].Prepare();
		}
		int num = (int)position.x * 4271 + (int)position.y * 9187 + (int)position.z * 2134;
		State state = Random.state;
		Random.InitState(num);
		Vector3 position2 = ((Component)component).transform.position;
		Quaternion val2 = Quaternion.Inverse(((Component)component).transform.rotation);
		Random.InitState(num);
		RandomSpawn[] array = enabledComponentsInChildren2;
		foreach (RandomSpawn val3 in array)
		{
			Vector3 val4 = val2 * (((Component)val3).gameObject.transform.position - position2);
			Vector3 val5 = position + rotation * val4;
			val3.Randomize(val5, (Location)null, val);
		}
		UndoHelper.BeginSubAction();
		ZNetView[] array2;
		if (!PlaceEmptyRoom)
		{
			array2 = enabledComponentsInChildren;
			foreach (ZNetView val6 in array2)
			{
				if (((Component)val6).gameObject.activeSelf)
				{
					Vector3 val7 = val2 * (((Component)val6).gameObject.transform.position - position2);
					Vector3 val8 = position + rotation * val7;
					Quaternion val9 = val2 * ((Component)val6).gameObject.transform.rotation;
					Quaternion val10 = rotation * val9;
					GameObjectExtentions.HoldReferenceTo(Object.Instantiate<GameObject>(((Component)val6).gameObject, val8, val10), room.m_prefab);
				}
			}
		}
		array2 = enabledComponentsInChildren;
		for (int j = 0; j < array2.Length; j++)
		{
			((Component)array2[j]).gameObject.SetActive(false);
		}
		UndoHelper.AddEditAction(val.m_nview.GetZDO());
		DungeonRooms.AddRoom(val.m_nview, Object.Prefab, obj.transform.position, obj.transform.rotation);
		Object.Instantiate<GameObject>(room.m_prefab.m_loadedAsset, position, rotation, ((Component)val).transform);
		UndoHelper.EndSubAction();
		Random.state = state;
		array = enabledComponentsInChildren2;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].Reset();
		}
		array2 = enabledComponentsInChildren;
		for (int j = 0; j < array2.Length; j++)
		{
			((Component)array2[j]).gameObject.SetActive(true);
		}
		room.m_prefab.Release();
	}
}
