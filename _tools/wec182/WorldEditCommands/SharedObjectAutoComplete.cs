using System;
using System.Collections.Generic;
using System.Linq;
using ServerDevcommands;

namespace WorldEditCommands;

public class SharedObjectAutoComplete
{
	public static List<string> VisualAutoComplete(string name, int index)
	{
		return index switch
		{
			0 => ParameterInfo.ItemIds, 
			1 => ParameterInfo.Create("$" + name + "=id,<color=yellow>integer</color> | Item variant for items that have multiple variants."), 
			_ => ParameterInfo.None, 
		};
	}

	public static List<string> WithSharedParameters(List<string> parameters)
	{
		List<string> collection = new List<string>(24)
		{
			"baby", "durability", "tame", "left_hand", "right_hand", "helmet", "chest", "shoulders", "legs", "utility",
			"radius", "health", "stars", "level", "scale", "model", "damage", "ammo", "ammoType", "field",
			"f", "par", "persist", "distant"
		};
		parameters.AddRange(collection);
		List<string> list = new List<string>();
		list.AddRange(from s in parameters.Distinct()
			orderby s
			select s);
		return list;
	}

	public static Dictionary<string, Func<int, List<string>>> WithSharedFetchers(Dictionary<string, Func<int, List<string>>> fetchers)
	{
		Dictionary<string, Func<int, List<string>>> dictionary = new Dictionary<string, Func<int, List<string>>>
		{
			{
				"tame",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("tame=<color=yellow>true/false</color> or no value for default.", "Sets is the creature tamed.")
			},
			{
				"baby",
				(int index) => ParameterInfo.Flag("Baby")
			},
			{
				"persist",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("persist=<color=yellow>true/false</color>.", "Sets if the object is saved to the world file.")
			},
			{
				"distant",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("distant=<color=yellow>true/false</color> or no value for default.", "Sets if the object is loaded from further distance than normally.")
			},
			{
				"left_hand",
				(int index) => VisualAutoComplete("left_hand", index)
			},
			{
				"right_hand",
				(int index) => VisualAutoComplete("right_hand", index)
			},
			{
				"helmet",
				(int index) => VisualAutoComplete("helmet", index)
			},
			{
				"chest",
				(int index) => VisualAutoComplete("chest", index)
			},
			{
				"shoulders",
				(int index) => VisualAutoComplete("shoulders", index)
			},
			{
				"legs",
				(int index) => VisualAutoComplete("legs", index)
			},
			{
				"utility",
				(int index) => VisualAutoComplete("utility", index)
			},
			{
				"durability",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("durability", "number", "Sets current durability/health (+ maximum health for creatures).")
			},
			{
				"health",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.CreateWithMinMax("health", "number", "Sets current durability/health (+ maximum health for creatures).")
			},
			{
				"damage",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.CreateWithMinMax("damage", "number", "Sets the damage multiplier.")
			},
			{
				"ammo",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.CreateWithMinMax("ammo", "number", "Sets the amount of ammo.")
			},
			{
				"ammoType",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.ItemIds
			},
			{
				"stars",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("stars", "integer", "Sets creature stars.")
			},
			{
				"level",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("level", "integer", "Sets creature and item level.")
			},
			{
				"model",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("model", "integer", "Sets the creature model (0 for male, 1 for female).")
			},
			{
				"scale",
				(int index) => ParameterInfo.Scale("scale", "Scaling for objects that support it", index)
			},
			{
				"field",
				(int index) => index switch
				{
					1 => FieldAutoComplete.GetFields(), 
					0 => FieldAutoComplete.GetComponents(), 
					_ => FieldAutoComplete.GetTypes(index - 2), 
				}
			},
			{
				"f",
				(int index) => index switch
				{
					1 => FieldAutoComplete.GetFields(), 
					0 => FieldAutoComplete.GetComponents(), 
					_ => FieldAutoComplete.GetTypes(index - 2), 
				}
			},
			{
				"par",
				(int index) => (index != 0) ? ParameterInfo.Create("par=key,<color=yellow>value</color>", "Value of the parameter.") : DataAutoComplete.GetDataParameters()
			}
		};
		foreach (KeyValuePair<string, Func<int, List<string>>> fetcher in fetchers)
		{
			dictionary[fetcher.Key] = fetcher.Value;
		}
		return dictionary;
	}
}
