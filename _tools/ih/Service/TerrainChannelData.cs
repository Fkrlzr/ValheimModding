using UnityEngine;

namespace Service;

public abstract class TerrainChannelData<TValue> where TValue : struct
{
	public Vector3 CenterPosition;

	public Quaternion CenterRotation = Quaternion.identity;

	public float DistanceBetweenNodes = 1f;

	public float Smooth;

	public int Width;

	public int Height;

	public float GetRadius()
	{
		return GetRequiredRadius();
	}

	public float GetSmoothWeight(Vector3 nodePos, Vector3 placementPos, Quaternion relativeRotation, float smooth)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		smooth = Mathf.Clamp01(smooth);
		if (smooth <= 0f)
		{
			return 1f;
		}
		Vector3 val = Quaternion.Inverse(relativeRotation) * (nodePos - placementPos);
		float num = val.x - CenterPosition.x;
		float num2 = val.z - CenterPosition.z;
		float requiredRadius = GetRequiredRadius();
		if (requiredRadius <= 0f)
		{
			return 1f;
		}
		float num3 = Mathf.Sqrt(num * num + num2 * num2);
		float num4 = requiredRadius * (1f - smooth);
		if (num3 <= num4)
		{
			return 1f;
		}
		if (num3 >= requiredRadius)
		{
			return 0f;
		}
		float num5 = Mathf.Max(0.001f, requiredRadius - num4);
		return Mathf.Clamp01(1f - (num3 - num4) / num5);
	}

	protected void InitializeReference(int width, int height, Vector3 centerPos)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		Width = width;
		Height = height;
		CenterPosition = centerPos;
	}

	private float GetRequiredRadius()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		if (Width <= 0 || Height <= 0)
		{
			return 0f;
		}
		float num = Mathf.Max(0.001f, DistanceBetweenNodes);
		float num2 = (float)(Width - 1) * num * 0.5f;
		float num3 = (float)(Height - 1) * num * 0.5f;
		Vector3 val = CenterPosition - new Vector3(num2, 0f, num3);
		float num4 = 0f;
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
				if (Get(i, j).HasValue)
				{
					Vector3 val2 = val + new Vector3((float)i * num, 0f, (float)j * num);
					num4 = Mathf.Max(num4, Utils.LengthXZ(val2));
				}
			}
		}
		return num4;
	}

	public void SetReference(Vector3 center, Quaternion rotation)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		CenterPosition -= center;
		CenterRotation = GetYawRotation(rotation);
		OnSetReference(center);
	}

	protected static Quaternion GetYawRotation(Quaternion rotation)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = rotation * Vector3.forward;
		val.y = 0f;
		if (((Vector3)(ref val)).sqrMagnitude <= 0.0001f)
		{
			return Quaternion.identity;
		}
		return Quaternion.LookRotation(((Vector3)(ref val)).normalized, Vector3.up);
	}

	protected virtual void OnSetReference(Vector3 center)
	{
	}

	protected Vector3 GetFirstNodeAnchor()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.Max(0.001f, DistanceBetweenNodes);
		float num2 = (float)(Width - 1) * num * 0.5f;
		float num3 = (float)(Height - 1) * num * 0.5f;
		return new Vector3(CenterPosition.x - num2, CenterPosition.y, CenterPosition.z - num3);
	}

	protected static bool TryGetGridCoordinates(Vector3 nodePos, Vector3 placementPos, Quaternion relativeRotation, Vector3 firstNodeAnchor, float distanceBetweenNodes, out int gridX, out int gridZ)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.Max(0.001f, distanceBetweenNodes);
		Vector3 val = nodePos - placementPos;
		Vector3 val2 = Quaternion.Inverse(relativeRotation) * val;
		float x = val2.x;
		float z = val2.z;
		float num2 = x - firstNodeAnchor.x;
		float num3 = z - firstNodeAnchor.z;
		gridX = Mathf.RoundToInt(num2 / num);
		gridZ = Mathf.RoundToInt(num3 / num);
		return true;
	}

	public abstract TValue? Get(int x, int z);

	public abstract void Set(int x, int z, TValue value);

	public abstract TValue? FindNearest(Vector3 nodePos, Vector3 placementPos, Quaternion relativeRotation);
}
