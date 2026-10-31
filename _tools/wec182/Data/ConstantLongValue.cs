namespace Data;

public class ConstantLongValue : ILongValue
{
	private readonly long Value;

	public ConstantLongValue(long value)
	{
		Value = value;
		base._002Ector();
	}

	public long? Get(Functions f)
	{
		return Value;
	}

	public bool? Match(Functions f, long value)
	{
		return Value == value;
	}
}
