using System;

namespace Common;

public static class Wildcard
{
	public static bool IsPattern(string value)
	{
		return value.IndexOf('*') >= 0;
	}

	public static bool Match(string value, string pattern, bool ignoreCase = false)
	{
		StringComparison comparisonType = (ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
		if (string.Equals(value, pattern, comparisonType))
		{
			return true;
		}
		if (pattern == "")
		{
			return false;
		}
		if (pattern == "*")
		{
			return true;
		}
		bool flag = pattern[0] == '*';
		bool flag2 = pattern[pattern.Length - 1] == '*';
		if (flag && flag2)
		{
			return value.IndexOf(pattern.Substring(1, pattern.Length - 2), comparisonType) >= 0;
		}
		if (flag)
		{
			return value.EndsWith(pattern.Substring(1), comparisonType);
		}
		if (flag2)
		{
			return value.StartsWith(pattern.Substring(0, pattern.Length - 1), comparisonType);
		}
		int num = pattern.IndexOf('*');
		if (num < 0)
		{
			return false;
		}
		if (value.Length >= pattern.Length - 1 && value.StartsWith(pattern.Substring(0, num), comparisonType))
		{
			return value.EndsWith(pattern.Substring(num + 1), comparisonType);
		}
		return false;
	}
}
