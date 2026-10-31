using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ZNet), "RPC_PeerInfo")]
public class RegisterRpcPrivatePositions
{
	public static readonly Dictionary<ZDOID, Vector3> PrivatePositions = new Dictionary<ZDOID, Vector3>();

	private static void RPC_PrivatePlayerList(ZRpc rpc, ZPackage pkg)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		if (!Settings.ShowPrivatePlayers)
		{
			return;
		}
		int num = pkg.ReadInt();
		for (int i = 0; i < num; i++)
		{
			ZDOID key = pkg.ReadZDOID();
			Vector3 value = pkg.ReadVector3();
			PrivatePositions[key] = value;
		}
		for (int j = 0; j < ZNet.instance.m_players.Count; j++)
		{
			if (!ZNet.instance.m_players[j].m_publicPosition && PrivatePositions.TryGetValue(ZNet.instance.m_players[j].m_characterID, out var value2))
			{
				List<PlayerInfo> players = ZNet.instance.m_players;
				int index = j;
				PlayerInfo value3 = ZNet.instance.m_players[j];
				value3.m_position = value2;
				players[index] = value3;
			}
		}
	}

	private static void Postfix(ZNet __instance, ZRpc rpc)
	{
		if (!__instance.IsServer())
		{
			rpc.Register<ZPackage>("DEV_PrivatePlayerList2", (Action<ZRpc, ZPackage>)RPC_PrivatePlayerList);
		}
	}
}
