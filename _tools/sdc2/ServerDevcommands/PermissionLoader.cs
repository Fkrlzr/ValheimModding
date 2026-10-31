using System.Collections.Generic;
using System.IO;
using BepInEx;
using Service;
using UnityEngine;

namespace ServerDevcommands;

public class PermissionLoader
{
	public static string RPC_Permissions = "DEV_Permissions";

	public static PermissionData Data = new PermissionData();

	public static string FileName = "permissions.yaml";

	public static string FilePath = Path.Combine(Paths.ConfigPath, FileName);

	private static bool SkipReload = false;

	public static PermissionEntry? GetOrCreatePeerEntry(string hostname, string characterId, string playerName)
	{
		if (Helper.IsClient())
		{
			return null;
		}
		string text = PermissionData.PeerKey(hostname, characterId);
		if (text == "")
		{
			return null;
		}
		Data.UpdatePeer(hostname, characterId, playerName);
		return Data.GetOrCreate(text);
	}

	public static void Save()
	{
		if (Helper.IsClient())
		{
			return;
		}
		SkipReload = true;
		try
		{
			ToFile();
		}
		finally
		{
			SkipReload = false;
		}
	}

	public static void SendPeerPermissions(string hostname, string characterId)
	{
		ZNet instance = ZNet.instance;
		if (!((Object)(object)instance == (Object)null) && instance.IsServer() && !(PermissionData.PeerKey(hostname, characterId) == ""))
		{
			ZNetPeer peerByHostName = instance.GetPeerByHostName(hostname);
			if (peerByHostName != null)
			{
				SendPermissions(peerByHostName.m_rpc, hostname, characterId);
			}
		}
	}

	public static void CreateFile()
	{
		if (!File.Exists(FilePath))
		{
			Data.GetOrCreate("Everyone").commands = new List<string>(1) { "permissions: no" };
			ToFile();
		}
	}

	public static void SendPermissions(ZRpc rpc, string hostname, string characterId)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		PermissionManager permissionManager = Data.Resolve(hostname, characterId);
		ZPackage val = new ZPackage();
		permissionManager.Write(val);
		rpc.Invoke(RPC_Permissions, new object[1] { val });
	}

	private static void SendChangedPermissions(HashSet<string> changedKeys, bool updateAllPlayers)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		if (changedKeys.Count == 0)
		{
			return;
		}
		ZNet instance = ZNet.instance;
		if ((Object)(object)instance == (Object)null || !instance.IsServer())
		{
			return;
		}
		foreach (ZNetPeer peer in instance.m_peers)
		{
			if (peer == null || peer.m_rpc == null || !peer.IsReady() || peer.m_characterID == ZDOID.None)
			{
				continue;
			}
			string hostName = peer.m_rpc.GetSocket().GetHostName();
			ZDO zDO = ZDOMan.instance.GetZDO(peer.m_characterID);
			if (zDO != null)
			{
				string characterId = zDO.GetLong(ZDOVars.s_playerID, 0L).ToString();
				if (!(PermissionData.PeerKey(hostName, characterId) == "") && (updateAllPlayers || PermissionData.IsPeerChanged(changedKeys, hostName, characterId)))
				{
					SendPermissions(peer.m_rpc, hostName, characterId);
				}
			}
		}
	}

	public static void ToFile()
	{
		File.WriteAllText(FilePath, PermissionYaml.SerializeEntries(Data.Entries));
	}

	public static void FromFile()
	{
		if (!Helper.IsClient() && !SkipReload)
		{
			PermissionData permissionData = new PermissionData(PermissionYaml.DeserializeEntries(File.ReadAllText(FilePath)));
			HashSet<string> changedKeys = PermissionData.ChangedKeys(Data, permissionData);
			bool updateAllPlayers = PermissionData.HasGroupChanges(changedKeys, Data, permissionData);
			Data = permissionData;
			SendChangedPermissions(changedKeys, updateAllPlayers);
			Log.Info($"Reloading {Data.Count} permission data.");
		}
	}

	public static void SetupWatcher()
	{
		CreateFile();
		Yaml.SetupWatcher(FileName, FromFile);
	}

	public static void UpdatePeer(ZRpc rpc, string characterId)
	{
		if (characterId == "" || characterId == "0")
		{
			return;
		}
		ZNetPeer peer = ZNet.instance.GetPeer(rpc);
		if (peer != null)
		{
			string hostName = rpc.GetSocket().GetHostName();
			if (Data.UpdatePeer(hostName, characterId, peer.m_playerName))
			{
				Save();
			}
		}
	}
}
