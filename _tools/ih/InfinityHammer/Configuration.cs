using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using ServerDevcommands;
using Service;
using UnityEngine;

namespace InfinityHammer;

public class Configuration
{
	public static ConfigEntry<bool> configNoResourceCost;

	public static ConfigEntry<bool> configNoDurabilityCost;

	public static ConfigEntry<bool> configNoStaminaCost;

	public static ConfigEntry<bool> configGroupResourceCost;

	public static ConfigEntry<bool> configIgnoreWards;

	public static ConfigEntry<bool> configIgnoreNoBuild;

	public static ConfigEntry<bool> configAllowInDungeons;

	public static ConfigEntry<bool> configIgnoreOtherRestrictions;

	public static ConfigEntry<bool> configRemoveAnything;

	public static ConfigEntry<bool> configDisableMessages;

	public static ConfigEntry<bool> configDisableSelectMessages;

	public static ConfigEntry<bool> configDisableOffsetMessages;

	public static ConfigEntry<bool> configDisableScaleMessages;

	public static ConfigEntry<bool> configDisableLoot;

	public static ConfigEntry<bool> configRemoveContents;

	public static ConfigEntry<bool> configRepairAnything;

	public static ConfigEntry<bool> configNoCreator;

	public static ConfigEntry<bool> configNoTarget;

	public static ConfigEntry<bool> configNoPhysics;

	public static ConfigEntry<bool> configNoRemove;

	public static ConfigEntry<string> configZoopMagic;

	public static ConfigEntry<bool> configResetOffsetOnUnfreeze;

	public static ConfigEntry<bool> configUnfreezeOnUnequip;

	public static ConfigEntry<bool> configUnfreezeOnSelect;

	public static ConfigEntry<string> configOverwriteHealth;

	public static ConfigEntry<string> configInvulnerability;

	public static ConfigEntry<string> configSnapping;

	public static ConfigEntry<bool> configPreserveWear;

	public static ConfigEntry<string> configRemoveArea;

	public static ConfigEntry<string> configRange;

	public static ConfigEntry<bool> configPlaceEmptyRooms;

	public static ConfigEntry<bool> configScaleZSyncObjects;

	public static ConfigEntry<string> configScalePrecision;

	public static ConfigWrapper Wrapper;

	public static ConfigEntry<KeyboardShortcut> buildScale;

	public static ConfigEntry<KeyboardShortcut> zoopHorizontal;

	public static ConfigEntry<KeyboardShortcut> zoopVertical;

	public static ConfigEntry<KeyboardShortcut> zoopForward;

	public static ConfigEntry<KeyboardShortcut> moveUp;

	public static ConfigEntry<KeyboardShortcut> moveUpLarge;

	public static ConfigEntry<KeyboardShortcut> moveDown;

	public static ConfigEntry<KeyboardShortcut> moveDownLarge;

	public static ConfigEntry<KeyboardShortcut> moveLeft;

	public static ConfigEntry<KeyboardShortcut> moveLeftLarge;

	public static ConfigEntry<KeyboardShortcut> moveRight;

	public static ConfigEntry<KeyboardShortcut> moveRightLarge;

	public static ConfigEntry<KeyboardShortcut> moveForward;

	public static ConfigEntry<KeyboardShortcut> moveForwardLarge;

	public static ConfigEntry<KeyboardShortcut> moveBackward;

	public static ConfigEntry<KeyboardShortcut> moveBackwardLarge;

	public static ConfigEntry<KeyboardShortcut> freeze;

	public static ConfigEntry<KeyboardShortcut> select;

	public static ConfigEntry<KeyboardShortcut> selectFrozen;

	public static ConfigEntry<KeyboardShortcut> pick;

	public static ConfigEntry<KeyboardShortcut> pickFrozen;

	public static ConfigEntry<KeyboardShortcut> selectAll;

	public static ConfigEntry<KeyboardShortcut> selectAllFrozen;

	public static ConfigEntry<KeyboardShortcut> pickAll;

	public static ConfigEntry<KeyboardShortcut> pickAllFrozen;

	public static ConfigEntry<KeyboardShortcut> resetOffset;

	public static ConfigEntry<KeyboardShortcut> undo;

	public static ConfigEntry<KeyboardShortcut> redo;

	public static ConfigEntry<KeyboardShortcut> grid;

	public static ConfigEntry<KeyboardShortcut> gridHold;

	public static ConfigEntry<string> moveAmount;

	public static ConfigEntry<string> moveAmountLarge;

	public static ConfigEntry<string> gridPrecision;

	public static ConfigEntry<bool> configSaveBlueprintData;

	public static ConfigEntry<string> configBlueprintCenterPiece;

	public static ConfigEntry<string> configBlueprintSnapPiece;

	public static ConfigEntry<bool> configSaveSimplerBlueprints;

	public static ConfigEntry<bool> configUseBlueprintChance;

	public static ConfigEntry<bool> configIncludeTerrainHeight;

	public static ConfigEntry<bool> configIncludeTerrainPaint;

	public static ConfigEntry<string> configBlueprintTerrainHeightOffset;

	public static ConfigEntry<string> configBlueprintTerrainPaintOffset;

	public static ConfigEntry<string> configBlueprintTerrainHeightSmooth;

	public static ConfigEntry<string> configBlueprintTerrainPaintSmooth;

	public static ConfigEntry<string> configBlueprintTerrainNodeSpacing;

	public static ConfigEntry<string> configBlueprintPaintNodeSpacing;

	public static ConfigEntry<string> configBlueprintFolder;

	public static ConfigEntry<bool> configSaveBlueprintsToProfile;

	public static ConfigEntry<string> configSavedObjectData;

	public static HashSet<string> SavedObjectData = new HashSet<string>();

	public static ConfigEntry<int> configMaxTabs;

	public static ConfigEntry<int> configItemsPerTab;

	public static ConfigEntry<string> configBlueprintSorting;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenuTypes;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenuBlueprints;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenuObjects;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenuLocations;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenuSounds;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenuVisuals;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenuTools;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenuRooms;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenuBinds;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenu;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenuBuilds;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenuBuildsHammer;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenuBuildsHoe;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenuBuildsCultivator;

	public static ConfigEntry<KeyboardShortcut> configKeybindMenuBuildsFeaster;

	public static ConfigEntry<string> configIgnoredRemoveIds;

	public static string[] RemoveIds = Array.Empty<string>();

	public static ConfigEntry<string> configIgnoredIds;

	public static string[] IgnoredIds = Array.Empty<string>();

	public static ConfigEntry<string> configHammerTools;

	public static HashSet<string> HammerTools = new HashSet<string>();

	public static ConfigEntry<string> configMirrorFlip;

	public static HashSet<int> MirrorFlip = new HashSet<int>();

	public static ConfigEntry<string> configDimensions;

	public static Dictionary<string, Vector3> Dimensions = new Dictionary<string, Vector3>();

	public static ConfigEntry<bool> configEnabled;

	public static ConfigEntry<bool> configToolsEnabled;

	public static ConfigEntry<bool> configShapeCircle;

	public static ConfigEntry<bool> configShapeRing;

	public static ConfigEntry<bool> configShapeSquare;

	public static ConfigEntry<bool> configShapeRectangle;

	public static ConfigEntry<bool> configShapeFrame;

	public static ConfigEntry<bool> configShowCommandValues;

	public static ConfigEntry<KeyboardShortcut> shapeKey;

	public static ConfigEntry<KeyboardShortcut> commandRadius;

	public static ConfigEntry<KeyboardShortcut> commandRotate;

	public static ConfigEntry<KeyboardShortcut> commandDepth;

	public static ConfigEntry<KeyboardShortcut> commandHeight;

	public static ConfigEntry<KeyboardShortcut> commandModifier1;

	public static ConfigEntry<KeyboardShortcut> commandModifier2;

	public static ConfigEntry<string> commandHeightAmount;

	public static ConfigEntry<string> configToolIncludedIds;

	public static ConfigEntry<string> configToolIgnoredIds;

	public static ConfigEntry<bool> hideEffects;

	public static ConfigEntry<bool> hidePlacementMarker;

	public static ConfigEntry<bool> hideSupportColor;

	public static ConfigEntry<bool> hidePieceHealth;

	public static bool NoResourceCost => IsEnabled(InfinityPermissionHash.NoResourceCost, configNoResourceCost.Value);

	public static bool NoDurabilityCost => IsEnabled(InfinityPermissionHash.NoDurabilityCost, configNoDurabilityCost.Value);

	public static bool NoStaminaCost => IsEnabled(InfinityPermissionHash.NoStaminaCost, configNoStaminaCost.Value);

	public static bool GroupResourceCost => IsEnabled(InfinityPermissionHash.GroupResourceCost, configGroupResourceCost.Value);

	public static bool IgnoreWards => IsEnabled(InfinityPermissionHash.IgnoreWards, configIgnoreWards.Value);

	public static bool IgnoreNoBuild => IsEnabled(InfinityPermissionHash.IgnoreNoBuild, configIgnoreNoBuild.Value);

	public static bool AllowInDungeons => IsEnabled(InfinityPermissionHash.AllowInDungeons, configAllowInDungeons.Value);

	public static bool IgnoreOtherRestrictions => IsEnabled(InfinityPermissionHash.IgnoreOtherRestrictions, configIgnoreOtherRestrictions.Value);

	public static bool RemoveAnything => IsEnabled(InfinityPermissionHash.RemoveAnything, configRemoveAnything.Value);

	public static bool DisableMessages => configDisableMessages.Value;

	public static bool DisableSelectMessages => configDisableSelectMessages.Value;

	public static bool DisableOffsetMessages => configDisableOffsetMessages.Value;

	public static bool DisableScaleMessages => configDisableScaleMessages.Value;

	public static bool DisableLoot => IsEnabled(InfinityPermissionHash.DisableLoot, configDisableLoot.Value);

	public static bool RemoveContents => configRemoveContents.Value;

	public static bool RepairAnything => IsEnabled(InfinityPermissionHash.RepairAnything, configRepairAnything.Value);

	public static bool NoCreator => IsEnabled(InfinityPermissionHash.NoCreator, configNoCreator.Value);

	public static bool NoTarget => IsEnabled(InfinityPermissionHash.NoTarget, configNoTarget.Value);

	public static bool NoPhysics => IsEnabled(InfinityPermissionHash.NoPhysics, configNoPhysics.Value);

	public static bool NoRemove => IsEnabled(InfinityPermissionHash.NoRemove, configNoRemove.Value);

	public static string ZoopMagic => configZoopMagic.Value;

	public static bool ResetOffsetOnUnfreeze => configResetOffsetOnUnfreeze.Value;

	public static bool UnfreezeOnUnequip => configUnfreezeOnUnequip.Value;

	public static bool UnfreezeOnSelect => configUnfreezeOnSelect.Value;

	public static float OverwriteHealth
	{
		get
		{
			if (!IsEnabled(InfinityPermissionHash.OverwriteHealth, localValue: true))
			{
				return 0f;
			}
			return Parse.Float(configOverwriteHealth.Value, 0f);
		}
	}

	public static string Invulnerability
	{
		get
		{
			if (!IsEnabled(InfinityPermissionHash.Invulnerability, localValue: true))
			{
				return "Off";
			}
			return configInvulnerability.Value;
		}
	}

	public static bool PreserveWear => configPreserveWear.Value;

	public static string Snapping => configSnapping.Value;

	public static float RemoveArea
	{
		get
		{
			if (!Enabled)
			{
				return 0f;
			}
			return Parse.Float(configRemoveArea.Value, 0f);
		}
	}

	public static float Range
	{
		get
		{
			if (!IsEnabled(InfinityPermissionHash.Range, localValue: true))
			{
				return 0f;
			}
			return Parse.Float(configRange.Value, 0f);
		}
	}

	public static bool PlaceEmptyRooms => configPlaceEmptyRooms.Value;

	public static bool ScaleZSyncObjects => configScaleZSyncObjects.Value;

	public static float ScalePrecision => ParsePercentage(configScalePrecision.Value);

	public static bool SaveBlueprintData => configSaveBlueprintData.Value;

	public static string BlueprintCenterPiece => configBlueprintCenterPiece.Value;

	public static string BlueprintSnapPiece => configBlueprintSnapPiece.Value;

	public static bool SimplerBlueprints => configSaveSimplerBlueprints.Value;

	public static bool UseBlueprintChance => configUseBlueprintChance.Value;

	public static bool IncludeTerrainHeight => configIncludeTerrainHeight.Value;

	public static bool IncludeTerrainPaint => configIncludeTerrainPaint.Value;

	public static float BlueprintTerrainHeightOffset => Parse.Float(configBlueprintTerrainHeightOffset.Value, 0f);

	public static float BlueprintTerrainPaintOffset => Parse.Float(configBlueprintTerrainPaintOffset.Value, 0f);

	public static float BlueprintTerrainHeightSmooth => ClampBlueprintSmooth(Parse.Float(configBlueprintTerrainHeightSmooth.Value, 0f));

	public static float BlueprintTerrainPaintSmooth => ClampBlueprintSmooth(Parse.Float(configBlueprintTerrainPaintSmooth.Value, 0f));

	public static float BlueprintTerrainNodeSpacing => Math.Max(0f, Parse.Float(configBlueprintTerrainNodeSpacing.Value, 0f));

	public static float BlueprintPaintNodeSpacing => Math.Max(0f, Parse.Float(configBlueprintPaintNodeSpacing.Value, 0f));

	public static string BlueprintGlobalFolder => Path.Combine("BepInEx", "config", configBlueprintFolder.Value);

	public static string BlueprintLocalFolder => Path.Combine(Paths.ConfigPath, configBlueprintFolder.Value);

	public static bool SaveBlueprintsToProfile => configSaveBlueprintsToProfile.Value;

	public static int MaxTabs => configMaxTabs.Value;

	public static int ItemsPerTab => configItemsPerTab.Value;

	public static string BlueprintSorting => configBlueprintSorting.Value;

	public static bool Enabled => configEnabled.Value;

	public static bool ToolsEnabled => IsEnabled(InfinityPermissionHash.ToolsEnabled, configToolsEnabled.Value);

	public static bool ShapeCircle => configShapeCircle.Value;

	public static bool ShapeRing => configShapeRing.Value;

	public static bool ShapeSquare => configShapeSquare.Value;

	public static bool ShapeRectangle => configShapeRectangle.Value;

	public static bool ShapeFrame => configShapeFrame.Value;

	public static bool AlwaysShowCommand => configShowCommandValues.Value;

	public static bool HideEffects
	{
		get
		{
			if (hideEffects.Value)
			{
				return Enabled;
			}
			return false;
		}
	}

	public static bool HidePlacementMarker
	{
		get
		{
			if (hidePlacementMarker.Value)
			{
				return Enabled;
			}
			return false;
		}
	}

	public static bool HideSupportColor
	{
		get
		{
			if (hideSupportColor.Value)
			{
				return Enabled;
			}
			return false;
		}
	}

	public static bool HidePieceHealth
	{
		get
		{
			if (hidePieceHealth.Value)
			{
				return Enabled;
			}
			return false;
		}
	}

	public static bool IsEnabled(int hash, bool localValue)
	{
		if (Enabled)
		{
			return PermissionManager.Instance.IsFeatureEnabledByHash("infinityhammer", hash, localValue);
		}
		return false;
	}

	private static float ParsePercentage(string value)
	{
		if (value.EndsWith("%", StringComparison.Ordinal))
		{
			return Parse.Float(value.Substring(0, value.Length - 1), 5f) / 100f;
		}
		return Parse.Float(value, 0.05f);
	}

	public static void Init(ConfigWrapper wrapper)
	{
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Expected O, but got Unknown
		Wrapper = wrapper;
		string group = "1. General";
		configRemoveArea = wrapper.Bind(group, "Remove area", "0", "Removes same objects within the radius.");
		configRange = wrapper.Bind(group, "Hammer range", "50", "Range for actions.");
		configNoResourceCost = wrapper.Bind(group, "No resource cost", value: false, "Removes resource costs.");
		configNoDurabilityCost = wrapper.Bind(group, "No durability cost", value: false, "Removes durability costs.");
		configNoStaminaCost = wrapper.Bind(group, "No stamina cost", value: false, "Removes stamina and eitr costs.");
		configGroupResourceCost = wrapper.Bind(group, "Group resource cost", value: false, "Requires resources (summed from all objects) when placing multiple objects or blueprints. Ignored when No resource cost is enabled.");
		configIgnoreWards = wrapper.Bind(group, "Ignore wards", value: true, "Ignores ward restrictions.");
		configIgnoreNoBuild = wrapper.Bind(group, "Ignore no build", value: true, "Ignores no build areas.");
		configAllowInDungeons = wrapper.Bind(group, "Allow in dungeons", value: true, "Allows building in dungeons.");
		configRemoveAnything = wrapper.Bind(group, "Remove anything", value: false, "Allows removing anything.");
		configDisableLoot = wrapper.Bind(group, "Disable loot", value: false, "Prevents creatures and structures dropping loot when removed with the hammer.");
		configRemoveContents = wrapper.Bind(group, "Remove chest contents", value: false, "Removes anything also removes armor stand, item stand and chest contents.");
		configRepairAnything = wrapper.Bind(group, "Repair anything", value: false, "Allows reparing anything.");
		configOverwriteHealth = wrapper.Bind(group, "Overwrite health", "0", "Overwrites the health of built or repaired objects.");
		configInvulnerability = wrapper.Bind(group, "Set invulnerability", "Off", new ConfigDescription("Built objects are invulnerable.", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[5] { "Off", "On", "Damaged", "Worn", "Legacy" }), Array.Empty<object>()));
		configPreserveWear = wrapper.Bind(group, "Preserve wear levels", value: false, "Prevents wear level from being overridden by invulnerability");
		configZoopMagic = wrapper.Bind(group, "Zoop magic mode", "Off", new ConfigDescription("Zoop magic mode.", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[3] { "Off", "Mild", "Wild" }), Array.Empty<object>()));
		configNoCreator = wrapper.Bind(group, "No creator", value: false, "Reduces save data by not setting the creator id.");
		configNoTarget = wrapper.Bind(group, "No target", value: false, "Prevents the build piece being targeted by enemies.");
		configNoPhysics = wrapper.Bind(group, "No physics", value: false, "Prevents objects from falling when placed.");
		configNoRemove = wrapper.Bind(group, "No remove", value: false, "Prevents the build piece from being removed by players.");
		configUnfreezeOnSelect = wrapper.Bind(group, "Unfreeze on select", value: true, "Removes the placement freeze when selecting a new object.");
		configResetOffsetOnUnfreeze = wrapper.Bind(group, "Reset offset on unfreeze", value: true, "Removes the placement offset when unfreezing the placement.");
		configUnfreezeOnUnequip = wrapper.Bind(group, "Unfreeze on unequip", value: true, "Removes the placement freeze when unequipping the hammer.");
		configSnapping = wrapper.Bind(group, "Snap points", "Corners", new ConfigDescription("Automatic snap points.", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[4] { "Off", "Edges", "Corners", "All" }), Array.Empty<object>()));
		configScaleZSyncObjects = wrapper.Bind(group, "Scale ZSyncTransform objects", value: false, "Allows scaling objects with ZSyncTransform (required Expand World Prefabs mod on server)");
		configScalePrecision = wrapper.Bind(group, "Scale precision", "5%", "Snaps hammer_zoom scaling changes to this percentage interval. Use 0 to disable.");
		configIgnoreOtherRestrictions = wrapper.Bind(group, "Ignore other restrictions", value: true, "Ignores any other restrictions (material, biome, etc.)");
		configPlaceEmptyRooms = wrapper.Bind(group, "Place empty rooms", value: false, "hammer_room command places rooms without their contents.");
		InitVisuals(wrapper);
		InitBinds(wrapper);
		group = "4. Messages";
		configDisableMessages = wrapper.Bind(group, "Disable messages", value: false, "Disables all messages from this mod.");
		configDisableOffsetMessages = wrapper.Bind(group, "Disable offset messages", value: false, "Disables messages from changing placement offset.");
		configDisableScaleMessages = wrapper.Bind(group, "Disable scale messages", value: false, "Disables messages from changing the scale.");
		configDisableSelectMessages = wrapper.Bind(group, "Disable select messages", value: false, "Disables messages from selecting objects.");
		InitOther(wrapper);
		InitTools(wrapper);
		InitBlueprint(wrapper);
		InitCustomMenu(wrapper);
	}

	private static void InitBinds(ConfigWrapper wrapper)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Unknown result type (might be due to invalid IL or missing references)
		string group = "3. Binds";
		moveAmount = wrapper.Bind(group, "Move amount", "0.1", "Meters to move with move binds.");
		moveAmountLarge = wrapper.Bind(group, "Move amount large", "1", "Meters to move with large move binds.");
		gridPrecision = wrapper.Bind(group, "Grid precision", "1", "Grid precision in meters.");
		buildScale = wrapper.BindWheelCommand("hammer_zoom 5%", group, "Build scaling (mouse wheel)", new KeyboardShortcut((KeyCode)304, Array.Empty<KeyCode>()), "Changes the selection scale.", "build");
		zoopForward = wrapper.BindWheelCommand("hammer_zoop_forward auto", group, "Zooping forward (mouse wheel)", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Zoops next to each other.", "build");
		zoopHorizontal = wrapper.BindWheelCommand("hammer_zoop_right auto", group, "Zooping horizontal (mouse wheel)", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Zoops next to each other.", "build");
		zoopVertical = wrapper.BindWheelCommand("hammer_zoop_up auto", group, "Zooping vertical (mouse wheel)", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Zoops next to each other.", "build");
		moveUp = wrapper.BindCommand(() => "hammer_move_up " + moveAmount.Value, group, "Move up", new KeyboardShortcut((KeyCode)280, Array.Empty<KeyCode>()), "Precise placement.", "build, command");
		moveUpLarge = wrapper.BindCommand(() => "hammer_move_up " + moveAmountLarge.Value, group, "Move up (large)", new KeyboardShortcut((KeyCode)280, (KeyCode[])(object)new KeyCode[1] { (KeyCode)308 }), "Precise placement.", "build, command");
		moveDown = wrapper.BindCommand(() => "hammer_move_down " + moveAmount.Value, group, "Move down", new KeyboardShortcut((KeyCode)281, Array.Empty<KeyCode>()), "Precise placement.", "build, command");
		moveDownLarge = wrapper.BindCommand(() => "hammer_move_down " + moveAmountLarge.Value, group, "Move down (large)", new KeyboardShortcut((KeyCode)281, (KeyCode[])(object)new KeyCode[1] { (KeyCode)308 }), "Precise placement.", "build, command");
		moveLeft = wrapper.BindCommand(() => "hammer_move_left " + moveAmount.Value, group, "Move left", new KeyboardShortcut((KeyCode)276, Array.Empty<KeyCode>()), "Precise placement.", "build, command");
		moveLeftLarge = wrapper.BindCommand(() => "hammer_move_left " + moveAmountLarge.Value, group, "Move left (large)", new KeyboardShortcut((KeyCode)276, (KeyCode[])(object)new KeyCode[1] { (KeyCode)308 }), "Precise placement.", "build, command");
		moveRight = wrapper.BindCommand(() => "hammer_move_right " + moveAmount.Value, group, "Move right", new KeyboardShortcut((KeyCode)275, Array.Empty<KeyCode>()), "Precise placement.", "build, command");
		moveRightLarge = wrapper.BindCommand(() => "hammer_move_right " + moveAmountLarge.Value, group, "Move right (large)", new KeyboardShortcut((KeyCode)275, (KeyCode[])(object)new KeyCode[1] { (KeyCode)308 }), "Precise placement.", "build, command");
		moveForward = wrapper.BindCommand(() => "hammer_move_forward " + moveAmount.Value, group, "Move forward", new KeyboardShortcut((KeyCode)273, Array.Empty<KeyCode>()), "Precise placement.", "build, command");
		moveForwardLarge = wrapper.BindCommand(() => "hammer_move_forward " + moveAmountLarge.Value, group, "Move forward (large)", new KeyboardShortcut((KeyCode)273, (KeyCode[])(object)new KeyCode[1] { (KeyCode)308 }), "Precise placement.", "build, command");
		moveBackward = wrapper.BindCommand(() => "hammer_move_backward " + moveAmount.Value, group, "Move backward", new KeyboardShortcut((KeyCode)274, Array.Empty<KeyCode>()), "Precise placement.", "build, command");
		moveBackwardLarge = wrapper.BindCommand(() => "hammer_move_backward " + moveAmountLarge.Value, group, "Move backward (large)", new KeyboardShortcut((KeyCode)274, (KeyCode[])(object)new KeyCode[1] { (KeyCode)308 }), "Precise placement.", "build, command");
		freeze = wrapper.BindCommand("hammer_freeze", group, "Freeze selection", new KeyboardShortcut((KeyCode)256, Array.Empty<KeyCode>()), "Freezes placement position for precise placement.", "build, command");
		resetOffset = wrapper.BindCommand("hammer_offset", group, "Reset offset", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Resets the offset.", "build, command");
		select = wrapper.BindCommand("hammer", group, "Select", new KeyboardShortcut((KeyCode)261, Array.Empty<KeyCode>()), "Select the hovered object.");
		selectFrozen = wrapper.BindCommand("hammer freeze", group, "Select frozen", new KeyboardShortcut((KeyCode)261, (KeyCode[])(object)new KeyCode[1] { (KeyCode)306 }), "Select the hovered object.");
		pick = wrapper.BindCommand("hammer pick", group, "Pick", new KeyboardShortcut((KeyCode)261, (KeyCode[])(object)new KeyCode[1] { (KeyCode)308 }), "Pick the hovered object.");
		pickFrozen = wrapper.BindCommand("hammer pick freeze", group, "Pick frozen", new KeyboardShortcut((KeyCode)261, (KeyCode[])(object)new KeyCode[2]
		{
			(KeyCode)308,
			(KeyCode)306
		}), "Select the hovered object.");
		selectAll = wrapper.BindCommand("hammer connect", group, "Select building", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Select the whole building.");
		pickAll = wrapper.BindCommand("hammer connect pick", group, "Pick building", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Pick the whole building.");
		selectAllFrozen = wrapper.BindCommand("hammer connect freeze", group, "Select building frozen", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Select the whole building.");
		pickAllFrozen = wrapper.BindCommand("hammer connect pick freeze", group, "Pick building frozen", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Pick the whole building.");
		undo = wrapper.BindCommand("undo", group, "Undo", new KeyboardShortcut((KeyCode)263, Array.Empty<KeyCode>()), "Undo actions.");
		redo = wrapper.BindCommand("redo", group, "Redo", new KeyboardShortcut((KeyCode)265, Array.Empty<KeyCode>()), "Redo actions.");
		grid = wrapper.BindCommand(() => "hammer_grid " + gridPrecision.Value + " 0,0,0", group, "Grid", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Toggles grid.", "build, command");
		gridHold = wrapper.BindCommand(() => "hammer_grid " + gridPrecision.Value + " 0,0,0", group, "Grid hold", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Enables grid when keys are down.", "hammer_grid", "build, command");
	}

	private static void UpdateSavedObjectData()
	{
		SavedObjectData = ParseHashList(configSavedObjectData.Value);
	}

	private static float ClampBlueprintSmooth(float value)
	{
		if (value < 0f)
		{
			return 0f;
		}
		if (value > 1f)
		{
			return 1f;
		}
		return value;
	}

	private static void InitBlueprint(ConfigWrapper wrapper)
	{
		string group = "6. Blueprints";
		configBlueprintFolder = wrapper.Bind(group, "Blueprint folder", "PlanBuild", "Folder relative to the config folder.");
		configSaveBlueprintsToProfile = wrapper.Bind(group, "Save blueprints to profile", value: false, "If enabled, blueprints are saved to the profile folder instead of base Valheim folder.");
		configSaveBlueprintData = wrapper.Bind(group, "Save data to blueprints", value: true, "If enabled, object data values are saved to blueprints.");
		configSaveSimplerBlueprints = wrapper.Bind(group, "Save simpler blueprints", value: false, "If enabled, only mandatory information is saved.");
		configSavedObjectData = wrapper.Bind(group, "Save object data blueprints", "", "Object ids that save extra data if save data is disabled.");
		configSavedObjectData.SettingChanged += delegate
		{
			UpdateSavedObjectData();
		};
		configUseBlueprintChance = wrapper.Bind(group, "Use blueprint chance", value: false, "If enabled, chance field is checked from the blueprint.");
		configIncludeTerrainHeight = wrapper.Bind(group, "Include terrain height", value: false, "If enabled, hammer area selection captures terrain height.");
		configIncludeTerrainPaint = wrapper.Bind(group, "Include terrain paint", value: false, "If enabled, hammer area selection captures terrain paint.");
		configBlueprintTerrainHeightOffset = wrapper.Bind(group, "Terrain height offset", "0", "Radius/size offset in meters used when capturing terrain height.");
		configBlueprintTerrainPaintOffset = wrapper.Bind(group, "Terrain paint offset", "0", "Radius/size offset in meters used when capturing terrain paint.");
		configBlueprintTerrainHeightSmooth = wrapper.Bind(group, "Terrain height smooth", "0", "How gradually blueprint terrain height changes are applied. 0 = exact, 1 = fully gradual.");
		configBlueprintTerrainPaintSmooth = wrapper.Bind(group, "Terrain paint smooth", "0", "How gradually blueprint terrain paint changes are applied. 0 = exact, 1 = fully gradual.");
		configBlueprintTerrainNodeSpacing = wrapper.Bind(group, "Terrain height spacing", "0", "Distance between captured terrain nodes in meters. Use 0 to auto-detect from the terrain heightmap scale.");
		configBlueprintPaintNodeSpacing = wrapper.Bind(group, "Terrain paint spacing", "0", "Distance between captured terrain paint nodes in meters. Use 0 to auto-detect from the terrain heightmap scale.");
		configBlueprintCenterPiece = wrapper.Bind(group, "Blueprint center piece", "", "Piece name that is used as the center point when saving a blueprint.");
		configBlueprintSnapPiece = wrapper.Bind(group, "Blueprint snap piece", "", "Piece name that is used as the snap point when saving a blueprint.");
		UpdateSavedObjectData();
	}

	private static void InitCustomMenu(ConfigWrapper wrapper)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		string group = "7. Build Menu";
		configMaxTabs = wrapper.Bind(group, "Max tabs", 5, "Maximum number of tabs in the custom build menu.");
		configItemsPerTab = wrapper.Bind(group, "Items per tab", 90, "Maximum number of items per tab in the custom build menu.");
		configBlueprintSorting = wrapper.Bind(group, "Blueprint sorting", "Folder", new ConfigDescription("How blueprints are sorted in the menu.", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[4] { "Category", "Folder", "Category and folder", "Only name" }), Array.Empty<object>()));
		configKeybindMenu = wrapper.BindCommand("hammer_menu", group, "Menu", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the custom menu.");
		configKeybindMenuBinds = wrapper.BindCommand("hammer_menu binds", group, "Menu: Binds", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the binds menu.");
		configKeybindMenuBlueprints = wrapper.BindCommand("hammer_menu blueprints", group, "Menu: Blueprints", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the blueprints menu.");
		configKeybindMenuBuilds = wrapper.BindCommand("hammer_menu builds", group, "Menu: Builds", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the builds menu.");
		configKeybindMenuLocations = wrapper.BindCommand("hammer_menu locations", group, "Menu: Locations", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the locations menu.");
		configKeybindMenuObjects = wrapper.BindCommand("hammer_menu objects", group, "Menu: Objects", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the objects menu.");
		configKeybindMenuRooms = wrapper.BindCommand("hammer_menu rooms", group, "Menu: Rooms", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the rooms menu.");
		configKeybindMenuSounds = wrapper.BindCommand("hammer_menu sounds", group, "Menu: Sounds", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the sounds menu.");
		configKeybindMenuTools = wrapper.BindCommand("hammer_menu tools", group, "Menu: Tools", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the tools menu.");
		configKeybindMenuTypes = wrapper.BindCommand("hammer_menu types", group, "Menu: Types", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the objects by type menu.");
		configKeybindMenuVisuals = wrapper.BindCommand("hammer_menu visuals", group, "Menu: Visuals", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the visuals menu.");
		configKeybindMenuBuildsHammer = wrapper.BindCommand("hammer_menu builds hammer", group, "Menu: Builds (Hammer)", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the builds menu with hammer filter.");
		configKeybindMenuBuildsHoe = wrapper.BindCommand("hammer_menu builds hoe", group, "Menu: Builds (Hoe)", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the builds menu with hoe filter.");
		configKeybindMenuBuildsCultivator = wrapper.BindCommand("hammer_menu builds cultivator", group, "Menu: Builds (Cultivator)", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the builds menu with cultivator filter.");
		configKeybindMenuBuildsFeaster = wrapper.BindCommand("hammer_menu builds feaster", group, "Menu: Builds (Feaster)", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Opens the builds menu with feaster filter.");
	}

	private static string[] ParseArray(string value)
	{
		return (from s in value.Split(',')
			select s.Trim().ToLower() into s
			where s != ""
			select s).ToArray();
	}

	private static HashSet<string> ParseHashList(string value)
	{
		return (from s in value.Split(',')
			select s.Trim().ToLower() into s
			where s != ""
			select s).ToHashSet();
	}

	private static HashSet<int> ParseHashedList(string value)
	{
		return (from s in value.Split(',')
			select StringExtensionMethods.GetStableHashCode(s.Trim())).ToHashSet();
	}

	private static Dictionary<string, Vector3> ParseSize(string value)
	{
		return (from s in value.Split('|')
			select s.Trim().ToLower() into s
			where s != ""
			select s.Split(',') into split
			where split.Length == 4
			select split).ToDictionary((string[] split) => split[0], (string[] split) => Parse.VectorXZY(split, 1));
	}

	private static void UpdateTools()
	{
		HammerTools = ParseHashList(configHammerTools.Value);
	}

	private static void UpdateMirrorFlip()
	{
		MirrorFlip = ParseHashedList(configMirrorFlip.Value);
	}

	private static void UpdateDimensions()
	{
		if (configDimensions.Value == "")
		{
			configDimensions.Value = "abomination,9.07,9.2,6.93|armorstand,0.55,0.3,1.93|armorstand_female,1.87,0.54,1.93|armorstand_male,1.88,0.54,1.93|barrell,0.84,0.85,1.1|bat,0.8,0.8,0.8|bed,1.2,2.8,0.4|beech_log,0.88,0.88,16.96|beech_log_half,0.8,0.8,7.31|beech_sapling,0.2,0.2,1|beech_small1,0.2,0.2,2|beech_small2,0.2,0.2,2|beech_stub,1.79,1.79,1.67|birch_log,1.02,1.02,12.7|birch_log_half,0.93,0.93,5.74|birch_sapling,0.2,0.2,1.6|birch1,4.71,8.11,19.23|birch1_aut,4.71,8.11,19.23|birch2,6.28,9.45,18.63|birch2_aut,6.28,9.45,18.63|birchstub,1.58,1.51,1.7|blastfurnace,3.57,3.79,5.2|blob,1.56,1.56,1.56|blobelite,2.4,2.4,2.4|blobtar,1.56,1.56,1.56|blueberrybush,1,1,1.09|boar,1,1,1.4|boar_piggy,0.6,0.6,1|bonemass_aoe,0,0,0|bonepilespawner,2,1.98,1.01|bonfire,4.31,4.37,1.03|bossstone_bonemass,6.17,1.87,7.44|bossstone_dragonqueen,9.69,1.92,5.56|bossstone_theelder,3,1.66,10.52|bossstone_yagluth,6.11,2.85,9.29|bow_projectile_needle,0.1,1.82,0.08|bucket,0.72,0.72,0.67|bush01,0.4,0.4,0.91|bush01_heath,0.4,0.4,0.91|bush02_en,0.8,0.8,2.96|cargocrate,0.76,0.76,0.76|cart,1.72,3.25,1.38|castlekit_braided_box01,0.76,0.76,0.76|castlekit_brazier,0.75,0.75,0.7|castlekit_groundtorch,0.23,0.24,1.48|castlekit_groundtorch_green,0.23,0.24,1.48|castlekit_groundtorch_unlit,0.12,0.13,1.41|castlekit_metal_groundtorch_unlit,0.14,0.15,1.7|cauldron_ext1_spice,2.18,0.1,1.89|cauldron_ext3_butchertable,1.2,1.2,0.95|cauldron_ext4_pots,2,0.63,1.9|caverock_ice_pillar_wall,9.25,3.24,11.4|caverock_ice_stalagmite,1.36,1.33,4.82|caverock_ice_stalagmite_broken,1.36,1.33,1.48|caverock_ice_stalagtite,0.94,0.94,3.39|caverock_ice_stalagtite_falling,0.37,0.38,1.69|charcoal_kiln,4.7,5.28,3.12|chest,1.05,0.78,0.86|cloth_hanging_door,2,0.15,4|cloth_hanging_door_double,4,0.15,4|cloth_hanging_long,2,0.15,8|cloudberrybush,0.74,0.74,0.74|coal_pile,1.46,1.49,1.03|crow,1,1,1|crystal_wall_1x1,1,0.3,1|darkwood_arch,2.23,0.4,2.2|darkwood_beam,2,0.4,0.4|darkwood_beam4x4,4,0.4,0.4|darkwood_decowall,1,0.1,2|darkwood_gate,2.02,0.45,4.02|darkwood_pole,0.4,0.4,2|darkwood_pole4,0.4,0.4,4|darkwood_roof_icorner,2.4,2.4,1.37|darkwood_roof_icorner_45,2.38,2.38,2.24|darkwood_roof_ocorner,2.74,2.74,1.35|darkwood_roof_ocorner_45,2.52,2.52,2.3|deathsquito,1,1,1|deer,1.6,1.6,1.78|dragon,18.03,5.38,11.35|dragoneggcup,1,1,0.43|draugr,0.8,0.8,2.2|draugr_elite,1,1,2.5|draugr_ranged,0.8,0.8,2.2|dungeon_forestcrypt_door,2.28,0.5,3|dungeon_sunkencrypt_irongate,2,0.2,3.29|eikthyr,2.91,5.55,3.71|evilheart_forest,0.75,0.89,1|evilheart_swamp,0.75,0.89,1|fenring,1.61,1.61,3.23|fenring_attack_flames_aoe,1.63,5.45,4.06|fenring_cultist,1,1,2.5|fermenter,1.6,1.55,2.2|fire_pit,1.4,1.4,0.2|firtree,0.8,0.8,10.89|firtree_log,0.62,0.62,7.75|firtree_log_half,0.57,0.57,3.45|firtree_oldlog,1.26,7.05,1.57|firtree_sapling,0.2,0.2,1|firtree_small,0.4,0.4,5.45|firtree_small_dead,0.8,0.8,10.89|firtree_stub,1.53,1.36,0.49|fish1,0.14,1.03,0.46|fish2,0.19,1.71,0.61|fish3,0.33,1.74,0.72|fish4_cave,0.17,0.87,0.36|fishingrodfloat,0.3,0.3,0.3|forge,1.8,1,0.91|forge_ext1,1.76,1.59,1.91|forge_ext2,0.84,0.9,0.84|forge_ext3,0.75,1.05,1.41|forge_ext4,0.9,0.9,1.4|forge_ext5,0.95,0.94,1|forge_ext6,2.5,0.2,0.3|ghost,1,1,2|glowingmushroom,1.49,1.37,1.53|goblin,0.8,0.8,1.45|goblin_bed,1.2,2.9,0.7|goblin_stairs,2,2.03,1.12|goblin_stepladder,1,2.16,2.16|goblinarcher,0.8,0.8,1.45|goblinbrute,1.6,1.6,3.5|goblinking,15.7,4.35,8.9|goblinshaman,0.8,0.8,1.45|goblinshaman_protect_aoe,0,0,0|greydwarf_elite,1.28,1.28,2.62|greydwarf_root,1.77,1.3,1.59|guard_stone,1.03,0.78,1.61|guard_stone_test,1.55,1.3,2|gucksack,2.41,2.79,3.92|gucksack_small,1.21,1.4,1.96|haldor,1,1,1.63|hanging_hairstrands,0.2,0.2,1.5|hatchling,4,2.02,2.3|hearth,4,3,0.6|heathrockpillar,9.86,12.54,44.38|heathrockpillar_frac,9.86,12.54,44.38|highstone,4.32,2.24,6.65|highstone_frac,4.32,2.24,6.65|hugeroot1,29.27,11.8,10.07|ice_floor,4.6,4.6,2|ice_floor_fractured,4.6,4.6,2|ice_rock1,24,15.55,29.14|ice_rock1_frac,24,15.55,29.14|ice1,8.35,8,0.69|iceblocker,2,2,3|incinerator,2.36,1.27,10.19|iron_grate,2.01,0.2,2.99|iron_wall_1x1,1,0.1,1|itemstand,0.5,0.09,0.55|itemstandh,0.2,0.2,0.04|jute_carpet,3.57,2.56,0.05|karve,7.68,10.12,8.82|leech,1.6,1.63,1.6|leech_cave,1.6,1.63,1.6|leviathan,69.97,60.41,45.02|loot_chest_stone,2.02,0.86,0.54|loot_chest_wood,1.64,0.75,0.74|lox,3,6.56,3.5|lox_calf,2,2,2|marker01,0.93,0.96,1.65|marker02,0.93,0.96,1.65|minerock_copper,3.27,3.48,2.88|minerock_iron,3.27,3.48,2.88|minerock_meteorite,2.17,2.35,2.03|minerock_obsidian,1.99,2.16,1.85|minerock_stone,3.27,3.48,2.88|minerock_tin,2.29,1.8,1.38|mountainkit_brazier,0.75,0.75,0.7|mountainkit_chair,0.55,0.57,1.21|mountainkit_table,2.53,2.46,0.78|mountainkit_wood_gate,3,0.5,4.52|mudpile,2.9,3.18,1.46|mudpile_beacon,2.9,3.18,1.46|mudpile_frac,2.9,3.18,1.39|mudpile_old,2.9,3.18,1.39|mudpile2,5.29,5.31,5.01|mudpile2_frac,5.29,5.31,5.01|oak_log,2.64,2.64,13.78|oak_log_half,2.4,2.4,6.29|oak_sapling,0.18,0.18,0.9|oakstub,4.2,3.97,2.9|odin,1.12,1,2.02|old_wood_roof_icorner,2.35,2.35,1.19|old_wood_roof_ocorner,2.47,2.47,1.27|old_wood_roof_top,2,2.21,0.91|old_wood_wall_roof,2.27,0.4,1.31|pickable_barley,0.8,0.8,1.45|pickable_barley_wild,0.8,0.8,1.45|pickable_bogironore,0.45,0.63,0.33|pickable_branch,1.12,3.29,1.12|pickable_carrot,0.28,0.28,0.89|pickable_dandelion,0.48,0.48,0.48|pickable_fishingrod,0.28,3.69,0.12|pickable_flax,0.6,0.6,1.45|pickable_flax_wild,0.8,0.8,1.45|pickable_flint,1,1,1|pickable_forestcryptremains01,1.03,0.86,0.44|pickable_forestcryptremains02,1.03,0.86,0.44|pickable_forestcryptremains03,1.03,0.86,0.44|pickable_forestcryptremains04,0.81,1.88,0.5|pickable_hairstrands01,0.3,0.3,2.25|pickable_hairstrands02,0.45,0.45,2.1|pickable_meatpile,0.8,0.8,0.35|pickable_meteorite,0.84,0.84,0.84|pickable_mountaincavecrystal,0.46,0.43,0.78|pickable_mountaincaveobsidian,0.5,0.5,0.5|pickable_mushroom,0.96,0.96,0.96|pickable_mushroom_blue,0.6,0.6,0.6|pickable_mushroom_yellow,0.6,0.6,0.6|pickable_obsidian,0.94,0.94,0.94|pickable_onion,0.28,0.28,1|pickable_seedcarrot,0.48,0.48,1.08|pickable_seedonion,0.6,0.6,2.15|pickable_seedturnip,0.4,0.4,1.04|pickable_stone,1,1,1|pickable_surtlingcorestand,0.4,0.46,1.4|pickable_tar,0.74,0.81,0.54|pickable_tarbig,1.9,2.1,1.53|pickable_thistle,0.4,0.38,1.52|pickable_tin,0.55,0.38,0.35|pickable_turnip,0.28,0.28,1|piece_banner01,0.3,1.41,0.3|piece_banner02,0.3,1.41,0.3|piece_banner03,0.3,1.41,0.3|piece_banner04,0.3,1.41,0.3|piece_banner05,0.3,1.41,0.3|piece_banner06,0.3,1.41,0.3|piece_banner07,0.3,1.41,0.3|piece_banner08,0.3,1.41,0.3|piece_banner09,0.3,1.41,0.3|piece_banner10,0.3,1.41,0.3|piece_banner11,0.3,1.41,0.3|piece_bathtub,3.17,3.81,2.73|piece_bed02,2.3,3.44,1.5|piece_beehive,1.34,1.3,1.58|piece_bench01,2.41,0.65,0.55|piece_brazierceiling01,0.97,0.91,1.96|piece_brazierfloor01,0.75,0.75,0.9|piece_cartographytable,3.93,1.95,1.39|piece_cauldron,0.94,0.94,1.4|piece_chair,0.54,0.5,0.53|piece_chair02,0.55,0.94,1.2|piece_chair03,0.55,0.57,1.21|piece_chest,1.84,0.95,1.07|piece_chest_blackmetal,2.06,1.35,0.92|piece_chest_treasure,1.16,0.76,0.52|piece_cloth_hanging_door,0.2,1.13,0.2|piece_cookingstation,1.73,0.78,1.28|piece_cookingstation_iron,3.6,1,2.08|piece_gift1,0.59,0.31,0.27|piece_gift2,0.71,0.37,0.32|piece_gift3,0.89,0.47,0.4|piece_groundtorch,0.23,0.24,1.48|piece_groundtorch_blue,0.23,0.24,1.48|piece_groundtorch_green,0.23,0.24,1.48|piece_groundtorch_wood,0.12,0.13,1.41|piece_jackoturnip,0.61,0.61,0.61|piece_logbench01,2.39,0.58,0.57|piece_maypole,1.22,4.13,5.1|piece_oven,3,2,2.37|piece_sharpstakes,1.8,1.51,0.84|piece_spinningwheel,2.04,2.67,2.27|piece_stonecutter,3.23,1.5,1.81|piece_table,2.45,1.22,0.79|piece_table_oak,6.45,1.85,0.79|piece_table_round,2.53,2.46,0.8|piece_throne01,1.4,0.84,2.49|piece_throne02,1.43,0.88,2.95|piece_walltorch,0.45,0.31,0.95|piece_workbench,3.01,1.1,1.03|piece_workbench_ext1,0.81,0.72,1.01|piece_workbench_ext2,2.01,0.6,1.75|piece_workbench_ext3,2.11,1.09,1.16|piece_workbench_ext4,1.5,0.3,1.3|piece_xmastree,0.2,0.2,2.72|pinetree,9.44,8.96,21.3|pinetree_01,0.62,0.62,24.43|pinetree_01_stub,1.2,1.07,0.46|pinetree_log_half,0.57,0.57,3.45|pinetree_log_halfold,0.8,0.8,6.36|pinetree_logold,0.88,0.88,14.65|pinetree_sapling,0.2,0.2,1|player_tombstone,0.5,0.39,1.16|portal,4.64,4.46,4.09|portal_wood,4.23,1.18,3.29|raft,6.09,6.91,6.56|raspberrybush,1,1,1.09|rock_3,2.1,1.89,2.08|rock_3_frac,2.1,1.89,2.08|rock_4,2.29,1.8,1.38|rock_4_plains,2.29,1.8,1.38|rock_destructible_test,3.27,3.48,2.88|rock1_mountain,24,15.55,29.14|rock1_mountain_frac,24,15.55,29.14|rock2_heath,14.58,17.57,14.48|rock2_heath_frac,14.58,17.57,14.48|rock2_mountain,19.44,19.31,23.42|rock2_mountain_frac,19.44,19.31,23.42|rock3_mountain,19.2,12.68,24.87|rock3_mountain_frac,19.2,12.68,24.87|rock3_silver,19.2,12.68,24.87|rock3_silver_frac,19.2,12.68,24.87|rock4_coast,27.75,24.16,16.84|rock4_coast_frac,27.75,24.16,16.84|rock4_copper,27.75,24.16,16.84|rock4_copper_frac,27.75,24.16,16.84|rock4_forest,27.75,24.16,16.84|rock4_forest_frac,27.75,24.16,16.84|rock4_heath,27.75,24.16,16.84|rock4_heath_frac,27.75,24.16,16.84|rockfinger,12.39,33.06,39.33|rockfinger_frac,12.39,33.06,39.33|rockfingerbroken_frac,12.39,14.45,32.73|rockformation1,31.55,36.81,57.95|rockthumb,15.99,23.3,37.88|rockthumb_frac,15.99,23.3,37.88|root07,1.22,0.44,1.61|rug_deer,2,3,0.05|rug_fur,2.56,3.57,0.05|rug_wolf,2,3,0.05|sapling_barley,0.8,0.8,1|sapling_carrot,0.36,0.36,1|sapling_flax,0.6,0.6,1|sapling_onion,0.36,0.36,1|sapling_seedcarrot,0.36,0.36,1|sapling_seedonion,0.36,0.36,1|sapling_seedturnip,0.36,0.36,1|sapling_turnip,0.36,0.36,1|seagal,1,1,1|serpent,2,32.26,2.24|shaman_attack_aoe,1.99,3.67,1|shaman_heal_aoe,0,0,0|ship_construction,11.76,26.91,14.64|shrub_2,1.14,1.14,1.14|shrub_2_heath,1.14,1.14,1.14|silvervein,17.17,11.13,3.23|silvervein_frac,17.17,11.13,3.23|skeleton,0.8,0.8,1.85|skeleton_noarcher,0.8,0.8,1.85|skeleton_poison,0.8,0.8,2.2|skull1,0.31,0.31,0.31|skull2,4.64,4.64,4.64|smelter,3.03,2.99,4.24|spawner_draugrpile,2.66,2.98,2.36|spawner_greydwarfnest,2,2,4.6|stake_wall,2,0.57,3.47|statueevil,1.9,2.26,5.19|stone_arch,2,1,1|stone_floor,4,4,1|stone_floor_2x2,2,2,1|stone_pile,1.67,1.54,0.92|stone_pillar,1,1,2|stone_stair,2,2,1|stone_wall_1x1,1,1,1|stone_wall_2x1,2,1,1|stone_wall_4x2,4,1,2|stoneblock_fracture,4,4,1|stonechest,2.02,0.86,0.54|stonegolem,2,2,4.5|stubbe,7.21,4.94,3.73|stubbe_spawner,7.34,4.94,4.09|sunken_crypt_gate,2,0.2,3.02|surtling,0.6,0.6,1.28|swamptree1,4.54,5.26,16.42|swamptree1_stub,4.45,5.26,3.06|swamptree2,14.07,16.5,36.22|swamptree2_darkland,14.07,16.5,36.22|swamptree2_log,18.76,48.29,22|tarliquid,64,64,40|tarlump1,27.75,24.16,8.42|tarlump1_frac,27.75,24.16,8.42|tentaroot,0.8,0.8,3|tolroko_flyer,2.49,3.99,1.35|trailership,9.16,21.57,11.65|trainingdummy,0.8,0.8,1.85|treasure_pile,1.5,1.51,0.27|treasure_stack,0.25,0.27,0.54|treasurechest_blackforest,1.64,0.75,0.74|treasurechest_fcrypt,2.02,0.86,0.54|treasurechest_forestcrypt,2.02,0.86,0.54|treasurechest_heath,1.64,0.75,0.74|treasurechest_meadows,1.64,0.75,0.74|treasurechest_mountaincave,2.42,1.04,0.65|treasurechest_plains_stone,2.02,0.86,0.54|treasurechest_sunkencrypt,2.02,0.86,0.54|treasurechest_swamp,1.64,0.75,0.74|troll,3.8,3.8,7.58|ulv,1,1.67,1.5|vfx_barrle_destroyed,0.81,0.82,1.14|vfx_destroyed_karve,7.81,10.1,8.82|vfx_destroyed_raft,6.31,6.42,7.29|vfx_destroyed_vikingship,8.49,22.68,11.7|vfx_greydwarfnest_destroyed,9.44,8.32,5.73|vfx_stonegolem_death,4.15,1.46,4.78|vikingship,9.16,21.57,11.65|vines,1,0.3,1|widestone,6.59,4.08,2.2|widestone_frac,6.59,4.08,2.2|windmill,5.66,5.6,8.64|wolf,1.3,1.3,1.5|wolf_cub,0.8,0.8,1|wood_beam,2,0.4,0.4|wood_beam_1,1,0.4,0.4|wood_beam_26,1.33,0.4,0.67|wood_beam_45,0.83,0.4,0.83|wood_core_stack,1.99,2.22,1.32|wood_door,2,0.5,2.02|wood_fence,2.69,0.29,2.08|wood_fine_stack,1.9,1.9,0.9|wood_floor,2,2,0.13|wood_floor_1x1,1,1,0.13|wood_gate,1.98,0.5,3|wood_ledge,1,0.6,0.05|wood_log_26,2.23,0.5,1.45|wood_log_45,2.35,0.5,2.35|wood_pole,0.4,0.4,1|wood_pole_log,0.4,0.4,2|wood_pole_log_4,0.4,0.4,4|wood_pole2,0.4,0.4,2|wood_roof_icorner,2.35,2.35,1.19|wood_roof_icorner_45,2.19,2.19,2.08|wood_roof_ocorner,2.47,2.47,1.27|wood_roof_ocorner_45,2.43,2.43,2.13|wood_stack,2.46,2.31,1.3|wood_stair,2,2.03,1.12|wood_stepladder,1,2.16,2.16|wood_wall_log,2,0.5,0.5|wood_wall_log_4x0.5,4,0.5,0.5|wood_wall_roof,2.27,0.4,1.54|wood_wall_roof_45,2.43,0.4,2.43|wood_wall_roof_45_upsidedown,2.43,0.42,2.43|wood_wall_roof_top_45,2,0.41,2|wood_wall_roof_upsidedown,2.27,0.42,1.54|wood_window,1.15,0.36,1.05|woodiron_beam,2,0.3,0.3|woodiron_pole,0.3,0.3,2|wraith,1,1,2.53|beech1,5.72,9.92,26.2|beehive,1.22,1.1,1.34|bonemass,7.6,7.6,11.35|bossstone_eikthyr,3,1.65,4.53|darkwood_raven,0.42,3.09,1.46|darkwood_roof,2,2.12,1.13|darkwood_roof_45,2,2.14,2.14|darkwood_roof_top,2,2.21,0.85|darkwood_roof_top_45,2,2.12,1.25|darkwood_wolf,0.83,2.18,1.79|fenrirhide_hanging,1.8,0.3,3|fenrirhide_hanging_door,2.4,0.3,3.2|gd_king,3.1,3.1,11|goblin_banner,2,0.3,1.3|goblin_fence,2,0.3,2.5|goblin_pole,0.3,0.3,3.5|goblin_pole_small,0.3,0.3,3.5|goblin_roof_45d,2,2.14,2.14|goblin_roof_45d_corner,2.43,2.43,2.13|goblin_roof_cap,8.59,7.89,5.94|goblin_totempole,0.42,0.45,2.43|goblin_woodwall_1m,1,0.3,2|goblin_woodwall_2m,2,0.3,2|goblin_woodwall_2m_ribs,2,0.3,2|goblinking_totemholder,0.4,0.4,0.21|greydwarf,0.8,0.8,1.8|greydwarf_shaman,1.2,1.2,2.24|greyling,0.8,0.8,1.5|iron_floor_1x1,1,1,0.1|iron_floor_2x2,2,2,0.1|iron_wall_2x2,2,0.1,2|lox_ribs,2.2,1.62,2.05|mountaingravestone01,1.05,2.37,2.63|neck,0.8,0.8,1.21|oak1,16.12,20.96,20.25|old_wood_roof,2,2.12,1.13|pickable_dragonegg,1,1,1.31|pickable_mountainremains01_buried,0.81,1.88,0.5|piece_artisanstation,2.65,1.15,2.46|piece_chest_private,0.92,0.48,0.53|piece_chest_wood,1.64,0.75,0.74|pinetree_log,0.62,0.62,7.75|rock_7,0.8,1.88,4.33|rock_destructible,2.62,2.36,2.14|rockdolmen_1,6.37,3.94,1.75|rockdolmen_2,4.47,3.96,1.56|rockdolmen_3,9.74,3.96,2.25|rockfingerbroken,12.39,14.45,32.73|root08,5.18,4.84,1.43|root11,2.27,2.81,0.51|root12,2.7,5.53,0.76|shipwreck_karve_bottomboards,3.13,9.98,0.13|shipwreck_karve_bow,4.22,7.96,2.78|shipwreck_karve_chest,1.64,0.75,0.74|shipwreck_karve_dragonhead,0.66,1.38,2.98|shipwreck_karve_stern,4.36,7.33,2.5|shipwreck_karve_sternpost,1.29,1.89,3.85|sign,1,0.09,0.55|sign_notext,1,0.09,0.55|statuecorgi,0.88,2.63,1.86|statuedeer,0.66,2.31,2.51|statuehare,1.03,2.06,1.74|statueseed,3.06,2.78,2.84|swamptree1_log,1.32,6.78,1.32|treasurechest_meadows_buried,1.64,0.75,0.74|treasurechest_mountains,1.64,0.75,0.74|treasurechest_trollcave,2.42,1.04,0.65|turf_roof,2,2.12,1.13|turf_roof_top,2,2.21,1.05|turf_roof_wall,2.31,0.4,1.43|wood_dragon1,0.58,3.34,1.46|wood_roof,2,2.12,1.13|wood_roof_45,2,2.12,2.12|wood_roof_top,2,2.21,0.91|wood_roof_top_45,2,2.12,1.21|wood_wall_half,2,0.3,1|wood_wall_quarter,1,0.3,1|wood_wall_roof_top,2,0.41,1|woodwall,2,0.3,2|ancient_skull,5.62,10.08,8.13|blackforge,3.6,1,0.91|blackforge_ext1,2.35,0.79,0.51|blackmarble_1x1,1,1,1|blackmarble_2x1x1,2,1,1|blackmarble_2x2_enforced,2,2,2|blackmarble_2x2x1,2,1,2|blackmarble_2x2x2,2,2,2|blackmarble_altar_crystal,1.23,1.23,1.67|blackmarble_altar_crystal_broken,0.72,0.72,0.76|blackmarble_arch,2,1,2|blackmarble_base_1,2,1,2|blackmarble_base_2,2,2,2|blackmarble_basecorner,2,2,2|blackmarble_column_1,1,1,1|blackmarble_column_2,1.95,1.95,1|blackmarble_column_3,2,2,8|blackmarble_creep_4x1x1,1,1,4|blackmarble_creep_4x2x1,2,1,4|blackmarble_creep_slope_inverted_1x1x2,1,1,2|blackmarble_creep_slope_inverted_2x2x1,2,2,1|blackmarble_creep_stair,2,2,1|blackmarble_floor,2,2,1|blackmarble_floor_large,8,8,2|blackmarble_floor_triangle,2,2,1|blackmarble_head_big01,2,2,2|blackmarble_head_big02,2,2,2|blackmarble_head01,1,1,1|blackmarble_head02,1,1,1|blackmarble_out_1,2,1,2|blackmarble_out_2,2,2,2|blackmarble_outcorner,2,2,2|blackmarble_pile,1.58,1.56,0.99|blackmarble_post01,1.75,1.75,5.65|blackmarble_slope_1x2,1,1,2|blackmarble_slope_inverted_1x2,1,1,2|blackmarble_stair,2,2,1|blackmarble_stair_corner,2,2,1|blackmarble_stair_corner_left,2,2,1|blackmarble_tile_floor_1x1,1,1,0.2|blackmarble_tile_floor_2x2,2,2,0.2|blackmarble_tile_wall_1x1,1,0.2,1|blackmarble_tile_wall_2x2,2,0.2,2|blackmarble_tile_wall_2x4,2,0.2,4|blackmarble_tip,1,1,2|bossstone_thequeen,7.89,2.73,8.83|bow_projectile_carapace,0.11,0.63,0.11|cauldron_ext5_mortarandpestle,1.85,0.72,0.88|chicken,1.13,1.13,1.16|cliff_mistlands1,18.59,73.01,72.65|cliff_mistlands1_creep,18.59,73.01,72.65|cliff_mistlands1_creep_frac,18.52,30.05,66.83|cliff_mistlands1_frac,18.52,30.05,66.83|cliff_mistlands2,22.4,31.04,36.96|cliff_mistlands2_frac,22.4,24.96,35.45|creepprop_egg_hanging01,1.3,1.26,2.8|creepprop_egg_hanging02,1.3,1.26,2.65|creepprop_entrance1,3.07,2.32,2.16|creepprop_entrance2,5.19,6.6,3.5|creepprop_hanging01,0.37,4.69,2.11|creepprop_wall01,2.58,0.35,3.04|darkwood_beam_26,2.19,0.4,1.36|darkwood_beam_45,2.33,0.4,2.33|dungeon_queen_door,7.99,0.8,8|dverger,0.8,0.8,1.45|dverger_demister,0.93,0.95,0.74|dverger_demister_broken,0.93,0.95,0.74|dverger_demister_large,0.93,0.95,0.74|dverger_demister_ruins,0.93,0.95,0.74|dverger_guardstone,1.03,0.78,1.61|dvergermage,0.8,0.8,1.45|dvergermagefire,0.8,0.8,1.45|dvergermageice,0.8,0.8,1.45|dvergermagesupport,0.8,0.8,1.45|dvergerstaffblocker_blockcircle,4.21,4.21,4.97|dvergerstaffblocker_blockcirclebig,11.45,11.45,5.4|dvergerstaffblocker_blockhemisphere,8.25,8.02,8.27|dvergerstaffblocker_blocku,8.69,9.36,6.08|dvergerstaffblocker_blockwall,9.63,0.21,6.08|dvergerstaffblocker_projectile,0.21,0.21,0.21|dvergerstafffire_clusterbomb_aoe,0,0,0|dvergerstafffire_fire_aoe,0,0,0|dvergerstaffheal_aoe,0,0,0|dvergerstaffnova_aoe,0,0,0|dvergerstaffsupport_aoe,0,0,0|dvergertest,0.8,0.8,1.45|dvergrprops_banner,0.3,2,0.5|dvergrprops_barrel,1.5,2,1.6|dvergrprops_bed,1,2,0.67|dvergrprops_chair,0.6,0.5,1|dvergrprops_crate,1.02,1.02,1.01|dvergrprops_crate_long,2,0.69,0.64|dvergrprops_curtain,0.3,4.36,0.79|dvergrprops_hooknchain,0.6,1.48,4.77|dvergrprops_lantern,1.09,0.3,1.18|dvergrprops_lantern_standing,0.3,0.3,0.55|dvergrprops_pickaxe,0.9,0.16,0.84|dvergrprops_shelf,1.83,0.53,1.5|dvergrprops_stool,0.5,0.5,0.4|dvergrprops_table,1.83,1.34,0.72|dvergrprops_wood_beam,6,0.8,0.8|dvergrprops_wood_floor,2,2,0.13|dvergrprops_wood_pole,0.8,0.8,4|dvergrprops_wood_stair,2,2.03,1.12|dvergrprops_wood_stake,0.43,0.43,2.98|dvergrprops_wood_stakewall,2.72,3.92,2.49|dvergrprops_wood_wall,4.4,0.8,4|dvergrtown_arch,2,1,2|dvergrtown_creep_door,5,0.45,5|dvergrtown_secretdoor,4,0.77,4|dvergrtown_slidingdoor,4,0.36,4|dvergrtown_stair_corner_wood_left,2.29,2.3,1.35|dvergrtown_wood_beam,6,0.8,0.8|dvergrtown_wood_crane,6.26,1.59,12.4|dvergrtown_wood_pole,0.8,0.8,4|dvergrtown_wood_stake,0.43,0.43,2.98|dvergrtown_wood_stakewall,2.72,3.92,2.57|dvergrtown_wood_support,6,1,4.65|dvergrtown_wood_wall01,6,0.5,5.25|dvergrtown_wood_wall02,6,0.5,5.25|dvergrtown_wood_wall03,3.91,0.79,4.02|eitrrefinery,5.59,3.1,4.33|flying_core,2.27,2.26,6.62|giant_arm,3.24,11.65,3.28|giant_brain,7.63,8.16,8.61|giant_brain_frac,7.63,8.16,8.61|giant_helmet1,10.31,13.12,9.94|giant_helmet1_destruction,10.31,13.12,9.94|giant_helmet2,10.57,5.76,9.77|giant_helmet2_destruction,10.57,5.76,9.77|giant_ribs,16.91,19.29,11.7|giant_ribs_frac,16.53,19.09,11.7|giant_skull,10.14,16.95,11.57|giant_skull_frac,10.13,16.96,11.57|giant_sword1,6.92,0.7,19|giant_sword1_destruction,6.92,0.7,19|giant_sword2,2.6,0.51,13.1|giant_sword2_destruction,2.6,0.51,13.1|gjall,7.71,8.68,6.03|hare,0.95,0.95,1.05|hen,0.68,0.68,0.87|hive,7.6,7.6,11.35|jute_carpet_blue,3,3,0.05|luredwisp,1.2,1.2,1.2|metalbar_1x2,2,1,1|mistile,1,1,1|pickable_blackcorestand,0.31,0.31,1.83|pickable_dvergerthing,1,1,1|pickable_dvergrlantern,0.28,0.28,0.5|pickable_dvergrminetreasure,1.5,1.51,0.27|pickable_dvergrstein,0.2,0.2,0.32|pickable_mushroom_jotunpuffs,0.9,0.8,1.07|pickable_mushroom_magecap,1.21,1.33,1.93|pickable_royaljelly,2.53,2.12,1.47|piece_blackmarble_bench,2.54,0.62,0.99|piece_blackmarble_table,2.45,1.33,0.82|piece_blackmarble_throne,1.39,1.09,2.97|piece_cloth_hanging_door_blue,0.22,1.37,0.35|piece_cloth_hanging_door_blue2,0.17,2.19,0.4|piece_dvergr_lantern,1.11,0.78,1.21|piece_dvergr_lantern_pole,1.21,0.5,3.44|piece_dvergr_metal_wall_2x2,2,0.1,2|piece_dvergr_pole,0.35,0.35,2|piece_dvergr_sharpstakes,2.4,3.94,1.7|piece_dvergr_spiralstair,2.29,2.3,1.35|piece_dvergr_spiralstair_right,2.29,2.3,1.35|piece_dvergr_stake_wall,2,0.78,3.39|piece_dvergr_wood_door,3.74,0.63,4.3|piece_dvergr_wood_wall,2,0.5,2|piece_groundtorch_mist,0.23,0.24,0.95|piece_magetable,2.64,2.19,2.68|piece_magetable_ext,1,1,1.08|piece_mistletoe,0.71,0.71,0.75|piece_sapcollector,1.2,0.36,2.55|piece_trap_troll,1.55,0.34,1.17|piece_turret,2.2,2.46,1.78|piece_wisplure,1,1.47,3.63|piece_xmascrown,0.23,0.85,0.85|piece_xmasgarland,0.17,2.19,0.4|rock_mistlands1,4.42,7.37,3.59|rock_mistlands1_frac,4.42,7.37,3.59|rock_mistlands2,1.92,1.93,3.86|rock1_mistlands,24,15.55,29.14|rug_hare,0.98,1.57,0.05|rug_straw,2,3,0.05|sapling_jotunpuffs,0.36,0.36,1|sapling_magecap,0.36,0.36,1|seeker,1.6,2.45,1.6|seekerbrood,1.2,1.2,1.2|seekerbrute,3.67,4.28,2.4|seekeregg,0.4,0.4,1.16|seekeregg_alwayshatch,0.4,0.4,1.16|seekerqueen,3.4,12.2,9.12|shootstump,4.25,3.8,5.69|skeleton_friendly,0.8,0.8,1.85|thehive,63.84,77.52,62.23|tick,1.2,1.2,1.2|trader_wagon_destructable,3.77,9.8,3.29|treasurechest_dvergr_loose_stone,2,2,0.5|treasurechest_dvergrtower,1.39,0.75,0.63|treasurechest_dvergrtown,1.67,0.9,0.76|triggerspawner_brood,3.67,2.86,3.23|triggerspawner_seeker,3.67,2.86,3.23|turret_projectile,0.15,1.11,0.15|waterliquid,64,64,40|wood_yggdrasil_stack,1,3,1.1|woodiron_beam_26,2.09,0.3,1.28|woodiron_beam_45,2.26,0.3,2.26|yggashoot_log,0.6,0.6,10.5|yggashoot_log_half,0.6,0.6,4.97|yggashoot_small1,0.48,0.97,1.92|yggashoot1,21.64,23.17,27.1|yggashoot2,30.32,28.07,26.65|yggashoot3,23.45,22.14,29.08|yggdrasilroot,43.9,17.71,15.1";
		}
		Dimensions = ParseSize(configDimensions.Value);
	}

	private static string Format(float f)
	{
		return f.ToString("0.##", CultureInfo.InvariantCulture);
	}

	public static void SetDimension(string value, Vector3 size)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Dimensions[value.ToLower()] = size;
		configDimensions.Value = string.Join("|", Dimensions.Select((KeyValuePair<string, Vector3> kvp) => kvp.Key + "," + Format(kvp.Value.x) + "," + Format(kvp.Value.z) + "," + Format(kvp.Value.y)));
	}

	private static void InitOther(ConfigWrapper wrapper)
	{
		string group = "7. Other";
		configEnabled = wrapper.Bind(group, "Enabled", value: true, "Whether this mod is enabled at all.");
		configDimensions = wrapper.Bind(group, "Dimensions", "", "Object dimensions.");
		configDimensions.SettingChanged += delegate
		{
			UpdateDimensions();
		};
		UpdateDimensions();
		configIgnoredIds = wrapper.BindList(group, "Ignored ids", "", "Object ids separated by , that are ignored by this mod.");
		configIgnoredIds.SettingChanged += delegate
		{
			IgnoredIds = ParseArray(configIgnoredIds.Value);
			RemoveIds = ParseArray(configIgnoredRemoveIds.Value);
			string[] removeIds3 = RemoveIds;
			string[] ignoredIds3 = IgnoredIds;
			int num3 = 0;
			string[] array5 = new string[removeIds3.Length + ignoredIds3.Length];
			string[] array6 = removeIds3;
			foreach (string text3 in array6)
			{
				array5[num3] = text3;
				num3++;
			}
			array6 = ignoredIds3;
			foreach (string text3 in array6)
			{
				array5[num3] = text3;
				num3++;
			}
			RemoveIds = array5;
		};
		IgnoredIds = ParseArray(configIgnoredIds.Value);
		configIgnoredRemoveIds = wrapper.BindList(group, "Ignored remove ids", "", "Additional ids that are ignored when removing anything.");
		configIgnoredRemoveIds.SettingChanged += delegate
		{
			RemoveIds = ParseArray(configIgnoredRemoveIds.Value);
			string[] removeIds2 = RemoveIds;
			string[] ignoredIds2 = IgnoredIds;
			int num2 = 0;
			string[] array3 = new string[removeIds2.Length + ignoredIds2.Length];
			string[] array4 = removeIds2;
			foreach (string text2 in array4)
			{
				array3[num2] = text2;
				num2++;
			}
			array4 = ignoredIds2;
			foreach (string text2 in array4)
			{
				array3[num2] = text2;
				num2++;
			}
			RemoveIds = array3;
		};
		RemoveIds = ParseArray(configIgnoredRemoveIds.Value);
		string[] removeIds = RemoveIds;
		string[] ignoredIds = IgnoredIds;
		int num = 0;
		string[] array = new string[removeIds.Length + ignoredIds.Length];
		string[] array2 = removeIds;
		foreach (string text in array2)
		{
			array[num] = text;
			num++;
		}
		array2 = ignoredIds;
		foreach (string text in array2)
		{
			array[num] = text;
			num++;
		}
		RemoveIds = array;
		configHammerTools = wrapper.BindList(group, "Hammer tools", "hammer", "List of hammers.");
		configHammerTools.SettingChanged += delegate
		{
			UpdateTools();
		};
		UpdateTools();
		configMirrorFlip = wrapper.Bind(group, "Mirror flip", "woodwall", "Object ids that get flipped instead of rotated when mirrored.");
		configMirrorFlip.SettingChanged += delegate
		{
			UpdateMirrorFlip();
		};
		UpdateMirrorFlip();
	}

	public static string ModifierKey1()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (commandModifier1 == null)
		{
			return "leftalt";
		}
		return ConfigWrapper.GetKeys(commandModifier1.Value);
	}

	public static string ModifierKey2()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (commandModifier2 == null)
		{
			return "leftcontrol";
		}
		return ConfigWrapper.GetKeys(commandModifier2.Value);
	}

	private static void InitTools(ConfigWrapper wrapper)
	{
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		string group = "5. Tools";
		configToolsEnabled = wrapper.Bind(group, "Tools enabled", value: true, "Enables the tools.");
		configShowCommandValues = wrapper.Bind(group, "Show command values", value: false, "Always shows the command in the tool descriptions.");
		configShapeCircle = wrapper.Bind(group, "Shape circle", value: true, "Enables circle shape for commands.");
		configShapeRing = wrapper.Bind(group, "Shape ring", value: false, "Enables ring shape for commands.");
		configShapeRectangle = wrapper.Bind(group, "Shape rectangle", value: true, "Enables rectangle shape for commands.");
		configShapeSquare = wrapper.Bind(group, "Shape square", value: true, "Enables square shape for commands.");
		configShapeFrame = wrapper.Bind(group, "Shape frame", value: false, "Enables frame shape for commands.");
		configToolIncludedIds = wrapper.Bind(group, "Tool included ids", "", "Object ids when using <include> in tools.");
		configToolIgnoredIds = wrapper.Bind(group, "Tool ignored ids", "", "Object ids when using <ignore> in tools.");
		commandHeightAmount = wrapper.Bind(group, "Command height amount", "0.1", "Meters to move.");
		group = "3. Binds";
		commandModifier1 = wrapper.Bind<KeyboardShortcut>(group, "Command modifier 1", new KeyboardShortcut((KeyCode)308, Array.Empty<KeyCode>()), "");
		commandModifier2 = wrapper.Bind<KeyboardShortcut>(group, "Command modifier 2", new KeyboardShortcut((KeyCode)306, Array.Empty<KeyCode>()), "");
		shapeKey = wrapper.BindCommand("tool_shape", group, "Change shape", new KeyboardShortcut((KeyCode)113, Array.Empty<KeyCode>()), "Changes the selection shape.", "command");
		commandRadius = wrapper.BindWheelCommand(() => "hammer_zoom_x " + (Selection.Get().TerrainGrid ? "0.5" : "1"), group, "Command radius (mouse wheel)", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Changes the command radius.", "command");
		commandDepth = wrapper.BindWheelCommand(() => "hammer_zoom_z " + (Selection.Get().TerrainGrid ? "0.5" : "1"), group, "Command depth (mouse wheel)", new KeyboardShortcut((KeyCode)304, (KeyCode[])(object)new KeyCode[1] { (KeyCode)308 }), "Changes the command rectangle depth.", "command");
		commandHeight = wrapper.BindWheelCommand(() => "hammer_zoom_y " + commandHeightAmount.Value, group, "Command height (mouse wheel)", new KeyboardShortcut((KeyCode)304, Array.Empty<KeyCode>()), "Changes the command height.", "command");
		commandRotate = wrapper.BindWheelCommand("hammer_rotate", group, "Command rotation (mouse wheel)", new KeyboardShortcut((KeyCode)304, (KeyCode[])(object)new KeyCode[1] { (KeyCode)306 }), "Changes the command rotation.", "command");
	}

	public static void InitVisuals(ConfigWrapper wrapper)
	{
		Wrapper = wrapper;
		string group = "2. Visual";
		hideEffects = wrapper.Bind(group, "No effects", value: false, "Hides visual effects of building, repairing and destroying.");
		hidePlacementMarker = wrapper.Bind(group, "No placement marker", value: false, "Hides the yellow placement marker (also affects Gizmo mod).");
		hideSupportColor = wrapper.Bind(group, "No support indicator", value: false, "Hides the color that shows support.");
		hidePieceHealth = wrapper.Bind(group, "No health indicator", value: false, "Hides the piece health bar.");
	}
}
