using System;
using System.Collections.Generic;
using Data;
using ServerDevcommands;

namespace WorldEditCommands;

public class TweakSpawnerCommand : TweakCommand
{
	protected override string DoOperation(ZNetView view, string operation, string? value)
	{
		if (operation == "globalkey")
		{
			return TweakActions.GlobalKey(view, value);
		}
		if (operation == "faction")
		{
			return TweakActions.Faction(view, value);
		}
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, float? value)
	{
		return operation switch
		{
			"spawnhealth" => TweakActions.SpawnHealth(view, value), 
			"levelchance" => TweakActions.LevelChance(view, value), 
			"spawnradius" => TweakActions.SpawnRadius(view, value), 
			"nearradius" => TweakActions.NearRadius(view, value), 
			"farradius" => TweakActions.FarRadius(view, value), 
			"triggerdistance" => TweakActions.TriggerDistance(view, value), 
			"respawn" => TweakActions.RespawnSeconds(view, Hash.RespawnSpawnArea, value), 
			_ => throw new NotImplementedException(), 
		};
	}

	protected override string DoOperation(ZNetView view, string operation, int? value)
	{
		return operation switch
		{
			"minlevel" => TweakActions.MinLevel(view, value), 
			"maxlevel" => TweakActions.MaxLevel(view, value), 
			"maxnear" => TweakActions.MaxNear(view, value), 
			"maxtotal" => TweakActions.MaxTotal(view, value), 
			"spawncondition" => TweakActions.SpawnCondition(view, value), 
			_ => throw new NotImplementedException(), 
		};
	}

	protected override string DoOperation(ZNetView view, string operation, string[] value)
	{
		if (operation == "spawn")
		{
			return TweakActions.Spawns(view, value);
		}
		if (operation == "spawneffect")
		{
			return TweakActions.SpawnEffect(view, value);
		}
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, bool? value)
	{
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, long? value)
	{
		throw new NotImplementedException();
	}

	public TweakSpawnerCommand()
	{
		Component = typeof(SpawnArea);
		ComponentName = "spawner";
		SupportedOperations.Add("globalkey", typeof(string));
		SupportedOperations.Add("maxnear", typeof(int));
		SupportedOperations.Add("maxtotal", typeof(int));
		SupportedOperations.Add("minlevel", typeof(int));
		SupportedOperations.Add("maxlevel", typeof(int));
		SupportedOperations.Add("spawncondition", typeof(int));
		SupportedOperations.Add("triggerdistance", typeof(float));
		SupportedOperations.Add("respawn", typeof(float));
		SupportedOperations.Add("spawnhealth", typeof(float));
		SupportedOperations.Add("levelchance", typeof(float));
		SupportedOperations.Add("spawnradius", typeof(float));
		SupportedOperations.Add("nearradius", typeof(float));
		SupportedOperations.Add("farradius", typeof(float));
		SupportedOperations.Add("spawn", typeof(string[]));
		SupportedOperations.Add("spawneffect", typeof(string[]));
		SupportedOperations.Add("faction", typeof(string));
		AutoComplete.Add("minlevel", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("minlevel=<color=yellow>number</color>", "Minimum level (level 1 = no star). No value to reset."));
		AutoComplete.Add("maxlevel", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("maxlevel=<color=yellow>number</color>", "Maximum level (level 1 = no star). No value to reset."));
		AutoComplete.Add("globalkey", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("text=<color=yellow>key</color>", "Required global keys to work. Start with - to remove the key. No value to reset."));
		AutoComplete.Add("maxnear", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("maxnear=<color=yellow>number</color>", "Maximum amount of spawns within the <color=yellow>nearradius</color>. No value to reset."));
		AutoComplete.Add("maxtotal", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("maxtotal=<color=yellow>number</color>", "Maximum amount of spawns within the <color=yellow>farradius</color>. No value to reset."));
		AutoComplete.Add("spawncondition", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("spawncondition=<color=yellow>flag</color>", "Sum up: 1 = day only, 2 = night only, 4 = ground only."));
		AutoComplete.Add("spawnhealth", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("spawnhealth=<color=yellow>number</color>", "Overrides the creature health. No value to reset."));
		AutoComplete.Add("respawn", (int index) => ParameterInfo.Create("respawn=<color=yellow>seconds</color>", "Sets the respawn time. No value to reset."));
		AutoComplete.Add("levelchance", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("levelchance=<color=yellow>percent</color>", "Level up chance (from 0 to 100). No value to reset."));
		AutoComplete.Add("faction", delegate(int index)
		{
			if (index == 0)
			{
				string[] names = Enum.GetNames(typeof(Faction));
				List<string> list = new List<string>(names.Length);
				list.AddRange(names);
				return list;
			}
			return ParameterInfo.None;
		});
		AutoComplete.Add("triggerdistance", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("triggerdistance=<color=yellow>meters</color>", "Player distance to activate the spawner. No value to reset."));
		AutoComplete.Add("spawnradius", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("spawnradius=<color=yellow>meters</color>", "Maximum spawn radius. No value to reset."));
		AutoComplete.Add("nearradius", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("nearradius=<color=yellow>meters</color>", "Radius for <color=yellow>maxnear</color>. No value to reset."));
		AutoComplete.Add("farradius", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("farradius=<color=yellow>meters</color>", "Radius for <color=yellow>maxtotal</color>. No value to reset."));
		AutoComplete.Add("spawn", (int index) => index switch
		{
			0 => ParameterInfo.Ids, 
			1 => ParameterInfo.Create("spawn=id,<color=yellow>weight</color>,minlevel,maxlevel,health/data", "Spawn chance relative to other spawns."), 
			2 => ParameterInfo.Create("spawn=id,weight,<color=yellow>minlevel</color>,maxlevel,health/data", "Minimum level (level 1 = 0 star)."), 
			3 => ParameterInfo.Create("spawn=id,weight,minlevel,<color=yellow>maxlevel</color>,health/data", "Maximum level (level 1 = 0 star)."), 
			4 => DataLoading.DataKeys, 
			_ => ParameterInfo.Create("For additional entries, add more <color>spawn=...</color> parameters."), 
		});
		AutoComplete.Add("spawneffect", (int index) => TweakAutoComplete.Effect("spawneffect", index));
		Init("tweak_spawner", "Modify spawners");
	}
}
