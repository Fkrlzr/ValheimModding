using System;

namespace Data;

public class BoolValue : DynamicValue, IBoolValue
{
	public BoolValue(string[] values)
		: base(values)
	{
	}

	private static bool IsTrue(string value)
	{
		return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
	}

	public int? GetInt(Functions f)
	{
		string value = GetValue(f);
		if (value == null)
		{
			return null;
		}
		return IsTrue(value) ? 1 : 0;
	}

	public bool? GetBool(Functions f)
	{
		string value = GetValue(f);
		if (value == null)
		{
			return null;
		}
		return IsTrue(value);
	}

	public bool? Match(Functions f, bool value)
	{
		bool flag = true;
		string[] values = Values;
		foreach (string str in values)
		{
			string text = f.Replace(str);
			if (text != null)
			{
				flag = false;
				if (IsTrue(text) == value)
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
