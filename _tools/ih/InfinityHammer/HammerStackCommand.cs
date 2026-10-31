using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;
using Service;
using UnityEngine;
using WorldEditCommands;

namespace InfinityHammer;

public class HammerStackCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<Range<int>, Range<Vector3Int>> _003C_003E9__4_0;

		public static Func<Range<int>, Range<Vector3Int>> _003C_003E9__4_1;

		public static Func<Range<int>, Range<Vector3Int>> _003C_003E9__4_2;

		public static Func<Range<int>, Range<Vector3Int>> _003C_003E9__4_3;

		public static Func<Range<int>, Range<Vector3Int>> _003C_003E9__4_4;

		public static Func<Range<int>, Range<Vector3Int>> _003C_003E9__4_5;

		public static Func<int, int, List<string>> _003C_003E9__4_6;

		public static ConsoleEvent _003C_003E9__4_7;

		internal Range<Vector3Int> _003C_002Ector_003Eb__4_0(Range<int> amount)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return new Range<Vector3Int>(new Vector3Int(-amount.Min, 0, 0), new Vector3Int(-amount.Max, 0, 0));
		}

		internal Range<Vector3Int> _003C_002Ector_003Eb__4_1(Range<int> amount)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			return new Range<Vector3Int>(new Vector3Int(0, amount.Min, 0), new Vector3Int(0, amount.Max, 0));
		}

		internal Range<Vector3Int> _003C_002Ector_003Eb__4_2(Range<int> amount)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return new Range<Vector3Int>(new Vector3Int(0, -amount.Min, 0), new Vector3Int(0, -amount.Max, 0));
		}

		internal Range<Vector3Int> _003C_002Ector_003Eb__4_3(Range<int> amount)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			return new Range<Vector3Int>(new Vector3Int(0, amount.Min, 0), new Vector3Int(0, amount.Max, 0));
		}

		internal Range<Vector3Int> _003C_002Ector_003Eb__4_4(Range<int> amount)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return new Range<Vector3Int>(new Vector3Int(0, 0, -amount.Min), new Vector3Int(0, 0, -amount.Max));
		}

		internal Range<Vector3Int> _003C_002Ector_003Eb__4_5(Range<int> amount)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			return new Range<Vector3Int>(new Vector3Int(0, 0, amount.Min), new Vector3Int(0, 0, amount.Max));
		}

		internal List<string> _003C_002Ector_003Eb__4_6(int index, int subIndex)
		{
			return index switch
			{
				0 => ParameterInfo.FRU("Amounts", subIndex), 
				1 => ParameterInfo.FRU("Step size (<color=yellow>number</color> or <color=yellow>number*auto</color> for automatic step size)", subIndex), 
				_ => ParameterInfo.None, 
			};
		}

		internal void _003C_002Ector_003Eb__4_7(ConsoleEventArgs args)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			Helper.ArgsCheck(args, 2, "Missing the amount.");
			Hammer.Equip();
			GameObject obj = SelectTarget();
			Range<Vector3Int> val = ParseValues(args[1]);
			Vector3 val2 = HammerHelper.TryParseSizesZYX(obj, args.Args, 2);
			Vector3 delta = Parse.Direction(args.Args, 3) * val2;
			Execute(obj, delta, val.Min, val.Max);
		}
	}

	private static void Execute(GameObject obj, Vector3 delta, Vector3Int min, Vector3Int max)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		UndoHelper.BeginAction();
		Player player = Helper.GetPlayer();
		Vector3 position = obj.transform.position;
		Quaternion rotation = obj.transform.rotation;
		bool flag = (Object)(object)obj != (Object)(object)player.m_placementGhost;
		Piece selectedPiece = player.GetSelectedPiece();
		for (int i = ((Vector3Int)(ref min)).x; i <= ((Vector3Int)(ref max)).x; i++)
		{
			for (int j = ((Vector3Int)(ref min)).y; j <= ((Vector3Int)(ref max)).y; j++)
			{
				for (int k = ((Vector3Int)(ref min)).z; k <= ((Vector3Int)(ref max)).z; k++)
				{
					if (!flag || i != 0 || j != 0 || k != 0)
					{
						Vector3 val = position;
						val += rotation * Vector3.right * (float)i * delta.x;
						val += rotation * Vector3.up * (float)j * delta.y;
						val += rotation * Vector3.forward * (float)k * delta.z;
						player.PlacePiece(selectedPiece, val, rotation, true, false);
					}
				}
			}
		}
		UndoHelper.EndAction();
		if (flag)
		{
			Hammer.SelectRepair();
		}
	}

	private static List<string> AC(int index)
	{
		return index switch
		{
			0 => ParameterInfo.Create("Amount of objects to be placed (number or min-max)."), 
			1 => ParameterInfo.Create("Step size (<color=yellow>number</color> or <color=yellow>number*auto</color> for automatic step size)."), 
			_ => ParameterInfo.None, 
		};
	}

	private static string Description(string direction)
	{
		return "[amount] [step=auto] - Places multiple objects towards the " + direction + " direction.";
	}

	private static void Command(string name, string description, Func<Range<int>, Range<Vector3Int>> action)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		Func<Range<int>, Range<Vector3Int>> action2 = action;
		AutoComplete.Register(name, (Func<int, List<string>>)AC);
		Helper.Command(name, Description(description), (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			Helper.ArgsCheck(args, 2, "Missing the amount.");
			Hammer.Equip();
			GameObject obj = SelectTarget();
			Range<int> arg = ParseValue(args[1]);
			Vector3 val = HammerHelper.TryParseSize(obj, args.Args, 2);
			Vector3 delta = Parse.Direction(args.Args, 3) * val;
			Range<Vector3Int> val2 = action2(arg);
			Execute(obj, delta, val2.Min, val2.Max);
		});
	}

	public HammerStackCommand()
	{
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		Command("hammer_stack_left", "left", (Range<int> amount) => new Range<Vector3Int>(new Vector3Int(-amount.Min, 0, 0), new Vector3Int(-amount.Max, 0, 0)));
		Command("hammer_stack_right", "right", (Range<int> amount) => new Range<Vector3Int>(new Vector3Int(0, amount.Min, 0), new Vector3Int(0, amount.Max, 0)));
		Command("hammer_stack_down", "down", (Range<int> amount) => new Range<Vector3Int>(new Vector3Int(0, -amount.Min, 0), new Vector3Int(0, -amount.Max, 0)));
		Command("hammer_stack_up", "up", (Range<int> amount) => new Range<Vector3Int>(new Vector3Int(0, amount.Min, 0), new Vector3Int(0, amount.Max, 0)));
		Command("hammer_stack_backward", "backward", (Range<int> amount) => new Range<Vector3Int>(new Vector3Int(0, 0, -amount.Min), new Vector3Int(0, 0, -amount.Max)));
		Command("hammer_stack_forward", "forward", (Range<int> amount) => new Range<Vector3Int>(new Vector3Int(0, 0, amount.Min), new Vector3Int(0, 0, amount.Max)));
		AutoComplete.Register("hammer_stack", (Func<int, int, List<string>>)((int index, int subIndex) => index switch
		{
			0 => ParameterInfo.FRU("Amounts", subIndex), 
			1 => ParameterInfo.FRU("Step size (<color=yellow>number</color> or <color=yellow>number*auto</color> for automatic step size)", subIndex), 
			_ => ParameterInfo.None, 
		}));
		object obj = _003C_003Ec._003C_003E9__4_7;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				Helper.ArgsCheck(args, 2, "Missing the amount.");
				Hammer.Equip();
				GameObject obj2 = SelectTarget();
				Range<Vector3Int> val2 = ParseValues(args[1]);
				Vector3 val3 = HammerHelper.TryParseSizesZYX(obj2, args.Args, 2);
				Vector3 delta = Parse.Direction(args.Args, 3) * val3;
				Execute(obj2, delta, val2.Min, val2.Max);
			};
			_003C_003Ec._003C_003E9__4_7 = val;
			obj = (object)val;
		}
		Helper.Command("hammer_stack", "[forward,up,right] [step=auto,auto,auto] - Places multiple objects next to each other.", (ConsoleEvent)obj);
	}

	private static Range<Vector3Int> ParseValues(string values)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		string[] array = Parse.Split(values, ',');
		Range<int> val = ParseValue(array[0]);
		Range<int> val2 = (Range<int>)((array.Length > 1) ? ((object)ParseValue(array[1])) : ((object)new Range<int>(0)));
		Range<int> val3 = (Range<int>)((array.Length > 2) ? ((object)ParseValue(array[2])) : ((object)new Range<int>(0)));
		Vector3Int val4 = new Vector3Int(val2.Min, val3.Min, val.Min);
		Vector3Int val5 = default(Vector3Int);
		((Vector3Int)(ref val5))._002Ector(val2.Max, val3.Max, val.Max);
		return new Range<Vector3Int>(val4, val5);
	}

	private static Range<int> ParseValue(string value)
	{
		string[] array = value.Split('-');
		if (array.Length == 1)
		{
			return new Range<int>(0, Parse.Int(array[0], 0));
		}
		return Parse.IntRange(value, 0);
	}

	private static GameObject SelectTarget()
	{
		Player player = Helper.GetPlayer();
		Piece val = default(Piece);
		if (Object.op_Implicit((Object)(object)player.m_placementGhost) && player.m_placementGhost.TryGetComponent<Piece>(ref val) && !val.m_repairPiece)
		{
			return player.m_placementGhost;
		}
		Hovered hovered = Selector.GetHovered(player, Configuration.Range, Array.Empty<string>(), Configuration.IgnoredIds, false);
		if (hovered == null)
		{
			return player.m_placementGhost;
		}
		Selection.CreateGhost(new ObjectSelection(hovered.Obj, singleUse: false, null, null));
		return ((Component)hovered.Obj).gameObject;
	}
}
