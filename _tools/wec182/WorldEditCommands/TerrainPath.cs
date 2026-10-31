using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WorldEditCommands;

public static class TerrainPath
{
	private const float Resolution = 0.5f;

	public static List<Vector3> Sample(List<Vector3> points, PathMode mode)
	{
		switch (mode)
		{
		case PathMode.Line:
		{
			List<Vector3> list = new List<Vector3>(points.Count);
			list.AddRange(points);
			return list;
		}
		case PathMode.Arc:
			return SampleArcs(points);
		default:
			return SampleCurve(points);
		}
	}

	private static List<Vector3> SampleCurve(List<Vector3> points)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		List<Vector3> list = new List<Vector3>();
		int count = points.Count;
		for (int i = 0; i < count - 1; i++)
		{
			Vector3 val = points[i];
			Vector3 val2 = points[i + 1];
			Vector3 p = ((i > 0) ? points[i - 1] : (2f * val - val2));
			Vector3 p2 = ((i < count - 2) ? points[i + 2] : (2f * val2 - val));
			int num = Mathf.Max(1, Mathf.CeilToInt(Utils.DistanceXZ(val, val2) / 0.5f));
			for (int j = 0; j < num; j++)
			{
				list.Add(CatmullRom(p, val, val2, p2, (float)j / (float)num));
			}
		}
		list.Add(points[count - 1]);
		return list;
	}

	private static float Knot(Vector3 a, Vector3 b)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return Mathf.Max(Mathf.Sqrt(Vector3.Distance(a, b)), 0.01f);
	}

	private static Vector3 Interp(Vector3 a, Vector3 b, float ta, float tb, float u)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		return Vector3.LerpUnclamped(a, b, (u - ta) / (tb - ta));
	}

	private static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		float num2 = num + Knot(p0, p1);
		float num3 = num2 + Knot(p1, p2);
		float tb = num3 + Knot(p2, p3);
		float u = Mathf.Lerp(num2, num3, t);
		Vector3 a = Interp(p0, p1, num, num2, u);
		Vector3 val = Interp(p1, p2, num2, num3, u);
		Vector3 b = Interp(p2, p3, num3, tb, u);
		Vector3 a2 = Interp(a, val, num, num3, u);
		Vector3 b2 = Interp(val, b, num2, tb, u);
		return Interp(a2, b2, num2, num3, u);
	}

	private static List<Vector3> SampleArcs(List<Vector3> points)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		int count = points.Count;
		Vector2[] array = (Vector2[])(object)new Vector2[count];
		for (int i = 1; i < count - 1; i++)
		{
			Vector2 val = Direction(points[i - 1], points[i]) + Direction(points[i], points[i + 1]);
			array[i] = ((((Vector2)(ref val)).sqrMagnitude > 1E-06f) ? ((Vector2)(ref val)).normalized : Direction(points[i], points[i + 1]));
		}
		Vector2 val2 = Direction(points[0], points[1]);
		Vector2 val3 = Direction(points[count - 2], points[count - 1]);
		array[0] = ((count > 2) ? Reflect(array[1], val2) : val2);
		array[count - 1] = ((count > 2) ? Reflect(array[count - 2], val3) : val3);
		List<Vector3> result = new List<Vector3>(1) { points[0] };
		for (int j = 0; j < count - 1; j++)
		{
			AddBiarc(result, points[j], array[j], points[j + 1], array[j + 1]);
		}
		return result;
	}

	private static Vector2 Reflect(Vector2 t, Vector2 chord)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		return 2f * Vector2.Dot(chord, t) * chord - t;
	}

	private static void AddBiarc(List<Vector3> result, Vector3 a, Vector2 ta, Vector3 b, Vector2 tb)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(b.x - a.x, b.z - a.z);
		float sqrMagnitude = ((Vector2)(ref val)).sqrMagnitude;
		if (sqrMagnitude != 0f)
		{
			Vector2 val2 = ta + tb;
			float num = Vector2.Dot(val, val2);
			float num2 = 2f * (1f - Vector2.Dot(ta, tb));
			float num4;
			if (num2 < 1E-06f)
			{
				float num3 = Vector2.Dot(val, tb);
				num4 = ((num3 > 1E-06f) ? (sqrMagnitude / (4f * num3)) : (Mathf.Sqrt(sqrMagnitude) / 2f));
			}
			else
			{
				num4 = (0f - num + Mathf.Sqrt(num * num + num2 * sqrMagnitude)) / num2;
			}
			Vector2 val3 = (new Vector2(a.x + b.x, a.z + b.z) + num4 * (ta - tb)) / 2f;
			Vector3 val4 = default(Vector3);
			((Vector3)(ref val4))._002Ector(val3.x, (a.y + b.y) / 2f, val3.y);
			Vector2 tangent = AddArc(result, a, val4, ta);
			AddArc(result, val4, b, tangent);
		}
	}

	private static Vector2 Direction(Vector3 from, Vector3 to)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(to.x - from.x, to.z - from.z);
		if (!(((Vector2)(ref val)).sqrMagnitude > 0f))
		{
			return Vector2.up;
		}
		return ((Vector2)(ref val)).normalized;
	}

	private static Vector2 AddArc(List<Vector3> result, Vector3 a, Vector3 b, Vector2 tangent)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		float num = b.x - a.x;
		float num2 = b.z - a.z;
		float num3 = num * num + num2 * num2;
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(0f - tangent.y, tangent.x);
		float num4 = num * val.x + num2 * val.y;
		if (num3 == 0f)
		{
			return tangent;
		}
		if (Mathf.Abs(num4) <= 0.0001f * Mathf.Sqrt(num3) || num * tangent.x + num2 * tangent.y < 0f)
		{
			int num5 = Mathf.Max(1, Mathf.CeilToInt(Mathf.Sqrt(num3) / 0.5f));
			for (int i = 1; i <= num5; i++)
			{
				result.Add(Vector3.Lerp(a, b, (float)i / (float)num5));
			}
			return Direction(a, b);
		}
		float num6 = num3 / (2f * num4);
		float num7 = Mathf.Abs(num6);
		float num8 = Mathf.Sign(num6);
		float num9 = a.x + val.x * num6;
		float num10 = a.z + val.y * num6;
		float num11 = Mathf.Atan2(a.z - num10, a.x - num9);
		float num12 = Mathf.Atan2(b.z - num10, b.x - num9);
		float num13 = (float)Math.PI * 2f;
		float num14 = ((num8 > 0f) ? Mathf.Repeat(num12 - num11, num13) : (0f - Mathf.Repeat(num11 - num12, num13)));
		int num15 = Mathf.Max(1, Mathf.CeilToInt(num7 * Mathf.Abs(num14) / 0.5f));
		for (int j = 1; j <= num15; j++)
		{
			float num16 = num11 + num14 * (float)j / (float)num15;
			result.Add(new Vector3(num9 + num7 * Mathf.Cos(num16), Mathf.Lerp(a.y, b.y, (float)j / (float)num15), num10 + num7 * Mathf.Sin(num16)));
		}
		return num8 * new Vector2(0f - Mathf.Sin(num12), Mathf.Cos(num12));
	}

	public static float ClosestSegment(List<Vector3> path, IEnumerable<int> segments, Vector3 pos, out int segment, out float t)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		float num = float.MaxValue;
		segment = -1;
		t = 0f;
		foreach (int segment2 in segments)
		{
			Vector3 val = path[segment2];
			Vector3 val2 = path[segment2 + 1];
			float num2 = val2.x - val.x;
			float num3 = val2.z - val.z;
			float num4 = num2 * num2 + num3 * num3;
			float num5 = ((num4 > 0f) ? (((pos.x - val.x) * num2 + (pos.z - val.z) * num3) / num4) : 0f);
			float num6 = Mathf.Clamp01(num5);
			float num7 = val.x + num2 * num6 - pos.x;
			float num8 = val.z + num3 * num6 - pos.z;
			float num9 = num7 * num7 + num8 * num8;
			if (!(num9 >= num))
			{
				num = num9;
				segment = segment2;
				t = num5;
			}
		}
		return Mathf.Sqrt(num);
	}

	public static float Distance(List<List<Vector3>> paths, Vector3 pos)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		int segment;
		float t;
		return paths.Min((List<Vector3> path) => ClosestSegment(path, Enumerable.Range(0, path.Count - 1), pos, out segment, out t));
	}
}
