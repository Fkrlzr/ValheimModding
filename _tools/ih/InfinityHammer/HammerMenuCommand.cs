using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;

namespace InfinityHammer;

public class HammerMenuCommand
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static ConsoleEvent _003C0_003E__Execute;
	}

	private static readonly List<MenuNavigation> NavigationStack = new List<MenuNavigation>();

	public static MenuMode CurrentMode
	{
		get
		{
			if (NavigationStack.Count <= 0)
			{
				return MenuMode.Menu;
			}
			return NavigationStack[NavigationStack.Count - 1].Mode;
		}
	}

	public static string CurrentFilter
	{
		get
		{
			if (NavigationStack.Count <= 0)
			{
				return "";
			}
			return NavigationStack[NavigationStack.Count - 1].Filter;
		}
	}

	public static int CurrentPage
	{
		get
		{
			if (NavigationStack.Count <= 0)
			{
				return -1;
			}
			return NavigationStack[NavigationStack.Count - 1].Page;
		}
	}

	public HammerMenuCommand()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		AutoComplete.Register("hammer_menu", (Func<int, int, List<string>>)((int index, int subIndex) => index switch
		{
			0 => new List<string>(11)
			{
				"close", "binds", "blueprints", "builds", "objects", "locations", "rooms", "sounds", "visuals", "tools",
				"types"
			}, 
			1 => ParameterInfo.Create("Filter text (optional)"), 
			_ => ParameterInfo.None, 
		}));
		object obj = _003C_003EO._003C0_003E__Execute;
		if (obj == null)
		{
			ConsoleEvent val = Execute;
			_003C_003EO._003C0_003E__Execute = val;
			obj = (object)val;
		}
		Helper.Command("hammer_menu", "[mode] [filter] - Opens the hammer menu; repeat the same menu to close, or use close.", (ConsoleEvent)obj);
	}

	private static void Execute(ConsoleEventArgs args)
	{
		string text = ((args.Length > 1) ? args[1].ToLowerInvariant() : "menu");
		string text2 = ((args.Length > 2) ? args[2] : "");
		bool flag = text == CurrentMode.ToString().ToLowerInvariant() && text2 == CurrentFilter && CurrentPage == -1;
		if (text == "close" || (flag && Hud.IsPieceSelectionVisible() && Hammer.IsInfinityHammer(((Humanoid)Helper.GetPlayer()).GetRightItem())))
		{
			Hud.CloseBuildUi();
			return;
		}
		if (text == "back")
		{
			if (NavigationStack.Count > 0)
			{
				NavigationStack.RemoveAt(NavigationStack.Count - 1);
			}
			Hammer.OpenBuildMenu();
			return;
		}
		if (text == "navigate")
		{
			if (args.Length < 3)
			{
				HammerHelper.Message(args.Context, "Missing the navigation step.");
				return;
			}
			int? num = Parse.IntNull(args[2]);
			if (num.HasValue)
			{
				NavigationStack.Add(new MenuNavigation(CurrentMode, CurrentFilter, num.Value));
			}
			else
			{
				NavigationStack.Add(new MenuNavigation(CurrentMode, args[2], CurrentPage));
			}
			Hammer.OpenBuildMenu();
			return;
		}
		string filter = ((args.Length > 2) ? args[2] : "");
		NavigationStack.Clear();
		switch (text)
		{
		case "menu":
			NavigationStack.Add(new MenuNavigation(MenuMode.Menu, filter, -1));
			break;
		case "binds":
			NavigationStack.Add(new MenuNavigation(MenuMode.Binds, filter, -1));
			break;
		case "types":
			NavigationStack.Add(new MenuNavigation(MenuMode.Types, filter, -1));
			break;
		case "objects":
			NavigationStack.Add(new MenuNavigation(MenuMode.Objects, filter, -1));
			break;
		case "locations":
			NavigationStack.Add(new MenuNavigation(MenuMode.Locations, filter, -1));
			break;
		case "blueprints":
			NavigationStack.Add(new MenuNavigation(MenuMode.Blueprints, filter, -1));
			break;
		case "rooms":
			NavigationStack.Add(new MenuNavigation(MenuMode.Rooms, filter, -1));
			break;
		case "sounds":
			NavigationStack.Add(new MenuNavigation(MenuMode.Sounds, filter, -1));
			break;
		case "visuals":
			NavigationStack.Add(new MenuNavigation(MenuMode.Visuals, filter, -1));
			break;
		case "tools":
			NavigationStack.Add(new MenuNavigation(MenuMode.Tools, filter, -1));
			break;
		case "builds":
			NavigationStack.Add(new MenuNavigation(MenuMode.Builds, filter, -1));
			break;
		case "vegetation":
			NavigationStack.Add(new MenuNavigation(MenuMode.Vegetation, filter, -1));
			break;
		default:
			HammerHelper.Message(args.Context, "Invalid mode.");
			return;
		}
		Hammer.OpenBuildMenu();
	}
}
