using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(MessageHud), "QueueUnlockMsg")]
public class DisableUnlockMessages
{
	private static bool Prefix()
	{
		return !Settings.DisableUnlockMessages;
	}
}
