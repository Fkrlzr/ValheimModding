using UnityEngine;

namespace Data;

public class FakeZDO
{
	public readonly PlainDataEntry Data = new PlainDataEntry(zdo);

	public int Prefab = zdo.m_prefab;

	public Vector3 Position = zdo.m_position;

	public Vector3 Rotation = zdo.m_rotation;

	public ZDOID Id = zdo.m_uid;

	public FakeZDO(ZDO zdo)
	{
	}//IL_001a: Unknown result type (might be due to invalid IL or missing references)
	//IL_001f: Unknown result type (might be due to invalid IL or missing references)
	//IL_0026: Unknown result type (might be due to invalid IL or missing references)
	//IL_002b: Unknown result type (might be due to invalid IL or missing references)
	//IL_0032: Unknown result type (might be due to invalid IL or missing references)
	//IL_0037: Unknown result type (might be due to invalid IL or missing references)


	public ZDO Create()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		ZDO val = ZDOMan.instance.CreateNewZDO(Position, Prefab);
		Write(val);
		val.DataRevision = 0u;
		val.IncreaseDataRevision();
		return val;
	}

	public void Write(ZDO zdo)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		zdo.m_prefab = Prefab;
		zdo.m_position = Position;
		zdo.m_rotation = Rotation;
		((ResolvedDataEntry)Data).Write(zdo);
	}

	public void Destroy()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		ZDO zDO = ZDOMan.instance.GetZDO(Id);
		if (zDO != null)
		{
			if (!zDO.IsOwner())
			{
				zDO.SetOwner(ZDOMan.instance.m_sessionID);
			}
			ZDOMan.instance.DestroyZDO(zDO);
		}
	}
}
