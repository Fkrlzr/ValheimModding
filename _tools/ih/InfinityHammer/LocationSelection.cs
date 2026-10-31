using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using WorldEditCommands;

namespace InfinityHammer;

public class LocationSelection : BaseSelection
{
	private readonly GameObject Wrapper;

	private readonly SelectedObject Object;

	public override void Destroy()
	{
		base.Destroy();
		Object.Destroy((Object)(object)Wrapper);
	}

	public LocationSelection(ZoneLocation location, int seed)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		if (location == null)
		{
			throw new InvalidOperationException("Location not found.");
		}
		Wrapper = new GameObject();
		Wrapper.SetActive(false);
		SelectedPrefab = HammerHelper.SafeInstantiateLocation(location, Hammer.AllLocationsObjects ? null : new int?(seed), Wrapper);
		DataEntry val = new DataEntry();
		DataEntryExtensions.Set(val, ZDOVars.s_location, location.Hash);
		DataEntryExtensions.Set(val, ZDOVars.s_seed, seed);
		Object = new SelectedObject(location.Hash, scalable: false, val);
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
		return ZoneSystem.instance.m_locationProxyPrefab;
	}

	public override void AfterPlace(GameObject obj)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		DataEntry data = GetData();
		if (data == null)
		{
			return;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		int num = default(int);
		int num2 = default(int);
		if (!DataEntryExtensions.TryGetInt(data, dictionary, ZDOVars.s_location, ref num) || !DataEntryExtensions.TryGetInt(data, dictionary, ZDOVars.s_seed, ref num2))
		{
			return;
		}
		ZoneLocation location = ZoneSystem.instance.GetLocation(num);
		if (location == null)
		{
			throw new InvalidOperationException("Location not found.");
		}
		GameObject placementGhost = HammerHelper.GetPlacementGhost();
		Vector3 position = placementGhost.transform.position;
		Quaternion rotation = placementGhost.transform.rotation;
		bool? spawnRandomDamage = Placing.SpawnRandomDamage;
		bool spawnAllViews = Placing.SpawnAllViews;
		bool randomInitialDamage = WearNTear.m_randomInitialDamage;
		UndoHelper.BeginSubAction();
		try
		{
			HammerHelper.RemoveZDO(obj.GetComponent<ZNetView>().GetZDO());
			Placing.SpawnAllViews = Hammer.AllLocationsObjects;
			Placing.SpawnRandomDamage = Hammer.RandomLocationDamage;
			ZoneSystem.instance.SpawnLocation(location, num2, position, rotation, (SpawnMode)0, new List<GameObject>(), false);
			foreach (ZDO item in UndoHelper.GetSpawned())
			{
				if (ZNetScene.instance.m_instances.TryGetValue(item, out var value))
				{
					BaseSelection.PostProcessPlaced(((Component)value).gameObject);
				}
			}
		}
		finally
		{
			Placing.SpawnRandomDamage = spawnRandomDamage;
			Placing.SpawnAllViews = spawnAllViews;
			WearNTear.m_randomInitialDamage = randomInitialDamage;
			UndoHelper.EndSubAction();
		}
	}
}
