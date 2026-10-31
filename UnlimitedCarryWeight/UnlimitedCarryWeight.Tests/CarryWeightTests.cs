using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace UnlimitedCarryWeight.Tests;

public class BugConditionTests
{
	// Property 1: for the local player with the mod enabled and an inventory heavier than
	// the vanilla limit, the effective limit must cover the inventory (not Encumbered).
	[Property(MaxTest = 200)]
	public Property LocalPlayerEnabled_NeverEncumbered(PositiveInt vanillaSeed, PositiveInt extraSeed)
	{
		float vanilla = vanillaSeed.Get % 100001;
		float inventory = vanilla + 1 + (extraSeed.Get % 900000);
		float result = CarryWeightLogic.ApplyLimit(true, true, vanilla);
		return (!(inventory > result)).ToProperty()
			.Label($"vanilla={vanilla}, inventory={inventory}, result={result}");
	}

	[Theory]
	[InlineData(300f, 450f)]
	[InlineData(300f, 5000f)]
	public void DesignExamples_NotEncumbered(float vanilla, float inventory)
	{
		float result = CarryWeightLogic.ApplyLimit(true, true, vanilla);
		Assert.False(inventory > result, $"ApplyLimit(true, true, {vanilla}) returned {result} while inventory is {inventory}");
	}
}

public class PreservationTests
{
	[Property(MaxTest = 200)]
	public bool DisabledOrRemote_ReturnsVanilla(bool enabled, bool isLocal, NormalFloat vanilla)
	{
		if (enabled && isLocal)
		{
			return true; // bug-condition inputs are covered by BugConditionTests
		}
		float v = (float)System.Math.Min(System.Math.Abs(vanilla.Get), 1e9);
		return CarryWeightLogic.ApplyLimit(enabled, isLocal, v) == v;
	}

	[Property(MaxTest = 200)]
	public bool NeverBelowVanilla(bool enabled, bool isLocal, NormalFloat vanilla)
	{
		float v = (float)System.Math.Min(System.Math.Abs(vanilla.Get), 1e9);
		return CarryWeightLogic.ApplyLimit(enabled, isLocal, v) >= v;
	}

	[Fact]
	public void ObservedBaselineValues()
	{
		Assert.Equal(300f, CarryWeightLogic.ApplyLimit(false, true, 300f));
		Assert.Equal(300f, CarryWeightLogic.ApplyLimit(true, false, 300f));
		Assert.Equal(1234.5f, CarryWeightLogic.ApplyLimit(false, false, 1234.5f));
	}
}

public class ConfiguredLimitTests
{
	[Theory]
	[InlineData(300f, 5000f, 5000f)]
	[InlineData(6000f, 5000f, 6000f)]
	public void EnabledLocal_UsesConfiguredLimitButNeverLowersVanilla(float vanilla, float limit, float expected)
	{
		Assert.Equal(expected, CarryWeightLogic.ApplyLimit(true, true, vanilla, limit));
	}

	[Fact]
	public void DisabledOrRemote_IgnoresConfiguredLimit()
	{
		Assert.Equal(300f, CarryWeightLogic.ApplyLimit(false, true, 300f, 5000f));
		Assert.Equal(300f, CarryWeightLogic.ApplyLimit(true, false, 300f, 5000f));
	}
}
