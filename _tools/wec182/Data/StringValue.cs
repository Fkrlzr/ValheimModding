using System.Linq;
using Common;

namespace Data;

public class StringValue : DynamicValue, IStringValue
{
	private readonly bool IsPattern = values.Any(Wildcard.IsPattern);

	public StringValue(string[] values)
		: base(values)
	{
	}

	public string? Get(Functions f)
	{
		return GetValue(f);
	}

	public string? GetWhole(Functions f)
	{
		return GetWholeValue(f);
	}

	public bool? Match(Functions f, string value)
	{
		bool flag = false;
		string[] values = Values;
		foreach (string str in values)
		{
			string text = f.Replace(str);
			if (!(text == "") && !(text == "<none>"))
			{
				flag = true;
				if (IsPattern ? Wildcard.Match(value, text) : (text == value))
				{
					return true;
				}
			}
		}
		if (!flag)
		{
			return null;
		}
		return false;
	}
}
