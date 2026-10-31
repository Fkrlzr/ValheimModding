using System;
using BepInEx;
using SkToolbox.Configuration;
using SkToolbox.Utility;
using UnityEngine;

namespace SkToolbox;

[BepInPlugin("com.speelo.speelostoolbox", "SpeelosToolbox", "1.1.7")]
public class SkBepInExLoader : BaseUnityPlugin
{
	public const string MODNAME = "SpeelosToolbox";

	public const string DISPLAYNAME = "Speelo's Toolbox";

	public const string AUTHOR = "Speelo";

	public const string ORIGINAL_AUTHOR = "Skrip";

	public const string GUID = "com.speelo.speelostoolbox";

	public const string VERSION = "1.1.7";

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if (Application.isBatchMode || (int)SystemInfo.graphicsDeviceType == 4)
		{
			Logger.LogInfo((object)"Headless server detected - SpeelosToolbox is client-only, not loading.");
			return;
		}
		InitConfig();
		SkCommandPatcher.ApplyAchievementBypass();
		((Component)this).transform.parent = null;
		Object.DontDestroyOnLoad((Object)(object)this);
		SkLoader.InitBepThreading(this);
	}

	public void InitConfig()
	{
		try
		{
			SkConfigEntry.CDescriptor = ((BaseUnityPlugin)this).Config.Bind<bool>("- Index", "ThisIsJustAnIndex-NotASetting", true, "Config sections:\n0 - General\n1 - Auto Run\n2 - Customize Console Look\n3 - Command Aliasing\n4 - On-Screen Menu\n5 - Command Hotkeys\n");
			SkConfigEntry.CConsoleEnabled = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "ConsoleEnabled", true, "Enables the console without launch option.");
			SkConfigEntry.CScrollable = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "ConsoleScrollable", true, "Enables the console to be scrollable.");
			SkConfigEntry.CScrollableLimit = ((BaseUnityPlugin)this).Config.Bind<int>("0 - General", "ConsoleScrollableLimit", 500, "Maximum number of lines to store in the console. Game default = 30 (lol)");
			SkConfigEntry.CConsoleAutoComplete = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "AutoComplete", true, "Press tab to auto-complete Speelo's Toolbox commands if you have partially typed a command.");
			SkConfigEntry.CAllowChatCommandInput = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "AllowChatCommandInput", true, "Toggle this if you want to allow or disable the entry of commands in the chat. If this is disabled, you can only input commands into the console.");
			SkConfigEntry.CAllowPublicChatOutput = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "AllowPublicResponse", true, "Toggle this to allow the mod to respond publicly with certain commands, when entered into chat.\nThe /portal command for example, if used in chat and this is true, others nearby will be able to see the response.\nNOTE: If you see a response from your name, it is shown publicly and everyone can see it. If it is a response from (Speelo's Toolbox), only you see it.");
			SkConfigEntry.CAllowChatOutput = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "AllowResponseInChat", true, "Toggle this to allow the mod to show output in the chat. If this is disabled, the mod will not output to the chat at all, publicly or not.");
			SkConfigEntry.CAllowExecuteOnClear = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "AllowExecuteOnClear", false, "Toggle this to enable the ability to execute commands by clearing the input (by hitting escape, or selecting all and removing).");
			SkConfigEntry.CKeepAchievements = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "KeepAchievements", true, "Keep Steam achievements working while cheats are used.\nValheim disables achievement progress as soon as anything counts as cheated, and simply loading BepInEx already counts (it sets Game.isModded).\nThis flips the game's own PlayerProfile.s_bypassCheatChecks switch, which every cheat check reads, so progress keeps counting.\nIt also stops cheat commands from permanently flagging the character, and clears that flag if it was already set.\nSet to false to let Valheim disable achievements normally.");
			SkConfigEntry.CPersistCheatsOnDeath = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "PersistCheatsOnDeath", true, "Put your cheats back on after you die.\nDying destroys your character object and the game builds a new one from the prefab, so every cheat that is stored on the character is silently lost: god mode, flying, no cost building, infinite stamina and far interact all switch themselves off on the respawn.\nWith this on, whatever you had switched on is switched back on once you respawn.\nSet to false to let each death clear them, which is what Valheim does on its own.");
			SkConfigEntry.CGodModeBlocksDamage = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "GodModeBlocksDamage", true, "Make god mode block damage outright.\nValheim's own god mode does not stop damage: it subtracts health as normal and only rescues you at the moment the hit would kill you, clamping health to 1. So your health bar still drains.\nWith this on, any damage aimed at you while god mode is active is discarded before it lands, including fire, poison and smoke.\nSet to false for Valheim's unmodified behaviour.");
			SkConfigEntry.CHammerOnAnyServer = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "HammerOnAnyServer", false, "Only for servers whose owner has agreed to it. Off by default.\nInfinity Hammer asks Server Devcommands whether you may use each of its features (no resource cost, remove anything, ignore wards, and so on). Without admin rights on the server the answer is no, even though the hammer itself runs on your own computer.\nWith this on, the toolbox answers yes on your client for Infinity Hammer features unless the server explicitly forbids that feature (a permission set to 'no', for it or for everything). It does not touch any other mod, does not give you admin rights, and changes nothing on the server.\nTakes effect after a restart.");
			SkConfigEntry.CPlayerTools = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "EnablePlayerTools", false, "Adds tools that act on OTHER players to the World > Server menu: damage a player, kill every other player, drain or refill their stamina, and chat under a chosen name. Off by default.\nIntended for a private server whose players have agreed to PvP and cheats. They are client side: they work for any player who turns this on in their own config, with no admin rights needed. The destructive ones ask for confirmation.\nTakes effect after a restart.");
			SkConfigEntry.CHiddenPieces = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "HiddenPieces", false, "Add building pieces the game never lets you craft (Ashlands wall and floor parts, dungeon and village parts, old test pieces) to the Hammer's build menu every time you spawn in. Pieces without snap points get corner snap points so they can still snap.\nNothing is saved to the world or the game files: turning it off or restarting puts the menu back. Pieces without a normal icon show a hammer icon, and ones with no readable name show their prefab name. Switch it on and off from Player > Cheats > Hidden Pieces.");
			SkConfigEntry.COpenConsoleWithSlash = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "OpenConsoleWithSlash", false, "Toggle this to enable the ability to open the console with the slash (/) button.\nThis option takes precedence over OpenChatWithSlash if both are true.");
			SkConfigEntry.COpenChatWithSlash = ((BaseUnityPlugin)this).Config.Bind<bool>("0 - General", "OpenChatWithSlash", false, "Toggle this to enable the ability to open chat with the slash (/) button.");
			SkConfigEntry.CAutoRun = ((BaseUnityPlugin)this).Config.Bind<bool>("1 - AutoRun", "AutoRunEnabled", false, "Toggle this to run commands automatically upon spawn into server.\nNote this will only occur once per game launch to prevent unintended command executions.");
			SkConfigEntry.CAutoRunCommand = ((BaseUnityPlugin)this).Config.Bind<string>("1 - AutoRun", "AutoRunCommand", "/nosup; /god", "Enter the commands to run upon spawn here. Seperate commands with a semicolon (;).");
			SkConfigEntry.OMenuToggleKey = ((BaseUnityPlugin)this).Config.Bind<string>("4 - OnScreenMenu", "MenuToggleKey", "F6", "Key that opens and closes the clickable menu. Valid key names: https://docs.unity3d.com/ScriptReference/KeyCode.html\nAlready taken: F11 and F12 take screenshots (Valheim and Steam), F5 opens the game console, F4 opens this mod's log console, F2 opens the connect panel, F9 cycles the controller layout, Ctrl+F1 toggles mouse capture, Ctrl+F3 hides the HUD.\nFree function keys: F6, F7, F8, F10.");
			SkConfigEntry.OMenuWalk = ((BaseUnityPlugin)this).Config.Bind<bool>("4 - OnScreenMenu", "WalkWithMenuOpen", true, "Let you walk around with the menu open, instead of freezing you in place.\nWASD moves as normal. Attacking, blocking, jumping and dodging stay switched off, which is the same state the game puts you in for a moment after closing its own menus.\nTyping in a search box moves nothing. Hold the look key below to turn the camera.\nThis is keyboard and mouse only: with a gamepad active the menu freezes you as before.");
			SkConfigEntry.OMenuLookKey = ((BaseUnityPlugin)this).Config.Bind<string>("4 - OnScreenMenu", "MenuLookKey", "Mouse1", "Hold this to turn the camera while the menu is open. Release it to get the cursor back.\nMouse1 is the right mouse button. It is safe to use even though it is normally block, because blocking is switched off while the menu is open.\nLeftAlt is a good alternative if you would rather leave the right button alone.\nValid key names: https://docs.unity3d.com/ScriptReference/KeyCode.html");
			SkConfigEntry.OBookmarks = ((BaseUnityPlugin)this).Config.Bind<string>("4 - OnScreenMenu", "Bookmarks", "", "Saved teleport spots, managed from the menu - there is no need to edit this by hand.\nEach entry is seed|name|x|y|z, and only the ones matching the world you are in are shown.");
			SkConfigEntry.OLoadouts = ((BaseUnityPlugin)this).Config.Bind<string>("4 - OnScreenMenu", "Loadouts", "", "Saved equipment sets, managed from the menu - there is no need to edit this by hand.\nEach entry is name|prefab:quality,prefab:quality.");
			SkConfigEntry.OMenuOpacity = ((BaseUnityPlugin)this).Config.Bind<float>("4 - OnScreenMenu", "MenuOpacity", 0.96f, "How solid the menu background is. 1 = fully opaque, 0.5 = half see-through. Takes effect immediately.");
			SkConfigEntry.CAllowLookCustomizations = ((BaseUnityPlugin)this).Config.Bind<bool>("2 - CustomizeConsoleLook", "ConsoleAllowLookCustomizations", true, "Toggle this to enable or disable the customization settings below.");
			SkConfigEntry.CConsoleFont = ((BaseUnityPlugin)this).Config.Bind<string>("2 - CustomizeConsoleLook", "ConsoleFont", "Consola.ttl", "Set the font size of the text in the console. Game default = AveriaSansLibre-Bold");
			SkConfigEntry.CConsoleFontSize = ((BaseUnityPlugin)this).Config.Bind<int>("2 - CustomizeConsoleLook", "ConsoleFontSize", 18, "Set the font size of the text in the console. Game default = 18");
			SkConfigEntry.CConsoleOutputTextColor = ((BaseUnityPlugin)this).Config.Bind<string>("2 - CustomizeConsoleLook", "ConsoleOutputTextColor", "#E6F7FFFF", "Set the color of the output text shown in the console. Game default = #FFFFFFFF. Color format is #RRGGBBAA");
			SkConfigEntry.CConsoleInputTextColor = ((BaseUnityPlugin)this).Config.Bind<string>("2 - CustomizeConsoleLook", "ConsoleInputTextColor", "#E6F7FFFF", "Set the color of the input text shown in the console. Game default = #FFFFFFFF. Color format is #RRGGBBAA");
			SkConfigEntry.CConsoleSelectionColor = ((BaseUnityPlugin)this).Config.Bind<string>("2 - CustomizeConsoleLook", "ConsoleSelectionColor", "#A8CEFFC0", "Set the color of the selection highlight in the console. Game default = #A8CEFFC0. Color format is #RRGGBBAA");
			SkConfigEntry.CConsoleCaretColor = ((BaseUnityPlugin)this).Config.Bind<string>("2 - CustomizeConsoleLook", "ConsoleCaretColor", "#DCE6F5FF", "Set the color of the input text caret shown in the console. Game default = #FFFFFFFF. Color format is #RRGGBBAA");
			SkConfigEntry.CAlias1 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias1", "/creative: /god; /ghost; /imacheater; /nores; /nocost; /echo Creative mode toggled!", "Set this to create a command alias. Specify what the user will type, and what will be executed. Command chaining does work here, and these aliases can be used with the AutoRun functionality above. Note that aliases cannot reference other aliases. The slash (/) prefix is also not required - command aliases can be set to anything you want! Aliases do work from chat as well, if the execute from chat setting is enabled above. Alias commands also appear in the /? help menus.\nFormat: WhatToType: WhatToExecute\nExample: This will create a new command, '/Cmd1', and when entered it will execute /god and /fly\n/Cmd1: /god; /fly\n##################\nSet this to create command alias 1.");
			SkConfigEntry.CAlias2 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias2", "", "Set this to create command alias 2.");
			SkConfigEntry.CAlias3 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias3", "", "Set this to create command alias 3.");
			SkConfigEntry.CAlias4 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias4", "", "Set this to create command alias 4.");
			SkConfigEntry.CAlias5 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias5", "", "Set this to create command alias 5.");
			SkConfigEntry.CAlias6 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias6", "", "Set this to create command alias 6.");
			SkConfigEntry.CAlias7 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias7", "", "Set this to create command alias 7.");
			SkConfigEntry.CAlias8 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias8", "", "Set this to create command alias 8.");
			SkConfigEntry.CAlias9 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias9", "", "Set this to create command alias 9.");
			SkConfigEntry.CAlias10 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias10", "", "Set this to create command alias 10.");
			SkConfigEntry.CAlias11 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias11", "", "Set this to create command alias 11.");
			SkConfigEntry.CAlias12 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias12", "", "Set this to create command alias 12.");
			SkConfigEntry.CAlias13 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias13", "", "Set this to create command alias 13.");
			SkConfigEntry.CAlias14 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias14", "", "Set this to create command alias 14.");
			SkConfigEntry.CAlias15 = ((BaseUnityPlugin)this).Config.Bind<string>("3 - CommandAliasing", "Alias15", "", "Set this to create command alias 15.");
			SkConfigEntry.CHotkey1 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 1", "", "Set this to create a hotkey for a command or command chain.\nValid Format is WhatToPress: WhatToExecute\nExample 1 Press z to input /fly; /god        | z: /fly; /god\nExample 2 Press Shift+Z to input /creative   | Z: /creative\nExample 3 Press Tilde to open the console    | `: /console\nOnly 'Printable ASCII characters' are valid! (https://theasciicode.com.ar/). Capital letters make the hotkey require Shift before the hotkey press. Only one command or command chain can be assigned to each key (if hotkey 1 assigns to q, hotkey 2 cannot also assign to q).\nOnly the first colon separates the key from the command, so the command itself may contain one. The key cannot be a colon or a space.\nHotkeys do not fire while you are typing - in chat, the console, a sign, a map pin, the inventory or this mod's own menu.\nChanges here are read at startup, so restart the game or use Reload Toolbox in the System tab after editing. A slot that cannot be read says so in the console at startup rather than failing silently.");
			SkConfigEntry.CHotkey2 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 2", "", "Set this to create a hotkey for a command or command chain.");
			SkConfigEntry.CHotkey3 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 3", "", "Set this to create a hotkey for a command or command chain.");
			SkConfigEntry.CHotkey4 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 4", "", "Set this to create a hotkey for a command or command chain.");
			SkConfigEntry.CHotkey5 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 5", "", "Set this to create a hotkey for a command or command chain.");
			SkConfigEntry.CHotkey6 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 6", "", "Set this to create a hotkey for a command or command chain.");
			SkConfigEntry.CHotkey7 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 7", "", "Set this to create a hotkey for a command or command chain.");
			SkConfigEntry.CHotkey8 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 8", "", "Set this to create a hotkey for a command or command chain.");
			SkConfigEntry.CHotkey9 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 9", "", "Set this to create a hotkey for a command or command chain.");
			SkConfigEntry.CHotkey10 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 10", "", "Set this to create a hotkey for a command or command chain.");
			SkConfigEntry.CHotkey11 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 11", "", "Set this to create a hotkey for a command or command chain.");
			SkConfigEntry.CHotkey12 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 12", "", "Set this to create a hotkey for a command or command chain.");
			SkConfigEntry.CHotkey13 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 13", "", "Set this to create a hotkey for a command or command chain.");
			SkConfigEntry.CHotkey14 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 14", "", "Set this to create a hotkey for a command or command chain.");
			SkConfigEntry.CHotkey15 = ((BaseUnityPlugin)this).Config.Bind<string>("5 - CommandHotkeys", "Hotkey 15", "", "Set this to create a hotkey for a command or command chain.");
		}
		catch (Exception ex)
		{
			SkUtilities.Logz(new string[1] { "ERR" }, new string[3] { "Could not load config. Please confirm there is a working version of BepInEx installed.", ex.Message, ex.Source }, (LogType)0);
		}
	}
}
