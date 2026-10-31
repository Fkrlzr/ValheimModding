using Data;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class BaseSelection
{
	protected static BaseSelection? ActiveSelection;

	protected GameObject SelectedPrefab;

	public bool SingleUse;

	public virtual bool IsTool => false;

	public virtual bool TerrainGrid => false;

	public virtual bool Continuous => false;

	public virtual bool PlayerHeight => false;

	public virtual float DungeonRoomSnapMultiplier => 1f;

	public virtual float SnapMultiplier => 1f;

	public virtual float MaxPlaceDistance(float value)
	{
		if (!(Configuration.Range > 0f))
		{
			return value;
		}
		return Configuration.Range;
	}

	public virtual Piece GetSelectedPiece()
	{
		if (!Object.op_Implicit((Object)(object)SelectedPrefab))
		{
			return null;
		}
		return SelectedPrefab.GetComponent<Piece>();
	}

	public virtual void Destroy()
	{
		if (Object.op_Implicit((Object)(object)SelectedPrefab))
		{
			SelectedPrefab.SetActive(false);
		}
		Object.Destroy((Object)(object)SelectedPrefab);
		SelectedPrefab = null;
		if (ActiveSelection == this)
		{
			ActiveSelection = null;
		}
	}

	public virtual DataEntry? GetData(int index = 0)
	{
		return null;
	}

	public virtual int GetPrefab(int index = 0)
	{
		return 0;
	}

	public virtual bool IsScalingSupported()
	{
		return false;
	}

	public virtual GameObject GetPrefab(GameObject obj, Vector3 pos, Quaternion rot)
	{
		return obj;
	}

	public virtual void AfterPlace(GameObject obj)
	{
	}

	public void SetScale(Vector3 value)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)SelectedPrefab) && IsScalingSupported())
		{
			SelectedPrefab.transform.localScale = value;
		}
	}

	public static void PostProcessPlaced(GameObject obj)
	{
		ZNetView component = obj.GetComponent<ZNetView>();
		if (!Configuration.Enabled || !Object.op_Implicit((Object)(object)component))
		{
			return;
		}
		Piece val = default(Piece);
		if (obj.TryGetComponent<Piece>(ref val))
		{
			val.m_canBeRemoved = true;
			NoCreator.Set(component, val);
		}
		EnableSyncScale(component);
		CustomHealth.SetHealth(component, isRepair: false);
		DungeonGenerator val2 = default(DungeonGenerator);
		if (obj.TryGetComponent<DungeonGenerator>(ref val2))
		{
			ZDO zDO = component.GetZDO();
			byte[] byteArray = zDO.GetByteArray(ZDOVars.s_roomData, (byte[])null);
			if (zDO.GetInt(ZDOVars.s_rooms, 0) == 0 && byteArray == null)
			{
				val2.Generate((SpawnMode)0);
			}
		}
	}

	private static void EnableSyncScale(ZNetView view)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		ZSyncTransform val = default(ZSyncTransform);
		if (!Configuration.ScaleZSyncObjects || view.m_syncInitialScale || !((Component)view).TryGetComponent<ZSyncTransform>(ref val))
		{
			return;
		}
		Vector3 prefabScale = GetPrefabScale(StringExtensionMethods.GetStableHashCode(view.GetPrefabName()));
		Vector3 val2 = Scaling.Get().Vec3 - prefabScale;
		if (!(((Vector3)(ref val2)).sqrMagnitude <= 0.0001f))
		{
			Vector3 vec = Scaling.Get().Vec3;
			((Component)view).transform.localScale = vec;
			val.m_syncScale = true;
			ZDO zDO = view.GetZDO();
			zDO.Set(Hashes.HasFields, true);
			zDO.Set(Hashes.HasFieldsZSyncTransform, true);
			zDO.Set(Hashes.ZSyncTransformSyncScale, true);
			if (Mathf.Approximately(vec.x, vec.y) && Mathf.Approximately(vec.x, vec.z))
			{
				zDO.Set(ZDOVars.s_scaleScalarHash, vec.x);
				zDO.Set(Hashes.ScaleBackupScalarHash, vec.x);
			}
			else
			{
				zDO.Set(ZDOVars.s_scaleHash, vec);
				zDO.Set(Hashes.ScaleBackupScalarHash, vec);
			}
		}
	}

	private static Vector3 GetPrefabScale(int hash)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		GameObject prefab = ZNetScene.instance.GetPrefab(hash);
		if (!Object.op_Implicit((Object)(object)prefab))
		{
			return Vector3.one;
		}
		return prefab.transform.localScale;
	}

	public virtual void Activate()
	{
		ActiveSelection?.Deactivate();
		BindCommand.SetMode("");
		ActiveSelection = this;
	}

	public virtual void Deactivate()
	{
	}
}
