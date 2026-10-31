namespace InfinityHammer;

public static class Hashes
{
	public static readonly int XRaySteamName = StringExtensionMethods.GetStableHashCode("steamName");

	public static readonly int XRaySteamID = StringExtensionMethods.GetStableHashCode("steamID");

	public static readonly int XRayCreatedID = StringExtensionMethods.GetStableHashCode("xray_created");

	public static readonly int HasFields = StringExtensionMethods.GetStableHashCode("HasFields");

	public static readonly int HasFieldsStaticTarget = StringExtensionMethods.GetStableHashCode("HasFieldsStaticTarget");

	public static readonly int HasFieldsPiece = StringExtensionMethods.GetStableHashCode("HasFieldsPiece");

	public static readonly int PieceCanBeRemoved = StringExtensionMethods.GetStableHashCode("Piece.m_canBeRemoved");

	public static readonly int StaticTargetPrimaryTarget = StringExtensionMethods.GetStableHashCode("StaticTarget.m_primaryTarget");

	public static readonly int StaticTargetRandomTarget = StringExtensionMethods.GetStableHashCode("StaticTarget.m_randomTarget");

	public static readonly int HasFieldsStaticPhysics = StringExtensionMethods.GetStableHashCode("HasFieldsStaticPhysics");

	public static readonly int StaticPhysicsFall = StringExtensionMethods.GetStableHashCode("StaticPhysics.m_fall");

	public static readonly int StaticPhysicsPushUp = StringExtensionMethods.GetStableHashCode("StaticPhysics.m_pushUp");

	public static readonly int HashFieldsDestructible = StringExtensionMethods.GetStableHashCode("HasFieldsDestructible");

	public static readonly int HashFieldsMineRock = StringExtensionMethods.GetStableHashCode("HasFieldsMineRock");

	public static readonly int HashFieldsMineRock5 = StringExtensionMethods.GetStableHashCode("HasFieldsMineRock5");

	public static readonly int HashFieldsTreeBase = StringExtensionMethods.GetStableHashCode("HasFieldsTreeBase");

	public static readonly int HashFieldsTreeLog = StringExtensionMethods.GetStableHashCode("HasFieldsTreeLog");

	public static readonly int HashFieldsWearNTear = StringExtensionMethods.GetStableHashCode("HasFieldsWearNTear");

	public static readonly int ToolTierDestructible = StringExtensionMethods.GetStableHashCode("Destructible.m_minToolTier");

	public static readonly int ToolTierMineRock = StringExtensionMethods.GetStableHashCode("MineRock.m_minToolTier");

	public static readonly int ToolTierMineRock5 = StringExtensionMethods.GetStableHashCode("MineRock5.m_minToolTier");

	public static readonly int ToolTierTreeBase = StringExtensionMethods.GetStableHashCode("TreeBase.m_minToolTier");

	public static readonly int ToolTierTreeLog = StringExtensionMethods.GetStableHashCode("TreeLog.m_minToolTier");

	public static readonly int HealthMineRock = StringExtensionMethods.GetStableHashCode("MineRock.m_health");

	public static readonly int MaxHealth = StringExtensionMethods.GetStableHashCode("WearNTear.m_health");

	public static readonly int Growth = StringExtensionMethods.GetStableHashCode("Growth");

	public static readonly int Wear = StringExtensionMethods.GetStableHashCode("Wear");

	public static readonly int Fall = StringExtensionMethods.GetStableHashCode("Fall");

	public static readonly int Collision = StringExtensionMethods.GetStableHashCode("Collision");

	public static readonly int Render = StringExtensionMethods.GetStableHashCode("Render");

	public static readonly int Restrict = StringExtensionMethods.GetStableHashCode("Restrict");

	public static readonly int Interact = StringExtensionMethods.GetStableHashCode("Interact");

	public static readonly int Text = StringExtensionMethods.GetStableHashCode("Text");

	public static readonly int HasFieldsZSyncTransform = StringExtensionMethods.GetStableHashCode("HasFieldsZSyncTransform");

	public static readonly int ZSyncTransformSyncScale = StringExtensionMethods.GetStableHashCode("ZSyncTransform.m_syncScale");

	public static readonly int BuildingSkillLevel = StringExtensionMethods.GetStableHashCode("BuildingSkillLevel");

	public static readonly int ScaleBackupVecHash = StringExtensionMethods.GetStableHashCode("scaleBackup");

	public static readonly int ScaleBackupScalarHash = StringExtensionMethods.GetStableHashCode("scaleScalarBackup");
}
