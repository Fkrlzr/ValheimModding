using Common;

namespace Data;

public class ConstantStringValue : IStringValue
{
	private readonly string Value;

	private readonly bool IsPattern;

	public ConstantStringValue(string value)
	{
		Value = value;
		IsPattern = Wildcard.IsPattern(value);
		base._002Ector();
	}

	public string? Get(Functions f)
	{
		return Value;
	}

	public string? GetWhole(Functions f)
	{
		return Value;
	}

	public bool? Match(Functions f, string value)
	{
		return IsPattern ? Wildcard.Match(value, Value) : (Value == value);
	}
}
