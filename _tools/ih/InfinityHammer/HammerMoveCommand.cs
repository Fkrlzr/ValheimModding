using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class HammerMoveCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Action<ConsoleEventArgs, GameObject> _003C_003E9__1_0;

		public static Action<ConsoleEventArgs, GameObject> _003C_003E9__1_1;

		public static Action<ConsoleEventArgs, GameObject> _003C_003E9__1_2;

		public static Action<ConsoleEventArgs, GameObject> _003C_003E9__1_3;

		public static Action<ConsoleEventArgs, GameObject> _003C_003E9__1_4;

		public static Action<ConsoleEventArgs, GameObject> _003C_003E9__1_5;

		public static Func<int, int, List<string>> _003C_003E9__1_6;

		public static ConsoleEvent _003C_003E9__1_7;

		internal void _003C_002Ector_003Eb__1_0(ConsoleEventArgs args, GameObject ghost)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			float x = HammerHelper.TryParseSize(ghost, args.Args, 1).x;
			Position.MoveLeft(Parse.Direction(args.Args, 2) * x);
		}

		internal void _003C_002Ector_003Eb__1_1(ConsoleEventArgs args, GameObject ghost)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			float x = HammerHelper.TryParseSize(ghost, args.Args, 1).x;
			Position.MoveRight(Parse.Direction(args.Args, 2) * x);
		}

		internal void _003C_002Ector_003Eb__1_2(ConsoleEventArgs args, GameObject ghost)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			float y = HammerHelper.TryParseSize(ghost, args.Args, 1).y;
			Position.MoveDown(Parse.Direction(args.Args, 2) * y);
		}

		internal void _003C_002Ector_003Eb__1_3(ConsoleEventArgs args, GameObject ghost)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			float y = HammerHelper.TryParseSize(ghost, args.Args, 1).y;
			Position.MoveUp(Parse.Direction(args.Args, 2) * y);
		}

		internal void _003C_002Ector_003Eb__1_4(ConsoleEventArgs args, GameObject ghost)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			float z = HammerHelper.TryParseSize(ghost, args.Args, 1).z;
			Position.MoveBackward(Parse.Direction(args.Args, 2) * z);
		}

		internal void _003C_002Ector_003Eb__1_5(ConsoleEventArgs args, GameObject ghost)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			float z = HammerHelper.TryParseSize(ghost, args.Args, 1).z;
			Position.MoveForward(Parse.Direction(args.Args, 2) * z);
		}

		internal List<string> _003C_002Ector_003Eb__1_6(int index, int subIndex)
		{
			if (index == 0)
			{
				return ParameterInfo.FRU("Meters to move the placement", subIndex);
			}
			return ParameterInfo.None;
		}

		internal void _003C_002Ector_003Eb__1_7(ConsoleEventArgs args)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			Position.Move(HammerHelper.TryParseSizesZYX(HammerHelper.GetPlacementGhost(), args.Args, 1, "0"));
			Position.Print(args.Context);
		}
	}

	private static void Command(string direction, Action<ConsoleEventArgs, GameObject> action)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		string direction2 = direction;
		Action<ConsoleEventArgs, GameObject> action2 = action;
		AutoComplete.Register("hammer_move_" + direction2, (Func<int, List<string>>)((int index) => (index == 0) ? ParameterInfo.Create("Meters towards the " + direction2 + " direction (<color=yellow>number</color> or <color=yellow>number*auto</color> for automatic step size).") : ParameterInfo.None));
		Helper.Command("hammer_move_" + direction2, "[value=auto] - Moves the placement towards the " + direction2 + " direction.", (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			GameObject placementGhost = HammerHelper.GetPlacementGhost();
			action2(args, placementGhost);
			Position.Print(args.Context);
		});
	}

	public HammerMoveCommand()
	{
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		Command("left", delegate(ConsoleEventArgs args, GameObject ghost)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			float x2 = HammerHelper.TryParseSize(ghost, args.Args, 1).x;
			Position.MoveLeft(Parse.Direction(args.Args, 2) * x2);
		});
		Command("right", delegate(ConsoleEventArgs args, GameObject ghost)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			float x = HammerHelper.TryParseSize(ghost, args.Args, 1).x;
			Position.MoveRight(Parse.Direction(args.Args, 2) * x);
		});
		Command("down", delegate(ConsoleEventArgs args, GameObject ghost)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			float y2 = HammerHelper.TryParseSize(ghost, args.Args, 1).y;
			Position.MoveDown(Parse.Direction(args.Args, 2) * y2);
		});
		Command("up", delegate(ConsoleEventArgs args, GameObject ghost)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			float y = HammerHelper.TryParseSize(ghost, args.Args, 1).y;
			Position.MoveUp(Parse.Direction(args.Args, 2) * y);
		});
		Command("backward", delegate(ConsoleEventArgs args, GameObject ghost)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			float z2 = HammerHelper.TryParseSize(ghost, args.Args, 1).z;
			Position.MoveBackward(Parse.Direction(args.Args, 2) * z2);
		});
		Command("forward", delegate(ConsoleEventArgs args, GameObject ghost)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			float z = HammerHelper.TryParseSize(ghost, args.Args, 1).z;
			Position.MoveForward(Parse.Direction(args.Args, 2) * z);
		});
		AutoComplete.Register("hammer_move", (Func<int, int, List<string>>)((int index, int subIndex) => (index == 0) ? ParameterInfo.FRU("Meters to move the placement", subIndex) : ParameterInfo.None));
		object obj = _003C_003Ec._003C_003E9__1_7;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				Position.Move(HammerHelper.TryParseSizesZYX(HammerHelper.GetPlacementGhost(), args.Args, 1, "0"));
				Position.Print(args.Context);
			};
			_003C_003Ec._003C_003E9__1_7 = val;
			obj = (object)val;
		}
		Helper.Command("hammer_move", "[forward,up,right] - Moves the placement.", (ConsoleEvent)obj);
	}
}
