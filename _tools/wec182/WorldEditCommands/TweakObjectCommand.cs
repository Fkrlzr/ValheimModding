using System;
using System.Collections.Generic;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class TweakObjectCommand : TweakCommand
{
	public static List<string> Wears = new List<string>(4) { "default", "broken", "damaged", "healthy" };

	public static List<string> Growths = new List<string>(5) { "big", "big_bad", "default", "small", "small_bad" };

	public static List<string> Smokes = new List<string>(3) { "off", "on", "ignore" };

	public static List<string> FallTypes = new List<string>(3) { "off", "solid", "terrain" };

	public static List<string> Waters = new List<string>(2) { "cave", "crypt" };

	public static List<string> Components = new List<string>(10) { "altar", "beehive", "chest", "fermenter", "itemstand", "pickable", "runestone", "smelter", "spawner", "spawnpoint" };

	protected override string DoOperation(ZNetView view, string operation, string? value)
	{
		return operation switch
		{
			"wear" => TweakActions.Wear(view, value), 
			"growth" => TweakActions.Growth(view, value), 
			"component" => TweakActions.Component(view, value), 
			"status" => TweakActions.Status(view, value), 
			"event" => TweakActions.Event(view, value), 
			"effect" => TweakActions.Effect(view, value), 
			"weather" => TweakActions.Weather(view, Hash.Weather, value), 
			"water" => TweakActions.Water(view, value), 
			"fall" => TweakActions.Fall(view, value), 
			_ => throw new NotImplementedException(), 
		};
	}

	protected override string DoOperation(ZNetView view, string operation, float? value)
	{
		if (operation == "destroy")
		{
			return TweakActions.Destroy(view, value);
		}
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, int? value)
	{
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, string[] value)
	{
		if (operation == "destroyeffect")
		{
			return TweakActions.DestroyEffect(view, value);
		}
		throw new NotImplementedException();
	}

	protected override string DoOperation(ZNetView view, string operation, bool? value)
	{
		return operation switch
		{
			"show" => TweakActions.Render(view, value), 
			"interact" => TweakActions.Interact(view, value), 
			"collision" => TweakActions.Collision(view, value), 
			"restrict" => TweakActions.Restrict(view, value), 
			"unlock" => TweakActions.Unlock(view, value), 
			_ => throw new NotImplementedException(), 
		};
	}

	protected override void Postprocess(GameObject? obj)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		StaticPhysics val = default(StaticPhysics);
		if (Object.op_Implicit((Object)(object)obj) && obj.TryGetComponent<StaticPhysics>(ref val))
		{
			val.m_updateTime = Time.time - 30f;
			((SlowUpdate)val).SUpdate(Time.time, ZoneSystem.GetZone(obj.transform.position));
		}
	}

	protected override string DoOperation(ZNetView view, string operation, long? value)
	{
		if (operation == "creator")
		{
			return TweakActions.Creator(view, value);
		}
		throw new NotImplementedException();
	}

	public TweakObjectCommand()
	{
		SupportedOperations.Add("component", typeof(string));
		SupportedOperations.Add("status", typeof(string));
		SupportedOperations.Add("effect", typeof(string));
		SupportedOperations.Add("weather", typeof(string));
		SupportedOperations.Add("event", typeof(string));
		SupportedOperations.Add("collision", typeof(bool));
		SupportedOperations.Add("show", typeof(bool));
		SupportedOperations.Add("interact", typeof(bool));
		SupportedOperations.Add("fall", typeof(string));
		SupportedOperations.Add("wear", typeof(string));
		SupportedOperations.Add("growth", typeof(string));
		SupportedOperations.Add("water", typeof(string));
		SupportedOperations.Add("creator", typeof(long));
		SupportedOperations.Add("destroy", typeof(float));
		SupportedOperations.Add("destroyeffect", typeof(string[]));
		AutoComplete.Add("component", (int index) => Components);
		AutoComplete.Add("status", (int index) => index switch
		{
			1 => ParameterInfo.StatusEffects, 
			0 => ParameterInfo.Create("status=<color=yellow>radius</color>,id,playeronly", "Adds status area."), 
			_ => ParameterInfo.None, 
		});
		AutoComplete.Add("effect", (int index) => (index != 0) ? ParameterInfo.EffectAreas : ParameterInfo.Create("effect=<color=yellow>radius</color>,id,playeronly", "Adds effect area."));
		AutoComplete.Add("weather", (int index) => index switch
		{
			2 => ParameterInfo.Create("weather=radius,id,<color=yellow>instant</color>,dungeon", "If given, the weather is changed instantly."), 
			1 => ParameterInfo.Environments, 
			0 => ParameterInfo.Create("weather=<color=yellow>radius</color>,id,instant,dungeon", "Adds weather area."), 
			_ => ParameterInfo.None, 
		});
		AutoComplete.Add("event", (int index) => index switch
		{
			1 => ParameterInfo.Events, 
			0 => ParameterInfo.Create("event=<color=yellow>radius</color>,id", "Adds event area."), 
			_ => ParameterInfo.None, 
		});
		AutoComplete.Add("collision", (int index) => ParameterInfo.Create("collision=<color=yellow>true/false</color> or no value to toggle.", "Sets object collision."));
		AutoComplete.Add("fall", (int index) => (index != 0) ? ParameterInfo.None : FallTypes);
		AutoComplete.Add("show", (int index) => ParameterInfo.Create("show=<color=yellow>true/false</color> or no value to toggle.", "Sets object visibility."));
		AutoComplete.Add("interact", (int index) => ParameterInfo.Create("interact=<color=yellow>true/false</color> or no value to toggle.", "Sets object interactability."));
		AutoComplete.Add("wear", (int index) => (index != 0) ? ParameterInfo.None : Wears);
		AutoComplete.Add("growth", (int index) => (index != 0) ? ParameterInfo.None : Growths);
		AutoComplete.Add("water", (int index) => (index != 0) ? ParameterInfo.XZY("water", "Scale", index - 1) : Waters);
		AutoComplete.Add("creator", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("creator=<color=yellow>player ID</color>", "Sets creator of objects (0 for no creator)."));
		AutoComplete.Add("destroy", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("destroy=<color=yellow>seconds</color>", "Timed destruction after seconds."));
		AutoComplete.Add("destroyeffect", (int index) => TweakAutoComplete.Effect("destroyeffect", index));
		Init("tweak_object", "Modify objects");
	}
}
