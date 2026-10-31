using System;

namespace UnlimitedCarryWeight;

// Pure decision logic, free of Unity and game types so it can be unit tested.
public static class CarryWeightLogic
{
	// Bounded (not float.MaxValue) so the inventory UI still renders sensibly.
	public const float UnlimitedLimit = 999999f;

	public static float ApplyLimit(bool enabled, bool isLocalPlayer, float vanilla, float limit = UnlimitedLimit)
	{
		return enabled && isLocalPlayer ? Math.Max(vanilla, limit) : vanilla;
	}
}
