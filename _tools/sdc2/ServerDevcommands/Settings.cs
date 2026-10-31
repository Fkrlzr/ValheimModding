using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using UnityEngine;

namespace ServerDevcommands;

public static class Settings
{
	public enum PickUpMode
	{
		Default,
		On,
		Off
	}

	public static ConfigEntry<bool> configMapCoordinates;

	public static ConfigEntry<bool> configMiniMapCoordinates;

	public static ConfigEntry<bool> configShowAlternativeBiomes;

	public static ConfigEntry<bool> configShowPrivatePlayers;

	public static ConfigEntry<bool> configAutoDevcommands;

	public static ConfigEntry<bool> configDebugModeFastTeleport;

	public static ConfigEntry<bool> configAutoDebugMode;

	public static ConfigEntry<bool> configKillDestroySpawners;

	public static ConfigEntry<bool> configAutoGodMode;

	public static ConfigEntry<bool> configDisableNoMap;

	public static ConfigEntry<bool> configAutoGhostMode;

	public static ConfigEntry<PickUpMode> configAutomaticItemPickUp;

	public static ConfigEntry<bool> configAutoNoCost;

	public static ConfigEntry<bool> configDisableEvents;

	public static ConfigEntry<bool> configDisableUnlockMessages;

	public static ConfigEntry<bool> configDisableDebugModeKeys;

	public static ConfigEntry<bool> configAutoFly;

	public static ConfigEntry<string> configAutoTod;

	public static ConfigEntry<string> configAutoEnv;

	public static ConfigEntry<bool> configDisableMessages;

	public static ConfigEntry<bool> configDisableHelpCommand;

	public static ConfigEntry<bool> configGodModeNoWeightLimit;

	public static ConfigEntry<bool> configGodModeNoStamina;

	public static ConfigEntry<bool> configGodModeNoEitr;

	public static ConfigEntry<bool> configGodModeNoUsage;

	public static ConfigEntry<bool> configGodModeAlwaysDodge;

	public static ConfigEntry<bool> configGodModeAlwaysParry;

	public static ConfigEntry<bool> configGodModeNoStagger;

	public static ConfigEntry<bool> configHideShoutPings;

	public static ConfigEntry<bool> configGodModeNoEdgeOfWorld;

	public static ConfigEntry<bool> configNoCostLimitRecipesToStation;

	public static ConfigEntry<bool> configNoCostRespectStationLevel;

	public static ConfigEntry<bool> configDisableStartShout;

	public static ConfigEntry<bool> configAccessPrivateChests;

	public static ConfigEntry<bool> configAccessWardedAreas;

	public static ConfigEntry<bool> configFlyNoClip;

	public static ConfigEntry<bool> configNoClipClearEnvironment;

	public static ConfigEntry<bool> configGodModeNoKnockback;

	public static ConfigEntry<bool> configGodModeNoMist;

	public static ConfigEntry<bool> configAliasing;

	public static ConfigEntry<string> configSubstitution;

	public static ConfigEntry<string> configWrapping;

	public static ConfigEntry<bool> configImprovedAutoComplete;

	public static ConfigEntry<bool> configMultiCommand;

	public static ConfigEntry<bool> configGhostInvisibility;

	public static ConfigEntry<bool> configGhostNoSpawns;

	public static ConfigEntry<bool> configServerChat;

	public static ConfigEntry<string> configServerChatName;

	public static ConfigEntry<bool> configGhostIgnoreSleep;

	public static ConfigEntry<string> configFlyUpKeys;

	public static List<KeyCode> FlyUpRequiredKeys = new List<KeyCode>();

	public static List<KeyCode> FlyUpBannedKeys = new List<KeyCode>();

	public static ConfigEntry<string> configFlyDownKeys;

	public static List<KeyCode> FlyDownRequiredKeys = new List<KeyCode>();

	public static List<KeyCode> FlyDownBannedKeys = new List<KeyCode>();

	public static ConfigEntry<bool> configFreeFlyInvertCamera;

	public static ConfigEntry<bool> configNoDrops;

	public static ConfigEntry<bool> configNoClipView;

	public static ConfigEntry<bool> configImprovedChat;

	public static ConfigEntry<bool> configDisableCheatTracking;

	public static ConfigEntry<KeyboardShortcut> configMapTeleport;

	public static ConfigEntry<string> configAutoExecBoot;

	public static ConfigEntry<string> configAutoExec;

	public static ConfigEntry<string> configAutoExecDevOn;

	public static ConfigEntry<string> configAutoExecDevOff;

	public static ConfigEntry<bool> configCommandDescriptions;

	public static ConfigEntry<string> configDisabledGlobalKeys;

	public static ConfigEntry<string> configUndoLimit;

	public static ConfigEntry<string> configPlayerListFormat;

	public static ConfigEntry<string> configCommandLogFormat;

	public static ConfigEntry<string> configFindFormat;

	public static ConfigEntry<string> configMinimapFormat;

	public static ConfigEntry<bool> configChatOutput;

	public static List<string> Options = new List<string>(66)
	{
		"access_private_chests", "access_warded_areas", "map_coordinates", "private_players", "auto_devcommands", "auto_debugmode", "auto_fly", "god_no_stagger", "auto_nocost", "auto_ghost",
		"auto_god", "debug_console", "no_drops", "disable_cheat_tracking", "aliasing", "god_no_stamina", "substitution", "wrapping", "improved_autocomplete", "disable_events",
		"disable_warnings", "multiple_commands", "god_no_knockback", "ghost_invibisility", "auto_exec_dev_on", "auto_exec_dev_off", "auto_exec_boot", "auto_exec", "command_descriptions", "server_commands",
		"fly_no_clip", "disable_command", "minimap_coordinates", "alternative_biomes", "disable_global_key", "disable_debug_mode_keys", "god_always_parry", "god_always_dodge", "fly_up_key", "fly_down_key",
		"disable_start_shout", "mouse_wheel_bind_key", "god_no_weight_limit", "automatic_item_pick_up", "disable_messages", "disable_help", "god_no_edge", "no_clip_clear_environment", "max_undo_steps", "best_command_match",
		"debug_fast_teleport", "disable_no_map", "hide_shout_pings", "disable_unlock_messages", "no_clip_view", "god_no_eitr", "god_no_item", "players_format", "command_log_format", "minimap_format",
		"chat_output", "free_fly_camera_invert", "server_chat", "server_chat_name", "limit_recipes_to_station", "respect_station_level"
	};

	private static readonly HashSet<string> Truthies = new HashSet<string> { "1", "t", "true", "yes", "on" };

	private static readonly HashSet<string> Falsies = new HashSet<string> { "0", "f", "false", "no", "off" };

	public static bool MapCoordinates => IsEnabled(PermissionHash.MapCoordinates, configMapCoordinates.Value);

	public static bool MiniMapCoordinates => IsEnabled(PermissionHash.MiniMapCoordinates, configMiniMapCoordinates.Value);

	public static bool ShowAlternativeBiomes => IsEnabled(PermissionHash.AlternativeBiomes, configShowAlternativeBiomes.Value);

	public static bool ShowPrivatePlayers => IsEnabled(PermissionHash.ShowPrivatePlayers, configShowPrivatePlayers.Value);

	public static bool AutoDevcommands => configAutoDevcommands.Value;

	public static bool DebugModeFastTeleport => configDebugModeFastTeleport.Value;

	public static bool AutoDebugMode => configAutoDebugMode.Value;

	public static bool KillDestroySpawners => configKillDestroySpawners.Value;

	public static bool AutoGodMode => configAutoGodMode.Value;

	public static bool DisableNoMap => IsEnabled(PermissionHash.IgnoreNoMap, configDisableNoMap.Value);

	public static bool AutoGhostMode => configAutoGhostMode.Value;

	public static bool AutoNoCost => configAutoNoCost.Value;

	public static bool DisableEvents => configDisableEvents.Value;

	public static bool DisableUnlockMessages => configDisableUnlockMessages.Value;

	public static bool DisableDebugModeKeys => configDisableDebugModeKeys.Value;

	public static bool AutoFly => configAutoFly.Value;

	public static string AutoTod => configAutoTod.Value;

	public static string AutoEnv => configAutoEnv.Value;

	public static bool DisableMessages => configDisableMessages.Value;

	public static bool DisableHelpCommand => configDisableHelpCommand.Value;

	public static bool GodModeNoWeightLimit => configGodModeNoWeightLimit.Value;

	public static bool GodModeNoStamina => configGodModeNoStamina.Value;

	public static bool GodModeNoEitr => configGodModeNoEitr.Value;

	public static bool GodModeNoUsage => configGodModeNoUsage.Value;

	public static bool GodModeAlwaysDodge => configGodModeAlwaysDodge.Value;

	public static bool GodModeAlwaysParry => configGodModeAlwaysParry.Value;

	public static bool GodModeNoStagger => configGodModeNoStagger.Value;

	public static bool HideShoutPings => IsEnabled(PermissionHash.HideShoutPings, configHideShoutPings.Value);

	public static bool NoCostLimitRecipesToStation => configNoCostLimitRecipesToStation.Value;

	public static bool NoCostRespectStationLevel => configNoCostRespectStationLevel.Value;

	public static bool GodModeNoEdgeOfWorld => configGodModeNoEdgeOfWorld.Value;

	public static bool DisableStartShout => configDisableStartShout.Value;

	public static bool AccessPrivateChests => IsEnabled(PermissionHash.IgnoreWards, configAccessPrivateChests.Value);

	public static bool AccessWardedAreas => IsEnabled(PermissionHash.IgnoreWards, configAccessWardedAreas.Value);

	public static bool FlyNoClip => configFlyNoClip.Value;

	public static bool NoClipClearEnvironment => configNoClipClearEnvironment.Value;

	public static bool GodModeNoKnockback => configGodModeNoKnockback.Value;

	public static bool GodModeNoMist => configGodModeNoMist.Value;

	public static bool Aliasing => configAliasing.Value;

	public static string Substitution => configSubstitution.Value;

	public static string Wrapping => configWrapping.Value;

	public static bool ImprovedAutoComplete => configImprovedAutoComplete.Value;

	public static bool MultiCommand => configMultiCommand.Value;

	public static bool GhostInvisibility => configGhostInvisibility.Value;

	public static bool GhostNoSpawns => configGhostNoSpawns.Value;

	public static bool IsServerChat => configServerChat.Value;

	public static string ServerChatName => configServerChatName.Value;

	public static bool GhostIgnoreSleep => configGhostIgnoreSleep.Value;

	public static bool FreeFlyCameraInvert => configFreeFlyInvertCamera.Value;

	public static bool NoDrops => IsEnabled(PermissionHash.NoDrops, configNoDrops.Value);

	public static bool NoClipView => IsEnabled(PermissionHash.NoClipCamera, configNoClipView.Value);

	public static bool ImprovedChat => configImprovedChat.Value;

	public static bool DisableCheatTracking => configDisableCheatTracking.Value;

	public static KeyboardShortcut MapTeleport => configMapTeleport.Value;

	public static string AutoExecBoot => configAutoExecBoot.Value;

	public static string AutoExec => configAutoExec.Value;

	public static string AutoExecDevOn => configAutoExecDevOn.Value;

	public static string AutoExecDevOff => configAutoExecDevOff.Value;

	public static bool CommandDescriptions => configCommandDescriptions.Value;

	public static int UndoLimit => Parse.Int(configUndoLimit.Value, 50);

	public static HashSet<string> DisabledGlobalKeys => ParseList(configDisabledGlobalKeys.Value);

	public static string PlayerListFormat => configPlayerListFormat.Value;

	public static string CommandLogFormat => configCommandLogFormat.Value;

	public static string FindFormat => configFindFormat.Value;

	public static string MinimapFormat => configMinimapFormat.Value;

	public static bool ChatOutput => configChatOutput.Value;

	public static bool IsEnabled(int hash, bool localValue)
	{
		return PermissionManager.Instance.IsFeatureEnabledByHash("serverdevcommands", hash, localValue);
	}

	public static bool AutomaticItemPickUp(bool current)
	{
		return configAutomaticItemPickUp.Value switch
		{
			PickUpMode.On => true, 
			PickUpMode.Off => false, 
			_ => current, 
		};
	}

	private static HashSet<string> ParseList(string value)
	{
		HashSet<string> hashSet = new HashSet<string>();
		foreach (string item in from s in Parse.Split(value)
			select s.ToLower())
		{
			hashSet.Add(item);
		}
		return hashSet;
	}

	public static bool IsGlobalKeyDisabled(string key)
	{
		return DisabledGlobalKeys.Contains(key.ToLower());
	}

	public static string Format(string format)
	{
		return format.Replace("{player_id", "{0").Replace("{character_name", "{1").Replace("{character_id", "{2")
			.Replace("{pos_x", "{3")
			.Replace("{pos_y", "{4")
			.Replace("{pos_z", "{5");
	}

	public static void Init(ConfigFile config)
	{
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		string text = "1. General";
		configGhostInvisibility = config.Bind<bool>(text, "Invisible to other players with ghost mode", false, "");
		configGhostNoSpawns = config.Bind<bool>(text, "Disables spawns with ghost mode", false, "");
		configGhostIgnoreSleep = config.Bind<bool>(text, "Ignores sleep check with ghost mode", false, "");
		configServerChat = config.Bind<bool>(text, "Server chat", false, "Adds the server as a fake player to allow server to send chat messages.");
		configServerChatName = config.Bind<string>(text, "Server chat name", "Server", "Name of the server chat player.");
		configServerChatName.SettingChanged += delegate
		{
			ServerChat.RefreshPlayerInfo();
		};
		configNoDrops = config.Bind<bool>(text, "No creature drops", false, "Disables drops from creatures (if you control the zone), intended to fix high star enemies crashing the game.");
		configNoClipView = config.Bind<bool>(text, "No clip view", false, "Removes collision check for the camera.");
		configAutoDebugMode = config.Bind<bool>(text, "Automatic debug mode", false, "Automatically enables debug mode when enabling devcommands.");
		configKillDestroySpawners = config.Bind<bool>(text, "Kill destroys spawners", true, "Destroys spawners when using kill commands");
		configAutoFly = config.Bind<bool>(text, "Automatic fly mode", false, "Automatically enables fly mode when enabling devcommands.");
		configAutoEnv = config.Bind<string>(text, "Automatic environment", "", "Automatically enables environment when enabling devcommands (for example clear).");
		configAutoTod = config.Bind<string>(text, "Automatic time of day", "", "Automatically enables time of day when enabling devcommands (for example 0.33).");
		configAutoNoCost = config.Bind<bool>(text, "Automatic no cost mode", false, "Automatically enables no cost mode when enabling devcommands.");
		configAutoGodMode = config.Bind<bool>(text, "Automatic god mode", false, "Automatically enables god mode when enabling devcommands.");
		configAutoGhostMode = config.Bind<bool>(text, "Automatic ghost mode", false, "Automatically enables ghost mode when enabling devcommands.");
		configAutomaticItemPickUp = config.Bind<PickUpMode>(text, "Automatic item pick up", PickUpMode.Default, "Sets the automatic item pick up feature. Default keeps the game's current value.");
		configAutomaticItemPickUp.SettingChanged += delegate
		{
			if (Object.op_Implicit((Object)(object)Player.m_localPlayer))
			{
				Player.m_enableAutoPickup = AutomaticItemPickUp(Player.m_enableAutoPickup);
			}
		};
		configAutoDevcommands = config.Bind<bool>(text, "Automatic devcommands", true, "Automatically enables devcommands when joining servers.");
		configDebugModeFastTeleport = config.Bind<bool>(text, "Debug mode fast teleport", true, "All teleporting is much faster with the debug mode.");
		configDisableNoMap = config.Bind<bool>(text, "Disable no map", false, "Disables no map having effect.");
		configDisableNoMap.SettingChanged += delegate
		{
			Game.UpdateNoMap();
		};
		configGodModeNoStamina = config.Bind<bool>(text, "No stamina usage with god mode", true, "");
		configGodModeNoEitr = config.Bind<bool>(text, "No eitr usage with god mode", true, "");
		configGodModeNoUsage = config.Bind<bool>(text, "No item usage with god mode", false, "");
		configGodModeNoWeightLimit = config.Bind<bool>(text, "No weight limit with god mode", false, "");
		configGodModeAlwaysDodge = config.Bind<bool>(text, "Always dodge with god mode", false, "");
		configGodModeAlwaysParry = config.Bind<bool>(text, "Always parry with god mode (when not blocking)", false, "");
		configGodModeNoStagger = config.Bind<bool>(text, "No staggering with god mode", true, "");
		configHideShoutPings = config.Bind<bool>(text, "Hide shout pings", false, "Forces shout pings at the world center.");
		configNoCostLimitRecipesToStation = config.Bind<bool>(text, "Limit recipes to station with nocost mode", true, "When nocost mode is active, only show recipes craftable at your current crafting station.");
		configNoCostRespectStationLevel = config.Bind<bool>(text, "Respect station level with nocost mode", false, "When limiting recipes to station, also require the station's upgrade level.");
		configGodModeNoEdgeOfWorld = config.Bind<bool>(text, "No edge of world pull with god mode", true, "");
		configDisableStartShout = config.Bind<bool>(text, "Disable start shout", false, "Removes the initial shout message when joining the server.");
		configAccessPrivateChests = config.Bind<bool>(text, "Access private chests", true, "Allows opening private chests.");
		configAccessWardedAreas = config.Bind<bool>(text, "Access warded areas", true, "Allows accessing warded areas.");
		configFlyNoClip = config.Bind<bool>(text, "No clip with fly mode", false, "");
		configFreeFlyInvertCamera = config.Bind<bool>(text, "Free fly uses camera invert", true, "Makes free fly to use camera invert settings.");
		configNoClipClearEnvironment = config.Bind<bool>(text, "No clip clears forced environments", true, "Removes any forced environments when the noclip is enabled. This disables any dark dungeon environments and prevents them from staying on when exiting the dungeon.");
		configGodModeNoKnockback = config.Bind<bool>(text, "No knockback with god mode", true, "");
		configGodModeNoMist = config.Bind<bool>(text, "No Mistlands mist with god mode", false, "");
		configMapCoordinates = config.Bind<bool>(text, "Show map coordinates", true, "The map shows coordinates on hover.");
		configMiniMapCoordinates = config.Bind<bool>(text, "Show minimap coordinates", false, "The minimap shows player coordinates.");
		configShowAlternativeBiomes = config.Bind<bool>(text, "Show alternative biomes", false, "The map shows alternative biomes in the current zone.");
		configShowPrivatePlayers = config.Bind<bool>(text, "Show private players", false, "The map shows private players.");
		configDisableEvents = config.Bind<bool>(text, "Disable random events", false, "Disables random events (server side setting).");
		configDisableUnlockMessages = config.Bind<bool>(text, "Disable unlock messages", false, "Disables messages about new pieces and items.");
		configDisableDebugModeKeys = config.Bind<bool>(text, "Disable debug mode keys", false, "Removes debug mode key bindings for killall, removedrops, fly and no cost.");
		configDisabledGlobalKeys = config.Bind<string>(text, "Disabled global keys", "", "Global keys separated by , that won't be set (server side setting).");
		configDisabledGlobalKeys.SettingChanged += delegate
		{
			DisableGlobalKeys.RemoveDisabled();
		};
		configUndoLimit = config.Bind<string>(text, "Max undo steps", "50", "How many undo actions are stored.");
		configUndoLimit.SettingChanged += delegate
		{
			UndoManager.MaxSteps = UndoLimit;
		};
		UndoManager.MaxSteps = UndoLimit;
		text = "2. Console";
		configDisableMessages = config.Bind<bool>(text, "Disable messages", false, "Prevents messages from commands.");
		configDisableHelpCommand = config.Bind<bool>(text, "Disable help command", false, "Disables the built-in help command.");
		configMapTeleport = config.Bind<KeyboardShortcut>(text, "Map teleport bind key", new KeyboardShortcut((KeyCode)325, (KeyCode[])(object)new KeyCode[1] { (KeyCode)306 }), "Key bind for map teleport.");
		configAutoExecBoot = config.Bind<string>(text, "Auto exec boot", "", "Executes the given command when starting the game.");
		configAutoExecDevOn = config.Bind<string>(text, "Auto exec dev on", "", "Executes the given command when enabling devcommands.");
		configAutoExecDevOff = config.Bind<string>(text, "Auto exec dev off", "", "Executes the given command when disabling devcommands.");
		configAutoExec = config.Bind<string>(text, "Auto exec", "", "Executes the given command when joining a server (before admin is checked).");
		configCommandDescriptions = config.Bind<bool>(text, "Command descriptions", true, "Shows command descriptions as autocomplete.");
		configAliasing = config.Bind<bool>(text, "Alias system", true, "Enables the command aliasing system (allows creating new commands).");
		configImprovedAutoComplete = config.Bind<bool>(text, "Improved autocomplete", true, "Enables parameter info or options for every parameter.");
		configMultiCommand = config.Bind<bool>(text, "Multiple commands per line", true, "Enables multiple commands when separated with ;.");
		configImprovedChat = config.Bind<bool>(text, "Improved chat", true, "Enables alias and multicommands system for chat.");
		configDisableCheatTracking = config.Bind<bool>(text, "Disable cheat tracking", true, "Prevents commands from marking the character as having used cheats.");
		configSubstitution = config.Bind<string>(text, "Substitution", "$$", "Enables the command parameter substitution system (substitution gets replaced with the next free parameter).");
		configWrapping = config.Bind<string>(text, "Wrapping", "\"", "Allows using space bars in command parameters.");
		configFlyUpKeys = config.Bind<string>(text, "Key for fly up", "Space", "Key codes separated by ,");
		configFlyUpKeys.SettingChanged += delegate
		{
			ParseFlyUp();
		};
		ParseFlyUp();
		configFlyDownKeys = config.Bind<string>(text, "Key for fly down", "LeftControl", "Key codes separated by ,");
		configFlyDownKeys.SettingChanged += delegate
		{
			ParseFlyDown();
		};
		ParseFlyDown();
		configChatOutput = config.Bind<bool>(text, "Chat output", false, "Sends messages to the chat window from bound keys.");
		text = "3. Formatting";
		configPlayerListFormat = config.Bind<string>(text, "Player list format", "{player_id}/{character_name}/{character_id} ({pos_x:F0}, {pos_z:F0}, {pos_y:F0})", "Format of playerlist command.");
		configCommandLogFormat = config.Bind<string>(text, "Command log format", "{player_id}/{character_name} ({pos_x:F0}, {pos_z:F0}, {pos_y:F0}): {command}", "Format for the command log.");
		configFindFormat = config.Bind<string>(text, "Find format", "{pos_x:F0}, {pos_z:F0}, {pos_y:F0}, distance {distance:F0} ({name})", "Format for the find command. Server side setting.");
		configMinimapFormat = config.Bind<string>(text, "Minimap format", "x: {pos_x:F0}, z: {pos_z:F0}, y: {pos_y:F0}", "Format for minimap coordinates.");
	}

	private static void ParseFlyUp()
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		FlyUpRequiredKeys.Clear();
		FlyUpBannedKeys.Clear();
		string[] array = Parse.Split(configFlyUpKeys.Value);
		foreach (string text in array)
		{
			KeyCode result2;
			if (text.StartsWith("-"))
			{
				if (Enum.TryParse<KeyCode>(text.Substring(1), ignoreCase: true, out KeyCode result))
				{
					FlyUpBannedKeys.Add(result);
				}
			}
			else if (Enum.TryParse<KeyCode>(text, ignoreCase: true, out result2))
			{
				FlyUpRequiredKeys.Add(result2);
			}
		}
	}

	private static void ParseFlyDown()
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		FlyDownRequiredKeys.Clear();
		FlyDownBannedKeys.Clear();
		string[] array = Parse.Split(configFlyDownKeys.Value);
		foreach (string text in array)
		{
			KeyCode result2;
			if (text.StartsWith("-"))
			{
				if (Enum.TryParse<KeyCode>(text.Substring(1), ignoreCase: true, out KeyCode result))
				{
					FlyDownBannedKeys.Add(result);
				}
			}
			else if (Enum.TryParse<KeyCode>(text, ignoreCase: true, out result2))
			{
				FlyDownRequiredKeys.Add(result2);
			}
		}
	}

	private static string State(bool value)
	{
		if (!value)
		{
			return "disabled";
		}
		return "enabled";
	}

	private static string Flag(bool value)
	{
		if (!value)
		{
			return "Added";
		}
		return "Removed";
	}

	private static bool IsTruthy(string value)
	{
		return Truthies.Contains(value);
	}

	private static bool IsFalsy(string value)
	{
		return Falsies.Contains(value);
	}

	private static void Toggle(Terminal context, ConfigEntry<bool> setting, string name, string value, bool reverse = false)
	{
		if (value == "")
		{
			setting.Value = !setting.Value;
		}
		else if (IsTruthy(value))
		{
			setting.Value = true;
		}
		else if (IsFalsy(value))
		{
			setting.Value = false;
		}
		Helper.AddMessage(context, name + " " + State(reverse ? (!setting.Value) : setting.Value) + ".");
	}

	private static void ToggleFlag(Terminal context, ConfigEntry<string> setting, string name, string value)
	{
		if (value == "")
		{
			Helper.AddMessage(context, name + ": " + setting.Value + ".");
			return;
		}
		HashSet<string> hashSet = ParseList(setting.Value);
		foreach (string item in ParseList(value))
		{
			bool flag = hashSet.Contains(item);
			if (flag)
			{
				hashSet.Remove(item);
			}
			else
			{
				hashSet.Add(item);
			}
			setting.Value = string.Join(",", hashSet);
			Helper.AddMessage(context, name + ": " + Flag(flag) + " " + item + ".");
		}
	}

	private static void SetValue(Terminal context, ConfigEntry<string> setting, string name, string value)
	{
		if (value == "")
		{
			Helper.AddMessage(context, name + ": " + setting.Value + ".");
			return;
		}
		if (value.Contains("/"))
		{
			string[] array = value.Split('/');
			value = array[0];
			for (int i = 0; i < array.Length - 1; i++)
			{
				if (string.Equals(setting.Value?.ToString(), array[i], StringComparison.OrdinalIgnoreCase))
				{
					value = array[i + 1];
					break;
				}
			}
		}
		setting.Value = value;
		Helper.AddMessage(context, name + " set to " + value + ".");
	}

	private static void SetPickUp(Terminal context, string value)
	{
		PickUpMode result;
		if (value == "")
		{
			configAutomaticItemPickUp.Value = ((!Player.m_enableAutoPickup) ? PickUpMode.On : PickUpMode.Off);
		}
		else if (IsTruthy(value))
		{
			configAutomaticItemPickUp.Value = PickUpMode.On;
		}
		else if (IsFalsy(value))
		{
			configAutomaticItemPickUp.Value = PickUpMode.Off;
		}
		else if (Enum.TryParse<PickUpMode>(value, ignoreCase: true, out result))
		{
			configAutomaticItemPickUp.Value = result;
		}
		Helper.AddMessage(context, $"Automatic item pick up: {configAutomaticItemPickUp.Value}.");
	}

	public static void UpdateValue(Terminal context, string key, string value)
	{
		if (key == "no_clip_view")
		{
			Toggle(context, configNoClipView, key, value);
		}
		if (key == "fly_up_key")
		{
			SetValue(context, configFlyUpKeys, key, value);
		}
		if (key == "fly_down_key")
		{
			SetValue(context, configFlyDownKeys, key, value);
		}
		if (key == "max_undo_steps")
		{
			SetValue(context, configUndoLimit, key, value);
		}
		if (key == "auto_exec_dev_on")
		{
			SetValue(context, configAutoExecDevOn, key, value);
		}
		if (key == "auto_exec_dev_off")
		{
			SetValue(context, configAutoExecDevOff, key, value);
		}
		if (key == "auto_exec_boot")
		{
			SetValue(context, configAutoExecBoot, key, value);
		}
		if (key == "auto_exec")
		{
			SetValue(context, configAutoExec, key, value);
		}
		if (key == "substitution")
		{
			SetValue(context, configSubstitution, key, value);
		}
		if (key == "wrapping")
		{
			SetValue(context, configWrapping, key, value);
		}
		if (key == "auto_tod")
		{
			SetValue(context, configAutoTod, key, value);
		}
		if (key == "auto_env")
		{
			SetValue(context, configAutoEnv, key, value);
		}
		if (key == "debug_fast_teleport")
		{
			Toggle(context, configDebugModeFastTeleport, key, value);
		}
		if (key == "improved_chat")
		{
			Toggle(context, configImprovedChat, key, value);
		}
		if (key == "disable_cheat_tracking")
		{
			Toggle(context, configDisableCheatTracking, key, value);
		}
		if (key == "access_private_chests")
		{
			Toggle(context, configAccessPrivateChests, key, value);
		}
		if (key == "access_warded_areas")
		{
			Toggle(context, configAccessWardedAreas, key, value);
		}
		if (key == "no_clip_clear_environment")
		{
			Toggle(context, configNoClipClearEnvironment, key, value);
		}
		if (key == "disable_messages")
		{
			Toggle(context, configDisableMessages, "Command messages", value, reverse: true);
		}
		if (key == "disable_help")
		{
			Toggle(context, configDisableHelpCommand, "Help command", value, reverse: true);
		}
		if (key == "automatic_item_pick_up")
		{
			SetPickUp(context, value);
		}
		if (key == "command_descriptions")
		{
			Toggle(context, configCommandDescriptions, "Command descriptions", value);
		}
		if (key == "map_coordinates")
		{
			Toggle(context, configMapCoordinates, "Map coordinates", value);
		}
		if (key == "minimap_coordinates")
		{
			Toggle(context, configMiniMapCoordinates, "Minimap coordinates", value);
		}
		if (key == "alternative_biomes")
		{
			Toggle(context, configShowAlternativeBiomes, "Alternative biomes", value);
		}
		if (key == "private_players")
		{
			Toggle(context, configShowPrivatePlayers, "Private players", value);
		}
		if (key == "auto_devcommands")
		{
			Toggle(context, configAutoDevcommands, "Automatic devcommands", value);
		}
		if (key == "auto_debugmode")
		{
			Toggle(context, configAutoDebugMode, "Automatic debug mode", value);
		}
		if (key == "auto_fly")
		{
			Toggle(context, configAutoFly, "Automatic fly", value);
		}
		if (key == "auto_nocost")
		{
			Toggle(context, configAutoNoCost, "Automatic no cost", value);
		}
		if (key == "auto_god")
		{
			Toggle(context, configAutoGodMode, "Automatic god mode", value);
		}
		if (key == "auto_ghost")
		{
			Toggle(context, configAutoGhostMode, "Automatic ghost mode", value);
		}
		if (key == "no_drops")
		{
			Toggle(context, configNoDrops, "Creature drops", value, reverse: true);
		}
		if (key == "aliasing")
		{
			Toggle(context, configAliasing, "Command aliasing", value);
		}
		if (key == "improved_autocomplete")
		{
			Toggle(context, configImprovedAutoComplete, "Improved autocomplete", value);
		}
		if (key == "disable_unlock_messages")
		{
			Toggle(context, configDisableUnlockMessages, "Unlock messages", value, reverse: true);
		}
		if (key == "disable_events")
		{
			Toggle(context, configDisableEvents, "Random events", value, reverse: true);
		}
		if (key == "multiple_commands")
		{
			Toggle(context, configMultiCommand, "Multiple commands per line", value);
		}
		if (key == "god_no_stamina")
		{
			Toggle(context, configGodModeNoStamina, "Stamina usage with god mode", value, reverse: true);
		}
		if (key == "god_no_eitr")
		{
			Toggle(context, configGodModeNoEitr, "Eitr usage with god mode", value, reverse: true);
		}
		if (key == "god_no_item")
		{
			Toggle(context, configGodModeNoUsage, "Item usage with god mode", value, reverse: true);
		}
		if (key == "god_no_weight_limit")
		{
			Toggle(context, configGodModeNoWeightLimit, "Weight limit with god mode", value, reverse: true);
		}
		if (key == "god_no_stagger")
		{
			Toggle(context, configGodModeNoStagger, "Staggering with god mode", value, reverse: true);
		}
		if (key == "hide_shout_pings")
		{
			Toggle(context, configHideShoutPings, "Shout pings", value, reverse: true);
		}
		if (key == "god_no_edge")
		{
			Toggle(context, configGodModeNoEdgeOfWorld, "Edge of world pull with god mode", value, reverse: true);
		}
		if (key == "god_no_knockback")
		{
			Toggle(context, configGodModeNoKnockback, "Knockback with god mode", value, reverse: true);
		}
		if (key == "god_no_mist")
		{
			Toggle(context, configGodModeNoMist, "Mist with god mode", value, reverse: true);
		}
		if (key == "fly_no_clip")
		{
			Toggle(context, configFlyNoClip, "No clip with fly mode", value);
		}
		if (key == "ghost_invibisility")
		{
			Toggle(context, configGhostInvisibility, "Invisibility with ghost mode", value);
		}
		if (key == "ghost_no_spawns")
		{
			Toggle(context, configGhostNoSpawns, "Spawns with ghost", value, reverse: true);
		}
		if (key == "ghost_ignore_sleep")
		{
			Toggle(context, configGhostIgnoreSleep, "Sleeping checked with ghost", value, reverse: true);
		}
		if (key == "disable_global_key")
		{
			ToggleFlag(context, configDisabledGlobalKeys, "Disabled global keys", value);
		}
		if (key == "disable_debug_mode_keys")
		{
			Toggle(context, configDisableDebugModeKeys, "Debug mode key bindings", value, reverse: true);
		}
		if (key == "god_always_parry")
		{
			Toggle(context, configGodModeAlwaysParry, "Always parry with god mode", value);
		}
		if (key == "god_always_dodge")
		{
			Toggle(context, configGodModeAlwaysDodge, "Always dodge with god mode", value);
		}
		if (key == "disable_start_shout")
		{
			Toggle(context, configDisableStartShout, "Start shout", value, reverse: true);
		}
		if (key == "disable_no_map")
		{
			Toggle(context, configDisableNoMap, "Disable no map", value);
		}
		if (key == "chat_output")
		{
			Toggle(context, configChatOutput, "Chat output", value);
		}
		if (key == "playerlist_format")
		{
			SetValue(context, configPlayerListFormat, key, value);
		}
		if (key == "command_log_format")
		{
			SetValue(context, configCommandLogFormat, key, value);
		}
		if (key == "find_format")
		{
			SetValue(context, configFindFormat, key, value);
		}
		if (key == "minimap_format")
		{
			SetValue(context, configMinimapFormat, key, value);
		}
		if (key == "kill_destroys_spawners")
		{
			Toggle(context, configKillDestroySpawners, "Kill commands destroy spawners", value);
		}
		if (key == "free_fly_camera_invert")
		{
			Toggle(context, configFreeFlyInvertCamera, "Free fly camera invert", value);
		}
		if (key == "server_chat")
		{
			Toggle(context, configServerChat, "Server chat", value);
		}
		if (key == "server_chat_name")
		{
			SetValue(context, configServerChatName, "Server chat name", value);
		}
		if (key == "limit_recipes_to_station")
		{
			Toggle(context, configNoCostLimitRecipesToStation, "Limit recipes to station with nocost mode", value);
		}
		if (key == "respect_station_level")
		{
			Toggle(context, configNoCostRespectStationLevel, "Respect station level with nocost mode", value);
		}
	}
}
