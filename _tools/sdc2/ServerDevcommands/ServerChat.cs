using System;
using HarmonyLib;
using Splatform;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Chat), "SendText")]
public class ServerChat
{
	private static PlayerInfo? serverClient;

	private static UserInfo? userInfo;

	public static PlayerInfo ServerClient
	{
		get
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			PlayerInfo valueOrDefault = serverClient.GetValueOrDefault();
			if (!serverClient.HasValue)
			{
				valueOrDefault = CreatePlayerInfo();
				serverClient = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
	}

	public static UserInfo UserInfo
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			object obj = userInfo;
			if (obj == null)
			{
				UserInfo val = new UserInfo
				{
					Name = ServerClient.m_userInfo.m_displayName,
					UserId = ServerClient.m_userInfo.m_id
				};
				userInfo = val;
				obj = (object)val;
			}
			return (UserInfo)obj;
		}
	}

	public static void Clear()
	{
		serverClient = null;
		userInfo = null;
	}

	private static PlayerInfo CreatePlayerInfo()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		PlayerInfo result = default(PlayerInfo);
		result.m_name = Settings.ServerChatName;
		result.m_characterID = new ZDOID(ZDOMan.GetSessionID(), uint.MaxValue);
		result.m_userInfo = new CrossNetworkUserInfo
		{
			m_id = GetServerUserId(),
			m_displayName = Settings.ServerChatName,
			m_serverAssignedDisplayName = Settings.ServerChatName
		};
		result.m_publicPosition = false;
		result.m_position = Vector3.zero;
		return result;
	}

	public static void RefreshPlayerInfo()
	{
		serverClient = null;
		userInfo = null;
	}

	private static PlatformUserID GetServerUserId()
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Invalid comparison between Unknown and I4
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = (Type.GetType("Steamworks.SteamGameServer, com.rlabrecque.steamworks.net", throwOnError: false)?.GetMethod("GetSteamID"))?.Invoke(null, null)?.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				return new PlatformUserID(ZNet.instance.m_steamPlatform, text);
			}
		}
		catch
		{
		}
		string text2 = ZPlayFabMatchmaking.m_instance?.m_serverData?.remotePlayerId;
		if ((int)ZNet.m_onlineBackend == 1 && !string.IsNullOrEmpty(text2))
		{
			return new PlatformUserID("playfab", text2);
		}
		if (ZNet.instance.m_hostSocket == null)
		{
			return new PlatformUserID(ZNet.instance.m_steamPlatform, "Server");
		}
		return new PlatformUserID(ZNet.instance.m_steamPlatform, ZNet.instance.m_hostSocket.GetHostName());
	}

	public static void Write(ZPackage pkg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		pkg.Write(ServerClient.m_name);
		pkg.Write(ServerClient.m_characterID);
		PlayerInfo val = ServerClient;
		((CrossNetworkUserInfo)(ref val.m_userInfo)).Write(pkg);
		pkg.Write(false);
	}

	private static void Postfix(Type type, string text)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected I4, but got Unknown
		if (!Object.op_Implicit((Object)(object)Player.m_localPlayer) && Settings.IsServerChat && ZRoutedRpc.instance != null)
		{
			ZRoutedRpc.instance.InvokeRoutedRPC(0L, "ChatMessage", new object[4]
			{
				Vector3.zero,
				(int)type,
				UserInfo,
				text
			});
		}
	}
}
