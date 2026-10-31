using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerDevcommands;

public class PermissionApi
{
	private const string EveryoneGroup = "Everyone";

	public static event Action? PermissionsUpdated;

	public static bool IsFeatureEnabled(string section, string feature, bool localConfigValue)
	{
		return IsFeatureEnabledByHash(section, StringExtensionMethods.GetStableHashCode(feature.ToLower()), localConfigValue);
	}

	public static bool IsFeatureEnabledByHash(string section, int featureHash, bool localConfigValue)
	{
		return PermissionManager.Instance.IsFeatureEnabledByHash(section, featureHash, localConfigValue);
	}

	public static bool IsCommandAllowed(string commandName)
	{
		string cmdName = Parse.Kvp(commandName, ' ').Key.ToLower();
		ConsoleCommand value = Terminal.commands.FirstOrDefault((KeyValuePair<string, ConsoleCommand> c) => c.Key.Equals(cmdName, StringComparison.OrdinalIgnoreCase)).Value;
		if (value == null)
		{
			return false;
		}
		return PermissionManager.Instance.IsCommandAllowed(value, commandName, remote: false);
	}

	public static bool HasGroup(string playerId, long characterId, string group)
	{
		group = group?.Trim() ?? "";
		if (group == "")
		{
			return false;
		}
		if (group.Equals("Everyone", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		PermissionData data = PermissionLoader.Data;
		string characterId2 = characterId.ToString();
		return data.HasGroup(playerId, characterId2, group);
	}

	public static void Subscribe(Action handler)
	{
		PermissionsUpdated += handler;
	}

	public static void Unsubscribe(Action handler)
	{
		PermissionsUpdated -= handler;
	}

	internal static void Notify()
	{
		PermissionApi.PermissionsUpdated?.Invoke();
	}
}
