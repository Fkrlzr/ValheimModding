using System;
using System.Collections.Generic;
using ServerDevcommands;

namespace WorldEditCommands;

public class TweakRunestoneCommand : TweakCommand
{
	protected override string DoOperation(ZNetView view, string operation, string? value)
	{
		return operation switch
		{
			"text" => TweakActions.Text(view, value), 
			"name" => TweakActions.Name(view, value), 
			"topic" => TweakActions.Topic(view, value), 
			"discover" => TweakActions.Discover(view, value), 
			_ => throw new NotImplementedException(), 
		};
	}

	protected override string DoOperation(ZNetView view, string operation, float? value)
	{
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, int? value)
	{
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, string[] value)
	{
		if (operation == "compendium")
		{
			return TweakActions.Compendium(view, value);
		}
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, bool? value)
	{
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, long? value)
	{
		throw new NotImplementedException();
	}

	public TweakRunestoneCommand()
	{
		Component = typeof(RuneStone);
		ComponentName = "runestone";
		SupportedOperations.Add("name", typeof(string));
		SupportedOperations.Add("text", typeof(string));
		SupportedOperations.Add("compendium", typeof(string[]));
		SupportedOperations.Add("topic", typeof(string));
		SupportedOperations.Add("discover", typeof(string));
		AutoComplete.Add("name", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("name=<color=yellow>text</color>", "Display name. Use _ as the space. No value to reset."));
		AutoComplete.Add("text", (int index) => ParameterInfo.Create("text=<color=yellow>text</color>", "Shown text. Use _ as the space. No value to reset."));
		AutoComplete.Add("compendium", (int index) => ParameterInfo.Create("compendium=<color=yellow>text</color>", "Entry in the compendium. Use _ as the space. No value to reset. Add new parameter for more entries."));
		AutoComplete.Add("topic", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("topic=<color=yellow>text</color>", "Show text topic. Use _ as the space. No value to reset."));
		AutoComplete.Add("discover", delegate(int index)
		{
			switch (index)
			{
			case 0:
				return ParameterInfo.LocationIds;
			case 1:
				return ParameterInfo.Create("discover=id,<color=yellow>pinName</color>,pinType,openMap", "Pin name on the map.");
			case 2:
			{
				string[] names = Enum.GetNames(typeof(PinType));
				List<string> list = new List<string>(names.Length);
				list.AddRange(names);
				return list;
			}
			case 3:
				return ParameterInfo.Create("discover=id,pinName,pinType,<color=yellow>openMap</color>", "1 = automatically open the map.");
			default:
				return ParameterInfo.None;
			}
		});
		Init("tweak_runestone", "Modify runestones");
	}
}
