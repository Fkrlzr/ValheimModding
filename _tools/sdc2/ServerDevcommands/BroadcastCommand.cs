using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Service;

namespace ServerDevcommands;

public class BroadcastCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__2_0;

		public static Func<int, List<string>> _003C_003E9__2_1;

		public static Func<int, List<string>> _003C_003E9__2_4;

		public static Func<int, List<string>> _003C_003E9__2_5;

		public static Func<int, List<string>> _003C_003E9__2_6;

		public static Func<int, List<string>> _003C_003E9__2_7;

		public static ConsoleEvent _003C_003E9__2_2;

		public static Func<int, List<string>> _003C_003E9__2_3;

		public static Func<int, List<string>> _003C_003E9__2_8;

		public static Func<int, List<string>> _003C_003E9__2_9;

		public static Func<int, List<string>> _003C_003E9__2_10;

		public static Func<int, List<string>> _003C_003E9__2_11;

		internal void _003C_002Ector_003Eb__2_0(ConsoleEventArgs args)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			Helper.ArgsCheck(args, 2, "Missing the center/side parameter.");
			Helper.ArgsCheck(args, 3, "Missing the message");
			MessageType val = (MessageType)((args[2] == "side") ? 1 : 2);
			string text = string.Join(" ", args.Args.Skip(2));
			MessageHud.instance.MessageAll(val, text);
		}

		internal List<string> _003C_002Ector_003Eb__2_1(int index)
		{
			if (index == 0)
			{
				return Types;
			}
			return Modifiers;
		}

		internal List<string> _003C_002Ector_003Eb__2_4(int index)
		{
			return ParameterInfo.Create("Bolds the text.");
		}

		internal List<string> _003C_002Ector_003Eb__2_5(int index)
		{
			return ParameterInfo.Create("Italics the text.");
		}

		internal List<string> _003C_002Ector_003Eb__2_6(int index)
		{
			return ParameterInfo.Colors;
		}

		internal List<string> _003C_002Ector_003Eb__2_7(int index)
		{
			return ParameterInfo.Create("Size in pixels.");
		}

		internal void _003C_002Ector_003Eb__2_2(ConsoleEventArgs args)
		{
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Expected I4, but got Unknown
			Helper.ArgsCheck(args, 2, "Missing the player parameter.");
			Helper.ArgsCheck(args, 3, "Missing the center/side parameter.");
			Helper.ArgsCheck(args, 4, "Missing the message");
			List<PlayerInfo> list = PlayerInfo.FindPlayers(Parse.Split(args[1]));
			if (list.Count == 0)
			{
				throw new Exception("Player " + args[1] + " not found.");
			}
			MessageType val = (MessageType)((args[3] == "side") ? 1 : 2);
			string text = string.Join(" ", args.Args.Skip(3));
			foreach (PlayerInfo item in list)
			{
				ZRoutedRpc.instance.InvokeRoutedRPC(item.PeerId, "ShowMessage", new object[2]
				{
					(int)val,
					text
				});
			}
		}

		internal List<string> _003C_002Ector_003Eb__2_3(int index)
		{
			return index switch
			{
				0 => ParameterInfo.PlayerNames, 
				1 => Types, 
				_ => Modifiers, 
			};
		}

		internal List<string> _003C_002Ector_003Eb__2_8(int index)
		{
			return ParameterInfo.Create("Bolds the text.");
		}

		internal List<string> _003C_002Ector_003Eb__2_9(int index)
		{
			return ParameterInfo.Create("Italics the text.");
		}

		internal List<string> _003C_002Ector_003Eb__2_10(int index)
		{
			return ParameterInfo.Colors;
		}

		internal List<string> _003C_002Ector_003Eb__2_11(int index)
		{
			return ParameterInfo.Create("Size in pixels.");
		}
	}

	private static readonly List<string> Types = new List<string>(2) { "center", "side" };

	private static readonly List<string> Modifiers = new List<string>(4) { "<b", "<color", "<i", "<size" };

	public BroadcastCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__2_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				Helper.ArgsCheck(args, 2, "Missing the center/side parameter.");
				Helper.ArgsCheck(args, 3, "Missing the message");
				MessageType val4 = (MessageType)((args[2] == "side") ? 1 : 2);
				string text2 = string.Join(" ", args.Args.Skip(2));
				MessageHud.instance.MessageAll(val4, text2);
			};
			_003C_003Ec._003C_003E9__2_0 = val;
			obj = (object)val;
		}
		Helper.Command("broadcast", "[center/side] [message] - Broadcasts a message.", (ConsoleEvent)obj);
		AutoComplete.Register("broadcast", (int index) => (index == 0) ? Types : Modifiers, new Dictionary<string, Func<int, List<string>>>
		{
			{
				"<b",
				(int index) => ParameterInfo.Create("Bolds the text.")
			},
			{
				"<i",
				(int index) => ParameterInfo.Create("Italics the text.")
			},
			{
				"<color",
				(int index) => ParameterInfo.Colors
			},
			{
				"<size",
				(int index) => ParameterInfo.Create("Size in pixels.")
			}
		});
		object obj2 = _003C_003Ec._003C_003E9__2_2;
		if (obj2 == null)
		{
			ConsoleEvent val2 = delegate(ConsoleEventArgs args)
			{
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b8: Expected I4, but got Unknown
				Helper.ArgsCheck(args, 2, "Missing the player parameter.");
				Helper.ArgsCheck(args, 3, "Missing the center/side parameter.");
				Helper.ArgsCheck(args, 4, "Missing the message");
				List<PlayerInfo> list = PlayerInfo.FindPlayers(Parse.Split(args[1]));
				if (list.Count == 0)
				{
					throw new Exception("Player " + args[1] + " not found.");
				}
				MessageType val3 = (MessageType)((args[3] == "side") ? 1 : 2);
				string text = string.Join(" ", args.Args.Skip(3));
				foreach (PlayerInfo item in list)
				{
					ZRoutedRpc.instance.InvokeRoutedRPC(item.PeerId, "ShowMessage", new object[2]
					{
						(int)val3,
						text
					});
				}
			};
			_003C_003Ec._003C_003E9__2_2 = val2;
			obj2 = (object)val2;
		}
		Helper.Command("message", "[player] [center/side] [message] - Sends a message to a player.", (ConsoleEvent)obj2);
		AutoComplete.Register("message", (int index) => index switch
		{
			0 => ParameterInfo.PlayerNames, 
			1 => Types, 
			_ => Modifiers, 
		}, new Dictionary<string, Func<int, List<string>>>
		{
			{
				"<b",
				(int index) => ParameterInfo.Create("Bolds the text.")
			},
			{
				"<i",
				(int index) => ParameterInfo.Create("Italics the text.")
			},
			{
				"<color",
				(int index) => ParameterInfo.Colors
			},
			{
				"<size",
				(int index) => ParameterInfo.Create("Size in pixels.")
			}
		});
	}
}
