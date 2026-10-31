using GUIFramework;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Terminal), "UpdateInput")]
public class PlainInputForAutoComplete
{
	private static int Anchor = 0;

	private static int Focus = 0;

	private static string[]? PreviousCommands = null;

	private static int CurrentCommand = -1;

	private static int DiscardCaretDelta = 0;

	private static bool Prefix(Terminal __instance)
	{
		if ((Object)(object)__instance == (Object)(object)Chat.instance)
		{
			return true;
		}
		if (ZInput.GetKeyDown((KeyCode)13, true) && ZInput.GetKeyDown((KeyCode)9, true))
		{
			return false;
		}
		if (ZInput.GetKeyDown((KeyCode)13, true))
		{
			return true;
		}
		if (ZInput.GetButtonDown("ChatUp") || ZInput.GetButtonDown("ChatDown"))
		{
			return true;
		}
		if (ZInput.GetKey((KeyCode)306, true) || ZInput.GetKey((KeyCode)305, true))
		{
			return true;
		}
		ToCurrentInput(__instance);
		return true;
	}

	private static void Postfix(Terminal __instance)
	{
		if (!((Object)(object)__instance == (Object)(object)Chat.instance) && !ZInput.GetKeyDown((KeyCode)13, true) && !ZInput.GetButtonDown("ChatUp") && !ZInput.GetButtonDown("ChatDown") && !ZInput.GetKey((KeyCode)306, true) && !ZInput.GetKey((KeyCode)305, true))
		{
			ToActualInput(__instance);
		}
	}

	private static void ToCurrentInput(Terminal terminal)
	{
		GuiInputField input = terminal.m_input;
		Anchor = ((TMP_InputField)input).selectionAnchorPosition;
		Focus = ((TMP_InputField)input).selectionFocusPosition;
		DiscardPreviousCommands(input);
		Aliasing.RemoveAlias(input);
	}

	private static void ToActualInput(Terminal terminal)
	{
		GuiInputField input = terminal.m_input;
		Aliasing.RestoreAlias(input);
		RestorePreviousCommands(input);
		if (ZInput.GetKeyDown((KeyCode)9, true))
		{
			((TMP_InputField)input).selectionAnchorPosition = ((TMP_InputField)input).text.Length;
			((TMP_InputField)input).selectionFocusPosition = ((TMP_InputField)input).text.Length;
		}
		else
		{
			((TMP_InputField)input).selectionAnchorPosition = Anchor;
			((TMP_InputField)input).selectionFocusPosition = Focus;
		}
	}

	private static void DiscardPreviousCommands(GuiInputField input)
	{
		if (!Settings.MultiCommand)
		{
			return;
		}
		PreviousCommands = ((TMP_InputField)input).text.Split(';');
		DiscardCaretDelta = ((TMP_InputField)input).caretPosition;
		for (CurrentCommand = 0; CurrentCommand < PreviousCommands.Length; CurrentCommand++)
		{
			string text = PreviousCommands[CurrentCommand];
			if (((TMP_InputField)input).caretPosition <= text.Length)
			{
				break;
			}
			((TMP_InputField)input).caretPosition = ((TMP_InputField)input).caretPosition - text.Length;
			int caretPosition = ((TMP_InputField)input).caretPosition;
			((TMP_InputField)input).caretPosition = caretPosition - 1;
		}
		DiscardCaretDelta -= ((TMP_InputField)input).caretPosition;
		if (CurrentCommand >= PreviousCommands.Length)
		{
			PreviousCommands = null;
			CurrentCommand = -1;
		}
		else
		{
			((TMP_InputField)input).text = PreviousCommands[CurrentCommand];
		}
	}

	private static void RestorePreviousCommands(GuiInputField input)
	{
		if (PreviousCommands != null)
		{
			PreviousCommands[CurrentCommand] = ((TMP_InputField)input).text;
			((TMP_InputField)input).text = string.Join(";", PreviousCommands);
			if (DiscardCaretDelta != 0)
			{
				((TMP_InputField)input).caretPosition = ((TMP_InputField)input).caretPosition + DiscardCaretDelta;
			}
			PreviousCommands = null;
			CurrentCommand = -1;
		}
	}
}
