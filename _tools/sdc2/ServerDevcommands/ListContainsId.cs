using HarmonyLib;
using Splatform;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ZNet), "ListContainsId")]
public class ListContainsId
{
	private static bool Prefix(ZNet __instance, SyncedList list, string idString, ref bool __result)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (!idString.Contains("_"))
		{
			idString = PlatformUserID.GetPlatformPrefix(ZNet.instance.m_steamPlatform) + idString;
		}
		if (list != __instance.m_adminList)
		{
			return true;
		}
		bool? flag = PermissionLoader.Data.ResolveAdminOverride(idString);
		if (!flag.HasValue)
		{
			return true;
		}
		__result = flag.Value;
		return false;
	}
}
