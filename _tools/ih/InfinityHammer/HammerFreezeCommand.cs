using System;
using System.Runtime.CompilerServices;
using ServerDevcommands;

namespace InfinityHammer;

public class HammerFreezeCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__0_0;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			if (args.Length > 1)
			{
				if (Parse.Boolean(args[1]).GetValueOrDefault())
				{
					Position.Freeze();
				}
				else if (Parse.Boolean(args[1]) == false)
				{
					Position.Unfreeze();
				}
				else
				{
					Position.ToggleFreeze();
				}
			}
			else
			{
				Position.ToggleFreeze();
			}
		}
	}

	public HammerFreezeCommand()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		AutoComplete.RegisterEmpty("hammer_freeze");
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				if (args.Length > 1)
				{
					if (Parse.Boolean(args[1]).GetValueOrDefault())
					{
						Position.Freeze();
					}
					else if (Parse.Boolean(args[1]) == false)
					{
						Position.Unfreeze();
					}
					else
					{
						Position.ToggleFreeze();
					}
				}
				else
				{
					Position.ToggleFreeze();
				}
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		new ConsoleCommand("hammer_freeze", "[true/false] - Toggles whether the mouse affects placement position.", (ConsoleEvent)obj, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
	}
}
