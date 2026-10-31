using Data;
using HarmonyLib;

namespace WorldEditCommands;

[HarmonyPatch(typeof(ZoneSystem), "Start")]
public class ZoneSystemStart
{
	private static void Postfix()
	{
		FieldAutoComplete.Init();
		DataLoading.LoadEntries();
	}
}
