using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class HUDCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__0_0;

		public static Func<int, List<string>> _003C_003E9__0_1;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			if (!Object.op_Implicit((Object)(object)Hud.m_instance))
			{
				Helper.AddMessage(args.Context, "Error: No HUD instance.");
				return;
			}
			if (args.Length >= 2)
			{
				Hud.m_instance.m_userHidden = args[1] != "1";
			}
			else
			{
				Hud.m_instance.m_userHidden = !Hud.m_instance.m_userHidden;
			}
			string text = (Hud.m_instance.m_userHidden ? "disabled" : "enabled");
			Helper.AddMessage(args.Context, "Hud " + text + ".");
		}

		internal List<string> _003C_002Ector_003Eb__0_1(int index)
		{
			if (index == 0)
			{
				return ParameterInfo.Create("1 = enable, 0 = disable, no value = toggle");
			}
			return ParameterInfo.None;
		}
	}

	public HUDCommand()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				if (!Object.op_Implicit((Object)(object)Hud.m_instance))
				{
					Helper.AddMessage(args.Context, "Error: No HUD instance.");
				}
				else
				{
					if (args.Length >= 2)
					{
						Hud.m_instance.m_userHidden = args[1] != "1";
					}
					else
					{
						Hud.m_instance.m_userHidden = !Hud.m_instance.m_userHidden;
					}
					string text = (Hud.m_instance.m_userHidden ? "disabled" : "enabled");
					Helper.AddMessage(args.Context, "Hud " + text + ".");
				}
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		new ConsoleCommand("hud", "[value] -  Toggles or sets the HUD visibility.", (ConsoleEvent)obj, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.Register("hud", (int index) => (index == 0) ? ParameterInfo.Create("1 = enable, 0 = disable, no value = toggle") : ParameterInfo.None);
	}
}
