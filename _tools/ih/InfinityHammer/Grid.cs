using UnityEngine;

namespace InfinityHammer;

public static class Grid
{
	private static float Precision;

	private static Vector3 Center;

	private static Vector3 PreciseCenter;

	public static bool Enabled
	{
		get
		{
			if (Precision == 0f)
			{
				return Selection.Get().TerrainGrid;
			}
			return true;
		}
	}

	public static Vector3 Apply(Vector3 point, Vector3 normal)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		float num = Precision;
		Vector3 val = Center;
		if (num == 0f && Selection.Get().TerrainGrid)
		{
			num = 1f;
			val = PreciseCenter;
		}
		if (num == 0f)
		{
			return point;
		}
		Quaternion val2 = Quaternion.FromToRotation(Vector3.up, normal);
		point = val2 * point;
		Vector3 val3 = val2 * val;
		point.x = val3.x + Mathf.Round((point.x - val3.x) / num) * num;
		point.z = val3.z + Mathf.Round((point.z - val3.z) / num) * num;
		return Quaternion.Inverse(val2) * point;
	}

	public static void Toggle(float precision, Vector3 center)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (Precision == precision)
		{
			Precision = 0f;
			return;
		}
		Center = center;
		Precision = precision;
	}

	public static void SetPreciseMode(Vector3 center)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PreciseCenter = center;
	}
}
