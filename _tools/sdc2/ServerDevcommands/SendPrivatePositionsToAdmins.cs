using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ZNet), "SendPlayerList")]
public class SendPrivatePositionsToAdmins
{
	private static void Postfix(ZNet __instance)
	{
		if (__instance.IsServer() && Settings.ShowPrivatePlayers && __instance.m_peers.Count != 0)
		{
			SendToAdmins(__instance);
		}
	}

	private static void SendToAdmins(ZNet obj)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		ZNet obj2 = obj;
		int num = obj2.m_players.Count((PlayerInfo p) => !p.m_publicPosition);
		if (num == 0)
		{
			return;
		}
		List<ZNetPeer> list = obj2.m_peers.Where((ZNetPeer peer) => peer.IsReady() && obj2.IsAdmin(peer.m_rpc.GetSocket().GetHostName())).ToList();
		if (list.Count == 0)
		{
			return;
		}
		ZPackage val = new ZPackage();
		val.Write(num);
		foreach (PlayerInfo player in obj2.m_players)
		{
			if (!player.m_publicPosition)
			{
				val.Write(player.m_characterID);
				val.Write(player.m_position);
			}
		}
		foreach (ZNetPeer item in list)
		{
			item.m_rpc.Invoke("DEV_PrivatePlayerList2", new object[1] { val });
		}
	}
}
