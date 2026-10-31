using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class ServerCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__1_0;

		public static Func<int, List<string>> _003C_003E9__1_1;

		internal void _003C_002Ector_003Eb__1_0(ConsoleEventArgs args)
		{
			if (args.Length >= 2)
			{
				ServerExecution.Send(string.Join(" ", args.Args.Skip(1)));
			}
		}

		internal List<string> _003C_002Ector_003Eb__1_1(int index)
		{
			if (index == 0)
			{
				return ParameterInfo.Create("Seconds", "a number (default 240.0)");
			}
			return ParameterInfo.None;
		}
	}

	private void MakeServer(string name)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		ConsoleCommand original = Terminal.commands[name];
		Helper.Command(name, original.Description, (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			if (Object.op_Implicit((Object)(object)ZNet.instance) && !ZNet.instance.IsServer())
			{
				ServerExecution.Send(args);
			}
			else
			{
				original.action.Invoke(args);
			}
		});
	}

	public ServerCommand()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__1_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				if (args.Length >= 2)
				{
					ServerExecution.Send(string.Join(" ", args.Args.Skip(1)));
				}
			};
			_003C_003Ec._003C_003E9__1_0 = val;
			obj = (object)val;
		}
		new ConsoleCommand("server", "[command] - Executes the given command on the server.", (ConsoleEvent)obj, false, true, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.RegisterEmpty("server");
		AutoComplete.Offsets["server"] = 0;
		MakeServer("randomevent");
		AutoComplete.RegisterEmpty("randomevent");
		MakeServer("stopevent");
		AutoComplete.RegisterEmpty("stopevent");
		AutoComplete.RegisterEmpty("genloc");
		AutoComplete.RegisterEmpty("sleep");
		AutoComplete.Register("skiptime", (int index) => (index == 0) ? ParameterInfo.Create("Seconds", "a number (default 240.0)") : ParameterInfo.None);
		AutoComplete.RegisterEmpty("resetkeys");
	}
}
