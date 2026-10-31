using HarmonyLib;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch(typeof(Player), "SetupPlacementGhost")]
[HarmonyPriority(0)]
public class Player_ManualUpdate
{
	public static CircleProjector? Projector;

	public static void Update()
	{
		Player localPlayer = Player.m_localPlayer;
		if (Object.op_Implicit((Object)(object)localPlayer) && Object.op_Implicit((Object)(object)localPlayer.m_placementGhost) && localPlayer.m_placementGhost.activeInHierarchy && Object.op_Implicit((Object)(object)Projector) && ((Component)Projector).gameObject.activeInHierarchy)
		{
			Projector.Update();
		}
	}

	private static void Postfix(Player __instance)
	{
		if ((Object)(object)__instance != (Object)(object)Player.m_localPlayer)
		{
			return;
		}
		Projector = null;
		if (Object.op_Implicit((Object)(object)__instance.m_placementGhost))
		{
			Projector = __instance.m_placementGhost.GetComponentInChildren<CircleProjector>(true);
			if (Object.op_Implicit((Object)(object)Projector))
			{
				((Behaviour)Projector).enabled = false;
			}
		}
	}
}
