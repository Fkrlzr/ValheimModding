using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Terminal), "IsCheatsEnabled")]
public class IsCheatsEnabledWithoutServerCheck
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
