using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Game), "Awake")]
public class AutoExec
{
	private static void Postfix()
	{
		if (Settings.AutoExec != "")
		{
			((Terminal)Console.instance).TryRunCommand(Settings.AutoExec, false, false);
		}
	}
}
