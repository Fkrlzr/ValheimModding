using Common;

namespace Data;

public class ConstantHashValue : IHashValue
{
	private readonly int Value = StringExtensionMethods.GetStableHashCode(value);

	private readonly string? Pattern = (Wildcard.IsPattern(value) ? value : null);

	public ConstantHashValue(string value)
	{
	}

	public int? Get(Functions f)
	{
		return Value;
	}

	public bool? Match(Functions f, int value)
	{
		return (Pattern == null) ? (Value == value) : PrefabHelper.GetPrefabs(Pattern, "").Contains(value);
	}
}
