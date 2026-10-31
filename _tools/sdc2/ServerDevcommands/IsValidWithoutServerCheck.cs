using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ConsoleCommand), "IsValid")]
public class IsValidWithoutServerCheck
{
	private static bool Postfix(bool result)
	{
		if (!result && !Terminal.m_cheat)
		{
			ZNet instance = ZNet.instance;
			if (instance == null)
			{
				return false;
			}
			return instance.IsDedicated();
		}
		return true;
	}
}
