using GUIFramework;
using TMPro;

namespace ServerDevcommands;

public static class Aliasing
{
	private static string Alias = "";

	public static void RemoveAlias(GuiInputField input)
	{
		if (!Settings.Aliasing)
		{
			return;
		}
		Alias = GetAlias(((TMP_InputField)input).text);
		if (!(Alias == string.Empty))
		{
			if (Alias == ((TMP_InputField)input).text)
			{
				Alias = string.Empty;
				return;
			}
			string text = Plain(Alias);
			((TMP_InputField)input).text = text + ((TMP_InputField)input).text.Substring(Alias.Length);
			((TMP_InputField)input).caretPosition = ((TMP_InputField)input).caretPosition + (text.Length - Alias.Length);
		}
	}

	public static void RestoreAlias(GuiInputField input)
	{
		if (Settings.Aliasing && !(Alias == string.Empty))
		{
			string text = Plain(Alias);
			if (((TMP_InputField)input).text.StartsWith(text))
			{
				((TMP_InputField)input).caretPosition = ((TMP_InputField)input).caretPosition - (text.Length - Alias.Length);
				((TMP_InputField)input).text = Alias + ((TMP_InputField)input).text.Substring(text.Length);
			}
		}
	}

	public static string Plain(string command)
	{
		return Plain(command, 10);
	}

	private static string Plain(string command, int rounds)
	{
		if (!Settings.Aliasing)
		{
			return command;
		}
		if (command == "")
		{
			return "";
		}
		if (TerminalUtils.SkipProcessing(command))
		{
			return command;
		}
		if (rounds == 0)
		{
			return command;
		}
		string[] aliasKeys = AliasManager.AliasKeys;
		foreach (string text in aliasKeys)
		{
			if (command.Length < text.Length)
			{
				continue;
			}
			if (command == text)
			{
				return Plain(AliasManager.GetAliasValue(text), rounds - 1);
			}
			if (command != text)
			{
				if (!command.StartsWith(text))
				{
					continue;
				}
				char c = command[text.Length];
				if (c != ' ' && c != ',' && c != '=' && c != ';')
				{
					continue;
				}
			}
			command = TerminalUtils.Substitute(AliasManager.GetAliasValue(text), command.Substring(text.Length + 1));
			return Plain(command, rounds - 1);
		}
		return command;
	}

	private static string GetAlias(string command)
	{
		if (command == "")
		{
			return "";
		}
		if (TerminalUtils.SkipProcessing(command))
		{
			return command;
		}
		string[] aliasKeys = AliasManager.AliasKeys;
		foreach (string text in aliasKeys)
		{
			if (command.Length < text.Length)
			{
				continue;
			}
			if (command != text)
			{
				if (!command.StartsWith(text))
				{
					continue;
				}
				char c = command[text.Length];
				if (c != ' ' && c != ',' && c != '=')
				{
					continue;
				}
			}
			return text;
		}
		return "";
	}
}
