namespace Data;

public class ConstantZdoIdValue : IZdoIdValue
{
	private readonly ZDOID Value;

	public ConstantZdoIdValue(ZDOID value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Value = value;
		base._002Ector();
	}

	public ZDOID? Get(Functions f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return Value;
	}

	public bool? Match(Functions f, ZDOID value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return Value == value;
	}
}
