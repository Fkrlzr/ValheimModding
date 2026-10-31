using System.Collections.Generic;
using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ZoneSystem))]
public class CleanServerModifiers
{
	[HarmonyPatch("GlobalKeyAdd")]
	[HarmonyPostfix]
	private static void GlobalKeyAdd()
	{
		Clean();
	}

	[HarmonyPatch("GlobalKeyRemove")]
	[HarmonyPostfix]
	private static void GlobalKeyRemove()
	{
		Clean();
	}

	private static void Clean()
	{
		if (ZNet.World?.m_startingGlobalKeys == null)
		{
			return;
		}
		List<string> startingGlobalKeys = ZNet.World.m_startingGlobalKeys;
		for (int num = startingGlobalKeys.Count - 1; num >= 0; num--)
		{
			string text = startingGlobalKeys[num].Split(' ')[0].ToLowerInvariant();
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				string text2 = startingGlobalKeys[num2].Split(' ')[0].ToLowerInvariant();
				if (text == text2)
				{
					startingGlobalKeys.RemoveAt(num2);
					num--;
				}
			}
		}
	}
}
