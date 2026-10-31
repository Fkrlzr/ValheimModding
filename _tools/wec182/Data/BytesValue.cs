using System;
using System.Linq;

namespace Data;

public class BytesValue : DynamicValue, IBytesValue
{
	public BytesValue(string[] values)
		: base(values)
	{
	}

	public byte[]? Get(Functions f)
	{
		string value = GetValue(f);
		if (value == null || value == "")
		{
			return null;
		}
		try
		{
			return Convert.FromBase64String(value);
		}
		catch (FormatException)
		{
			return null;
		}
	}

	public bool? Match(Functions f, byte[]? value)
	{
		bool flag = true;
		string[] values = Values;
		foreach (string str in values)
		{
			string text = f.Replace(str);
			if (text == null)
			{
				continue;
			}
			flag = false;
			if (text == "" && value == null)
			{
				return true;
			}
			if (text == "" || value == null)
			{
				continue;
			}
			try
			{
				if (Convert.FromBase64String(text).SequenceEqual(value))
				{
					return true;
				}
			}
			catch (FormatException)
			{
			}
		}
		if (!flag)
		{
			return false;
		}
		return null;
	}
}
