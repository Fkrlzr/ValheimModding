using System;
using System.Collections.Generic;
using System.Linq;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class TweakChestCommand : TweakCommand
{
	protected override string DoOperation(ZNetView view, string operation, string? value)
	{
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
		if (operation == "minamount")
		{
			return TweakActions.MinAmount(view, value);
		}
		if (operation == "maxamount")
		{
			return TweakActions.MaxAmount(view, value);
		}
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, string[] value)
	{
		if (operation == "item")
		{
			return TweakActions.Items(view, value);
		}
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, bool? value)
	{
		if (operation == "unlock")
		{
			return TweakActions.Unlock(view, value);
		}
		throw new NotImplementedException();
	}

	protected override Dictionary<string, object?> Postprocess(ZNetView view, Dictionary<string, object?> operations)
	{
		if (!operations.ContainsKey("respawn") || operations.ContainsKey("item"))
		{
			return operations;
		}
		Container component = ((Component)view).GetComponent<Container>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return operations;
		}
		string[] array = (from item in component.GetInventory().GetAllItems()
			select $"{((Object)item.m_dropPrefab).name},1,{item.m_stack}").ToArray();
		Dictionary<string, object> dictionary = operations.ToDictionary<KeyValuePair<string, object>, string, object>((KeyValuePair<string, object> kvp) => kvp.Key, (KeyValuePair<string, object> kvp) => kvp.Value);
		dictionary["item"] = array;
		if (!dictionary.ContainsKey("minamount"))
		{
			dictionary["minamount"] = array.Count();
		}
		if (!dictionary.ContainsKey("maxamount"))
		{
			dictionary["maxamount"] = array.Count();
		}
		return dictionary;
	}

	protected override string DoOperation(ZNetView view, string operation, long? value)
	{
		throw new NotImplementedException();
	}

	public TweakChestCommand()
	{
		Component = typeof(Container);
		ComponentName = "chest";
		SupportedOperations.Add("name", typeof(string));
		if (WorldEditCommands.IsStructureTweaks)
		{
			SupportedOperations.Add("unlock", typeof(bool));
		}
		if (WorldEditCommands.IsSpawnerTweaks)
		{
			SupportedOperations.Add("respawn", typeof(float));
			SupportedOperations.Add("minamount", typeof(int));
			SupportedOperations.Add("maxamount", typeof(int));
			SupportedOperations.Add("item", typeof(string[]));
		}
		AutoComplete.Add("name", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("name=<color=yellow>text</color>", "Display name. Use _ as the space. No value to reset."));
		if (WorldEditCommands.IsStructureTweaks)
		{
			AutoComplete.Add("unlock", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("unlock=<color=yellow>true/false</color>", "Ignores wards. No value to toggle."));
		}
		if (WorldEditCommands.IsSpawnerTweaks)
		{
			AutoComplete.Add("respawn", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("respawn=<color=yellow>minutes</color>", "Respawn time. No value to reset."));
			AutoComplete.Add("minamount", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("minamount=<color=yellow>number</color>", "Minimum amount of items. No value to reset."));
			AutoComplete.Add("maxamount", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("maxamount=<color=yellow>number</color>", "Maximum amount of items. No value to reset."));
			AutoComplete.Add("item", (int index) => index switch
			{
				0 => ParameterInfo.ItemIds, 
				1 => ParameterInfo.Create("item=id,<color=yellow>weight</color>,minamount,maxamount", "Chance relative to other items."), 
				2 => ParameterInfo.Create("item=id,weight,<color=yellow>minamount</color>,maxamount", "Minimum amount."), 
				3 => ParameterInfo.Create("item=id,weight,minamount,<color=yellow>maxamount</color>", "Maximum amount."), 
				_ => ParameterInfo.Create("For additional entries, add more <color>item=...</color> parameters."), 
			});
		}
		Init("tweak_chest", "Modify chests");
	}
}
