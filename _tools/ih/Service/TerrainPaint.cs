using UnityEngine;

namespace Service;

public class TerrainPaint : TerrainChannelData<Color>
{
	public Color?[,] Paints = new Color?[0, 0];

	public void InitializeGrid(int width, int height, Vector3 centerPosition)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		InitializeReference(width, height, centerPosition);
		Paints = new Color?[width, height];
	}

	public override Color? Get(int x, int z)
	{
		if (x >= 0 && x < Width && z >= 0 && z < Height)
		{
			return Paints[x, z];
		}
		return null;
	}

	public override void Set(int x, int z, Color value)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (x >= 0 && x < Width && z >= 0 && z < Height)
		{
			Paints[x, z] = value;
		}
	}

	public override Color? FindNearest(Vector3 nodePos, Vector3 placementPos, Quaternion relativeRotation)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		if (!TerrainChannelData<Color>.TryGetGridCoordinates(nodePos, placementPos, relativeRotation, GetFirstNodeAnchor(), DistanceBetweenNodes, out var gridX, out var gridZ))
		{
			return null;
		}
		if (gridX < 0 || gridX >= Width || gridZ < 0 || gridZ >= Height)
		{
			return null;
		}
		return Get(gridX, gridZ);
	}

	public TerrainPaint Clone()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		TerrainPaint terrainPaint = new TerrainPaint
		{
			CenterPosition = CenterPosition,
			CenterRotation = CenterRotation,
			DistanceBetweenNodes = DistanceBetweenNodes,
			Smooth = Smooth,
			Width = Width,
			Height = Height,
			Paints = new Color?[Width, Height]
		};
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
				terrainPaint.Paints[i, j] = Paints[i, j];
			}
		}
		return terrainPaint;
	}
}
