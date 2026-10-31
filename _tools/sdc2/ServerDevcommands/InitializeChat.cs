using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Chat), "Awake")]
public class InitializeChat
{
	private static void Postfix()
	{
		BindManager.Load();
	}
}
