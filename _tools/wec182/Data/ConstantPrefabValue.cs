namespace Data;

public class ConstantPrefabValue : IPrefabValue
{
	private readonly int? Value;

	public ConstantPrefabValue(int? value)
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
