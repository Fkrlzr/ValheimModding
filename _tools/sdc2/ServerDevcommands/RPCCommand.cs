using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Service;

namespace ServerDevcommands;

public class RPCCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<int, List<string>> _003C_003E9__0_0;

		public static Func<string, ZDO> _003C_003E9__0_2;

		public static ConsoleEvent _003C_003E9__0_1;

		internal List<string> _003C_002Ector_003Eb__0_0(int index)
		{
			return index switch
			{
				0 => ParameterInfo.PlayerNames, 
				1 => ParameterInfo.Create("RPC name."), 
				_ => ParameterInfo.Create("Parameters."), 
			};
		}

		internal void _003C_002Ector_003Eb__0_1(ConsoleEventArgs args)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			Helper.ArgsCheck(args, 2, "Missing id.");
			Helper.ArgsCheck(args, 3, "Missing RPC name");
			string[] array = Parse.Split(args[1]);
			foreach (PlayerInfo item in PlayerInfo.FindPlayers(array))
			{
				ZRoutedRpc instance = ZRoutedRpc.instance;
				long uID = ZNet.GetUID();
				ZDOID zDOID = item.ZDOID;
				string text = args[2];
				object[] array2 = args.Args.Skip(3).ToArray();
				instance.InvokeRoutedRPC(uID, zDOID, text, array2);
			}
			ZDO[] array3 = ((IEnumerable<string>)array).Select((Func<string, ZDO>)delegate(string s)
			{
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				string[] array4 = s.Split(':');
				if (array4.Length == 1)
				{
					return (ZDO)null;
				}
				if (!long.TryParse(array4[0], out var result))
				{
					return (ZDO)null;
				}
				if (!uint.TryParse(array4[1], out var result2))
				{
					return (ZDO)null;
				}
				ZDOID key = default(ZDOID);
				((ZDOID)(ref key))._002Ector(result, result2);
				ZDO value;
				return (!ZDOMan.instance.m_objectsByID.TryGetValue(key, out value)) ? null : value;
			}).ToArray();
			foreach (ZDO val in array3)
			{
				if (val != null)
				{
					ZRoutedRpc instance2 = ZRoutedRpc.instance;
					long uID2 = ZNet.GetUID();
					ZDOID uid = val.m_uid;
					string text2 = args[2];
					object[] array2 = args.Args.Skip(3).ToArray();
					instance2.InvokeRoutedRPC(uID2, uid, text2, array2);
				}
			}
		}

		internal ZDO _003C_002Ector_003Eb__0_2(string s)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			string[] array = s.Split(':');
			if (array.Length == 1)
			{
				return null;
			}
			if (!long.TryParse(array[0], out var result))
			{
				return null;
			}
			if (!uint.TryParse(array[1], out var result2))
			{
				return null;
			}
			ZDOID key = default(ZDOID);
			((ZDOID)(ref key))._002Ector(result, result2);
			if (!ZDOMan.instance.m_objectsByID.TryGetValue(key, out var value))
			{
				return null;
			}
			return value;
		}
	}

	public RPCCommand()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		AutoComplete.Register("rpc", (int index) => index switch
		{
			0 => ParameterInfo.PlayerNames, 
			1 => ParameterInfo.Create("RPC name."), 
			_ => ParameterInfo.Create("Parameters."), 
		});
		object obj = _003C_003Ec._003C_003E9__0_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
				Helper.ArgsCheck(args, 2, "Missing id.");
				Helper.ArgsCheck(args, 3, "Missing RPC name");
				string[] array = Parse.Split(args[1]);
				foreach (PlayerInfo item in PlayerInfo.FindPlayers(array))
				{
					ZRoutedRpc instance = ZRoutedRpc.instance;
					long uID = ZNet.GetUID();
					ZDOID zDOID = item.ZDOID;
					string text = args[2];
					object[] array2 = args.Args.Skip(3).ToArray();
					instance.InvokeRoutedRPC(uID, zDOID, text, array2);
				}
				ZDO[] array3 = ((IEnumerable<string>)array).Select((Func<string, ZDO>)delegate(string s)
				{
					//IL_0049: Unknown result type (might be due to invalid IL or missing references)
					string[] array4 = s.Split(':');
					if (array4.Length == 1)
					{
						return (ZDO)null;
					}
					if (!long.TryParse(array4[0], out var result))
					{
						return (ZDO)null;
					}
					if (!uint.TryParse(array4[1], out var result2))
					{
						return (ZDO)null;
					}
					ZDOID key = default(ZDOID);
					((ZDOID)(ref key))._002Ector(result, result2);
					ZDO value;
					return (!ZDOMan.instance.m_objectsByID.TryGetValue(key, out value)) ? null : value;
				}).ToArray();
				foreach (ZDO val2 in array3)
				{
					if (val2 != null)
					{
						ZRoutedRpc instance2 = ZRoutedRpc.instance;
						long uID2 = ZNet.GetUID();
						ZDOID uid = val2.m_uid;
						string text2 = args[2];
						object[] array2 = args.Args.Skip(3).ToArray();
						instance2.InvokeRoutedRPC(uID2, uid, text2, array2);
					}
				}
			};
			_003C_003Ec._003C_003E9__0_1 = val;
			obj = (object)val;
		}
		Helper.Command("rpc", "[id1,id2,...] [name] [arg1] [arg2] ... - Sends rpc.", (ConsoleEvent)obj);
	}
}
