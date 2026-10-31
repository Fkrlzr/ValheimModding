using UnityEngine;

namespace Data;

public class ConstantVector3Value : IVector3Value
{
	private readonly Vector3 Value;

	public ConstantVector3Value(Vector3 value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Value = value;
		base._002Ector();
	}

	public Vector3? Get(Functions f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return Value;
	}

	public bool? Match(Functions f, Vector3 value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return Value == value;
	}
}
