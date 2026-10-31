using System;
using System.Collections.Generic;
using System.Linq;
using Splatform;
using UnityEngine;

namespace Service;

public class PlayerInfo
{
	public string Name;

	public Vector3 Pos;

	public Quaternion Rot;

	public string HostId;

	public long PeerId;

	public ZDOID ZDOID;

	public PlayerInfo(ZNetPeer peer)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		HostId = peer.m_rpc.GetSocket().GetHostName();
		Name = peer.m_playerName;
		Pos = peer.m_refPos;
		ZDOID = peer.m_characterID;
		PeerId = ((ZDOID)(ref ZDOID)).UserID;
		ZDO zDO = ZDOMan.instance.GetZDO(peer.m_characterID);
		if (zDO != null)
		{
			Pos = zDO.m_position;
			Rot = zDO.GetRotation();
		}
	}

	public PlayerInfo(PlayerInfo info)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		HostId = ((object)(PlatformUserID)(ref info.m_userInfo.m_id)).ToString();
		Name = info.m_name;
		Pos = info.m_position;
		ZDOID = info.m_characterID;
		PeerId = ((ZDOID)(ref ZDOID)).UserID;
		ZDO zDO = ZDOMan.instance.GetZDO(info.m_characterID);
		if (zDO != null)
		{
			Pos = zDO.m_position;
			Rot = zDO.GetRotation();
		}
	}

	public PlayerInfo(Player player)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		HostId = "self";
		Name = player.GetPlayerName();
		ZDOID = ((Character)player).GetZDOID();
		PeerId = ((ZDOID)(ref ZDOID)).UserID;
		Pos = ((Component)player).transform.position;
		Rot = ((Component)player).transform.rotation;
	}

	public static List<PlayerInfo> FindPlayers(string[] args)
	{
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		List<PlayerInfo> list = (ZNet.instance.IsServer() ? (from peer in ZNet.instance.GetPeers()
			select new PlayerInfo(peer)).ToList() : ZNet.instance.m_players.Select((PlayerInfo player) => new PlayerInfo(player)).ToList());
		if (Object.op_Implicit((Object)(object)Player.m_localPlayer) && list.All((PlayerInfo p) => p.ZDOID != ((Character)Player.m_localPlayer).GetZDOID()))
		{
			list.Add(new PlayerInfo(Player.m_localPlayer));
		}
		Dictionary<ZDOID, PlayerInfo> dictionary = new Dictionary<ZDOID, PlayerInfo>();
		foreach (string text in args)
		{
			switch (text)
			{
			case "*":
			case "all":
				return list;
			case "others":
			{
				List<PlayerInfo> list2 = new List<PlayerInfo>();
				list2.AddRange(list.Where(delegate(PlayerInfo p)
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					//IL_001b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0031: Unknown result type (might be due to invalid IL or missing references)
					//IL_0034: Unknown result type (might be due to invalid IL or missing references)
					ZDOID zDOID = p.ZDOID;
					Player localPlayer = Player.m_localPlayer;
					ZDOID? val = ((localPlayer != null) ? new ZDOID?(((Character)localPlayer).GetZDOID()) : null);
					return !val.HasValue || zDOID != val.GetValueOrDefault();
				}));
				return list2;
			}
			}
			string text2 = text.ToLowerInvariant();
			foreach (PlayerInfo item in list)
			{
				string text3 = item.Name.ToLowerInvariant();
				if (item.HostId == text)
				{
					dictionary[item.ZDOID] = item;
				}
				else if (text3 == text2)
				{
					dictionary[item.ZDOID] = item;
				}
				else if (text2[0] == '*' && text2[text2.Length - 1] == '*' && text3.Contains(text2.Substring(1, text2.Length - 2)))
				{
					dictionary[item.ZDOID] = item;
				}
				else if (text2[0] == '*' && item.Name.EndsWith(text2.Substring(1), StringComparison.OrdinalIgnoreCase))
				{
					dictionary[item.ZDOID] = item;
				}
				else if (text2[text2.Length - 1] == '*' && item.Name.StartsWith(text2.Substring(0, text2.Length - 1), StringComparison.OrdinalIgnoreCase))
				{
					dictionary[item.ZDOID] = item;
				}
			}
		}
		Dictionary<ZDOID, PlayerInfo>.ValueCollection values = dictionary.Values;
		List<PlayerInfo> list3 = new List<PlayerInfo>(values.Count);
		list3.AddRange(values);
		if (list3.Count == 0)
		{
			throw new InvalidOperationException("No target player found with id/name '" + string.Join(",", args) + "'.");
		}
		return list3;
	}
}
