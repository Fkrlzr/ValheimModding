using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class ScalingData
{
	private readonly bool OnlyPositiveHeight;

	private readonly bool MinXZ;

	private Vector3 Value;

	private Vector3 RawValue;

	private readonly bool PrintChanges;

	public Vector3 Vec3 => Value;

	public float X
	{
		get
		{
			if (!MinXZ)
			{
				return Value.x;
			}
			return Mathf.Max(0.25f, Value.x);
		}
	}

	public float Y => Value.y;

	public float Z
	{
		get
		{
			if (!MinXZ)
			{
				return Value.z;
			}
			return Mathf.Max(0.25f, Value.z);
		}
	}

	public ScalingData(bool sanityY, bool minXZ, bool printChanges, Vector3 value)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		OnlyPositiveHeight = sanityY;
		MinXZ = minXZ;
		Value = value;
		RawValue = value;
		PrintChanges = printChanges;
		base._002Ector();
	}

	public void Print(Terminal terminal)
	{
		if (PrintChanges && !Configuration.DisableScaleMessages)
		{
			if (X != Y || X != Z)
			{
				HammerHelper.Message(terminal, $"Scale set to X: {X:P0}, Z: {Z:P0}, Y: {Y:P0}.");
			}
			else
			{
				HammerHelper.Message(terminal, $"Scale set to {Y:P0}.");
			}
		}
	}

	public void SetPrecisionXZ(float min, float precision)
	{
		RawValue.x = min + precision * Mathf.Floor((RawValue.x - min) / precision);
		RawValue.z = min + precision * Mathf.Floor((RawValue.z - min) / precision);
		Value.x = RawValue.x;
		Value.z = RawValue.z;
		AfterScaling();
	}

	public void Zoom(float amount)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		TryCommit(RawValue, new Vector3(amount, amount, amount));
	}

	public void ZoomPercentage(float percentage)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		TryCommitPercentage(RawValue, new Vector3(1f + percentage, 1f + percentage, 1f + percentage));
	}

	public void ZoomX(float amount)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		TryCommit(RawValue, new Vector3(amount, 0f, 0f));
	}

	public void ZoomXPercentage(float percentage)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		TryCommitPercentage(RawValue, new Vector3(1f + percentage, 1f, 1f));
	}

	public void ZoomY(float amount)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		TryCommit(RawValue, new Vector3(0f, amount, 0f));
	}

	public void ZoomYPercentage(float percentage)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		TryCommitPercentage(RawValue, new Vector3(1f, 1f + percentage, 1f));
	}

	public void ZoomZ(float amount)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		TryCommit(RawValue, new Vector3(0f, 0f, amount));
	}

	public void ZoomZPercentage(float percentage)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		TryCommitPercentage(RawValue, new Vector3(1f, 1f, 1f + percentage));
	}

	private void TryCommit(Vector3 original, Vector3 change)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = original + change;
		if (IsValid(val))
		{
			RawValue = val;
			Value = val;
			AfterScaling();
		}
	}

	private void TryCommitPercentage(Vector3 original, Vector3 multiplier, int step = 0)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = default(Vector3);
		((Vector3)(ref val))._002Ector(original.x * multiplier.x, original.y * multiplier.y, original.z * multiplier.z);
		if ((!(val.x < RawValue.x) || !AtLimit(Value.x)) && (!(val.y < RawValue.y) || !AtLimit(Value.y)) && (!(val.z < RawValue.z) || !AtLimit(Value.z)) && IsValid(val))
		{
			RawValue = val;
			Vector3 val2 = SnapToPrecision(val);
			if (val2 == Value && step < 100)
			{
				TryCommitPercentage(RawValue, multiplier, step + 1);
				return;
			}
			Value = val2;
			AfterScaling();
		}
	}

	private bool IsValid(Vector3 value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if (value.x <= 0f || value.z <= 0f)
		{
			return false;
		}
		if (OnlyPositiveHeight)
		{
			return value.y > 0f;
		}
		return true;
	}

	private bool AtLimit(float value)
	{
		float scalePrecision = Configuration.ScalePrecision;
		if (scalePrecision <= 0f)
		{
			return false;
		}
		if (!OnlyPositiveHeight)
		{
			return Mathf.Abs(value) <= scalePrecision;
		}
		return value <= scalePrecision;
	}

	private static float SnapToPrecision(float value)
	{
		float scalePrecision = Configuration.ScalePrecision;
		if (scalePrecision <= 0f)
		{
			return value;
		}
		return Mathf.Sign(value) * Mathf.Max(scalePrecision, Mathf.Round(Mathf.Abs(value) / scalePrecision) * scalePrecision);
	}

	private static Vector3 SnapToPrecision(Vector3 value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(SnapToPrecision(value.x), SnapToPrecision(value.y), SnapToPrecision(value.z));
	}

	public void SetScale(float value)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		RawValue = value * Vector3.one;
		Value = RawValue;
		AfterScaling();
	}

	public void SetScaleX(float value)
	{
		RawValue.x = value;
		Value.x = value;
		AfterScaling();
	}

	public void SetScaleY(float value)
	{
		RawValue.y = value;
		Value.y = value;
		AfterScaling();
	}

	public void SetScaleZ(float value)
	{
		RawValue.z = value;
		Value.z = value;
		AfterScaling();
	}

	public void SetScale(Vector3 value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		RawValue = value;
		Value = RawValue;
		AfterScaling();
	}

	private void AfterScaling()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		Player player = Helper.GetPlayer();
		if (Object.op_Implicit((Object)(object)player.m_placementGhost))
		{
			player.m_placementGhost.transform.localScale = Value;
		}
	}
}
