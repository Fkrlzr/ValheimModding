using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ServerDevcommands;

public class WaitCommand
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
		}

		internal List<string> _003C_002Ector_003Eb__0_1(int index)
		{
			if (index != 0)
			{
				return ParameterInfo.None;
			}
			return ParameterInfo.Create("Duration in milliseconds.");
		}
	}

	public WaitCommand()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate
			{
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		new ConsoleCommand("wait", "[duration] - Milliseconds to wait before executing the next command.", (ConsoleEvent)obj, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.Register("wait", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("Duration in milliseconds."));
	}
}
