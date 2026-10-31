using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Data;

public static class RequiredParameters
{
	public static HashSet<string> Get(DataEntry data)
	{
		HashSet<string> result = new HashSet<string>();
		IDictionary[] array = new IDictionary[9] { data.Strings, data.Floats, data.Ints, data.Bools, data.Hashes, data.Longs, data.Vecs, data.Quats, data.ByteArrays };
		foreach (IDictionary dictionary in array)
		{
			if (dictionary == null)
			{
				continue;
			}
			foreach (object value in dictionary.Values)
			{
				Collect(value, result);
			}
		}
		Collect(data.Persistent, result);
		Collect(data.Distant, result);
		Collect(data.Position, result);
		Collect(data.Rotation, result);
		Collect(data.ItemAmount, result);
		Collect(data.Item, result);
		if (data.Items != null)
		{
			foreach (ItemValue item in data.Items)
			{
				Collect(item, result);
			}
		}
		return result;
	}

	private static void Collect(object? value, HashSet<string> result)
	{
		if (value is DynamicValue dynamicValue)
		{
			{
				foreach (string rawValue in dynamicValue.RawValues)
				{
					Extract(rawValue, result);
				}
				return;
			}
		}
		if (!(value is ItemValue))
		{
			return;
		}
		FieldInfo[] fields = typeof(ItemValue).GetFields(BindingFlags.Instance | BindingFlags.Public);
		for (int i = 0; i < fields.Length; i++)
		{
			object value2 = fields[i].GetValue(value);
			if (value2 is IDictionary dictionary)
			{
				foreach (object value3 in dictionary.Values)
				{
					Collect(value3, result);
				}
			}
			else
			{
				Collect(value2, result);
			}
		}
	}

	private static void Extract(string value, HashSet<string> result)
	{
		string[] array = value.Split('<', '>');
		for (int i = 1; i < array.Length; i += 2)
		{
			string text = array[i].Split('=')[0];
			if (text != "")
			{
				result.Add(text);
			}
		}
	}
}
