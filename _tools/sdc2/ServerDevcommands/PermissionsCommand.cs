using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Service;

namespace ServerDevcommands;

public class PermissionsCommand
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static ConsoleEvent _003C0_003E__Handle;

		public static Func<int, int, string[], List<string>> _003C1_003E__GetOperationSpecificHints;
	}

	private static readonly List<string> Operations = new List<string>(11)
	{
		"add_group", "remove_group", "clear_groups", "add_command", "ban_command", "clear_command", "add_feature", "ban_feature", "force_feature", "clear_feature",
		"clear_all"
	};

	private static List<PlayerInfo> FindTargetPlayers(string target)
	{
		try
		{
			return PlayerInfo.FindPlayers(new string[1] { target });
		}
		catch
		{
			return new List<PlayerInfo>();
		}
	}

	private static string JoinArgs(ConsoleEventArgs args, int startIndex)
	{
		if (args.Length <= startIndex)
		{
			return "";
		}
		return string.Join(" ", args.Args.Skip(startIndex)).Trim();
	}

	private static (string Hostname, string CharacterId, string DisplayName) ResolvePlayerTarget(string target)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		string target2 = target;
		List<PlayerInfo> list = FindTargetPlayers(target2);
		if (list.Count > 1)
		{
			throw new InvalidOperationException("Target '" + target2 + "' matched multiple online players.");
		}
		if (list.Count == 1)
		{
			PlayerInfo playerInfo = list[0];
			ZDO zDO = ZDOMan.instance.GetZDO(playerInfo.ZDOID);
			string text = ((zDO != null) ? zDO.GetLong(ZDOVars.s_playerID, 0L).ToString() : null);
			if (text == null)
			{
				throw new InvalidOperationException("Unable to resolve character id for player '" + playerInfo.Name + "'.");
			}
			return (Hostname: playerInfo.HostId, CharacterId: text, DisplayName: playerInfo.Name);
		}
		List<PermissionEntry> list2 = (from entry in PermissionLoader.Data.Entries
			where entry.character == target2 || entry.name.Equals(target2, StringComparison.OrdinalIgnoreCase)
			where !string.IsNullOrWhiteSpace(entry.id) && !string.IsNullOrWhiteSpace(entry.character)
			select entry).ToList();
		if (list2.Count == 0)
		{
			throw new InvalidOperationException("Unable to find player by id/name '" + target2 + "'.");
		}
		if (list2.Count > 1)
		{
			throw new InvalidOperationException("Target '" + target2 + "' is ambiguous. Use a unique character id.");
		}
		PermissionEntry permissionEntry = list2[0];
		string item = (string.IsNullOrWhiteSpace(permissionEntry.name) ? permissionEntry.character : permissionEntry.name);
		return (Hostname: permissionEntry.id, CharacterId: permissionEntry.character, DisplayName: item);
	}

	private static void HandlePlayerEdit(ConsoleEventArgs args, string target, Func<PermissionEntry, bool> update)
	{
		(string Hostname, string CharacterId, string DisplayName) tuple = ResolvePlayerTarget(target);
		string item = tuple.Hostname;
		string item2 = tuple.CharacterId;
		string item3 = tuple.DisplayName;
		string text = PermissionData.PeerKey(item, item2);
		PermissionEntry arg = PermissionLoader.GetOrCreatePeerEntry(item, item2, item3) ?? throw new InvalidOperationException("Unable to create permission entry.");
		bool flag = update(arg);
		if (flag)
		{
			PermissionLoader.Save();
			PermissionLoader.SendPeerPermissions(item, item2);
		}
		Helper.AddMessage(args.Context, flag ? ("Permissions updated for " + item3 + " (" + text + ").") : ("No permission changes for " + item3 + " (" + text + ")."));
	}

	private static void Handle(ConsoleEventArgs args)
	{
		ConsoleEventArgs args2 = args;
		if (args2.Length < 2)
		{
			throw new InvalidOperationException("Missing operation. Use add_group, remove_group, clear_groups, add_command, ban_command, clear_command, add_feature, ban_feature, force_feature, clear_feature, clear_all.");
		}
		if (!ZNet.instance.IsServer())
		{
			ServerExecution.Send(args2);
			return;
		}
		string text = args2[1].ToLowerInvariant();
		switch (text)
		{
		case "add_group":
		{
			Helper.ArgsCheck(args2, 4, "Usage: permissions add_group <group> <character id or name>");
			string group = args2[2].Trim();
			string text3 = JoinArgs(args2, 3);
			if (text3 == "")
			{
				throw new InvalidOperationException("Missing target character id or name.");
			}
			HandlePlayerEdit(args2, text3, (PermissionEntry entry) => PermissionLoader.Data.AddGroup(entry, group));
			break;
		}
		case "remove_group":
		{
			Helper.ArgsCheck(args2, 4, "Usage: permissions remove_group <group> <character id or name>");
			string group2 = args2[2].Trim();
			string text2 = JoinArgs(args2, 3);
			if (text2 == "")
			{
				throw new InvalidOperationException("Missing target character id or name.");
			}
			HandlePlayerEdit(args2, text2, (PermissionEntry entry) => PermissionLoader.Data.RemoveGroup(entry, group2));
			break;
		}
		case "clear_groups":
		{
			Helper.ArgsCheck(args2, 3, "Usage: permissions clear_groups <character id or name>");
			string text4 = JoinArgs(args2, 2);
			if (text4 == "")
			{
				throw new InvalidOperationException("Missing target character id or name.");
			}
			HandlePlayerEdit(args2, text4, (PermissionEntry entry) => PermissionLoader.Data.ClearGroups(entry));
			break;
		}
		case "add_command":
		{
			Helper.ArgsCheck(args2, 4, "Usage: permissions add_command <command> <character id or name>");
			string target8 = JoinArgs(args2, 3);
			HandlePlayerEdit(args2, target8, (PermissionEntry entry) => PermissionLoader.Data.SetCommandPermission(entry, args2[2], PermissionManager.FeaturePermission.Yes));
			break;
		}
		case "ban_command":
		{
			Helper.ArgsCheck(args2, 4, "Usage: permissions ban_command <command> <character id or name>");
			string target7 = JoinArgs(args2, 3);
			HandlePlayerEdit(args2, target7, (PermissionEntry entry) => PermissionLoader.Data.SetCommandPermission(entry, args2[2], PermissionManager.FeaturePermission.No));
			break;
		}
		case "clear_command":
		{
			Helper.ArgsCheck(args2, 4, "Usage: permissions clear_command <command> <character id or name>");
			string target6 = JoinArgs(args2, 3);
			HandlePlayerEdit(args2, target6, (PermissionEntry entry) => PermissionLoader.Data.ClearCommand(entry, args2[2]));
			break;
		}
		case "add_feature":
		{
			Helper.ArgsCheck(args2, 5, "Usage: permissions add_feature <section> <feature> <character id or name>");
			string target5 = JoinArgs(args2, 4);
			HandlePlayerEdit(args2, target5, (PermissionEntry entry) => PermissionLoader.Data.SetFeaturePermission(entry, args2[2], args2[3], PermissionManager.FeaturePermission.Yes));
			break;
		}
		case "ban_feature":
		{
			Helper.ArgsCheck(args2, 5, "Usage: permissions ban_feature <section> <feature> <character id or name>");
			string target4 = JoinArgs(args2, 4);
			HandlePlayerEdit(args2, target4, (PermissionEntry entry) => PermissionLoader.Data.SetFeaturePermission(entry, args2[2], args2[3], PermissionManager.FeaturePermission.No));
			break;
		}
		case "force_feature":
		{
			Helper.ArgsCheck(args2, 5, "Usage: permissions force_feature <section> <feature> <character id or name>");
			string target3 = JoinArgs(args2, 4);
			HandlePlayerEdit(args2, target3, (PermissionEntry entry) => PermissionLoader.Data.SetFeaturePermission(entry, args2[2], args2[3], PermissionManager.FeaturePermission.Force));
			break;
		}
		case "clear_feature":
		{
			Helper.ArgsCheck(args2, 5, "Usage: permissions clear_feature <section> <feature> <character id or name>");
			string target2 = JoinArgs(args2, 4);
			HandlePlayerEdit(args2, target2, (PermissionEntry entry) => PermissionLoader.Data.ClearFeature(entry, args2[2], args2[3]));
			break;
		}
		case "clear_all":
		{
			Helper.ArgsCheck(args2, 3, "Usage: permissions clear_all <character id or name>");
			string target = JoinArgs(args2, 2);
			HandlePlayerEdit(args2, target, (PermissionEntry entry) => PermissionLoader.Data.ClearAll(entry));
			break;
		}
		default:
			throw new InvalidOperationException("Unknown operation '" + text + "'.");
		}
	}

	private static List<string> GetOperationSpecificHints(int index, int _, string[] args)
	{
		if (index == 0)
		{
			return Operations;
		}
		switch ((args.Length != 0) ? args[0].Trim().ToLowerInvariant() : "")
		{
		case "add_group":
		case "remove_group":
			if (index == 1)
			{
				return ParameterInfo.Create("Group name");
			}
			if (index >= 2)
			{
				return ParameterInfo.PlayerNames;
			}
			return ParameterInfo.None;
		case "clear_groups":
		case "clear_all":
			if (index >= 1)
			{
				return ParameterInfo.PlayerNames;
			}
			return ParameterInfo.None;
		case "add_command":
		case "ban_command":
		case "clear_command":
			if (index == 1)
			{
				return ParameterInfo.Create("Command name");
			}
			if (index >= 2)
			{
				return ParameterInfo.PlayerNames;
			}
			return ParameterInfo.None;
		case "add_feature":
		case "ban_feature":
		case "force_feature":
		case "clear_feature":
			if (index == 1)
			{
				return ParameterInfo.Create("Feature section");
			}
			if (index == 2)
			{
				return ParameterInfo.Create("Feature name");
			}
			if (index >= 3)
			{
				return ParameterInfo.PlayerNames;
			}
			return ParameterInfo.None;
		default:
			return ParameterInfo.Create("Unknown operation. Use: add_group, remove_group, clear_groups, add_command, ban_command, clear_command, add_feature, ban_feature, force_feature, clear_feature, clear_all");
		}
	}

	public PermissionsCommand()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		object obj = _003C_003EO._003C0_003E__Handle;
		if (obj == null)
		{
			ConsoleEvent val = Handle;
			_003C_003EO._003C0_003E__Handle = val;
			obj = (object)val;
		}
		Helper.Command("permissions", "[operation] - Manage player permission overrides.", (ConsoleEvent)obj);
		AutoComplete.Register("permissions", GetOperationSpecificHints);
	}
}
