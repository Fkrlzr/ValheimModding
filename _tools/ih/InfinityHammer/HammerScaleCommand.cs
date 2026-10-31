using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;

namespace InfinityHammer;

public class HammerScaleCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<int, List<string>> _003C_003E9__1_0;

		public static Func<int, int, List<string>> _003C_003E9__2_0;

		public static ConsoleEvent _003C_003E9__2_1;

		public static Func<ScalingData, Action<float>> _003C_003E9__3_0;

		public static Func<ScalingData, Action<float>> _003C_003E9__3_1;

		public static Func<ScalingData, Action<float>> _003C_003E9__3_2;

		internal List<string> _003CCommandAxis_003Eb__1_0(int index)
		{
			if (index == 0)
			{
				return ParameterInfo.Create("Amount.");
			}
			return ParameterInfo.None;
		}

		internal List<string> _003CCommand_003Eb__2_0(int index, int subIndex)
		{
			if (index == 0)
			{
				return ParameterInfo.XZY("Amount of scale.", subIndex);
			}
			return ParameterInfo.None;
		}

		internal void _003CCommand_003Eb__2_1(ConsoleEventArgs args)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			BaseSelection baseSelection = Selection.Get();
			if (((Character)Helper.GetPlayer()).InPlaceMode())
			{
				if (!baseSelection.IsScalingSupported())
				{
					HammerHelper.Message(args.Context, "Selected object doesn't support scaling.");
					return;
				}
				ScalingData scalingData = Scaling.Get();
				scalingData.SetScale(Parse.Scale(Parse.Split(args[1], ',')) * Parse.Direction(args.Args, 2));
				baseSelection.SetScale(scalingData.Vec3);
				Scaling.Print(args.Context);
			}
		}

		internal Action<float> _003C_002Ector_003Eb__3_0(ScalingData scale)
		{
			return scale.SetScaleX;
		}

		internal Action<float> _003C_002Ector_003Eb__3_1(ScalingData scale)
		{
			return scale.SetScaleY;
		}

		internal Action<float> _003C_002Ector_003Eb__3_2(ScalingData scale)
		{
			return scale.SetScaleZ;
		}
	}

	private static void Scale(string amountStr, string direction, Action<float> action)
	{
		float obj = Parse.Direction(direction) * Parse.Float(amountStr, 1f);
		action(obj);
	}

	private static void CommandAxis(string name, string axis, Func<ScalingData, Action<float>> action)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		Func<ScalingData, Action<float>> action2 = action;
		name = name + "_" + axis;
		AutoComplete.Register(name, (Func<int, List<string>>)((int index) => (index == 0) ? ParameterInfo.Create("Amount.") : ParameterInfo.None));
		Helper.Command(name, "[amount] - Sets the scale of " + axis + " axis (if the object supports it).", (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			Helper.ArgsCheck(args, 2, "Missing the amount.");
			BaseSelection baseSelection = Selection.Get();
			if (((Character)Helper.GetPlayer()).InPlaceMode())
			{
				if (!baseSelection.IsScalingSupported())
				{
					HammerHelper.Message(args.Context, "Selected object doesn't support scaling.");
				}
				else
				{
					string direction = ((args.Length > 2) ? args[2] : "");
					ScalingData scalingData = Scaling.Get();
					Scale(args[1], direction, action2(scalingData));
					baseSelection.SetScale(scalingData.Vec3);
					Scaling.Print(args.Context);
				}
			}
		});
	}

	private static void Command(string name)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		AutoComplete.Register(name, (Func<int, int, List<string>>)((int index, int subIndex) => (index == 0) ? ParameterInfo.XZY("Amount of scale.", subIndex) : ParameterInfo.None));
		object obj = _003C_003Ec._003C_003E9__2_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				BaseSelection baseSelection = Selection.Get();
				if (((Character)Helper.GetPlayer()).InPlaceMode())
				{
					if (!baseSelection.IsScalingSupported())
					{
						HammerHelper.Message(args.Context, "Selected object doesn't support scaling.");
					}
					else
					{
						ScalingData scalingData = Scaling.Get();
						scalingData.SetScale(Parse.Scale(Parse.Split(args[1], ',')) * Parse.Direction(args.Args, 2));
						baseSelection.SetScale(scalingData.Vec3);
						Scaling.Print(args.Context);
					}
				}
			};
			_003C_003Ec._003C_003E9__2_1 = val;
			obj = (object)val;
		}
		Helper.Command(name, "[amount or x,z,y] - Sets the scale (if the object supports it).", (ConsoleEvent)obj);
	}

	public HammerScaleCommand()
	{
		CommandAxis("hammer_scale", "x", (ScalingData scale) => scale.SetScaleX);
		CommandAxis("hammer_scale", "y", (ScalingData scale) => scale.SetScaleY);
		CommandAxis("hammer_scale", "z", (ScalingData scale) => scale.SetScaleZ);
		Command("hammer_scale");
	}
}
