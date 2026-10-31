using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using SkToolbox.Utility;
using UnityEngine;

namespace SkToolbox.SkModules;

internal class ModWorld : SkBaseModule, IModule
{
	private int radius = 5;

	private int height = 5;

	private int timeOfDay = 50;

	private int windAngle;

	private int windIntensity = 50;

	private static readonly Func<SkMenuController.SkForm, string> ConfirmRequired = delegate(SkMenuController.SkForm f)
	{
		SkMenuController.SkFormField skFormField = f.Field("confirm");
		return (skFormField == null || skFormField.BoolValue) ? null : "Switch the confirmation to ON first.";
	};

	private static MethodInfo exploreAround;

	public ModWorld()
	{
		ModuleName = "World";
		Loading();
	}

	public void Start()
	{
		BeginMenu();
		Ready();
	}

	public void BeginMenu()
	{
		base.MenuOptions = new SkMenu();
	}

	private static void Run(string command)
	{
		SkCommandProcessor.ProcessCommand(command, SkCommandProcessor.LogTo.Chat);
	}

	private static string Fraction(int percent)
	{
		return ((float)percent / 100f).ToString("0.00", CultureInfo.InvariantCulture);
	}

	private static SkMenuController.SkGridItem Cell(string label, string tip, Sprite icon, Action run, SkMenuController.SkScope scope)
	{
		return Cell(null, label, tip, icon, run, scope);
	}

	private static SkMenuController.SkGridItem Cell(string section, string label, string tip, Sprite icon, Action run, SkMenuController.SkScope scope)
	{
		return new SkMenuController.SkGridItem
		{
			Name = label,
			Display = label,
			Tip = label + "  -  " + tip,
			Icon = icon,
			Section = section,
			Scope = scope,
			OnClick = delegate
			{
				run();
			}
		};
	}

	private static SkMenuController.SkGridItem Toggle(string section, string label, string tip, Sprite icon, Action run, Func<bool> isOn, SkMenuController.SkScope scope)
	{
		SkMenuController.SkGridItem skGridItem = Cell(section, label, tip, icon, run, scope);
		skGridItem.IsOn = isOn;
		return skGridItem;
	}

	private static bool IsWorldHost()
	{
		if ((Object)(object)ZNet.instance != (Object)null)
		{
			return ZNet.instance.IsServer();
		}
		return false;
	}

	private static void RunEvent(string command, string done)
	{
		if (!IsWorldHost())
		{
			SkCommandProcessor.Notify("Only the host or the server can change events.");
		}
		else
		{
			SkRun.CmdNotify(command, done);
		}
	}

	private SkMenuController.SkForm NewForm(string title, string note)
	{
		return new SkMenuController.SkForm
		{
			Title = title,
			Note = note
		};
	}

	private static SkMenuController.SkFormField Choice(string id, string label, List<string> options, List<string> labels, int height)
	{
		SkMenuController.SkFormField skFormField = new SkMenuController.SkFormField
		{
			Id = id,
			Label = label,
			Kind = SkMenuController.SkFieldKind.Choice,
			Height = height
		};
		if (options != null)
		{
			skFormField.Options.AddRange(options);
		}
		if (labels != null)
		{
			skFormField.OptionLabels.AddRange(labels);
		}
		return skFormField;
	}

	private static SkMenuController.SkFormField Slider(string id, string label, int min, int max, int value)
	{
		return new SkMenuController.SkFormField
		{
			Id = id,
			Label = label,
			Kind = SkMenuController.SkFieldKind.IntSlider,
			Min = min,
			Max = max,
			IntValue = value
		};
	}

	private static SkMenuController.SkFormField TextBox(string id, string label, string value)
	{
		return new SkMenuController.SkFormField
		{
			Id = id,
			Label = label,
			Kind = SkMenuController.SkFieldKind.Text,
			TextValue = (value ?? "")
		};
	}

	private static SkMenuController.SkFormField ConfirmBox(string label)
	{
		return new SkMenuController.SkFormField
		{
			Id = "confirm",
			Label = label,
			Kind = SkMenuController.SkFieldKind.Toggle
		};
	}

	private static List<string> EnumNames(Type type, params string[] skip)
	{
		List<string> list = new List<string>();
		string[] names = Enum.GetNames(type);
		foreach (string text in names)
		{
			bool flag = false;
			foreach (string b in skip)
			{
				if (string.Equals(text, b, StringComparison.Ordinal))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(text);
			}
		}
		return list;
	}

	internal void ShowGrid()
	{
		List<SkMenuController.SkGridItem> list = new List<SkMenuController.SkGridItem>();
		list.Add(Cell("Terrain", "Terrain Tools", "Level, raise, dig or undo terrain around you", SkIcons.First("Hoe"), ShowTerrain, SkMenuController.SkScope.Server));
		list.Add(Cell("Building", "Repair Nearby", "Repair every building piece around you", SkIcons.First("Hammer"), ShowRepairForm, SkMenuController.SkScope.Server));
		list.Add(Cell("Building", "Refuel Fires", "Top up every torch, fire and hearth around you", SkIcons.First("Resin", "Coal"), ShowRefuelForm, SkMenuController.SkScope.Server));
		list.Add(Cell("Terrain", "Optimize Terrain", "Compact old terrain edits to help performance", SkIcons.First("PickaxeIron", "PickaxeStone", "PickaxeAntler"), delegate
		{
			Run("/optterrain");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Time & Weather", "Weather", "Force a weather type, or hand control back to the game", SkIcons.First("Thunderstone", "Ruby"), ShowWeather, SkMenuController.SkScope.Client));
		list.Add(Cell("Time & Weather", "Time of Day", "Set and lock the time, or unlock it", SkIcons.First("SurtlingCore", "Coins"), ShowTime, SkMenuController.SkScope.Client));
		list.Add(Cell("Time & Weather", "Wind", "Set wind direction and strength, or reset it", SkIcons.First("Feathers"), ShowWind, SkMenuController.SkScope.Client));
		list.Add(Cell("Time & Weather", "Events", "Start or stop a random event", SkIcons.First("TrophyEikthyr", "Wishbone"), ShowEvents, SkMenuController.SkScope.HostOnly));
		list.Add(Cell("Time & Weather", "Sleep", "Skip straight to the next morning", SkIcons.First("DeerHide", "LeatherScraps"), delegate
		{
			SkRun.CmdNotify("sleep", "Skipped to morning.");
		}, SkMenuController.SkScope.Admin));
		list.Add(Cell("Time & Weather", "Skip Time", "Jump forward by a number of in-game hours", SkIcons.First("Coins", "Ruby"), ShowSkipTimeForm, SkMenuController.SkScope.Admin));
		list.Add(Cell("Time & Weather", "Game Speed", "Speed the whole world up or slow it down", SkIcons.First("GreydwarfEye", "SurtlingCore"), ShowTimescaleForm, SkMenuController.SkScope.Client));
		list.Add(Cell("Rules", "Difficulty", "Scale enemies as if a set number of players were nearby", SkIcons.First("TrophyDeer", "Coins"), ShowDifficultyForm, SkMenuController.SkScope.Admin));
		list.Add(Toggle("Rules", "No Spawn", "Stop creatures spawning naturally", SkIcons.First("TrophyGreydwarf", "GreydwarfEye"), delegate
		{
			SkRun.CmdNotify("nospawn", "Toggled natural spawning.");
		}, () => SpawnSystem.m_nospawn, SkMenuController.SkScope.Client));
		list.Add(Toggle("Rules", "No Map", "Disable the map for this character", SkIcons.First("Chain", "LeatherScraps"), delegate
		{
			SkRun.CmdNotify("nomap", "Toggled the map.");
		}, () => (Object)(object)ZoneSystem.instance != (Object)null && ZoneSystem.instance.GetGlobalKey((GlobalKeys)31), SkMenuController.SkScope.Client));
		list.Add(Toggle("Rules", "No Portals", "Disable portals for the world", SkIcons.First("SurtlingCore"), delegate
		{
			SkRun.CmdNotify("noportals", "Toggled portals.");
		}, () => (Object)(object)ZoneSystem.instance != (Object)null && ZoneSystem.instance.GetGlobalKey((GlobalKeys)32), SkMenuController.SkScope.Server));
		list.Add(Cell("Rules", "World Modifier", "Change one world modifier, the same values the world settings screen offers", SkIcons.First("Bronze", "Copper"), ShowWorldModifierForm, SkMenuController.SkScope.Admin));
		list.Add(Cell("Rules", "World Preset", "Apply a whole difficulty preset, replacing every modifier", SkIcons.First("Iron", "Bronze"), ShowWorldPresetForm, SkMenuController.SkScope.Admin));
		list.Add(Cell("Progression", "Set Key", "Mark a boss as defeated or set another world key", SkIcons.First("CryptKey", "Wishbone"), ShowSetKeyForm, SkMenuController.SkScope.Admin));
		list.Add(Cell("Progression", "Remove Key", "Clear a world key that is currently set", SkIcons.First("Flint", "Stone"), ShowRemoveKeyForm, SkMenuController.SkScope.Admin));
		list.Add(Cell("Progression", "List Keys", "Show every key set on this world and character", SkIcons.First("Coins"), delegate
		{
			SkRun.Show("World Keys", "listkeys", "No keys are set.");
		}, SkMenuController.SkScope.Admin));
		list.Add(Cell("Progression", "Reset World Keys", "Put every world modifier back to its default", SkIcons.First("Ruby"), ShowResetWorldKeysForm, SkMenuController.SkScope.Admin));
		list.Add(Cell("Progression", "Player Keys", "Add or remove a key on your character rather than the world", SkIcons.First("Amber", "Coins"), ShowPlayerKeyForm, SkMenuController.SkScope.Client));
		list.Add(Cell("Travel", "Go To", "Teleport to a map coordinate", SkIcons.First("SurtlingCore", "Thunderstone"), ShowGoToForm, SkMenuController.SkScope.HostOnly));
		list.Add(Cell("Travel", "Teleport to Player", "Teleport next to another player on the server", SkIcons.First("Ruby", "Coins", "SurtlingCore"), ShowTeleportToPlayerForm, SkMenuController.SkScope.Client));
		list.Add(Cell("Travel", "Bring Player", "Pull one chosen player to where you stand, without moving anyone else", SkIcons.First("Chain", "Ruby", "Coins"), ShowBringPlayerForm, SkMenuController.SkScope.Server));
		list.Add(Cell("Travel", "Recall Players", "Pull every other player to you", SkIcons.First("Chain", "LeatherScraps"), delegate
		{
			SkRun.CmdNotify("recall", "Recalled other players.");
		}, SkMenuController.SkScope.HostOnly));
		list.Add(Cell("Travel", "Find", "Ping loaded objects and locations on the map, or teleport to the closest", SkIcons.First("Wishbone"), ShowFindForm, SkMenuController.SkScope.Client));
		list.Add(Cell("Travel", "Find Biome", "Mark a biome on the map, or teleport to it", SkIcons.First("Raspberry", "Blueberries"), ShowFindBiomeForm, SkMenuController.SkScope.Client));
		list.Add(Cell("Travel", "List Locations", "Show the loaded locations around you", SkIcons.First("Stone", "Wood"), delegate
		{
			SkRun.Show("Loaded Locations", "printlocations", "No locations are loaded.");
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("Travel", "Reveal Map", "Explore the entire minimap", SkIcons.First("Torch"), delegate
		{
			Run("/revealmap");
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("Travel", "Reveal Around Me", "Explore the map out to a distance you choose, rather than all of it", SkIcons.First("Coal", "Resin"), ShowRevealForm, SkMenuController.SkScope.Client));
		list.Add(Cell("Travel", "Reset Map", "Erase map exploration", SkIcons.First("Coins", "Ruby"), delegate
		{
			Run("/resetmap");
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("Travel", "List Portals", "Print every portal tag", SkIcons.First("GreydwarfEye", "SurtlingCore"), delegate
		{
			Run("/portals");
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("Travel", "Show Seed", "Print the world seed", SkIcons.First("Barley", "Carrot"), delegate
		{
			Run("/seed");
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("Cleanup", "Kill Enemies", "Kill every hostile creature nearby. Leaves their nests alone", SkIcons.First("SwordIron", "SwordBronze", "AxeStone"), delegate
		{
			SkRun.CmdNotify("killenemycreatures", "Killed nearby enemies.");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Cleanup", "Kill Enemies & Nests", "Also destroys every creature spawner in every loaded zone, permanently", SkIcons.First("PickaxeIron", "SledgeStagbreaker", "Club"), ShowKillSpawnersForm, SkMenuController.SkScope.Server));
		list.Add(Cell("Cleanup", "Kill Tame", "Kill every tamed creature nearby", SkIcons.First("Carrot", "Raspberry"), delegate
		{
			SkRun.CmdNotify("killtame", "Killed nearby tame creatures.");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Cleanup", "Remove Drops", "Clear dropped items in the area", SkIcons.First("Wood"), delegate
		{
			Run("/removedrops");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Cleanup", "Remove Fish", "Clear fish in the area", SkIcons.First("FishRaw", "FishingRod"), delegate
		{
			SkRun.CmdNotify("removefish", "Removed nearby fish.");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Cleanup", "Remove Birds", "Clear birds in the area", SkIcons.First("Feathers"), delegate
		{
			SkRun.CmdNotify("removebirds", "Removed nearby birds.");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Cleanup", "Stop Fire", "Put out spreading fire and its smoke", SkIcons.First("Resin", "Coal"), delegate
		{
			SkRun.CmdNotify("stopfire", "Put out spreading fires.");
		}, SkMenuController.SkScope.HostOnly));
		list.Add(Cell("Cleanup", "Stop Smoke", "Clear lingering smoke", SkIcons.First("Coal", "Resin"), delegate
		{
			SkRun.CmdNotify("stopsmoke", "Cleared smoke.");
		}, SkMenuController.SkScope.HostOnly));
		list.Add(Cell("Cleanup", "Save World", "Force a save now and restart the save timer", SkIcons.First("Coins", "Ruby"), delegate
		{
			SkRun.CmdNotify("save", "World saved.");
		}, SkMenuController.SkScope.Admin));
		list.Add(Cell("Server", "Kick", "Disconnect a player", SkIcons.First("Club", "AxeStone"), delegate
		{
			ShowUserForm("Kick Player", "kick", "Kick");
		}, SkMenuController.SkScope.Admin));
		list.Add(Cell("Server", "Ban", "Ban a player from the world", SkIcons.First("SledgeStagbreaker", "Club"), delegate
		{
			ShowUserForm("Ban Player", "ban", "Ban");
		}, SkMenuController.SkScope.Admin));
		list.Add(Cell("Server", "Unban", "Lift a ban by IP or user ID", SkIcons.First("MeadHealthMinor", "Honey"), ShowUnbanForm, SkMenuController.SkScope.Admin));
		list.Add(Cell("Server", "Who's Online", "List the players connected right now", SkIcons.First("Amber", "Coins"), delegate
		{
			SkRun.Show("Players Online", "/whois", "Nobody else is connected.");
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("Server", "Banned List", "Show who is banned", SkIcons.First("Coins"), delegate
		{
			SkRun.Show("Banned Users", "banned", "Nobody is banned.");
		}, SkMenuController.SkScope.Admin));
		list.Add(Cell("Server", "Ping", "Measure the round trip to the server", SkIcons.First("Feathers"), delegate
		{
			SkRun.CmdNotify("ping", "Ping sent. The reply prints in the console.");
		}, SkMenuController.SkScope.Client));
		List<SkMenuController.SkGridItem> grid = list;
		SkMC.RequestGridMenu(grid, null, "World", showFilter: true, null);
	}

	private void ShowTerrain()
	{
		List<SkMenuController.SkGridItem> list = new List<SkMenuController.SkGridItem>();
		list.Add(Cell("Level Terrain", "Flatten to your height, uses Radius", SkIcons.First("Hoe"), delegate
		{
			Run("/tl " + radius);
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Raise Terrain", "Raise ground, uses Radius and Height", SkIcons.First("Stone", "Wood"), delegate
		{
			Run("/tr " + radius + " " + height);
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Dig Terrain", "Lower ground, uses Radius and Height", SkIcons.First("PickaxeIron", "PickaxeStone", "PickaxeAntler"), delegate
		{
			Run("/td " + radius + " " + height);
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Undo Terrain Edits", "Revert player terraforming back to the original ground, uses Radius", SkIcons.First("Cultivator"), delegate
		{
			Run("/tu " + radius);
		}, SkMenuController.SkScope.Server));
		if (EditCompat.HasWec && (Object)(object)ModEdit.Instance != (Object)null)
		{
			list.Add(Cell("Advanced", "World Edit Terrain", "Circle or square shapes, smoothing, hills, craters, ramps, roads, paint and undo (World Edit Commands)", SkIcons.First("Cultivator", "Hoe"), ModEdit.Instance.ShowTerrain, SkMenuController.SkScope.Server));
		}
		List<SkMenuController.SkGridItem> grid = list;
		List<SkMenuController.SkMenuSlider> sliders = new List<SkMenuController.SkMenuSlider>
		{
			new SkMenuController.SkMenuSlider
			{
				Label = "Radius",
				Min = 1,
				Max = 50,
				Get = () => radius,
				Set = delegate(int value)
				{
					radius = value;
				}
			},
			new SkMenuController.SkMenuSlider
			{
				Label = "Height",
				Min = 1,
				Max = 8,
				Get = () => height,
				Set = delegate(int value)
				{
					height = value;
				}
			}
		};
		SkMC.RequestGridMenu(grid, sliders, "Terrain", showFilter: false);
	}

	private void ShowWeather()
	{
		SkMenu skMenu = new SkMenu();
		skMenu.AddItem("Let the game control weather", (Action)delegate
		{
			Run("/env -1");
		}, "Stop forcing a weather type");
		if ((Object)(object)EnvMan.instance != (Object)null && EnvMan.instance.m_environments != null)
		{
			List<string> list = new List<string>();
			foreach (EnvSetup environment in EnvMan.instance.m_environments)
			{
				if (environment != null && !string.IsNullOrEmpty(environment.m_name))
				{
					list.Add(environment.m_name);
				}
			}
			list.Sort(StringComparer.OrdinalIgnoreCase);
			foreach (string item in list)
			{
				string captured = item;
				skMenu.AddItem(captured, (Action)delegate
				{
					Run("/env " + captured);
				}, "Force weather: " + captured);
			}
		}
		else
		{
			skMenu.AddItem("Weather list unavailable", (Action)delegate
			{
			}, "Load into a world first");
		}
		SkMC.RequestSubMenu(skMenu.FlushMenu());
	}

	private void ShowTime()
	{
		List<SkMenuController.SkGridItem> list = new List<SkMenuController.SkGridItem>();
		list.Add(Cell("Apply Time", "Set and lock the time to the slider value", SkIcons.First("SurtlingCore"), delegate
		{
			Run("/tod " + Fraction(timeOfDay));
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("Morning", "Set the time to sunrise", SkIcons.First("Coins"), delegate
		{
			Run("/tod 0.25");
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("Noon", "Set the time to midday", SkIcons.First("Ruby"), delegate
		{
			Run("/tod 0.5");
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("Night", "Set the time to midnight", SkIcons.First("Thunderstone"), delegate
		{
			Run("/tod 0");
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("Unlock Time", "Hand the clock back to the game", SkIcons.First("Feathers"), delegate
		{
			Run("/tod -1");
		}, SkMenuController.SkScope.Client));
		List<SkMenuController.SkGridItem> grid = list;
		List<SkMenuController.SkMenuSlider> sliders = new List<SkMenuController.SkMenuSlider>
		{
			new SkMenuController.SkMenuSlider
			{
				Label = "Time %",
				Min = 0,
				Max = 100,
				Get = () => timeOfDay,
				Set = delegate(int value)
				{
					timeOfDay = value;
				}
			}
		};
		SkMC.RequestGridMenu(grid, sliders, "Time of Day", showFilter: false);
	}

	private void ShowSkipTimeForm()
	{
		SkMenuController.SkForm skForm = NewForm("Skip Time", "Jumps the clock forward. Everything that depends on time moves with it, including crops, food and raids.");
		skForm.Fields.Add(Slider("hours", "In-game hours", 1, 72, 12));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Skip",
			Run = delegate(SkMenuController.SkForm f)
			{
				long num = (((Object)(object)EnvMan.instance != (Object)null) ? EnvMan.instance.m_dayLengthSec : 1200);
				SkRun.CmdNotify("skiptime " + Mathf.RoundToInt((float)f.Field("hours").IntValue * ((float)num / 24f)), "Skipped " + f.Field("hours").IntValue + " hours.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowTimescaleForm()
	{
		SkMenuController.SkForm skForm = NewForm("Game Speed", "Changes how fast the whole world runs. 100% is normal.");
		skForm.Fields.Add(Slider("speed", "Speed %", 25, 500, 100));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Apply",
			Run = delegate(SkMenuController.SkForm f)
			{
				SkRun.Cmd("timescale " + Fraction(f.Field("speed").IntValue) + " 1");
				SkCommandProcessor.Notify("Game speed: " + f.Field("speed").IntValue + "%");
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Back to normal",
			Run = delegate
			{
				SkRun.Cmd("timescale 1 1");
				SkCommandProcessor.Notify("Game speed back to normal.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowWind()
	{
		List<SkMenuController.SkGridItem> list = new List<SkMenuController.SkGridItem>();
		list.Add(Cell("Apply Wind", "Set wind to the slider values", SkIcons.First("Feathers"), delegate
		{
			Run("/wind " + windAngle + " " + Fraction(windIntensity));
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("Reset Wind", "Hand the wind back to the game", SkIcons.First("Thunderstone"), delegate
		{
			Run("/resetwind");
		}, SkMenuController.SkScope.Client));
		List<SkMenuController.SkGridItem> grid = list;
		List<SkMenuController.SkMenuSlider> sliders = new List<SkMenuController.SkMenuSlider>
		{
			new SkMenuController.SkMenuSlider
			{
				Label = "Angle",
				Min = 0,
				Max = 360,
				Get = () => windAngle,
				Set = delegate(int value)
				{
					windAngle = value;
				}
			},
			new SkMenuController.SkMenuSlider
			{
				Label = "Strength %",
				Min = 0,
				Max = 100,
				Get = () => windIntensity,
				Set = delegate(int value)
				{
					windIntensity = value;
				}
			}
		};
		SkMC.RequestGridMenu(grid, sliders, "Wind", showFilter: false);
	}

	private void ShowEvents()
	{
		if ((Object)(object)RandEventSystem.instance == (Object)null || RandEventSystem.instance.m_events == null)
		{
			SkCommandProcessor.Notify("Event list is not available yet. Load into a world first.");
			return;
		}
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (RandomEvent @event in RandEventSystem.instance.m_events)
		{
			if (@event != null && !string.IsNullOrEmpty(@event.m_name))
			{
				list.Add(@event.m_name);
				list2.Add(@event.m_name + EventBlurb(@event));
			}
		}
		if (list.Count == 0)
		{
			SkCommandProcessor.Notify("This world has no events registered.");
			return;
		}
		SortPairs(list, list2);
		SkMenuController.SkForm skForm = NewForm("Events", IsWorldHost() ? "Raids spawn around you and run for a set time. Starting one replaces whatever is already running." : "Only the host or the server can change events, so these will be refused while you are a client.");
		skForm.Fields.Add(Choice("event", "Event", list, list2, 220));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Start event",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selectedOption = f.Field("event").SelectedOption;
				if (!string.IsNullOrEmpty(selectedOption))
				{
					RunEvent("event " + selectedOption, "Started event: " + selectedOption);
				}
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Random",
			Run = delegate
			{
				RunEvent("randomevent", "Started a random event.");
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Stop current",
			Run = delegate
			{
				RunEvent("stopevent", "Stopped the current event.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private static string EventBlurb(RandomEvent randomEvent)
	{
		try
		{
			if (Localization.instance != null && !string.IsNullOrEmpty(randomEvent.m_startMessage))
			{
				string text = Localization.instance.Localize(randomEvent.m_startMessage);
				if (!string.IsNullOrEmpty(text) && !text.StartsWith("["))
				{
					return "     <color=#9FB6CC>" + text + "</color>";
				}
			}
		}
		catch (Exception)
		{
		}
		return "";
	}

	private static void SortPairs(List<string> values, List<string> labels)
	{
		int[] array = new int[values.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = i;
		}
		Array.Sort(array, (int a, int b) => string.Compare(values[a], values[b], StringComparison.OrdinalIgnoreCase));
		List<string> list = new List<string>(values.Count);
		List<string> list2 = new List<string>(labels.Count);
		int[] array2 = array;
		foreach (int index in array2)
		{
			list.Add(values[index]);
			list2.Add(labels[index]);
		}
		values.Clear();
		values.AddRange(list);
		labels.Clear();
		labels.AddRange(list2);
	}

	private void ShowKillSpawnersForm()
	{
		SkMenuController.SkForm skForm = NewForm("Kill Enemies & Nests", "Spawners do not regenerate. Everyone on this world loses them, and the only way back is a world with those zones never loaded.");
		skForm.Warning = "This DESTROYS every creature spawner in every loaded zone.";
		skForm.Fields.Add(ConfirmBox("Destroy the spawners too"));
		skForm.Validate = ConfirmRequired;
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Kill and destroy",
			Run = delegate
			{
				SkRun.CmdNotify("killenemies", "Killed nearby enemies and their spawners.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowRepairForm()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		SkMenuController.SkForm skForm = NewForm("Repair Nearby", "Repairs worn and damaged building pieces around you. Pieces that are already whole are left alone.");
		skForm.Fields.Add(Slider("radius", "Radius", 5, 100, 30));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Repair",
			Run = delegate(SkMenuController.SkForm f)
			{
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				Player localPlayer2 = Player.m_localPlayer;
				if ((Object)(object)localPlayer2 == (Object)null)
				{
					SkCommandProcessor.Notify("No player yet.");
				}
				else
				{
					float num = f.Field("radius").IntValue;
					Vector3 position = ((Component)localPlayer2).transform.position;
					int num2 = 0;
					WearNTear[] array = Object.FindObjectsByType<WearNTear>((FindObjectsSortMode)0);
					WearNTear[] array2 = array;
					foreach (WearNTear val in array2)
					{
						if (!((Object)(object)val == (Object)null) && !(Vector3.Distance(((Component)val).transform.position, position) > num))
						{
							try
							{
								if (val.Repair())
								{
									num2++;
								}
							}
							catch (Exception)
							{
							}
						}
					}
					SkCommandProcessor.Notify((num2 > 0) ? ("Repaired " + num2 + " pieces.") : "Nothing nearby needed repairing.");
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowRefuelForm()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		SkMenuController.SkForm skForm = NewForm("Refuel Fires", "Tops up torches, fires, hearths and bonfires around you, each to its own maximum. Ones already full are left alone.");
		skForm.Fields.Add(Slider("radius", "Radius", 5, 100, 30));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Fill nearby",
			Run = delegate(SkMenuController.SkForm f)
			{
				Refuel(f.Field("radius").IntValue, fill: true);
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Put out",
			Run = delegate(SkMenuController.SkForm f)
			{
				Refuel(f.Field("radius").IntValue, fill: false);
			}
		});
		SkMC.ShowForm(skForm);
	}

	private static void Refuel(float radius, bool fill)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		Vector3 position = ((Component)localPlayer).transform.position;
		int num = 0;
		Fireplace[] array = Object.FindObjectsByType<Fireplace>((FindObjectsSortMode)0);
		Fireplace[] array2 = array;
		foreach (Fireplace val in array2)
		{
			if ((Object)(object)val == (Object)null || Vector3.Distance(((Component)val).transform.position, position) > radius)
			{
				continue;
			}
			try
			{
				ZNetView privateField = val.GetPrivateField<ZNetView>("m_nview");
				if (!((Object)(object)privateField == (Object)null) && privateField.IsValid())
				{
					float @float = privateField.GetZDO().GetFloat(ZDOVars.s_fuel, 0f);
					float num2 = (fill ? val.m_maxFuel : 0f);
					if (!Mathf.Approximately(@float, num2))
					{
						val.SetFuel(num2);
						num++;
					}
				}
			}
			catch (Exception)
			{
			}
		}
		if (num > 0)
		{
			SkCommandProcessor.Notify((fill ? "Filled " : "Put out ") + num + " nearby.");
		}
		else
		{
			SkCommandProcessor.Notify(fill ? "Nothing nearby needed fuel." : "Nothing nearby was lit.");
		}
	}

	private void ShowRevealForm()
	{
		if ((Object)(object)Player.m_localPlayer == (Object)null || (Object)(object)Minimap.instance == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		SkMenuController.SkForm skForm = NewForm("Reveal Around Me", "Uncovers the map out to this distance from where you are standing. The whole world is about 10000 across, so a few hundred is a comfortable walk.");
		skForm.Fields.Add(Slider("radius", "Distance", 50, 3000, 500));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Reveal",
			Run = delegate(SkMenuController.SkForm f)
			{
				//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
				Player localPlayer = Player.m_localPlayer;
				if ((Object)(object)localPlayer == (Object)null || (Object)(object)Minimap.instance == (Object)null)
				{
					SkCommandProcessor.Notify("No player yet.");
				}
				else
				{
					if (exploreAround == null)
					{
						exploreAround = typeof(Minimap).GetMethod("Explore", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[2]
						{
							typeof(Vector3),
							typeof(float)
						}, null);
					}
					if (exploreAround == null)
					{
						SkCommandProcessor.Notify("This build of the game does not expose a partial map reveal.");
					}
					else
					{
						int intValue = f.Field("radius").IntValue;
						exploreAround.Invoke(Minimap.instance, new object[2]
						{
							((Component)localPlayer).transform.position,
							(float)intValue
						});
						SkCommandProcessor.Notify("Revealed the map within " + intValue + "m.");
					}
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowDifficultyForm()
	{
		SkMenuController.SkForm skForm = NewForm("Force Difficulty", "Scales enemy health and damage as if this many players were nearby. 0 hands it back to the game.");
		skForm.Fields.Add(Slider("players", "Players", 0, 10, 1));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Apply",
			Run = delegate(SkMenuController.SkForm f)
			{
				SkRun.CmdNotify("players " + f.Field("players").IntValue, "Difficulty set.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowWorldModifierForm()
	{
		SkMenuController.SkForm skForm = NewForm("World Modifier", "The same settings the world options screen offers. They apply to everyone on this world.");
		skForm.Fields.Add(Choice("modifier", "Modifier", EnumNames(typeof(WorldModifiers), "Default"), null, 120));
		skForm.Fields.Add(Choice("value", "Value", EnumNames(typeof(WorldModifierOption)), null, 120));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Apply",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selectedOption = f.Field("modifier").SelectedOption;
				string selectedOption2 = f.Field("value").SelectedOption;
				if (!string.IsNullOrEmpty(selectedOption) && !string.IsNullOrEmpty(selectedOption2))
				{
					SkRun.Cmd("setworldmodifier " + selectedOption + " " + selectedOption2);
					SkCommandProcessor.Notify(selectedOption + ": " + selectedOption2);
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowWorldPresetForm()
	{
		SkMenuController.SkForm skForm = NewForm("World Preset", "Replaces every world modifier with the preset's values. Anything you set by hand is lost.");
		skForm.Fields.Add(Choice("preset", "Preset", EnumNames(typeof(WorldPresets), "Custom"), null, 150));
		skForm.Fields.Add(ConfirmBox("Replace all current modifiers"));
		skForm.Validate = ConfirmRequired;
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Apply preset",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selectedOption = f.Field("preset").SelectedOption;
				if (!string.IsNullOrEmpty(selectedOption))
				{
					SkRun.Cmd("setworldpreset " + selectedOption);
					SkCommandProcessor.Notify("World preset: " + selectedOption);
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private static List<string> KnownKeys()
	{
		List<string> list = EnumNames(typeof(GlobalKeys), "NonServerOption", "Count", "Preset");
		if ((Object)(object)ZNetScene.instance != (Object)null && ZNetScene.instance.m_prefabs != null)
		{
			foreach (GameObject prefab in ZNetScene.instance.m_prefabs)
			{
				if (!((Object)(object)prefab == (Object)null))
				{
					Character component = prefab.GetComponent<Character>();
					if (!((Object)(object)component == (Object)null) && !string.IsNullOrEmpty(component.m_defeatSetGlobalKey) && !list.Contains(component.m_defeatSetGlobalKey))
					{
						list.Add(component.m_defeatSetGlobalKey);
					}
				}
			}
		}
		return list;
	}

	private void ShowSetKeyForm()
	{
		if ((Object)(object)ZoneSystem.instance == (Object)null)
		{
			SkCommandProcessor.Notify("Load into a world first.");
			return;
		}
		List<string> list = KnownKeys();
		List<string> list2 = new List<string>();
		foreach (string item in list)
		{
			bool flag = false;
			try
			{
				flag = ZoneSystem.instance.GetGlobalKey(item);
			}
			catch (Exception)
			{
			}
			list2.Add(flag ? (item + "     <color=#7CFC00>already set</color>") : item);
		}
		SkMenuController.SkForm skForm = NewForm("Set World Key", "Boss keys unlock crafting and change what spawns. This is the whole world, for everyone on it.");
		skForm.Fields.Add(Choice("key", "Key", list, list2, 200));
		skForm.Fields.Add(ConfirmBox("Change this world for everyone"));
		skForm.Validate = ConfirmRequired;
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Set key",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selectedOption = f.Field("key").SelectedOption;
				if (!string.IsNullOrEmpty(selectedOption))
				{
					SkRun.CmdNotify("setkey " + selectedOption, "Set key: " + selectedOption);
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowRemoveKeyForm()
	{
		List<string> list = (((Object)(object)ZoneSystem.instance != (Object)null) ? ZoneSystem.instance.GetGlobalKeys() : null);
		if (list == null || list.Count == 0)
		{
			SkCommandProcessor.Notify("No world keys are set.");
			return;
		}
		SkMenuController.SkForm skForm = NewForm("Remove World Key", "Only keys currently set on this world are listed.");
		skForm.Fields.Add(Choice("key", "Key", list, null, 220));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Remove key",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selectedOption = f.Field("key").SelectedOption;
				if (!string.IsNullOrEmpty(selectedOption))
				{
					SkRun.CmdNotify("removekey " + selectedOption, "Removed key: " + selectedOption);
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowResetWorldKeysForm()
	{
		SkMenuController.SkForm skForm = NewForm("Reset World Keys", "Puts every world modifier back to its default. Boss progress set through the world keys goes with it.");
		skForm.Fields.Add(ConfirmBox("Reset the world modifiers"));
		skForm.Validate = ConfirmRequired;
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Reset",
			Run = delegate
			{
				SkRun.CmdNotify("resetworldkeys", "World keys reset.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowPlayerKeyForm()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		List<string> list = EnumNames(typeof(PlayerKeys));
		List<string> uniqueKeys = localPlayer.GetUniqueKeys();
		if (uniqueKeys != null)
		{
			foreach (string item in uniqueKeys)
			{
				if (!string.IsNullOrEmpty(item) && !list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		List<string> list2 = new List<string>();
		foreach (string item2 in list)
		{
			bool flag = uniqueKeys?.Contains(item2) ?? false;
			list2.Add(flag ? (item2 + "     <color=#7CFC00>on your character</color>") : item2);
		}
		SkMenuController.SkForm skForm = NewForm("Player Keys", "Keys stored on your character rather than the world. They are saved with the character.");
		skForm.Fields.Add(Choice("key", "Key", list, list2, 220));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Add to player",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selectedOption2 = f.Field("key").SelectedOption;
				if (!string.IsNullOrEmpty(selectedOption2))
				{
					SkRun.CmdNotify("setkeyplayer " + selectedOption2, "Added player key: " + selectedOption2);
				}
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Remove from player",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selectedOption = f.Field("key").SelectedOption;
				if (!string.IsNullOrEmpty(selectedOption))
				{
					SkRun.CmdNotify("removekeyplayer " + selectedOption, "Removed player key: " + selectedOption);
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowGoToForm()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Player.m_localPlayer;
		string value = "0";
		string value2 = "0";
		if ((Object)(object)localPlayer != (Object)null)
		{
			value = Mathf.RoundToInt(((Component)localPlayer).transform.position.x).ToString(CultureInfo.InvariantCulture);
			value2 = Mathf.RoundToInt(((Component)localPlayer).transform.position.z).ToString(CultureInfo.InvariantCulture);
		}
		SkMenuController.SkForm skForm = NewForm("Go To", "Map coordinates, the same pair the position readout shows. Starts on where you are now.");
		skForm.Fields.Add(TextBox("x", "X", value));
		skForm.Fields.Add(TextBox("z", "Z", value2));
		skForm.Validate = delegate(SkMenuController.SkForm f)
		{
			if (!int.TryParse((f.Field("x").TextValue ?? "").Trim(), out var result))
			{
				return "X has to be a whole number.";
			}
			return (!int.TryParse((f.Field("z").TextValue ?? "").Trim(), out result)) ? "Z has to be a whole number." : null;
		};
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Teleport",
			Run = delegate(SkMenuController.SkForm f)
			{
				string text = (f.Field("x").TextValue ?? "").Trim() + " " + (f.Field("z").TextValue ?? "").Trim();
				SkRun.Cmd("goto " + text);
				SkCommandProcessor.Notify("Teleporting to " + text);
			}
		});
		SkMC.ShowForm(skForm);
	}

	// One other player that can be teleported to. Position is empty when the game does not know where they are.
	private sealed class PlayerTarget
	{
		public string Key = "";
		public string Label = "";
		public Vector3? Position;
		// The network id of the player's game session, which is what the game addresses a message to. 0 when unknown.
		public long Uid;
	}

	// Everyone else in the world. Positions come from the player list when they share their position on the map,
	// otherwise from their network object, which is known whenever they are within your game's range.
	private static List<PlayerTarget> OtherPlayers()
	{
		List<PlayerTarget> result = new List<PlayerTarget>();
		List<string> names = new List<string>();
		Player localPlayer = Player.m_localPlayer;
		if (localPlayer == null)
		{
			return result;
		}
		ZDOID myId = ((Character)localPlayer).GetZDOID();
		Vector3 here = ((Component)localPlayer).transform.position;
		if (ZNet.instance != null)
		{
			foreach (ZNet.PlayerInfo info in ZNet.instance.GetPlayerList())
			{
				if (info.m_characterID == myId || string.IsNullOrEmpty(info.m_name))
				{
					continue;
				}
				Vector3? position = null;
				if (info.m_publicPosition)
				{
					position = info.m_position;
				}
				else if (ZDOMan.instance != null)
				{
					ZDO zdo = ZDOMan.instance.GetZDO(info.m_characterID);
					if (zdo != null)
					{
						position = zdo.GetPosition();
					}
				}
				result.Add(MakeTarget(info.m_name, position, here, names, info.m_characterID.UserID));
			}
		}
		foreach (Player allPlayer in Player.GetAllPlayers())
		{
			if (allPlayer == null || allPlayer == localPlayer)
			{
				continue;
			}
			string name = allPlayer.GetPlayerName();
			if (!string.IsNullOrEmpty(name) && !names.Contains(name))
			{
				result.Add(MakeTarget(name, ((Component)allPlayer).transform.position, here, names, ((Character)allPlayer).GetZDOID().UserID));
			}
		}
		return result;
	}

	private static PlayerTarget MakeTarget(string name, Vector3? position, Vector3 here, List<string> usedNames, long uid)
	{
		// Two players can share a name, keep the keys unique so the picker can tell them apart.
		string key = name;
		int suffix = 2;
		while (usedNames.Contains(key))
		{
			key = name + " #" + suffix++;
		}
		usedNames.Add(key);
		string where = "position hidden";
		if (position.HasValue)
		{
			Vector3 value = position.Value;
			float distance = Vector2.Distance(new Vector2(value.x, value.z), new Vector2(here.x, here.z));
			where = Mathf.RoundToInt(value.x) + ", " + Mathf.RoundToInt(value.z) + "   " + Mathf.RoundToInt(distance) + " m away";
		}
		return new PlayerTarget { Key = key, Label = key + "   (" + where + ")", Position = position, Uid = uid };
	}

	// Moves yourself (who == null) or another player to a spot. Other players are sent the game's own teleport message.
	private static bool MoveTo(PlayerTarget who, Vector3 destination)
	{
		Player localPlayer = Player.m_localPlayer;
		if (localPlayer == null)
		{
			return false;
		}
		if (who == null)
		{
			return ((Character)localPlayer).TeleportTo(destination, ((Component)localPlayer).transform.rotation, true);
		}
		if (who.Uid == 0L || Chat.instance == null || ZRoutedRpc.instance == null)
		{
			return false;
		}
		Chat.instance.TeleportPlayer(who.Uid, destination, ((Component)localPlayer).transform.rotation, distantTeleport: true);
		return true;
	}

	private static float WaterLevel()
	{
		return (ZoneSystem.instance != null) ? ZoneSystem.instance.m_waterLevel : 30f;
	}

	internal void ShowSendPlayerForm()
	{
		Player localPlayer = Player.m_localPlayer;
		if (localPlayer == null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		List<PlayerTarget> others = OtherPlayers();
		List<string> whoKeys = new List<string> { "me" };
		List<string> whoLabels = new List<string> { "Me" };
		foreach (PlayerTarget other in others)
		{
			whoKeys.Add(other.Key);
			whoLabels.Add(other.Key);
		}
		List<string> toKeys = new List<string>();
		List<string> toLabels = new List<string>();
		List<Vector3> toPositions = new List<Vector3>();
		toKeys.Add("here");
		toLabels.Add("Where I am now");
		toPositions.Add(((Component)localPlayer).transform.position);
		foreach (PlayerTarget other in others)
		{
			if (other.Position.HasValue)
			{
				toKeys.Add("p:" + other.Key);
				toLabels.Add("Player " + other.Key);
				toPositions.Add(other.Position.Value);
			}
		}
		if (Minimap.instance != null)
		{
			foreach (Minimap.PinData pin in Minimap.instance.m_pins)
			{
				if (pin != null && !string.IsNullOrEmpty(pin.m_name))
				{
					toKeys.Add("pin:" + toKeys.Count);
					toLabels.Add("Pin: " + pin.m_name + " (" + Mathf.RoundToInt(pin.m_pos.x) + ", " + Mathf.RoundToInt(pin.m_pos.z) + ")");
					toPositions.Add(new Vector3(pin.m_pos.x, Mathf.Max(pin.m_pos.y, WaterLevel()), pin.m_pos.z));
				}
			}
		}
		SkMenuController.SkForm skForm = NewForm("Teleport Player", "Pick who to move and where to. Another player is moved without being asked. Players you cannot see on the map can only be reached while they are close to you.");
		skForm.Fields.Add(Choice("who", "Move", whoKeys, whoLabels, 90));
		skForm.Fields.Add(Choice("to", "To", toKeys, toLabels, 150));
		skForm.Fields.Add(TextBox("x", "Or X", Mathf.RoundToInt(((Component)localPlayer).transform.position.x).ToString(CultureInfo.InvariantCulture)));
		skForm.Fields.Add(TextBox("z", "Or Z", Mathf.RoundToInt(((Component)localPlayer).transform.position.z).ToString(CultureInfo.InvariantCulture)));
		Func<SkMenuController.SkForm, PlayerTarget> pickWho = delegate(SkMenuController.SkForm f)
		{
			string selected = f.Field("who")?.SelectedOption;
			return (selected == null || selected == "me") ? null : others.Find((PlayerTarget t) => t.Key == selected);
		};
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Teleport to selection",
			Run = delegate(SkMenuController.SkForm f)
			{
				PlayerTarget who = pickWho(f);
				string selectedTo = f.Field("to")?.SelectedOption;
				int index = toKeys.IndexOf(selectedTo);
				if (index < 0)
				{
					SkCommandProcessor.Notify("Pick a destination.");
				}
				else if (MoveTo(who, toPositions[index]))
				{
					SkCommandProcessor.Notify("Teleporting " + ((who == null) ? "you" : who.Key) + ".");
				}
				else
				{
					SkCommandProcessor.Notify("Could not teleport right now.");
				}
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Teleport to X, Z",
			Run = delegate(SkMenuController.SkForm f)
			{
				if (!int.TryParse((f.Field("x").TextValue ?? "").Trim(), out var x) || !int.TryParse((f.Field("z").TextValue ?? "").Trim(), out var z))
				{
					SkCommandProcessor.Notify("X and Z have to be whole numbers.");
				}
				else
				{
					PlayerTarget who = pickWho(f);
					Vector3 destination = new Vector3(x, WaterLevel(), z);
					if (ZoneSystem.instance != null && ZoneSystem.instance.IsZoneLoaded(destination))
					{
						destination.y = Mathf.Max(WaterLevel(), ZoneSystem.instance.GetGroundHeight(destination)) + 1f;
					}
					if (MoveTo(who, destination))
					{
						SkCommandProcessor.Notify("Teleporting " + ((who == null) ? "you" : who.Key) + " to " + x + ", " + z + ".");
					}
					else
					{
						SkCommandProcessor.Notify("Could not teleport right now.");
					}
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	internal void ShowHealPlayerForm()
	{
		List<Player> players = Player.GetAllPlayers();
		if (players == null || players.Count == 0)
		{
			SkCommandProcessor.Notify("No players are loaded around you.");
			return;
		}
		List<string> names = new List<string>();
		foreach (Player player in players)
		{
			if (player != null)
			{
				names.Add(player.GetPlayerName());
			}
		}
		SkMenuController.SkForm skForm = NewForm("Heal Player", "Restores health. Only players loaded around you are listed.");
		skForm.Fields.Add(Choice("player", "Player", names, null, 130));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Heal selected",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selected = f.Field("player")?.SelectedOption;
				foreach (Player item in Player.GetAllPlayers())
				{
					if (item != null && item.GetPlayerName() == selected)
					{
						((Character)item).Heal(((Character)item).GetMaxHealth(), true);
						SkCommandProcessor.Notify("Healed " + selected + ".");
						return;
					}
				}
				SkCommandProcessor.Notify("That player is no longer loaded.");
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Heal everyone",
			Run = delegate
			{
				int num = 0;
				foreach (Player item2 in Player.GetAllPlayers())
				{
					if (item2 != null)
					{
						((Character)item2).Heal(((Character)item2).GetMaxHealth(), true);
						num++;
					}
				}
				SkCommandProcessor.Notify("Healed " + num + " player(s).");
			}
		});
		SkMC.ShowForm(skForm);
	}

	internal void ShowBroadcastForm()
	{
		SkMenuController.SkForm skForm = NewForm("Event Message", "Shows the text in the middle of the screen (the yellow event message) for every player loaded around you.");
		skForm.Fields.Add(TextBox("text", "Message", ""));
		skForm.Validate = (SkMenuController.SkForm f) => string.IsNullOrWhiteSpace(f.Field("text").TextValue) ? "Type a message first." : null;
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Send",
			Run = delegate(SkMenuController.SkForm f)
			{
				string text = f.Field("text").TextValue.Trim();
				int num = 0;
				foreach (Player item in Player.GetAllPlayers())
				{
					if (item != null)
					{
						((Character)item).Message(MessageHud.MessageType.Center, text, 0, (Sprite)null, false);
						num++;
					}
				}
				SkCommandProcessor.Notify("Sent to " + num + " player(s).");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private static List<string> OtherLoadedPlayerNames()
	{
		List<string> names = new List<string>();
		Player me = Player.m_localPlayer;
		foreach (Player player in Player.GetAllPlayers())
		{
			if (player != null && me != null && player.GetPlayerID() != me.GetPlayerID())
			{
				names.Add(player.GetPlayerName());
			}
		}
		return names;
	}

	internal void ShowDamagePlayerForm()
	{
		if (!SkPlayerTools.Allowed())
		{
			return;
		}
		List<string> names = OtherLoadedPlayerNames();
		if (names.Count == 0)
		{
			SkCommandProcessor.Notify("No other players are loaded around you.");
			return;
		}
		SkMenuController.SkForm skForm = NewForm("Damage Player", "Hits the chosen player for this much damage. It ignores the no-PvP setting. A player who is flying, in god mode or otherwise protected can still shrug it off.");
		skForm.Fields.Add(Choice("player", "Player", names, null, 120));
		skForm.Fields.Add(Slider("damage", "Damage", 1, 1000, 50));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Hit",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selected = f.Field("player")?.SelectedOption;
				foreach (Player player in Player.GetAllPlayers())
				{
					if (player != null && player.GetPlayerName() == selected && player != Player.m_localPlayer)
					{
						SkPlayerTools.Damage(player, f.Field("damage").IntValue);
						SkCommandProcessor.Notify("Hit " + selected + " for " + f.Field("damage").IntValue + ".");
						return;
					}
				}
				SkCommandProcessor.Notify("That player is no longer loaded.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	internal void ShowKillPlayerForm()
	{
		if (!SkPlayerTools.Allowed())
		{
			return;
		}
		List<string> names = OtherLoadedPlayerNames();
		if (names.Count == 0)
		{
			SkCommandProcessor.Notify("No other players are loaded around you.");
			return;
		}
		SkMenuController.SkForm skForm = NewForm("Kill Player", "Kills the one player you choose. They drop their tombstone and respawn as normal. A player who is flying or in god mode can shrug it off.");
		skForm.Warning = "This kills the chosen player.";
		skForm.Fields.Add(Choice("player", "Player", names, null, 120));
		skForm.Fields.Add(ConfirmBox("Kill them"));
		skForm.Validate = ConfirmRequired;
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Kill",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selected = f.Field("player")?.SelectedOption;
				foreach (Player player in Player.GetAllPlayers())
				{
					if (player != null && player != Player.m_localPlayer && player.GetPlayerName() == selected)
					{
						SkPlayerTools.Damage(player, 1E+10f);
						SkCommandProcessor.Notify("Killed " + selected + ".");
						return;
					}
				}
				SkCommandProcessor.Notify("That player is no longer loaded.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	internal void ShowKillPlayersForm()
	{
		if (!SkPlayerTools.Allowed())
		{
			return;
		}
		SkMenuController.SkForm skForm = NewForm("Kill All Players", "Kills every other player loaded around you. They drop their tombstone and respawn as normal.");
		skForm.Warning = "This kills every other player near you.";
		skForm.Fields.Add(ConfirmBox("Kill them"));
		skForm.Validate = ConfirmRequired;
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Kill all",
			Run = delegate
			{
				int num = 0;
				foreach (Player player in Player.GetAllPlayers())
				{
					if (player != null && player != Player.m_localPlayer)
					{
						SkPlayerTools.Damage(player, 1E+10f);
						num++;
					}
				}
				SkCommandProcessor.Notify("Hit " + num + " player(s).");
			}
		});
		SkMC.ShowForm(skForm);
	}

	internal void ShowChatAsForm()
	{
		if (!SkPlayerTools.Allowed())
		{
			return;
		}
		SkMenuController.SkForm skForm = NewForm("Chat As", "Sends a chat line that shows the name you type. Leave the name blank to use your own. If a loaded player has that name, a normal line comes out of their position. The line still carries your account id.");
		skForm.Fields.Add(TextBox("name", "Show as", ""));
		skForm.Fields.Add(TextBox("text", "Message", ""));
		skForm.Fields.Add(new SkMenuController.SkFormField
		{
			Id = "shout",
			Label = "Shout (everyone sees it)",
			Kind = SkMenuController.SkFieldKind.Toggle
		});
		skForm.Validate = (SkMenuController.SkForm f) => string.IsNullOrWhiteSpace(f.Field("text").TextValue) ? "Type a message first." : null;
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Send",
			Run = delegate(SkMenuController.SkForm f)
			{
				SkPlayerTools.ChatAs(f.Field("name").TextValue, f.Field("text").TextValue.Trim(), f.Field("shout").BoolValue);
				SkCommandProcessor.Notify("Sent.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowBringPlayerForm()
	{
		List<PlayerTarget> targets = OtherPlayers();
		SkMenuController.SkForm skForm = NewForm("Bring Player", (targets.Count > 0) ? "Pick one player to pull to where you are standing. Unlike Recall Players this moves only the player you choose. They are moved without being asked." : "Nobody else is in this world.");
		if (targets.Count > 0)
		{
			List<string> keys = new List<string>();
			List<string> labels = new List<string>();
			foreach (PlayerTarget target in targets)
			{
				keys.Add(target.Key);
				labels.Add(target.Label);
			}
			skForm.Fields.Add(Choice("player", "Players", keys, labels, 130));
		}
		skForm.Validate = delegate(SkMenuController.SkForm f)
		{
			string selected = f.Field("player")?.SelectedOption;
			if (string.IsNullOrEmpty(selected))
			{
				return "Pick a player.";
			}
			PlayerTarget playerTarget = targets.Find((PlayerTarget t) => t.Key == selected);
			return (playerTarget != null && playerTarget.Uid != 0L) ? null : "The game does not know how to reach that player yet.";
		};
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Bring to me",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selected = f.Field("player")?.SelectedOption;
				// Look the player up again, the list may have changed since the form was built.
				PlayerTarget playerTarget = OtherPlayers().Find((PlayerTarget t) => t.Key == selected);
				Player localPlayer = Player.m_localPlayer;
				if (localPlayer == null || playerTarget == null || playerTarget.Uid == 0L || Chat.instance == null || ZRoutedRpc.instance == null)
				{
					SkCommandProcessor.Notify("Could not reach that player.");
				}
				else
				{
					// Drop them a couple of meters in front of you rather than inside you.
					Transform transform = ((Component)localPlayer).transform;
					Vector3 destination = transform.position + transform.forward * 2f + Vector3.up * 0.5f;
					Chat.instance.TeleportPlayer(playerTarget.Uid, destination, transform.rotation, distantTeleport: true);
					SkCommandProcessor.Notify("Brought " + playerTarget.Key + " to you.");
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowTeleportToPlayerForm()
	{
		List<PlayerTarget> targets = OtherPlayers();
		SkMenuController.SkForm skForm = NewForm("Teleport to Player", (targets.Count > 0) ? "Pick a player to teleport next to. A player who hides their map position can only be reached while they are close enough for your game to know where they are." : "Nobody else is in this world.");
		if (targets.Count > 0)
		{
			List<string> keys = new List<string>();
			List<string> labels = new List<string>();
			foreach (PlayerTarget target in targets)
			{
				keys.Add(target.Key);
				labels.Add(target.Label);
			}
			skForm.Fields.Add(Choice("player", "Players", keys, labels, 130));
		}
		skForm.Validate = delegate(SkMenuController.SkForm f)
		{
			string selected = f.Field("player")?.SelectedOption;
			if (string.IsNullOrEmpty(selected))
			{
				return "Pick a player.";
			}
			PlayerTarget playerTarget = targets.Find((PlayerTarget t) => t.Key == selected);
			return (playerTarget != null && playerTarget.Position.HasValue) ? null : "That player's position is not known to you right now.";
		};
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Teleport",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selected = f.Field("player")?.SelectedOption;
				// Look the player up again, they may have moved since the list was built.
				PlayerTarget playerTarget = OtherPlayers().Find((PlayerTarget t) => t.Key == selected);
				Player localPlayer = Player.m_localPlayer;
				if (localPlayer == null || playerTarget == null || !playerTarget.Position.HasValue)
				{
					SkCommandProcessor.Notify("Could not find that player's position.");
				}
				else
				{
					// Land a couple of meters beside them rather than inside them.
					Vector3 destination = playerTarget.Position.Value + new Vector3(1.5f, 0.5f, 1.5f);
					if (((Character)localPlayer).TeleportTo(destination, ((Component)localPlayer).transform.rotation, true))
					{
						SkCommandProcessor.Notify("Teleporting to " + playerTarget.Key);
					}
					else
					{
						SkCommandProcessor.Notify("Could not teleport right now.");
					}
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowFindForm()
	{
		SkMenuController.SkForm skForm = NewForm("Find", "Searches loaded objects and known locations by name. Pinging marks them on the map; teleporting takes you to the closest.");
		skForm.Fields.Add(TextBox("text", "Search for", ""));
		skForm.Fields.Add(Slider("pings", "Most pins to place", 1, 20, 1));
		skForm.Validate = (SkMenuController.SkForm f) => (!string.IsNullOrEmpty((f.Field("text").TextValue ?? "").Trim())) ? null : "Type something to search for.";
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Pin on map",
			Run = delegate(SkMenuController.SkForm f)
			{
				SkRun.CmdNotify("find " + (f.Field("text").TextValue ?? "").Trim() + " " + f.Field("pings").IntValue, "Searched the map.");
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Teleport to closest",
			Run = delegate(SkMenuController.SkForm f)
			{
				SkRun.CmdNotify("findtp " + (f.Field("text").TextValue ?? "").Trim(), "Looking for the closest match.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowFindBiomeForm()
	{
		SkMenuController.SkForm skForm = NewForm("Find Biome", "Marks every sector of that biome on the map, or takes you to the closest one.");
		skForm.Fields.Add(Choice("biome", "Biome", EnumNames(typeof(Biome), "None", "All", "Land"), null, 180));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Pin on map",
			Run = delegate(SkMenuController.SkForm f)
			{
				SkRun.CmdNotify("findbiome " + f.Field("biome").SelectedOption, "Marked that biome.");
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Teleport there",
			Run = delegate(SkMenuController.SkForm f)
			{
				SkRun.CmdNotify("findbiometp " + f.Field("biome").SelectedOption, "Looking for that biome.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private static List<string> ConnectedPlayers()
	{
		List<string> list = new List<string>();
		if ((Object)(object)ZNet.instance == (Object)null)
		{
			return list;
		}
		List<ZNetPeer> peers = ZNet.instance.GetPeers();
		if (peers == null)
		{
			return list;
		}
		foreach (ZNetPeer item in peers)
		{
			if (item != null && !string.IsNullOrEmpty(item.m_playerName) && !list.Contains(item.m_playerName))
			{
				list.Add(item.m_playerName);
			}
		}
		return list;
	}

	private void ShowUserForm(string title, string command, string actionLabel)
	{
		List<string> list = ConnectedPlayers();
		SkMenuController.SkForm skForm = NewForm(title, (list.Count > 0) ? "Pick someone who is connected, or type a name, IP or user ID instead." : "Nobody else is connected. Type a name, IP or user ID.");
		if (list.Count > 0)
		{
			skForm.Fields.Add(Choice("peer", "Connected players", list, null, 130));
		}
		skForm.Fields.Add(TextBox("name", "Or type a name / IP / user ID", ""));
		skForm.Fields.Add(ConfirmBox(actionLabel + " this player"));
		skForm.Validate = (SkMenuController.SkForm f) => string.IsNullOrEmpty(UserTarget(f)) ? "Pick a player or type a name." : ConfirmRequired(f);
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = actionLabel,
			Run = delegate(SkMenuController.SkForm f)
			{
				string text = UserTarget(f);
				SkRun.CmdNotify(command + " " + text, actionLabel + ": " + text);
			}
		});
		SkMC.ShowForm(skForm);
	}

	private static string UserTarget(SkMenuController.SkForm form)
	{
		SkMenuController.SkFormField skFormField = form.Field("name");
		string text = ((skFormField != null) ? (skFormField.TextValue ?? "").Trim() : "");
		if (text.Length > 0)
		{
			return text;
		}
		return form.Field("peer")?.SelectedOption;
	}

	private void ShowUnbanForm()
	{
		SkMenuController.SkForm skForm = NewForm("Unban", "Lifts a ban. The banned list shows what to type here.");
		skForm.Fields.Add(TextBox("name", "IP or user ID", ""));
		skForm.Validate = (SkMenuController.SkForm f) => (!string.IsNullOrEmpty((f.Field("name").TextValue ?? "").Trim())) ? null : "Type an IP or user ID.";
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Unban",
			Run = delegate(SkMenuController.SkForm f)
			{
				string text = (f.Field("name").TextValue ?? "").Trim();
				SkRun.CmdNotify("unban " + text, "Unbanned " + text);
			}
		});
		SkMC.ShowForm(skForm);
	}
}
