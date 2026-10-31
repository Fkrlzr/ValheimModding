using System;
using Common;
using UnityEngine;

namespace Data;

public class IntValue : DynamicValue, IIntValue
{
	public IntValue(string[] values)
		: base(values)
	{
	}

	public int? Get(Functions f)
	{
		string value = GetValue(f);
		if (value == null)
		{
			return null;
		}
		if (!value.Contains(";"))
		{
			return Parse.IntNull(value);
		}
		string[] array = value.Split(';');
		if (array.Length < 2)
		{
			throw new InvalidOperationException("Invalid range format: " + value);
		}
		int? num = Parse.IntNull(array[0]);
		int? num2 = Parse.IntNull(array[1]);
		if (!num.HasValue || !num2.HasValue)
		{
			return null;
		}
		int? result;
		if (array.Length < 3 || array[2] == "")
		{
			result = Random.Range(num.Value, num2.Value + 1);
		}
		else
		{
			int? num3 = Parse.IntNull(array[2]);
			if (num3.HasValue && num3 != 0)
			{
				int num4 = Random.Range(0, ((num2 - num) / num3).Value + 1);
				return num + num4 * num3;
			}
			result = Random.Range(num.Value, num2.Value + 1);
		}
		return result;
	}

	public bool? Match(Functions f, int value)
	{
		bool flag = true;
		string[] values = Values;
		foreach (string str in values)
		{
			string text = f.Replace(str);
			if (!text.Contains(";"))
			{
				int? num = Parse.IntNull(text);
				if (num.HasValue)
				{
					flag = false;
					if (num.Value == value)
					{
						return true;
					}
				}
				continue;
			}
			string[] array = text.Split(';');
			if (array.Length < 2)
			{
				throw new InvalidOperationException("Invalid range format: " + text);
			}
			int? num2 = Parse.IntNull(array[0]);
			int? num3 = Parse.IntNull(array[1]);
			if (!num2.HasValue || !num3.HasValue)
			{
				continue;
			}
			if (array.Length < 3)
			{
				flag = false;
				if (value >= num2.Value && value <= num3.Value)
				{
					return true;
				}
				continue;
			}
			int? num4 = Parse.IntNull(array[2]);
			if (!num4.HasValue)
			{
				continue;
			}
			flag = false;
			if (num4.Value == 0)
			{
				if (value >= num2.Value && value <= num3.Value)
				{
					return true;
				}
				continue;
			}
			int num5 = (num3.Value - num2.Value) / num4.Value;
			for (int j = 0; j <= num5; j++)
			{
				if (num2.Value + j * num4.Value == value)
				{
					return true;
				}
			}
		}
		if (!flag)
		{
			return false;
		}
		return null;
	}
}
