using System;
using System.Runtime.CompilerServices;

namespace ServerDevcommands;

public class UndoRedoCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__0_0;

		public static ConsoleEvent _003C_003E9__0_1;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			UndoManager.Undo(args.Context);
		}

		internal void _003C_002Ector_003Eb__0_1(ConsoleEventArgs args)
		{
			UndoManager.Redo(args.Context);
		}
	}

	public UndoRedoCommand()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				UndoManager.Undo(args.Context);
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		new ConsoleCommand("undo", "Reverts some commands.", (ConsoleEvent)obj, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj2 = _003C_003Ec._003C_003E9__0_1;
		if (obj2 == null)
		{
			ConsoleEvent val2 = delegate(ConsoleEventArgs args)
			{
				UndoManager.Redo(args.Context);
			};
			_003C_003Ec._003C_003E9__0_1 = val2;
			obj2 = (object)val2;
		}
		new ConsoleCommand("redo", "Restores reverted commands.", (ConsoleEvent)obj2, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.RegisterEmpty("undo");
		AutoComplete.RegisterEmpty("redo");
	}
}
