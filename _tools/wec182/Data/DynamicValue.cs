using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data;

public class DynamicValue
{
	protected readonly string[] Values;

	public IReadOnlyList<string> RawValues => Values;

	public DynamicValue(string[] values)
	{
		Values = values;
		base._002Ector();
	}

	private string? RollValue()
	{
		if (Values.Length == 1)
		{
			return Values[0];
		}
		return Values[Random.Range(0, Values.Length)];
	}

	protected string? GetValue(Functions f)
	{
		string text = RollValue();
		if (text == null || text == "<none>")
		{
			return null;
		}
		return f.Replace(text);
	}

	protected string? GetValue()
	{
		string text = RollValue();
		if (text != null && !(text == "<none>"))
		{
			return text;
		}
		return null;
	}

	protected List<string> GetAllValues(Functions f)
	{
		List<string> list = new List<string>();
		list.AddRange(from v in Values.Select(f.Replace)
			where v != null && v != "" && v != "<none>"
			select v);
		return list;
	}

	protected string GetWholeValue(Functions f)
	{
		return string.Join(",", Values.Select(f.Replace));
	}
}
