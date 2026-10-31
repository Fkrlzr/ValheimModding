using System;
using System.Linq;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class TweakDungeonCommand : TweakCommand
{
	protected override string DoOperation(ZNetView view, string operation, string? value)
	{
		return operation switch
		{
			"enter_text" => TweakActions.EnterText(view, value), 
			"enter_hover" => TweakActions.EnterHover(view, value), 
			"exit_text" => TweakActions.ExitText(view, value), 
			"exit_hover" => TweakActions.ExitHover(view, value), 
			"weather" => TweakActions.Weather(view, Hash.DungeonWeather, value), 
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

	protected override ZNetView Preprocess(Terminal context, ZNetView view)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)((Component)view).GetComponent<LocationProxy>()))
		{
			return view;
		}
		if (Object.op_Implicit((Object)(object)((Component)view).GetComponent<DungeonGenerator>()))
		{
			Vector2s zone = ZoneSystem.GetZone(((Component)view).transform.position);
			Location val = Location.s_allLocations.FirstOrDefault((Location l) => ZoneSystem.GetZone(((Component)l).transform.position) == zone);
			if (Object.op_Implicit((Object)(object)val))
			{
				return ((Component)val).GetComponentInParent<ZNetView>();
			}
		}
		context.AddString("Skipped: " + ((Object)view).name + " is not a location.");
		return null;
	}

	public TweakDungeonCommand()
	{
		SupportedOperations.Add("enter_text", typeof(string));
		SupportedOperations.Add("enter_hover", typeof(string));
		SupportedOperations.Add("exit_text", typeof(string));
		SupportedOperations.Add("exit_hover", typeof(string));
		SupportedOperations.Add("weather", typeof(string));
		AutoComplete.Add("enter_text", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("enter_text=<color=yellow>text</color>", "Text when entering the dungeon."));
		AutoComplete.Add("enter_hover", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("enter_hover=<color=yellow>text</color>", "Text when entering the dungeon."));
		AutoComplete.Add("exit_text", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("exit_text=<color=yellow>text</color>", "Text when entering the dungeon."));
		AutoComplete.Add("exit_hover", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("exit_hover=<color=yellow>text</color>", "Text when entering the dungeon."));
		AutoComplete.Add("weather", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Environments);
		Init("tweak_dungeon", "Modify dungeons");
	}
}
