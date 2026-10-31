using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ServerDevcommands;

public static class AutoComplete
{
	private static readonly Dictionary<string, Func<int, int, string[], List<string>>> OptionsFetchers = new Dictionary<string, Func<int, int, string[], List<string>>>();

	private static readonly Dictionary<string, Dictionary<string, Func<int, List<string>>>> OptionsNamedFetchers = new Dictionary<string, Dictionary<string, Func<int, List<string>>>>();

	public static Dictionary<string, int> Offsets = new Dictionary<string, int>();

	public static List<string>? GetOptions(string command, int index)
	{
		command = command.ToLower();
		if (OptionsFetchers.ContainsKey(command))
		{
			return OptionsFetchers[command](index, 0, Array.Empty<string>());
		}
		if (index == 0 && Terminal.commands.TryGetValue(command, out var value) && value.m_tabOptionsFetcher != null)
		{
			return value.m_tabOptionsFetcher.Invoke();
		}
		return null;
	}

	private static List<string> GetOptions(string command, int index, int subIndex, string namedArg, string[] args)
	{
		command = command.ToLower();
		if (namedArg != "")
		{
			if (OptionsNamedFetchers.TryGetValue(command, out Dictionary<string, Func<int, List<string>>> value) && value.TryGetValue(namedArg.ToLower(), out var value2))
			{
				return value2(index) ?? ParameterInfo.None;
			}
			return ParameterInfo.InvalidNamed(namedArg);
		}
		if (OptionsFetchers.ContainsKey(command))
		{
			return OptionsFetchers[command](index, subIndex, args) ?? ParameterInfo.None;
		}
		if (Terminal.commands.TryGetValue(command, out var value3) && value3.m_tabOptionsFetcher != null)
		{
			if (index == 0)
			{
				return value3.m_tabOptionsFetcher.Invoke() ?? ParameterInfo.Missing;
			}
			CaptureCollection captures = Regex.Match(value3.Description ?? "", "^(?:\\s*\\[([^\\]]+)\\])+").Groups[1].Captures;
			if (captures.Count == 0)
			{
				return ParameterInfo.Missing;
			}
			if (index >= captures.Count)
			{
				return ParameterInfo.None;
			}
			return ParameterInfo.Create(captures[index].Value);
		}
		return ParameterInfo.Missing;
	}

	public static void Register(string command, Func<int, int, string[], List<string>> fetcher, Dictionary<string, Func<int, List<string>>> namedFetchers)
	{
		OptionsFetchers[command.ToLower()] = fetcher;
		OptionsNamedFetchers[command.ToLower()] = namedFetchers.ToDictionary<KeyValuePair<string, Func<int, List<string>>>, string, Func<int, List<string>>>((KeyValuePair<string, Func<int, List<string>>> kvp) => kvp.Key.ToLower(), (KeyValuePair<string, Func<int, List<string>>> kvp) => kvp.Value);
	}

	public static void Register(string command, Func<int, int, List<string>> fetcher, Dictionary<string, Func<int, List<string>>> namedFetchers)
	{
		Func<int, int, List<string>> fetcher2 = fetcher;
		Register(command, (int index, int subIndex, string[] args) => fetcher2(index, subIndex), namedFetchers);
	}

	public static void Register(string command, Func<int, List<string>> fetcher, Dictionary<string, Func<int, List<string>>> namedFetchers)
	{
		Func<int, List<string>> fetcher2 = fetcher;
		Register(command, (int index, int subIndex, string[] args) => fetcher2(index), namedFetchers);
	}

	public static void Register(string command, Func<int, int, string[], List<string>> fetcher)
	{
		OptionsFetchers[command] = fetcher;
	}

	public static void Register(string command, Func<int, int, List<string>> fetcher)
	{
		Func<int, int, List<string>> fetcher2 = fetcher;
		OptionsFetchers[command] = (int index, int subIndex, string[] args) => fetcher2(index, subIndex);
	}

	public static void Register(string command, Func<int, List<string>> fetcher)
	{
		Func<int, List<string>> fetcher2 = fetcher;
		OptionsFetchers[command] = (int index, int subIndex, string[] args) => fetcher2(index);
	}

	public static void RegisterEmpty(string command)
	{
		Register(command, (int index) => ParameterInfo.None);
	}

	public static void RegisterAdmin(string command)
	{
		Register(command, (int index) => (index == 0) ? ParameterInfo.Create("Name / IP / UserId") : ParameterInfo.None);
	}

	public static void RegisterDefault(string command)
	{
		string command2 = command;
		Register(command2, (int index) => (index == 0) ? Terminal.commands[command2].m_tabOptionsFetcher.Invoke() : ParameterInfo.None);
	}

	public static List<string> GetOptions(string[] args)
	{
		string text = args.First();
		if (Offsets.TryGetValue(text, out var value) && args.Length - 1 > value)
		{
			List<string> list = new List<string>();
			list.AddRange(args.Skip(value + 1));
			args = list.ToArray();
			text = args.First();
		}
		if (args.Length < 2)
		{
			return ((Terminal)Console.instance).m_commandList;
		}
		args = args.Skip(1).ToArray();
		string parameter = args.Last();
		string name = GetName(parameter);
		int subIndex = GetSubIndex(parameter);
		int num;
		if (name != "")
		{
			num = subIndex;
		}
		else
		{
			num = TerminalUtils.GetPositionalParameters(args).Count() - 1;
			if (Settings.Substitution != "")
			{
				int num2 = TerminalUtils.GetAmountOfSubstitutions(args);
				for (int i = 0; i < args.Length; i++)
				{
					if (num2 <= 0)
					{
						break;
					}
					string parameter2 = args[i];
					int num3 = CountSubstitution(parameter2);
					num -= num3;
					num2 -= num3;
					if (num2 <= 0)
					{
						name = GetName(parameter2);
						num = ((!(name != "")) ? i : (num2 + GetSubIndex(parameter) + GetSubIndex(parameter2)));
					}
				}
			}
		}
		return GetOptions(text, num, subIndex, name, args);
	}

	private static string GetName(string parameter)
	{
		string[] array = parameter.Split('=');
		if (array.Length < 2)
		{
			return "";
		}
		return array[0];
	}

	private static int GetSubIndex(string parameter)
	{
		return parameter.Split(',').Length - 1;
	}

	private static int CountSubstitution(string parameter)
	{
		int num = 0;
		int length = Settings.Substitution.Length;
		int num2 = -length;
		while ((num2 = parameter.IndexOf(Settings.Substitution, num2 + length)) > -1)
		{
			num++;
		}
		return num;
	}
}
