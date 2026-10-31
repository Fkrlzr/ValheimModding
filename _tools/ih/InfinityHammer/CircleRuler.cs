using System;
using UnityEngine;

namespace InfinityHammer;

public class CircleRuler : BaseRuler
{
	public float Radius = 20f;

	private readonly float Speed = 0.1f;

	protected override void CreateLines()
	{
		int count = Math.Max(3, (int)(Radius * 4f));
		CreateSegments(count);
		UpdateLines();
	}

	private void UpdateLines()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		int count = Segments.Count;
		float num = (float)Math.PI * 2f / (float)count;
		float num2 = Time.time * Speed;
		Vector3 pos = default(Vector3);
		for (int i = 0; i < count; i++)
		{
			float num3 = (float)i * num + num2;
			((Vector3)(ref pos))._002Ector(Mathf.Sin(num3) * Radius, 0f, Mathf.Cos(num3) * Radius);
			Set(i, pos);
		}
		for (int j = 0; j < count; j++)
		{
			GameObject obj = ((j == 0) ? Segments[Segments.Count - 1] : Segments[j - 1]);
			GameObject val = ((j == Segments.Count - 1) ? Segments[0] : Segments[j + 1]);
			Vector3 val2 = obj.transform.localPosition - val.transform.localPosition;
			Vector3 normalized = ((Vector3)(ref val2)).normalized;
			SetRot(j, normalized);
		}
	}
}
