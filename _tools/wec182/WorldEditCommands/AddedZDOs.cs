using HarmonyLib;

namespace WorldEditCommands;

[HarmonyPatch(typeof(ZNetView), "Awake")]
public class AddedZDOs
{
	public static bool Track;

	public static void Postfix(ZNetView __instance)
	{
		if (Track)
		{
			UndoHelper.AddSpawnAction(__instance.GetZDO());
		}
	}
}
