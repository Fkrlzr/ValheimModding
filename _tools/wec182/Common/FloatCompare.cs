using UnityEngine;

namespace Common;

public static class FloatCompare
{
	public static bool Approx(float a, float b)
	{
		return Mathf.Abs(a - b) < 0.001f;
	}

	public static bool ApproxBetween(float a, float min, float max)
	{
		if (min - 0.001f <= a)
		{
			return a <= max + 0.001f;
		}
		return false;
	}
}
