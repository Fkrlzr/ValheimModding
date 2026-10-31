using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Terminal), "UpdateInput")]
public class ConsoleWheelScroll
{
	private const int LinesPerTick = 3;

	private static void Prefix(Terminal __instance, out int __state)
	{
		__state = __instance.m_scrollHeight;
	}

	private static void Postfix(Terminal __instance, int __state)
	{
		if ((Object)(object)__instance != (Object)(object)Console.instance)
		{
			return;
		}
		int num = Mathf.Max(1, __instance.m_maxVisibleBufferLength - 1);
		bool flag = ZInput.GetKey((KeyCode)304, true) || ZInput.GetKey((KeyCode)303, true);
		bool flag2 = ZInput.GetKey((KeyCode)306, true) || ZInput.GetKey((KeyCode)305, true);
		float mouseScrollWheel = ZInput.GetMouseScrollWheel();
		if (mouseScrollWheel != 0f)
		{
			int num2 = (flag ? num : 3);
			ConsoleScrollBar.Scroll(__instance, __instance.m_scrollHeight + ((mouseScrollWheel > 0f) ? num2 : (-num2)));
			return;
		}
		bool keyDown = ZInput.GetKeyDown((KeyCode)280, true);
		if (keyDown || ZInput.GetKeyDown((KeyCode)281, true))
		{
			if (flag2)
			{
				ConsoleScrollBar.Scroll(__instance, keyDown ? int.MaxValue : 0);
			}
			else if (flag)
			{
				ConsoleScrollBar.Scroll(__instance, __state + (keyDown ? num : (-num)));
			}
		}
	}
}
