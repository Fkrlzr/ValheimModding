using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ConsoleCommand), "GetTabOptions")]
public class GetTabOptionsWithImprovedAutoComplete
{
	private static bool Prefix(Terminal __instance, ref List<string> __result)
	{
		if ((Object)(object)__instance == (Object)(object)Chat.instance)
		{
			return true;
		}
		if (!Settings.ImprovedAutoComplete)
		{
			return true;
		}
		if (TerminalUtils.IsExecuting)
		{
			__result = new List<string>();
		}
		else
		{
			string[] args = ParseParameters(((TMP_InputField)((Terminal)Console.instance).m_input).text.Split(';').Last());
			__result = AutoComplete.GetOptions(args);
		}
		return false;
	}

	private static string[] ParseParameters(string arg)
	{
		List<string> list = new List<string>();
		string[] array = arg.Split(' ');
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].StartsWith("\""))
			{
				int j;
				for (j = i; j < array.Length && !array[j].TrimEnd().EndsWith("\""); j++)
				{
				}
				list.Add(string.Join(" ", array.Skip(i).Take(j - i + 1)));
				i = j;
			}
			else
			{
				list.Add(array[i].Trim());
			}
		}
		return list.ToArray();
	}
}
