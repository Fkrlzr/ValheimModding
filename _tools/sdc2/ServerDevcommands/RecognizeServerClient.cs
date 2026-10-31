using HarmonyLib;
using Splatform;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ZNet), "TryGetPlayerByPlatformUserID")]
public class RecognizeServerClient
{
	private static bool Postfix(bool result, PlatformUserID platformUserID, ref PlayerInfo playerInfo)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (result || !Settings.IsServerChat)
		{
			return result;
		}
		if (platformUserID != ServerChat.ServerClient.m_userInfo.m_id)
		{
			return result;
		}
		playerInfo = ServerChat.ServerClient;
		return true;
	}
}
