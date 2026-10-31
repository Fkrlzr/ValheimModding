namespace ServerDevcommands;

public static class PermissionHash
{
	public const string Section = "serverdevcommands";

	public static readonly int MapCoordinates = StringExtensionMethods.GetStableHashCode("mapcoordinates");

	public static readonly int MiniMapCoordinates = StringExtensionMethods.GetStableHashCode("minimapcoordinates");

	public static readonly int AlternativeBiomes = StringExtensionMethods.GetStableHashCode("alternativebiomes");

	public static readonly int ShowPrivatePlayers = StringExtensionMethods.GetStableHashCode("showprivateplayers");

	public static readonly int Ghost = StringExtensionMethods.GetStableHashCode("ghost");

	public static readonly int God = StringExtensionMethods.GetStableHashCode("god");

	public static readonly int Fly = StringExtensionMethods.GetStableHashCode("fly");

	public static readonly int IgnoreWards = StringExtensionMethods.GetStableHashCode("ignorewards");

	public static readonly int NoClipCamera = StringExtensionMethods.GetStableHashCode("noclipcamera");

	public static readonly int NoCost = StringExtensionMethods.GetStableHashCode("nocost");

	public static readonly int IgnoreNoMap = StringExtensionMethods.GetStableHashCode("ignorenomap");

	public static readonly int DisableStartShout = StringExtensionMethods.GetStableHashCode("disablestartshout");

	public static readonly int NoDrops = StringExtensionMethods.GetStableHashCode("nodrops");

	public static readonly int HideShoutPings = StringExtensionMethods.GetStableHashCode("hideshoutpings");
}
