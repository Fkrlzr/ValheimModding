using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(/*Could not decode attribute arguments.*/)]
public class Wrapping
{
	private static void Postfix(ConsoleEventArgs __instance)
	{
		if (string.IsNullOrWhiteSpace(Settings.Wrapping) || !__instance.FullLine.Contains(Settings.Wrapping))
		{
			return;
		}
		List<string> list = new List<string>();
		string text = "";
		string[] args = __instance.Args;
		foreach (string text2 in args)
		{
			if (text == "")
			{
				if (text2.Contains(Settings.Wrapping))
				{
					if (text2.Count((char c) => c == Settings.Wrapping[0]) % 2 == 0 && text2.EndsWith(Settings.Wrapping, StringComparison.OrdinalIgnoreCase))
					{
						int startIndex = text2.IndexOf(Settings.Wrapping);
						int startIndex2 = text2.LastIndexOf(Settings.Wrapping);
						string item = text2.Remove(startIndex2, 1).Remove(startIndex, 1);
						list.Add(item);
					}
					else
					{
						text = text2;
					}
				}
				else
				{
					list.Add(text2);
				}
			}
			else
			{
				text = text + " " + text2;
				if (text2.EndsWith(Settings.Wrapping, StringComparison.OrdinalIgnoreCase))
				{
					int startIndex3 = text.IndexOf(Settings.Wrapping);
					int startIndex4 = text.LastIndexOf(Settings.Wrapping);
					string item2 = text.Remove(startIndex4, 1).Remove(startIndex3, 1);
					list.Add(item2);
					text = "";
				}
			}
		}
		__instance.Args = list.ToArray();
	}
}
