using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ZNet), "RPC_RemotePrint")]
public class ZNet_RPC_RemotePrint
{
	private static bool Prefix(string text)
	{
		if (!Admin.Checking)
		{
			return true;
		}
		return !Admin.Verify(text);
	}
}
