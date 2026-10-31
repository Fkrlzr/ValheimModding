using System;
using System.Collections.Generic;
using System.Linq;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public static class CustomMenu
{
	public const int CATEGORY_OFFSET = 1000;

	private static readonly Dictionary<string, Piece> pieceCache = new Dictionary<string, Piece>();

	private static int MaxTabs => Configuration.MaxTabs;

	private static int MaxItemsPerTab => Configuration.ItemsPerTab - 1;

	private static int MaxItems => MaxTabs * MaxItemsPerTab;

	public static BuildItem BuildItem(string command, string shortName, string? fullName = null, bool instant = true, string? icon = null)
	{
		return new BuildItem
		{
			Command = command,
			ShortName = shortName,
			FullName = (fullName ?? shortName),
			Instant = instant,
			Icon = icon
		};
	}

	public static List<CategoryInfo> GenerateObjects()
	{
		List<BuildItem> list = (from prefab in ZNetScene.instance.m_namedPrefabs.Values
			where !((Object)prefab).name.StartsWith("sfx_") && !((Object)prefab).name.StartsWith("vfx_") && !((Object)prefab).name.StartsWith("fx_") && !((Object)prefab).name.StartsWith("vx_")
			where string.IsNullOrEmpty(HammerMenuCommand.CurrentFilter) || ((Object)prefab).name.StartsWith(HammerMenuCommand.CurrentFilter, StringComparison.InvariantCultureIgnoreCase)
			select BuildItem("hammer " + ((Object)prefab).name, ((Object)prefab).name)).OrderBy<BuildItem, string>((BuildItem item) => item.ShortName, StringComparer.InvariantCultureIgnoreCase).ToList();
		if (list.Count == 0)
		{
			return new List<CategoryInfo>();
		}
		IOrderedEnumerable<IGrouping<string, BuildItem>> orderedEnumerable = from item in list
			group item by item.ShortName.Substring(0, 1).ToUpper() into g
			orderby g.Key
			select g;
		List<CategoryInfo> list2 = new List<CategoryInfo>();
		foreach (IGrouping<string, BuildItem> item in orderedEnumerable)
		{
			string key = item.Key;
			List<BuildItem> list3 = item.OrderBy<BuildItem, string>((BuildItem item) => item.ShortName, StringComparer.InvariantCultureIgnoreCase).ToList();
			if (list3.Count <= MaxItemsPerTab)
			{
				list2.Add(new CategoryInfo
				{
					Name = key,
					Items = list3
				});
				continue;
			}
			int num = list3.Count;
			int num2 = 1;
			while (num > 0)
			{
				List<BuildItem> list4 = list3.Take(MaxItemsPerTab).ToList();
				num -= list4.Count;
				List<BuildItem> list5 = new List<BuildItem>();
				list5.AddRange(list3.Skip(list4.Count));
				list3 = list5;
				string text = list4.First().ShortName.Substring(1, 1).ToLower();
				list2.Add(new CategoryInfo
				{
					Name = key + text,
					Items = list4
				});
				num2++;
			}
		}
		return list2;
	}

	public static List<CategoryInfo> GenerateMenu()
	{
		List<BuildItem> list = new List<BuildItem>();
		list.AddRange(from item in GetBuildItems()
			select BuildItem("hammer_menu builds " + ((Object)item).name, ((Object)item).name, ((Object)item).name, instant: true, ((Object)item).name));
		list.AddRange(from mode in Enum.GetNames(typeof(MenuMode))
			where mode != MenuMode.Menu.ToString() && mode != MenuMode.Builds.ToString()
			orderby mode
			select BuildItem("hammer_menu " + mode.ToString().ToLower(), mode.ToString()));
		return new List<CategoryInfo>
		{
			new CategoryInfo
			{
				Name = "Menu",
				Items = list
			}
		};
	}

	public static List<CategoryInfo> GenerateBinds()
	{
		return new List<CategoryInfo>(2)
		{
			CreateBindCategory(BindManager.GetBinds(), "Binds"),
			CreateBindCategory(BindManager.GetWheelBinds(), "Wheel")
		};
	}

	private static CategoryInfo CreateBindCategory(Tuple<string, string>[] binds, string categoryName)
	{
		List<BuildItem> items = binds.Select((Tuple<string, string> bind) => BuildItem(bind.Item2, bind.Item1, bind.Item2)).ToList();
		return new CategoryInfo
		{
			Name = categoryName,
			Items = items
		};
	}

	public static List<CategoryInfo> GenerateSounds()
	{
		return GenerateCategories((from prefab in ZNetScene.instance.m_namedPrefabs.Values
			where ((Object)prefab).name.StartsWith("sfx_")
			where string.IsNullOrEmpty(HammerMenuCommand.CurrentFilter) || ((Object)prefab).name.Substring(4).Trim().StartsWith(HammerMenuCommand.CurrentFilter, StringComparison.InvariantCultureIgnoreCase)
			select BuildItem("hammer " + ((Object)prefab).name, ((Object)prefab).name.Substring(4).Trim(), ((Object)prefab).name)).ToList(), MaxItemsPerTab);
	}

	public static List<CategoryInfo> GenerateVisuals()
	{
		return GenerateCategories((from prefab in ZNetScene.instance.m_namedPrefabs.Values
			where ((Object)prefab).name.StartsWith("vfx_") || ((Object)prefab).name.StartsWith("fx_") || ((Object)prefab).name.StartsWith("vx_")
			where string.IsNullOrEmpty(HammerMenuCommand.CurrentFilter) || ((Object)prefab).name.Split(new char[1] { '_' }, 2).Last().Trim()
				.StartsWith(HammerMenuCommand.CurrentFilter, StringComparison.InvariantCultureIgnoreCase)
			select BuildItem("hammer " + ((Object)prefab).name, ((Object)prefab).name.Split(new char[1] { '_' }, 2).Last().Trim(), ((Object)prefab).name)).ToList(), MaxItemsPerTab);
	}

	public static List<CategoryInfo> GenerateComponents()
	{
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		foreach (GameObject value2 in ZNetScene.instance.m_namedPrefabs.Values)
		{
			if (!Object.op_Implicit((Object)(object)value2))
			{
				continue;
			}
			value2.GetComponentsInChildren<MonoBehaviour>(ZNetView.m_tempComponents);
			if (ZNetView.m_tempComponents.Count == 0)
			{
				continue;
			}
			foreach (MonoBehaviour tempComponent in ZNetView.m_tempComponents)
			{
				string name2 = ((object)tempComponent).GetType().Name;
				if (!(name2 == "ZNetView") && (string.IsNullOrEmpty(HammerMenuCommand.CurrentFilter) || name2.StartsWith(HammerMenuCommand.CurrentFilter, StringComparison.InvariantCultureIgnoreCase)))
				{
					if (!dictionary.ContainsKey(name2))
					{
						dictionary[name2] = new List<string>();
					}
					dictionary[name2].Add(((Object)value2).name);
				}
			}
		}
		int num = ((dictionary.Count > MaxTabs) ? MaxItems : MaxItemsPerTab);
		List<CategoryInfo> list = new List<CategoryInfo>();
		foreach (KeyValuePair<string, List<string>> item in dictionary)
		{
			string key = item.Key;
			List<string> value = item.Value;
			value.Sort(StringComparer.InvariantCultureIgnoreCase);
			List<BuildItem> list2 = value.Select((string name) => BuildItem("hammer " + name, name)).ToList();
			if (list2.Count <= num)
			{
				list.Add(new CategoryInfo
				{
					Name = key,
					Items = list2
				});
				continue;
			}
			List<BuildItem> list3 = list2.OrderBy<BuildItem, string>((BuildItem p) => p.ShortName, StringComparer.InvariantCultureIgnoreCase).ToList();
			int num2 = list3.Count;
			while (num2 > 0)
			{
				List<BuildItem> list4 = list3.Take(num).ToList();
				num2 -= list4.Count;
				list3 = list3.Skip(list4.Count).ToList();
				list.Add(new CategoryInfo
				{
					Name = key,
					Items = list4
				});
			}
		}
		return list;
	}

	public static List<CategoryInfo> GenerateVegetations()
	{
		return GenerateCategories((from veg in ParameterInfo.VegetationIds
			where string.IsNullOrEmpty(HammerMenuCommand.CurrentFilter) || veg.StartsWith(HammerMenuCommand.CurrentFilter, StringComparison.InvariantCultureIgnoreCase)
			select BuildItem("hammer " + veg, veg)).ToList(), MaxItemsPerTab);
	}

	public static List<CategoryInfo> GenerateLocations()
	{
		return GenerateCategories((from loc in ParameterInfo.LocationIds
			where string.IsNullOrEmpty(HammerMenuCommand.CurrentFilter) || loc.StartsWith(HammerMenuCommand.CurrentFilter, StringComparison.InvariantCultureIgnoreCase)
			select BuildItem("hammer_location " + loc, loc)).ToList(), MaxItemsPerTab);
	}

	public static List<CategoryInfo> GenerateRooms()
	{
		return GenerateCategories((from room in DungeonDB.m_instance.m_rooms
			where room.m_prefab.IsValid
			where string.IsNullOrEmpty(HammerMenuCommand.CurrentFilter) || room.m_prefab.Name.StartsWith(HammerMenuCommand.CurrentFilter, StringComparison.InvariantCultureIgnoreCase)
			select BuildItem("hammer_room " + room.m_prefab.Name, room.m_prefab.Name)).ToList(), MaxItemsPerTab);
	}

	public static List<CategoryInfo> GenerateBluePrints()
	{
		List<Blueprint> bps = new List<Blueprint>();
		switch (Configuration.BlueprintSorting)
		{
		case "Category":
			bps = HammerBlueprintCommand.GetBlueprintsByCategory();
			break;
		case "Folder":
			bps = HammerBlueprintCommand.GetBlueprintsByFolder();
			break;
		case "Category and folder":
			bps = HammerBlueprintCommand.GetBlueprintsByCategoryAndFolder();
			break;
		case "Only name":
			return GenerateBluePrintsByName();
		}
		return GenerateBluePrintCategories(bps);
	}

	private static List<CategoryInfo> GenerateBluePrintCategories(List<Blueprint> bps)
	{
		if (!string.IsNullOrEmpty(HammerMenuCommand.CurrentFilter))
		{
			List<Blueprint> list = new List<Blueprint>();
			list.AddRange(bps.Where((Blueprint bp) => bp.Name.StartsWith(HammerMenuCommand.CurrentFilter, StringComparison.InvariantCultureIgnoreCase)));
			bps = list;
		}
		IOrderedEnumerable<IGrouping<string, Blueprint>> orderedEnumerable = (from bp in bps
			group bp by bp.Category).OrderBy((IGrouping<string, Blueprint> g) => g.Key, StringComparer.InvariantCultureIgnoreCase);
		List<CategoryInfo> list2 = new List<CategoryInfo>();
		foreach (IGrouping<string, Blueprint> item in orderedEnumerable)
		{
			string key = item.Key;
			List<BuildItem> list3 = (from bp in item.OrderBy<Blueprint, string>((Blueprint bp) => bp.Name, StringComparer.InvariantCultureIgnoreCase)
				select BuildItem("hammer_blueprint " + bp.Name, bp.Name)).ToList();
			if (list3.Count <= MaxItemsPerTab)
			{
				list2.Add(new CategoryInfo
				{
					Name = key,
					Items = list3
				});
				continue;
			}
			List<BuildItem> list4 = list3.OrderBy<BuildItem, string>((BuildItem p) => p.ShortName, StringComparer.InvariantCultureIgnoreCase).ToList();
			int num = list4.Count;
			while (num > 0)
			{
				List<BuildItem> list5 = list4.Take(MaxItemsPerTab).ToList();
				num -= list5.Count;
				list4 = list4.Skip(list5.Count).ToList();
				string text = list5.First().ShortName.Substring(0, 1).ToUpper();
				string text2 = list5.Last().ShortName.Substring(0, 1).ToUpper();
				string name = ((text == text2) ? (key + "-" + text) : (key + "-" + text + "-" + text2));
				list2.Add(new CategoryInfo
				{
					Name = name,
					Items = list5
				});
			}
		}
		return list2;
	}

	private static List<CategoryInfo> GenerateBluePrintsByName()
	{
		return GenerateCategories((from bp in HammerBlueprintCommand.GetBlueprints()
			where string.IsNullOrEmpty(HammerMenuCommand.CurrentFilter) || bp.StartsWith(HammerMenuCommand.CurrentFilter, StringComparison.InvariantCultureIgnoreCase)
			select BuildItem("hammer_blueprint " + bp, bp)).ToList(), MaxItemsPerTab);
	}

	public static List<CategoryInfo> GenerateTools()
	{
		List<CategoryInfo> list = new List<CategoryInfo>();
		foreach (string key in ToolManager.Tools.Keys)
		{
			List<Tool> list2 = ToolManager.Get(key);
			if (list2.Count == 0)
			{
				continue;
			}
			List<Tool> list3 = list2;
			if (!string.IsNullOrEmpty(HammerMenuCommand.CurrentFilter))
			{
				List<Tool> list4 = new List<Tool>();
				list4.AddRange(list2.Where((Tool tool) => tool.Name.StartsWith(HammerMenuCommand.CurrentFilter, StringComparison.InvariantCultureIgnoreCase) || tool.Description.IndexOf(HammerMenuCommand.CurrentFilter, StringComparison.InvariantCultureIgnoreCase) >= 0));
				list3 = list4;
			}
			if (list3.Count == 0)
			{
				continue;
			}
			List<BuildItem> list5 = (from tool in list3.OrderBy<Tool, string>((Tool tool) => tool.Name, StringComparer.InvariantCultureIgnoreCase)
				select BuildItem("tool " + tool.Name, tool.Name, tool.Description, tool.Instant)).ToList();
			if (list5.Count <= MaxItemsPerTab)
			{
				list.Add(new CategoryInfo
				{
					Name = key,
					Items = list5
				});
				continue;
			}
			int num = list5.Count;
			while (num > 0)
			{
				List<BuildItem> list6 = list5.Take(MaxItemsPerTab).ToList();
				num -= list6.Count;
				list5 = list5.Skip(list6.Count).ToList();
				string text = list6.First().ShortName.Substring(0, 1).ToUpper();
				string text2 = list6.Last().ShortName.Substring(0, 1).ToUpper();
				string name = ((text == text2) ? (key + "-" + text) : (key + "-" + text + "-" + text2));
				list.Add(new CategoryInfo
				{
					Name = name,
					Items = list6
				});
			}
		}
		return list;
	}

	public static List<CategoryInfo> GenerateBuilds()
	{
		List<ItemDrop> buildItems = GetBuildItems();
		CategoryInfo categoryInfo = new CategoryInfo
		{
			Name = "Builds",
			Items = new List<BuildItem>()
		};
		foreach (ItemDrop item in buildItems)
		{
			categoryInfo.Items.Add(BuildItem("hammer_menu navigate " + ((Object)item).name, ((Object)item).name, ((Object)item).name, instant: true, ((Object)item).name));
		}
		return new List<CategoryInfo>(1) { categoryInfo };
	}

	public static void AddPipettes(PieceTable pt)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (pt.m_availablePiecesByCategory.Count == 0)
		{
			pt.m_availablePiecesByCategory.Add(new List<Piece>());
		}
		int index = ((pt.m_categories.Count > 0) ? ((int)pt.m_categories[0]) : 0);
		foreach (Tool item2 in ToolManager.Get("hammer"))
		{
			if ((!(item2.Name != "Pipette") || !(item2.Name != "Building pipette") || !(item2.Name != "Area pipette")) && IsToolCommandAllowed(item2))
			{
				Piece item = Build(pt, item2);
				if (pt.m_availablePieces.Add(item))
				{
					pt.m_availablePiecesByCategory[index].Add(item);
					pt.m_enabledPieces.Add(item);
				}
			}
		}
	}

	public static void AddTools(PieceTable pt)
	{
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		string text = PieceTableToEquipment(pt);
		List<Tool> list = ToolManager.Get(text);
		int num = 0;
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (Tool item in list)
		{
			if (!IsToolCommandAllowed(item))
			{
				continue;
			}
			num = item.TabIndex.GetValueOrDefault(num);
			if (num >= 0 && pt.m_availablePiecesByCategory.Count > num)
			{
				if (!dictionary.ContainsKey(num))
				{
					dictionary[num] = ((!(text == "hammer")) ? (pt.m_availablePiecesByCategory[num].Count - 1) : 0);
				}
				int val = item.Index ?? (dictionary[num] + 1);
				List<Piece> list2 = pt.m_availablePiecesByCategory[num];
				val = Math.Max(0, Math.Min(val, list2.Count));
				Piece val2 = Build(pt, item);
				val2.m_category = (PieceCategory)num;
				list2.Insert(val, val2);
				pt.m_availablePieces.Add(val2);
				pt.m_enabledPieces.Add(val2);
				dictionary[num] = val;
			}
		}
	}

	private static bool IsToolCommandAllowed(Tool tool)
	{
		string command = tool.GetCommand();
		if (string.IsNullOrWhiteSpace(command))
		{
			return true;
		}
		string[] array = command.Split(';');
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (!(text == "") && !PermissionApi.IsCommandAllowed(text.Split(' ')[0]))
			{
				return false;
			}
		}
		return true;
	}

	private static string PieceTableToEquipment(PieceTable pt)
	{
		string text = ((Object)pt).name;
		if (text.StartsWith("_"))
		{
			text = text.Substring(1);
		}
		if (text.EndsWith("PieceTable"))
		{
			text = text.Substring(0, text.Length - "PieceTable".Length);
		}
		return text.ToLowerInvariant();
	}

	private static Piece Build(PieceTable pt, Tool tool)
	{
		return (Piece)(object)ToolMenuPieces.Get(pt, tool);
	}

	public static List<ItemDrop> GetBuildItems()
	{
		ItemDrop val = default(ItemDrop);
		return (from item in ObjectDB.instance.m_items
			where item.TryGetComponent<ItemDrop>(ref val) && Object.op_Implicit((Object)(object)val.m_itemData.m_shared.m_buildPieces)
			select item.GetComponent<ItemDrop>()).OrderBy((ItemDrop item) => ((Object)item).name, StringComparer.InvariantCultureIgnoreCase).ToList();
	}

	public static List<CategoryInfo> GenerateCategories(List<BuildItem> items, int limit)
	{
		List<CategoryInfo> list = new List<CategoryInfo>();
		if (items.Count == 0)
		{
			return list;
		}
		List<BuildItem> list2 = items.OrderBy<BuildItem, string>((BuildItem i) => i.ShortName, StringComparer.InvariantCultureIgnoreCase).ToList();
		int num = list2.Count;
		while (num > 0)
		{
			List<BuildItem> list3 = list2.Take(limit).ToList();
			num -= list3.Count;
			List<BuildItem> list4 = new List<BuildItem>();
			list4.AddRange(list2.Skip(list3.Count));
			list2 = list4;
			string text = list3.First().ShortName.Substring(0, 1).ToUpper();
			string text2 = list3.Last().ShortName.Substring(0, 1).ToUpper();
			string name = ((text == text2) ? text : (text + "-" + text2));
			list.Add(new CategoryInfo
			{
				Name = name,
				Items = list3
			});
		}
		if (list.Count > MaxTabs && limit < MaxItems)
		{
			return GenerateCategories(items, MaxItems);
		}
		return list;
	}

	public static Piece BuildObject(BuildItem item, PieceCategory category)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		string text = item.Command + "|" + item.ShortName;
		if (pieceCache.TryGetValue(text, out Piece value))
		{
			value.m_category = category;
			return value;
		}
		GameObject val = new GameObject("_IH_Menu_" + text);
		Object.DontDestroyOnLoad((Object)val);
		BuildMenuTool buildMenuTool = val.AddComponent<BuildMenuTool>();
		ToolData data = new ToolData
		{
			name = item.ShortName,
			description = item.FullName,
			icon = (item.Icon ?? ("_" + item.ShortName)),
			command = item.Command,
			instant = item.Instant
		};
		buildMenuTool.tool = new Tool(data);
		((Piece)buildMenuTool).m_description = item.FullName;
		((Piece)buildMenuTool).m_name = item.ShortName;
		((Piece)buildMenuTool).m_icon = buildMenuTool.tool.Icon;
		pieceCache[text] = (Piece)(object)buildMenuTool;
		((Piece)buildMenuTool).m_category = category;
		return (Piece)(object)buildMenuTool;
	}

	public static Piece RepairButton()
	{
		if (pieceCache.TryGetValue("piece_repair", out Piece value))
		{
			if (Object.op_Implicit((Object)(object)value))
			{
				return value;
			}
			pieceCache.Remove("piece_repair");
		}
		ItemDrop val = GetBuildItems().FirstOrDefault((ItemDrop i) => ((Object)i).name.Equals("hammer", StringComparison.InvariantCultureIgnoreCase));
		if (!Object.op_Implicit((Object)(object)val))
		{
			return BackButton();
		}
		PieceTable buildPieces = val.m_itemData.m_shared.m_buildPieces;
		if (!Object.op_Implicit((Object)(object)buildPieces))
		{
			return BackButton();
		}
		GameObject val2 = buildPieces.m_pieces.FirstOrDefault((GameObject p) => ((Object)p).name.Equals("piece_repair", StringComparison.InvariantCultureIgnoreCase));
		if (!Object.op_Implicit((Object)(object)val2))
		{
			return BackButton();
		}
		Piece component = val2.GetComponent<Piece>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return BackButton();
		}
		pieceCache["piece_repair"] = component;
		return component;
	}

	public static Piece BackButton()
	{
		return BuildObject(BuildItem("hammer_menu back", "←", "Back"), (PieceCategory)100);
	}
}
