using System;
using System.Collections.Generic;
using System.Linq;
using ServerDevcommands;

namespace WorldEditCommands;

public class TweakAutoComplete
{
	public List<string> NamedParameters = new List<string>();

	public static List<string> WithFilters(List<string> parameters)
	{
		List<string> collection = new List<string>(11)
		{
			"id", "ignore", "radius", "center", "from", "rect", "angle", "chance", "type", "connect",
			"force"
		};
		parameters.AddRange(collection);
		List<string> list = new List<string>();
		list.AddRange(from s in parameters.Distinct()
			orderby s
			select s);
		return list;
	}

	public static Dictionary<string, Func<int, List<string>>> WithFilters(Dictionary<string, Func<int, List<string>>> fetchers)
	{
		Dictionary<string, Func<int, List<string>>> dictionary = new Dictionary<string, Func<int, List<string>>>
		{
			{
				"type",
				(int index) => ParameterInfo.Components
			},
			{
				"connect",
				(int index) => ParameterInfo.Flag("Connect")
			},
			{
				"force",
				(int index) => ParameterInfo.Flag("Force", "Sets the component if missing.")
			},
			{
				"id",
				(int index) => ParameterInfo.Ids
			},
			{
				"ignore",
				(int index) => ParameterInfo.Ids
			},
			{
				"center",
				(int index) => ParameterInfo.XZY("center", "Overrides the player position. For <color=yellow>rotate</color> sets also the rotation center point.", index)
			},
			{
				"from",
				(int index) => ParameterInfo.XZY("center", "Overrides the player position. For <color=yellow>rotate</color> sets also the rotation center point.", index)
			},
			{
				"rect",
				(int index) => index switch
				{
					0 => ParameterInfo.Create("rect=<color=yellow>size</color> or rect=<color=yellow>width</color>,depth", "Area of affected objects."), 
					1 => ParameterInfo.Create("rect=width,<color=yellow>depth</color>", "Area of affected objects."), 
					_ => ParameterInfo.None, 
				}
			},
			{
				"angle",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("angle=<color=yellow>degrees</color>", "Direction of the rectangle when used with <color=yellow>rect</color>.")
			},
			{
				"circle",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("circle=<color=yellow>number</color>", "Radius of affected objects.")
			},
			{
				"radius",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("radius=<color=yellow>number</color>", "Radius of affected objects.")
			},
			{
				"chance",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("chance=<color=yellow>number</color>", "Chance to affect the object (from 0.0 to 1.0).")
			}
		};
		foreach (KeyValuePair<string, Func<int, List<string>>> fetcher in fetchers)
		{
			dictionary[fetcher.Key] = fetcher.Value;
		}
		return dictionary;
	}

	public static List<string> Effect(string name, int index)
	{
		return index switch
		{
			0 => ParameterInfo.Ids, 
			1 => ParameterInfo.Create(name + "=id,<color=yellow>flag</color>", "1 = random rotation"), 
			_ => ParameterInfo.Create("For additional entries, add more <color>" + name + "=...</color> parameters."), 
		};
	}

	public static List<string> EffectFull(string name, int index)
	{
		return index switch
		{
			0 => ParameterInfo.Ids, 
			1 => ParameterInfo.Create(name + "=id,<color=yellow>flag</color>,variant,childTransform", "Sum up: 1 = random rotation, 2 = inherit rotation, 4 = allow scaling, 8 = inherit scale, 16 = attach to the provided object."), 
			2 => ParameterInfo.Create(name + "t=id,flag,<color=yellow>variant</color>,childTransform", "Variant number (very rarely needed)."), 
			3 => ParameterInfo.Create(name + "=id,flag,variant,<color=yellow>childTransform</color>", "Name of the transformation to attach."), 
			_ => ParameterInfo.Create("For additional entries, add more <color>" + name + "=...</color> parameters."), 
		};
	}
}
