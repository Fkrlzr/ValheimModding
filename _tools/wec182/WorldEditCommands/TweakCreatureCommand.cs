using System;
using System.Collections.Generic;
using ServerDevcommands;

namespace WorldEditCommands;

public class TweakCreatureCommand : TweakCommand
{
	protected override string DoOperation(ZNetView view, string operation, string? value)
	{
		if (operation == "name")
		{
			return TweakActions.Name(view, value);
		}
		if (operation == "faction")
		{
			return TweakActions.Faction(view, value);
		}
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, float? value)
	{
		if (operation == "health")
		{
			return TweakActions.Health(view, value);
		}
		if (operation == "damage")
		{
			return Actions.Damage(view, value);
		}
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, int? value)
	{
		if (operation == "level")
		{
			return TweakActions.Level(view, value);
		}
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, string[] value)
	{
		return operation switch
		{
			"item" => TweakActions.Items(view, value), 
			"affix" => TweakActions.CLLC_BossEffect(view, value), 
			"effect" => TweakActions.CLLC_Effect(view, value), 
			"infusion" => TweakActions.CLLC_Infusion(view, value), 
			"resistance" => TweakActions.Resistances(view, value), 
			"attacks" => TweakActions.Attacks(view, value), 
			_ => throw new NotImplementedException(), 
		};
	}

	protected override string DoOperation(ZNetView view, string operation, bool? value)
	{
		return operation switch
		{
			"boss" => TweakActions.Boss(view, value), 
			"hunt" => TweakActions.Hunt(view, value), 
			"tame" => TweakActions.Tame(view, value), 
			_ => throw new NotImplementedException(), 
		};
	}

	protected override string DoOperation(ZNetView view, string operation, long? value)
	{
		throw new NotImplementedException();
	}

	public TweakCreatureCommand()
	{
		Component = typeof(Character);
		ComponentName = "character";
		SupportedOperations.Add("damage", typeof(float));
		SupportedOperations.Add("faction", typeof(string));
		SupportedOperations.Add("tame", typeof(bool));
		SupportedOperations.Add("health", typeof(float));
		SupportedOperations.Add("level", typeof(int));
		SupportedOperations.Add("hunt", typeof(bool));
		if (WorldEditCommands.IsSpawnerTweaks)
		{
			SupportedOperations.Add("boss", typeof(bool));
			SupportedOperations.Add("resistance", typeof(string[]));
			SupportedOperations.Add("name", typeof(string));
			SupportedOperations.Add("item", typeof(string[]));
			SupportedOperations.Add("attacks", typeof(string[]));
		}
		if (WorldEditCommands.IsCLLC)
		{
			SupportedOperations.Add("affix", typeof(string[]));
			SupportedOperations.Add("effect", typeof(string[]));
			SupportedOperations.Add("infusion", typeof(string[]));
		}
		if (WorldEditCommands.IsCLLC)
		{
			AutoComplete.Add("affix", delegate
			{
				string[] names5 = Enum.GetNames(typeof(Enum_CLLC_Boss));
				List<string> list6 = new List<string>(names5.Length);
				list6.AddRange(names5);
				return list6;
			});
			AutoComplete.Add("effect", delegate
			{
				string[] names4 = Enum.GetNames(typeof(Enum_CLLC_Effect));
				List<string> list5 = new List<string>(names4.Length);
				list5.AddRange(names4);
				return list5;
			});
			AutoComplete.Add("infusion", delegate
			{
				string[] names3 = Enum.GetNames(typeof(Enum_CLLC_Infusion));
				List<string> list4 = new List<string>(names3.Length);
				list4.AddRange(names3);
				return list4;
			});
		}
		AutoComplete.Add("name", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("name=<color=yellow>text</color>", "Display name. Use _ as the space. No value to reset."));
		AutoComplete.Add("faction", delegate(int index)
		{
			if (index == 0)
			{
				string[] names2 = Enum.GetNames(typeof(Faction));
				List<string> list3 = new List<string>(names2.Length);
				list3.AddRange(names2);
				return list3;
			}
			return ParameterInfo.None;
		});
		AutoComplete.Add("boss", (int index) => ParameterInfo.Create("boss=<color=yellow>true/false</color> ", "Sets the boss health bar. No value to reset."));
		AutoComplete.Add("tame", (int index) => ParameterInfo.Create("tame=<color=yellow>true/false</color> ", "Sets the tamed status. No value to toggle."));
		AutoComplete.Add("hunt", (int index) => ParameterInfo.Create("hunt=<color=yellow>true/false</color>", "Sets the extra aggressiveness. No value to toggle."));
		AutoComplete.Add("level", (int index) => ParameterInfo.Create("level=<color=yellow>number</color>", "Sets the level (level 1 = 0 star)"));
		AutoComplete.Add("health", (int index) => ParameterInfo.Create("health=<color=yellow>number</color>", "Sets the health."));
		AutoComplete.Add("damage", (int index) => ParameterInfo.Create("damage=<color=yellow>number</color>", "Sets the damage multiplier."));
		AutoComplete.Add("resistance", delegate(int index)
		{
			switch (index)
			{
			case 0:
			{
				string[] names = Enum.GetNames(typeof(DamageType));
				List<string> list2 = new List<string>(names.Length);
				list2.AddRange(names);
				return list2;
			}
			case 1:
			{
				string[] names = Enum.GetNames(typeof(DamageModifier));
				List<string> list = new List<string>(names.Length);
				list.AddRange(names);
				return list;
			}
			default:
				return ParameterInfo.Create("For additional entries, add more <color>resistance=...</color> parameters.");
			}
		});
		AutoComplete.Add("item", (int index) => index switch
		{
			0 => ParameterInfo.ItemIds, 
			1 => ParameterInfo.Create("item=id,<color=yellow>chance</color>,minamount,maxamount,flag", "Drop chance."), 
			2 => ParameterInfo.Create("item=id,chance,<color=yellow>minamount</color>,maxamount,flag", "Minimum amount."), 
			3 => ParameterInfo.Create("item=id,chance,minamount,<color=yellow>maxamount</color>,flag", "Maximum amount."), 
			4 => ParameterInfo.Create("item=id,chance,minamount,maxamount,<color=yellow>flag</color>", "Sum up: 1 = star multiplier, 2 = one per player."), 
			_ => ParameterInfo.Create("For additional entries, add more <color>item=...</color> parameters."), 
		});
		AutoComplete.Add("attacks", (int index) => ParameterInfo.ItemIds);
		Init("tweak_creature", "Modify creatures");
	}
}
