using System.Collections.Generic;

namespace ServerDevcommands;

public static class DefaultAutoComplete
{
	public static void Register()
	{
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		AutoComplete.RegisterEmpty("aggravate");
		AutoComplete.RegisterEmpty("challenge");
		AutoComplete.RegisterEmpty("cheers");
		AutoComplete.RegisterEmpty("clear");
		AutoComplete.Register("fov ", (int index) => (index == 0) ? ParameterInfo.Create("Amount", "a positive number") : ParameterInfo.None);
		AutoComplete.RegisterEmpty("hidebetatext");
		AutoComplete.Register("help ", (int index) => index switch
		{
			0 => ParameterInfo.Create("Page", "number"), 
			1 => ParameterInfo.Create("Page size", "a positive integer (default is 5)"), 
			_ => ParameterInfo.None, 
		});
		AutoComplete.RegisterEmpty("info");
		AutoComplete.Register("lodbias", (int index) => (index == 0) ? ParameterInfo.Create("Amount", "a positive number") : ParameterInfo.None);
		AutoComplete.RegisterEmpty("nonono");
		AutoComplete.RegisterEmpty("opterrain");
		AutoComplete.RegisterEmpty("point");
		AutoComplete.RegisterEmpty("ping");
		ConsoleOptionsFetcher originalFetcher = Terminal.commands["raiseskill"].m_tabOptionsFetcher;
		Terminal.commands["raiseskill"].m_tabOptionsFetcher = new ConsoleOptionsFetcher(newFetcher);
		AutoComplete.Register("raiseskill", delegate(int index)
		{
			ConsoleOptionsFetcher tabOptionsFetcher = Terminal.commands["raiseskill"].m_tabOptionsFetcher;
			return index switch
			{
				0 => tabOptionsFetcher.Invoke(), 
				1 => ParameterInfo.Create("Amount of skill levels gained or lost (if negative)."), 
				_ => ParameterInfo.None, 
			};
		});
		AutoComplete.RegisterEmpty("resetsharedmap");
		AutoComplete.RegisterEmpty("resetspawn");
		AutoComplete.RegisterEmpty("respawn");
		AutoComplete.Register("s", (int index) => ParameterInfo.Create("Message"));
		AutoComplete.Register("say", (int index) => ParameterInfo.Create("Message"));
		AutoComplete.RegisterEmpty("restartparty");
		AutoComplete.RegisterEmpty("sit");
		AutoComplete.RegisterEmpty("thumbsup");
		AutoComplete.RegisterEmpty("tutorialreset");
		AutoComplete.RegisterEmpty("tutorialtoggle");
		AutoComplete.RegisterEmpty("wave");
		AutoComplete.Register("W", (int index) => (index == 0) ? ParameterInfo.PlayerNames : ParameterInfo.Create("Message"));
		AutoComplete.RegisterAdmin("ban");
		AutoComplete.RegisterEmpty("banned");
		AutoComplete.RegisterAdmin("kick");
		AutoComplete.RegisterEmpty("save");
		AutoComplete.RegisterAdmin("unban");
		AutoComplete.Register("beard", (int index) => (index == 0) ? ParameterInfo.Beards : ParameterInfo.None);
		AutoComplete.RegisterEmpty("clearstatus");
		AutoComplete.RegisterEmpty("dpsdebug");
		AutoComplete.RegisterEmpty("exploremap");
		AutoComplete.Register("ffsmooth", (int index) => (index == 0) ? ParameterInfo.Create("0 = normal, 1 = add smooth movement") : ParameterInfo.None);
		AutoComplete.RegisterEmpty("fly");
		AutoComplete.RegisterEmpty("freefly");
		AutoComplete.Register("forcedelete", (int index) => index switch
		{
			0 => ParameterInfo.Create("Radius", "in meters (from 0.0 to 50.0, default is 5.0)."), 
			1 => ParameterInfo.ObjectIds, 
			_ => ParameterInfo.None, 
		});
		AutoComplete.RegisterEmpty("gc");
		AutoComplete.RegisterEmpty("ghost");
		AutoComplete.RegisterEmpty("god");
		AutoComplete.Register("hair", (int index) => (index == 0) ? ParameterInfo.Hairs : ParameterInfo.None);
		AutoComplete.RegisterEmpty("heal");
		AutoComplete.Register("itemset", (int index) => index switch
		{
			0 => Terminal.commands["itemset"].m_tabOptionsFetcher.Invoke(), 
			1 => new List<string>(2) { "keep", "clear" }, 
			_ => ParameterInfo.None, 
		});
		AutoComplete.RegisterEmpty("killenemycreatures");
		AutoComplete.RegisterEmpty("killtame");
		AutoComplete.RegisterEmpty("listkeys");
		if (Terminal.commands.TryGetValue("listkeys", out var value))
		{
			value.RemoteCommand = false;
		}
		AutoComplete.Register("location", (int index) => index switch
		{
			0 => ParameterInfo.LocationIds, 
			1 => new List<string>(1) { "SAVE" }, 
			_ => ParameterInfo.None, 
		});
		AutoComplete.Register("maxfps", (int index) => (index == 0) ? ParameterInfo.Create("Amount", "a positive integer") : ParameterInfo.None);
		AutoComplete.Register("model", (int index) => (index == 0) ? ParameterInfo.Create("<color=yellow>0</color> = male, <color=yellow>1</color> = female") : ParameterInfo.None);
		AutoComplete.RegisterEmpty("nocost");
		AutoComplete.RegisterEmpty("noportals");
		AutoComplete.Register("players", (int index) => (index == 0) ? ParameterInfo.Create("Amount", "a positive integer (0 disables the override)") : ParameterInfo.None);
		AutoComplete.RegisterEmpty("printcreatures");
		AutoComplete.RegisterEmpty("printseeds");
		AutoComplete.RegisterEmpty("printlocations");
		AutoComplete.RegisterEmpty("puke");
		AutoComplete.RegisterEmpty("removebirds");
		AutoComplete.RegisterEmpty("removedrops");
		AutoComplete.RegisterEmpty("removefish");
		AutoComplete.Register("recall ", (int index) => (index == 0) ? ParameterInfo.PlayerNames : ParameterInfo.None);
		AutoComplete.RegisterEmpty("resetcharacter");
		AutoComplete.RegisterEmpty("resetenv");
		AutoComplete.RegisterEmpty("resetwind");
		AutoComplete.Register("removekey", (int index) => (index == 0) ? ParameterInfo.ActiveGlobalKeys : ParameterInfo.None);
		AutoComplete.Register("setkey", (int index) => (index == 0) ? ParameterInfo.GlobalKeys : ParameterInfo.None);
		AutoComplete.Register("setworldmodifier", delegate(int index, int subIndex, string[] args)
		{
			switch (index)
			{
			case 0:
				return ParameterInfo.WorldModifiers;
			case 1:
				if (args.Length == 0)
				{
					return new List<string>();
				}
				return ParameterInfo.WorldModifierOptions(args[0]);
			default:
				return ParameterInfo.None;
			}
		});
		AutoComplete.RegisterDefault("setpower");
		AutoComplete.Register("spawn", delegate(int index)
		{
			switch (index)
			{
			case 0:
				return ParameterInfo.ObjectIds;
			case 1:
				return ParameterInfo.Create("Amount", "a positive integer (default 1)");
			case 2:
				return ParameterInfo.Create("Level", "a positive integer (default 1)");
			default:
				if (index == 2)
				{
					return ParameterInfo.Create("<color=yellow>p</color> to automatically pick up items. <color=yellow>e</color> to automatically equip items.");
				}
				return ParameterInfo.None;
			}
		});
		AutoComplete.RegisterEmpty("tame");
		AutoComplete.Register("test", (int index) => (index == 0) ? new List<string>(1) { "oldcomfort" } : ParameterInfo.None);
		AutoComplete.RegisterDefault("resetskill");
		AutoComplete.RegisterEmpty("time");
		AutoComplete.Register("timescale", (int index) => index switch
		{
			0 => ParameterInfo.Create("Multiplier", "sets how fast the time goes (from 0.0 to 3.0). Value 0 can be used to pause the game."), 
			1 => ParameterInfo.Create("Transition duration", "causes the change to be applied gradually over time (seconds). Default value 0 applies the change instantly."), 
			_ => ParameterInfo.None, 
		});
		AutoComplete.Register("tod", (int index) => (index == 0) ? ParameterInfo.Create("Time", "overrides the time of the day (from 0.0 to 1.0, with 0.5 being the mid day). Value -1 removes the override.") : ParameterInfo.None);
		List<string> newFetcher()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			List<string> list = originalFetcher.Invoke();
			SkillType val = (SkillType)999;
			if (!list.Contains(((object)(SkillType)(ref val)).ToString()))
			{
				val = (SkillType)999;
				list.Add(((object)(SkillType)(ref val)).ToString());
			}
			return list;
		}
	}
}
