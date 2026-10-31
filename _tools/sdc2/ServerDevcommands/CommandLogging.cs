using System;
using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ZNet), "RPC_PeerInfo")]
public class CommandLogging
{
	public static string RPC_Log = "DEV_Log";

	private static string Format(ZNetPeer peer, string command)
	{
		return string.Format(Settings.Format(Settings.CommandLogFormat).Replace("{command", "{6"), peer.m_socket.GetHostName(), peer.m_playerName, ((ZDOID)(ref peer.m_characterID)).UserID.ToString(), peer.m_refPos.x, peer.m_refPos.y, peer.m_refPos.z, command);
	}

	private static void RPC_LogCommand(ZRpc rpc, string command)
	{
		string text = Format(ZNet.instance.GetPeer(rpc), command);
		if (text != "")
		{
			ZLog.Log((object)text);
		}
	}

	private static void Postfix(ZNet __instance, ZRpc rpc)
	{
		if (__instance.IsServer())
		{
			rpc.Register<string>(RPC_Log, (Action<ZRpc, string>)RPC_LogCommand);
		}
	}
}
