using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch]
public static class DungeonRooms
{
	private static readonly List<DungeonGenerator> Dgs = new List<DungeonGenerator>();

	public static void AddRoom(ZNetView obj, int hash, Vector3 pos, Quaternion rot)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		ZDO zDO = obj.GetZDO();
		int @int = zDO.GetInt(ZDOVars.s_rooms, 0);
		byte[] byteArray = zDO.GetByteArray(ZDOVars.s_roomData, (byte[])null);
		if (@int > 0)
		{
			AddRoom(zDO, @int, hash, pos, rot);
		}
		else if (byteArray != null)
		{
			AddRoom(zDO, byteArray, hash, pos, rot);
		}
	}

	private static void AddRoom(ZDO zdo, int rooms, int hash, Vector3 pos, Quaternion rot)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		zdo.Set(ZDOVars.s_rooms, rooms + 1, false);
		zdo.Set(StringExtensionMethods.GetStableHashCode("room" + rooms), hash, false);
		zdo.Set(StringExtensionMethods.GetStableHashCode("room" + rooms + "_pos"), pos);
		zdo.Set(StringExtensionMethods.GetStableHashCode("room" + rooms + "_rot"), rot);
	}

	private static void AddRoom(ZDO zdo, byte[] data, int hash, Vector3 pos, Quaternion rot)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		DungeonGenerator.saveStream.SetLength(0L);
		BinaryReader binaryReader = new BinaryReader(new MemoryStream(data));
		int num = binaryReader.ReadInt32();
		DungeonGenerator.saveWriter.Write(num + 1);
		for (int i = 0; i < num; i++)
		{
			DungeonGenerator.saveWriter.Write(binaryReader.ReadInt32());
			Utils.Write(DungeonGenerator.saveWriter, Utils.ReadVector3(binaryReader));
			Utils.Write(DungeonGenerator.saveWriter, Utils.ReadQuaternion(binaryReader));
		}
		DungeonGenerator.saveWriter.Write(hash);
		Utils.Write(DungeonGenerator.saveWriter, pos);
		Utils.Write(DungeonGenerator.saveWriter, rot);
		zdo.Set(ZDOVars.s_roomData, DungeonGenerator.saveStream.ToArray());
	}

	public static void RemoveRoom(ZNetView obj, int index)
	{
		ZDO zDO = obj.GetZDO();
		int @int = zDO.GetInt(ZDOVars.s_rooms, 0);
		if (@int > 0)
		{
			Remove(zDO, @int, index);
		}
		byte[] byteArray = zDO.GetByteArray(ZDOVars.s_roomData, (byte[])null);
		if (byteArray != null)
		{
			Remove(zDO, byteArray, index);
		}
		if (((Component)obj).transform.childCount >= 1)
		{
			Transform child = ((Component)obj).transform.GetChild(index);
			if (Object.op_Implicit((Object)(object)child))
			{
				child.parent = null;
				Object.Destroy((Object)(object)((Component)child).gameObject);
			}
		}
	}

	private static void Remove(ZDO zdo, int rooms, int index)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		for (int i = index + 1; i < rooms; i++)
		{
			int stableHashCode = StringExtensionMethods.GetStableHashCode("room" + i);
			int stableHashCode2 = StringExtensionMethods.GetStableHashCode("room" + (i - 1));
			zdo.Set(stableHashCode2, zdo.GetInt(stableHashCode, 0), false);
			stableHashCode = StringExtensionMethods.GetStableHashCode("room" + i + "_pos");
			stableHashCode2 = StringExtensionMethods.GetStableHashCode("room" + (i - 1) + "_pos");
			zdo.Set(stableHashCode2, zdo.GetVec3(stableHashCode, Vector3.zero));
			stableHashCode = StringExtensionMethods.GetStableHashCode("room" + i + "_rot");
			stableHashCode2 = StringExtensionMethods.GetStableHashCode("room" + (i - 1) + "_rot");
			zdo.Set(stableHashCode2, zdo.GetQuaternion(stableHashCode, Quaternion.identity));
		}
		zdo.Set(ZDOVars.s_rooms, rooms - 1, false);
		zdo.RemoveInt(StringExtensionMethods.GetStableHashCode("room" + rooms));
		zdo.RemoveVec3(StringExtensionMethods.GetStableHashCode("room" + rooms + "_pos"));
		zdo.RemoveQuaternion(StringExtensionMethods.GetStableHashCode("room" + rooms + "_rot"));
	}

	private static void Remove(ZDO zdo, byte[] data, int index)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		DungeonGenerator.saveStream.SetLength(0L);
		BinaryReader binaryReader = new BinaryReader(new MemoryStream(data));
		int num = binaryReader.ReadInt32();
		DungeonGenerator.saveWriter.Write(num - 1);
		for (int i = 0; i < num; i++)
		{
			int value = binaryReader.ReadInt32();
			Vector3 val = Utils.ReadVector3(binaryReader);
			Quaternion val2 = Utils.ReadQuaternion(binaryReader);
			if (i != index)
			{
				DungeonGenerator.saveWriter.Write(value);
				Utils.Write(DungeonGenerator.saveWriter, val);
				Utils.Write(DungeonGenerator.saveWriter, val2);
			}
		}
		zdo.Set(ZDOVars.s_roomData, DungeonGenerator.saveStream.ToArray());
	}

	public static void Reposition(ZDO zdo, Transform target)
	{
		GameObject prefab = ZNetScene.instance.GetPrefab(zdo.GetPrefab());
		if (Object.op_Implicit((Object)(object)prefab) && Object.op_Implicit((Object)(object)prefab.GetComponent<DungeonGenerator>()))
		{
			List<Transform> trs = (from r in ((Component)target).GetComponentsInChildren<Room>()
				select ((Component)r).transform).ToList();
			byte[] byteArray = zdo.GetByteArray(ZDOVars.s_roomData, (byte[])null);
			int @int = zdo.GetInt(ZDOVars.s_rooms, 0);
			if (@int > 0)
			{
				Reposition(zdo, @int, trs);
			}
			else if (byteArray != null)
			{
				Reposition(zdo, byteArray, trs);
			}
		}
	}

	private static void Reposition(ZDO zdo, int rooms, List<Transform> trs)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < rooms; i++)
		{
			int stableHashCode = StringExtensionMethods.GetStableHashCode("room" + i + "_pos");
			int stableHashCode2 = StringExtensionMethods.GetStableHashCode("room" + i + "_rot");
			if (trs.Count <= i)
			{
				zdo.Set(stableHashCode, zdo.GetPosition());
				zdo.Set(stableHashCode2, zdo.GetRotation());
			}
			else
			{
				zdo.Set(stableHashCode, trs[i].position);
				zdo.Set(stableHashCode2, trs[i].rotation);
			}
		}
	}

	private static void Reposition(ZDO zdo, byte[] data, List<Transform> trs)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		DungeonGenerator.saveStream.SetLength(0L);
		BinaryReader binaryReader = new BinaryReader(new MemoryStream(data));
		int num = binaryReader.ReadInt32();
		DungeonGenerator.saveWriter.Write(num);
		for (int i = 0; i < num; i++)
		{
			DungeonGenerator.saveWriter.Write(binaryReader.ReadInt32());
			if (trs.Count <= i)
			{
				Utils.Write(DungeonGenerator.saveWriter, zdo.GetPosition());
				Utils.Write(DungeonGenerator.saveWriter, zdo.GetRotation());
			}
			else
			{
				Utils.Write(DungeonGenerator.saveWriter, trs[i].position);
				Utils.Write(DungeonGenerator.saveWriter, trs[i].rotation);
			}
		}
		zdo.Set(ZDOVars.s_roomData, DungeonGenerator.saveStream.ToArray());
	}

	public static DungeonGenerator? FindClosestDg(Vector3 pos)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return Dgs.OrderBy(delegate(DungeonGenerator dg)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			float num = Vector3.Distance(pos, ((Component)dg).transform.position);
			Room val = default(Room);
			foreach (Transform item in ((Component)dg).transform)
			{
				if (((Component)item).TryGetComponent<Room>(ref val))
				{
					RoomConnection[] connections = val.GetConnections();
					foreach (RoomConnection val2 in connections)
					{
						float num2 = Vector3.Distance(pos, ((Component)val2).transform.position);
						if (num2 < num)
						{
							num = num2;
						}
					}
				}
			}
			return num;
		}).FirstOrDefault();
	}

	[HarmonyPatch(typeof(DungeonGenerator), "Awake")]
	[HarmonyPostfix]
	private static void DungeonGeneratorAwake(DungeonGenerator __instance)
	{
		Dgs.Add(__instance);
	}

	[HarmonyPatch(typeof(DungeonGenerator), "OnDestroy")]
	[HarmonyPostfix]
	private static void DungeonGeneratorDestroy(DungeonGenerator __instance)
	{
		Dgs.Remove(__instance);
	}

	[HarmonyPatch(typeof(Piece), "GetSnapPoints", new Type[]
	{
		typeof(Vector3),
		typeof(float),
		typeof(List<Transform>),
		typeof(List<Piece>)
	})]
	[HarmonyPrefix]
	private static void GetSnapPointsRadius(ref float radius)
	{
		float snapMultiplier = Selection.Get().SnapMultiplier;
		radius *= snapMultiplier;
	}

	[HarmonyPatch(typeof(Piece), "GetSnapPoints", new Type[]
	{
		typeof(Vector3),
		typeof(float),
		typeof(List<Transform>),
		typeof(List<Piece>)
	})]
	[HarmonyPostfix]
	private static void GetSnapPoints(Vector3 point, float radius, List<Transform> points)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		float dungeonRoomSnapMultiplier = Selection.Get().DungeonRoomSnapMultiplier;
		Room val = default(Room);
		foreach (DungeonGenerator dg in Dgs)
		{
			foreach (Transform item in ((Component)dg).transform)
			{
				if (!((Component)item).TryGetComponent<Room>(ref val))
				{
					continue;
				}
				RoomConnection[] connections = val.GetConnections();
				foreach (RoomConnection val2 in connections)
				{
					if (Vector3.Distance(point, ((Component)val2).transform.position) <= radius * dungeonRoomSnapMultiplier)
					{
						points.Add(((Component)val2).transform);
					}
				}
			}
		}
	}
}
