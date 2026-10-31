using System;
using System.Collections.Generic;
using System.Linq;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class TerrainCommand
{
	public const string Name = "terrain";

	public static Dictionary<string, Color> Paints = new Dictionary<string, Color>
	{
		{
			"grass",
			Color.black
		},
		{
			"patches",
			new Color(0f, 0.75f, 0f)
		},
		{
			"grass_dark",
			new Color(0.6f, 0.5f, 0f)
		},
		{
			"dirt",
			Color.red
		},
		{
			"cultivated",
			Color.green
		},
		{
			"paved",
			Color.blue
		},
		{
			"paved_moss",
			new Color(0f, 0f, 0.5f)
		},
		{
			"paved_dirt",
			new Color(1f, 0f, 0.5f)
		},
		{
			"paved_dark",
			new Color(0f, 1f, 0.5f)
		},
		{
			"lava",
			new Color(float.NaN, float.NaN, float.NaN, -1f)
		},
		{
			"lava_shallow",
			new Color(float.NaN, float.NaN, float.NaN, -0.5f)
		},
		{
			"lava_light",
			new Color(float.NaN, float.NaN, float.NaN, -0.25f)
		}
	};

	private TerrainComp[] GetCompilers(TerrainParameters pars)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		if (pars.Paths != null)
		{
			return Terrain.GetCompilers(pars.Paths, pars.PathRadius.Max);
		}
		if (pars.Radius != null)
		{
			return Terrain.GetCompilers(pars.Position, pars.Radius);
		}
		if (pars.Width != null && pars.Depth != null)
		{
			return Terrain.GetCompilers(pars.Position, pars.Width, pars.Depth, pars.Angle);
		}
		throw new InvalidOperationException("Unable to select any terrain");
	}

	private List<HeightNode> GetHeightNodes(TerrainParameters pars, IEnumerable<TerrainComp> compilers)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		TerrainParameters pars2 = pars;
		List<HeightNode> list = new List<HeightNode>();
		foreach (TerrainComp compiler in compilers)
		{
			if (pars2.Paths != null)
			{
				Terrain.GetNodesWithPath(list, compiler, pars2.Paths, pars2.PathRadius, pars2.Radius == null);
			}
			else if (pars2.Radius != null)
			{
				Terrain.GetHeightNodesWithCircle(list, compiler, pars2.Position, pars2.Radius);
			}
			else if (pars2.Width != null && pars2.Depth != null)
			{
				Terrain.GetHeightNodesWithRect(list, compiler, pars2.Position, pars2.Width, pars2.Depth, pars2.Angle);
			}
		}
		if (pars2.Chance < 1f)
		{
			list = list.Where((HeightNode n) => Random.value < pars2.Chance).ToList();
		}
		return list;
	}

	private List<PaintNode> GetPaintNodes(TerrainParameters pars, IEnumerable<TerrainComp> compilers)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		TerrainParameters pars2 = pars;
		List<PaintNode> list = new List<PaintNode>();
		foreach (TerrainComp compiler in compilers)
		{
			if (pars2.Paths != null)
			{
				Terrain.GetNodesWithPath(list, compiler, pars2.Paths, pars2.PathRadius, pars2.Radius == null);
			}
			else if (pars2.Radius != null)
			{
				Terrain.GetPaintNodesWithCircle(list, compiler, pars2.Position, pars2.Radius);
			}
			else if (pars2.Width != null && pars2.Depth != null)
			{
				Terrain.GetPaintNodesWithRect(list, compiler, pars2.Position, pars2.Width, pars2.Depth, pars2.Angle);
			}
		}
		if (pars2.Chance < 1f)
		{
			list = list.Where((PaintNode n) => Random.value < pars2.Chance).ToList();
		}
		return list;
	}

	private List<Func<TerrainNode, bool>> GetFilterers(TerrainParameters pars)
	{
		List<Func<TerrainNode, bool>> list = new List<Func<TerrainNode, bool>>();
		if (pars.BlockCheck != 0)
		{
			list.Add(Terrain.CreateBlockCheckFilter(pars.BlockCheck, pars.IncludedIds, pars.ExcludedIds));
		}
		if (pars.Within != null)
		{
			list.Add(Terrain.CreateHeightFilter(pars.Within.Min, pars.Within.Max));
		}
		return list;
	}

	public TerrainCommand()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		new TerrainAutoComplete();
		Helper.Command("terrain", "Manipulates the terrain.", (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			UndoHelper.BeginAction();
			TerrainParameters terrainParameters = new TerrainParameters(args);
			TerrainComp[] compilers = GetCompilers(terrainParameters);
			List<Func<TerrainNode, bool>> filterers = GetFilterers(terrainParameters);
			List<HeightNode> list = (from n in GetHeightNodes(terrainParameters, compilers)
				where filterers.All((Func<TerrainNode, bool> f) => f(n))
				select n).ToList();
			List<PaintNode> list2 = (from n in GetPaintNodes(terrainParameters, compilers)
				where filterers.All((Func<TerrainNode, bool> f) => f(n))
				select n).ToList();
			Dictionary<Vector3, TerrainUndoData> data = Terrain.GetData(list, list2);
			if (terrainParameters.Reset)
			{
				List<List<Vector3>> paths = terrainParameters.Paths;
				Range<float>? pathRadius = terrainParameters.PathRadius;
				float? maxDistance = ((pathRadius != null) ? new float?(pathRadius.Max + 1f) : null);
				Func<Vector3, bool> filter = ((paths == null) ? null : ((Func<Vector3, bool>)((Vector3 pos) => TerrainPath.Distance(paths, pos) <= maxDistance)));
				Terrain.ResetTerrain(list, list2, terrainParameters.Position, terrainParameters.Size, filter);
			}
			if (terrainParameters.Set.HasValue)
			{
				Terrain.SetTerrain(list, terrainParameters.Position, terrainParameters.Size, terrainParameters.Smooth, terrainParameters.Set.Value);
			}
			if (terrainParameters.Slope.HasValue && !terrainParameters.Level.HasValue)
			{
				Terrain.SlopeTerrain(list, terrainParameters.Position, terrainParameters.Size, terrainParameters.SlopeAngle, terrainParameters.Smooth, terrainParameters.Position.y, terrainParameters.Slope.Value);
			}
			if (terrainParameters.Level.HasValue && terrainParameters.Paths != null && terrainParameters.LevelToPath)
			{
				Terrain.LevelTerrainToPath(list, terrainParameters.Position, terrainParameters.Size, terrainParameters.Smooth);
			}
			else if (terrainParameters.Level.HasValue)
			{
				Terrain.LevelTerrain(list, terrainParameters.Position, terrainParameters.Size, terrainParameters.Smooth, terrainParameters.Level.Value);
			}
			if (terrainParameters.Delta.HasValue)
			{
				Terrain.RaiseTerrain(list, terrainParameters.Position, terrainParameters.Size, terrainParameters.Smooth, terrainParameters.Delta.Value);
			}
			if (terrainParameters.Min.HasValue)
			{
				Terrain.MinTerrain(list, terrainParameters.Position, terrainParameters.Size, terrainParameters.Min.Value);
			}
			if (terrainParameters.Max.HasValue)
			{
				Terrain.MaxTerrain(list, terrainParameters.Position, terrainParameters.Size, terrainParameters.Max.Value);
			}
			if (terrainParameters.Void)
			{
				Terrain.VoidTerrain(list, terrainParameters.Position, terrainParameters.Size);
			}
			if (terrainParameters.Paint != "")
			{
				string[] array = terrainParameters.Paint.Split(',');
				Color value;
				if (array.Length > 2)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].StartsWith("*"))
						{
							array[i] = "NaN";
						}
					}
					Color color = default(Color);
					((Color)(ref color))._002Ector(Parse.Float(array, 0, 0f), Parse.Float(array, 1, 0f), Parse.Float(array, 2, 0f), Parse.Float(array, 3, 1f));
					Terrain.PaintTerrain(list2, terrainParameters.Position, terrainParameters.Size, terrainParameters.Smooth, color);
				}
				else if (Paints.TryGetValue(terrainParameters.Paint, out value))
				{
					Terrain.PaintTerrain(list2, terrainParameters.Position, terrainParameters.Size, terrainParameters.Smooth, value);
				}
			}
			TerrainComp[] array2 = compilers;
			for (int j = 0; j < array2.Length; j++)
			{
				Terrain.Save(array2[j]);
			}
			Dictionary<Vector3, TerrainUndoData> data2 = Terrain.GetData(list, list2);
			UndoHelper.AddTerrainAction(data, data2, terrainParameters.Position, terrainParameters.Size);
			UndoHelper.EndAction();
		});
	}
}
