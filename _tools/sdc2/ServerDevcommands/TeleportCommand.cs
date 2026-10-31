using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Service;
using UnityEngine;

namespace ServerDevcommands;

public class TeleportCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<int, int, List<string>> _003C_003E9__0_0;

		public static ConsoleEvent _003C_003E9__0_1;

		internal List<string> _003C_002Ector_003Eb__0_0(int index, int subIndex)
		{
			switch (index)
			{
			case 0:
				return ParameterInfo.PlayerNames;
			case 1:
				if (subIndex != 0)
				{
					return ParameterInfo.XZYR("Coordinates", subIndex);
				}
				return ParameterInfo.PublicPlayerNames;
			case 2:
				return ParameterInfo.Flag("fast", subIndex);
			default:
				return ParameterInfo.None;
			}
		}

		internal void _003C_002Ector_003Eb__0_1(ConsoleEventArgs args)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			Helper.ArgsCheck(args, 2, "Missing player id/name.");
			List<PlayerInfo> list = PlayerInfo.FindPlayers(Parse.Split(args[1]));
			Vector3 val = (Object.op_Implicit((Object)(object)Player.m_localPlayer) ? ((Component)Player.m_localPlayer).transform.position : Vector3.zero);
			Quaternion val2 = (Object.op_Implicit((Object)(object)Player.m_localPlayer) ? ((Component)Player.m_localPlayer).transform.rotation : Quaternion.identity);
			if (args.Length > 2)
			{
				string[] array = Parse.Split(args[2]);
				if (array.Length > 1)
				{
					val = Parse.VectorXZY(array);
					float height = WorldGenerator.instance.GetHeight(val.x, val.z);
					val.y = Mathf.Max(val.y, height);
					if (array.Length > 3)
					{
						float num = Parse.Float(array[3]);
						val2 = Quaternion.Euler(0f, num, 0f);
					}
				}
				else
				{
					List<PlayerInfo> list2 = PlayerInfo.FindPlayers(array);
					if (list2.Count == 0)
					{
						throw new InvalidOperationException("No target player found with id/name '" + args[2] + "'.");
					}
					val = list2[0].Pos;
					val2 = list2[0].Rot;
				}
			}
			bool flag = args.Length > 3 && args[3].Equals("fast", StringComparison.OrdinalIgnoreCase);
			foreach (PlayerInfo item in list)
			{
				ZRoutedRpc.instance.InvokeRoutedRPC(0L, item.ZDOID, "RPC_TeleportTo", new object[3]
				{
					val,
					val2,
					!flag
				});
			}
		}
	}

	public TeleportCommand()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		AutoComplete.Register("tp", delegate(int index, int subIndex)
		{
			switch (index)
			{
			case 0:
				return ParameterInfo.PlayerNames;
			case 1:
				if (subIndex != 0)
				{
					return ParameterInfo.XZYR("Coordinates", subIndex);
				}
				return ParameterInfo.PublicPlayerNames;
			case 2:
				return ParameterInfo.Flag("fast", subIndex);
			default:
				return ParameterInfo.None;
			}
		});
		object obj = _003C_003Ec._003C_003E9__0_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_0098: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
				//IL_0113: Unknown result type (might be due to invalid IL or missing references)
				//IL_0118: Unknown result type (might be due to invalid IL or missing references)
				//IL_011f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0124: Unknown result type (might be due to invalid IL or missing references)
				//IL_00da: Unknown result type (might be due to invalid IL or missing references)
				//IL_00df: Unknown result type (might be due to invalid IL or missing references)
				//IL_015f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0171: Unknown result type (might be due to invalid IL or missing references)
				//IL_017a: Unknown result type (might be due to invalid IL or missing references)
				Helper.ArgsCheck(args, 2, "Missing player id/name.");
				List<PlayerInfo> list = PlayerInfo.FindPlayers(Parse.Split(args[1]));
				Vector3 val2 = (Object.op_Implicit((Object)(object)Player.m_localPlayer) ? ((Component)Player.m_localPlayer).transform.position : Vector3.zero);
				Quaternion val3 = (Object.op_Implicit((Object)(object)Player.m_localPlayer) ? ((Component)Player.m_localPlayer).transform.rotation : Quaternion.identity);
				if (args.Length > 2)
				{
					string[] array = Parse.Split(args[2]);
					if (array.Length > 1)
					{
						val2 = Parse.VectorXZY(array);
						float height = WorldGenerator.instance.GetHeight(val2.x, val2.z);
						val2.y = Mathf.Max(val2.y, height);
						if (array.Length > 3)
						{
							float num = Parse.Float(array[3]);
							val3 = Quaternion.Euler(0f, num, 0f);
						}
					}
					else
					{
						List<PlayerInfo> list2 = PlayerInfo.FindPlayers(array);
						if (list2.Count == 0)
						{
							throw new InvalidOperationException("No target player found with id/name '" + args[2] + "'.");
						}
						val2 = list2[0].Pos;
						val3 = list2[0].Rot;
					}
				}
				bool flag = args.Length > 3 && args[3].Equals("fast", StringComparison.OrdinalIgnoreCase);
				foreach (PlayerInfo item in list)
				{
					ZRoutedRpc.instance.InvokeRoutedRPC(0L, item.ZDOID, "RPC_TeleportTo", new object[3]
					{
						val2,
						val3,
						!flag
					});
				}
			};
			_003C_003Ec._003C_003E9__0_1 = val;
			obj = (object)val;
		}
		Helper.Command("tp", "[player1,player2,...] [x,z,y,rot/player] [fast=false] - Teleports the player to coordinates or another player.", (ConsoleEvent)obj);
	}
}
