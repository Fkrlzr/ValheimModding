using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Chat), "SendText")]
public class HideShoutPing
{
	private static void Prefix(ref Vector3 __state)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)Player.m_localPlayer) && Settings.HideShoutPings)
		{
			__state = ((Character)Player.m_localPlayer).m_head.position;
			((Character)Player.m_localPlayer).m_head.position = default(Vector3);
		}
	}

	private static void Postfix(Vector3 __state)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)Player.m_localPlayer) && Settings.HideShoutPings)
		{
			((Character)Player.m_localPlayer).m_head.position = __state;
		}
	}
}
