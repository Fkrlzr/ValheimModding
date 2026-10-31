using UnityEngine;

namespace InfinityHammer;

public class BlueprintObject
{
	public string Prefab;

	public Vector3 Pos;

	public Quaternion Rot;

	public string Data;

	public Vector3 Scale;

	public float Chance;

	public string ExtraInfo;

	public BlueprintObject(string name, Vector3 pos, Quaternion rot, Vector3 scale, string info, string data, float chance)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		Prefab = name;
		Pos = pos;
		Rot = ((Quaternion)(ref rot)).normalized;
		Data = data;
		Scale = scale;
		Chance = chance;
		ExtraInfo = info;
		base._002Ector();
	}
}
