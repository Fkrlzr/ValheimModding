namespace Data;

public class ConstantBoolValue : IBoolValue
{
	private readonly bool Value;

	public ConstantBoolValue(bool value)
	{
		Value = value;
		base._002Ector();
	}

	public int? GetInt(Functions f)
	{
		return Value ? 1 : 0;
	}

	public bool? GetBool(Functions f)
	{
		return Value;
	}

	public bool? Match(Functions f, bool value)
	{
		return Value == value;
	}
}
