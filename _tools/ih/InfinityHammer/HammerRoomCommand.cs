using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class HammerRoomCommand
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
			return index switch
			{
				0 => ParameterInfo.RoomIds, 
				1 => new List<string>(2) { "true", "false" }, 
				_ => ParameterInfo.None, 
			};
		}

		internal void _003C_002Ector_003Eb__1_1(ConsoleEventArgs args)
		{
			Helper.ArgsCheck(args, 2, "Missing the room id.");
			HammerHelper.Init();
			bool emptyRoom = ((args.Length <= 2) ? Configuration.PlaceEmptyRooms : (Parse.Boolean(args[2]) ?? Configuration.PlaceEmptyRooms));
			try
			{
				GameObject val = Selection.CreateGhost(new RoomSelection(StringExtensionMethods.GetStableHashCode(args[1]), emptyRoom));
				Hammer.SelectEmpty();
				((Object)val).name = args[1];
				PrintSelected(args.Context, val);
			}
			catch (InvalidOperationException ex)
			{
				HammerHelper.Message(args.Context, ex.Message);
			}
		}
	}

	private static void PrintSelected(Terminal terminal, GameObject obj)
	{
		if (!Configuration.DisableSelectMessages)
		{
			string text = (Object.op_Implicit((Object)(object)obj) ? Utils.GetPrefabName(obj.gameObject) : "");
			HammerHelper.Message(terminal, "Selected " + text + ".");
		}
	}

	public HammerRoomCommand()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		AutoComplete.Register("hammer_room", (Func<int, int, List<string>>)((int index, int subIndex) => index switch
		{
			0 => ParameterInfo.RoomIds, 
			1 => new List<string>(2) { "true", "false" }, 
			_ => ParameterInfo.None, 
		}));
		object obj = _003C_003Ec._003C_003E9__1_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				Helper.ArgsCheck(args, 2, "Missing the room id.");
				HammerHelper.Init();
				bool emptyRoom = ((args.Length <= 2) ? Configuration.PlaceEmptyRooms : (Parse.Boolean(args[2]) ?? Configuration.PlaceEmptyRooms));
				try
				{
					GameObject val2 = Selection.CreateGhost(new RoomSelection(StringExtensionMethods.GetStableHashCode(args[1]), emptyRoom));
					Hammer.SelectEmpty();
					((Object)val2).name = args[1];
					PrintSelected(args.Context, val2);
				}
				catch (InvalidOperationException ex)
				{
					HammerHelper.Message(args.Context, ex.Message);
				}
			};
			_003C_003Ec._003C_003E9__1_1 = val;
			obj = (object)val;
		}
		Helper.Command("hammer_room", "[room id] [empty_room] - Selects the room to be placed.", (ConsoleEvent)obj);
	}
}
