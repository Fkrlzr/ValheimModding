using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Player), "UpdatePlacement")]
public class PreventRotation
{
	private static void Prefix(Player __instance, ref int __state)
	{
		__state = __instance.m_placeRotation;
	}

	private static void Postfix(Player __instance, int __state)
	{
		if (MouseWheelBinding.CouldExecute())
		{
			__instance.m_placeRotation = __state;
		}
	}
}
