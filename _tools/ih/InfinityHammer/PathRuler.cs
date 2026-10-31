using System;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityHammer;

public class PathRuler : BaseRuler
{
	public List<Vector3> Points = new List<Vector3>();

	public Vector3 Cursor;

	public float Radius = 0.5f;

	private const int MaxLineSegments = 500;

	private const float Speed = 0.1f;

	protected override bool HasCenter => false;

	private (Vector3 from, Vector3 to) Leg(int index)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		return (from: Points[index], to: (index + 1 < Points.Count) ? Points[index + 1] : Cursor);
	}

	private static int Steps(float length, float spacing)
	{
		if (!(length < 0.1f))
		{
			return Math.Max(1, Mathf.RoundToInt(length / spacing));
		}
		return 0;
	}

	protected override void CreateLines()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		int num = ((Points.Count != 0) ? Math.Max(3, (int)(Radius * 4f)) : 0);
		float num2 = 0f;
		for (int i = 0; i < Points.Count; i++)
		{
			(Vector3 from, Vector3 to) tuple = Leg(i);
			Vector3 item = tuple.from;
			Vector3 item2 = tuple.to;
			num2 += Utils.DistanceXZ(item, item2);
		}
		float spacing = Mathf.Max(1f, num2 / 500f);
		int num3 = 0;
		for (int j = 0; j < Points.Count; j++)
		{
			(Vector3 from, Vector3 to) tuple2 = Leg(j);
			Vector3 item3 = tuple2.from;
			Vector3 item4 = tuple2.to;
			num3 += Steps(Utils.DistanceXZ(item3, item4), spacing);
		}
		CreateSegments(num * Points.Count + num3);
		int num4 = 0;
		float num5 = Time.time * 0.1f;
		foreach (Vector3 point in Points)
		{
			int num6 = 0;
			while (num6 < num)
			{
				float num7 = (float)num6 * (float)Math.PI * 2f / (float)num + num5;
				Set(num4, point + new Vector3(Mathf.Sin(num7) * Radius, 0f, Mathf.Cos(num7) * Radius));
				SetRot(num4, new Vector3(Mathf.Cos(num7), 0f, 0f - Mathf.Sin(num7)));
				num6++;
				num4++;
			}
		}
		for (int k = 0; k < Points.Count; k++)
		{
			(Vector3 from, Vector3 to) tuple3 = Leg(k);
			Vector3 item5 = tuple3.from;
			Vector3 item6 = tuple3.to;
			int num8 = Steps(Utils.DistanceXZ(item5, item6), spacing);
			Vector3 rot = Utils.DirectionXZ(item6 - item5);
			int num9 = 0;
			while (num9 < num8)
			{
				Set(num4, Vector3.Lerp(item5, item6, ((float)num9 + 0.5f) / (float)num8));
				SetRot(num4, rot);
				num9++;
				num4++;
			}
		}
	}
}
