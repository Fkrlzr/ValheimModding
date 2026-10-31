using System;
using System.Collections.Generic;
using System.Linq;
using Service;
using TMPro;

namespace ServerDevcommands;

public static class TerminalUtils
{
	public static bool IsExecuting;

	public static bool TryResolveInputPlaceholders(Terminal t, string command)
	{
		Terminal t2 = t;
		return CommandInputResolver.TryResolve(command, delegate(string resolved)
		{
			t2.TryRunCommand(resolved, false, false);
		});
	}

	public static string GetLastWord(Terminal obj)
	{
		return ((TMP_InputField)obj.m_input).text.Split(' ').Last().Split('=')
			.Last()
			.Split(',')
			.Last();
	}

	public static IEnumerable<string> GetPositionalParameters(string[] parameters)
	{
		return parameters.Where((string par) => !par.Contains("=") && !par.StartsWith("<", StringComparison.OrdinalIgnoreCase));
	}

	public static void GetSubstitutions(string[] parameters, out IEnumerable<string> mainPars, out IEnumerable<string> substitutions)
	{
		int num = -1;
		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i].Contains(Settings.Substitution))
			{
				num = i + 1;
			}
		}
		if (num == -1)
		{
			mainPars = parameters;
			substitutions = Array.Empty<string>();
		}
		else
		{
			mainPars = parameters.Take(num);
			substitutions = parameters.Skip(num);
		}
	}

	public static int GetAmountOfSubstitutions(string[] parameters)
	{
		int num = -1;
		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i].Contains(Settings.Substitution))
			{
				num = i + 1;
			}
		}
		if (num == -1)
		{
			return 0;
		}
		return parameters.Skip(num).Count((string par) => !par.Contains("="));
	}

	private static string ReplaceValues(string text, string search, Queue<string> replace)
	{
		int num = text.IndexOf(search);
		while (num >= 0 && replace.Count > 0)
		{
			string text2 = replace.Dequeue();
			text = text.Substring(0, num) + text2 + text.Substring(num + search.Length);
			num = text.IndexOf(search);
		}
		return text;
	}

	public static bool CanSubstitute(string input)
	{
		return input.Contains(Settings.Substitution);
	}

	public static string Substitute(string alias, string command)
	{
		if (Settings.Substitution == "")
		{
			return alias + " " + command;
		}
		if (alias.StartsWith("alias"))
		{
			return alias + " " + command;
		}
		if (!CanSubstitute(alias))
		{
			return alias + " " + command;
		}
		Queue<string> queue = new Queue<string>(command.Split(' '));
		alias = ReplaceValues(alias, Settings.Substitution, queue);
		alias = alias.Replace("," + Settings.Substitution, "");
		if (CanSubstitute(alias))
		{
			alias = string.Join(" ", from s in alias.Split(' ')
				where !s.Contains(Settings.Substitution)
				select s);
		}
		return alias + " " + string.Join(" ", queue);
	}

	public static bool IsBlockedHelp(string command)
	{
		if (Settings.DisableHelpCommand)
		{
			return command.Split(' ')[0].Equals("help", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	public static bool SkipProcessing(string command)
	{
		string command2 = command;
		return AutoComplete.Offsets.Any<KeyValuePair<string, int>>((KeyValuePair<string, int> kvp) => command2.StartsWith(kvp.Key + " ", StringComparison.OrdinalIgnoreCase));
	}
}
