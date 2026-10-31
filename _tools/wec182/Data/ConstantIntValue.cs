namespace Data;

public class ConstantIntValue : IIntValue
{
	private readonly int Value;

	public ConstantIntValue(int value)
	{
		Value = value;
		base._002Ector();
	}

	public int? Get(Functions f)
	{
		return Value;
	}

	public bool? Match(Functions f, int value)
	{
		return Value == value;
	}
}
