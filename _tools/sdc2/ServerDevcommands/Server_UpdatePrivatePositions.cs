using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ZNet), "UpdatePlayerList")]
public class Server_UpdatePrivatePositions
{
	private static void Postfix(ZNet __instance)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		if (!__instance.IsServer() || !Settings.ShowPrivatePlayers)
		{
			return;
		}
		Dictionary<ZDOID, ZNetPeer> dictionary = __instance.m_peers.Where((ZNetPeer peer) => peer.IsReady() && peer.m_characterID != ZDOID.None).ToDictionary((ZNetPeer peer) => peer.m_characterID, (ZNetPeer peer) => peer);
		for (int i = 0; i < __instance.m_players.Count; i++)
		{
			PlayerInfo val = __instance.m_players[i];
			if (!(val.m_characterID == __instance.m_characterID) && dictionary.TryGetValue(val.m_characterID, out var value) && !value.m_publicRefPos)
			{
				List<PlayerInfo> players = __instance.m_players;
				int index = i;
				PlayerInfo value2 = val;
				value2.m_position = value.m_refPos;
				players[index] = value2;
			}
		}
	}
}
