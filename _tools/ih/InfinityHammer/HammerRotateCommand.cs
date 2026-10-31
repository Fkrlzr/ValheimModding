using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class HammerRotateCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<int, List<string>> _003C_003E9__1_0;

		public static ConsoleEvent _003C_003E9__1_1;

		public static Func<int, List<string>> _003C_003E9__1_2;

		public static ConsoleEvent _003C_003E9__1_3;

		public static Func<int, List<string>> _003C_003E9__1_4;

		public static ConsoleEvent _003C_003E9__1_5;

		public static Func<int, List<string>> _003C_003E9__1_6;

		public static ConsoleEvent _003C_003E9__1_7;

		internal List<string> _003C_002Ector_003Eb__1_0(int index)
		{
			if (index == 0)
			{
				return ParameterInfo.Create("Degrees to rotate. <color=yellow>step*random</color> randomly rotates within a given precision or <color=yellow>random</color> for 90/180 degrees depending on the object shape.");
			}
			return ParameterInfo.None;
		}

		internal void _003C_002Ector_003Eb__1_1(ConsoleEventArgs args)
		{
			if (args.Length >= 2)
			{
				PlaceRotation.RotateX(ParseArgs(args.Args, HammerHelper.IsSquareX));
			}
		}

		internal List<string> _003C_002Ector_003Eb__1_2(int index)
		{
			if (index == 0)
			{
				return ParameterInfo.Create("Degrees to rotate. <color=yellow>step*random</color> randomly rotates within a given precision or <color=yellow>random</color> for 90/180 degrees depending on the object shape.");
			}
			return ParameterInfo.None;
		}

		internal void _003C_002Ector_003Eb__1_3(ConsoleEventArgs args)
		{
			if (args.Length >= 2)
			{
				PlaceRotation.RotateY(ParseArgs(args.Args, HammerHelper.IsSquareY));
			}
		}

		internal List<string> _003C_002Ector_003Eb__1_4(int index)
		{
			if (index == 0)
			{
				return ParameterInfo.Create("Degrees to rotate. <color=yellow>step*random</color> randomly rotates within a given precision or <color=yellow>random</color> for 90/180 degrees depending on the object shape.");
			}
			return ParameterInfo.None;
		}

		internal void _003C_002Ector_003Eb__1_5(ConsoleEventArgs args)
		{
			if (args.Length >= 2)
			{
				PlaceRotation.RotateZ(ParseArgs(args.Args, HammerHelper.IsSquareZ));
			}
		}

		internal List<string> _003C_002Ector_003Eb__1_6(int index)
		{
			if (index == 0)
			{
				return ParameterInfo.Create("Direction (1 or -1) to rotate.)");
			}
			return ParameterInfo.None;
		}

		internal void _003C_002Ector_003Eb__1_7(ConsoleEventArgs args)
		{
			if (args.Length >= 2)
			{
				Helper.GetPlayer();
				if (Parse.Direction(args.Args, 1) > 0f)
				{
					PlaceRotation.RotateY(-22.5f);
				}
				else
				{
					PlaceRotation.RotateY(22.5f);
				}
			}
		}
	}

	private static float ParseArgs(string[] values, Func<GameObject, bool> isSquare)
	{
		if (values[1].Contains("random"))
		{
			GameObject placementGhost = HammerHelper.GetPlacementGhost();
			Random random = new Random();
			int num = (int)Parse.Multiplier(values[1]);
			if (num == 1)
			{
				if (isSquare(placementGhost))
				{
					return 90f * (float)random.Next(0, 4);
				}
				return 180f * (float)random.Next(0, 2);
			}
			return 360f / (float)num * (float)random.Next(0, num + 1);
		}
		return Parse.Direction(values, 2) * Parse.Float(values[1], 0f);
	}

	public HammerRotateCommand()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		AutoComplete.Register("hammer_rotate_x", (Func<int, List<string>>)((int index) => (index == 0) ? ParameterInfo.Create("Degrees to rotate. <color=yellow>step*random</color> randomly rotates within a given precision or <color=yellow>random</color> for 90/180 degrees depending on the object shape.") : ParameterInfo.None));
		object obj = _003C_003Ec._003C_003E9__1_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				if (args.Length >= 2)
				{
					PlaceRotation.RotateX(ParseArgs(args.Args, HammerHelper.IsSquareX));
				}
			};
			_003C_003Ec._003C_003E9__1_1 = val;
			obj = (object)val;
		}
		new ConsoleCommand("hammer_rotate_x", "[degrees/random/number*random] - Rotates around the X axis.", (ConsoleEvent)obj, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.Register("hammer_rotate_y", (Func<int, List<string>>)((int index) => (index == 0) ? ParameterInfo.Create("Degrees to rotate. <color=yellow>step*random</color> randomly rotates within a given precision or <color=yellow>random</color> for 90/180 degrees depending on the object shape.") : ParameterInfo.None));
		object obj2 = _003C_003Ec._003C_003E9__1_3;
		if (obj2 == null)
		{
			ConsoleEvent val2 = delegate(ConsoleEventArgs args)
			{
				if (args.Length >= 2)
				{
					PlaceRotation.RotateY(ParseArgs(args.Args, HammerHelper.IsSquareY));
				}
			};
			_003C_003Ec._003C_003E9__1_3 = val2;
			obj2 = (object)val2;
		}
		new ConsoleCommand("hammer_rotate_y", "[degrees/random/number*random] - Rotates around the Y axis.", (ConsoleEvent)obj2, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.Register("hammer_rotate_z", (Func<int, List<string>>)((int index) => (index == 0) ? ParameterInfo.Create("Degrees to rotate. <color=yellow>step*random</color> randomly rotates within a given precision or <color=yellow>random</color> for 90/180 degrees depending on the object shape.") : ParameterInfo.None));
		object obj3 = _003C_003Ec._003C_003E9__1_5;
		if (obj3 == null)
		{
			ConsoleEvent val3 = delegate(ConsoleEventArgs args)
			{
				if (args.Length >= 2)
				{
					PlaceRotation.RotateZ(ParseArgs(args.Args, HammerHelper.IsSquareZ));
				}
			};
			_003C_003Ec._003C_003E9__1_5 = val3;
			obj3 = (object)val3;
		}
		new ConsoleCommand("hammer_rotate_z", "[degrees/random/number*random] - Rotates around the Z axis.", (ConsoleEvent)obj3, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.Register("hammer_rotate", (Func<int, List<string>>)((int index) => (index == 0) ? ParameterInfo.Create("Direction (1 or -1) to rotate.)") : ParameterInfo.None));
		object obj4 = _003C_003Ec._003C_003E9__1_7;
		if (obj4 == null)
		{
			ConsoleEvent val4 = delegate(ConsoleEventArgs args)
			{
				if (args.Length >= 2)
				{
					Helper.GetPlayer();
					if (Parse.Direction(args.Args, 1) > 0f)
					{
						PlaceRotation.RotateY(-22.5f);
					}
					else
					{
						PlaceRotation.RotateY(22.5f);
					}
				}
			};
			_003C_003Ec._003C_003E9__1_7 = val4;
			obj4 = (object)val4;
		}
		new ConsoleCommand("hammer_rotate", "[direction] - Simulates normal rotating.", (ConsoleEvent)obj4, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
	}
}
