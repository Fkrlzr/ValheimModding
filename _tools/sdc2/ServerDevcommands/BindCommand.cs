using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch]
public class BindCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__0_0;

		public static ConsoleOptionsFetcher _003C_003E9__0_1;

		public static Func<int, int, List<string>> _003C_003E9__0_2;

		public static Func<int, List<string>> _003C_003E9__0_7;

		public static ConsoleEvent _003C_003E9__0_3;

		public static Func<int, List<string>> _003C_003E9__0_4;

		public static ConsoleEvent _003C_003E9__0_5;

		public static ConsoleEvent _003C_003E9__0_6;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			if (args.Length >= 2)
			{
				BindManager.AddBind(args[1], string.Join(" ", args.Args.Skip(2)));
			}
		}

		internal List<string> _003C_002Ector_003Eb__0_1()
		{
			return ParameterInfo.KeyCodes;
		}

		internal List<string> _003C_002Ector_003Eb__0_2(int index, int subIndex)
		{
			if (index == 0 && subIndex == 0)
			{
				return ParameterInfo.KeyCodes;
			}
			if (index == 0 && subIndex == 1)
			{
				return ParameterInfo.KeyCodesWithNegative;
			}
			return ParameterInfo.None;
		}

		internal List<string> _003C_002Ector_003Eb__0_7(int index)
		{
			return ParameterInfo.KeyCodesWithNegative;
		}

		internal void _003C_002Ector_003Eb__0_3(ConsoleEventArgs args)
		{
			if (args.Length >= 2)
			{
				BindManager.RemoveBind(args[1]);
			}
		}

		internal List<string> _003C_002Ector_003Eb__0_4(int index)
		{
			if (index == 0)
			{
				return ParameterInfo.KeyCodes;
			}
			return ParameterInfo.None;
		}

		internal void _003C_002Ector_003Eb__0_5(ConsoleEventArgs args)
		{
			BindManager.PrintBinds(args.Context);
		}

		internal void _003C_002Ector_003Eb__0_6(ConsoleEventArgs args)
		{
			BindManager.ClearBinds();
		}
	}

	public BindCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				if (args.Length >= 2)
				{
					BindManager.AddBind(args[1], string.Join(" ", args.Args.Skip(2)));
				}
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		object obj2 = _003C_003Ec._003C_003E9__0_1;
		if (obj2 == null)
		{
			ConsoleOptionsFetcher val2 = () => ParameterInfo.KeyCodes;
			_003C_003Ec._003C_003E9__0_1 = val2;
			obj2 = (object)val2;
		}
		new ConsoleCommand("bind", "[keycode,modifier1,modifier2,...] [command] [parameters] - Binds a key (with modifier keys) to a command.", (ConsoleEvent)obj, false, false, false, false, false, false, (ConsoleOptionsFetcher)obj2, false, false, false);
		AutoComplete.Register("bind", delegate(int index, int subIndex)
		{
			if (index == 0 && subIndex == 0)
			{
				return ParameterInfo.KeyCodes;
			}
			return (index == 0 && subIndex == 1) ? ParameterInfo.KeyCodesWithNegative : ParameterInfo.None;
		}, new Dictionary<string, Func<int, List<string>>> { 
		{
			"keys",
			(int index) => ParameterInfo.KeyCodesWithNegative
		} });
		AutoComplete.Offsets["bind"] = 1;
		object obj3 = _003C_003Ec._003C_003E9__0_3;
		if (obj3 == null)
		{
			ConsoleEvent val3 = delegate(ConsoleEventArgs args)
			{
				if (args.Length >= 2)
				{
					BindManager.RemoveBind(args[1]);
				}
			};
			_003C_003Ec._003C_003E9__0_3 = val3;
			obj3 = (object)val3;
		}
		new ConsoleCommand("unbind", "[keycode] - Clears binds from a key.", (ConsoleEvent)obj3, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.Register("unbind", (int index) => (index == 0) ? ParameterInfo.KeyCodes : ParameterInfo.None);
		object obj4 = _003C_003Ec._003C_003E9__0_5;
		if (obj4 == null)
		{
			ConsoleEvent val4 = delegate(ConsoleEventArgs args)
			{
				BindManager.PrintBinds(args.Context);
			};
			_003C_003Ec._003C_003E9__0_5 = val4;
			obj4 = (object)val4;
		}
		new ConsoleCommand("printbinds", "Prints all key binds.", (ConsoleEvent)obj4, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.RegisterEmpty("printbinds");
		object obj5 = _003C_003Ec._003C_003E9__0_6;
		if (obj5 == null)
		{
			ConsoleEvent val5 = delegate
			{
				BindManager.ClearBinds();
			};
			_003C_003Ec._003C_003E9__0_6 = val5;
			obj5 = (object)val5;
		}
		new ConsoleCommand("resetbinds", "Removes all custom key binds.", (ConsoleEvent)obj5, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.RegisterEmpty("resetbinds");
	}

	[HarmonyPatch(typeof(Chat), "Update")]
	[HarmonyPostfix]
	private static void ExecuteBestBinds(Chat __instance)
	{
		if (((TMP_InputField)((Terminal)__instance).m_input).isFocused || ((Object)(object)ReportUser.instance != (Object)null && ReportUser.instance.IsReportUserWindowActive()) || (Object.op_Implicit((Object)(object)Console.instance) && ((Component)((Terminal)Console.instance).m_chatWindow).gameObject.activeInHierarchy) || ZNet.IsPasswordDialogShowing())
		{
			return;
		}
		foreach (CommandBind bestKeyCommand in BindManager.GetBestKeyCommands())
		{
			bestKeyCommand.Executed = true;
			if (!bestKeyCommand.WasExecuted && !(bestKeyCommand.Command == ""))
			{
				((Terminal)__instance).TryRunCommand(bestKeyCommand.Command, true, true);
			}
		}
		foreach (CommandBind offBind in BindManager.GetOffBinds())
		{
			offBind.WasExecuted = false;
			if (!(offBind.OffCommand == ""))
			{
				((Terminal)__instance).TryRunCommand(offBind.OffCommand, true, true);
			}
		}
	}

	public static void SetMode(string mode)
	{
		BindManager.SetMode(mode);
	}
}
