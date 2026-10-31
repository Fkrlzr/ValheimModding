using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Terminal), "TryRunCommand")]
public class LogCommand
{
	private static void Postfix(string text)
	{
		if (!TerminalUtils.IsBlockedHelp(text) && Object.op_Implicit((Object)(object)ZNet.instance) && !ZNet.instance.IsServer())
		{
			ZRpc serverRPC = ZNet.instance.GetServerRPC();
			if (serverRPC != null)
			{
				serverRPC.Invoke(CommandLogging.RPC_Log, new object[1] { text });
			}
		}
	}
}
