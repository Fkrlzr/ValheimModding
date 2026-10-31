using System.Collections.Generic;
using System.Linq;
using Service;
using UnityEngine;

namespace InfinityHammer;

public class Blueprint
{
	public string Name = "";

	public string Description = "";

	public string Creator = "";

	public string Category = "";

	public Vector3 Coordinates = Vector3.zero;

	public Vector3 Rotation = Vector3.zero;

	public string CenterPiece = Configuration.BlueprintCenterPiece;

	public List<BlueprintObject> Objects = new List<BlueprintObject>();

	public List<Vector3> SnapPoints = new List<Vector3>();

	public TerrainHeight? TerrainHeight;

	public TerrainPaint? TerrainPaint;

	public float Radius;

	public Vector3 Center(string centerPiece)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		if (centerPiece != "")
		{
			CenterPiece = centerPiece;
		}
		Bounds val = default(Bounds);
		float num = float.MaxValue;
		Quaternion rot = Quaternion.identity;
		foreach (BlueprintObject @object in Objects)
		{
			num = Mathf.Min(num, @object.Pos.y);
			((Bounds)(ref val)).Encapsulate(@object.Pos);
		}
		Vector3 center = new Vector3(((Bounds)(ref val)).center.x, num, ((Bounds)(ref val)).center.z);
		foreach (BlueprintObject object2 in Objects)
		{
			if (object2.Prefab == CenterPiece)
			{
				center = object2.Pos;
				rot = Quaternion.Inverse(object2.Rot);
				break;
			}
		}
		Radius = Utils.LengthXZ(((Bounds)(ref val)).extents);
		foreach (BlueprintObject object3 in Objects)
		{
			object3.Pos -= center;
		}
		SnapPoints = SnapPoints.Select((Vector3 p) => p - center).ToList();
		if (TerrainHeight != null)
		{
			TerrainHeight? terrainHeight = TerrainHeight;
			terrainHeight.CenterPosition -= TerrainHeight.CenterRotation * center;
			for (int i = 0; i < TerrainHeight.Width; i++)
			{
				for (int j = 0; j < TerrainHeight.Height; j++)
				{
					if (TerrainHeight.Heights[i, j].HasValue)
					{
						TerrainHeight.Heights[i, j] -= center.y;
					}
				}
			}
		}
		if (TerrainPaint != null)
		{
			TerrainPaint? terrainPaint = TerrainPaint;
			terrainPaint.CenterPosition -= TerrainPaint.CenterRotation * center;
		}
		if (rot != Quaternion.identity)
		{
			foreach (BlueprintObject object4 in Objects)
			{
				object4.Pos = rot * object4.Pos;
				object4.Rot = rot * object4.Rot;
			}
			SnapPoints = SnapPoints.Select((Vector3 p) => rot * p).ToList();
			if (TerrainHeight != null)
			{
				Quaternion val2 = TerrainHeight.CenterRotation * Quaternion.Inverse(rot);
				TerrainHeight.CenterRotation = Quaternion.Euler(0f, ((Quaternion)(ref val2)).eulerAngles.y, 0f);
			}
			if (TerrainPaint != null)
			{
				Quaternion val3 = TerrainPaint.CenterRotation * Quaternion.Inverse(rot);
				TerrainPaint.CenterRotation = Quaternion.Euler(0f, ((Quaternion)(ref val3)).eulerAngles.y, 0f);
			}
		}
		return center;
	}
}
