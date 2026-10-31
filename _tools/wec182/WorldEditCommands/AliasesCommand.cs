using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;

namespace WorldEditCommands;

public class AliasesCommand
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
			string substitution = Settings.Substitution;
			if (args.Length > 1 && args[1] == "clear")
			{
				args.Context.TryRunCommand("alias move", false, false);
				args.Context.TryRunCommand("alias rotate", false, false);
				args.Context.TryRunCommand("alias scal", false, false);
				args.Context.TryRunCommand("alias stars", false, false);
				args.Context.TryRunCommand("alias health", false, false);
				args.Context.TryRunCommand("alias remove", false, false);
				args.Context.TryRunCommand("alias change_helmet", false, false);
				args.Context.TryRunCommand("alias change_left", false, false);
				args.Context.TryRunCommand("alias change_right", false, false);
				args.Context.TryRunCommand("alias change_legs", false, false);
				args.Context.TryRunCommand("alias change_chest", false, false);
				args.Context.TryRunCommand("alias change_shoulders", false, false);
				args.Context.TryRunCommand("alias change_utility", false, false);
				args.Context.TryRunCommand("alias essential", false, false);
				args.Context.TryRunCommand("alias spawn", false, false);
				return;
			}
			args.Context.TryRunCommand("alias move object move=" + substitution + "," + substitution + " radius=" + substitution + " id=" + substitution, false, false);
			args.Context.TryRunCommand("alias rotate object rotate=" + substitution + "," + substitution + " radius=" + substitution + " id=" + substitution, false, false);
			args.Context.TryRunCommand("alias scale object scale=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
			args.Context.TryRunCommand("alias stars object stars=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
			args.Context.TryRunCommand("alias health object stars=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
			args.Context.TryRunCommand("alias remove object remove=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
			args.Context.TryRunCommand("alias change_helmet object helmet=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
			args.Context.TryRunCommand("alias change_left object left_hand=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
			args.Context.TryRunCommand("alias change_right object right_hand=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
			args.Context.TryRunCommand("alias change_legs object legs=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
			args.Context.TryRunCommand("alias change_chest object chest=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
			args.Context.TryRunCommand("alias change_shoulders object shoulders=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
			args.Context.TryRunCommand("alias change_utility object utility=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
			args.Context.TryRunCommand("alias essential object tame health=1E30 radius=" + substitution + " id=" + substitution, false, false);
			args.Context.TryRunCommand("alias spawn spawn_object " + substitution + " amount=" + substitution + " level=" + substitution, false, false);
		}

		internal List<string> _003C_002Ector_003Eb__0_1(int index)
		{
			if (index == 0)
			{
				return new List<string>(2) { "set", "clear" };
			}
			return ParameterInfo.None;
		}
	}

	public AliasesCommand()
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
				string substitution = Settings.Substitution;
				if (args.Length > 1 && args[1] == "clear")
				{
					args.Context.TryRunCommand("alias move", false, false);
					args.Context.TryRunCommand("alias rotate", false, false);
					args.Context.TryRunCommand("alias scal", false, false);
					args.Context.TryRunCommand("alias stars", false, false);
					args.Context.TryRunCommand("alias health", false, false);
					args.Context.TryRunCommand("alias remove", false, false);
					args.Context.TryRunCommand("alias change_helmet", false, false);
					args.Context.TryRunCommand("alias change_left", false, false);
					args.Context.TryRunCommand("alias change_right", false, false);
					args.Context.TryRunCommand("alias change_legs", false, false);
					args.Context.TryRunCommand("alias change_chest", false, false);
					args.Context.TryRunCommand("alias change_shoulders", false, false);
					args.Context.TryRunCommand("alias change_utility", false, false);
					args.Context.TryRunCommand("alias essential", false, false);
					args.Context.TryRunCommand("alias spawn", false, false);
				}
				else
				{
					args.Context.TryRunCommand("alias move object move=" + substitution + "," + substitution + " radius=" + substitution + " id=" + substitution, false, false);
					args.Context.TryRunCommand("alias rotate object rotate=" + substitution + "," + substitution + " radius=" + substitution + " id=" + substitution, false, false);
					args.Context.TryRunCommand("alias scale object scale=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
					args.Context.TryRunCommand("alias stars object stars=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
					args.Context.TryRunCommand("alias health object stars=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
					args.Context.TryRunCommand("alias remove object remove=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
					args.Context.TryRunCommand("alias change_helmet object helmet=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
					args.Context.TryRunCommand("alias change_left object left_hand=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
					args.Context.TryRunCommand("alias change_right object right_hand=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
					args.Context.TryRunCommand("alias change_legs object legs=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
					args.Context.TryRunCommand("alias change_chest object chest=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
					args.Context.TryRunCommand("alias change_shoulders object shoulders=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
					args.Context.TryRunCommand("alias change_utility object utility=" + substitution + " radius=" + substitution + " id=" + substitution, false, false);
					args.Context.TryRunCommand("alias essential object tame health=1E30 radius=" + substitution + " id=" + substitution, false, false);
					args.Context.TryRunCommand("alias spawn spawn_object " + substitution + " amount=" + substitution + " level=" + substitution, false, false);
				}
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		new ConsoleCommand("world_edit_aliases", "[set/clear] - Sets some useful aliases.", (ConsoleEvent)obj, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.Register("world_edit_aliases", (Func<int, List<string>>)((int index) => (index == 0) ? new List<string>(2) { "set", "clear" } : ParameterInfo.None));
	}
}
