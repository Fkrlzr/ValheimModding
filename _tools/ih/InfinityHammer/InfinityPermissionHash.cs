namespace InfinityHammer;

public static class InfinityPermissionHash
{
	public const string Section = "infinityhammer";

	public static readonly int NoResourceCost = StringExtensionMethods.GetStableHashCode("no_resource_cost");

	public static readonly int NoDurabilityCost = StringExtensionMethods.GetStableHashCode("no_durability_cost");

	public static readonly int NoStaminaCost = StringExtensionMethods.GetStableHashCode("no_stamina_cost");

	public static readonly int GroupResourceCost = StringExtensionMethods.GetStableHashCode("group_resource_cost");

	public static readonly int IgnoreWards = StringExtensionMethods.GetStableHashCode("ignore_wards");

	public static readonly int IgnoreNoBuild = StringExtensionMethods.GetStableHashCode("ignore_no_build");

	public static readonly int AllowInDungeons = StringExtensionMethods.GetStableHashCode("allow_in_dungeons");

	public static readonly int IgnoreOtherRestrictions = StringExtensionMethods.GetStableHashCode("ignore_other_restrictions");

	public static readonly int RemoveAnything = StringExtensionMethods.GetStableHashCode("remove_anything");

	public static readonly int DisableLoot = StringExtensionMethods.GetStableHashCode("disable_loot");

	public static readonly int RepairAnything = StringExtensionMethods.GetStableHashCode("repair_anything");

	public static readonly int NoCreator = StringExtensionMethods.GetStableHashCode("no_creator");

	public static readonly int NoTarget = StringExtensionMethods.GetStableHashCode("no_target");

	public static readonly int NoPhysics = StringExtensionMethods.GetStableHashCode("no_physics");

	public static readonly int NoRemove = StringExtensionMethods.GetStableHashCode("no_remove");

	public static readonly int OverwriteHealth = StringExtensionMethods.GetStableHashCode("overwrite_health");

	public static readonly int Invulnerability = StringExtensionMethods.GetStableHashCode("invulnerability");

	public static readonly int Range = StringExtensionMethods.GetStableHashCode("range");

	public static readonly int ToolsEnabled = StringExtensionMethods.GetStableHashCode("tools_enabled");
}
