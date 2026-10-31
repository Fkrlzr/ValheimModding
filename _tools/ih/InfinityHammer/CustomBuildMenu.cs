using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinityHammer;

public static class CustomBuildMenu
{
	private static int MaxTabs => Configuration.MaxTabs;

	private static int MaxItemsPerTab => Configuration.ItemsPerTab - 1;

	private static List<CategoryInfo> CategoryInfos { get; set; } = new List<CategoryInfo>();


	public static void HandleCustomMenuMode(PieceTable pt)
	{
		CategoryInfos.Clear();
		List<CategoryInfo> categories = new List<CategoryInfo>();
		switch (HammerMenuCommand.CurrentMode)
		{
		case MenuMode.Menu:
			categories = CustomMenu.GenerateMenu();
			break;
		case MenuMode.Binds:
			categories = CustomMenu.GenerateBinds();
			break;
		case MenuMode.Objects:
			categories = CustomMenu.GenerateObjects();
			break;
		case MenuMode.Types:
			categories = CustomMenu.GenerateComponents();
			break;
		case MenuMode.Vegetation:
			categories = CustomMenu.GenerateVegetations();
			break;
		case MenuMode.Locations:
			categories = CustomMenu.GenerateLocations();
			break;
		case MenuMode.Blueprints:
			categories = CustomMenu.GenerateBluePrints();
			break;
		case MenuMode.Rooms:
			categories = CustomMenu.GenerateRooms();
			break;
		case MenuMode.Sounds:
			categories = CustomMenu.GenerateSounds();
			break;
		case MenuMode.Visuals:
			categories = CustomMenu.GenerateVisuals();
			break;
		case MenuMode.Tools:
			categories = CustomMenu.GenerateTools();
			break;
		case MenuMode.Builds:
			if (HammerMenuCommand.CurrentFilter != "")
			{
				return;
			}
			categories = CustomMenu.GenerateBuilds();
			break;
		}
		categories = HandleNavigation(categories);
		foreach (List<Piece> item in pt.m_availablePiecesByCategory)
		{
			item.Clear();
		}
		pt.m_availablePieces.Clear();
		pt.m_enabledPieces.Clear();
		AddCategories(pt.m_availablePiecesByCategory, categories);
		foreach (List<Piece> item2 in pt.m_availablePiecesByCategory)
		{
			foreach (Piece item3 in item2)
			{
				pt.m_availablePieces.Add(item3);
				pt.m_enabledPieces.Add(item3);
			}
		}
		List<PieceCategory> list = new List<PieceCategory>();
		list.AddRange(categories.Select((CategoryInfo _, int index) => (PieceCategory)(1000 + index)));
		pt.m_categories = list;
		List<string> list2 = new List<string>();
		list2.AddRange(categories.Select((CategoryInfo c) => c.Name));
		pt.m_categoryLabels = list2;
		SanityCheck(pt);
	}

	private static void SanityCheck(PieceTable pt)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		int num = 1 + ((pt.m_categories.Count > 0) ? pt.m_categories.Max((PieceCategory c) => (int)c) : 0);
		if (pt.m_selectedPiece.Length < num)
		{
			Array.Resize(ref pt.m_selectedPiece, num);
		}
		if (pt.m_lastSelectedPiece.Length < num)
		{
			Array.Resize(ref pt.m_lastSelectedPiece, num);
		}
		if (!pt.m_categories.Contains(pt.m_selectedCategory))
		{
			pt.m_selectedCategory = (PieceCategory)((pt.m_categories.Count > 0) ? ((int)pt.m_categories[0]) : 0);
		}
	}

	private static List<CategoryInfo> HandleNavigation(List<CategoryInfo> categories)
	{
		if (HammerMenuCommand.CurrentPage >= 0 && HammerMenuCommand.CurrentPage < categories.Count)
		{
			return CustomMenu.GenerateCategories(categories[HammerMenuCommand.CurrentPage].Items, MaxItemsPerTab);
		}
		if (categories.Count <= MaxTabs && categories.All((CategoryInfo c) => c.Items.Count <= MaxItemsPerTab))
		{
			return categories;
		}
		List<CategoryInfo> list = CustomMenu.GenerateCategories(categories.Select((CategoryInfo c, int i) => CustomMenu.BuildItem($"hammer_menu navigate {i}", c.Name, FormatItemsList(c.Items))).ToList(), MaxItemsPerTab);
		if (list.Count == 1)
		{
			list[0].Name = HammerMenuCommand.CurrentMode.ToString();
		}
		return list;
	}

	private static string FormatItemsList(List<BuildItem> items)
	{
		List<string> list = new List<string>();
		string text = "";
		foreach (BuildItem item in items)
		{
			string fullName = item.FullName;
			if (string.IsNullOrEmpty(text) || (text + ", " + fullName).Length <= 50)
			{
				text = ((!string.IsNullOrEmpty(text)) ? (text + ", " + fullName) : fullName);
				continue;
			}
			list.Add(text);
			text = fullName;
			if (list.Count >= 5)
			{
				break;
			}
		}
		if (!string.IsNullOrEmpty(text) && list.Count < 5)
		{
			list.Add(text);
		}
		return string.Join("\n", list);
	}

	private static void AddCategories(List<List<Piece>> tabs, List<CategoryInfo> categories)
	{
		Piece item2 = ((HammerMenuCommand.CurrentMode == MenuMode.Menu) ? CustomMenu.RepairButton() : CustomMenu.BackButton());
		while (tabs.Count < 1000 + categories.Count)
		{
			tabs.Add(new List<Piece>());
		}
		int index = 1000;
		foreach (CategoryInfo category in categories)
		{
			List<Piece> list = tabs[index];
			list.Clear();
			list.Add(item2);
			List<Piece> collection = category.Items.Select((BuildItem item) => CustomMenu.BuildObject(item, (PieceCategory)index)).ToList();
			list.AddRange(collection);
			index++;
		}
	}
}
