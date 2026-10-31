using System;
using ServerDevcommands;

namespace WorldEditCommands;

public class TweakDoorCommand : TweakCommand
{
	protected override string DoOperation(ZNetView view, string operation, string? value)
	{
		if (operation == "key")
		{
			return TweakActions.DoorKey(view, value);
		}
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, float? value)
	{
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, int? value)
	{
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, string[] value)
	{
		return operation switch
		{
			"openeffect" => TweakActions.OpenEffect(view, value), 
			"closeeffect" => TweakActions.CloseEffect(view, value), 
			"lockedeffect" => TweakActions.LockedEffect(view, value), 
			_ => throw new NotImplementedException(), 
		};
	}

	protected override string DoOperation(ZNetView view, string operation, bool? value)
	{
		return operation switch
		{
			"unlock" => TweakActions.Unlock(view, value), 
			"consume" => TweakActions.Consume(view, value), 
			"noclose" => TweakActions.NoClose(view, value), 
			_ => throw new NotImplementedException(), 
		};
	}

	protected override string DoOperation(ZNetView view, string operation, long? value)
	{
		throw new NotImplementedException();
	}

	public TweakDoorCommand()
	{
		Component = typeof(Door);
		ComponentName = "door";
		SupportedOperations.Add("unlock", typeof(bool));
		SupportedOperations.Add("consume", typeof(bool));
		SupportedOperations.Add("noclose", typeof(bool));
		SupportedOperations.Add("key", typeof(string));
		SupportedOperations.Add("openeffect", typeof(string[]));
		SupportedOperations.Add("closeeffect", typeof(string[]));
		SupportedOperations.Add("lockedeffect", typeof(string[]));
		AutoComplete.Add("unlock", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("unlock=<color=yellow>true/false</color>", "Ignores wards. No value to toggle."));
		AutoComplete.Add("consume", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("consume=<color=yellow>true/false</color>", "Usage consumes the key. No value to toggle."));
		AutoComplete.Add("noclose", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("noclose=<color=yellow>true/false</color>", "Door can't be closed. No value to toggle."));
		AutoComplete.Add("key", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.ItemIds);
		AutoComplete.Add("openeffect", (int index) => TweakAutoComplete.Effect("openeffect", index));
		AutoComplete.Add("closeeffect", (int index) => TweakAutoComplete.Effect("closeeffect", index));
		AutoComplete.Add("lockedeffect", (int index) => TweakAutoComplete.Effect("lockedeffect", index));
		Init("tweak_door", "Modify doors");
	}
}
