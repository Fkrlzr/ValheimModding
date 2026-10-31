using System;
using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ZNet), "RPC_Unban")]
public class RPC_Unban
{
	private static bool Prefix(ZNet __instance, ZRpc rpc, string user)
	{
		if (!user.StartsWith("admintest_", StringComparison.Ordinal))
		{
			return true;
		}
		if (!__instance.IsServer())
		{
			return true;
		}
		string value = Parse.Kvp(user, '_').Value;
		PermissionLoader.UpdatePeer(rpc, value);
		string hostName = rpc.GetSocket().GetHostName();
		PermissionLoader.SendPermissions(rpc, hostName, value);
		return false;
	}
}
