using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ServerDevcommands;

namespace InfinityHammer;

public class CommandValue
{
	public string Command = Plain(MultiCommands.Split(ReplaceHelpers(command)));

	public string[] Keys;

	public string[] BannedKeys;

	public CommandValue(string command, string keys)
	{
		List<string> list = new List<string>();
		list.AddRange(from k in Parse.Split(keys, ',')
			where k[0] != '-'
			select k);
		Keys = list.ToArray();
		List<string> list2 = new List<string>();
		list2.AddRange(from k in Parse.Split(keys, ',')
			where k[0] == '-'
			select k.Substring(1));
		BannedKeys = list2.ToArray();
		base._002Ector();
	}

	public bool IsDown()
	{
		if (Keys.All(HammerHelper.IsDown))
		{
			return !BannedKeys.Any(HammerHelper.IsDown);
		}
		return false;
	}

	private static string ReplaceHelpers(string command)
	{
		return Regex.Replace(command, "<p(\\d+)>", "<x$1>,<z$1>,<y$1>").Replace("hoe_", "tool_").Replace("hammer_command", "")
			.Replace("<area>", "from=<x>,<z>,<y> circle=<r>-<r2> angle=<a> rect=<w>-<w2>,<d> ignore=<ignore> id=<include>")
			.Replace("<place>", "from=<x>,<z>,<y>")
			.Replace("<to>", "to=<x>,<z>,<y> circle=<r>-<r2> rect=<w>-<w2>,<d>");
	}

	private static string Plain(string[] commands)
	{
		for (int i = 0; i < commands.Length; i++)
		{
			commands[i] = Aliasing.Plain(commands[i]);
		}
		return string.Join("; ", commands);
	}
}
