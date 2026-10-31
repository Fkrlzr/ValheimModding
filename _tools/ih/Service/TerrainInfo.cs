using System;
using System.Collections.Generic;
using System.Linq;
using ServerDevcommands;
using UnityEngine;
using WorldEditCommands;

namespace Service;

public class TerrainInfo
{
	public static float ResolveRadius(TerrainHeight? height, TerrainPaint? paint)
	{
		float num = Mathf.Max(height?.GetRadius() ?? 0f, paint?.GetRadius() ?? 0f);
		float num2 = Mathf.Max(new float[3]
		{
			1f,
			height?.DistanceBetweenNodes ?? 0f,
			paint?.DistanceBetweenNodes ?? 0f
		});
		return num + num2;
	}

	private static Vector3 VertexToWorld(Heightmap hmap, int x, int z)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Component)hmap).transform.position;
		position.x += (float)(x - hmap.m_width / 2) * hmap.m_scale;
		position.z += (float)(z - hmap.m_width / 2) * hmap.m_scale;
		return position;
	}

	private static float GetX(float x, float z, float angle)
	{
		return Mathf.Cos(angle) * x - Mathf.Sin(angle) * z;
	}

	private static float GetZ(float x, float z, float angle)
	{
		return Mathf.Sin(angle) * x + Mathf.Cos(angle) * z;
	}

	public static TerrainHeight CollectTerrainHeightInRadius(Vector3 centerPos, Quaternion centerRot, Vector3 searchPos, Range<float> radius, float nodeSpacingOverride = 0f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		TerrainHeight terrainHeight = MergeHeightmapsWithCircle(Terrain.GetCompilers(searchPos, new Range<float>(radius.Max)).ToList(), searchPos, radius, nodeSpacingOverride);
		terrainHeight.SetReference(centerPos, centerRot);
		return terrainHeight;
	}

	public static TerrainPaint CollectTerrainPaintInRadius(Vector3 centerPos, Quaternion centerRot, Vector3 searchPos, Range<float> radius, float nodeSpacingOverride = 0f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		TerrainPaint terrainPaint = MergePaintmapsWithCircle(Terrain.GetCompilers(searchPos, new Range<float>(radius.Max)).ToList(), searchPos, radius, nodeSpacingOverride);
		terrainPaint.SetReference(centerPos, centerRot);
		return terrainPaint;
	}

	public static TerrainHeight CollectTerrainHeightInRect(Vector3 centerPos, Quaternion centerRot, Vector3 searchPos, Range<float> width, Range<float> depth, float angle, float nodeSpacingOverride = 0f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		TerrainHeight terrainHeight = MergeHeightmapsWithRect(Terrain.GetCompilers(searchPos, width, depth, angle).ToList(), searchPos, width, depth, angle, nodeSpacingOverride);
		terrainHeight.SetReference(centerPos, centerRot);
		return terrainHeight;
	}

	public static TerrainPaint CollectTerrainPaintInRect(Vector3 centerPos, Quaternion centerRot, Vector3 searchPos, Range<float> width, Range<float> depth, float angle, float nodeSpacingOverride = 0f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		TerrainPaint terrainPaint = MergePaintmapsWithRect(Terrain.GetCompilers(searchPos, width, depth, angle).ToList(), searchPos, width, depth, angle, nodeSpacingOverride);
		terrainPaint.SetReference(centerPos, centerRot);
		return terrainPaint;
	}

	private static float ResolveNodeSpacing(List<TerrainComp> compilers, float nodeSpacingOverride)
	{
		if (nodeSpacingOverride > 0f)
		{
			return Mathf.Max(0.001f, nodeSpacingOverride);
		}
		if (compilers.Count == 0)
		{
			return 1f;
		}
		return Mathf.Max(0.001f, compilers[0].m_hmap.m_scale);
	}

	private static TerrainHeight MergeHeightmapsWithCircle(List<TerrainComp> compilers, Vector3 centerPos, Range<float> radius, float nodeSpacingOverride)
	{
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		TerrainHeight terrainHeight = new TerrainHeight();
		if (compilers.Count == 0)
		{
			return terrainHeight;
		}
		float num = ResolveNodeSpacing(compilers, nodeSpacingOverride);
		float num2 = float.MaxValue;
		float num3 = float.MinValue;
		float num4 = float.MaxValue;
		float num5 = float.MinValue;
		bool flag = false;
		foreach (TerrainComp compiler in compilers)
		{
			int num6 = compiler.m_width + 1;
			for (int i = 0; i < num6; i++)
			{
				for (int j = 0; j < num6; j++)
				{
					Vector3 val = VertexToWorld(compiler.m_hmap, i, j);
					float num7 = Utils.DistanceXZ(centerPos, val);
					if (Helper.Within(radius, num7))
					{
						flag = true;
						num2 = Math.Min(num2, val.x);
						num3 = Math.Max(num3, val.x);
						num4 = Math.Min(num4, val.z);
						num5 = Math.Max(num5, val.z);
					}
				}
			}
		}
		if (!flag)
		{
			return terrainHeight;
		}
		int num8 = Mathf.RoundToInt((num3 - num2) / num) + 1;
		int num9 = Mathf.RoundToInt((num5 - num4) / num) + 1;
		terrainHeight.InitializeGrid(num8, num9, new Vector3(num2 + (float)(num8 - 1) * num * 0.5f, 0f, num4 + (float)(num9 - 1) * num * 0.5f));
		terrainHeight.DistanceBetweenNodes = num;
		foreach (TerrainComp compiler2 in compilers)
		{
			int num10 = compiler2.m_width + 1;
			for (int k = 0; k < num10; k++)
			{
				for (int l = 0; l < num10; l++)
				{
					Vector3 val2 = VertexToWorld(compiler2.m_hmap, k, l);
					float num11 = Utils.DistanceXZ(centerPos, val2);
					if (!Helper.Within(radius, num11))
					{
						continue;
					}
					int num12 = l * num10 + k;
					if (num12 < compiler2.m_hmap.m_heights.Count)
					{
						int x = Mathf.RoundToInt((val2.x - num2) / num);
						int z = Mathf.RoundToInt((val2.z - num4) / num);
						if (!terrainHeight.Get(x, z).HasValue)
						{
							terrainHeight.Set(x, z, compiler2.m_hmap.m_heights[num12]);
						}
					}
				}
			}
		}
		return terrainHeight;
	}

	private static TerrainPaint MergePaintmapsWithCircle(List<TerrainComp> compilers, Vector3 centerPos, Range<float> radius, float nodeSpacingOverride)
	{
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		TerrainPaint terrainPaint = new TerrainPaint();
		if (compilers.Count == 0)
		{
			return terrainPaint;
		}
		float num = ResolveNodeSpacing(compilers, nodeSpacingOverride);
		float num2 = float.MaxValue;
		float num3 = float.MinValue;
		float num4 = float.MaxValue;
		float num5 = float.MinValue;
		bool flag = false;
		foreach (TerrainComp compiler in compilers)
		{
			int num6 = compiler.m_width + 1;
			for (int i = 0; i < num6; i++)
			{
				for (int j = 0; j < num6; j++)
				{
					Vector3 val = VertexToWorld(compiler.m_hmap, i, j);
					float num7 = Utils.DistanceXZ(centerPos, val);
					if (Helper.Within(radius, num7))
					{
						flag = true;
						num2 = Math.Min(num2, val.x);
						num3 = Math.Max(num3, val.x);
						num4 = Math.Min(num4, val.z);
						num5 = Math.Max(num5, val.z);
					}
				}
			}
		}
		if (!flag)
		{
			return terrainPaint;
		}
		int num8 = Mathf.RoundToInt((num3 - num2) / num) + 1;
		int num9 = Mathf.RoundToInt((num5 - num4) / num) + 1;
		terrainPaint.InitializeGrid(num8, num9, new Vector3(num2 + (float)(num8 - 1) * num * 0.5f, 0f, num4 + (float)(num9 - 1) * num * 0.5f));
		terrainPaint.DistanceBetweenNodes = num;
		foreach (TerrainComp compiler2 in compilers)
		{
			int num10 = compiler2.m_width + 1;
			for (int k = 0; k < num10; k++)
			{
				for (int l = 0; l < num10; l++)
				{
					Vector3 val2 = VertexToWorld(compiler2.m_hmap, k, l);
					float num11 = Utils.DistanceXZ(centerPos, val2);
					if (Helper.Within(radius, num11))
					{
						int x = Mathf.RoundToInt((val2.x - num2) / num);
						int z = Mathf.RoundToInt((val2.z - num4) / num);
						if (!terrainPaint.Get(x, z).HasValue)
						{
							terrainPaint.Set(x, z, compiler2.m_hmap.GetPaintMask(k, l));
						}
					}
				}
			}
		}
		return terrainPaint;
	}

	private static TerrainHeight MergeHeightmapsWithRect(List<TerrainComp> compilers, Vector3 centerPos, Range<float> width, Range<float> depth, float angle, float nodeSpacingOverride)
	{
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		TerrainHeight terrainHeight = new TerrainHeight();
		if (compilers.Count == 0)
		{
			return terrainHeight;
		}
		float num = ResolveNodeSpacing(compilers, nodeSpacingOverride);
		float num2 = float.MaxValue;
		float num3 = float.MinValue;
		float num4 = float.MaxValue;
		float num5 = float.MinValue;
		bool flag = false;
		foreach (TerrainComp compiler in compilers)
		{
			int num6 = compiler.m_width + 1;
			for (int i = 0; i < num6; i++)
			{
				for (int j = 0; j < num6; j++)
				{
					Vector3 val = VertexToWorld(compiler.m_hmap, i, j);
					float x = val.x - centerPos.x;
					float z = val.z - centerPos.z;
					float x2 = GetX(x, z, angle);
					float z2 = GetZ(x, z, angle);
					if (Helper.Within(width, depth, Mathf.Abs(x2), Mathf.Abs(z2)))
					{
						flag = true;
						num2 = Math.Min(num2, val.x);
						num3 = Math.Max(num3, val.x);
						num4 = Math.Min(num4, val.z);
						num5 = Math.Max(num5, val.z);
					}
				}
			}
		}
		if (!flag)
		{
			return terrainHeight;
		}
		int num7 = Mathf.RoundToInt((num3 - num2) / num) + 1;
		int num8 = Mathf.RoundToInt((num5 - num4) / num) + 1;
		terrainHeight.InitializeGrid(num7, num8, new Vector3(num2 + (float)(num7 - 1) * num * 0.5f, 0f, num4 + (float)(num8 - 1) * num * 0.5f));
		terrainHeight.DistanceBetweenNodes = num;
		foreach (TerrainComp compiler2 in compilers)
		{
			int num9 = compiler2.m_width + 1;
			for (int k = 0; k < num9; k++)
			{
				for (int l = 0; l < num9; l++)
				{
					Vector3 val2 = VertexToWorld(compiler2.m_hmap, k, l);
					float x3 = val2.x - centerPos.x;
					float z3 = val2.z - centerPos.z;
					float x4 = GetX(x3, z3, angle);
					float z4 = GetZ(x3, z3, angle);
					if (!Helper.Within(width, depth, Mathf.Abs(x4), Mathf.Abs(z4)))
					{
						continue;
					}
					int num10 = l * num9 + k;
					if (num10 < compiler2.m_hmap.m_heights.Count)
					{
						int x5 = Mathf.RoundToInt((val2.x - num2) / num);
						int z5 = Mathf.RoundToInt((val2.z - num4) / num);
						if (!terrainHeight.Get(x5, z5).HasValue)
						{
							terrainHeight.Set(x5, z5, compiler2.m_hmap.m_heights[num10]);
						}
					}
				}
			}
		}
		return terrainHeight;
	}

	private static TerrainPaint MergePaintmapsWithRect(List<TerrainComp> compilers, Vector3 centerPos, Range<float> width, Range<float> depth, float angle, float nodeSpacingOverride)
	{
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		TerrainPaint terrainPaint = new TerrainPaint();
		if (compilers.Count == 0)
		{
			return terrainPaint;
		}
		float num = ResolveNodeSpacing(compilers, nodeSpacingOverride);
		float num2 = float.MaxValue;
		float num3 = float.MinValue;
		float num4 = float.MaxValue;
		float num5 = float.MinValue;
		bool flag = false;
		foreach (TerrainComp compiler in compilers)
		{
			int num6 = compiler.m_width + 1;
			for (int i = 0; i < num6; i++)
			{
				for (int j = 0; j < num6; j++)
				{
					Vector3 val = VertexToWorld(compiler.m_hmap, i, j);
					float x = val.x - centerPos.x;
					float z = val.z - centerPos.z;
					float x2 = GetX(x, z, angle);
					float z2 = GetZ(x, z, angle);
					if (Helper.Within(width, depth, Mathf.Abs(x2), Mathf.Abs(z2)))
					{
						flag = true;
						num2 = Math.Min(num2, val.x);
						num3 = Math.Max(num3, val.x);
						num4 = Math.Min(num4, val.z);
						num5 = Math.Max(num5, val.z);
					}
				}
			}
		}
		if (!flag)
		{
			return terrainPaint;
		}
		int num7 = Mathf.RoundToInt((num3 - num2) / num) + 1;
		int num8 = Mathf.RoundToInt((num5 - num4) / num) + 1;
		terrainPaint.InitializeGrid(num7, num8, new Vector3(num2 + (float)(num7 - 1) * num * 0.5f, 0f, num4 + (float)(num8 - 1) * num * 0.5f));
		terrainPaint.DistanceBetweenNodes = num;
		foreach (TerrainComp compiler2 in compilers)
		{
			int num9 = compiler2.m_width + 1;
			for (int k = 0; k < num9; k++)
			{
				for (int l = 0; l < num9; l++)
				{
					Vector3 val2 = VertexToWorld(compiler2.m_hmap, k, l);
					float x3 = val2.x - centerPos.x;
					float z3 = val2.z - centerPos.z;
					float x4 = GetX(x3, z3, angle);
					float z4 = GetZ(x3, z3, angle);
					if (Helper.Within(width, depth, Mathf.Abs(x4), Mathf.Abs(z4)))
					{
						int x5 = Mathf.RoundToInt((val2.x - num2) / num);
						int z5 = Mathf.RoundToInt((val2.z - num4) / num);
						if (!terrainPaint.Get(x5, z5).HasValue)
						{
							terrainPaint.Set(x5, z5, compiler2.m_hmap.GetPaintMask(k, l));
						}
					}
				}
			}
		}
		return terrainPaint;
	}
}
