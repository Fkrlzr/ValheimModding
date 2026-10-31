using Common;

namespace Data;

public class ConstantRangeIntValue : IRangeIntValue
{
	private readonly ValueRange<int> Value;

	public ConstantRangeIntValue(ValueRange<int> value)
	{
		Value = value;
		base._002Ector();
	}

	public ValueRange<int>? Get(Functions f)
	{
		return Value;
	}
}
