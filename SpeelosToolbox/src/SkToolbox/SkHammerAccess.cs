using System;
using System.Reflection;
using HarmonyLib;
using SkToolbox.Configuration;
using SkToolbox.Utility;
using UnityEngine;

namespace SkToolbox;

// Optional, off by default (config: HammerOnAnyServer).
//
// Infinity Hammer decides whether you may use each feature by asking Server Devcommands' PermissionManager. With no
// explicit permission from the server the answer is "your setting AND you are an admin with devcommands on", so on a
// server where you are not an admin every gated feature reads as off, even though the hammer runs on your own client.
//
// For a server whose owner has agreed to it, this postfix answers "your setting" for the "infinityhammer" section only.
// It respects the server: a feature (or the "*" wildcard) that the server set to No stays off. It never touches other
// sections, so it does not grant fly, god or any other Server Devcommands feature, and it changes nothing server side.
//
// Nothing here references Server Devcommands at build time. If it is not installed the patch is skipped.
[HarmonyPatch]
internal static class SkHammerAccess
{
	private const string Section = "infinityhammer";

	// Server Devcommands' FeaturePermission enum: Unknown = -1, Yes = 0, No = 1, Force = 2.
	private const int PermissionNo = 1;

	private static readonly Type managerType = AccessTools.TypeByName("ServerDevcommands.PermissionManager");

	private static readonly MethodInfo getPermission = ((managerType != null) ? AccessTools.Method(managerType, "GetFeaturePermissionByHash", new Type[2]
	{
		typeof(string),
		typeof(int)
	}, (Type[])null) : null);

	private static readonly FieldInfo wildcardField = ((managerType != null) ? AccessTools.Field(managerType, "WildCardHash") : null);

	private static bool Enabled
	{
		get
		{
			if (SkConfigEntry.CHammerOnAnyServer != null)
			{
				return SkConfigEntry.CHammerOnAnyServer.Value;
			}
			return false;
		}
	}

	private static bool Prepare()
	{
		return managerType != null && getPermission != null && wildcardField != null && AccessTools.Method(managerType, "IsFeatureEnabledByHash", (Type[])null, (Type[])null) != null;
	}

	private static MethodBase TargetMethod()
	{
		return AccessTools.Method(managerType, "IsFeatureEnabledByHash", (Type[])null, (Type[])null);
	}

	private static void Postfix(object __instance, string section, int featureHash, bool localConfigValue, ref bool __result)
	{
		if (__result || !localConfigValue || !Enabled || !string.Equals(section, Section, StringComparison.OrdinalIgnoreCase))
		{
			return;
		}
		try
		{
			int wildcard = (int)wildcardField.GetValue(null);
			if (Permission(__instance, section, featureHash) == PermissionNo || Permission(__instance, section, wildcard) == PermissionNo)
			{
				return;
			}
			__result = true;
		}
		catch (Exception ex)
		{
			SkUtilities.Logz(new string[2] { "HAMMER", "ERROR" }, new string[1] { ex.Message }, (LogType)2);
		}
	}

	private static int Permission(object manager, string section, int hash)
	{
		return Convert.ToInt32(getPermission.Invoke(manager, new object[2] { section, hash }));
	}

	internal static bool Active => Enabled && EditCompat.HasHammer;

	// The commands Infinity Hammer's build menu tools run. Everything else keeps the normal rules.
	private static readonly string[] ToolCommands = new string[7] { "terrain", "object", "spawn_object", "spawn_location", "undo", "redo", "data" };

	private static readonly FieldInfo bannedField = ((managerType != null) ? AccessTools.Field(managerType, "_bannedCommands") : null);

	private static readonly PropertyInfo instanceProperty = ((managerType != null) ? AccessTools.Property(managerType, "Instance") : null);

	internal static bool IsToolCommand(string command)
	{
		if (string.IsNullOrEmpty(command))
		{
			return false;
		}
		string text = command.Trim().ToLowerInvariant();
		// "tool" and "tool_*" are how the build menu selects a tool (Infinity Hammer runs "tool <name>" when you pick one).
		// "_bind_*" are the wrapper commands Infinity Hammer registers for its key and mouse wheel binds (resize, change shape,
		// move, freeze). Each one only runs the command you bound, and that command is checked on its own.
		if (text.StartsWith("hammer", StringComparison.Ordinal) || text == "tool" || text.StartsWith("tool_", StringComparison.Ordinal) || text.StartsWith("_bind_", StringComparison.Ordinal))
		{
			return true;
		}
		return Array.IndexOf(ToolCommands, text) >= 0;
	}

	// True when the server has banned this command for you. Those stay banned.
	internal static bool BannedByServer(string command)
	{
		try
		{
			object obj = instanceProperty?.GetValue(null, null);
			if (obj == null || bannedField == null || !(bannedField.GetValue(obj) is System.Collections.Generic.List<string> list))
			{
				return false;
			}
			string text = command.Trim().ToLowerInvariant();
			foreach (string item in list)
			{
				if (text.StartsWith(item, StringComparison.Ordinal))
				{
					return true;
				}
			}
		}
		catch (Exception)
		{
		}
		return false;
	}

	// Infinity Hammer hides a build menu tool when Server Devcommands says its command is not allowed for you.
	[HarmonyPatch]
	private static class PatchCommandAllowed
	{
		private static bool Prepare()
		{
			return managerType != null && instanceProperty != null && bannedField != null && AccessTools.Method(managerType, "IsCommandAllowed", (Type[])null, (Type[])null) != null;
		}

		private static MethodBase TargetMethod()
		{
			return AccessTools.Method(managerType, "IsCommandAllowed", (Type[])null, (Type[])null);
		}

		private static void Postfix(string commandName, bool remote, ref bool __result)
		{
			if (!__result && !remote && Active && IsToolCommand(commandName) && !BannedByServer(commandName))
			{
				__result = true;
			}
		}
	}

	// ...and running the tool sends that command to the console, which refuses cheat commands without cheat access.
	[HarmonyPatch(typeof(ConsoleCommand), "IsValid")]
	private static class PatchToolCommandValid
	{
		private static void Postfix(ConsoleCommand __instance, Terminal context, ref bool __result)
		{
			if (!__result && !((Object)(object)context == (Object)null) && Active && IsToolCommand(__instance.Command) && !BannedByServer(__instance.Command))
			{
				__result = true;
			}
		}
	}
}
