using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Player), "Awake")]
public class AutomaticPickUp
{
	private static void Postfix()
	{
		Player.m_enableAutoPickup = Settings.AutomaticItemPickUp(Player.m_enableAutoPickup);
	}
}
