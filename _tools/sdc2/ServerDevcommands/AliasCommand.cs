using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ServerDevcommands;

public class AliasCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<string, string> _003C_003E9__0_2;

		public static ConsoleEvent _003C_003E9__0_0;

		public static Func<int, int, List<string>> _003C_003E9__0_1;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			if (args.Length < 2)
			{
				args.Context.AddString(string.Join("\n", AliasManager.AliasKeys.Select((string key) => key + " -> " + AliasManager.GetAliasValue(key))));
			}
			else if (args.Length < 3)
			{
				AliasManager.RemoveAlias(args[1]);
				if (Terminal.commands.ContainsKey(args[1]))
				{
					Terminal.commands.Remove(args[1]);
				}
				args.Context.updateCommandList();
			}
			else
			{
				string value = string.Join(" ", args.Args.Skip(2));
				AliasManager.AddAlias(args[1], value);
				args.Context.updateCommandList();
			}
		}

		internal string _003C_002Ector_003Eb__0_2(string key)
		{
			return key + " -> " + AliasManager.GetAliasValue(key);
		}

		internal List<string> _003C_002Ector_003Eb__0_1(int index, int subIndex)
		{
			if (index == 0)
			{
				return ParameterInfo.Create("Name of the alias.");
			}
			return ParameterInfo.None;
		}
	}

	public AliasCommand()
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
				if (args.Length < 2)
				{
					args.Context.AddString(string.Join("\n", AliasManager.AliasKeys.Select((string key) => key + " -> " + AliasManager.GetAliasValue(key))));
				}
				else if (args.Length < 3)
				{
					AliasManager.RemoveAlias(args[1]);
					if (Terminal.commands.ContainsKey(args[1]))
					{
						Terminal.commands.Remove(args[1]);
					}
					args.Context.updateCommandList();
				}
				else
				{
					string value = string.Join(" ", args.Args.Skip(2));
					AliasManager.AddAlias(args[1], value);
					args.Context.updateCommandList();
				}
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		new ConsoleCommand("alias", "[name] [command] - Sets a command alias.", (ConsoleEvent)obj, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.Register("alias", (int index, int subIndex) => (index == 0) ? ParameterInfo.Create("Name of the alias.") : ParameterInfo.None);
		AutoComplete.Offsets["alias"] = 1;
	}
}
