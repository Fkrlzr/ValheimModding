using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using ServerDevcommands;
using Service;
using UnityEngine;

namespace Data;

public class DataLoading
{
	private static readonly string GamePath = Path.GetFullPath(Path.Combine("BepInEx", "config", "data"));

	private static readonly string ProfilePath = Path.GetFullPath(Path.Combine(Paths.ConfigPath, "data"));

	public static readonly Dictionary<int, DataEntry> Data = new Dictionary<int, DataEntry>();

	public static readonly List<string> DataKeys = new List<string>();

	public static void LoadEntries()
	{
		if (!Object.op_Implicit((Object)(object)ZNet.instance))
		{
			return;
		}
		Data.Clear();
		DataKeys.Clear();
		ValueGroups.Clear();
		List<(string File, DataYaml Data)> loaded = new List<(string, DataYaml)>();
		Yaml.LoadListsFromDirectory<DataYaml>(GamePath, "*.yaml", (Action<string, DataYaml>)Collect);
		if (ProfilePath != GamePath)
		{
			Yaml.LoadListsFromDirectory<DataYaml>(ProfilePath, "*.yaml", (Action<string, DataYaml>)Collect);
		}
		foreach (var item in loaded)
		{
			ValueGroups.Add(item.Data, item.File);
		}
		if (ValueGroups.Count > 0)
		{
			Log.Info($"Loaded {ValueGroups.Count} value groups.");
		}
		ValueGroups.Resolve();
		foreach (var item2 in loaded)
		{
			LoadEntry(item2.File, item2.Data);
		}
		PrefabHelper.ClearCache();
		Log.Info($"Loaded {Data.Count} data entries.");
		void Collect(string file, DataYaml data)
		{
			loaded.Add((file, data));
		}
	}

	private static void LoadEntry(string file, DataYaml data)
	{
		if (data.name != null)
		{
			int stableHashCode = StringExtensionMethods.GetStableHashCode(data.name);
			if (Data.ContainsKey(stableHashCode))
			{
				Log.Warning("Duplicate data entry: " + data.name + " at " + file);
			}
			DataKeys.Add(data.name);
			Data[stableHashCode] = new DataEntry(data);
		}
	}

	public static void Save(PlainDataEntry data, string name, bool profile, bool dump)
	{
		string path = Path.Combine(profile ? ProfilePath : GamePath, "data.yaml");
		if (!File.Exists(path))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			File.Create(path).Close();
		}
		string text = File.ReadAllText(path);
		text = text + "\n" + Yaml.Serializer().Serialize((object)new DataYaml[1] { ToData(data, name, dump) });
		File.WriteAllText(path, text);
	}

	public static DataYaml ToData(PlainDataEntry zdo, string name, bool dump)
	{
		DataYaml dataYaml = new DataYaml
		{
			name = name
		};
		zdo.Write(dataYaml, dump);
		return dataYaml;
	}

	public static void SetupWatcher()
	{
		if (!Directory.Exists(GamePath))
		{
			Directory.CreateDirectory(GamePath);
		}
		if (!Directory.Exists(ProfilePath))
		{
			Directory.CreateDirectory(ProfilePath);
		}
		Yaml.SetupWatcher(GamePath, "*", (Action)LoadEntries);
		if (GamePath != ProfilePath)
		{
			Yaml.SetupWatcher(ProfilePath, "*", (Action)LoadEntries);
		}
	}
}
