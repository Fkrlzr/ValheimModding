using System;
using System.Collections.Generic;
using System.Linq;
using ServerDevcommands;
using Service;
using UnityEngine;

namespace WorldEditCommands;

public class Terrain
{
	private static float CalculateSmooth(float smooth, float distance)
	{
		if (!(1f - distance >= smooth))
		{
			return (1f - distance) / smooth;
		}
		return 1f;
	}

	private static float CalculateSlope(float angle, float distanceWidth, float distanceDepth)
	{
		return Mathf.Sin(angle) * distanceWidth + Mathf.Cos(angle) * distanceDepth;
	}

	public static void SetTerrain(List<HeightNode> nodes, Vector3 pos, float radius, float smooth, float delta)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		DoOperation(nodes, pos, radius, action);
		void action(TerrainComp compiler, int index, TerrainNode node)
		{
			float num = CalculateSmooth(smooth, node.Distance);
			compiler.m_levelDelta[index] = delta * num;
			compiler.m_smoothDelta[index] = 0f;
			compiler.m_modifiedHeight[index] = compiler.m_levelDelta[index] != 0f;
		}
	}

	public static void RaiseTerrain(List<HeightNode> nodes, Vector3 pos, float radius, float smooth, float amount)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		DoOperation(nodes, pos, radius, action);
		void action(TerrainComp compiler, int index, TerrainNode node)
		{
			float num = CalculateSmooth(smooth, node.Distance);
			compiler.m_levelDelta[index] += num * amount + compiler.m_smoothDelta[index];
			compiler.m_smoothDelta[index] = 0f;
			compiler.m_modifiedHeight[index] = compiler.m_levelDelta[index] != 0f;
		}
	}

	public static void VoidTerrain(List<HeightNode> nodes, Vector3 pos, float radius)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		DoOperation(nodes, pos, radius, action);
		static void action(TerrainComp compiler, int index, TerrainNode node)
		{
			compiler.m_levelDelta[index] = float.NaN;
			compiler.m_smoothDelta[index] = 0f;
			compiler.m_modifiedHeight[index] = true;
		}
	}

	public static void LevelTerrain(List<HeightNode> nodes, Vector3 pos, float radius, float smooth, float y)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		DoOperation(nodes, pos, radius, action);
		void action(TerrainComp compiler, int index, TerrainNode node)
		{
			float num = CalculateSmooth(smooth, node.Distance);
			compiler.m_levelDelta[index] += num * (y - compiler.m_hmap.m_heights[index]);
			compiler.m_smoothDelta[index] = 0f;
			compiler.m_modifiedHeight[index] = compiler.m_levelDelta[index] != 0f;
		}
	}

	public static void LevelTerrainToPath(List<HeightNode> nodes, Vector3 pos, float radius, float smooth)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		DoOperation(nodes, pos, radius, action);
		void action(TerrainComp compiler, int index, TerrainNode node)
		{
			float num = CalculateSmooth(smooth, node.Distance);
			compiler.m_levelDelta[index] += num * (node.Height - compiler.m_hmap.m_heights[index]);
			compiler.m_smoothDelta[index] = 0f;
			compiler.m_modifiedHeight[index] = compiler.m_levelDelta[index] != 0f;
		}
	}

	public static void MaxTerrain(List<HeightNode> nodes, Vector3 pos, float radius, float y)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		DoOperation(nodes, pos, radius, action);
		void action(TerrainComp compiler, int index, TerrainNode node)
		{
			float num = Mathf.Min(y, compiler.m_hmap.m_heights[index]);
			compiler.m_levelDelta[index] += num - compiler.m_hmap.m_heights[index];
			compiler.m_smoothDelta[index] = 0f;
			compiler.m_modifiedHeight[index] = compiler.m_levelDelta[index] != 0f;
		}
	}

	public static void MinTerrain(List<HeightNode> nodes, Vector3 pos, float radius, float y)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		DoOperation(nodes, pos, radius, action);
		void action(TerrainComp compiler, int index, TerrainNode node)
		{
			float num = Mathf.Max(y, compiler.m_hmap.m_heights[index]);
			compiler.m_levelDelta[index] += num - compiler.m_hmap.m_heights[index];
			compiler.m_smoothDelta[index] = 0f;
			compiler.m_modifiedHeight[index] = compiler.m_levelDelta[index] != 0f;
		}
	}

	public static void SlopeTerrain(List<HeightNode> nodes, Vector3 pos, float radius, float angle, float smooth, float y, float amount)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		DoOperation(nodes, pos, radius, action);
		void action(TerrainComp compiler, int index, TerrainNode node)
		{
			float num = CalculateSlope(angle, node.DistanceWidth, node.DistanceDepth) * CalculateSmooth(smooth, node.Distance);
			compiler.m_levelDelta[index] += y - compiler.m_hmap.m_heights[index] + num * amount / 2f;
			compiler.m_smoothDelta[index] = 0f;
			compiler.m_modifiedHeight[index] = compiler.m_levelDelta[index] != 0f;
		}
	}

	public static void PaintTerrain(List<PaintNode> nodes, Vector3 pos, float radius, float smooth, Color color)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		DoOperation(nodes, pos, radius, action);
		void action(TerrainComp compiler, int index, TerrainNode node)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			float num = CalculateSmooth(smooth, node.Distance);
			Color val = compiler.m_paintMask[index];
			_ = compiler.m_hmap;
			bool flag = WorldGenerator.IsAshlands(pos.x, pos.z);
			if (flag && !compiler.m_modifiedPaint[index])
			{
				val.a = 0f;
			}
			Color val2 = color;
			if (float.IsNaN(color.r))
			{
				val2.r = val.r;
			}
			if (float.IsNaN(color.g))
			{
				val2.g = val.g;
			}
			if (float.IsNaN(color.b))
			{
				val2.b = val.b;
			}
			if (float.IsNaN(color.a))
			{
				val2.a = val.a;
			}
			else if (color.a >= 0f)
			{
				if (flag)
				{
					val2.a = 1f - color.a;
				}
				else
				{
					val2.a = color.a;
				}
			}
			else
			{
				val2.a = 0f - color.a;
			}
			compiler.m_paintMask[index] = Color.Lerp(val, val2, num);
			compiler.m_modifiedPaint[index] = true;
		}
	}

	public static void ClearPaint(List<PaintNode> nodes, Vector3 pos, float radius)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		DoOperation(nodes, pos, radius, action);
		static void action(TerrainComp compiler, int index, TerrainNode node)
		{
			compiler.m_modifiedPaint[index] = false;
		}
	}

	public static void ResetTerrain(List<HeightNode> heightNodes, List<PaintNode> paintNodes, Vector3 pos, float radius, Func<Vector3, bool>? filter = null)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		List<TerrainModifier> list = new List<TerrainModifier>();
		TerrainModifier.GetModifiers(pos, radius + 1f, list, (TerrainModifier)null);
		foreach (TerrainModifier item in list)
		{
			if (!((Object)(object)item.m_nview == (Object)null) && (filter == null || filter(((Component)item).transform.position)))
			{
				item.m_nview.ClaimOwnership();
				ZNetScene.instance.Destroy(((Component)item).gameObject);
			}
		}
		DoOperation(heightNodes, pos, radius, action);
		ClearPaint(paintNodes, pos, radius);
		static void action(TerrainComp compiler, int index, TerrainNode node)
		{
			compiler.m_levelDelta[index] = 0f;
			compiler.m_smoothDelta[index] = 0f;
			compiler.m_modifiedHeight[index] = false;
		}
	}

	private static void DoOperation(List<PaintNode> nodes, Vector3 pos, float radius, Action<TerrainComp, int, TerrainNode> action)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		foreach (PaintNode node in nodes)
		{
			action(node.Compiler, node.Index, node);
		}
		ClutterSystem instance = ClutterSystem.instance;
		if (instance != null)
		{
			instance.ResetGrass(pos, radius);
		}
	}

	private static void DoOperation(List<HeightNode> nodes, Vector3 pos, float radius, Action<TerrainComp, int, TerrainNode> action)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		foreach (HeightNode node in nodes)
		{
			action(node.Compiler, node.Index, node);
		}
		ClutterSystem instance = ClutterSystem.instance;
		if (instance != null)
		{
			instance.ResetGrass(pos, radius);
		}
	}

	public static Dictionary<Vector3, TerrainUndoData> GetData(List<HeightNode> heightNodes, List<PaintNode> paintNodes)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<Vector3, TerrainUndoData> dictionary = new Dictionary<Vector3, TerrainUndoData>();
		foreach (HeightNode heightNode in heightNodes)
		{
			TerrainComp compiler = heightNode.Compiler;
			if (!((Object)(object)compiler == (Object)null))
			{
				Vector3 position = ((Component)compiler).transform.position;
				if (!dictionary.ContainsKey(position))
				{
					dictionary.Add(position, new TerrainUndoData());
				}
				dictionary[position].Heights.Add(new HeightUndoData
				{
					Index = heightNode.Index,
					HeightModified = compiler.m_modifiedHeight[heightNode.Index],
					Level = compiler.m_levelDelta[heightNode.Index],
					Smooth = compiler.m_smoothDelta[heightNode.Index]
				});
			}
		}
		foreach (PaintNode paintNode in paintNodes)
		{
			TerrainComp compiler2 = paintNode.Compiler;
			if (!((Object)(object)compiler2 == (Object)null))
			{
				Vector3 position2 = ((Component)compiler2).transform.position;
				if (!dictionary.ContainsKey(position2))
				{
					dictionary.Add(position2, new TerrainUndoData());
				}
				dictionary[position2].Paints.Add(new PaintUndoData
				{
					Index = paintNode.Index,
					PaintModified = compiler2.m_modifiedPaint[paintNode.Index],
					Paint = compiler2.m_paintMask[paintNode.Index]
				});
			}
		}
		return dictionary;
	}

	public static void ApplyData(Dictionary<Vector3, TerrainUndoData> data, Vector3 pos, float radius)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		foreach (KeyValuePair<Vector3, TerrainUndoData> datum in data)
		{
			TerrainComp val = TerrainComp.FindTerrainCompiler(datum.Key);
			if (!Object.op_Implicit((Object)(object)val))
			{
				continue;
			}
			foreach (HeightUndoData height in datum.Value.Heights)
			{
				val.m_smoothDelta[height.Index] = height.Smooth;
				val.m_levelDelta[height.Index] = height.Level;
				val.m_modifiedHeight[height.Index] = height.HeightModified;
			}
			foreach (PaintUndoData paint in datum.Value.Paints)
			{
				val.m_modifiedPaint[paint.Index] = paint.PaintModified;
				val.m_paintMask[paint.Index] = paint.Paint;
			}
			Save(val);
		}
		ClutterSystem instance = ClutterSystem.instance;
		if (instance != null)
		{
			instance.ResetGrass(pos, radius);
		}
	}

	public static void Save(TerrainComp compiler)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		ZNetView component = ((Component)compiler).GetComponent<ZNetView>();
		if (component != null)
		{
			component.ClaimOwnership();
		}
		compiler.m_operations++;
		compiler.m_lastOpPoint = Vector3.zero;
		compiler.m_lastOpRadius = 0f;
		compiler.Save(false);
		compiler.m_hmap.Poke(0, false);
	}

	public static TerrainComp[] GetCompilers(Vector3 position, Range<float> radius)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		List<Heightmap> list = new List<Heightmap>();
		Heightmap.FindHeightmap(position, radius.Max + 1f, list);
		Vector3 pos = ZNet.instance.GetReferencePosition();
		_ = ZNetScene.instance;
		return (from hmap in list
			where ZNetScene.InActiveArea(((Component)hmap).transform.position, pos)
			select hmap.GetAndCreateTerrainCompiler()).ToArray();
	}

	public static TerrainComp[] GetCompilers(Vector3 position, Range<float> width, Range<float> depth, float angle)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		List<Heightmap> list = new List<Heightmap>();
		float num = Mathf.Max(width.Max, depth.Max);
		float num2 = Mathf.Abs(Mathf.Sin(angle)) + Mathf.Abs(Mathf.Cos(angle));
		float num3 = num * num2;
		Heightmap.FindHeightmap(position, num3 + 1f, list);
		Vector3 pos = ZNet.instance.GetReferencePosition();
		_ = ZNetScene.instance;
		return (from hmap in list
			where ZNetScene.InActiveArea(((Component)hmap).transform.position, pos)
			select hmap.GetAndCreateTerrainCompiler()).ToArray();
	}

	public static Func<TerrainNode, bool> CreateBlockCheckFilter(BlockCheck blockCheck, string[] includedIds, string[] excludedIds)
	{
		HashSet<int> included = Selector.GetPrefabs(includedIds);
		HashSet<int> excluded = Selector.GetAllPrefabs(excludedIds);
		ZoneSystem zs = ZoneSystem.instance;
		return delegate(TerrainNode index)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (blockCheck == BlockCheck.Off)
			{
				return true;
			}
			Vector3 position = index.Position;
			position.y += 2000f;
			bool flag = ((IEnumerable<RaycastHit>)Physics.RaycastAll(position, Vector3.down, 10000f, zs.m_blockRayMask)).Select((Func<RaycastHit, int>)Selector.GetPrefabFromHit).Any((int prefab) => included.Contains(prefab) && !excluded.Contains(prefab));
			if (flag && blockCheck == BlockCheck.On)
			{
				return false;
			}
			return (flag || blockCheck != BlockCheck.Inverse) ? true : false;
		};
	}

	public static Func<TerrainNode, bool> CreateHeightFilter(float min, float max)
	{
		return delegate(TerrainNode index)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			float groundHeight = ZoneSystem.instance.GetGroundHeight(index.Position);
			return groundHeight >= min && groundHeight <= max;
		};
	}

	public static void GetHeightNodesWithCircle(List<HeightNode> nodes, TerrainComp compiler, Vector3 centerPos, Range<float> radius)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		if (radius.Max == 0f)
		{
			return;
		}
		int num = compiler.m_width + 1;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				Vector3 val = VertexToWorld(compiler.m_hmap, i, j);
				float num2 = val.x - centerPos.x;
				float num3 = val.z - centerPos.z;
				float num4 = Utils.DistanceXZ(centerPos, val);
				if (Helper.Within(radius, num4))
				{
					nodes.Add(new HeightNode
					{
						Index = j * num + i,
						Position = val,
						DistanceWidth = num2 / radius.Max,
						DistanceDepth = num3 / radius.Max,
						Distance = num4 / radius.Max,
						Compiler = compiler
					});
				}
			}
		}
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

	public static void GetHeightNodesWithRect(List<HeightNode> nodes, TerrainComp compiler, Vector3 centerPos, Range<float> width, Range<float> depth, float angle)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		if (width.Max == 0f || depth.Max == 0f)
		{
			return;
		}
		int num = compiler.m_width + 1;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				Vector3 val = VertexToWorld(compiler.m_hmap, i, j);
				float x = val.x - centerPos.x;
				float z = val.z - centerPos.z;
				float x2 = GetX(x, z, angle);
				float z2 = GetZ(x, z, angle);
				if (Helper.Within(width, depth, Mathf.Abs(x2), Mathf.Abs(z2)))
				{
					float num2 = x2 / width.Max;
					float num3 = z2 / depth.Max;
					nodes.Add(new HeightNode
					{
						Index = j * num + i,
						Position = val,
						DistanceWidth = num2,
						DistanceDepth = num3,
						Distance = Mathf.Max(Mathf.Abs(num2), Mathf.Abs(num3)),
						Compiler = compiler
					});
				}
			}
		}
	}

	public static void GetPaintNodesWithRect(List<PaintNode> nodes, TerrainComp compiler, Vector3 centerPos, Range<float> width, Range<float> depth, float angle)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		int num = compiler.m_width + 1;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				Vector3 val = VertexToWorld(compiler.m_hmap, i, j);
				float x = val.x - centerPos.x;
				float z = val.z - centerPos.z;
				float x2 = GetX(x, z, angle);
				float z2 = GetZ(x, z, angle);
				if (Helper.Within(width, depth, Mathf.Abs(x2), Mathf.Abs(z2)))
				{
					float num2 = x2 / width.Max;
					float num3 = z2 / depth.Max;
					nodes.Add(new PaintNode
					{
						Index = j * num + i,
						Position = val,
						DistanceWidth = num2,
						DistanceDepth = num3,
						Distance = Mathf.Max(Mathf.Abs(num2), Mathf.Abs(num3)),
						Compiler = compiler
					});
				}
			}
		}
	}

	public static void GetPaintNodesWithCircle(List<PaintNode> nodes, TerrainComp compiler, Vector3 centerPos, Range<float> radius)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		int num = compiler.m_width + 1;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				Vector3 val = VertexToWorld(compiler.m_hmap, i, j);
				float num2 = val.x - centerPos.x;
				float num3 = val.z - centerPos.z;
				float num4 = Utils.DistanceXZ(centerPos, val);
				if (Helper.Within(radius, num4))
				{
					nodes.Add(new PaintNode
					{
						Index = j * num + i,
						Position = val,
						DistanceWidth = num2 / radius.Max,
						DistanceDepth = num3 / radius.Max,
						Distance = num4 / radius.Max,
						Compiler = compiler
					});
				}
			}
		}
	}

	public static TerrainComp[] GetCompilers(List<List<Vector3>> paths, float radius)
	{
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		HashSet<Heightmap> hashSet = new HashSet<Heightmap>();
		List<Heightmap> list = new List<Heightmap>();
		foreach (List<Vector3> path in paths)
		{
			for (int i = 0; i < path.Count; i++)
			{
				int num = ((i >= path.Count - 1) ? 1 : Mathf.Max(1, Mathf.CeilToInt(Utils.DistanceXZ(path[i], path[i + 1]) / 16f)));
				for (int j = 0; j < num; j++)
				{
					Vector3 val = ((i < path.Count - 1) ? Vector3.Lerp(path[i], path[i + 1], (float)j / (float)num) : path[i]);
					list.Clear();
					Heightmap.FindHeightmap(val, radius + 1f, list);
					hashSet.UnionWith(list);
				}
			}
		}
		Vector3 pos = ZNet.instance.GetReferencePosition();
		return (from hmap in hashSet
			where ZNetScene.InActiveArea(((Component)hmap).transform.position, pos)
			select hmap.GetAndCreateTerrainCompiler()).ToArray();
	}

	public static void GetNodesWithPath<T>(List<T> nodes, TerrainComp compiler, List<List<Vector3>> paths, Range<float> radius, bool flatEnds) where T : TerrainNode, new()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		if (radius.Max == 0f)
		{
			return;
		}
		Heightmap hmap = compiler.m_hmap;
		Vector3 center = ((Component)hmap).transform.position;
		float half = (float)hmap.m_width * hmap.m_scale / 2f + radius.Max;
		List<List<int>> list = paths.Select(delegate(List<Vector3> path)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			List<int> list3 = new List<int>();
			for (int l = 0; l < path.Count - 1; l++)
			{
				Vector3 val2 = path[l];
				Vector3 val3 = path[l + 1];
				if (!(Mathf.Max(val2.x, val3.x) < center.x - half) && !(Mathf.Min(val2.x, val3.x) > center.x + half) && !(Mathf.Max(val2.z, val3.z) < center.z - half) && !(Mathf.Min(val2.z, val3.z) > center.z + half))
				{
					list3.Add(l);
				}
			}
			return list3;
		}).ToList();
		if (list.All((List<int> s) => s.Count == 0))
		{
			return;
		}
		int num = compiler.m_width + 1;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				Vector3 val = VertexToWorld(hmap, i, j);
				float num2 = float.MaxValue;
				float height = 0f;
				for (int k = 0; k < paths.Count; k++)
				{
					if (list[k].Count != 0)
					{
						List<Vector3> list2 = paths[k];
						int segment;
						float t;
						float num3 = TerrainPath.ClosestSegment(list2, list[k], val, out segment, out t);
						if (!(num3 >= num2) && (!flatEnds || ((segment != 0 || !(t < 0f)) && (segment != list2.Count - 2 || !(t > 1f)))))
						{
							num2 = num3;
							height = Mathf.Lerp(list2[segment].y, list2[segment + 1].y, Mathf.Clamp01(t));
						}
					}
				}
				if (!(num2 > radius.Max) && Helper.Within(radius, num2))
				{
					nodes.Add(new T
					{
						Index = j * num + i,
						Position = val,
						Distance = num2 / radius.Max,
						Height = height,
						Compiler = compiler
					});
				}
			}
		}
	}
}
