using System;
using System.IO;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HarmonyLib;
using ServerDevcommands;
using Service;

namespace InfinityHammer;

[BepInPlugin("infinity_hammer", "Infinity Hammer", "1.91")]
[BepInDependency(/*Could not decode attribute arguments.*/)]
[BepInDependency(/*Could not decode attribute arguments.*/)]
[BepInDependency("server_devcommands", "1.112")]
[BepInDependency("world_edit_commands", "1.81")]
public class InfinityHammer : BaseUnityPlugin
{
	public const string GUID = "infinity_hammer";

	public const string NAME = "Infinity Hammer";

	public const string VERSION = "1.91";

	public static bool StructureTweaks;

	public static ConfigWrapper Wrapper;

	public void Awake()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		new Harmony("infinity_hammer").PatchAll();
		Wrapper = new ConfigWrapper("hammer_config", ((BaseUnityPlugin)this).Config);
		Configuration.Init(Wrapper);
		if (File.Exists(((BaseUnityPlugin)this).Config.ConfigFilePath.Replace("infinity_hammer", "infinity_tools")))
		{
			Migrate();
		}
		try
		{
			SetupWatcher();
		}
		catch
		{
		}
	}

	private void OnDestroy()
	{
		((BaseUnityPlugin)this).Config.Save();
	}

	private void Migrate()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		ConfigFile configFile = new ConfigFile(((BaseUnityPlugin)this).Config.ConfigFilePath.Replace("infinity_hammer", "infinity_tools"), false);
		ConfigurationLegacy.Init(new ConfigWrapper("tool_config", configFile));
		Configuration.configShapeCircle.Value = ConfigurationLegacy.configShapeCircle.Value;
		Configuration.configShapeRing.Value = ConfigurationLegacy.configShapeRing.Value;
		Configuration.configShapeSquare.Value = ConfigurationLegacy.configShapeSquare.Value;
		Configuration.configShapeRectangle.Value = ConfigurationLegacy.configShapeRectangle.Value;
		Configuration.configShapeFrame.Value = ConfigurationLegacy.configShapeFrame.Value;
		Configuration.configShowCommandValues.Value = ConfigurationLegacy.configShowCommandValues.Value;
		Configuration.commandHeightAmount.Value = ConfigurationLegacy.commandHeightAmount.Value;
		Configuration.commandModifier1.Value = ConfigurationLegacy.commandModifier1.Value;
		Configuration.commandModifier2.Value = ConfigurationLegacy.commandModifier2.Value;
		Configuration.shapeKey.Value = ConfigurationLegacy.shapeKey.Value;
		Configuration.commandRadius.Value = ConfigurationLegacy.commandRadius.Value;
		Configuration.commandRotate.Value = ConfigurationLegacy.commandRotate.Value;
		Configuration.commandDepth.Value = ConfigurationLegacy.commandDepth.Value;
		Configuration.commandHeight.Value = ConfigurationLegacy.commandHeight.Value;
		((BaseUnityPlugin)this).Config.Save();
		File.Delete(((BaseUnityPlugin)this).Config.ConfigFilePath.Replace("infinity_hammer", "infinity_tools"));
	}

	private void SetupWatcher()
	{
		FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(((BaseUnityPlugin)this).Config.ConfigFilePath), Path.GetFileName(((BaseUnityPlugin)this).Config.ConfigFilePath));
		fileSystemWatcher.NotifyFilter = NotifyFilters.Size;
		fileSystemWatcher.Changed += ReadConfigValues;
		fileSystemWatcher.IncludeSubdirectories = true;
		fileSystemWatcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
		fileSystemWatcher.EnableRaisingEvents = true;
	}

	private void ReadConfigValues(object sender, FileSystemEventArgs e)
	{
		if (!File.Exists(((BaseUnityPlugin)this).Config.ConfigFilePath))
		{
			return;
		}
		try
		{
			((BaseUnityPlugin)this).Config.Reload();
		}
		catch
		{
			Log.Error("There was an issue loading your " + ((BaseUnityPlugin)this).Config.ConfigFilePath);
			Log.Error("Please check your config entries for spelling and format!");
		}
	}

	public void Start()
	{
		if (Chainloader.PluginInfos.TryGetValue("bruce.valheim.comfymods.gizmo", out var value))
		{
			PlaceRotation.Comfy = ((object)value.Instance).GetType().Assembly;
		}
		if (Chainloader.PluginInfos.TryGetValue("m3to.mods.GizmoReloaded", out value))
		{
			PlaceRotation.Reloaded = ((object)value.Instance).GetType().Assembly;
		}
		StructureTweaks = Chainloader.PluginInfos.ContainsKey("structure_tweaks");
		PermissionApi.Subscribe((Action)UpdateBuildMenu);
	}

	public void LateUpdate()
	{
		Player_ManualUpdate.Update();
	}

	private void UpdateBuildMenu()
	{
		Player localPlayer = Player.m_localPlayer;
		if (localPlayer != null)
		{
			localPlayer.UpdateAvailablePiecesList();
		}
	}
}
