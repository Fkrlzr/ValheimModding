using System;
using System.Collections.Generic;
using Data;
using ServerDevcommands;

namespace WorldEditCommands;

public class TweakSpawnPointCommand : TweakCommand
{
	protected override string DoOperation(ZNetView view, string operation, string? value)
	{
		return operation switch
		{
			"spawndata" => TweakActions.SpawnData(view, value), 
			"spawn" => TweakActions.Spawn(view, Hash.Spawn, value), 
			"faction" => TweakActions.Faction(view, value), 
			_ => throw new NotImplementedException(), 
		};
	}

	protected override string DoOperation(ZNetView view, string operation, float? value)
	{
		return operation switch
		{
			"levelchance" => TweakActions.LevelChance(view, value), 
			"respawn" => TweakActions.Respawn(view, Hash.Respawn, value), 
			"spawnhealth" => TweakActions.SpawnHealth(view, value), 
			"triggerdistance" => TweakActions.TriggerDistance(view, value), 
			"triggernoise" => TweakActions.TriggerNoise(view, value), 
			_ => throw new NotImplementedException(), 
		};
	}

	protected override string DoOperation(ZNetView view, string operation, int? value)
	{
		return operation switch
		{
			"minlevel" => TweakActions.MinLevel(view, value), 
			"maxlevel" => TweakActions.MaxLevel(view, value), 
			"spawncondition" => TweakActions.SpawnCondition(view, value), 
			_ => throw new NotImplementedException(), 
		};
	}

	protected override string DoOperation(ZNetView view, string operation, string[] value)
	{
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

	public TweakSpawnPointCommand()
	{
		Component = typeof(CreatureSpawner);
		ComponentName = "spawnpoint";
		SupportedOperations.Add("minlevel", typeof(int));
		SupportedOperations.Add("maxlevel", typeof(int));
		SupportedOperations.Add("spawncondition", typeof(int));
		SupportedOperations.Add("levelchance", typeof(float));
		SupportedOperations.Add("triggerdistance", typeof(float));
		SupportedOperations.Add("respawn", typeof(float));
		SupportedOperations.Add("spawnhealth", typeof(float));
		SupportedOperations.Add("spawn", typeof(string));
		SupportedOperations.Add("spawneffect", typeof(string[]));
		SupportedOperations.Add("faction", typeof(string));
		SupportedOperations.Add("spawndata", typeof(string));
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
		AutoComplete.Add("minlevel", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("minlevel=<color=yellow>number</color>", "Minimum level (level 1 = no star). No value to reset."));
		AutoComplete.Add("maxlevel", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("maxlevel=<color=yellow>number</color>", "Maximum level (level 1 = no star). No value to reset."));
		AutoComplete.Add("triggernoise", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("triggernoise=<color=yellow>meters</color>", "Required noise to activate the spawn point. No value to reset."));
		AutoComplete.Add("triggerdistance", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("triggerdistance=<color=yellow>meters</color>", "Required distance to activate the spawn point. No value to reset."));
		AutoComplete.Add("spawncondition", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("spawncondition=<color=yellow>flag</color>", "1 = day only, 2 = night only."));
		AutoComplete.Add("respawn", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("respawn=<color=yellow>minutes/false</color>", "Respawn time. No value to reset."));
		AutoComplete.Add("levelchance", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("levelchance=<color=yellow>percent</color>", "Level up chance (from 0 to 100). No value to reset."));
		AutoComplete.Add("spawnhealth", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("spawnhealth=<color=yellow>number</color>", "Overrides the creature health. No value to reset."));
		AutoComplete.Add("spawn", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.ObjectIds);
		AutoComplete.Add("spawneffect", (int index) => TweakAutoComplete.Effect("spawneffect", index));
		AutoComplete.Add("spawndata", (int index) => (index != 0) ? ParameterInfo.None : DataLoading.DataKeys);
		Init("tweak_spawnpoint", "Modify spawn points");
	}
}
