using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using ServerDevcommands;
using TMPro;

namespace WorldEditCommands;

public class DataAutoComplete
{
	public List<string> NamedParameters = new List<string>();

	public static List<string> GetOptions()
	{
		List<string> list = new List<string>();
		list.Add("id");
		list.Add("ignore");
		list.Add("radius");
		list.Add("center");
		list.Add("from");
		list.Add("rect");
		list.Add("angle");
		list.Add("chance");
		list.Add("type");
		list.Add("connect");
		list.Add("match");
		list.Add("unmatch");
		list.Add("par");
		list.AddRange(DataParameters.SupportedOperations.Keys);
		List<string> source = list;
		List<string> list2 = new List<string>();
		list2.AddRange(from s in source.Distinct()
			orderby s
			select s);
		return list2;
	}

	public static Dictionary<string, Func<int, List<string>>> GetNamedOptions()
	{
		List<string> dataTypes = new List<string>(9) { "array", "bool", "float", "hash", "int", "long", "quat", "string", "vec3" };
		return new Dictionary<string, Func<int, List<string>>>
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
			},
			{
				"save",
				(int index) => ParameterInfo.Create("save=<color=yellow>name</color>", "Saves the object data to the file.")
			},
			{
				"dump",
				(int index) => ParameterInfo.Create("dump=<color=yellow>name</color>", "Saves the object data with default fields to the file.")
			},
			{
				"keep",
				(int index) => ParameterInfo.Create("keep=<color=yellow>key,key,key,...</color>", "Removes all data except given keys.")
			},
			{
				"clear",
				(int index) => ParameterInfo.Flag("Removes all data.")
			},
			{
				"print",
				(int index) => ParameterInfo.Flag("Prints data.")
			},
			{
				"remove",
				(int index) => ParameterInfo.Create("remove=<color=yellow>key,key,key,...</color>", "Removes given keys.")
			},
			{
				"set",
				(int index) => index switch
				{
					2 => ParameterInfo.Create("set=type,key,<color=yellow>value</color>", "Value of the key."), 
					1 => ParameterInfo.Create("set=type,<color=yellow>key</color>,value", "Name of the key."), 
					0 => dataTypes, 
					_ => ParameterInfo.None, 
				}
			},
			{
				"load",
				(int index) => DataLoading.DataKeys
			},
			{
				"merge",
				(int index) => DataLoading.DataKeys
			},
			{
				"match",
				(int index) => (index != 0) ? ParameterInfo.None : DataLoading.DataKeys
			},
			{
				"unmatch",
				(int index) => (index != 0) ? ParameterInfo.None : DataLoading.DataKeys
			},
			{
				"par",
				(int index) => (index != 0) ? ParameterInfo.Create("par=key,<color=yellow>value</color>", "Value of the parameter.") : GetDataParameters()
			},
			{
				"copy_raw",
				(int index) => (index == 0) ? ParameterInfo.Create("copy_raw or copy_raw=<color=yellow>all</color> or copy_raw=<color=yellow>key1,key2,...</color>", "Prints and copies object data to clipboard. By default excludes some data for better results.") : ParameterInfo.None
			}
		};
	}

	public static List<string> GetDataParameters()
	{
		HashSet<string> hashSet = DataFromCommand(GetInput());
		if (hashSet.Count == 0)
		{
			return ParameterInfo.Create("par=<color=yellow>key</color>,value", "Name of the parameter.");
		}
		HashSet<string> hashSet2 = hashSet;
		List<string> list = new List<string>(hashSet2.Count);
		list.AddRange(hashSet2);
		return list;
	}

	public static List<string> GetRawDataParameters()
	{
		string[] array = GetInput().Split(' ');
		if (array.Length < 2)
		{
			return ParameterInfo.Create("par=<color=yellow>key</color>,value", "Name of the parameter.");
		}
		if (DataLoading.Data.TryGetValue(ZDOKeys.Hash(array[1]), out DataEntry value))
		{
			HashSet<string> hashSet = RequiredParameters.Get(value);
			List<string> list = new List<string>(hashSet.Count);
			list.AddRange(hashSet);
			return list;
		}
		return ParameterInfo.Create("par=<color=yellow>key</color>,value", "Name of the parameter.");
	}

	private static HashSet<string> DataFromCommand(string command)
	{
		string[] array = (from s in command.Split(' ')
			where s.StartsWith("data=", StringComparison.Ordinal) || s.StartsWith("merge=", StringComparison.Ordinal) || s.StartsWith("load=", StringComparison.Ordinal)
			select s).SelectMany((string s) => Parse.Split(s.Split('=')[1], ',')).ToArray();
		HashSet<string> hashSet = new HashSet<string>();
		string[] array2 = array;
		foreach (string key in array2)
		{
			if (!DataLoading.Data.TryGetValue(ZDOKeys.Hash(key), out DataEntry value))
			{
				continue;
			}
			foreach (string item in RequiredParameters.Get(value))
			{
				hashSet.Add(item);
			}
		}
		return hashSet;
	}

	private static string GetInput()
	{
		Aliasing.RestoreAlias(((Terminal)Console.m_instance).m_input);
		string result = Aliasing.Plain(((TMP_InputField)((Terminal)Console.m_instance).m_input).text);
		Aliasing.RemoveAlias(((Terminal)Console.m_instance).m_input);
		return result;
	}
}
