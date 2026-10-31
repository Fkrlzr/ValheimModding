using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(MessageHud), "ShowMessage")]
public class PreventMessages
{
	public static bool Enabled;

	private static bool Prefix()
	{
		return !Enabled;
	}
}
