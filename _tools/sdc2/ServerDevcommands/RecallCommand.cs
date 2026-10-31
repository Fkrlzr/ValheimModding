using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ServerDevcommands;

public class RecallCommand
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
			if (index == 0)
			{
				return ParameterInfo.PublicPlayerNames;
			}
			return ParameterInfo.None;
		}

		internal void _003C_002Ector_003Eb__0_1(ConsoleEventArgs args)
		{
			Helper.ArgsCheck(args, 2, "Missing players");
			args.Context.TryRunCommand("tp " + args[1], false, false);
		}
	}

	public RecallCommand()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		AutoComplete.Register("recall", (int index) => (index == 0) ? ParameterInfo.PublicPlayerNames : ParameterInfo.None);
		object obj = _003C_003Ec._003C_003E9__0_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				Helper.ArgsCheck(args, 2, "Missing players");
				args.Context.TryRunCommand("tp " + args[1], false, false);
			};
			_003C_003Ec._003C_003E9__0_1 = val;
			obj = (object)val;
		}
		Helper.Command("recall", "[players] - Teleports players to you", (ConsoleEvent)obj);
	}
}
