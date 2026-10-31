using System.Collections.Generic;
using System.Linq;
using Common;

namespace Data;

public class HashValue : DynamicValue, IHashValue
{
	public HashValue(string[] values)
		: base(values)
	{
	}

	public int? Get(Functions f)
	{
		string value = GetValue(f);
		if (value == null || value == "")
		{
			return null;
		}
		return StringExtensionMethods.GetStableHashCode(value);
	}

	public bool? Match(Functions f, int value)
	{
		List<string> allValues = GetAllValues(f);
		if (allValues.Count == 0)
		{
			return null;
		}
		return allValues.Any((string v) => (!Wildcard.IsPattern(v)) ? (StringExtensionMethods.GetStableHashCode(v) == value) : PrefabHelper.GetPrefabs(v, "").Contains(value));
	}
}
