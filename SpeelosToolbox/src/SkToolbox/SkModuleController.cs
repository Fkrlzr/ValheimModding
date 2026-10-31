using System;
using System.Collections.Generic;
using SkToolbox.SkModules;
using SkToolbox.Utility;
using UnityEngine;

namespace SkToolbox;

public class SkModuleController : MonoBehaviour
{
	private static System.Version SkMainVersion = new System.Version(1, 1, 3);

	internal SkUtilities.Status SkMainStatus;

	private bool FirstLoad = true;

	private bool NeedLoadModules = true;

	private bool NeedRetry;

	private bool ErrorMonitor;

	private int RetryCount = 1;

	private int RetryCountMax = 3;

	private SkMenuController menuController;

	private ModConsole moduleConsole;

	private ModPlayer modulePlayer;

	private ModWorld moduleWorld;

	private ModGive moduleGive;

	private ModSpawn moduleSpawn;

	private ModEdit moduleEdit;

	private ModTooler moduleTooler;

	private List<SkBaseModule> MenuOptions { get; set; } = new List<SkBaseModule>();


	private List<SkBaseModule> RetryModule { get; set; } = new List<SkBaseModule>();


	public void Start()
	{
		SkMainStatus = SkUtilities.Status.Loading;
		SkUtilities.Logz(new string[2] { "TOOLBOX", "NOTIFY" }, new string[2] { "LOADING...", "MODULES LOADING..." }, (LogType)3);
		BeginMainMenu();
		menuController = ((Component)this).GetComponent<SkMenuController>();
		if (MenuOptions.Count > 0 && (Object)(object)menuController != (Object)null)
		{
			SkMainStatus = SkUtilities.Status.Loading;
		}
		else
		{
			SkMainStatus = SkUtilities.Status.Error;
		}
		Init();
	}

	public void Update()
	{
		if (!FirstLoad)
		{
			if (SkMainStatus == SkUtilities.Status.Loading && NeedLoadModules && !NeedRetry)
			{
				foreach (SkBaseModule menuOption in MenuOptions)
				{
					SkUtilities.Logz(new string[3] { "TOOLBOX", "MODULE", "NOTIFY" }, new string[2]
					{
						"NAME: " + menuOption.ModuleName.ToUpper(),
						"STATUS: " + menuOption.ModuleStatus.ToString().ToUpper()
					}, (LogType)3);
					if (menuOption.ModuleStatus != SkUtilities.Status.Ready)
					{
						NeedRetry = true;
						RetryModule.Add(menuOption);
					}
				}
				if (!NeedRetry)
				{
					SkMainStatus = SkUtilities.Status.Ready;
					ErrorMonitor = true;
					RetryCount = 1;
				}
				if (SkMainStatus == SkUtilities.Status.Ready && MenuOptions.Count > 0)
				{
					NeedLoadModules = false;
					SkUtilities.Logz(new string[2] { "TOOLBOX", "NOTIFY" }, new string[2]
					{
						MenuOptions.Count + " MODULES LOADED",
						"TOOLBOX READY."
					}, (LogType)3);
				}
				else if (SkMainStatus == SkUtilities.Status.Error || MenuOptions.Count <= 0)
				{
					SkUtilities.Logz(new string[2] { "TOOLBOX", "NOTIFY" }, new string[2]
					{
						MenuOptions.Count + " MODULES LOADED",
						"TOOLBOX FAILED TO LOAD MODULES."
					}, (LogType)0);
				}
			}
			else if (SkMainStatus == SkUtilities.Status.Loading && NeedRetry)
			{
				if (RetryCount < RetryCountMax + 1)
				{
					for (int i = 0; i < RetryModule?.Count; i++)
					{
						SkUtilities.Logz(new string[4]
						{
							"TOOLBOX",
							"MODULE",
							"NOTIFY",
							"RECHECK " + RetryCount
						}, new string[2]
						{
							"NAME: " + RetryModule[i].ModuleName.ToUpper(),
							"STATUS: " + RetryModule[i].ModuleStatus.ToString().ToUpper()
						}, (LogType)3);
						if (RetryModule[i].ModuleStatus != SkUtilities.Status.Ready)
						{
							SkMainStatus = SkUtilities.Status.Loading;
							NeedRetry = true;
						}
						else if (RetryModule[i].ModuleStatus == SkUtilities.Status.Ready)
						{
							RetryModule.Remove(RetryModule[i]);
							if (RetryModule.Count == 0)
							{
								SkMainStatus = SkUtilities.Status.Ready;
								break;
							}
						}
					}
					RetryCount++;
				}
				if (MenuOptions.Count <= 0)
				{
					SkMainStatus = SkUtilities.Status.Error;
				}
				if (SkMainStatus == SkUtilities.Status.Ready)
				{
					ErrorMonitor = true;
					RetryCount = 1;
					SkUtilities.Logz(new string[2] { "TOOLBOX", "NOTIFY" }, new string[2]
					{
						MenuOptions.Count + " MODULES LOADED",
						"TOOLBOX READY."
					}, (LogType)3);
				}
				else if (RetryCount >= RetryCountMax + 1)
				{
					SkUtilities.Logz(new string[2] { "TOOLBOX", "NOTIFY" }, new string[2] { "MODULE NOT MOVING TO READY STATUS.", "UNLOADING THE MODULE(S)." }, (LogType)2);
					foreach (SkBaseModule item in RetryModule)
					{
						if (item.ModuleStatus != SkUtilities.Status.Ready)
						{
							item.RemoveModule();
							MenuOptions.Remove(item);
						}
					}
					RetryModule.Clear();
					NeedRetry = false;
					SkMainStatus = SkUtilities.Status.Loading;
					menuController.UpdateMenuOptions(MenuOptions);
				}
			}
		}
		else
		{
			FirstLoad = false;
		}
		if (ErrorMonitor)
		{
			for (int j = 0; j < MenuOptions?.Count; j++)
			{
				SkBaseModule skBaseModule = MenuOptions[j];
				if (skBaseModule != null && skBaseModule.ModuleStatus == SkUtilities.Status.Error && !RetryModule.Contains(MenuOptions[j]))
				{
					SkUtilities.Logz(new string[2] { "TOOLBOX", "NOTIFY" }, new string[2]
					{
						"MODULE IN ERROR STATUS.",
						"CHECKING MODULE: " + MenuOptions[j].ModuleName.ToUpper()
					}, (LogType)2);
					RetryModule.Add(MenuOptions[j]);
					continue;
				}
				SkBaseModule skBaseModule2 = MenuOptions[j];
				if (skBaseModule2 != null && skBaseModule2.ModuleStatus == SkUtilities.Status.Unload)
				{
					SkUtilities.Logz(new string[2] { "TOOLBOX", "NOTIFY" }, new string[1] { "MODULE READY TO UNLOAD. UNLOADING MODULE: " + MenuOptions[j].ModuleName.ToUpper() }, (LogType)2);
					MenuOptions[j].RemoveModule();
					MenuOptions.Remove(MenuOptions[j]);
					menuController.UpdateMenuOptions(MenuOptions);
				}
			}
			List<SkBaseModule> retryModule = RetryModule;
			if (retryModule != null && retryModule.Count > 0 && RetryCount < RetryCountMax + 1)
			{
				for (int k = 0; k < RetryModule.Count; k++)
				{
					if (RetryModule[k].ModuleStatus == SkUtilities.Status.Ready)
					{
						SkUtilities.Logz(new string[2] { "TOOLBOX", "NOTIFY" }, new string[2]
						{
							"MODULE READY.",
							"MODULE: " + RetryModule[k].ModuleName.ToUpper()
						}, (LogType)3);
						RetryModule.Remove(RetryModule[k]);
						if (RetryModule.Count == 0)
						{
							break;
						}
					}
				}
				RetryCount++;
			}
			else
			{
				List<SkBaseModule> retryModule2 = RetryModule;
				if (retryModule2 != null && retryModule2.Count > 0 && RetryCount >= RetryCountMax + 1)
				{
					foreach (SkBaseModule item2 in RetryModule)
					{
						if (item2.ModuleStatus != SkUtilities.Status.Ready)
						{
							SkUtilities.Logz(new string[2] { "TOOLBOX", "NOTIFY" }, new string[2]
							{
								"COULD NOT RESOLVE ERROR.",
								"UNLOADING THE MODULE: " + item2.ModuleName.ToUpper()
							}, (LogType)2);
							item2.RemoveModule();
							MenuOptions.Remove(item2);
						}
					}
					RetryModule.Clear();
					RetryCount = 1;
					menuController.UpdateMenuOptions(MenuOptions);
					if (MenuOptions.Count == 0)
					{
						SkMainStatus = SkUtilities.Status.Error;
						SkUtilities.Logz(new string[2] { "TOOLBOX", "NOTIFY" }, new string[2] { "NO MODULES LOADED.", "TOOLBOX ENTERING ERROR STATE." }, (LogType)0);
					}
				}
			}
		}
		OnUpdate();
	}

	internal List<SkBaseModule> GetOptions()
	{
		return MenuOptions;
	}

	public void BeginMainMenu()
	{
		((Component)this).gameObject.AddComponent<SkEsp>();
		((Component)this).gameObject.AddComponent<SkPlayerTools>();
		moduleConsole = ((Component)this).gameObject.AddComponent<ModConsole>();
		modulePlayer = ((Component)this).gameObject.AddComponent<ModPlayer>();
		moduleWorld = ((Component)this).gameObject.AddComponent<ModWorld>();
		moduleGive = ((Component)this).gameObject.AddComponent<ModGive>();
		moduleSpawn = ((Component)this).gameObject.AddComponent<ModSpawn>();
		moduleConsole.CallerEntry = new SkMenuItem("System", (Action)delegate
		{
			moduleConsole.ShowGrid();
		}, "Display settings, and the toolbox itself");
		modulePlayer.CallerEntry = new SkMenuItem("Player", (Action)delegate
		{
			modulePlayer.ShowGrid();
		}, "Food, healing, cheats and readouts for your character");
		moduleWorld.CallerEntry = new SkMenuItem("World", (Action)delegate
		{
			moduleWorld.ShowGrid();
		}, "Terrain, weather, time, wind, events and the map");
		moduleGive.CallerEntry = new SkMenuItem("Give", (Action)delegate
		{
			moduleGive.ShowGrid();
		}, "Every item in the game");
		moduleSpawn.CallerEntry = new SkMenuItem("Spawn", (Action)delegate
		{
			moduleSpawn.ShowGrid();
		}, "Every creature in the game");
		MenuOptions.Add(modulePlayer);
		MenuOptions.Add(moduleGive);
		MenuOptions.Add(moduleSpawn);
		MenuOptions.Add(moduleWorld);
		moduleTooler = ((Component)this).gameObject.AddComponent<ModTooler>();
		moduleTooler.Player = modulePlayer;
		moduleTooler.World = moduleWorld;
		moduleTooler.CallerEntry = new SkMenuItem("ValheimTooler", (Action)delegate
		{
			moduleTooler.ShowGrid();
		}, "Every feature brought over from ValheimTooler");
		MenuOptions.Add(moduleTooler);
		// World Edit Commands and Infinity Hammer are optional: the Edit menu only exists when one of them is loaded.
		if (EditCompat.Any)
		{
			moduleEdit = ((Component)this).gameObject.AddComponent<ModEdit>();
			moduleEdit.CallerEntry = new SkMenuItem("Edit", (Action)delegate
			{
				moduleEdit.ShowGrid();
			}, "Terraforming, object editing and building tools from World Edit Commands and Infinity Hammer");
			MenuOptions.Add(moduleEdit);
		}
		MenuOptions.Add(moduleConsole);
	}

	public static T ParseEnum<T>(string value)
	{
		return (T)Enum.Parse(typeof(T), value, ignoreCase: true);
	}

	private void Init()
	{
	}

	private void OnUpdate()
	{
	}

	public void OnGUI()
	{
	}
}
