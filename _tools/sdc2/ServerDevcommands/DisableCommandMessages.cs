using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ConsoleCommand), "RunAction")]
public class DisableCommandMessages
{
	private static void Prefix()
	{
		PreventMessages.Enabled = Settings.DisableMessages;
	}

	private static void Postfix()
	{
		PreventMessages.Enabled = false;
	}
}
