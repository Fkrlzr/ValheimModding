using System.Collections.Generic;
using UnityEngine;

namespace WorldEditCommands;

public class UndoTerrain
{
	private readonly Dictionary<Vector3, TerrainUndoData> Before;

	private readonly Dictionary<Vector3, TerrainUndoData> After;

	public Vector3 Position;

	public float Radius;

	public UndoTerrain(Dictionary<Vector3, TerrainUndoData> before, Dictionary<Vector3, TerrainUndoData> after, Vector3 position, float radius)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		Before = before;
		After = after;
		Position = position;
		Radius = radius;
		base._002Ector();
	}

	public void Undo()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		Terrain.ApplyData(Before, Position, Radius);
	}

	public void Redo()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		Terrain.ApplyData(After, Position, Radius);
	}
}
