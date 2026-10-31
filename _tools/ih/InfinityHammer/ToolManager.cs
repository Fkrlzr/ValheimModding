using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using HarmonyLib;
using ServerDevcommands;
using Service;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch]
public class ToolManager
{
	public const string CmdMod1 = "<mod1>";

	public const string CmdMod2 = "<mod2>";

	public const string CmdAlt = "<alt>";

	public static string DefaultFile = "infinity_tools.yaml";

	public static string Folder = "tools";

	public static string Pattern = "infinity_tools*.yaml";

	private static Dictionary<string, List<ToolData>> ToolData = new Dictionary<string, List<ToolData>>();

	public static Dictionary<string, List<Tool>> Tools = new Dictionary<string, List<Tool>>();

	private static string GetFolderPath()
	{
		string text = Path.Combine(Paths.ConfigPath, Folder);
		if (!Directory.Exists(text))
		{
			return Paths.ConfigPath;
		}
		return text;
	}

	public static void Initialize()
	{
		CreateAlias();
		Yaml.ConsolidateDefaultFile(Paths.ConfigPath, Folder, DefaultFile);
		FromFiles();
		SetupWatcher();
	}

	private static void CreateAlias()
	{
		string text = "from=<x>,<z>,<y> circle=<r>-<r2> angle=<a> rect=<w>-<w2>,<d>";
		string text2 = "from=<x>,<z>,<y> radius=<r>-<r2>";
		string text3 = "to=<x>,<z>,<y> circle=<r>-<r2> rect=<w>-<w2>,<d>";
		string substitution = Settings.Substitution;
		AliasManager.AddAlias("tool_terrain", "terrain " + text);
		AliasManager.AddAlias("t_t", "tool tool_terrain");
		AliasManager.AddAlias("tool_object", "object " + text + " height=<h> ignore=<ignore> id=<include>");
		AliasManager.AddAlias("t_o", "tool tool_object");
		AliasManager.AddAlias("tool_spawn", "spawn_object " + substitution + " " + text2);
		AliasManager.AddAlias("t_s", "tool tool_spawn");
		AliasManager.AddAlias("tool_terrain_to", "terrain " + text3);
		AliasManager.AddAlias("tool_slope", "tool_terrain_to slope");
		AliasManager.AddAlias("tool_area", "hammer " + text + " height=<h> ignore=<ignore> id=<include>");
	}

	public static void CreateFile()
	{
		File.WriteAllText(Path.Combine(GetFolderPath(), DefaultFile), InitialData.Get());
	}

	public static void ToFile()
	{
		Dictionary<string, List<ToolData>> dictionary = ToolData.ToDictionary<KeyValuePair<string, List<ToolData>>, string, List<ToolData>>((KeyValuePair<string, List<ToolData>> kvp) => kvp.Key, (KeyValuePair<string, List<ToolData>> kvp) => kvp.Value.Where((ToolData tool) => tool.IsDefaultData).ToList());
		string contents = Yaml.Serializer().Serialize((object)dictionary);
		File.WriteAllText(Path.Combine(GetFolderPath(), DefaultFile), contents);
	}

	public static ToolData Import(string equipment, string tool)
	{
		ToolData toolData = Yaml.Deserialize<ToolData>(tool.Replace("\\n", "\n"), "Import");
		Add(equipment, toolData);
		return toolData;
	}

	public static void Add(string equipment, ToolData tool)
	{
		if (!ToolData.ContainsKey(equipment))
		{
			ToolData.Add(equipment, new List<ToolData>());
		}
		tool.IsDefaultData = true;
		ToolData[equipment].Add(tool);
		ToFile();
	}

	public static string Export(string equipment, string name)
	{
		if (!TryGetToolData(equipment, name, out ToolData tool) && !TryGetToolData(name, out tool, out equipment))
		{
			return "";
		}
		string text = Yaml.Serializer().Serialize((object)tool).Replace("\r\n", "\\n")
			.Replace("\n", "\\n");
		return "tool_import " + equipment + " " + text;
	}

	public static bool TryGetTool(string equipment, string name, out Tool tool)
	{
		string name2 = name;
		tool = null;
		if (Tools.TryGetValue(equipment, out List<Tool> value))
		{
			tool = value.FirstOrDefault((Tool tool) => string.Equals(tool.Name, name2, StringComparison.OrdinalIgnoreCase));
		}
		if (tool == null)
		{
			tool = Tools.Values.SelectMany((List<Tool> tool) => tool).FirstOrDefault((Tool tool) => string.Equals(tool.Name, name2, StringComparison.OrdinalIgnoreCase));
		}
		return tool != null;
	}

	public static bool TryGetToolData(string equipment, string name, out ToolData tool)
	{
		string name2 = name;
		tool = null;
		if (ToolData.TryGetValue(equipment, out List<ToolData> value))
		{
			tool = value.FirstOrDefault((ToolData tool) => string.Equals(tool.name, name2, StringComparison.OrdinalIgnoreCase));
			return tool != null;
		}
		return tool != null;
	}

	public static bool TryGetToolData(string name, out ToolData tool, out string equipment)
	{
		tool = null;
		equipment = "";
		foreach (KeyValuePair<string, List<ToolData>> toolDatum in ToolData)
		{
			foreach (ToolData item in toolDatum.Value)
			{
				if (string.Equals(item.name, name, StringComparison.OrdinalIgnoreCase))
				{
					tool = item;
					equipment = toolDatum.Key;
					return true;
				}
			}
		}
		return false;
	}

	public static List<Tool> Get(string equipment)
	{
		if (!Tools.TryGetValue(equipment, out List<Tool> value))
		{
			return new List<Tool>();
		}
		return value;
	}

	public static List<Tool> GetAll()
	{
		List<Tool> list = new List<Tool>();
		list.AddRange(Tools.SelectMany<KeyValuePair<string, List<Tool>>, Tool>((KeyValuePair<string, List<Tool>> kvp) => kvp.Value));
		return list;
	}

	public static void FromFiles()
	{
		ToolMenuPieces.Invalidate();
		ToolData.Clear();
		Tools.Clear();
		Yaml.LoadDictFromDirectory<List<ToolData>>(Paths.ConfigPath, Pattern, Folder, (Action<string, string, List<ToolData>>)LoadTool);
		if (ToolData.Count == 0)
		{
			if (Yaml.AnyFileExists(Paths.ConfigPath, Pattern, Folder))
			{
				Log.Warning("Failed to load any tools.");
			}
			else
			{
				CreateFile();
				Yaml.LoadDictFromDirectory<List<ToolData>>(Paths.ConfigPath, Pattern, Folder, (Action<string, string, List<ToolData>>)LoadTool);
			}
			if (ToolData.Count == 0)
			{
				RefreshMenu();
				return;
			}
		}
		Tools = ToolData.ToDictionary<KeyValuePair<string, List<ToolData>>, string, List<Tool>>((KeyValuePair<string, List<ToolData>> kvp) => kvp.Key.ToLower(), (KeyValuePair<string, List<ToolData>> kvp) => kvp.Value.Select((ToolData s) => new Tool(s)).ToList());
		Log.Info($"Reloading {ToolData.Values.SelectMany((List<ToolData> x) => x).Count()} tools.");
		RefreshMenu();
	}

	private static void RefreshMenu()
	{
		Player localPlayer = Player.m_localPlayer;
		if (localPlayer != null)
		{
			localPlayer.UpdateAvailablePiecesList();
		}
		if (Object.op_Implicit((Object)(object)Hud.instance) && Hud.IsPieceSelectionVisible())
		{
			Hud.instance.m_buildUi.m_currentBuildTool = null;
			Hud.instance.m_buildUi.OpenBuildMenu();
		}
	}

	private static void LoadTool(string file, string equipment, List<ToolData> tools)
	{
		if (!ToolData.ContainsKey(equipment))
		{
			ToolData.Add(equipment, new List<ToolData>());
		}
		bool isDefaultData = Yaml.IsDefaultFile(file, Folder, DefaultFile);
		foreach (ToolData tool in tools)
		{
			tool.IsDefaultData = isDefaultData;
		}
		ToolData[equipment].AddRange(tools);
	}

	public static void SetupWatcher()
	{
		Yaml.SetupWatcher(Paths.ConfigPath, Pattern, Folder, (Action)FromFiles);
	}
}
