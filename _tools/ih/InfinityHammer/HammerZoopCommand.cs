using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;

namespace InfinityHammer;

public class HammerZoopCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<int, List<string>> _003C_003E9__1_0;

		public static ConsoleEvent _003C_003E9__1_1;

		internal List<string> _003C_002Ector_003Eb__1_0(int index)
		{
			return ParameterInfo.None;
		}

		internal void _003C_002Ector_003Eb__1_1(ConsoleEventArgs args)
		{
			HammerHelper.GetPlacementGhost();
			if (Selection.Get() is ObjectSelection objectSelection)
			{
				objectSelection.ZoopReset();
			}
		}
	}

	private static void Command(string direction, string action, string reverse)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		string direction2 = direction;
		string reverse2 = reverse;
		string action2 = action;
		AutoComplete.Register("hammer_zoop_" + direction2, (Func<int, List<string>>)((int index) => (index == 0) ? ParameterInfo.Create("Meters towards the " + direction2 + " direction (<color=yellow>number</color> or <color=yellow>number*auto</color> for automatic step size).") : ParameterInfo.None));
		Helper.Command("hammer_zoop_" + direction2, "[value=auto] - Zoops towards the " + direction2 + " direction.", (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			HammerHelper.GetPlacementGhost();
			string offset = "auto";
			if (args.Args.Length > 1)
			{
				offset = args[1];
			}
			string text = ((Parse.Direction(args.Args, 2) < 0f) ? reverse2 : action2);
			if (!(Selection.Get() is ObjectSelection objectSelection))
			{
				return;
			}
			try
			{
				switch (text)
				{
				case "left":
					objectSelection.ZoopLeft(offset);
					break;
				case "right":
					objectSelection.ZoopRight(offset);
					break;
				case "up":
					objectSelection.ZoopUp(offset);
					break;
				case "down":
					objectSelection.ZoopDown(offset);
					break;
				case "forward":
					objectSelection.ZoopForward(offset);
					break;
				case "backward":
					objectSelection.ZoopBackward(offset);
					break;
				}
			}
			catch (InvalidOperationException ex)
			{
				objectSelection.ZoopReset();
				throw ex;
			}
		});
	}

	public HammerZoopCommand()
	{
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		Command("left", "left", "right");
		Command("right", "right", "left");
		Command("down", "down", "up");
		Command("up", "up", "down");
		Command("backward", "backward", "forward");
		Command("forward", "forward", "backward");
		AutoComplete.Register("hammer_zoop_reset", (Func<int, List<string>>)((int index) => ParameterInfo.None));
		object obj = _003C_003Ec._003C_003E9__1_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate
			{
				HammerHelper.GetPlacementGhost();
				if (Selection.Get() is ObjectSelection objectSelection)
				{
					objectSelection.ZoopReset();
				}
			};
			_003C_003Ec._003C_003E9__1_1 = val;
			obj = (object)val;
		}
		Helper.Command("hammer_zoop_reset", "- Resets the zoop offset.", (ConsoleEvent)obj);
	}
}
