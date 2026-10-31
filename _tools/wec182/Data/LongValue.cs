using System;
using Common;
using UnityEngine;

namespace Data;

public class LongValue : DynamicValue, ILongValue
{
	public LongValue(string[] values)
		: base(values)
	{
	}

	public long? Get(Functions f)
	{
		string value = GetValue(f);
		if (value == null)
		{
			return null;
		}
		if (!value.Contains(";"))
		{
			return Parse.LongNull(value);
		}
		string[] array = value.Split(';');
		if (array.Length < 2)
		{
			throw new InvalidOperationException("Invalid range format: " + value);
		}
		long? num = Parse.LongNull(array[0]);
		long? num2 = Parse.LongNull(array[1]);
		if (!num.HasValue || !num2.HasValue)
		{
			return null;
		}
		long? result;
		if (array.Length < 3 || array[2] == "")
		{
			result = (long)(Random.value * (float)(num2.Value - num.Value) + (float)num.Value);
		}
		else
		{
			long? num3 = Parse.LongNull(array[2]);
			if (num3.HasValue && num3 != 0)
			{
				int num4 = (int)((num2.Value - num.Value) / num3.Value);
				int num5 = Random.Range(0, num4 + 1);
				return num + num5 * num3;
			}
			result = (long)(Random.value * (float)(num2.Value - num.Value) + (float)num.Value);
		}
		return result;
	}

	public bool? Match(Functions f, long value)
	{
		bool flag = true;
		string[] values = Values;
		foreach (string str in values)
		{
			string text = f.Replace(str);
			if (!text.Contains(";"))
			{
				long? num = Parse.LongNull(text);
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
			long? num2 = Parse.LongNull(array[0]);
			long? num3 = Parse.LongNull(array[1]);
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
			long? num4 = Parse.LongNull(array[2]);
			if (!num4.HasValue)
			{
				continue;
			}
			flag = false;
			if (num4.Value == 0L)
			{
				if (value >= num2.Value && value <= num3.Value)
				{
					return true;
				}
				continue;
			}
			long num5 = (num3.Value - num2.Value) / num4.Value;
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
