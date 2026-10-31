using System;
using System.Runtime.CompilerServices;
using ServerDevcommands;

namespace InfinityHammer;

public class HammerMirrorCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__0_0;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			(((ObjectSelection)Selection.Get()) ?? throw new InvalidOperationException("Mirroring only works for multiple objects.")).Mirror();
			HammerHelper.Message(args.Context, "Mirrored the selection.");
		}
	}

	public HammerMirrorCommand()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		AutoComplete.RegisterEmpty("hammer_mirror");
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				(((ObjectSelection)Selection.Get()) ?? throw new InvalidOperationException("Mirroring only works for multiple objects.")).Mirror();
				HammerHelper.Message(args.Context, "Mirrored the selection.");
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		Helper.Command("hammer_mirror", " Mirrors the selection.", (ConsoleEvent)obj);
	}
}
