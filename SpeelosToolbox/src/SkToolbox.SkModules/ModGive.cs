using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkToolbox.SkModules;

internal class ModGive : SkBaseModule, IModule
{
	private int quantity = 1;

	// The inventory saves item quality as a 32 bit int, but some other serialization paths in the game use an
	// unsigned 16 bit number (max 65535) and wrap around above that. Levels over 65535 are untested: keep them
	// to single player, items may lose their level when dropped or sent over the network.
	// The original limit was the item's own m_maxQuality (4 for most gear).
	internal const int MaxLevel = 1000000000;

	private int level = 1;

	private List<SkMenuController.SkGridItem> cache;

	private int cachedFor;

	public ModGive()
	{
		ModuleName = "Give";
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
		int num = (((Object)(object)ObjectDB.instance != (Object)null) ? ((Object)ObjectDB.instance).GetInstanceID() : 0);
		if (cache == null || cache.Count == 0 || cachedFor != num)
		{
			cache = BuildItems();
			cachedFor = num;
		}
		if (cache.Count == 0)
		{
			SkCommandProcessor.Notify("Item list is not available yet. Load into a world first.");
			return;
		}
		List<SkMenuController.SkMenuSlider> sliders = new List<SkMenuController.SkMenuSlider>
		{
			new SkMenuController.SkMenuSlider
			{
				Label = "Quantity",
				Min = 1,
				Max = 100,
				Get = () => quantity,
				Set = delegate(int value)
				{
					quantity = value;
				}
			},
			new SkMenuController.SkMenuSlider
			{
				Label = "Level",
				Min = 1,
				Max = MaxLevel,
				Get = () => level,
				Set = delegate(int value)
				{
					level = value;
				}
			}
		};
		SkMC.RequestGridMenu(cache, sliders, "Give");
	}

	private List<SkMenuController.SkGridItem> BuildItems()
	{
		List<SkMenuController.SkGridItem> list = new List<SkMenuController.SkGridItem>();
		if ((Object)(object)ObjectDB.instance == (Object)null || ObjectDB.instance.m_items == null)
		{
			return list;
		}
		foreach (GameObject item in ObjectDB.instance.m_items)
		{
			if ((Object)(object)item == (Object)null || string.IsNullOrEmpty(((Object)item).name))
			{
				continue;
			}
			ItemDrop component = item.GetComponent<ItemDrop>();
			if ((Object)(object)component == (Object)null || component.m_itemData == null || component.m_itemData.m_shared == null)
			{
				continue;
			}
			Sprite icon = null;
			Sprite[] icons = component.m_itemData.m_shared.m_icons;
			if (icons != null && icons.Length != 0)
			{
				icon = icons[0];
			}
			string text = ((Object)item).name;
			try
			{
				if (Localization.instance != null && !string.IsNullOrEmpty(component.m_itemData.m_shared.m_name))
				{
					string text2 = Localization.instance.Localize(component.m_itemData.m_shared.m_name);
					if (!string.IsNullOrEmpty(text2))
					{
						text = text2;
					}
				}
			}
			catch (Exception)
			{
			}
			list.Add(new SkMenuController.SkGridItem
			{
				Name = ((Object)item).name,
				Display = text,
				Icon = icon,
				Tip = text + "   (" + ((Object)item).name + ")",
				Scope = SkMenuController.SkScope.Client,
				OnClick = Give
			});
		}
		list.Sort((SkMenuController.SkGridItem a, SkMenuController.SkGridItem b) => string.Compare(a.Display, b.Display, StringComparison.OrdinalIgnoreCase));
		return list;
	}

	private void Give(string prefabName)
	{
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		GameObject val = (((Object)(object)ZNetScene.instance != (Object)null) ? ZNetScene.instance.GetPrefab(prefabName) : null);
		ItemDrop val2 = (((Object)(object)val != (Object)null) ? val.GetComponent<ItemDrop>() : null);
		if ((Object)(object)val2 == (Object)null || val2.m_itemData == null || val2.m_itemData.m_shared == null)
		{
			SkCommandProcessor.Notify("Item not found: " + prefabName);
			return;
		}
		int num = MaxLevel;
		int num2 = Mathf.Clamp(level, 1, num);
		int num3 = Mathf.Max(1, quantity);
		int num4 = Mathf.Max(1, val2.m_itemData.m_shared.m_maxStackSize);
		bool flag = !PlayerProfile.s_bypassCheatChecks;
		int num5 = 0;
		while (num3 > 0)
		{
			int num6 = Mathf.Min(num3, num4);
			ItemData val3 = ((Humanoid)localPlayer).GetInventory().AddItem(prefabName, num6, num2, val2.m_itemData.m_variant, 0L, "", flag, true);
			if (val3 == null)
			{
				break;
			}
			num5 += num6;
			num3 -= num6;
		}
		if (num5 > 0)
		{
			SkCommandProcessor.Notify("Gave " + num5 + "x " + prefabName + ((num2 > 1) ? (" (level " + num2 + ")") : "") + ((num3 > 0) ? " (inventory full)" : ""));
		}
		else
		{
			SkCommandProcessor.Notify("Inventory full, could not give " + prefabName);
		}
	}
}
