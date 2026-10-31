using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ServerDevcommands;
using Service;
using UnityEngine;

namespace InfinityHammer;

public class HammerBlueprintCommand : TextReceiver
{
	private enum PlanBuildSection
	{
		Skip,
		Pieces,
		SnapPoints,
		TerrainHeight,
		TerrainPaint
	}

	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<string, string> _003C_003E9__3_0;

		public static Func<string, string> _003C_003E9__4_0;

		public static Func<int, int, List<string>> _003C_003E9__23_0;

		public static Func<int, List<string>> _003C_003E9__23_1;

		public static Func<int, List<string>> _003C_003E9__23_2;

		public static Func<int, List<string>> _003C_003E9__23_3;

		public static Func<int, List<string>> _003C_003E9__23_4;

		public static Func<int, List<string>> _003C_003E9__23_5;

		public static Func<int, List<string>> _003C_003E9__23_6;

		public static Func<int, List<string>> _003C_003E9__23_7;

		public static Func<int, List<string>> _003C_003E9__23_8;

		public static Func<int, List<string>> _003C_003E9__23_9;

		public static Func<int, List<string>> _003C_003E9__23_10;

		public static ConsoleEvent _003C_003E9__24_1;

		internal string _003CFiles_003Eb__3_0(string s)
		{
			return s;
		}

		internal string _003CGetBlueprints_003Eb__4_0(string path)
		{
			return Path.GetFileNameWithoutExtension(path).Replace(" ", "_");
		}

		internal List<string> _003CRegisterAutoComplete_003Eb__23_0(int index, int subIndex)
		{
			if (index == 0)
			{
				return GetBlueprints();
			}
			return new List<string>(10) { "c", "center", "d", "data", "sc", "scale", "s", "snap", "sm", "smooth" };
		}

		internal List<string> _003CRegisterAutoComplete_003Eb__23_1(int index)
		{
			return ParameterInfo.Scale("scale", "Size of the object (if the object can be scaled).", index);
		}

		internal List<string> _003CRegisterAutoComplete_003Eb__23_2(int index)
		{
			return ParameterInfo.Scale("scale", "Size of the object (if the object can be scaled).", index);
		}

		internal List<string> _003CRegisterAutoComplete_003Eb__23_3(int index)
		{
			return ParameterInfo.ObjectIds;
		}

		internal List<string> _003CRegisterAutoComplete_003Eb__23_4(int index)
		{
			return ParameterInfo.ObjectIds;
		}

		internal List<string> _003CRegisterAutoComplete_003Eb__23_5(int index)
		{
			return ParameterInfo.ObjectIds;
		}

		internal List<string> _003CRegisterAutoComplete_003Eb__23_6(int index)
		{
			return ParameterInfo.ObjectIds;
		}

		internal List<string> _003CRegisterAutoComplete_003Eb__23_7(int index)
		{
			return new List<string>(2) { "true", "false" };
		}

		internal List<string> _003CRegisterAutoComplete_003Eb__23_8(int index)
		{
			return new List<string>(2) { "true", "false" };
		}

		internal List<string> _003CRegisterAutoComplete_003Eb__23_9(int index)
		{
			return ParameterInfo.Create("Terrain smooth (0.0-1.0) for heigh or height,paint.");
		}

		internal List<string> _003CRegisterAutoComplete_003Eb__23_10(int index)
		{
			return ParameterInfo.Create("Terrain smooth (0.0-1.0) for height or or height,paint.");
		}

		internal void _003C_002Ector_003Eb__24_1(ConsoleEventArgs args)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			Helper.ArgsCheck(args, 2, "Blueprint name is missing.");
			Hammer.Equip();
			HammerBlueprintPars hammerBlueprintPars = new HammerBlueprintPars(args);
			Blueprint bluePrint = GetBluePrint(hammerBlueprintPars.Name, hammerBlueprintPars.LoadData);
			Vector3 val = bluePrint.Center(hammerBlueprintPars.CenterPiece);
			if (hammerBlueprintPars.SnapPiece != "")
			{
				foreach (Vector3 snapPoint in bluePrint.SnapPoints)
				{
					bluePrint.Objects.Add(new BlueprintObject(hammerBlueprintPars.SnapPiece, snapPoint, Quaternion.identity, Vector3.one, "", "", 1f));
				}
			}
			if (bluePrint.TerrainHeight != null)
			{
				bluePrint.TerrainHeight.Smooth = hammerBlueprintPars.HeightSmooth ?? Configuration.BlueprintTerrainHeightSmooth;
			}
			if (bluePrint.TerrainPaint != null)
			{
				bluePrint.TerrainPaint.Smooth = hammerBlueprintPars.PaintSmooth ?? Configuration.BlueprintTerrainPaintSmooth;
			}
			Selection.CreateGhost(new ObjectSelection(args.Context, bluePrint, hammerBlueprintPars.Scale));
			Position.Override = bluePrint.Coordinates + Quaternion.Euler(bluePrint.Rotation) * val;
			PlaceRotation.Set(Quaternion.Euler(bluePrint.Rotation));
			PrintSelected(args.Context, bluePrint.Name);
		}
	}

	private HammerBlueprintPars? Pars;

	private ConsoleEventArgs? Args;

	private static void PrintSelected(Terminal terminal, string name)
	{
		if (!Configuration.DisableSelectMessages)
		{
			HammerHelper.Message(terminal, "Selected " + name + ".");
		}
	}

	private static IEnumerable<string> LoadFiles(string folder, IEnumerable<string> bps)
	{
		if (Directory.Exists(folder))
		{
			IEnumerable<string> second = Directory.EnumerateFiles(folder, "*.blueprint", SearchOption.AllDirectories);
			IEnumerable<string> second2 = Directory.EnumerateFiles(folder, "*.vbuild", SearchOption.AllDirectories);
			return bps.Concat<string>(second).Concat(second2);
		}
		return bps;
	}

	private static IEnumerable<string> Files()
	{
		IEnumerable<string> bps = Array.Empty<string>();
		bps = LoadFiles(Configuration.BlueprintGlobalFolder, bps);
		if (Path.GetFullPath(Configuration.BlueprintLocalFolder) != Path.GetFullPath(Configuration.BlueprintGlobalFolder))
		{
			bps = LoadFiles(Configuration.BlueprintLocalFolder, bps);
		}
		return from s in bps.Distinct()
			orderby s
			select s;
	}

	public static List<string> GetBlueprints()
	{
		List<string> list = new List<string>();
		list.AddRange(from path in Files()
			select Path.GetFileNameWithoutExtension(path).Replace(" ", "_"));
		return list;
	}

	public static List<Blueprint> GetBlueprintsByCategory()
	{
		List<Blueprint> list = new List<Blueprint>();
		list.AddRange(Files().Select(GetBluePrintCategory));
		return list;
	}

	public static List<Blueprint> GetBlueprintsByFolder()
	{
		List<Blueprint> list = new List<Blueprint>();
		list.AddRange(Files().Select(GetBluePrintWithFolder));
		return list;
	}

	public static List<Blueprint> GetBlueprintsByCategoryAndFolder()
	{
		List<Blueprint> list = new List<Blueprint>();
		list.AddRange(Files().Select(GetBluePrintWithCategoryAndFolder));
		return list;
	}

	private static Blueprint GetBluePrintCategory(string filePath)
	{
		string name = Path.GetFileNameWithoutExtension(filePath).Replace(" ", "_");
		string[] rows = File.ReadAllLines(filePath);
		string extension = Path.GetExtension(filePath);
		Blueprint blueprint = new Blueprint
		{
			Name = name
		};
		if (extension == ".vbuild")
		{
			return blueprint;
		}
		if (extension == ".blueprint")
		{
			return GetPlanBuildCategory(blueprint, rows);
		}
		throw new InvalidOperationException("Unknown file format.");
	}

	private static Blueprint GetBluePrintWithFolder(string filePath)
	{
		string name = Path.GetFileNameWithoutExtension(filePath).Replace(" ", "_");
		string folderNameFromPath = GetFolderNameFromPath(filePath);
		return new Blueprint
		{
			Name = name,
			Category = folderNameFromPath
		};
	}

	private static Blueprint GetBluePrintWithCategoryAndFolder(string filePath)
	{
		string[] rows = File.ReadAllLines(filePath);
		string extension = Path.GetExtension(filePath);
		Blueprint bluePrintWithFolder = GetBluePrintWithFolder(filePath);
		if (extension == ".vbuild")
		{
			return bluePrintWithFolder;
		}
		if (extension == ".blueprint")
		{
			return GetPlanBuildCategory(bluePrintWithFolder, rows);
		}
		throw new InvalidOperationException("Unknown file format.");
	}

	private static string GetFolderNameFromPath(string filePath)
	{
		string directoryName = Path.GetDirectoryName(filePath);
		if (string.IsNullOrEmpty(directoryName))
		{
			return "Blueprints";
		}
		string fileName = Path.GetFileName(directoryName);
		if (!string.IsNullOrEmpty(fileName))
		{
			return fileName;
		}
		return "Blueprints";
	}

	private static Blueprint GetBluePrint(string name, bool loadData)
	{
		string name2 = name;
		string path2 = Files().FirstOrDefault((string path) => Path.GetFileNameWithoutExtension(path).Replace(" ", "_") == name2) ?? throw new InvalidOperationException("Blueprint not found.");
		string[] rows = File.ReadAllLines(path2);
		string extension = Path.GetExtension(path2);
		Blueprint bp = new Blueprint
		{
			Name = name2
		};
		if (extension == ".vbuild")
		{
			return GetBuildShare(bp, rows, loadData);
		}
		if (extension == ".blueprint")
		{
			return GetPlanBuild(bp, rows, loadData);
		}
		throw new InvalidOperationException("Unknown file format.");
	}

	private static Blueprint GetPlanBuildCategory(Blueprint bp, string[] rows)
	{
		foreach (string text in rows)
		{
			if (text.StartsWith("#name:", StringComparison.OrdinalIgnoreCase))
			{
				bp.Name = text.Split(':')[1];
			}
			else if (text.StartsWith("#category:", StringComparison.OrdinalIgnoreCase))
			{
				bp.Category = text.Split(':')[1];
			}
			else if (text.StartsWith("#pieces", StringComparison.OrdinalIgnoreCase))
			{
				return bp;
			}
		}
		return bp;
	}

	internal static Blueprint GetPlanBuild(Blueprint bp, string[] rows, bool loadData)
	{
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		PlanBuildSection planBuildSection = PlanBuildSection.Pieces;
		TerrainHeight terrainHeight = null;
		TerrainPaint terrainPaint = null;
		int num = 0;
		foreach (string text in rows)
		{
			bool flag = text.StartsWith("#", StringComparison.Ordinal);
			if (flag)
			{
				num = 0;
				if ((uint)(planBuildSection - 3) <= 1u)
				{
					planBuildSection = PlanBuildSection.Skip;
				}
			}
			if (text.StartsWith("#name:", StringComparison.OrdinalIgnoreCase))
			{
				bp.Name = text.Split(':')[1];
				continue;
			}
			if (text.StartsWith("#creator:", StringComparison.OrdinalIgnoreCase))
			{
				bp.Creator = text.Split(':')[1];
				continue;
			}
			if (text.StartsWith("#description:", StringComparison.OrdinalIgnoreCase))
			{
				bp.Description = text.Split(':')[1];
				continue;
			}
			if (text.StartsWith("#category:", StringComparison.OrdinalIgnoreCase))
			{
				bp.Category = text.Split(':')[1];
				continue;
			}
			if (text.StartsWith("#center:", StringComparison.OrdinalIgnoreCase))
			{
				bp.CenterPiece = text.Split(':')[1];
				continue;
			}
			if (text.StartsWith("#coordinates:", StringComparison.OrdinalIgnoreCase))
			{
				bp.Coordinates = Parse.VectorXZY(text.Split(':')[1]);
				continue;
			}
			if (text.StartsWith("#rotation:", StringComparison.OrdinalIgnoreCase))
			{
				bp.Rotation = Parse.VectorXZY(text.Split(':')[1]);
				continue;
			}
			if (text.StartsWith("#height:", StringComparison.OrdinalIgnoreCase) || text.StartsWith("#paint:", StringComparison.OrdinalIgnoreCase))
			{
				throw new InvalidOperationException("Legacy #Height/#Paint terrain format is no longer supported. Re-save the blueprint with a newer Infinity Hammer version.");
			}
			if (text.StartsWith("#terrainheight:", StringComparison.OrdinalIgnoreCase))
			{
				planBuildSection = PlanBuildSection.Skip;
				string[] array = text.Split(':')[1].Split(';');
				if (array.Length >= 3)
				{
					float num2 = InvariantFloat(array, 1);
					terrainHeight = new TerrainHeight
					{
						CenterPosition = Parse.VectorXZY(array[0]),
						CenterRotation = Quaternion.Euler(0f, num2, 0f),
						DistanceBetweenNodes = InvariantFloat(array, 2, 1f)
					};
					planBuildSection = PlanBuildSection.TerrainHeight;
				}
				continue;
			}
			if (text.StartsWith("#terrainpaint:", StringComparison.OrdinalIgnoreCase))
			{
				planBuildSection = PlanBuildSection.Skip;
				string[] array2 = text.Split(':')[1].Split(';');
				if (array2.Length >= 3)
				{
					float num3 = InvariantFloat(array2, 1);
					terrainPaint = new TerrainPaint
					{
						CenterPosition = Parse.VectorXZY(array2[0]),
						CenterRotation = Quaternion.Euler(0f, num3, 0f),
						DistanceBetweenNodes = InvariantFloat(array2, 2, 1f)
					};
					planBuildSection = PlanBuildSection.TerrainPaint;
				}
				continue;
			}
			if (text.StartsWith("#snappoints", StringComparison.OrdinalIgnoreCase))
			{
				planBuildSection = PlanBuildSection.SnapPoints;
				continue;
			}
			if (text.StartsWith("#pieces", StringComparison.OrdinalIgnoreCase))
			{
				planBuildSection = PlanBuildSection.Pieces;
				continue;
			}
			if (flag)
			{
				if (text.Length != 1 && !char.IsWhiteSpace(text[1]))
				{
					planBuildSection = PlanBuildSection.Skip;
				}
				continue;
			}
			switch (planBuildSection)
			{
			case PlanBuildSection.Pieces:
				bp.Objects.Add(GetPlanBuildObject(text, loadData));
				break;
			case PlanBuildSection.SnapPoints:
				bp.SnapPoints.Add(GetPlanBuildSnapPoint(text));
				break;
			case PlanBuildSection.TerrainHeight:
				if (terrainHeight != null)
				{
					ParseTerrainHeightRow(terrainHeight, text, num);
					num++;
				}
				break;
			case PlanBuildSection.TerrainPaint:
				if (terrainPaint != null)
				{
					ParseTerrainPaintRow(terrainPaint, text, num);
					num++;
				}
				break;
			}
		}
		bp.TerrainHeight = terrainHeight;
		bp.TerrainPaint = terrainPaint;
		return bp;
	}

	private static BlueprintObject GetPlanBuildObject(string row, bool loadData)
	{
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		if (row.IndexOf(',') > -1)
		{
			row = row.Replace(',', '.');
		}
		string[] array = row.Split(';');
		string name = array[0];
		float num = InvariantFloat(array, 2);
		float num2 = InvariantFloat(array, 3);
		float num3 = InvariantFloat(array, 4);
		float num4 = InvariantFloat(array, 5);
		float num5 = InvariantFloat(array, 6);
		float num6 = InvariantFloat(array, 7);
		float num7 = InvariantFloat(array, 8);
		string info = ((array.Length > 9) ? array[9] : "");
		float num8 = InvariantFloat(array, 10, 1f);
		float num9 = InvariantFloat(array, 11, 1f);
		float num10 = InvariantFloat(array, 12, 1f);
		return new BlueprintObject(data: (loadData && array.Length > 13) ? array[13] : "", chance: InvariantFloat(array, 14, 1f), name: name, pos: new Vector3(num, num2, num3), rot: new Quaternion(num4, num5, num6, num7), scale: new Vector3(num8, num9, num10), info: info);
	}

	private static Vector3 GetPlanBuildSnapPoint(string row)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (row.IndexOf(',') > -1)
		{
			row = row.Replace(',', '.');
		}
		string[] row2 = row.Split(';');
		float num = InvariantFloat(row2, 0);
		float num2 = InvariantFloat(row2, 1);
		float num3 = InvariantFloat(row2, 2);
		return new Vector3(num, num2, num3);
	}

	private static Blueprint GetBuildShare(Blueprint bp, string[] rows, bool loadData)
	{
		bp.Objects = rows.Select((string r) => GetBuildShareObject(r, loadData)).ToList();
		return bp;
	}

	private static BlueprintObject GetBuildShareObject(string row, bool loadData)
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		if (row.IndexOf(',') > -1)
		{
			row = row.Replace(',', '.');
		}
		string[] array = row.Split(' ');
		string name = array[0];
		float num = InvariantFloat(array, 1);
		float num2 = InvariantFloat(array, 2);
		float num3 = InvariantFloat(array, 3);
		float num4 = InvariantFloat(array, 4);
		float num5 = InvariantFloat(array, 5);
		float num6 = InvariantFloat(array, 6);
		float num7 = InvariantFloat(array, 7);
		return new BlueprintObject(data: (loadData && array.Length > 8) ? array[8] : "", chance: InvariantFloat(array, 9, 1f), name: name, pos: new Vector3(num5, num6, num7), rot: new Quaternion(num, num2, num3, num4), scale: Vector3.one, info: "");
	}

	private static void ParseTerrainHeightRow(TerrainHeight terrainData, string row, int rowIndex)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		if (row.IndexOf(',') > -1)
		{
			row = row.Replace(',', '.');
		}
		string[] array = row.Split(';');
		if (rowIndex == 0)
		{
			terrainData.InitializeGrid(array.Length, 0, terrainData.CenterPosition);
		}
		if (rowIndex >= terrainData.Height)
		{
			float?[,] heights = terrainData.Heights;
			terrainData.Heights = new float?[terrainData.Width, rowIndex + 1];
			for (int i = 0; i < terrainData.Width; i++)
			{
				for (int j = 0; j < terrainData.Height; j++)
				{
					terrainData.Heights[i, j] = heights[i, j];
				}
			}
			terrainData.Height = rowIndex + 1;
		}
		for (int k = 0; k < array.Length && k < terrainData.Width; k++)
		{
			if (!string.IsNullOrEmpty(array[k]) && float.TryParse(array[k], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out var result))
			{
				terrainData.Set(k, rowIndex, result);
			}
		}
	}

	private static void ParseTerrainPaintRow(TerrainPaint terrainData, string row, int rowIndex)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		if (row.IndexOf(',') > -1)
		{
			row = row.Replace(',', '.');
		}
		string[] array = row.Split(';');
		if (rowIndex == 0)
		{
			terrainData.InitializeGrid(array.Length, 0, terrainData.CenterPosition);
		}
		if (rowIndex >= terrainData.Height)
		{
			Color?[,] paints = terrainData.Paints;
			terrainData.Paints = new Color?[terrainData.Width, rowIndex + 1];
			for (int i = 0; i < terrainData.Width; i++)
			{
				for (int j = 0; j < terrainData.Height; j++)
				{
					terrainData.Paints[i, j] = paints[i, j];
				}
			}
			terrainData.Height = rowIndex + 1;
		}
		for (int k = 0; k < array.Length && k < terrainData.Width; k++)
		{
			if (!string.IsNullOrEmpty(array[k]))
			{
				string[] array2 = array[k].Split(':');
				if (array2.Length >= 3 && float.TryParse(array2[0], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out var result) && float.TryParse(array2[1], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out var result2) && float.TryParse(array2[2], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out var result3))
				{
					float result4;
					float num = ((array2.Length > 3 && float.TryParse(array2[3], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out result4)) ? result4 : 1f);
					terrainData.Set(k, rowIndex, new Color(result, result2, result3, num));
				}
			}
		}
	}

	private static float InvariantFloat(string[] row, int index, float defaultValue = 0f)
	{
		if (index >= row.Length)
		{
			return defaultValue;
		}
		string text = row[index];
		if (string.IsNullOrEmpty(text))
		{
			return defaultValue;
		}
		return float.Parse(text, NumberStyles.Any, NumberFormatInfo.InvariantInfo);
	}

	private void SelectBlueprint(ConsoleEventArgs args, HammerBlueprintPars pars)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		if (pars.Name == "")
		{
			return;
		}
		Hammer.Equip();
		Blueprint bluePrint = GetBluePrint(pars.Name, pars.LoadData);
		bluePrint.Center(pars.CenterPiece);
		if (pars.SnapPiece != "")
		{
			foreach (Vector3 snapPoint in bluePrint.SnapPoints)
			{
				bluePrint.Objects.Add(new BlueprintObject(pars.SnapPiece, snapPoint, Quaternion.identity, Vector3.one, "", "", 1f));
			}
		}
		if (bluePrint.TerrainHeight != null)
		{
			bluePrint.TerrainHeight.Smooth = pars.HeightSmooth ?? Configuration.BlueprintTerrainHeightSmooth;
		}
		if (bluePrint.TerrainPaint != null)
		{
			bluePrint.TerrainPaint.Smooth = pars.PaintSmooth ?? Configuration.BlueprintTerrainPaintSmooth;
		}
		Selection.CreateGhost(new ObjectSelection(args.Context, bluePrint, pars.Scale));
		PrintSelected(args.Context, bluePrint.Name);
	}

	private static void RegisterAutoComplete(string command)
	{
		AutoComplete.Register(command, (Func<int, int, List<string>>)((int index, int subIndex) => (index == 0) ? GetBlueprints() : new List<string>(10) { "c", "center", "d", "data", "sc", "scale", "s", "snap", "sm", "smooth" }), new Dictionary<string, Func<int, List<string>>>
		{
			{
				"scale",
				(int index) => ParameterInfo.Scale("scale", "Size of the object (if the object can be scaled).", index)
			},
			{
				"sc",
				(int index) => ParameterInfo.Scale("scale", "Size of the object (if the object can be scaled).", index)
			},
			{
				"center",
				(int index) => ParameterInfo.ObjectIds
			},
			{
				"c",
				(int index) => ParameterInfo.ObjectIds
			},
			{
				"snap",
				(int index) => ParameterInfo.ObjectIds
			},
			{
				"s",
				(int index) => ParameterInfo.ObjectIds
			},
			{
				"data",
				(int index) => new List<string>(2) { "true", "false" }
			},
			{
				"d",
				(int index) => new List<string>(2) { "true", "false" }
			},
			{
				"smooth",
				(int index) => ParameterInfo.Create("Terrain smooth (0.0-1.0) for heigh or height,paint.")
			},
			{
				"sm",
				(int index) => ParameterInfo.Create("Terrain smooth (0.0-1.0) for height or or height,paint.")
			}
		});
	}

	public HammerBlueprintCommand()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		RegisterAutoComplete("hammer_blueprint");
		Helper.Command("hammer_blueprint", "[blueprint file] [center=piece] [snap=piece] [scale=x,z,y] [data=true/false] - Selects the blueprint to be placed.", (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			Args = args;
			Pars = new HammerBlueprintPars(args);
			if (Pars.Name != "")
			{
				SelectBlueprint(args, Pars);
				Pars = null;
				Args = null;
			}
			else
			{
				TextInput.instance.RequestText((TextReceiver)(object)this, "Name", 1000);
			}
		});
		RegisterAutoComplete("hammer_restore");
		object obj = _003C_003Ec._003C_003E9__24_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0116: Unknown result type (might be due to invalid IL or missing references)
				//IL_0127: Unknown result type (might be due to invalid IL or missing references)
				//IL_012d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0132: Unknown result type (might be due to invalid IL or missing references)
				//IL_0137: Unknown result type (might be due to invalid IL or missing references)
				//IL_0138: Unknown result type (might be due to invalid IL or missing references)
				//IL_013d: Unknown result type (might be due to invalid IL or missing references)
				//IL_014d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0152: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				Helper.ArgsCheck(args, 2, "Blueprint name is missing.");
				Hammer.Equip();
				HammerBlueprintPars hammerBlueprintPars = new HammerBlueprintPars(args);
				Blueprint bluePrint = GetBluePrint(hammerBlueprintPars.Name, hammerBlueprintPars.LoadData);
				Vector3 val2 = bluePrint.Center(hammerBlueprintPars.CenterPiece);
				if (hammerBlueprintPars.SnapPiece != "")
				{
					foreach (Vector3 snapPoint in bluePrint.SnapPoints)
					{
						bluePrint.Objects.Add(new BlueprintObject(hammerBlueprintPars.SnapPiece, snapPoint, Quaternion.identity, Vector3.one, "", "", 1f));
					}
				}
				if (bluePrint.TerrainHeight != null)
				{
					bluePrint.TerrainHeight.Smooth = hammerBlueprintPars.HeightSmooth ?? Configuration.BlueprintTerrainHeightSmooth;
				}
				if (bluePrint.TerrainPaint != null)
				{
					bluePrint.TerrainPaint.Smooth = hammerBlueprintPars.PaintSmooth ?? Configuration.BlueprintTerrainPaintSmooth;
				}
				Selection.CreateGhost(new ObjectSelection(args.Context, bluePrint, hammerBlueprintPars.Scale));
				Position.Override = bluePrint.Coordinates + Quaternion.Euler(bluePrint.Rotation) * val2;
				PlaceRotation.Set(Quaternion.Euler(bluePrint.Rotation));
				PrintSelected(args.Context, bluePrint.Name);
			};
			_003C_003Ec._003C_003E9__24_1 = val;
			obj = (object)val;
		}
		Helper.Command("hammer_restore", "[blueprint file] [center=piece] [snap=piece] [scale=x,z,y] [data=true/false] - Restores the blueprint at its saved position.", (ConsoleEvent)obj);
	}

	public string GetText()
	{
		return "";
	}

	public void SetText(string text)
	{
		if (Pars != null && Args != null)
		{
			Pars.Name = text;
			SelectBlueprint(Args, Pars);
			Pars = null;
			Args = null;
		}
	}
}
