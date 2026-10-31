using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class HammerOffsetCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<int, int, List<string>> _003C_003E9__1_0;

		public static ConsoleEvent _003C_003E9__1_1;

		internal List<string> _003C_002Ector_003Eb__1_0(int index, int subIndex)
		{
			if (index == 0)
			{
				return ParameterInfo.FRU("Sets the offset", subIndex);
			}
			return ParameterInfo.None;
		}

		internal void _003C_002Ector_003Eb__1_1(ConsoleEventArgs args)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			Vector3 value = Vector3.zero;
			if (args.Length > 1)
			{
				value = Parse.VectorZYX(args[1]);
			}
			Position.Set(value);
			Position.Print(args.Context);
		}
	}

	private static void Command(string name, string direction, Action<float> action)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		string direction2 = direction;
		Action<float> action2 = action;
		AutoComplete.Register("hammer_offset_" + name, (Func<int, List<string>>)((int index) => (index == 0) ? ParameterInfo.Create("Meters in the " + direction2 + " direction.") : ParameterInfo.None));
		Helper.Command("hammer_offset_" + name, "[value=0] - Sets the " + direction2 + " offset.", (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			action2(Parse.Float(args[1], 0f));
			Position.Print(args.Context);
		});
	}

	public HammerOffsetCommand()
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		Command("x", "right / left", Position.SetX);
		Command("y", "up / down", Position.SetY);
		Command("z", "forward / backward", Position.SetZ);
		AutoComplete.Register("hammer_offset", (Func<int, int, List<string>>)((int index, int subIndex) => (index == 0) ? ParameterInfo.FRU("Sets the offset", subIndex) : ParameterInfo.None));
		object obj = _003C_003Ec._003C_003E9__1_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				Vector3 value = Vector3.zero;
				if (args.Length > 1)
				{
					value = Parse.VectorZYX(args[1]);
				}
				Position.Set(value);
				Position.Print(args.Context);
			};
			_003C_003Ec._003C_003E9__1_1 = val;
			obj = (object)val;
		}
		Helper.Command("hammer_offset", "[forward,up,right=0,0,0] - Sets the offset.", (ConsoleEvent)obj);
	}
}
