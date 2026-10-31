using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Player), "OnSpawned")]
public class AdminCheck
{
	private static void Postfix()
	{
		if (Game.instance.m_firstSpawn)
		{
			Admin.AutomaticCheck();
		}
		else
		{
			DevcommandsCommand.EnableAutoFeatures();
		}
	}
}
