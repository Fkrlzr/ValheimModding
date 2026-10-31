using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ZNet), "GetOtherPublicPlayers")]
public class IncludePrivatePlayersInTheMap
{
	private static void Postfix(ZNet __instance, List<PlayerInfo> playerList)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (!Settings.ShowPrivatePlayers)
		{
			return;
		}
		foreach (PlayerInfo player in __instance.m_players)
		{
			if (!player.m_publicPosition)
			{
				ZDOID characterID = player.m_characterID;
				if (!((ZDOID)(ref characterID)).IsNone() && !(player.m_characterID == __instance.m_characterID) && !(player.m_position == Vector3.zero))
				{
					playerList.Add(player);
				}
			}
		}
	}
}
