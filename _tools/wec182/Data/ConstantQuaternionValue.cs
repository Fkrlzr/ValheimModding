using UnityEngine;

namespace Data;

public class ConstantQuaternionValue : IQuaternionValue
{
	private readonly Quaternion Value;

	public ConstantQuaternionValue(Quaternion value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Value = value;
		base._002Ector();
	}

	public Quaternion? Get(Functions f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return Value;
	}

	public bool? Match(Functions f, Quaternion value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return Value == value;
	}
}
