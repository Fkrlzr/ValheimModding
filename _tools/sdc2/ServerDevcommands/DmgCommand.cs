using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Service;

namespace ServerDevcommands;

public class DmgCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<PlayerInfo, string> _003C_003E9__0_2;

		public static ConsoleEvent _003C_003E9__0_0;

		public static Func<int, List<string>> _003C_003E9__0_1;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			Helper.ArgsCheck(args, 2, "Missing target.");
			Helper.ArgsCheck(args, 3, "Missing amount.");
			string text = args.Args[1];
			float num = Parse.Float(args.Args[2]);
			float num2 = Math.Abs(num);
			string arg = ((num >= 0f) ? " damage" : " healing");
			List<PlayerInfo> list = PlayerInfo.FindPlayers(new string[1] { text });
			foreach (PlayerInfo item in list)
			{
				if (num >= 0f)
				{
					HitData val = new HitData
					{
						m_damage = 
						{
							m_damage = num
						},
						m_hitType = (HitType)14
					};
					ZRoutedRpc.instance.InvokeRoutedRPC(0L, item.ZDOID, "RPC_Damage", new object[1] { val });
				}
				else
				{
					ZRoutedRpc.instance.InvokeRoutedRPC(0L, item.ZDOID, "RPC_Heal", new object[2] { num2, true });
				}
			}
			string text2 = string.Format("{0}{1} applied to: {2}", num2, arg, string.Join(", ", list.Select((PlayerInfo p) => p.Name)));
			args.Context.AddString(text2);
		}

		internal string _003C_002Ector_003Eb__0_2(PlayerInfo p)
		{
			return p.Name;
		}

		internal List<string> _003C_002Ector_003Eb__0_1(int index)
		{
			switch (index)
			{
			case 0:
			{
				string item = "others";
				string item2 = "all";
				List<string> playerNames = ParameterInfo.PlayerNames;
				List<string> list = new List<string>(2 + playerNames.Count);
				list.Add(item);
				list.Add(item2);
				list.AddRange(playerNames);
				return list;
			}
			case 1:
				return ParameterInfo.Create("Value", "Positive = damage / Negative = healing");
			default:
				return ParameterInfo.None;
			}
		}
	}

	public DmgCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_0091: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_009b: Expected O, but got Unknown
				//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
				Helper.ArgsCheck(args, 2, "Missing target.");
				Helper.ArgsCheck(args, 3, "Missing amount.");
				string text = args.Args[1];
				float num = Parse.Float(args.Args[2]);
				float num2 = Math.Abs(num);
				string arg = ((num >= 0f) ? " damage" : " healing");
				List<PlayerInfo> list2 = PlayerInfo.FindPlayers(new string[1] { text });
				foreach (PlayerInfo item3 in list2)
				{
					if (num >= 0f)
					{
						HitData val2 = new HitData
						{
							m_damage = 
							{
								m_damage = num
							},
							m_hitType = (HitType)14
						};
						ZRoutedRpc.instance.InvokeRoutedRPC(0L, item3.ZDOID, "RPC_Damage", new object[1] { val2 });
					}
					else
					{
						ZRoutedRpc.instance.InvokeRoutedRPC(0L, item3.ZDOID, "RPC_Heal", new object[2] { num2, true });
					}
				}
				string text2 = string.Format("{0}{1} applied to: {2}", num2, arg, string.Join(", ", list2.Select((PlayerInfo p) => p.Name)));
				args.Context.AddString(text2);
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		Helper.Command("dmg", "[target] [amount] - (Negative values heal the character).", (ConsoleEvent)obj);
		AutoComplete.Register("dmg", delegate(int index)
		{
			switch (index)
			{
			case 0:
			{
				string item = "others";
				string item2 = "all";
				List<string> playerNames = ParameterInfo.PlayerNames;
				List<string> list = new List<string>(2 + playerNames.Count);
				list.Add(item);
				list.Add(item2);
				list.AddRange(playerNames);
				return list;
			}
			case 1:
				return ParameterInfo.Create("Value", "Positive = damage / Negative = healing");
			default:
				return ParameterInfo.None;
			}
		});
	}
}
