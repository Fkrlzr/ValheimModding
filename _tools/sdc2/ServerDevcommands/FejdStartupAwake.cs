using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(FejdStartup), "Awake")]
public class FejdStartupAwake
{
	private static void Postfix()
	{
		if (Settings.AutoExecBoot != "")
		{
			((Terminal)Console.instance).TryRunCommand(Settings.AutoExecBoot, false, false);
		}
	}
}
