using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class TweakItemStandCommand : TweakCommand
{
	protected override string DoOperation(ZNetView view, string operation, string? value)
	{
		if (operation == "item")
		{
			return TweakActions.Item(view, value);
		}
		if (operation == "name")
		{
			return TweakActions.Name(view, value);
		}
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, float? value)
	{
		if (operation == "respawn")
		{
			return TweakActions.Respawn(view, Hash.Respawn, value);
		}
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

	protected override Dictionary<string, object?> Postprocess(ZNetView view, Dictionary<string, object?> operations)
	{
		if (!operations.ContainsKey("respawn") || operations.ContainsKey("item"))
		{
			return operations;
		}
		ItemStand component = ((Component)view).GetComponent<ItemStand>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return operations;
		}
		string value = DataHelper.Name(component.m_visualHash) + "," + component.m_visualVariant;
		Dictionary<string, object> dictionary = operations.ToDictionary<KeyValuePair<string, object>, string, object>((KeyValuePair<string, object> kvp) => kvp.Key, (KeyValuePair<string, object> kvp) => kvp.Value);
		dictionary["item"] = value;
		return dictionary;
	}

	protected override string DoOperation(ZNetView view, string operation, long? value)
	{
		throw new NotImplementedException();
	}

	public TweakItemStandCommand()
	{
		Component = typeof(ItemStand);
		ComponentName = "itemstand";
		SupportedOperations.Add("respawn", typeof(float));
		SupportedOperations.Add("name", typeof(string));
		SupportedOperations.Add("item", typeof(string));
		AutoComplete.Add("name", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("name=<color=yellow>text</color>", "Display name. Use _ as the space. No value to reset."));
		AutoComplete.Add("respawn", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("respawn=<color=yellow>minutes</color>", "Respawn time. No value to reset."));
		AutoComplete.Add("item", (int index) => index switch
		{
			0 => ParameterInfo.ItemIds, 
			1 => ParameterInfo.Create("item=id,<color=yellow>variant</color>", "Item variant (number)."), 
			_ => ParameterInfo.None, 
		});
		Init("tweak_itemstand", "Modify item stands");
	}
}
