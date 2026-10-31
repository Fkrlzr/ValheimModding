using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class NoMapCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<int, List<string>> _003C_003E9__0_0;

		public static ConsoleEvent _003C_003E9__0_1;

		internal List<string> _003C_002Ector_003Eb__0_0(int index)
		{
			if (index != 0)
			{
				return ParameterInfo.None;
			}
			return ParameterInfo.Create("1 = disable map, 0 = enable map, no value = toggle");
		}

		internal void _003C_002Ector_003Eb__0_1(ConsoleEventArgs args)
		{
			bool flag = false;
			bool flag2 = Object.op_Implicit((Object)(object)ZNet.instance) && ZNet.instance.IsServer();
			Player localPlayer = Player.m_localPlayer;
			if (!Object.op_Implicit((Object)(object)localPlayer) && !flag2)
			{
				return;
			}
			if (args.Length < 2)
			{
				if (flag2)
				{
					flag = !ZoneSystem.instance.GetGlobalKey((GlobalKeys)31);
				}
				else if (Object.op_Implicit((Object)(object)localPlayer))
				{
					flag = PlayerPrefs.GetFloat("mapenabled_" + Player.m_localPlayer.GetPlayerName(), 1f) == 1f;
				}
			}
			else
			{
				flag = args[1] == "1";
			}
			if (flag2)
			{
				if (flag)
				{
					ZoneSystem.instance.SetGlobalKey((GlobalKeys)31);
				}
				else
				{
					ZoneSystem.instance.RemoveGlobalKey((GlobalKeys)31);
				}
			}
			if (Object.op_Implicit((Object)(object)localPlayer))
			{
				PlayerPrefs.SetFloat("mapenabled_" + Player.m_localPlayer.GetPlayerName(), flag ? 0f : 1f);
			}
			string text = "client and server";
			if (Object.op_Implicit((Object)(object)localPlayer) && !flag2)
			{
				text = "client";
			}
			if (!Object.op_Implicit((Object)(object)localPlayer) && flag2)
			{
				text = "server";
			}
			Helper.AddMessage(args.Context, "Map " + (flag ? "disabled" : "enabled") + " for the " + text + ".");
		}
	}

	public NoMapCommand()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		AutoComplete.Register("nomap", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("1 = disable map, 0 = enable map, no value = toggle"));
		object obj = _003C_003Ec._003C_003E9__0_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				bool flag = false;
				bool flag2 = Object.op_Implicit((Object)(object)ZNet.instance) && ZNet.instance.IsServer();
				Player localPlayer = Player.m_localPlayer;
				if (Object.op_Implicit((Object)(object)localPlayer) || flag2)
				{
					if (args.Length < 2)
					{
						if (flag2)
						{
							flag = !ZoneSystem.instance.GetGlobalKey((GlobalKeys)31);
						}
						else if (Object.op_Implicit((Object)(object)localPlayer))
						{
							flag = PlayerPrefs.GetFloat("mapenabled_" + Player.m_localPlayer.GetPlayerName(), 1f) == 1f;
						}
					}
					else
					{
						flag = args[1] == "1";
					}
					if (flag2)
					{
						if (flag)
						{
							ZoneSystem.instance.SetGlobalKey((GlobalKeys)31);
						}
						else
						{
							ZoneSystem.instance.RemoveGlobalKey((GlobalKeys)31);
						}
					}
					if (Object.op_Implicit((Object)(object)localPlayer))
					{
						PlayerPrefs.SetFloat("mapenabled_" + Player.m_localPlayer.GetPlayerName(), flag ? 0f : 1f);
					}
					string text = "client and server";
					if (Object.op_Implicit((Object)(object)localPlayer) && !flag2)
					{
						text = "client";
					}
					if (!Object.op_Implicit((Object)(object)localPlayer) && flag2)
					{
						text = "server";
					}
					Helper.AddMessage(args.Context, "Map " + (flag ? "disabled" : "enabled") + " for the " + text + ".");
				}
			};
			_003C_003Ec._003C_003E9__0_1 = val;
			obj = (object)val;
		}
		new ConsoleCommand("nomap", "[set] - Toggles or sets the nomap mode. Can be executed on the server.", (ConsoleEvent)obj, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
	}
}
