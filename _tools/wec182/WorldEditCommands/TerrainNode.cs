using UnityEngine;

namespace WorldEditCommands;

public abstract class TerrainNode
{
	public int Index;

	public Vector3 Position;

	public float DistanceWidth;

	public float DistanceDepth;

	public float Distance;

	public float Height;

	public TerrainComp? Compiler;
}
