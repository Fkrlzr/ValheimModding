using System;
using System.Runtime.CompilerServices;

namespace ServerDevcommands;

public class SeedCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__0_0;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			Helper.AddMessage(args.Context, WorldGenerator.instance.m_world.m_seedName);
		}
	}

	public SeedCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				Helper.AddMessage(args.Context, WorldGenerator.instance.m_world.m_seedName);
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		Helper.Command("seed", "Prints the world seed.", (ConsoleEvent)obj);
		AutoComplete.RegisterEmpty("seed");
	}
}
