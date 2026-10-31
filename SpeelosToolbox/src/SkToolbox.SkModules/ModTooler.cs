using System;
using System.Collections.Generic;
using SkToolbox.Utility;
using UnityEngine;

namespace SkToolbox.SkModules;

// One menu for every feature brought over from ValheimTooler (Yohann MARTIN / Astropilot, MIT licence). The forms behind
// the buttons live in ModPlayer and ModWorld, where the helpers they share are; this module only lays them out.
internal class ModTooler : SkBaseModule, IModule
{
	internal ModPlayer Player;

	internal ModWorld World;

	public ModTooler()
	{
		ModuleName = "ValheimTooler";
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

	internal void ShowGrid()
	{
		List<SkMenuController.SkGridItem> list = new List<SkMenuController.SkGridItem>();
		list.Add(Toggle("Player", "Instant Craft", "Crafting and upgrading finish at once", SkIcons.First("Hammer", "Flint"), delegate
		{
			SkTooler.InstantCraft = !SkTooler.InstantCraft;
			SkCommandProcessor.Notify("Instant craft: " + (SkTooler.InstantCraft ? "on" : "off"));
		}, () => SkTooler.InstantCraft, SkMenuController.SkScope.Client));
		list.Add(Toggle("Player", "Teleport With Any Item", "Teleport even while carrying items the game normally blocks, such as metals", SkIcons.First("Copper", "Iron", "Bronze"), delegate
		{
			SkTooler.RestrictedTeleport = !SkTooler.RestrictedTeleport;
			SkCommandProcessor.Notify("Teleport with any item: " + (SkTooler.RestrictedTeleport ? "on" : "off"));
		}, () => SkTooler.RestrictedTeleport, SkMenuController.SkScope.Client));
		list.Add(Cell("Player", "Remove My Tombstones", "Delete the graves of your own character that are loaded around you", SkIcons.First("Flint", "Stone"), Player.ShowRemoveTombstonesForm, SkMenuController.SkScope.Client));
		list.Add(Cell("Player", "Teleport Player", "Send yourself or any player to another player, a map pin or a coordinate", SkIcons.First("SurtlingCore", "Thunderstone", "Ruby"), World.ShowSendPlayerForm, SkMenuController.SkScope.Server));
		list.Add(Cell("Player", "Heal Player", "Restore the health of one player, or of everyone loaded around you", SkIcons.First("MeadHealthMedium", "MeadHealthMinor", "Honey"), World.ShowHealPlayerForm, SkMenuController.SkScope.Server));
		list.Add(Cell("Map", "ESP", "Labels over players, creatures, pickables, ore deposits and dropped items, with a distance limit", SkIcons.First("Wishbone", "Coins"), Player.ShowEspGrid, SkMenuController.SkScope.Client));
		list.Add(Toggle("Map", "Auto-Pin Deposits", "Put a map pin on every ore deposit as it loads, and on the ones already loaded", SkIcons.First("CopperOre", "TinOre", "IronScrap"), delegate
		{
			SkTooler.AutoPin = !SkTooler.AutoPin;
			if (SkTooler.AutoPin)
			{
				SkTooler.PinExisting();
			}
			SkCommandProcessor.Notify("Auto-pin deposits: " + (SkTooler.AutoPin ? "on" : "off"));
		}, () => SkTooler.AutoPin, SkMenuController.SkScope.Client));
		list.Add(Toggle("Map", "Keep Deposit Pins", "Keep the deposit pins on the map after you leave, instead of removing them when the deposit unloads", SkIcons.First("Coins", "Ruby"), delegate
		{
			SkTooler.PermanentPins = !SkTooler.PermanentPins;
		}, () => SkTooler.PermanentPins, SkMenuController.SkScope.Client));
		list.Add(Cell("Map", "Clear Deposit Pins", "Remove every pin the auto-pin feature placed", SkIcons.First("Coal", "Resin"), delegate
		{
			SkCommandProcessor.Notify("Removed " + SkTooler.ClearPins() + " deposit pin(s).");
		}, SkMenuController.SkScope.Client));
		list.Add(Cell("Messages", "Event Message", "Show a message in the middle of the screen (the yellow event message) for every player loaded around you", SkIcons.First("Wishbone", "Feathers"), World.ShowBroadcastForm, SkMenuController.SkScope.Server));
		if (SkPlayerTools.Enabled)
		{
			list.Add(Cell("Player Tools", "Damage Player", "Hit one player for a damage amount you choose", SkIcons.First("SwordIron", "SwordBronze", "AxeStone"), World.ShowDamagePlayerForm, SkMenuController.SkScope.Server));
			list.Add(Cell("Player Tools", "Kill Player", "Kill one chosen player", SkIcons.First("SwordBlackmetal", "SwordIron", "AxeStone"), World.ShowKillPlayerForm, SkMenuController.SkScope.Server));
			list.Add(Cell("Player Tools", "Kill All Players", "Kill every other player loaded around you", SkIcons.First("SledgeStagbreaker", "Club"), World.ShowKillPlayersForm, SkMenuController.SkScope.Server));
			list.Add(Toggle("Player Tools", "Infinite Stamina (Others)", "Keep every other player's stamina full", SkIcons.First("MeadStaminaMedium", "MeadStaminaMinor"), delegate
			{
				if (SkPlayerTools.Allowed())
				{
					SkPlayerTools.InfiniteStaminaOthers = !SkPlayerTools.InfiniteStaminaOthers;
					SkPlayerTools.NoStaminaOthers = false;
				}
			}, () => SkPlayerTools.InfiniteStaminaOthers, SkMenuController.SkScope.Server));
			list.Add(Toggle("Player Tools", "No Stamina (Others)", "Keep every other player's stamina at zero", SkIcons.First("Entrails", "RawMeat"), delegate
			{
				if (SkPlayerTools.Allowed())
				{
					SkPlayerTools.NoStaminaOthers = !SkPlayerTools.NoStaminaOthers;
					SkPlayerTools.InfiniteStaminaOthers = false;
				}
			}, () => SkPlayerTools.NoStaminaOthers, SkMenuController.SkScope.Server));
			list.Add(Cell("Player Tools", "Chat As", "Send a chat line that shows a name you choose. It still carries your account id", SkIcons.First("Wishbone", "Feathers"), World.ShowChatAsForm, SkMenuController.SkScope.Server));
		}
		SkMC.RequestGridMenu(list, null, "ValheimTooler", showFilter: true, null);
	}
}
