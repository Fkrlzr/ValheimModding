using UnityEngine;

namespace Service;

public class TerrainHeight : TerrainChannelData<float>
{
	public float?[,] Heights = new float?[0, 0];

	public void InitializeGrid(int width, int height, Vector3 centerPosition)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		InitializeReference(width, height, centerPosition);
		Heights = new float?[width, height];
	}

	public override float? Get(int x, int z)
	{
		if (x >= 0 && x < Width && z >= 0 && z < Height)
		{
			return Heights[x, z];
		}
		return null;
	}

	public override void Set(int x, int z, float value)
	{
		if (x >= 0 && x < Width && z >= 0 && z < Height)
		{
			Heights[x, z] = value;
		}
	}

	public override float? FindNearest(Vector3 nodePos, Vector3 placementPos, Quaternion relativeRotation)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		if (!TerrainChannelData<float>.TryGetGridCoordinates(nodePos, placementPos, relativeRotation, GetFirstNodeAnchor(), DistanceBetweenNodes, out var gridX, out var gridZ))
		{
			return null;
		}
		if (gridX < 0 || gridX >= Width || gridZ < 0 || gridZ >= Height)
		{
			return null;
		}
		return Get(gridX, gridZ);
	}

	protected override void OnSetReference(Vector3 center)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
				if (Heights[i, j].HasValue)
				{
					Heights[i, j] -= center.y;
				}
			}
		}
	}

	public TerrainHeight Clone()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		TerrainHeight terrainHeight = new TerrainHeight
		{
			CenterPosition = CenterPosition,
			CenterRotation = CenterRotation,
			DistanceBetweenNodes = DistanceBetweenNodes,
			Smooth = Smooth,
			Width = Width,
			Height = Height,
			Heights = new float?[Width, Height]
		};
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
				terrainHeight.Heights[i, j] = Heights[i, j];
			}
		}
		return terrainHeight;
	}
}
