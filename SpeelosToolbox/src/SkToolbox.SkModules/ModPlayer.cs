using System;
using System.Collections.Generic;
using System.Text;
using SkToolbox.Utility;
using UnityEngine;

namespace SkToolbox.SkModules;

internal class ModPlayer : SkBaseModule, IModule
{
	private enum FoodKind
	{
		Health,
		Stamina,
		Eitr,
		Balanced
	}

	private bool bTeleport;

	private Rect rectClock = new Rect(5f, 5f, 125f, 20f);

	private Rect rectCoords = new Rect(5f, 27f, 125f, 20f);

	private Rect rectEnemy = new Rect(5f, 280f, 425f, 50f);

	private List<Character> nearbyCharacters = new List<Character>();

	private static readonly Dictionary<string, string> PowerTrophies = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
	{
		{ "GP_Eikthyr", "TrophyEikthyr" },
		{ "GP_TheElder", "TrophyTheElder" },
		{ "GP_Bonemass", "TrophyBonemass" },
		{ "GP_Moder", "TrophyDragonQueen" },
		{ "GP_Yagluth", "TrophyGoblinKing" },
		{ "GP_Queen", "TrophySeekerQueen" },
		{ "GP_Fader", "TrophyFader" }
	};

	private static readonly List<string> PowerOrder = new List<string> { "GP_Eikthyr", "GP_TheElder", "GP_Bonemass", "GP_Moder", "GP_Yagluth", "GP_Queen", "GP_Fader" };

	private static List<ItemDrop> foodCache;

	private static int foodCacheFor = 0;

	public ModPlayer()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		ModuleName = "Player";
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

	internal void ShowGrid()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer != (Object)null)
		{
			SkCommandProcessor.godEnabled = localPlayer.m_godMode;
			SkCommandProcessor.flyEnabled = localPlayer.m_debugFly;
			SkCommandProcessor.noCostEnabled = localPlayer.NoCostCheat();
		}
		SkMC.RequestGridMenu(BuildGrid(), null, "Player", showFilter: false, null);
	}

	private List<SkMenuController.SkGridItem> BuildGrid()
	{
		List<SkMenuController.SkGridItem> list = new List<SkMenuController.SkGridItem>();
		AddFoodCell(list, "Best Health Food", FoodKind.Health, null);
		AddFoodCell(list, "Best Stamina Food", FoodKind.Stamina, null);
		AddFoodCell(list, "Best Eitr Food", FoodKind.Eitr, null);
		AddFoodCell(list, "Balanced Food", FoodKind.Balanced, SkIcons.First("Tankard", "BarleyWine"));
		Action(list, "Character", "Heal Self", "Restore health and stamina", SkIcons.First("MeadHealthMedium", "MeadHealthMinor", "Honey"), Heal);
		Action(list, "Character", "Repair All", "Repair every item you carry", SkIcons.First("Hammer"), RepairAll);
		Action(list, "Character", "Skills", "Set or reset a skill level", SkIcons.First("Wishbone", "Coins"), ShowSkillForm);
		Action(list, "Character", "Puke", "Empty your stomach, clearing all three food slots", SkIcons.First("Entrails", "RawMeat"), delegate
		{
			SkRun.CmdNotify("puke", "Stomach emptied.");
		});
		Action(list, "Character", "Inventory Size", "Change how many rows your inventory has", SkIcons.First("ArmorLeatherChest", "ArmorRagsChest"), ShowInventoryForm);
		Action(list, "Character", "Stats", "Show what the game has recorded for this character", SkIcons.First("Ruby", "Coins"), delegate
		{
			SkRun.Show("Player Stats", "stats", "No stats to show yet.");
		});
		Action(list, "Character", "Tweaks", "Carry weight, pickup range, jump, speeds and map reveal radius", SkIcons.First("Bronze", "Copper"), ShowTuningForm);
		Action(list, "Character", "Clear Inventory", "Throw away everything you are carrying", SkIcons.First("Flint", "Stone"), ShowClearInventoryForm);
		Toggle(list, "Cheats", "Godmode", "Take no damage", SkIcons.First("HelmetOdin", "CapeOdin"), ToggleGodmode, () => (Object)(object)Player.m_localPlayer != (Object)null && Player.m_localPlayer.m_godMode);
		Toggle(list, "Cheats", "Flying", "Free flight", SkIcons.First("Feathers"), ToggleFlying, () => (Object)(object)Player.m_localPlayer != (Object)null && Player.m_localPlayer.m_debugFly);
		Toggle(list, "Cheats", "Infinite Stamina", "Nothing costs stamina: running, swimming, sneaking, fishing, attacking, jumping, dodging", SkIcons.First("MeadStaminaMedium", "MeadStaminaMinor"), ToggleInfStam, () => (Object)(object)Player.m_localPlayer != (Object)null && SkCommandProcessor.infStamina);
		Toggle(list, "Cheats", "Infinite Eitr", "Spells and staves cost nothing and eitr refills at once. Needs eitr food - without it your maximum is zero", SkIcons.First("MeadEitrMinor", "Eitr", "Sap"), ToggleInfEitr, () => (Object)(object)Player.m_localPlayer != (Object)null && SkCommandProcessor.infEitr);
		Toggle(list, "Cheats", "No Carry Limit", "Carry as much as you like, with no slowdown and no encumbered warning", SkIcons.First("ArmorBronzeChest", "ArmorLeatherChest"), ToggleNoCarryLimit, () => (Object)(object)Player.m_localPlayer != (Object)null && SkCommandProcessor.noCarryLimit);
		Toggle(list, "Cheats", "No Cost Building", "Unlock all pieces and build for free", SkIcons.First("Wood", "Stone"), ToggleNoCost, () => (Object)(object)Player.m_localPlayer != (Object)null && Player.m_localPlayer.NoCostCheat());
		Toggle(list, "Cheats", "Build Anywhere", "Remove build restrictions", SkIcons.First("Cultivator", "Hoe"), ToggleAnywhere, () => SkCommandPatcher.bBuildAnywhere);
		Toggle(list, "Cheats", "No Support Needed", "Pieces float without support. Turning this off lets unsupported builds collapse", SkIcons.First("FineWood", "RoundLog", "Wood"), ToggleNoSupport, () => SkCommandPatcher.BFreeSupport);
		Toggle(list, "Cheats", "Ghost", "Creatures cannot see you", SkIcons.First("CapeLinen", "CapeDeerHide", "Feathers"), ToggleGhost, () => (Object)(object)Player.m_localPlayer != (Object)null && Player.m_localPlayer.m_ghostMode);
		Action(list, "Cheats", "Reach", "Interact with and place things from far away", SkIcons.First("Chain", "LeatherScraps"), ShowReachForm);
		Action(list, "Cheats", "Tame", "Tame all nearby creatures", SkIcons.First("Carrot", "Raspberry"), Tame, SkMenuController.SkScope.Server);
		Toggle(list, "Cheats", "Hidden Pieces", "Add building pieces you cannot normally craft (Ashlands walls and floors, dungeon and village parts) to the Hammer menu. Turn on No Cost Building too if they ask for materials", SkIcons.First("Hammer", "Stone"), ToggleHiddenPieces, () => SkHiddenPieces.Added > 0);
		Action(list, "Cheats", "Immunities", "Choose what is allowed to hurt you, rather than god mode's all or nothing", SkIcons.First("ShieldIronTower", "ShieldWood", "HelmetOdin"), ShowImmunityForm);
		Action(list, "Cheats", "Guardian Power", "Pick a boss power by its trophy, use it, or clear its cooldown", SkIcons.First("TrophyEikthyr", "Wishbone"), ShowPowerGrid);
		Action(list, "Cheats", "Status Effects", "Apply a status effect, or clear the ones you have", SkIcons.First("MeadFrostResist", "MeadPoisonResist", "MeadHealthMinor"), ShowStatusForm);
		Toggle(list, "Info", "Teleport to Mouse", "Press tilde (~) to teleport where you look", SkIcons.First("SurtlingCore", "Thunderstone"), ToggleTeleport, () => bTeleport);
		Action(list, "Info", "Portals", "List every portal tag", SkIcons.First("SurtlingCore"), delegate
		{
			SkCommandProcessor.ProcessCommand("/portals", SkCommandProcessor.LogTo.Chat);
		});
		Toggle(list, "Info", "Display Coordinates", "Show coords in the top left corner", SkIcons.First("FishingRodFloat", "Thunderstone", "Ruby"), ToggleCoords, () => SkCommandProcessor.bCoords);
		Toggle(list, "Info", "Detect Nearby Enemies", "Range: 20m", SkIcons.First("Wishbone"), ToggleESPEnemies, () => SkCommandProcessor.bDetectEnemies);
		Action(list, "Info", "Position", "Print your coordinates, zone and distance from the centre", SkIcons.First("Amber", "Coins"), delegate
		{
			SkRun.Show("Position", "pos", "No position yet.");
		});
		Action(list, "Info", "Find Tombstone", "Pin your nearby graves on the map", SkIcons.First("AmberPearl", "Ruby"), delegate
		{
			SkCommandProcessor.ProcessCommand("/findtomb", SkCommandProcessor.LogTo.Chat);
		});
		Action(list, "Info", "Bookmarks", "Save where you are standing, and teleport back to it later", SkIcons.First("SurtlingCore", "Thunderstone"), ShowBookmarkForm);
		Action(list, "Info", "Loadouts", "Snapshot your whole inventory, and restore it later", SkIcons.First("ArmorBronzeChest", "ArmorLeatherChest", "ArmorRagsChest"), ShowLoadoutForm);
		return list;
	}

	private static void Action(List<SkMenuController.SkGridItem> grid, string section, string label, string tip, Sprite icon, Action run, SkMenuController.SkScope scope = SkMenuController.SkScope.Client)
	{
		grid.Add(new SkMenuController.SkGridItem
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
		});
	}

	private static void Toggle(List<SkMenuController.SkGridItem> grid, string section, string label, string tip, Sprite icon, Action run, Func<bool> isOn, SkMenuController.SkScope scope = SkMenuController.SkScope.Client)
	{
		grid.Add(new SkMenuController.SkGridItem
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
			},
			IsOn = isOn
		});
	}

	private void ToggleHiddenPieces()
	{
		if (SkHiddenPieces.Added > 0)
		{
			int num = SkHiddenPieces.Remove();
			SkCommandProcessor.Notify("Removed " + num + " hidden pieces from the Hammer menu.");
			return;
		}
		int num2 = SkHiddenPieces.Apply();
		if (num2 < 0)
		{
			SkCommandProcessor.Notify("The piece list is not ready yet. Load into a world first.");
		}
		else
		{
			SkCommandProcessor.Notify("Added " + num2 + " pieces to the Hammer menu (" + SkHiddenPieces.SnapPointsMade + " snap points made). Open the hammer and look in the Misc tab.");
		}
	}

	internal void ShowEspGrid()
	{
		List<SkMenuController.SkGridItem> list = new List<SkMenuController.SkGridItem>();
		Toggle(list, "Show", "Players", "Name and distance of other players", SkIcons.First("HelmetOdin", "CapeOdin"), delegate
		{
			SkEsp.Players = !SkEsp.Players;
		}, () => SkEsp.Players);
		Toggle(list, "Show", "Creatures", "Every creature, tamed ones in orange", SkIcons.First("TrophyDeer", "TrophyBoar"), delegate
		{
			SkEsp.Monsters = !SkEsp.Monsters;
		}, () => SkEsp.Monsters);
		Toggle(list, "Show", "Pickables", "Berries, mushrooms, flowers, branches, stones", SkIcons.First("Raspberry", "Blueberries"), delegate
		{
			SkEsp.Pickables = !SkEsp.Pickables;
		}, () => SkEsp.Pickables);
		Toggle(list, "Show", "Ore Deposits", "Copper, tin, iron, silver and other deposits", SkIcons.First("CopperOre", "TinOre", "IronScrap"), delegate
		{
			SkEsp.Deposits = !SkEsp.Deposits;
		}, () => SkEsp.Deposits);
		Toggle(list, "Show", "Dropped Items", "Items lying on the ground", SkIcons.First("Wood", "Stone"), delegate
		{
			SkEsp.Drops = !SkEsp.Drops;
		}, () => SkEsp.Drops);
		Toggle(list, "Range", "Limit Distance", "Only label things within the distance below. Off labels everything that is loaded", SkIcons.First("Chain", "LeatherScraps"), delegate
		{
			SkEsp.LimitRadius = !SkEsp.LimitRadius;
		}, () => SkEsp.LimitRadius);
		List<SkMenuController.SkMenuSlider> sliders = new List<SkMenuController.SkMenuSlider>
		{
			new SkMenuController.SkMenuSlider
			{
				Label = "Distance (m)",
				Min = 5,
				Max = 500,
				Get = () => SkEsp.Radius,
				Set = delegate(int value)
				{
					SkEsp.Radius = value;
				}
			}
		};
		SkMC.RequestGridMenu(list, sliders, "ESP", showFilter: false);
	}

	internal void ShowRemoveTombstonesForm()
	{
		SkMenuController.SkForm skForm = new SkMenuController.SkForm
		{
			Title = "Remove My Tombstones",
			Note = "Deletes the tombstones of your own character that are loaded near you. Anything still inside them is gone for good, so pick your things up first.",
			Warning = "This destroys the items left in those graves."
		};
		skForm.Fields.Add(new SkMenuController.SkFormField
		{
			Id = "confirm",
			Label = "Delete them",
			Kind = SkMenuController.SkFieldKind.Toggle
		});
		skForm.Validate = delegate(SkMenuController.SkForm f)
		{
			SkMenuController.SkFormField skFormField = f.Field("confirm");
			return (skFormField != null && skFormField.BoolValue) ? null : "Switch the confirmation to ON first.";
		};
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Remove",
			Run = delegate
			{
				int num = SkTooler.RemoveOwnTombstones();
				SkCommandProcessor.Notify((num > 0) ? ("Removed " + num + " tombstone(s).") : "No tombstones of yours are loaded nearby.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowSkillForm()
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Invalid comparison between Unknown and I4
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		SkMenuController.SkFormField skFormField = new SkMenuController.SkFormField
		{
			Id = "skill",
			Label = "Skill",
			Kind = SkMenuController.SkFieldKind.Choice
		};
		skFormField.Options.Add("All");
		skFormField.OptionLabels.Add("All skills");
		Skills skills = ((Character)localPlayer).GetSkills();
		foreach (SkillType value in Enum.GetValues(typeof(SkillType)))
		{
			SkillType val = value;
			if ((int)val != 0 && (int)val != 999)
			{
				float num = 0f;
				try
				{
					num = (((Object)(object)skills != (Object)null) ? skills.GetSkillLevel(val) : 0f);
				}
				catch (Exception)
				{
				}
				skFormField.Options.Add(((object)val).ToString());
				skFormField.OptionLabels.Add(((object)val).ToString() + "     <color=#9FB6CC>level " + Mathf.RoundToInt(num) + "</color>");
			}
		}
		skFormField.Selected = 1;
		SkMenuController.SkFormField item = new SkMenuController.SkFormField
		{
			Id = "level",
			Label = "Level to set",
			Kind = SkMenuController.SkFieldKind.IntSlider,
			Min = 0,
			Max = 100,
			IntValue = 50
		};
		SkMenuController.SkForm skForm = new SkMenuController.SkForm
		{
			Title = "Skills",
			Note = "Pick a skill, choose a level, then Set. Reset puts it back to zero."
		};
		skForm.Fields.Add(skFormField);
		skForm.Fields.Add(item);
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Set level",
			Run = delegate(SkMenuController.SkForm f)
			{
				SetSkill(f.Field("skill").SelectedOption, f.Field("level").IntValue);
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Reset to zero",
			Run = delegate(SkMenuController.SkForm f)
			{
				ResetSkill(f.Field("skill").SelectedOption);
			}
		});
		SkMC.ShowForm(skForm);
	}

	private static void SetSkill(string name, int target)
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Invalid comparison between Unknown and I4
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null || string.IsNullOrEmpty(name))
		{
			return;
		}
		Skills skills = ((Character)localPlayer).GetSkills();
		if ((Object)(object)skills == (Object)null)
		{
			return;
		}
		if (name == "All")
		{
			foreach (SkillType value in Enum.GetValues(typeof(SkillType)))
			{
				SkillType val = value;
				if ((int)val != 0 && (int)val != 999)
				{
					skills.CheatRaiseSkill(((object)val).ToString(), (float)target - skills.GetSkillLevel(val), false);
				}
			}
			SkCommandProcessor.Notify("All skills set to " + target);
		}
		else
		{
			SkillType val2;
			try
			{
				val2 = (SkillType)Enum.Parse(typeof(SkillType), name, ignoreCase: true);
			}
			catch (Exception)
			{
				SkCommandProcessor.Notify("Unknown skill: " + name);
				return;
			}
			skills.CheatRaiseSkill(name, (float)target - skills.GetSkillLevel(val2), false);
			SkCommandProcessor.Notify(name + " set to " + target);
		}
	}

	private static void ResetSkill(string name)
	{
		Player localPlayer = Player.m_localPlayer;
		if (!((Object)(object)localPlayer == (Object)null) && !string.IsNullOrEmpty(name))
		{
			Skills skills = ((Character)localPlayer).GetSkills();
			if (!((Object)(object)skills == (Object)null))
			{
				skills.CheatResetSkill((name == "All") ? "all" : name);
				SkCommandProcessor.Notify(((name == "All") ? "All skills" : name) + " reset to zero");
			}
		}
	}

	private void ShowImmunityForm()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Invalid comparison between Unknown and I4
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected I4, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected I4, but got Unknown
		SkMenuController.SkFormField skFormField = new SkMenuController.SkFormField
		{
			Id = "types",
			Label = "Cannot hurt me",
			Kind = SkMenuController.SkFieldKind.Checklist,
			Height = 210
		};
		foreach (HitType value in Enum.GetValues(typeof(HitType)))
		{
			HitType val = value;
			if ((int)val != 0 && (int)val != 1 && (int)val != 2)
			{
				skFormField.Options.Add(((int)val).ToString());
				skFormField.OptionLabels.Add(Spaced(((object)val).ToString()));
				skFormField.Checked.Add(SkCommandProcessor.immuneTo.Contains((int)val));
			}
		}
		SkMenuController.SkForm skForm = new SkMenuController.SkForm
		{
			Title = "Immunities",
			Note = "Anything switched on here cannot damage you. Creature and player attacks are deliberately not listed - turning those off is what god mode is for."
		};
		skForm.Fields.Add(skFormField);
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Apply",
			Run = delegate(SkMenuController.SkForm f)
			{
				SkMenuController.SkFormField skFormField3 = f.Field("types");
				SkCommandProcessor.immuneTo.Clear();
				int num = 0;
				for (int j = 0; j < skFormField3.Options.Count && j < skFormField3.Checked.Count; j++)
				{
					if (skFormField3.Checked[j] && int.TryParse(skFormField3.Options[j], out var result2))
					{
						SkCommandProcessor.immuneTo.Add(result2);
						num++;
					}
				}
				SkCommandProcessor.Notify((num > 0) ? ("Immune to " + num + " kinds of damage.") : "No immunities set.");
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "All on",
			Run = delegate(SkMenuController.SkForm f)
			{
				SkMenuController.SkFormField skFormField2 = f.Field("types");
				SkCommandProcessor.immuneTo.Clear();
				for (int i = 0; i < skFormField2.Options.Count; i++)
				{
					if (int.TryParse(skFormField2.Options[i], out var result))
					{
						SkCommandProcessor.immuneTo.Add(result);
					}
				}
				SkCommandProcessor.Notify("Nothing but combat can hurt you now.");
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "All off",
			Run = delegate
			{
				SkCommandProcessor.immuneTo.Clear();
				SkCommandProcessor.Notify("Immunities cleared.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private static string Spaced(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder(name.Length + 4);
		for (int i = 0; i < name.Length; i++)
		{
			if (i > 0 && char.IsUpper(name[i]) && !char.IsUpper(name[i - 1]))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append(name[i]);
		}
		return stringBuilder.ToString();
	}

	private void ShowPowerGrid()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		List<StatusEffect> list = new List<StatusEffect>();
		ObjectDB instance = ObjectDB.instance;
		if ((Object)(object)instance != (Object)null && instance.m_StatusEffects != null)
		{
			foreach (StatusEffect statusEffect in instance.m_StatusEffects)
			{
				if (!((Object)(object)statusEffect == (Object)null) && !string.IsNullOrEmpty(((Object)statusEffect).name) && ((Object)statusEffect).name.StartsWith("GP_", StringComparison.OrdinalIgnoreCase))
				{
					list.Add(statusEffect);
				}
			}
		}
		if (list.Count == 0)
		{
			SkCommandProcessor.Notify("Power list is not available yet. Load into a world first.");
			return;
		}
		list.Sort(delegate(StatusEffect a, StatusEffect b)
		{
			int num = PowerOrder.IndexOf(((Object)a).name);
			if (num < 0)
			{
				num = int.MaxValue;
			}
			int num2 = PowerOrder.IndexOf(((Object)b).name);
			if (num2 < 0)
			{
				num2 = int.MaxValue;
			}
			return (num == num2) ? string.Compare(((Object)a).name, ((Object)b).name, StringComparison.OrdinalIgnoreCase) : num.CompareTo(num2);
		});
		List<SkMenuController.SkGridItem> list2 = new List<SkMenuController.SkGridItem>();
		foreach (StatusEffect item in list)
		{
			string gp = ((Object)item).name;
			string label = PowerLabel(item);
			string value;
			Sprite icon = (PowerTrophies.TryGetValue(gp, out value) ? SkIcons.First(value, "Wishbone") : SkIcons.First("Wishbone"));
			list2.Add(new SkMenuController.SkGridItem
			{
				Name = gp,
				Display = label,
				Tip = label + "  -  set this as your power. The ring marks the one you have",
				Icon = icon,
				Section = "Powers",
				Scope = SkMenuController.SkScope.Client,
				IsOn = delegate
				{
					Player localPlayer2 = Player.m_localPlayer;
					return (Object)(object)localPlayer2 != (Object)null && localPlayer2.GetGuardianPowerName() == gp;
				},
				OnClick = delegate
				{
					SetPower(gp, label);
				}
			});
		}
		Action(list2, "Actions", "Use now", "Fire the power you have set. Like praying at the stone, it also buffs any player within 10m", SkIcons.First("Wishbone", "TrophyEikthyr"), UsePower, SkMenuController.SkScope.Server);
		Action(list2, "Actions", "Clear cooldown", "Make the power ready again without waiting", SkIcons.First("MeadStaminaMedium", "Coins"), ClearPowerCooldown);
		SkMC.RequestGridMenu(list2, null, "Guardian Power", showFilter: false, null);
	}

	private static string PowerLabel(StatusEffect effect)
	{
		try
		{
			if (Localization.instance != null && !string.IsNullOrEmpty(effect.m_name))
			{
				string text = Localization.instance.Localize(effect.m_name);
				if (!string.IsNullOrEmpty(text) && !text.StartsWith("["))
				{
					return text;
				}
			}
		}
		catch (Exception)
		{
		}
		return ((Object)effect).name;
	}

	private static void SetPower(string gp, string label)
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		localPlayer.SetGuardianPower(gp);
		localPlayer.m_guardianPowerCooldown = 0f;
		SkCommandProcessor.Notify("Power set: " + label);
	}

	private static void UsePower()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		if (string.IsNullOrEmpty(localPlayer.GetGuardianPowerName()))
		{
			SkCommandProcessor.Notify("No power set - pick a trophy first.");
			return;
		}
		localPlayer.m_guardianPowerCooldown = 0f;
		localPlayer.StartGuardianPower();
		localPlayer.ActivateGuardianPower();
		SkCommandProcessor.Notify("Power used.");
	}

	private static void ClearPowerCooldown()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		if (localPlayer.m_guardianPowerCooldown <= 0f)
		{
			SkCommandProcessor.Notify(string.IsNullOrEmpty(localPlayer.GetGuardianPowerName()) ? "No power set yet." : "Power is already ready.");
			return;
		}
		localPlayer.m_guardianPowerCooldown = 0f;
		SkCommandProcessor.Notify("Power is ready again.");
	}

	private void ShowBookmarkForm()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		List<SkBookmarks.Bookmark> list = SkBookmarks.ForThisWorld();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		foreach (SkBookmarks.Bookmark item in list)
		{
			list2.Add(item.Name);
			list3.Add(item.Name + "     <color=#9FB6CC>" + Mathf.RoundToInt(item.Pos.x) + ", " + Mathf.RoundToInt(item.Pos.z) + "</color>");
		}
		SkMenuController.SkForm skForm = new SkMenuController.SkForm
		{
			Title = "Bookmarks",
			Note = ((list.Count > 0) ? "Places you saved in this world." : "Nothing saved in this world yet. Press Save here to name your current spot.")
		};
		if (list2.Count > 0)
		{
			skForm.Fields.Add(new SkMenuController.SkFormField
			{
				Id = "mark",
				Label = "Saved places",
				Kind = SkMenuController.SkFieldKind.Choice,
				Height = 180
			});
			skForm.Field("mark").Options.AddRange(list2);
			skForm.Field("mark").OptionLabels.AddRange(list3);
		}
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Save here",
			Run = delegate
			{
				SkMC.ShowPrompt(new SkMenuController.SkPrompt
				{
					Title = "Save bookmark",
					Label = "A name for where you are standing.",
					Accept = "Save",
					OnAccept = delegate(string name)
					{
						//IL_0034: Unknown result type (might be due to invalid IL or missing references)
						Player localPlayer3 = Player.m_localPlayer;
						if ((Object)(object)localPlayer3 == (Object)null)
						{
							SkCommandProcessor.Notify("No player yet.");
						}
						else if (name.Length == 0)
						{
							SkCommandProcessor.Notify("Give the bookmark a name first.");
						}
						else
						{
							SkCommandProcessor.Notify(SkBookmarks.Save(name, ((Component)localPlayer3).transform.position) ? ("Saved bookmark: " + name) : "Load into a world first.");
							ShowBookmarkForm();
						}
					}
				});
			}
		});
		if (list2.Count > 0)
		{
			skForm.Actions.Add(new SkMenuController.SkFormAction
			{
				Label = "Teleport",
				Run = delegate(SkMenuController.SkForm f)
				{
					//IL_004f: Unknown result type (might be due to invalid IL or missing references)
					//IL_005a: Unknown result type (might be due to invalid IL or missing references)
					Player localPlayer2 = Player.m_localPlayer;
					string selectedOption2 = f.Field("mark").SelectedOption;
					if ((Object)(object)localPlayer2 == (Object)null || string.IsNullOrEmpty(selectedOption2))
					{
						return;
					}
					foreach (SkBookmarks.Bookmark item2 in SkBookmarks.ForThisWorld())
					{
						if (string.Equals(item2.Name, selectedOption2, StringComparison.OrdinalIgnoreCase))
						{
							((Character)localPlayer2).TeleportTo(item2.Pos, ((Component)localPlayer2).transform.rotation, true);
							SkCommandProcessor.Notify("Teleporting to " + item2.Name);
							break;
						}
					}
				}
			});
			skForm.Actions.Add(new SkMenuController.SkFormAction
			{
				Label = "Delete",
				Run = delegate(SkMenuController.SkForm f)
				{
					string selectedOption = f.Field("mark").SelectedOption;
					if (!string.IsNullOrEmpty(selectedOption))
					{
						SkBookmarks.Delete(selectedOption);
						SkCommandProcessor.Notify("Deleted bookmark: " + selectedOption);
					}
				}
			});
		}
		SkMC.ShowForm(skForm);
	}

	private void ShowLoadoutForm()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		List<string> list = SkLoadouts.Names();
		SkMenuController.SkForm skForm = new SkMenuController.SkForm
		{
			Title = "Loadouts",
			Note = "Saves your whole inventory, not just what you are wearing: every stack, its size, quality, durability and crafter, and which pieces were equipped. Loading one puts all of it back."
		};
		if (list.Count > 0)
		{
			skForm.Warning = "Loading a set REPLACES everything you are carrying.";
			SkMenuController.SkFormField skFormField = new SkMenuController.SkFormField
			{
				Id = "set",
				Label = "Saved sets",
				Kind = SkMenuController.SkFieldKind.Choice,
				Height = 150
			};
			foreach (string item in list)
			{
				skFormField.Options.Add(item);
				int num = SkLoadouts.CountOf(item);
				skFormField.OptionLabels.Add((num >= 0) ? (item + "     <color=#9FB6CC>" + num + " stacks</color>") : item);
			}
			skForm.Fields.Add(skFormField);
			skForm.Fields.Add(new SkMenuController.SkFormField
			{
				Id = "confirm",
				Label = "Replace what I am carrying",
				Kind = SkMenuController.SkFieldKind.Toggle
			});
		}
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Save worn",
			Run = delegate
			{
				SkMC.ShowPrompt(new SkMenuController.SkPrompt
				{
					Title = "Save loadout",
					Label = "A name for your inventory as it is now.",
					Accept = "Save",
					OnAccept = delegate(string name)
					{
						Player localPlayer3 = Player.m_localPlayer;
						if ((Object)(object)localPlayer3 == (Object)null)
						{
							SkCommandProcessor.Notify("No player yet.");
						}
						else if (name.Length == 0)
						{
							SkCommandProcessor.Notify("Give the set a name first.");
						}
						else
						{
							int num2 = SkLoadouts.Save(name, localPlayer3);
							SkCommandProcessor.Notify((num2 > 0) ? ("Saved " + num2 + " stacks as: " + name) : "Your inventory is empty, nothing to save.");
							ShowLoadoutForm();
						}
					}
				});
			}
		});
		if (list.Count > 0)
		{
			skForm.Validate = delegate(SkMenuController.SkForm f)
			{
				SkMenuController.SkFormField skFormField2 = f.Field("confirm");
				return (skFormField2 == null || skFormField2.BoolValue) ? null : "Switch the confirmation to ON to load a set.";
			};
			skForm.Actions.Add(new SkMenuController.SkFormAction
			{
				Label = "Load set",
				Run = delegate(SkMenuController.SkForm f)
				{
					Player localPlayer2 = Player.m_localPlayer;
					string selectedOption2 = f.Field("set").SelectedOption;
					if (!((Object)(object)localPlayer2 == (Object)null) && !string.IsNullOrEmpty(selectedOption2))
					{
						if (!SkLoadouts.Restore(selectedOption2, localPlayer2, out var stacks, out var equipped))
						{
							SkCommandProcessor.Notify("Could not read that set.");
						}
						else
						{
							SkCommandProcessor.Notify("Restored " + stacks + " stacks, " + equipped + " equipped.");
						}
					}
				}
			});
			skForm.Actions.Add(new SkMenuController.SkFormAction
			{
				Label = "Delete",
				Run = delegate(SkMenuController.SkForm f)
				{
					string selectedOption = f.Field("set").SelectedOption;
					if (!string.IsNullOrEmpty(selectedOption))
					{
						SkLoadouts.Delete(selectedOption);
						SkCommandProcessor.Notify("Deleted set: " + selectedOption);
					}
				}
			});
		}
		SkMC.ShowForm(skForm);
	}

	private void ShowTuningForm()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		SkCommandProcessor.CaptureTuningBaseline(localPlayer);
		SkMenuController.SkForm skForm = new SkMenuController.SkForm
		{
			Title = "Tweaks",
			Note = "Each slider starts where the game has it now. These are restored after you die, unless PersistCheatsOnDeath is off."
		};
		skForm.Fields.Add(Slider("carry", "Carry weight", 50, 2000, (SkCommandProcessor.carryWeight > 0) ? SkCommandProcessor.carryWeight : Mathf.RoundToInt(localPlayer.m_maxCarryWeight)));
		skForm.Fields.Add(Slider("pickup", "Auto pickup range", 2, 20, (SkCommandProcessor.pickupRange > 0) ? SkCommandProcessor.pickupRange : Mathf.RoundToInt(localPlayer.m_autoPickupRange)));
		skForm.Fields.Add(Slider("jump", "Jump force", 4, 40, (SkCommandProcessor.jumpForce > 0) ? SkCommandProcessor.jumpForce : Mathf.RoundToInt(((Character)localPlayer).m_jumpForce)));
		skForm.Fields.Add(Slider("run", "Run speed", 2, 40, (SkCommandProcessor.runSpeed > 0) ? SkCommandProcessor.runSpeed : Mathf.RoundToInt(((Character)localPlayer).m_runSpeed)));
		skForm.Fields.Add(Slider("swim", "Swim speed", 1, 20, (SkCommandProcessor.swimSpeed > 0) ? SkCommandProcessor.swimSpeed : Mathf.RoundToInt(((Character)localPlayer).m_swimSpeed)));
		skForm.Fields.Add(Slider("explore", "Map reveal radius", 50, 500, (SkCommandProcessor.exploreRadius > 0) ? SkCommandProcessor.exploreRadius : (((Object)(object)Minimap.instance != (Object)null) ? Mathf.RoundToInt(Minimap.instance.m_exploreRadius) : 100)));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Apply",
			Run = delegate(SkMenuController.SkForm f)
			{
				Player localPlayer2 = Player.m_localPlayer;
				if ((Object)(object)localPlayer2 == (Object)null)
				{
					SkCommandProcessor.Notify("No player yet.");
				}
				else
				{
					SkCommandProcessor.carryWeight = f.Field("carry").IntValue;
					SkCommandProcessor.pickupRange = f.Field("pickup").IntValue;
					SkCommandProcessor.jumpForce = f.Field("jump").IntValue;
					SkCommandProcessor.runSpeed = f.Field("run").IntValue;
					SkCommandProcessor.swimSpeed = f.Field("swim").IntValue;
					SkCommandProcessor.exploreRadius = f.Field("explore").IntValue;
					SkCommandProcessor.ApplyTuning(localPlayer2);
					SkCommandProcessor.Notify("Tweaks applied.");
				}
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Defaults",
			Run = delegate
			{
				SkCommandProcessor.ResetTuning(Player.m_localPlayer);
				SkCommandProcessor.Notify("Tweaks back to normal.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowReachForm()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		SkMenuController.SkForm skForm = new SkMenuController.SkForm
		{
			Title = "Reach",
			Note = "How far away you can interact with things and place pieces. Normal is about 5."
		};
		skForm.Fields.Add(Slider("reach", "Reach", 20, 100, Mathf.RoundToInt(SkCommandProcessor.farInteractDistance)));
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Apply",
			Run = delegate(SkMenuController.SkForm f)
			{
				Player localPlayer3 = Player.m_localPlayer;
				if ((Object)(object)localPlayer3 == (Object)null)
				{
					SkCommandProcessor.Notify("No player yet.");
				}
				else
				{
					SkCommandProcessor.farInteractDistance = f.Field("reach").IntValue;
					SkCommandProcessor.farInteract = true;
					SkCommandProcessor.ApplyFarInteract(localPlayer3, applyOffValues: true);
					SkCommandProcessor.Notify("Reach set to " + f.Field("reach").IntValue);
				}
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Back to normal",
			Run = delegate
			{
				Player localPlayer2 = Player.m_localPlayer;
				if ((Object)(object)localPlayer2 == (Object)null)
				{
					SkCommandProcessor.Notify("No player yet.");
				}
				else
				{
					SkCommandProcessor.farInteract = false;
					SkCommandProcessor.ApplyFarInteract(localPlayer2, applyOffValues: true);
					SkCommandProcessor.Notify("Reach back to normal.");
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowClearInventoryForm()
	{
		SkMenuController.SkForm skForm = new SkMenuController.SkForm
		{
			Title = "Clear Inventory",
			Warning = "This DESTROYS every item you are carrying.",
			Note = "Equipped gear included, and everything in any extra slots you have. Nothing is dropped on the ground, no tombstone is left, and there is no way to get any of it back. Put anything you want to keep in a chest first."
		};
		skForm.Fields.Add(new SkMenuController.SkFormField
		{
			Id = "confirm",
			Label = "I understand everything will be destroyed",
			Kind = SkMenuController.SkFieldKind.Toggle
		});
		skForm.Validate = delegate(SkMenuController.SkForm f)
		{
			SkMenuController.SkFormField skFormField = f.Field("confirm");
			return (skFormField == null || skFormField.BoolValue) ? null : "Switch the confirmation to ON first.";
		};
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Destroy everything",
			Run = delegate
			{
				SkCommandProcessor.ProcessCommand("/clearinventory", SkCommandProcessor.LogTo.Chat);
			}
		});
		SkMC.ShowForm(skForm);
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
			IntValue = Mathf.Clamp(value, min, max)
		};
	}

	public void ToggleNoSupport()
	{
		SkCommandProcessor.ProcessCommand("/nosup", SkCommandProcessor.LogTo.Chat);
	}

	public void ToggleGhost()
	{
		SkCommandProcessor.ProcessCommand("/ghost", SkCommandProcessor.LogTo.Chat);
	}

	private void ShowInventoryForm()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		int num = 4;
		try
		{
			num = ((Humanoid)localPlayer).GetInventory().GetHeight();
		}
		catch (Exception)
		{
		}
		SkMenuController.SkForm skForm = new SkMenuController.SkForm
		{
			Title = "Inventory Size",
			Note = "Rows in your inventory. Four is the normal size; shrinking it drops whatever no longer fits."
		};
		skForm.Fields.Add(new SkMenuController.SkFormField
		{
			Id = "rows",
			Label = "Rows",
			Kind = SkMenuController.SkFieldKind.IntSlider,
			Min = 1,
			Max = 9,
			IntValue = Mathf.Clamp(num, 1, 9)
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Apply",
			Run = delegate(SkMenuController.SkForm f)
			{
				SkRun.CmdNotify("inventorysize " + f.Field("rows").IntValue, "Inventory set to " + f.Field("rows").IntValue + " rows.");
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Back to 4",
			Run = delegate
			{
				SkRun.CmdNotify("inventorysize 4", "Inventory back to 4 rows.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowStatusForm()
	{
		ObjectDB instance = ObjectDB.instance;
		List<string> list = new List<string>();
		if ((Object)(object)instance != (Object)null && instance.m_StatusEffects != null)
		{
			foreach (StatusEffect statusEffect in instance.m_StatusEffects)
			{
				if ((Object)(object)statusEffect != (Object)null && !string.IsNullOrEmpty(((Object)statusEffect).name) && !list.Contains(((Object)statusEffect).name))
				{
					list.Add(((Object)statusEffect).name);
				}
			}
		}
		if (list.Count == 0)
		{
			SkCommandProcessor.Notify("Status effect list is not available yet. Load into a world first.");
			return;
		}
		list.Sort(StringComparer.OrdinalIgnoreCase);
		SkMenuController.SkFormField skFormField = new SkMenuController.SkFormField
		{
			Id = "status",
			Label = "Status effect",
			Kind = SkMenuController.SkFieldKind.Choice,
			Height = 220
		};
		skFormField.Options.AddRange(list);
		SkMenuController.SkForm skForm = new SkMenuController.SkForm
		{
			Title = "Status Effects",
			Note = "Adds one effect at its normal duration. Clearing removes everything you currently have, food included."
		};
		skForm.Fields.Add(skFormField);
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Add effect",
			Run = delegate(SkMenuController.SkForm f)
			{
				string selectedOption = f.Field("status").SelectedOption;
				if (!string.IsNullOrEmpty(selectedOption))
				{
					Player localPlayer2 = Player.m_localPlayer;
					if ((Object)(object)localPlayer2 == (Object)null)
					{
						SkCommandProcessor.Notify("No player yet.");
					}
					else
					{
						((Character)localPlayer2).GetSEMan().AddStatusEffect(StringExtensionMethods.GetStableHashCode(selectedOption), true, 0, 0f, (short)(-1));
						SkCommandProcessor.Notify("Added " + selectedOption);
					}
				}
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Clear all",
			Run = delegate
			{
				Player localPlayer = Player.m_localPlayer;
				if ((Object)(object)localPlayer == (Object)null)
				{
					SkCommandProcessor.Notify("No player yet.");
				}
				else
				{
					localPlayer.ClearHardDeath();
					((Character)localPlayer).GetSEMan().RemoveAllStatusEffects(false);
					SkCommandProcessor.Notify("Cleared status effects.");
				}
			}
		});
		SkMC.ShowForm(skForm);
	}

	private static List<ItemDrop> AllFoods()
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Invalid comparison between Unknown and I4
		ObjectDB instance = ObjectDB.instance;
		if ((Object)(object)instance == (Object)null || instance.m_items == null)
		{
			return new List<ItemDrop>();
		}
		int instanceID = ((Object)instance).GetInstanceID();
		if (foodCache != null && foodCacheFor == instanceID)
		{
			return foodCache;
		}
		List<ItemDrop> list = new List<ItemDrop>();
		foreach (GameObject item in instance.m_items)
		{
			if ((Object)(object)item == (Object)null)
			{
				continue;
			}
			ItemDrop component = item.GetComponent<ItemDrop>();
			if (!((Object)(object)component == (Object)null) && component.m_itemData != null && component.m_itemData.m_shared != null)
			{
				SharedData shared = component.m_itemData.m_shared;
				if ((int)shared.m_itemType == 2 && (!(shared.m_food <= 0f) || !(shared.m_foodStamina <= 0f) || !(shared.m_foodEitr <= 0f)) && !(shared.m_foodBurnTime <= 0f))
				{
					component.m_itemData.m_dropPrefab = item;
					list.Add(component);
				}
			}
		}
		foodCache = list;
		foodCacheFor = instanceID;
		return foodCache;
	}

	private static float Stat(ItemDrop drop, FoodKind kind)
	{
		SharedData shared = drop.m_itemData.m_shared;
		return kind switch
		{
			FoodKind.Health => shared.m_food, 
			FoodKind.Stamina => shared.m_foodStamina, 
			FoodKind.Eitr => shared.m_foodEitr, 
			_ => shared.m_food + shared.m_foodStamina + shared.m_foodEitr, 
		};
	}

	private static ItemDrop Best(List<ItemDrop> pool, FoodKind kind, List<ItemDrop> taken)
	{
		ItemDrop val = null;
		float num = 0f;
		foreach (ItemDrop item in pool)
		{
			if (Stat(item, kind) <= 0f)
			{
				continue;
			}
			bool flag = false;
			foreach (ItemDrop item2 in taken)
			{
				if (item2.m_itemData.m_shared.m_name == item.m_itemData.m_shared.m_name)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				float num2 = Stat(item, kind);
				if ((Object)(object)val == (Object)null || num2 > num)
				{
					val = item;
					num = num2;
				}
			}
		}
		return val;
	}

	private static List<ItemDrop> PickFoods(FoodKind kind)
	{
		List<ItemDrop> list = AllFoods();
		List<ItemDrop> list2 = new List<ItemDrop>();
		if (list.Count == 0)
		{
			return list2;
		}
		if (kind == FoodKind.Balanced)
		{
			FoodKind[] array = new FoodKind[3]
			{
				FoodKind.Health,
				FoodKind.Stamina,
				FoodKind.Eitr
			};
			foreach (FoodKind kind2 in array)
			{
				ItemDrop val = Best(list, kind2, list2);
				if ((Object)(object)val != (Object)null)
				{
					list2.Add(val);
				}
			}
		}
		else
		{
			while (list2.Count < 3)
			{
				ItemDrop val2 = Best(list, kind, list2);
				if ((Object)(object)val2 == (Object)null)
				{
					break;
				}
				list2.Add(val2);
			}
		}
		return list2;
	}

	private static string FoodName(ItemDrop drop)
	{
		try
		{
			if (Localization.instance != null)
			{
				string text = Localization.instance.Localize(drop.m_itemData.m_shared.m_name);
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
		}
		catch (Exception)
		{
		}
		return ((Object)drop).name;
	}

	private static void Feed(FoodKind kind)
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		List<ItemDrop> list = PickFoods(kind);
		if (list.Count == 0)
		{
			SkCommandProcessor.Notify("No food found for that. Load into a world first.");
			return;
		}
		localPlayer.ClearFood();
		List<string> list2 = new List<string>();
		foreach (ItemDrop item in list)
		{
			ItemData val = item.m_itemData.Clone();
			val.m_dropPrefab = item.m_itemData.m_dropPrefab;
			val.m_stack = 1;
			if (localPlayer.EatFood(val))
			{
				list2.Add(FoodName(item));
			}
		}
		SkCommandProcessor.Notify((list2.Count > 0) ? ("Ate: " + string.Join(", ", list2.ToArray())) : "Could not eat anything.");
	}

	private static Sprite IconOf(ItemDrop drop)
	{
		if ((Object)(object)drop == (Object)null || drop.m_itemData == null || drop.m_itemData.m_shared == null)
		{
			return null;
		}
		Sprite[] icons = drop.m_itemData.m_shared.m_icons;
		if (icons == null || icons.Length == 0)
		{
			return null;
		}
		return icons[0];
	}

	private void AddFoodCell(List<SkMenuController.SkGridItem> grid, string label, FoodKind kind, Sprite iconOverride)
	{
		List<ItemDrop> list = PickFoods(kind);
		string tip = ((list.Count == 0) ? (label + "  -  fills all three food slots") : (label + "  -  " + string.Join(", ", list.ConvertAll(FoodName).ToArray())));
		grid.Add(new SkMenuController.SkGridItem
		{
			Name = label,
			Display = label,
			Tip = tip,
			Icon = (((Object)(object)iconOverride != (Object)null) ? iconOverride : ((list.Count > 0) ? IconOf(list[0]) : null)),
			Section = "Character",
			Scope = SkMenuController.SkScope.Client,
			OnClick = delegate
			{
				Feed(kind);
			}
		});
	}

	public void ToggleTeleport()
	{
		bTeleport = !bTeleport;
		if (bTeleport)
		{
			((Character)Player.m_localPlayer).Message((MessageType)1, "Teleport enabled. Press tilde (~)!", 0, (Sprite)null, false);
		}
		else
		{
			((Character)Player.m_localPlayer).Message((MessageType)1, "Teleport disabled.", 0, (Sprite)null, false);
		}
		BeginMenu();
	}

	public void Heal()
	{
		SkCommandProcessor.ProcessCommand("/heal", SkCommandProcessor.LogTo.Chat);
	}

	public void Tame()
	{
		SkCommandProcessor.ProcessCommand("/tame", SkCommandProcessor.LogTo.Chat);
	}

	public void ToggleNoCost()
	{
		SkCommandProcessor.ProcessCommand("/nocost", SkCommandProcessor.LogTo.Chat);
		BeginMenu();
	}

	public void ToggleESPEnemies()
	{
		SkCommandProcessor.ProcessCommand("/detect 20", SkCommandProcessor.LogTo.Chat);
		BeginMenu();
	}

	public void ToggleCoords()
	{
		SkCommandProcessor.ProcessCommand("/coords", SkCommandProcessor.LogTo.Chat);
		BeginMenu();
	}

	public void ToggleAnywhere()
	{
		SkCommandProcessor.ProcessCommand("/nores", SkCommandProcessor.LogTo.Chat);
		BeginMenu();
	}

	public void ToggleGodmode()
	{
		SkCommandProcessor.ProcessCommand("/god", SkCommandProcessor.LogTo.Chat);
		BeginMenu();
	}

	public void ToggleFlying()
	{
		SkCommandProcessor.ProcessCommand("/fly", SkCommandProcessor.LogTo.Chat);
		BeginMenu();
	}

	public void ToggleInfStam()
	{
		SkCommandProcessor.ProcessCommand("/infstam", SkCommandProcessor.LogTo.Chat);
		BeginMenu();
	}

	public void ToggleInfEitr()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		SkCommandProcessor.infEitr = !SkCommandProcessor.infEitr;
		SkCommandProcessor.ApplyInfEitr(localPlayer, applyOffValues: true);
		if (!SkCommandProcessor.infEitr)
		{
			SkCommandProcessor.Notify("Infinite eitr off.");
		}
		else
		{
			SkCommandProcessor.Notify((((Character)localPlayer).GetMaxEitr() > 0f) ? "Infinite eitr on." : "Infinite eitr on - but you have eaten no eitr food, so your maximum is still zero.");
		}
	}

	public void ToggleNoCarryLimit()
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		SkCommandProcessor.CaptureTuningBaseline(localPlayer);
		SkCommandProcessor.noCarryLimit = !SkCommandProcessor.noCarryLimit;
		SkCommandProcessor.ApplyTuning(localPlayer);
		if (!SkCommandProcessor.noCarryLimit && SkCommandProcessor.carryWeight <= 0)
		{
			SkCommandProcessor.RestoreCarryBaseline(localPlayer);
		}
		SkCommandProcessor.Notify(SkCommandProcessor.noCarryLimit ? "Carry limit lifted." : "Carry limit back to normal.");
	}

	public void RepairAll()
	{
		SkCommandProcessor.ProcessCommand("/repair", SkCommandProcessor.LogTo.Chat);
	}

	private void Update()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		if (bTeleport && Input.GetKeyDown((KeyCode)96))
		{
			Ray val = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit val2 = default(RaycastHit);
			if (Physics.Raycast(val, out val2))
			{
				Vector3 point = val2.point;
				Debug.DrawRay(((Component)Player.m_localPlayer).transform.position, point, Color.white);
				((Component)Player.m_localPlayer).transform.position = point;
				((Character)Player.m_localPlayer).Message((MessageType)1, "Warp!", 0, (Sprite)null, false);
			}
		}
	}

	private void OnGUI()
	{
	}
}
