using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class UndoHelper
{
	private static readonly Dictionary<ZDOID, EditData> EditedInfo = new Dictionary<ZDOID, EditData>();

	private static readonly List<RemoveData> RemovedInfo = new List<RemoveData>();

	private static readonly List<ZDO> SpawnedInfo = new List<ZDO>();

	private static readonly List<UndoTerrain> TerrainInfo = new List<UndoTerrain>();

	private static int SubStack = 0;

	private static readonly Dictionary<ZDOID, ZDOID> Regenerated = new Dictionary<ZDOID, ZDOID>();

	public static void BeginAction()
	{
		SubStack = 0;
		AddedZDOs.Track = true;
		EditedInfo.Clear();
		RemovedInfo.Clear();
		SpawnedInfo.Clear();
		TerrainInfo.Clear();
	}

	public static List<ZDO> GetSpawned()
	{
		return SpawnedInfo;
	}

	public static void BeginSubAction()
	{
		if (SubStack == 0)
		{
			BeginAction();
		}
		SubStack++;
	}

	public static void EndSubAction()
	{
		SubStack--;
		if (SubStack == 0)
		{
			EndAction();
		}
	}

	public static void AddRemoveAction(ZDO zdo)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		RemovedInfo.Add(new RemoveData(zdo));
		EditedInfo.Remove(zdo.m_uid);
	}

	public static void AddSpawnAction(ZDO zdo)
	{
		SpawnedInfo.Add(zdo);
	}

	public static void AddTerrainAction(Dictionary<Vector3, TerrainUndoData> before, Dictionary<Vector3, TerrainUndoData> after, Vector3 position, float radius)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		TerrainInfo.Add(new UndoTerrain(before, after, position, radius));
	}

	public static void AddEditAction(ZNetView view)
	{
		AddEditAction(view.GetZDO());
	}

	public static void AddEditAction(ZDO zdo)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (zdo.GetPrefab() != Hash.Player && !EditedInfo.ContainsKey(zdo.m_uid))
		{
			EditedInfo[zdo.m_uid] = new EditData(zdo);
		}
	}

	public static void RefreshEditAction(ZDOID original, ZDO zdo)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		EditData editData = EditedInfo[original];
		editData.Original = zdo;
		EditedInfo[zdo.m_uid] = editData;
		EditedInfo.Remove(original);
	}

	public static void EndAction()
	{
		SubStack = 0;
		AddedZDOs.Track = false;
		if (EditedInfo.Count == 0 && RemovedInfo.Count == 0 && SpawnedInfo.Count == 0 && TerrainInfo.Count == 0)
		{
			return;
		}
		foreach (EditData value in EditedInfo.Values)
		{
			value.Update();
		}
		List<UndoData> list = new List<UndoData>();
		list.AddRange(EditedInfo.Values);
		list.AddRange(RemovedInfo);
		list.AddRange(from zdo in SpawnedInfo
			where zdo != null && zdo.IsValid()
			select zdo into s
			select new SpawnData(s));
		UndoData[] array = list.ToArray();
		if (array.Length != 0 || TerrainInfo.Count != 0)
		{
			UndoManager.Add((IUndoAction)(object)new UndoAction(array, TerrainInfo));
		}
	}

	public static void Place(FakeZDO zdo)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		GameObject prefab = ZNetScene.instance.GetPrefab(zdo.Prefab);
		if (!Object.op_Implicit((Object)(object)prefab))
		{
			throw new InvalidOperationException("Error: Prefab not found.");
		}
		ZNetView.m_initZDO = zdo.Create();
		Regenerated[zdo.Id] = ZNetView.m_initZDO.m_uid;
		zdo.Id = ZNetView.m_initZDO.m_uid;
		Object.Instantiate<GameObject>(prefab, ZNetView.m_initZDO.GetPosition(), ZNetView.m_initZDO.GetRotation());
	}

	public static void Destroy(FakeZDO zdo)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		while (Regenerated.ContainsKey(zdo.Id))
		{
			zdo.Id = Regenerated[zdo.Id];
		}
		zdo.Destroy();
	}

	public static string Print(IEnumerable<FakeZDO> zdos)
	{
		return Print(zdos.Select((FakeZDO zdo) => zdo.Prefab));
	}

	public static string Print(IEnumerable<int> data)
	{
		if (data.Count() == 1)
		{
			return DataHelper.Name(data.First());
		}
		IEnumerable<IGrouping<string, int>> source = data.GroupBy(DataHelper.Name);
		if (source.Count() == 1)
		{
			return $"{source.First().Key} {source.First().Count()}x";
		}
		return $" objects {data.Count()}x";
	}
}
