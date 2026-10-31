using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class DevcommandsCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__5_0;

		public static ConsoleEvent _003C_003E9__5_1;

		internal void _003C_002Ector_003Eb__5_0(ConsoleEventArgs args)
		{
			if (Terminal.m_cheat)
			{
				Set(args.Context, value: false);
				return;
			}
			if (Object.op_Implicit((Object)(object)ZNet.instance) && ZNet.instance.IsServer())
			{
				Admin.OnSuccess();
				return;
			}
			args.Context.AddString("Authenticating for devcommands...");
			Admin.ManualCheck();
		}

		internal void _003C_002Ector_003Eb__5_1(ConsoleEventArgs args)
		{
			if ((Object)(object)Game.instance == (Object)null)
			{
				return;
			}
			Game.instance.GetPlayerProfile().m_usedCheats = false;
			Player localPlayer = Player.m_localPlayer;
			if (Object.op_Implicit((Object)(object)localPlayer))
			{
				foreach (ItemData allItem in ((Humanoid)localPlayer).GetInventory().GetAllItems())
				{
					allItem.m_cheated = false;
				}
			}
			Achievements.m_cheatCheckFrame = -1;
			args.Context.AddString("Character cheat status cleared.");
		}
	}

	public static void Set(Terminal terminal, bool value)
	{
		terminal.AddString("Devcommands: " + value);
		Set(value);
	}

	public static void EnableAutoFeatures()
	{
		if (!Terminal.m_cheat)
		{
			return;
		}
		Player localPlayer = Player.m_localPlayer;
		if (Object.op_Implicit((Object)(object)localPlayer) && ((Character)localPlayer).m_nview.IsValid())
		{
			if (Settings.AutoGodMode && Settings.IsEnabled(PermissionHash.God, localValue: true))
			{
				localPlayer.SetGodMode(true);
			}
			if (Settings.AutoGhostMode && Settings.IsEnabled(PermissionHash.Ghost, localValue: true))
			{
				localPlayer.SetGhostMode(true);
			}
			if (Settings.AutoFly && Settings.IsEnabled(PermissionHash.Fly, localValue: true))
			{
				localPlayer.m_debugFly = true;
				((Character)localPlayer).m_nview.GetZDO().Set(ZDOVars.s_debugFly, true);
			}
			if (Settings.AutoNoCost && Settings.IsEnabled(PermissionHash.NoCost, localValue: true))
			{
				localPlayer.m_noPlacementCost = true;
			}
			if (Settings.AutoDebugMode)
			{
				Player.m_debugMode = true;
			}
		}
		if (Object.op_Implicit((Object)(object)EnvMan.instance))
		{
			if (Settings.AutoTod != "" && PermissionApi.IsCommandAllowed("tod"))
			{
				EnvMan.instance.m_debugTimeOfDay = true;
				EnvMan.instance.m_debugTime = Mathf.Clamp01(Parse.Float(Settings.AutoTod));
			}
			if (Settings.AutoEnv != "" && PermissionApi.IsCommandAllowed("env"))
			{
				EnvMan.instance.m_debugEnv = Settings.AutoEnv;
			}
		}
	}

	public static void DisableAutoFeatures()
	{
		if (Terminal.m_cheat)
		{
			return;
		}
		Player localPlayer = Player.m_localPlayer;
		if (Object.op_Implicit((Object)(object)localPlayer) && ((Character)localPlayer).m_nview.IsValid())
		{
			if (Settings.AutoDebugMode)
			{
				Player.m_debugMode = false;
			}
			if (Settings.AutoGodMode)
			{
				localPlayer.SetGodMode(false);
			}
			if (Settings.AutoGhostMode)
			{
				localPlayer.SetGhostMode(false);
			}
			if (Settings.AutoFly)
			{
				localPlayer.m_debugFly = false;
				((Character)localPlayer).m_nview.GetZDO().Set(ZDOVars.s_debugFly, false);
			}
			if (Settings.AutoNoCost)
			{
				localPlayer.m_noPlacementCost = false;
			}
		}
		if (Object.op_Implicit((Object)(object)EnvMan.instance))
		{
			if (Settings.AutoTod != "")
			{
				EnvMan.instance.m_debugTimeOfDay = false;
			}
			if (Settings.AutoEnv != "")
			{
				EnvMan.instance.m_debugEnv = "";
			}
		}
	}

	public static void SetCheats(bool value)
	{
		if (Terminal.m_cheat != value)
		{
			Gogan.LogEvent("Cheat", "CheatsEnabled", value.ToString(), 0L);
		}
		Terminal.m_cheat = value;
	}

	public static void Set(bool value)
	{
		SetCheats(value);
		((Terminal)Console.instance).updateCommandList();
		((Terminal)Chat.instance).updateCommandList();
		DisableAutoFeatures();
		EnableAutoFeatures();
		if (value && Settings.AutoExecDevOn != "")
		{
			((Terminal)Console.instance).TryRunCommand(Settings.AutoExecDevOn, false, false);
		}
		if (!value && Settings.AutoExecDevOff != "")
		{
			((Terminal)Console.instance).TryRunCommand(Settings.AutoExecDevOff, false, false);
		}
	}

	public DevcommandsCommand()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__5_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				if (Terminal.m_cheat)
				{
					Set(args.Context, value: false);
				}
				else if (Object.op_Implicit((Object)(object)ZNet.instance) && ZNet.instance.IsServer())
				{
					Admin.OnSuccess();
				}
				else
				{
					args.Context.AddString("Authenticating for devcommands...");
					Admin.ManualCheck();
				}
			};
			_003C_003Ec._003C_003E9__5_0 = val;
			obj = (object)val;
		}
		new ConsoleCommand("devcommands", "Toggles cheats", (ConsoleEvent)obj, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj2 = _003C_003Ec._003C_003E9__5_1;
		if (obj2 == null)
		{
			ConsoleEvent val2 = delegate(ConsoleEventArgs args)
			{
				if (!((Object)(object)Game.instance == (Object)null))
				{
					Game.instance.GetPlayerProfile().m_usedCheats = false;
					Player localPlayer = Player.m_localPlayer;
					if (Object.op_Implicit((Object)(object)localPlayer))
					{
						foreach (ItemData allItem in ((Humanoid)localPlayer).GetInventory().GetAllItems())
						{
							allItem.m_cheated = false;
						}
					}
					Achievements.m_cheatCheckFrame = -1;
					args.Context.AddString("Character cheat status cleared.");
				}
			};
			_003C_003Ec._003C_003E9__5_1 = val2;
			obj2 = (object)val2;
		}
		new ConsoleCommand("clearcheats", "Clears the character cheat status.", (ConsoleEvent)obj2, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.RegisterEmpty("devcommands");
		AutoComplete.RegisterEmpty("clearcheats");
	}
}
