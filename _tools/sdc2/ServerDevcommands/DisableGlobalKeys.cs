using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ZoneSystem), "RPC_SetGlobalKey")]
public class DisableGlobalKeys
{
	private static bool Prefix(string name)
	{
		return !Settings.IsGlobalKeyDisabled(name);
	}

	public static void RemoveDisabled()
	{
		if (!Object.op_Implicit((Object)(object)ZNet.instance) || !ZNet.instance.IsServer())
		{
			return;
		}
		ZoneSystem instance = ZoneSystem.instance;
		if (Object.op_Implicit((Object)(object)instance) && instance.m_globalKeys.Any(Settings.IsGlobalKeyDisabled))
		{
			instance.m_globalKeys.RemoveWhere(Settings.IsGlobalKeyDisabled);
			instance.m_globalKeysValues = instance.m_globalKeysValues.Where((KeyValuePair<string, string> key) => !Settings.IsGlobalKeyDisabled(key.Key)).ToDictionary((KeyValuePair<string, string> key) => key.Key, (KeyValuePair<string, string> key) => key.Value);
			instance.SendGlobalKeys(0L);
		}
	}
}
