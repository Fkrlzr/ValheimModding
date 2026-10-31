namespace Data;

public class ConstantFloatValue : IFloatValue
{
	private readonly float Value;

	public ConstantFloatValue(float value)
	{
		Value = value;
		base._002Ector();
	}

	public float? Get(Functions f)
	{
		return Value;
	}

	public bool TryGet(Functions f, out float value)
	{
		value = Value;
		return true;
	}

	public bool? Match(Functions f, float value)
	{
		return Value == value;
	}
}
