using System.Globalization;
using UnityEngine;

namespace Common;

public static class Formatting
{
	public static string Format(float value)
	{
		return value.ToString("0.#####", NumberFormatInfo.InvariantInfo);
	}

	public static string Format(double value)
	{
		return value.ToString("0.#####", NumberFormatInfo.InvariantInfo);
	}

	public static string FormatPos(Vector3 value)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		return Format(value.x) + "," + Format(value.z) + "," + Format(value.y);
	}

	public static string FormatRot(Vector3 value)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		return Format(value.y) + "," + Format(value.x) + "," + Format(value.z);
	}
}
