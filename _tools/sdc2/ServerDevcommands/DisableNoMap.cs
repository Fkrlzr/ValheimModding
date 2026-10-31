using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Game), "UpdateNoMap")]
public class DisableNoMap
{
	private static void Postfix()
	{
		if (Object.op_Implicit((Object)(object)Player.m_localPlayer) && Settings.DisableNoMap)
		{
			Game.m_noMap = false;
			Minimap.instance.SetMapMode((MapMode)1);
		}
	}
}
