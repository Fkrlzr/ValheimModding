using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HarmonyLib;

namespace SkToolbox.Utility;

// Soft compatibility with Jere Kuusela's World Edit Commands and Infinity Hammer.
// Nothing here references those assemblies: the toolbox only checks that the plugins are loaded and then drives them
// through their console commands (and, for the hammer's switches, through the plugin's BepInEx config).
// That keeps the toolbox working with or without them, and with any version that keeps the same commands.
internal static class EditCompat
{
	internal const string WecGuid = "world_edit_commands";

	internal const string HammerGuid = "infinity_hammer";

	private static readonly FieldInfo commandsField = AccessTools.Field(typeof(Terminal), "commands");

	private static bool Loaded(string guid)
	{
		try
		{
			return Chainloader.PluginInfos != null && Chainloader.PluginInfos.ContainsKey(guid);
		}
		catch (Exception)
		{
			return false;
		}
	}

	internal static bool HasWec => Loaded(WecGuid);

	internal static bool HasHammer => Loaded(HammerGuid);

	internal static bool Any => HasWec || HasHammer;

	// True when the game console knows the command. A plugin can be loaded and still not have registered its commands yet
	// (they are added once the terminal starts), or have registered none because Server Devcommands is missing.
	internal static bool CommandExists(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return false;
		}
		try
		{
			IDictionary dictionary = commandsField?.GetValue(null) as IDictionary;
			return dictionary != null && dictionary.Contains(name.ToLowerInvariant());
		}
		catch (Exception)
		{
			return false;
		}
	}

	// Runs a console command that belongs to World Edit Commands, Infinity Hammer or Server Devcommands.
	// Returns false (after telling the player why) when it cannot be run.
	internal static bool Ready(string command)
	{
		if (string.IsNullOrEmpty(command))
		{
			return false;
		}
		string text = command.Split(' ')[0];
		bool flag = text.StartsWith("hammer", StringComparison.OrdinalIgnoreCase);
		if (flag ? HasHammer : HasWec)
		{
			if (CommandExists(text))
			{
				return true;
			}
			SkCommandProcessor.Notify("'" + text + "' is not available. Server Devcommands must be installed, and a world loaded.");
			return false;
		}
		SkCommandProcessor.Notify(flag ? "Infinity Hammer is not installed." : "World Edit Commands is not installed.");
		return false;
	}

	internal static void Run(string command, string done)
	{
		if (Ready(command))
		{
			SkRun.CmdNotify(command, done);
		}
	}

	internal static void Show(string title, string command, string empty)
	{
		if (Ready(command))
		{
			SkRun.Show(title, command, empty);
		}
	}

	// The key hammer_config expects: the config name in lower case with underscores.
	internal static string ConfigKey(string name)
	{
		return name.ToLowerInvariant().Replace(' ', '_').Replace("(", "").Replace(")", "");
	}

	// Reads one of Infinity Hammer's settings straight from its BepInEx config, or null when it cannot be found.
	internal static object HammerSetting(string name)
	{
		try
		{
			if (!Chainloader.PluginInfos.TryGetValue(HammerGuid, out var value) || (UnityEngine.Object)(object)value.Instance == (UnityEngine.Object)null)
			{
				return null;
			}
			ConfigFile config = value.Instance.Config;
			foreach (ConfigDefinition key in config.Keys)
			{
				if (string.Equals(key.Key, name, StringComparison.OrdinalIgnoreCase))
				{
					return config[key].BoxedValue;
				}
			}
		}
		catch (Exception)
		{
		}
		return null;
	}

	internal static bool HammerFlag(string name)
	{
		object obj = HammerSetting(name);
		return obj is bool && (bool)obj;
	}

	internal static string HammerText(string name, string fallback)
	{
		object obj = HammerSetting(name);
		return (obj != null) ? obj.ToString() : fallback;
	}
}
