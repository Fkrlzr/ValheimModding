using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Minimap), "UpdatePlayerPins")]
public class AddCheckedToPrivatePlayers
{
	private static void Postfix(Minimap __instance)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (Settings.ShowPrivatePlayers)
		{
			for (int i = 0; i < __instance.m_tempPlayerInfo.Count; i++)
			{
				PinData obj = __instance.m_playerPins[i];
				PlayerInfo val = __instance.m_tempPlayerInfo[i];
				obj.m_checked = !val.m_publicPosition;
			}
		}
	}
}
