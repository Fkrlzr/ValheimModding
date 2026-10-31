using System.IO;
using BepInEx;
using BepInEx.Bootstrap;
using EWP;
using HarmonyLib;
using Service;
using UnityEngine;

namespace ServerDevcommands;

[BepInDependency(/*Could not decode attribute arguments.*/)]
[BepInDependency(/*Could not decode attribute arguments.*/)]
[BepInPlugin("server_devcommands", "Server Devcommands", "1.116")]
public class ServerDevcommands : BaseUnityPlugin
{
	public const string GUID = "server_devcommands";

	public const string NAME = "Server Devcommands";

	public const string VERSION = "1.116";

	public const string COMFY_GIZMO_GUID = "bruce.valheim.comfymods.gizmo";

	public const string RELOADED_GIZMO_GUID = "m3to.mods.GizmoReloaded";

	public void Awake()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		Log.Init(((BaseUnityPlugin)this).Logger);
		Settings.Init(((BaseUnityPlugin)this).Config);
		new Harmony("server_devcommands").PatchAll();
		try
		{
			SetupWatcher();
			PermissionLoader.SetupWatcher();
		}
		catch
		{
		}
	}

	public void Start()
	{
		if (Chainloader.PluginInfos.TryGetValue("bruce.valheim.comfymods.gizmo", out var value))
		{
			ComfyGizmoPatcher.DoPatching(((object)value.Instance).GetType().Assembly);
		}
		if (Chainloader.PluginInfos.TryGetValue("m3to.mods.GizmoReloaded", out value))
		{
			GizmoReloadedPatcher.DoPatching(((object)value.Instance).GetType().Assembly);
		}
		Api.RegisterGroupHandler("serverdevcommands", PermissionApi.HasGroup);
	}

	public void LateUpdate()
	{
		MultiCommands.Execute(Time.deltaTime);
		MouseWheelBinding.Execute(ZInput.GetMouseScrollWheel() * 20f);
		if (AliasManager.ToBeSaved)
		{
			AliasManager.ToFile();
		}
		if (BindManager.ToBeSaved)
		{
			BindManager.ToFile();
		}
		if (!Object.op_Implicit((Object)(object)ZNet.instance))
		{
			ParameterInfo.SetServerLocationIds(null);
			ParameterInfo.SetServerVegetationIds(null);
		}
		CommandInputResolver.CheckActiveRequest();
	}

	private void OnDestroy()
	{
		((BaseUnityPlugin)this).Config.Save();
	}

	private void SetupWatcher()
	{
		FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(((BaseUnityPlugin)this).Config.ConfigFilePath), Path.GetFileName(((BaseUnityPlugin)this).Config.ConfigFilePath));
		fileSystemWatcher.Changed += ReadConfigValues;
		fileSystemWatcher.Created += ReadConfigValues;
		fileSystemWatcher.Renamed += ReadConfigValues;
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
}
