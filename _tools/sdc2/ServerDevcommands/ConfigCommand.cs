using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ServerDevcommands;

public class ConfigCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<int, List<string>> _003C_003E9__0_0;

		public static ConsoleEvent _003C_003E9__1_0;

		public static ConsoleOptionsFetcher _003C_003E9__1_1;

		internal List<string> _003CRegisterAutoComplete_003Eb__0_0(int index)
		{
			if (index == 0)
			{
				return Settings.Options;
			}
			return ParameterInfo.Create("Value");
		}

		internal void _003C_002Ector_003Eb__1_0(ConsoleEventArgs args)
		{
			if (args.Length >= 2)
			{
				if (args.Length == 2)
				{
					Settings.UpdateValue(args.Context, args[1], "");
				}
				else
				{
					Settings.UpdateValue(args.Context, args[1], string.Join(" ", args.Args.Skip(2)));
				}
			}
		}

		internal List<string> _003C_002Ector_003Eb__1_1()
		{
			return Settings.Options;
		}
	}

	private void RegisterAutoComplete(string command)
	{
		AutoComplete.Register(command, (int index) => (index == 0) ? Settings.Options : ParameterInfo.Create("Value"));
	}

	public ConfigCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__1_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				if (args.Length >= 2)
				{
					if (args.Length == 2)
					{
						Settings.UpdateValue(args.Context, args[1], "");
					}
					else
					{
						Settings.UpdateValue(args.Context, args[1], string.Join(" ", args.Args.Skip(2)));
					}
				}
			};
			_003C_003Ec._003C_003E9__1_0 = val;
			obj = (object)val;
		}
		object obj2 = _003C_003Ec._003C_003E9__1_1;
		if (obj2 == null)
		{
			ConsoleOptionsFetcher val2 = () => Settings.Options;
			_003C_003Ec._003C_003E9__1_1 = val2;
			obj2 = (object)val2;
		}
		new ConsoleCommand("dev_config", "[key] [value] - Toggles or sets config value.", (ConsoleEvent)obj, false, false, false, false, false, false, (ConsoleOptionsFetcher)obj2, false, false, false);
		RegisterAutoComplete("dev_config");
	}
}
