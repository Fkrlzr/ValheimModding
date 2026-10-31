using System;
using Common;
using UnityEngine;

namespace Data;

public class FloatValue : DynamicValue, IFloatValue
{
	public FloatValue(string[] values)
		: base(values)
	{
	}

	public float? Get(Functions f)
	{
		string value = GetValue(f);
		if (value == null)
		{
			return null;
		}
		if (!value.Contains(";"))
		{
			return Parse.FloatNull(value);
		}
		string[] array = value.Split(';');
		if (array.Length < 2)
		{
			throw new InvalidOperationException("Invalid range format: " + value);
		}
		float? num = Parse.FloatNull(array[0]);
		float? num2 = Parse.FloatNull(array[1]);
		if (!num.HasValue || !num2.HasValue)
		{
			return null;
		}
		float? result;
		if (array.Length < 3 || array[2] == "")
		{
			result = Random.Range(num.Value, num2.Value);
		}
		else
		{
			float? num3 = Parse.FloatNull(array[2]);
			if (num3.HasValue && num3 != 0f)
			{
				int num4 = (int)((num2.Value - num.Value) / num3.Value);
				int num5 = Random.Range(0, num4 + 1);
				return num + (float)num5 * num3;
			}
			result = Random.Range(num.Value, num2.Value);
		}
		return result;
	}

	public bool TryGet(Functions f, out float value)
	{
		float? num = Get(f);
		if (num.HasValue)
		{
			value = num.Value;
		}
		else
		{
			value = 0f;
		}
		return num.HasValue;
	}

	public bool? Match(Functions f, float value)
	{
		bool flag = true;
		string[] values = Values;
		foreach (string str in values)
		{
			string text = f.Replace(str);
			if (!text.Contains(";"))
			{
				float? num = Parse.FloatNull(text);
				if (num.HasValue)
				{
					flag = false;
					if (FloatCompare.Approx(num.Value, value))
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
			float? num2 = Parse.FloatNull(array[0]);
			float? num3 = Parse.FloatNull(array[1]);
			if (!num2.HasValue || !num3.HasValue)
			{
				continue;
			}
			if (array.Length < 3)
			{
				flag = false;
				if (FloatCompare.ApproxBetween(value, num2.Value, num3.Value))
				{
					return true;
				}
				continue;
			}
			float? num4 = Parse.FloatNull(array[2]);
			if (!num4.HasValue)
			{
				continue;
			}
			flag = false;
			if (num4.Value == 0f)
			{
				if (FloatCompare.ApproxBetween(value, num2.Value, num3.Value))
				{
					return true;
				}
				continue;
			}
			int num5 = (int)((num3.Value - num2.Value) / num4.Value);
			for (int j = 0; j <= num5; j++)
			{
				if (FloatCompare.Approx(num2.Value + (float)j * num4.Value, value))
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
