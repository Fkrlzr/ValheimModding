using System;
using System.Collections.Generic;
using ServerDevcommands;

namespace WorldEditCommands;

public class TweakBeehiveCommand : TweakCommand
{
	protected override string DoOperation(ZNetView view, string operation, string? value)
	{
		return operation switch
		{
			"spawn" => TweakActions.Spawn(view, Hash.Spawn, value), 
			"spawnoffset" => TweakActions.SpawnOffset(view, value), 
			"coveroffset" => TweakActions.CoverOffset(view, value), 
			"biome" => TweakActions.Biome(view, value), 
			"name" => TweakActions.Name(view, value), 
			"textbiome" => TweakActions.TextBiome(view, value), 
			"textcheck" => TweakActions.TextCheck(view, value), 
			"textextract" => TweakActions.TextExtract(view, value), 
			"texthappy" => TweakActions.TextHappy(view, value), 
			"textsleep" => TweakActions.TextSleep(view, value), 
			"textspace" => TweakActions.TextSpace(view, value), 
			_ => throw new NotImplementedException(), 
		};
	}

	protected override string DoOperation(ZNetView view, string operation, float? value)
	{
		if (operation == "speed")
		{
			return TweakActions.Speed(view, value);
		}
		if (operation == "maxcover")
		{
			return TweakActions.MaxCover(view, value);
		}
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, int? value)
	{
		if (operation == "maxamount")
		{
			return TweakActions.MaxAmount(view, value);
		}
		if (operation == "spawncondition")
		{
			return TweakActions.SpawnCondition(view, value);
		}
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, string[] value)
	{
		if (operation == "spawneffect")
		{
			return TweakActions.OutputEffect(view, value);
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

	public TweakBeehiveCommand()
	{
		Component = typeof(Beehive);
		ComponentName = "beehive";
		SupportedOperations.Add("maxamount", typeof(int));
		SupportedOperations.Add("maxcover", typeof(float));
		SupportedOperations.Add("spawncondition", typeof(int));
		SupportedOperations.Add("spawn", typeof(string));
		SupportedOperations.Add("speed", typeof(float));
		SupportedOperations.Add("biome", typeof(string));
		SupportedOperations.Add("name", typeof(string));
		SupportedOperations.Add("textbiome", typeof(string));
		SupportedOperations.Add("textcheck", typeof(string));
		SupportedOperations.Add("textextract", typeof(string));
		SupportedOperations.Add("texthappy", typeof(string));
		SupportedOperations.Add("textsleep", typeof(string));
		SupportedOperations.Add("textspace", typeof(string));
		SupportedOperations.Add("spawnoffset", typeof(string));
		SupportedOperations.Add("coveroffset", typeof(string));
		SupportedOperations.Add("spawneffect", typeof(string[]));
		AutoComplete.Add("maxamount", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("maxamount=<color=yellow>number</color>", "Maximum amount of stored items. No value to reset."));
		AutoComplete.Add("maxcover", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("maxcover=<color=yellow>number</color>", "Coverage limit (from 0.0 to 1.0). No value to reset."));
		AutoComplete.Add("name", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("name=<color=yellow>text</color>", "Display name. No value to reset."));
		AutoComplete.Add("textbiome", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("textbiome=<color=yellow>text</color>", "Text for wrong biome No value to reset."));
		AutoComplete.Add("textcheck", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("textcheck=<color=yellow>text</color>", "Text for checking the amount. No value to reset."));
		AutoComplete.Add("textextract", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("textextract=<color=yellow>text</color>", "Text for taking the items. No value to reset."));
		AutoComplete.Add("texthappy", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("texthappy=<color=yellow>text</color>", "Text when being happy. No value to reset."));
		AutoComplete.Add("textsleep", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("textsleep=<color=yellow>text</color>", "Text when sleeping. No value to reset."));
		AutoComplete.Add("textspace", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("textspace=<color=yellow>text</color>", "Text when covered. No value to reset."));
		AutoComplete.Add("spawn", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.ItemIds);
		AutoComplete.Add("biome", delegate
		{
			string[] names = Enum.GetNames(typeof(Biome));
			List<string> list = new List<string>(names.Length);
			list.AddRange(names);
			return list;
		});
		AutoComplete.Add("speed", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("speed=<color=yellow>number</color>", "Production speed in seconds. No value to reset."));
		AutoComplete.Add("spawncondition", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("spawncondition=<color=yellow>flag</color>", "Sum up: 1 = produces also during the night."));
		AutoComplete.Add("coveroffset", (int index) => ParameterInfo.XZY("coveroffset", "Offset for calculating cover.", index));
		AutoComplete.Add("spawnoffset", (int index) => ParameterInfo.XZY("spawnoffset", "Offset for spawning items. Also sets the <color=yellow>spawneffect</color> position.", index));
		AutoComplete.Add("spawneffect", (int index) => TweakAutoComplete.Effect("spawneffect", index));
		Init("tweak_beehive", "Modify beehives");
	}
}
