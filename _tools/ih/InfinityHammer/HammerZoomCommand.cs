using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;

namespace InfinityHammer;

public class HammerZoomCommand
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

		public static Func<ScalingData, Action<float>> _003C_003E9__3_3;

		public static Func<ScalingData, Action<float>> _003C_003E9__3_4;

		public static Func<ScalingData, Action<float>> _003C_003E9__3_5;

		internal List<string> _003CCommandAxis_003Eb__1_0(int index)
		{
			if (index == 0)
			{
				return ParameterInfo.Create("Flat amount or percentage to scale.");
			}
			return ParameterInfo.None;
		}

		internal List<string> _003CCommand_003Eb__2_0(int index, int subIndex)
		{
			if (index == 0)
			{
				return ParameterInfo.Create("Flat amount or percentage to scale.");
			}
			return ParameterInfo.None;
		}

		internal void _003CCommand_003Eb__2_1(ConsoleEventArgs args)
		{
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			Helper.ArgsCheck(args, 2, "Missing the amount.");
			BaseSelection baseSelection = Selection.Get();
			if (!((Character)Helper.GetPlayer()).InPlaceMode())
			{
				return;
			}
			if (!baseSelection.IsScalingSupported())
			{
				HammerHelper.Message(args.Context, "Selected object doesn't support scaling.");
				return;
			}
			ScalingData scalingData = Scaling.Get();
			string[] array = args[1].Split(',');
			string direction = ((args.Length > 2) ? args[2] : "");
			if (array.Length == 1)
			{
				Zoom(array[0], direction, scalingData.Zoom, scalingData.ZoomPercentage);
			}
			else
			{
				if (array.Length != 3)
				{
					throw new InvalidOperationException("Must either have 1 or 3 values.");
				}
				Zoom(array[0], direction, scalingData.ZoomX, scalingData.ZoomXPercentage);
				Zoom(array[1], direction, scalingData.ZoomZ, scalingData.ZoomZPercentage);
				Zoom(array[2], direction, scalingData.ZoomY, scalingData.ZoomYPercentage);
			}
			baseSelection.SetScale(scalingData.Vec3);
			Scaling.Print(args.Context);
		}

		internal Action<float> _003C_002Ector_003Eb__3_0(ScalingData scale)
		{
			return scale.ZoomX;
		}

		internal Action<float> _003C_002Ector_003Eb__3_1(ScalingData scale)
		{
			return scale.ZoomXPercentage;
		}

		internal Action<float> _003C_002Ector_003Eb__3_2(ScalingData scale)
		{
			return scale.ZoomY;
		}

		internal Action<float> _003C_002Ector_003Eb__3_3(ScalingData scale)
		{
			return scale.ZoomYPercentage;
		}

		internal Action<float> _003C_002Ector_003Eb__3_4(ScalingData scale)
		{
			return scale.ZoomZ;
		}

		internal Action<float> _003C_002Ector_003Eb__3_5(ScalingData scale)
		{
			return scale.ZoomZPercentage;
		}
	}

	private static void Zoom(string amountStr, string direction, Action<float> amountAction, Action<float> percentageAction)
	{
		if (amountStr.EndsWith("%", StringComparison.Ordinal))
		{
			float obj = Parse.Direction(direction) * Parse.Float(amountStr.Substring(0, amountStr.Length - 1), 5f) / 100f;
			percentageAction(obj);
		}
		else
		{
			float obj2 = Parse.Direction(direction) * Parse.Float(amountStr, 1f);
			amountAction(obj2);
		}
	}

	private static void CommandAxis(string name, string axis, Func<ScalingData, Action<float>> amountAction, Func<ScalingData, Action<float>> percentageAction)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		Func<ScalingData, Action<float>> amountAction2 = amountAction;
		Func<ScalingData, Action<float>> percentageAction2 = percentageAction;
		name = name + "_" + axis;
		AutoComplete.Register(name, (Func<int, List<string>>)((int index) => (index == 0) ? ParameterInfo.Create("Flat amount or percentage to scale.") : ParameterInfo.None));
		Helper.Command(name, "[amount or percentage] - Zooms the " + axis + " axis (if the object supports it).", (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
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
					Zoom(args[1], direction, amountAction2(scalingData), percentageAction2(scalingData));
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
		AutoComplete.Register(name, (Func<int, int, List<string>>)((int index, int subIndex) => (index == 0) ? ParameterInfo.Create("Flat amount or percentage to scale.") : ParameterInfo.None));
		object obj = _003C_003Ec._003C_003E9__2_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_010f: Unknown result type (might be due to invalid IL or missing references)
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
						ScalingData scalingData = Scaling.Get();
						string[] array = args[1].Split(',');
						string direction = ((args.Length > 2) ? args[2] : "");
						if (array.Length == 1)
						{
							Zoom(array[0], direction, scalingData.Zoom, scalingData.ZoomPercentage);
						}
						else
						{
							if (array.Length != 3)
							{
								throw new InvalidOperationException("Must either have 1 or 3 values.");
							}
							Zoom(array[0], direction, scalingData.ZoomX, scalingData.ZoomXPercentage);
							Zoom(array[1], direction, scalingData.ZoomZ, scalingData.ZoomZPercentage);
							Zoom(array[2], direction, scalingData.ZoomY, scalingData.ZoomYPercentage);
						}
						baseSelection.SetScale(scalingData.Vec3);
						Scaling.Print(args.Context);
					}
				}
			};
			_003C_003Ec._003C_003E9__2_1 = val;
			obj = (object)val;
		}
		Helper.Command(name, "[amount/percentage or x,z,y] - Zooms the selection (if the object supports it).", (ConsoleEvent)obj);
	}

	public HammerZoomCommand()
	{
		CommandAxis("hammer_zoom", "x", (ScalingData scale) => scale.ZoomX, (ScalingData scale) => scale.ZoomXPercentage);
		CommandAxis("hammer_zoom", "y", (ScalingData scale) => scale.ZoomY, (ScalingData scale) => scale.ZoomYPercentage);
		CommandAxis("hammer_zoom", "z", (ScalingData scale) => scale.ZoomZ, (ScalingData scale) => scale.ZoomZPercentage);
		Command("hammer_zoom");
	}
}
