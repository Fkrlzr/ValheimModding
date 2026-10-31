using System;
using System.Runtime.CompilerServices;

namespace ServerDevcommands;

public class ShutdownCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__0_0;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			Helper.AddMessage(args.Context, "Shutting down...");
			Menu.instance.OnQuitYes();
		}
	}

	public ShutdownCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				Helper.AddMessage(args.Context, "Shutting down...");
				Menu.instance.OnQuitYes();
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		Helper.Command("shutdown", "- Closes the game.", (ConsoleEvent)obj);
		AutoComplete.RegisterEmpty("shutdown");
	}
}
