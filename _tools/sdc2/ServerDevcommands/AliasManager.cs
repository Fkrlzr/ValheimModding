using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using BepInEx;
using HarmonyLib;
using Service;

namespace ServerDevcommands;

[HarmonyPatch]
public class AliasManager
{
	private class AliasEntry
	{
		public string Value = "";

		public bool IsDefault;
	}

	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<string, string> _003C_003E9__6_0;

		public static Func<KeyValuePair<string, AliasEntry>, bool> _003C_003E9__10_0;

		public static Func<KeyValuePair<string, AliasEntry>, string> _003C_003E9__10_1;

		public static Func<KeyValuePair<string, AliasEntry>, string> _003C_003E9__10_2;

		public static ConsoleEvent _003C_003E9__16_0;

		internal string _003Cget_AliasKeys_003Eb__6_0(string key)
		{
			return key;
		}

		internal bool _003CToFile_003Eb__10_0(KeyValuePair<string, AliasEntry> kvp)
		{
			return kvp.Value.IsDefault;
		}

		internal string _003CToFile_003Eb__10_1(KeyValuePair<string, AliasEntry> kvp)
		{
			return kvp.Key;
		}

		internal string _003CToFile_003Eb__10_2(KeyValuePair<string, AliasEntry> kvp)
		{
			return kvp.Value.Value;
		}

		internal void _003CAddCommand_003Eb__16_0(ConsoleEventArgs args)
		{
		}
	}

	public static string Pattern = "alias*.yaml";

	public static string Folder = "alias";

	public const string DefaultFile = "alias.yaml";

	private static readonly Dictionary<string, AliasEntry> Aliases = new Dictionary<string, AliasEntry>();

	public static bool ToBeSaved = false;

	public static string[] AliasKeys
	{
		get
		{
			List<string> list = new List<string>();
			list.AddRange(Aliases.Keys.OrderBy((string key) => key));
			return list.ToArray();
		}
	}

	private static string GetFolderPath()
	{
		string text = Path.Combine(Paths.ConfigPath, Folder);
		if (!Directory.Exists(text))
		{
			return Paths.ConfigPath;
		}
		return text;
	}

	public static void Init()
	{
		Yaml.ConsolidateDefaultFile(Paths.ConfigPath, Folder, "alias.yaml");
		FromFiles();
		Yaml.SetupWatcher(Paths.ConfigPath, Pattern, Folder, FromFiles);
	}

	public static void ToFile()
	{
		ToBeSaved = false;
		Dictionary<string, string> graph = Aliases.Where<KeyValuePair<string, AliasEntry>>((KeyValuePair<string, AliasEntry> kvp) => kvp.Value.IsDefault).ToDictionary((KeyValuePair<string, AliasEntry> kvp) => kvp.Key, (KeyValuePair<string, AliasEntry> kvp) => kvp.Value.Value);
		string contents = Yaml.Serializer().Serialize(graph);
		File.WriteAllText(Path.Combine(GetFolderPath(), "alias.yaml"), contents);
	}

	public static void FromFiles()
	{
		foreach (string key in Aliases.Keys)
		{
			RemoveCommand(key);
		}
		Aliases.Clear();
		Yaml.LoadDictFromDirectory<string>(Paths.ConfigPath, Pattern, Folder, LoadAlias);
		Log.Info($"Reloading {Aliases.Count} alias data.");
	}

	public static string GetAliasValue(string key)
	{
		if (!Aliases.TryGetValue(key, out AliasEntry value))
		{
			return "_";
		}
		return value.Value;
	}

	public static void LoadAlias(string file, string alias, string value)
	{
		if (Aliases.ContainsKey(alias))
		{
			Log.Warning("Duplicate alias '" + alias + "' in " + file + ". Overwriting previous value.");
		}
		AddCommand(alias, value);
		Aliases[alias] = new AliasEntry
		{
			Value = value,
			IsDefault = Yaml.IsDefaultFile(file, Folder, "alias.yaml")
		};
	}

	public static void AddAlias(string alias, string value)
	{
		Aliases[alias] = new AliasEntry
		{
			Value = value,
			IsDefault = true
		};
		AddCommand(alias, value);
		ToBeSaved = true;
	}

	public static void RemoveAlias(string alias)
	{
		Aliases.Remove(alias);
		RemoveCommand(alias);
		ToBeSaved = true;
	}

	public static void AddCommand(string key, string value)
	{
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		string text = Aliasing.Plain(value);
		string key2 = text.Split(' ').First();
		if (Terminal.commands.TryGetValue(key2, out var value2))
		{
			if (value2.action != null)
			{
				new ConsoleCommand(key, text, value2.action, value2.IsCheat, value2.IsNetwork, value2.OnlyServer, value2.IsSecret, value2.AllowInDevBuild, value2.HideBehindDevCommands, value2.m_tabOptionsFetcher, value2.m_alwaysRefreshTabOptions, value2.RemoteCommand, value2.OnlyAdmin);
			}
			else
			{
				new ConsoleCommand(key, text, value2.actionFailable, value2.IsCheat, value2.IsNetwork, value2.OnlyServer, value2.IsSecret, value2.AllowInDevBuild, value2.HideBehindDevCommands, value2.m_tabOptionsFetcher, value2.m_alwaysRefreshTabOptions, value2.RemoteCommand, value2.OnlyAdmin);
			}
			return;
		}
		object obj = _003C_003Ec._003C_003E9__16_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate
			{
			};
			_003C_003Ec._003C_003E9__16_0 = val;
			obj = (object)val;
		}
		new ConsoleCommand(key, text, (ConsoleEvent)obj, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
	}

	public static void RemoveCommand(string key)
	{
		if (Terminal.commands.ContainsKey(key))
		{
			Terminal.commands.Remove(key);
		}
	}
}
