using System;
using System.Collections.Generic;
using System.Globalization;
using SkToolbox.Utility;
using UnityEngine;

namespace SkToolbox.SkModules;

// The "Edit" menu: terraforming, object editing and building tools from World Edit Commands and Infinity Hammer.
// Everything is sent as a console command, so this module works with any version of those mods and costs nothing
// when they are not installed (the menu entry is not even added).
internal class ModEdit : SkBaseModule, IModule
{
	private static readonly string[] Paints = new string[12]
	{
		"grass", "patches", "grass_dark", "dirt", "cultivated", "paved", "paved_moss", "paved_dirt", "paved_dark", "lava",
		"lava_shallow", "lava_light"
	};

	private static readonly string[] Invulnerability = new string[4] { "Off", "On", "Damaged", "Worn" };

	private int radius = 8;

	private int height = 3;

	private int smooth;

	private bool square;

	private bool skipBuildings;

	private bool pave;

	private bool followHeight = true;

	private int roadWidth = 3;

	private readonly List<string> roadPoints = new List<string>();

	internal static ModEdit Instance;

	public ModEdit()
	{
		ModuleName = "Edit";
		Loading();
	}

	public void Start()
	{
		Instance = this;
		BeginMenu();
		Ready();
	}

	public void BeginMenu()
	{
		base.MenuOptions = new SkMenu();
	}

	private static string Num(float value)
	{
		return value.ToString("0.##", CultureInfo.InvariantCulture);
	}

	private static string Fraction(int percent)
	{
		return ((float)percent / 100f).ToString("0.00", CultureInfo.InvariantCulture);
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

	private SkMenuController.SkForm NewForm(string title, string note)
	{
		return new SkMenuController.SkForm
		{
			Title = title,
			Note = note
		};
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

	private static SkMenuController.SkFormField Switch(string id, string label, bool value)
	{
		return new SkMenuController.SkFormField
		{
			Id = id,
			Label = label,
			Kind = SkMenuController.SkFieldKind.Toggle,
			BoolValue = value
		};
	}

	private static SkMenuController.SkFormField Choice(string id, string label, IEnumerable<string> options, int height)
	{
		SkMenuController.SkFormField skFormField = new SkMenuController.SkFormField
		{
			Id = id,
			Label = label,
			Kind = SkMenuController.SkFieldKind.Choice,
			Height = height
		};
		skFormField.Options.AddRange(options);
		return skFormField;
	}

	private static bool HavePlayer()
	{
		if ((Object)(object)Player.m_localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return false;
		}
		return true;
	}

	// Cleans a free text id list so it can sit inside a command: no spaces, no stray separators.
	private static string CleanIds(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return "";
		}
		string[] array = text.Split(new char[3] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
		return string.Join(",", array);
	}

	// The shape and size every terrain command needs ("circle=8" or "rect=8").
	private string Shape()
	{
		return (square ? "rect=" : "circle=") + radius;
	}

	private string Options()
	{
		string text = "";
		if (smooth > 0)
		{
			text = text + " smooth=" + Fraction(smooth);
		}
		if (skipBuildings)
		{
			text += " blockcheck";
		}
		return text;
	}

	private static void RunTerrain(string command, string done)
	{
		if (HavePlayer())
		{
			EditCompat.Run("terrain " + command, done);
		}
	}

	internal void ShowGrid()
	{
		List<SkMenuController.SkGridItem> list = new List<SkMenuController.SkGridItem>();
		if (EditCompat.HasWec)
		{
			list.Add(Cell("Terraform", "Terrain Editor", "Level, raise, lower, smooth or reset the ground in a circle or square", SkIcons.First("Hoe"), ShowTerrain, SkMenuController.SkScope.Server));
			list.Add(Cell("Terraform", "Level to Height", "Flatten the ground to an exact height", SkIcons.First("Cultivator"), ShowLevelForm, SkMenuController.SkScope.Server));
			list.Add(Cell("Terraform", "Ramp", "Build a ramp in the direction you are facing", SkIcons.First("Stone", "Wood"), ShowRampForm, SkMenuController.SkScope.Server));
			list.Add(Cell("Terraform", "Road / Path", "Mark points as you walk, then lay a levelled road through them", SkIcons.First("Flint", "Stone"), ShowRoad, SkMenuController.SkScope.Server));
			list.Add(Cell("Terraform", "Paint Ground", "Paint the ground with paved, dirt, grass, cultivated and more", SkIcons.First("Raspberry", "Blueberries"), ShowPaintForm, SkMenuController.SkScope.Server));
			list.Add(Cell("Terraform", "Undo", "Undo your last World Edit action (terrain, objects, spawns)", SkIcons.First("Coins"), delegate
			{
				EditCompat.Run("undo", "Undid the last edit.");
			}, SkMenuController.SkScope.Client));
			list.Add(Cell("Terraform", "Redo", "Redo the edit you just undid", SkIcons.First("Ruby"), delegate
			{
				EditCompat.Run("redo", "Redid the edit.");
			}, SkMenuController.SkScope.Client));
			list.Add(Cell("Objects", "Modify Objects", "Scale, move, rotate, respawn or inspect objects around you", SkIcons.First("Club", "AxeStone"), ShowObjectForm, SkMenuController.SkScope.Server));
			list.Add(Cell("Objects", "Remove Objects", "Delete objects by id around you. Undo brings them back", SkIcons.First("PickaxeIron", "PickaxeStone"), ShowRemoveForm, SkMenuController.SkScope.Server));
			list.Add(Cell("Objects", "Spawn Location", "Place a whole point of interest (ruins, camps, dungeons) in front of you", SkIcons.First("CryptKey", "Wishbone"), ShowLocationForm, SkMenuController.SkScope.Server));
		}
		if (EditCompat.HasHammer)
		{
			list.Add(Cell("Hammer", "Hammer Menu", "Open the Infinity Hammer piece menu: every object, location, room and tool", SkIcons.First("Hammer"), delegate
			{
				EditCompat.Run("hammer_menu", "Hammer menu toggled.");
			}, SkMenuController.SkScope.Client));
			list.Add(Cell("Hammer", "Select by Id", "Pick any object by prefab id to build with the hammer", SkIcons.First("Hammer"), ShowSelectForm, SkMenuController.SkScope.Client));
			list.Add(Cell("Hammer", "Copy Area", "Select every object around you, optionally with the terrain shape and paint", SkIcons.First("Cultivator", "Hammer"), ShowAreaForm, SkMenuController.SkScope.Client));
			list.Add(Cell("Hammer", "Repair Tool", "Select the repair tool, which also clears the current selection", SkIcons.First("Resin"), delegate
			{
				EditCompat.Run("hammer_repair", "Repair tool selected.");
			}, SkMenuController.SkScope.Client));
			list.Add(Cell("Hammer", "Place", "Place the current selection without clicking", SkIcons.First("Wood"), delegate
			{
				EditCompat.Run("hammer_place", "Placed.");
			}, SkMenuController.SkScope.Client));
			list.Add(Cell("Hammer", "Freeze", "Lock the placement ghost in place so you can fine tune it", SkIcons.First("Chain", "Iron"), delegate
			{
				EditCompat.Run("hammer_freeze", "Placement freeze toggled.");
			}, SkMenuController.SkScope.Client));
			list.Add(Cell("Hammer", "Mirror", "Mirror the current selection", SkIcons.First("Coins"), delegate
			{
				EditCompat.Run("hammer_mirror", "Selection mirrored.");
			}, SkMenuController.SkScope.Client));
			list.Add(Cell("Hammer", "Random Rotation", "Turn the selection to a random yaw", SkIcons.First("Thunderstone", "Feathers"), delegate
			{
				EditCompat.Run("hammer_rotate_y random", "Selection rotated.");
			}, SkMenuController.SkScope.Client));
			list.Add(Cell("Hammer", "Reset Offset", "Clear the placement offset", SkIcons.First("Flint"), delegate
			{
				EditCompat.Run("hammer_offset", "Offset reset.");
			}, SkMenuController.SkScope.Client));
			list.Add(Cell("Hammer", "Reset Scale", "Put the selection back to its normal size", SkIcons.First("Stone"), delegate
			{
				EditCompat.Run("hammer_scale", "Scale reset.");
			}, SkMenuController.SkScope.Client));
			list.Add(Cell("Hammer", "Reset Zoop", "Stop duplicating the selection", SkIcons.First("Resin", "Coal"), delegate
			{
				EditCompat.Run("hammer_zoop_reset", "Zoop reset.");
			}, SkMenuController.SkScope.Client));
			AddHammerToggle(list, "No Resource Cost", "Building costs no materials", "Wood");
			AddHammerToggle(list, "No Durability Cost", "The hammer never wears down", "Iron");
			AddHammerToggle(list, "No Stamina Cost", "Building costs no stamina or eitr", "Honey");
			AddHammerToggle(list, "Remove Anything", "The hammer can remove any object, not just pieces", "PickaxeIron");
			AddHammerToggle(list, "Repair Anything", "The hammer can repair or heal any object", "Resin");
			AddHammerToggle(list, "Ignore Wards", "Wards no longer block building", "Amber");
			AddHammerToggle(list, "Ignore No Build", "Build in areas that normally forbid it", "Coins");
			AddHammerToggle(list, "Ignore Other Restrictions", "Skip biome, material and other placement rules", "Ruby");
			AddHammerToggle(list, "Allow In Dungeons", "Allow building inside dungeons", "CryptKey");
			AddHammerToggle(list, "Disable Loot", "Removed creatures and structures drop nothing", "LeatherScraps");
			AddHammerToggle(list, "No Physics", "Placed objects do not fall", "Feathers");
			AddHammerToggle(list, "No Target", "Enemies will not attack what you build", "TrophyGreydwarf");
			AddHammerToggle(list, "No Creator", "Do not record who built a piece (smaller save)", "Coal");
			list.Add(Cell("Hammer Rules", "Invulnerable Pieces", "Make what you build unbreakable, or damaged and worn looking" + BypassNote(), SkIcons.First("Iron", "Bronze"), ShowInvulnerabilityForm, SkMenuController.SkScope.Client));
			list.Add(Cell("Hammer Rules", "Hammer Range", "How far the hammer can reach" + BypassNote(), SkIcons.First("Feathers", "Chain"), ShowRangeForm, SkMenuController.SkScope.Client));
		}
		if (list.Count == 0)
		{
			SkCommandProcessor.Notify("Install World Edit Commands or Infinity Hammer to use this menu.");
		}
		else
		{
			SkMC.RequestGridMenu(list, null, "Edit", showFilter: true, null);
		}
	}

	// Shown in the tips when the HammerOnAnyServer option is on, so it is clear these switches are not coming from the server.
	private static string BypassNote()
	{
		return SkHammerAccess.Active ? "  [HammerOnAnyServer is on: server admin permission is not required on your side]" : "";
	}

	private void AddHammerToggle(List<SkMenuController.SkGridItem> list, string name, string tip, string icon)
	{
		string key = EditCompat.ConfigKey(name);
		list.Add(Toggle("Hammer Rules", name, tip + BypassNote(), SkIcons.First(icon), delegate
		{
			EditCompat.Run("hammer_config " + key, name + " toggled.");
		}, () => EditCompat.HammerFlag(name), SkMenuController.SkScope.Client));
	}

	// Opened from here and from World > Terrain Tools.
	internal void ShowTerrain()
	{
		if (!EditCompat.HasWec)
		{
			SkCommandProcessor.Notify("World Edit Commands is not installed.");
			return;
		}
		List<SkMenuController.SkGridItem> list = new List<SkMenuController.SkGridItem>();
		list.Add(Toggle("Options", "Square", "Use a square instead of a circle. Radius is then half the side", SkIcons.First("Stone"), delegate
		{
			square = !square;
		}, () => square, SkMenuController.SkScope.Client));
		list.Add(Toggle("Options", "Skip Buildings", "Leave the ground under buildings and other objects alone", SkIcons.First("Wood"), delegate
		{
			skipBuildings = !skipBuildings;
		}, () => skipBuildings, SkMenuController.SkScope.Client));
		list.Add(Cell("Shape", "Level", "Flatten to the ground height where you stand", SkIcons.First("Hoe"), delegate
		{
			RunTerrain("level " + Shape() + Options(), "Levelled the ground.");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Shape", "Level + Pave", "Flatten and pave the ground", SkIcons.First("Stone"), delegate
		{
			RunTerrain("level paint=paved " + Shape() + Options(), "Levelled and paved the ground.");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Shape", "Raise", "Raise the ground by Height", SkIcons.First("Wood"), delegate
		{
			RunTerrain("raise=" + height + " " + Shape() + Options(), "Raised the ground.");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Shape", "Lower", "Lower the ground by Height", SkIcons.First("PickaxeIron", "PickaxeStone", "PickaxeAntler"), delegate
		{
			RunTerrain("lower=" + height + " " + Shape() + Options(), "Lowered the ground.");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Shape", "Hill", "Raise a smooth rounded mound by Height", SkIcons.First("Stone", "Wood"), delegate
		{
			RunTerrain("raise=" + height + " smooth=1 " + Shape() + (skipBuildings ? " blockcheck" : ""), "Raised a hill.");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Shape", "Crater", "Dig a smooth rounded hollow by Height", SkIcons.First("PickaxeIron", "PickaxeStone"), delegate
		{
			RunTerrain("lower=" + height + " smooth=1 " + Shape() + (skipBuildings ? " blockcheck" : ""), "Dug a crater.");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Shape", "Smooth Out", "Level the area, easing the edges into the surroundings", SkIcons.First("Cultivator"), delegate
		{
			RunTerrain("level smooth=1 " + Shape() + (skipBuildings ? " blockcheck" : ""), "Smoothed the ground.");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Shape", "Pyramid", "raise a pyramid shape by Height", SkIcons.First("Stone"), delegate
		{
			RunTerrain("level raise=" + height + " smooth=1 rect=" + radius, "Raised a pyramid.");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Reset", "Reset Terrain", "Put the ground and paint back to the original in this area", SkIcons.First("Cultivator"), delegate
		{
			RunTerrain("reset " + Shape(), "Reset the ground.");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("History", "Undo", "Undo the last edit", SkIcons.First("Coins"), delegate
		{
			EditCompat.Run("undo", "Undid the last edit.");
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("History", "Redo", "Redo the edit you just undid", SkIcons.First("Ruby"), delegate
		{
			EditCompat.Run("redo", "Redid the edit.");
		}, SkMenuController.SkScope.Client));
		List<SkMenuController.SkMenuSlider> sliders = new List<SkMenuController.SkMenuSlider>
		{
			new SkMenuController.SkMenuSlider
			{
				Label = "Radius",
				Min = 1,
				Max = 64,
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
				Max = 40,
				Get = () => height,
				Set = delegate(int value)
				{
					height = value;
				}
			},
			new SkMenuController.SkMenuSlider
			{
				Label = "Smooth %",
				Min = 0,
				Max = 100,
				Get = () => smooth,
				Set = delegate(int value)
				{
					smooth = value;
				}
			}
		};
		SkMC.RequestGridMenu(list, sliders, "Terrain Editor", showFilter: false);
	}

	private void ShowLevelForm()
	{
		if (!HavePlayer())
		{
			return;
		}
		int value = Mathf.RoundToInt(((Component)Player.m_localPlayer).transform.position.y);
		SkMenuController.SkForm skForm = NewForm("Level to Height", "Flattens the ground in the Terrain Editor's shape and radius to a fixed height above sea level (the water is at 30).");
		skForm.Fields.Add(Slider("y", "Height (Y)", -50, 500, Mathf.Clamp(value, -50, 500)));
		skForm.Fields.Add(Slider("radius", "Radius", 1, 64, radius));
		skForm.Fields.Add(Switch("pave", "Pave the ground too", false));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Level",
			Run = delegate(SkMenuController.SkForm f)
			{
				string text = (square ? "rect=" : "circle=") + f.Field("radius").IntValue;
				RunTerrain("level=" + f.Field("y").IntValue + (f.Field("pave").BoolValue ? " paint=paved " : " ") + text + Options(), "Levelled the ground to " + f.Field("y").IntValue + ".");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowRampForm()
	{
		if (!HavePlayer())
		{
			return;
		}
		SkMenuController.SkForm skForm = NewForm("Ramp", "Builds a ramp from where you stand, in the direction you are facing. A positive height climbs, a negative one descends, 0 follows the ground.");
		skForm.Fields.Add(Slider("length", "Length", 4, 150, 20));
		skForm.Fields.Add(Slider("width", "Half width", 1, 30, 4));
		skForm.Fields.Add(Slider("rise", "Rise", -40, 40, 5));
		skForm.Fields.Add(Switch("pave", "Pave the ramp", true));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Build ramp",
			Run = delegate(SkMenuController.SkForm f)
			{
				Player localPlayer = Player.m_localPlayer;
				if ((Object)(object)localPlayer == (Object)null)
				{
					SkCommandProcessor.Notify("No player yet.");
				}
				else
				{
					Vector3 position = ((Component)localPlayer).transform.position;
					Vector3 forward = ((Component)localPlayer).transform.forward;
					forward.y = 0f;
					if (forward.sqrMagnitude < 0.0001f)
					{
						forward = Vector3.forward;
					}
					forward.Normalize();
					Vector3 vector = position + forward * (float)f.Field("length").IntValue;
					string text = "slope=" + f.Field("rise").IntValue + " rect=" + f.Field("width").IntValue + " from=" + Num(position.x) + "," + Num(position.z) + " to=" + Num(vector.x) + "," + Num(vector.z) + (f.Field("pave").BoolValue ? " paint=paved" : "");
					RunTerrain(text, "Built a ramp.");
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowPaintForm()
	{
		SkMenuController.SkForm skForm = NewForm("Paint Ground", "Paints the ground without changing its height. Uses the Terrain Editor's shape (circle or square).");
		skForm.Fields.Add(Choice("paint", "Paint", Paints, 190));
		skForm.Fields.Add(Slider("radius", "Radius", 1, 64, radius));
		skForm.Fields.Add(Slider("smooth", "Soft edge %", 0, 100, smooth));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Paint",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selectedOption = f.Field("paint").SelectedOption;
				if (!string.IsNullOrEmpty(selectedOption))
				{
					string text = (square ? "rect=" : "circle=") + f.Field("radius").IntValue;
					int intValue = f.Field("smooth").IntValue;
					RunTerrain("paint=" + selectedOption + " " + text + ((intValue > 0) ? (" smooth=" + Fraction(intValue)) : "") + (skipBuildings ? " blockcheck" : ""), "Painted the ground " + selectedOption + ".");
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowRoad()
	{
		List<SkMenuController.SkGridItem> list = new List<SkMenuController.SkGridItem>();
		list.Add(Cell("Points", "Add Point Here", "Mark where you are standing as the next point of the road", SkIcons.First("Flint"), AddRoadPoint, SkMenuController.SkScope.Client));
		list.Add(Cell("Points", "Remove Last Point", "Forget the most recent point", SkIcons.First("Coal"), delegate
		{
			if (roadPoints.Count > 0)
			{
				roadPoints.RemoveAt(roadPoints.Count - 1);
			}
			SkCommandProcessor.Notify("Road points: " + roadPoints.Count);
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("Points", "Clear Points", "Forget every point", SkIcons.First("Stone"), delegate
		{
			roadPoints.Clear();
			SkCommandProcessor.Notify("Road points cleared.");
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("Points", "List Points", "Show the points marked so far", SkIcons.First("Wishbone"), delegate
		{
			SkRun.Text("Road Points", (roadPoints.Count == 0) ? "No points yet. Walk to a spot and use Add Point Here." : string.Join("\n", roadPoints.ToArray()));
		}, SkMenuController.SkScope.Client));
		list.Add(Toggle("Options", "Follow Height", "Level the road along the heights of the points. Off leaves the ground as it is", SkIcons.First("Hoe"), delegate
		{
			followHeight = !followHeight;
		}, () => followHeight, SkMenuController.SkScope.Client));
		list.Add(Toggle("Options", "Pave", "Paint the road paved", SkIcons.First("Stone"), delegate
		{
			pave = !pave;
		}, () => pave, SkMenuController.SkScope.Client));
		list.Add(Cell("Build", "Straight Road", "Straight lines between the points", SkIcons.First("Wood"), delegate
		{
			BuildRoad("path");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Build", "Smooth Road", "A smooth curve passing through every point", SkIcons.First("Honey", "Resin"), delegate
		{
			BuildRoad("curve");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Build", "Arc Road", "Circular arcs through the points", SkIcons.First("Feathers"), delegate
		{
			BuildRoad("arc");
		}, SkMenuController.SkScope.Server));
		list.Add(Cell("Build", "Undo", "Undo the last edit", SkIcons.First("Coins"), delegate
		{
			EditCompat.Run("undo", "Undid the last edit.");
		}, SkMenuController.SkScope.Client));
		List<SkMenuController.SkMenuSlider> sliders = new List<SkMenuController.SkMenuSlider>
		{
			new SkMenuController.SkMenuSlider
			{
				Label = "Half width",
				Min = 1,
				Max = 20,
				Get = () => roadWidth,
				Set = delegate(int value)
				{
					roadWidth = value;
				}
			}
		};
		SkMC.RequestGridMenu(list, sliders, "Road / Path", showFilter: false);
	}

	private void AddRoadPoint()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		Vector3 position = ((Component)localPlayer).transform.position;
		roadPoints.Add(Num(position.x) + "," + Num(position.z));
		SkCommandProcessor.Notify("Road point " + roadPoints.Count + " added.");
	}

	private void BuildRoad(string mode)
	{
		if (!HavePlayer())
		{
			return;
		}
		if (roadPoints.Count < 2)
		{
			SkCommandProcessor.Notify("Add at least two points first.");
			return;
		}
		string text = mode + (followHeight ? " level" : "") + (pave ? " paint=paved" : "") + " rect=" + roadWidth;
		foreach (string roadPoint in roadPoints)
		{
			text = text + " p=" + roadPoint;
		}
		RunTerrain(text, "Built the road.");
	}

	private void ShowObjectForm()
	{
		if (!HavePlayer())
		{
			return;
		}
		SkMenuController.SkForm skForm = NewForm("Modify Objects", "Acts on every matching object around you. Leave the id empty to match everything that is not a player, or list prefab ids separated by commas (use * as a wildcard, like Beech*). Undo reverses these.");
		skForm.Fields.Add(Slider("radius", "Radius", 1, 100, 10));
		skForm.Fields.Add(TextBox("ids", "Object ids", ""));
		skForm.Fields.Add(Slider("value", "Value", -360, 500, 100));
		skForm.Fields.Add(new SkMenuController.SkFormField
		{
			Id = "info",
			Label = "Value is",
			Kind = SkMenuController.SkFieldKind.Info,
			TextValue = "Scale: percent (100 = normal).  Move up: meters.  Rotate: degrees."
		});
		string[] array = new string[5] { "Scale", "Move up", "Rotate", "Respawn", "Info" };
		foreach (string action in array)
		{
			skForm.Actions.Add(new SkMenuController.SkFormAction
			{
				Label = action,
				Run = delegate(SkMenuController.SkForm f)
				{
					RunObject(action, f);
				}
			});
		}
		SkMC.ShowForm(skForm);
	}

	private static void RunObject(string action, SkMenuController.SkForm f)
	{
		if (!HavePlayer())
		{
			return;
		}
		string text = CleanIds(f.Field("ids").TextValue);
		string text2 = "object radius=" + f.Field("radius").IntValue + ((text.Length > 0) ? (" id=" + text) : "");
		int intValue = f.Field("value").IntValue;
		switch (action)
		{
		case "Scale":
			EditCompat.Run(text2 + " scale=" + Fraction(Mathf.Max(1, intValue)), "Scaled to " + Mathf.Max(1, intValue) + "%.");
			break;
		case "Move up":
			EditCompat.Run(text2 + " move=0,0," + intValue, "Moved objects " + intValue + "m.");
			break;
		case "Rotate":
			EditCompat.Run(text2 + " rotate=" + intValue, "Rotated objects " + intValue + " degrees.");
			break;
		case "Respawn":
			EditCompat.Run(text2 + " respawn", "Respawned chests, pickables and spawn points.");
			break;
		default:
			EditCompat.Show("Object Info", text2 + " info", "No objects matched.");
			break;
		}
	}

	private void ShowRemoveForm()
	{
		if (!HavePlayer())
		{
			return;
		}
		SkMenuController.SkForm skForm = NewForm("Remove Objects", "Deletes every matching object around you, trees, rocks and buildings included. You must name what to remove: ids separated by commas, * as a wildcard (Beech*, *Rock*). Undo can bring them back right after.");
		skForm.Warning = "This removes objects from the world for everyone on it.";
		skForm.Fields.Add(Slider("radius", "Radius", 1, 100, 10));
		skForm.Fields.Add(TextBox("ids", "Object ids", ""));
		skForm.Fields.Add(Switch("confirm", "Remove them", false));
		skForm.Validate = delegate(SkMenuController.SkForm f)
		{
			if (CleanIds(f.Field("ids").TextValue).Length == 0)
			{
				return "Type which object ids to remove.";
			}
			return f.Field("confirm").BoolValue ? null : "Switch the confirmation to ON first.";
		};
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Remove",
			Run = delegate(SkMenuController.SkForm f)
			{
				EditCompat.Run("object remove radius=" + f.Field("radius").IntValue + " id=" + CleanIds(f.Field("ids").TextValue), "Removed the objects.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowLocationForm()
	{
		if (!HavePlayer())
		{
			return;
		}
		List<string> list = new List<string>();
		if ((Object)(object)ZoneSystem.instance != (Object)null && ZoneSystem.instance.m_locations != null)
		{
			foreach (ZoneSystem.ZoneLocation location in ZoneSystem.instance.m_locations)
			{
				if (location != null && !string.IsNullOrEmpty(location.m_prefabName) && !list.Contains(location.m_prefabName))
				{
					list.Add(location.m_prefabName);
				}
			}
		}
		if (list.Count == 0)
		{
			SkCommandProcessor.Notify("Location list is not available yet. Load into a world first.");
			return;
		}
		list.Sort(StringComparer.OrdinalIgnoreCase);
		SkMenuController.SkForm skForm = NewForm("Spawn Location", "Places a point of interest in front of you. Unlike the vanilla command this keeps saving on and supports Undo.");
		skForm.Fields.Add(Choice("location", "Location", list, 230));
		skForm.Fields.Add(Slider("distance", "Distance ahead", 0, 100, 15));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Spawn",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selectedOption = f.Field("location").SelectedOption;
				if (!string.IsNullOrEmpty(selectedOption))
				{
					EditCompat.Run("spawn_location " + selectedOption + " pos=" + f.Field("distance").IntValue, "Spawned " + selectedOption + ".");
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowSelectForm()
	{
		SkMenuController.SkForm skForm = NewForm("Select by Id", "Selects any object by prefab id so the hammer can place it. Equip the hammer, then click to build. NumPad5 selects whatever you are looking at.");
		skForm.Fields.Add(TextBox("id", "Prefab id", ""));
		skForm.Fields.Add(Slider("scale", "Scale %", 10, 500, 100));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Select",
			Run = delegate(SkMenuController.SkForm f)
			{
				string text = CleanIds(f.Field("id").TextValue);
				if (text.Length == 0 || text.Contains(","))
				{
					SkCommandProcessor.Notify("Type one prefab id.");
				}
				else
				{
					int intValue = f.Field("scale").IntValue;
					EditCompat.Run("hammer " + text + ((intValue != 100) ? (" scale=" + Fraction(intValue)) : ""), "Selected " + text + ".");
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowAreaForm()
	{
		if (!HavePlayer())
		{
			return;
		}
		SkMenuController.SkForm skForm = NewForm("Copy Area", "Selects everything around you so the hammer can paste it elsewhere. Add the terrain shape and paint to copy the ground too.");
		skForm.Fields.Add(Slider("radius", "Radius", 1, 64, 10));
		skForm.Fields.Add(Switch("square", "Square instead of circle", false));
		skForm.Fields.Add(Switch("terrain", "Include terrain height", false));
		skForm.Fields.Add(Switch("paint", "Include terrain paint", false));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Select area",
			Run = delegate(SkMenuController.SkForm f)
			{
				string text = "hammer " + (f.Field("square").BoolValue ? "rect=" : "circle=") + f.Field("radius").IntValue;
				if (f.Field("terrain").BoolValue)
				{
					text += " terrain";
				}
				if (f.Field("paint").BoolValue)
				{
					text += " paint";
				}
				EditCompat.Run(text, "Area selected.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowInvulnerabilityForm()
	{
		SkMenuController.SkForm skForm = NewForm("Invulnerable Pieces", "What you build with the hammer from now on. On = cannot be damaged. Damaged and Worn only change how the structure looks. Pieces that already exist keep their state.");
		skForm.Fields.Add(Choice("mode", "Mode (now: " + EditCompat.HammerText("Set invulnerability", "Off") + ")", Invulnerability, 110));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Apply",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selectedOption = f.Field("mode").SelectedOption;
				if (!string.IsNullOrEmpty(selectedOption))
				{
					EditCompat.Run("hammer_config set_invulnerability " + selectedOption, "Invulnerability: " + selectedOption);
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowRangeForm()
	{
		int result;
		int value = (int.TryParse(EditCompat.HammerText("Hammer range", "50"), NumberStyles.Integer, CultureInfo.InvariantCulture, out result) ? result : 50);
		SkMenuController.SkForm skForm = NewForm("Hammer Range", "How far you can build and remove from. The game caps this at about 50 meters.");
		skForm.Fields.Add(Slider("range", "Meters", 5, 50, Mathf.Clamp(value, 5, 50)));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Apply",
			Run = delegate(SkMenuController.SkForm f)
			{
				EditCompat.Run("hammer_config hammer_range " + f.Field("range").IntValue, "Hammer range: " + f.Field("range").IntValue + "m");
			}
		});
		SkMC.ShowForm(skForm);
	}
}
