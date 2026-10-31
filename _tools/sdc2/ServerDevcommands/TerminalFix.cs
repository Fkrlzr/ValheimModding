using System;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(TMP_InputField), "KeyPressed")]
public class TerminalFix
{
	private static int caretPos;

	private static int textLen;

	private static int anchorPos;

	private static void Prefix(TMP_InputField __instance)
	{
		textLen = __instance.text.Length;
		caretPos = __instance.caretPosition;
		anchorPos = __instance.selectionAnchorPosition;
	}

	private static void Postfix(TMP_InputField __instance, Event evt)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		if (caretPos != anchorPos)
		{
			if (__instance.caretPosition == 0 && ((int)evt.keyCode == 127 || (int)evt.keyCode == 8) && textLen != __instance.text.Length)
			{
				__instance.caretPosition = Math.Min(Math.Min(caretPos, anchorPos), textLen);
			}
			if (__instance.caretPosition == 1 && evt.character != 0)
			{
				__instance.caretPosition = Math.Min(Math.Min(caretPos, anchorPos), textLen) + 1;
			}
		}
	}
}
