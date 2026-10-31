using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Splatform;

namespace ServerDevcommands;

[HarmonyPatch]
public class ServerExecution
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static Method _003C0_003E__RPC_DoRequestIds;

		public static Action<ZRpc, string> _003C1_003E__RPC_Do_Pins;

		public static Action<ZRpc, string> _003C2_003E__ReceiveLocationIds;

		public static Action<ZRpc, string> _003C3_003E__ReceiveVegetationIds;

		public static Action<ZRpc, ZPackage> _003C4_003E__ReceivePermissions;
	}

	public static string RPC_Pins = "DEV_Pins";

	public static string RPC_RequestIds = "EW_RequestIds";

	public static string RPC_SyncLocationIds = "EW_SyncLocationIds";

	public static string RPC_SyncVegetationIds = "EW_SyncVegetationIds";

	[HarmonyPatch(typeof(ZNet), "RPC_RemoteCommand")]
	[HarmonyPrefix]
	private static bool ReplceAdminCheckWithCustomCheck(ZNet __instance, ZRpc rpc, string command)
	{
		if (!__instance.IsServer())
		{
			return true;
		}
		RedirectOutput.Target = rpc;
		if (!IsRpcCommandAllowed(__instance, rpc, command))
		{
			__instance.RemotePrint(rpc, "Unauthorized to use command '" + GetCommandName(command) + "'.");
			return false;
		}
		__instance.InternalCommand(rpc, command);
		return false;
	}

	[HarmonyPatch(typeof(ZNet), "RPC_RemoteCommand")]
	[HarmonyFinalizer]
	private static void ClearTarget()
	{
		RedirectOutput.Target = null;
	}

	private static bool IsRpcCommandAllowed(ZNet znet, ZRpc rpc, string command)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		if (rpc == null)
		{
			return true;
		}
		string commandName = GetCommandName(command);
		if (!Terminal.commands.TryGetValue(commandName, out var value))
		{
			return false;
		}
		ZNetPeer peer = znet.GetPeer(rpc);
		if (peer == null)
		{
			return false;
		}
		string hostName = rpc.GetSocket().GetHostName();
		ZDO zDO = ZDOMan.instance.GetZDO(peer.m_characterID);
		if (zDO == null)
		{
			return false;
		}
		string characterId = zDO.GetLong(ZDOVars.s_playerID, 0L).ToString();
		return PermissionLoader.Data.Resolve(hostName, characterId).IsCommandAllowed(value, command, remote: true);
	}

	private static string GetCommandName(string command)
	{
		return Parse.Kvp(command, ' ').Key;
	}

	public static void Send(string command)
	{
		((Terminal)Console.instance).AddString("Sending command: " + command);
		ZNet.instance.RemoteCommand(command);
	}

	public static void Send(IEnumerable<string> args)
	{
		Send(string.Join(" ", args));
	}

	public static void Send(ConsoleEventArgs args)
	{
		Send((IEnumerable<string>)args.Args);
	}

	public static void RPC_Do_Pins(ZRpc? rpc, string data)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		string[][] array = (from s in Parse.Split(data, '|')
			select Parse.Split(s)).ToArray();
		List<PinData> findPins = ((Terminal)Console.instance).m_findPins;
		foreach (PinData item in findPins)
		{
			Minimap instance = Minimap.instance;
			if (instance != null)
			{
				instance.RemovePin(item);
			}
		}
		findPins.Clear();
		string[][] array2 = array;
		foreach (string[] array3 in array2)
		{
			PinType result;
			PinType val = (PinType)((array3.Length <= 3 || !Enum.TryParse<PinType>(array3[3], ignoreCase: true, out result)) ? 3 : ((int)result));
			Minimap instance2 = Minimap.instance;
			PinData val2 = ((instance2 != null) ? instance2.AddPin(Parse.VectorXZY(array3), val, "", false, false, Player.m_localPlayer.GetPlayerID(), default(PlatformUserID)) : null);
			if (val2 != null)
			{
				findPins.Add(val2);
			}
		}
	}

	public static void RequestIds()
	{
		ZRpc serverRPC = ZNet.instance.GetServerRPC();
		if (serverRPC != null)
		{
			serverRPC.Invoke(RPC_RequestIds, Array.Empty<object>());
		}
	}

	public static void RPC_DoRequestIds(ZRpc rpc)
	{
		string text = string.Join("|", ParameterInfo.LocationIds);
		string text2 = string.Join("|", ParameterInfo.VegetationIds);
		rpc.Invoke(RPC_SyncLocationIds, new object[1] { text });
		rpc.Invoke(RPC_SyncVegetationIds, new object[1] { text2 });
	}

	public static void ReceiveLocationIds(ZRpc rpc, string locationIds)
	{
		List<string> list = new List<string>();
		list.AddRange(from s in locationIds.Split('|')
			where !string.IsNullOrEmpty(s)
			select s);
		ParameterInfo.SetServerLocationIds(list);
	}

	public static void ReceiveVegetationIds(ZRpc rpc, string vegetationIds)
	{
		List<string> list = new List<string>();
		list.AddRange(from s in vegetationIds.Split('|')
			where !string.IsNullOrEmpty(s)
			select s);
		ParameterInfo.SetServerVegetationIds(list);
	}

	[HarmonyPatch(typeof(ZNet), "RPC_PeerInfo")]
	[HarmonyPostfix]
	private static void RegisterRPCs(ZNet __instance, ZRpc rpc)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		if (__instance.IsServer())
		{
			string rPC_RequestIds = RPC_RequestIds;
			object obj = _003C_003EO._003C0_003E__RPC_DoRequestIds;
			if (obj == null)
			{
				Method val = RPC_DoRequestIds;
				_003C_003EO._003C0_003E__RPC_DoRequestIds = val;
				obj = (object)val;
			}
			rpc.Register(rPC_RequestIds, (Method)obj);
		}
		else if (__instance.GetPeer(rpc).m_server)
		{
			rpc.Register<string>(RPC_Pins, (Action<ZRpc, string>)RPC_Do_Pins);
			rpc.Register<string>(RPC_SyncLocationIds, (Action<ZRpc, string>)ReceiveLocationIds);
			rpc.Register<string>(RPC_SyncVegetationIds, (Action<ZRpc, string>)ReceiveVegetationIds);
			rpc.Register<ZPackage>(PermissionLoader.RPC_Permissions, (Action<ZRpc, ZPackage>)Admin.ReceivePermissions);
		}
	}

	[HarmonyPatch(typeof(ZoneSystem), "Start")]
	[HarmonyPostfix]
	private static void InitServer()
	{
		if (ZNet.instance.IsServer())
		{
			PermissionLoader.FromFile();
		}
	}
}
