using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using ServerDevcommands;
using Service;
using UnityEngine;

namespace WorldEditCommands;

public abstract class BaseParameters
{
	public Vector3 From;

	public Vector3? Center;

	public string[] ConnectionIds = Array.Empty<string>();

	public string[] IncludedIds = Array.Empty<string>();

	public string[] ExcludedIds = Array.Empty<string>();

	public float Angle;

	public Range<float>? Radius;

	public Range<float>? Width;

	public Range<float>? Depth;

	public float Height;

	public float Chance = 1f;

	public string Match = "";

	public string Unmatch = "";

	public bool Connect;

	public HashSet<string> Components = new HashSet<string>();

	public Dictionary<string, string> DataParameters = new Dictionary<string, string>();

	private readonly Dictionary<string, Type> SupportedOperations;

	private Terminal terminal;

	public Dictionary<string, object?> Operations;

	public static Random Random = new Random();

	protected BaseParameters(Dictionary<string, Type> supportedOperations)
	{
		SupportedOperations = supportedOperations;
		Operations = new Dictionary<string, object>();
		base._002Ector();
	}

	public void ParseCommand(ConsoleEventArgs args)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)Player.m_localPlayer))
		{
			From = ((Component)Player.m_localPlayer).transform.position;
		}
		terminal = args.Context;
		ParseArgs(args.Args);
	}

	protected abstract void ParseArg(string arg);

	protected abstract void ParseArg(string arg, string value);

	protected virtual void ParseArgs(string[] args)
	{
		for (int i = 0; i < args.Length; i++)
		{
			string[] array = args[i].Split(new char[1] { '=' }, 2);
			string text = array[0].ToLower();
			ParseArgInternal(text);
			ParseArg(text);
			if (array.Length >= 2)
			{
				string value = array[1].Trim();
				ParseArgInternal(text, value);
				ParseArg(text, value);
			}
		}
		if (Operations.Count == 0)
		{
			throw new InvalidOperationException("Missing the operation.");
		}
		if (Radius != null && Depth != null)
		{
			throw new InvalidOperationException("<color=yellow>circle</color> and <color=yellow>rect</color> parameters can't be used together.");
		}
		if (Radius != null && Connect)
		{
			throw new InvalidOperationException("<color=yellow>circle</color> and <color=yellow>connect</color> parameters can't be used together.");
		}
		if (Depth != null && Connect)
		{
			throw new InvalidOperationException("<color=yellow>connect</color> and <color=yellow>rect</color> parameters can't be used together.");
		}
	}

	private void ParseArgInternal(string name)
	{
		if (SupportedOperations.TryGetValue(name, out Type value))
		{
			if (Operations.ContainsKey(name) && value != typeof(string[]))
			{
				throw new InvalidOperationException("Operation " + name + " used multiple times.");
			}
			if (value == typeof(string[]))
			{
				if (!Operations.ContainsKey(name))
				{
					Operations.Add(name, new string[0]);
				}
			}
			else
			{
				Operations.Add(name, null);
			}
		}
		if (name == "connect")
		{
			Connect = true;
		}
	}

	private void ParseArgInternal(string name, string value)
	{
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		if (SupportedOperations.TryGetValue(name, out Type value2))
		{
			if (value2 == typeof(int))
			{
				Operations[name] = Parse.Int(value, 0);
			}
			else if (value2 == typeof(float))
			{
				Operations[name] = Parse.Float(value, 0f);
			}
			else if (value2 == typeof(bool))
			{
				Operations[name] = Parse.Boolean(value);
			}
			else if (value2 == typeof(string[]))
			{
				Operations[name] = (Operations[name] as string[]).Append<string>(value).ToArray();
			}
			else
			{
				Operations[name] = value;
			}
		}
		string[] array = Parse.Split(value, ',');
		if (name == "par")
		{
			KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, ',');
			if (keyValuePair.Key == "")
			{
				throw new InvalidOperationException("Invalid data parameter " + value + ".");
			}
			DataParameters["<" + keyValuePair.Key + ">"] = keyValuePair.Value;
		}
		if (name == "connect")
		{
			ConnectionIds = array;
		}
		if (name == "center" || name == "from")
		{
			Center = Parse.VectorXZY(array);
		}
		if (name == "id")
		{
			IncludedIds = array;
		}
		if (name == "ignore")
		{
			ExcludedIds = array;
		}
		if (name == "chance")
		{
			Chance = Parse.Float(value, 1f);
		}
		if (name == "type")
		{
			AddComponents(array);
		}
		if (name == "match")
		{
			Match = value;
		}
		if (name == "unmatch")
		{
			Unmatch = value;
		}
		if (name == "rect")
		{
			Range<Vector3> val = Parse.ScaleRange(value);
			Width = new Range<float>(val.Min.x, val.Max.x);
			Depth = new Range<float>(val.Min.z, val.Max.z);
		}
		switch (name)
		{
		case "radius":
		case "range":
		case "circle":
			Radius = Parse.FloatRange(value, 0f);
			break;
		}
		if (name == "height")
		{
			Height = Parse.Float(value, 0f);
		}
		if (name == "angle")
		{
			Angle = Parse.Float(value, 0f) * (float)Math.PI / 180f;
		}
	}

	private void AddComponents(string[] values)
	{
		foreach (string text in values)
		{
			string text2 = text.ToLowerInvariant();
			if (text2 == "structure")
			{
				Components.Add("WearNTear");
			}
			else if (text2 == "creature")
			{
				Components.Add("Humanoid");
			}
			else
			{
				Components.Add(text);
			}
		}
	}

	public ZNetView[] GetObjects()
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		ZNetView[] source;
		if (Connect)
		{
			ZNetView hovered = Selector.GetHovered(50f, IncludedIds, Components, ExcludedIds);
			if ((Object)(object)hovered == (Object)null)
			{
				return Array.Empty<ZNetView>();
			}
			source = Selector.GetConnected(hovered, ConnectionIds, IncludedIds, ExcludedIds);
		}
		else if (Radius != null)
		{
			source = Selector.GetNearby(IncludedIds, Components, ExcludedIds, (Vector3)(((_003F?)Center) ?? From), Radius, Height);
		}
		else if (Width != null && Depth != null)
		{
			source = Selector.GetNearby(IncludedIds, Components, ExcludedIds, (Vector3)(((_003F?)Center) ?? From), Angle, Width, Depth, Height);
		}
		else
		{
			ZNetView hovered2 = Selector.GetHovered(50f, IncludedIds, Components, ExcludedIds);
			if ((Object)(object)hovered2 == (Object)null)
			{
				return Array.Empty<ZNetView>();
			}
			if (!Selector.GetPrefabs(IncludedIds).Contains(hovered2.GetZDO().GetPrefab()))
			{
				Helper.AddMessage(terminal, "Skipped: " + ((Object)hovered2).name + " has invalid id.", false);
				return Array.Empty<ZNetView>();
			}
			source = (ZNetView[])(object)new ZNetView[1] { hovered2 };
		}
		DataEntry matchData = ((Match == "") ? null : DataHelper.Get(Match));
		DataEntry unmatchData = ((Unmatch == "") ? null : DataHelper.Get(Unmatch));
		List<ZNetView> list = new List<ZNetView>();
		list.AddRange(source.Where(delegate(ZNetView view)
		{
			if (!Object.op_Implicit((Object)(object)view))
			{
				return false;
			}
			if (!view.GetZDO().IsValid())
			{
				terminal.AddString("Skipped: " + ((Object)view).name + " is not loaded.");
				return false;
			}
			ZDO zDO = view.GetZDO();
			if (matchData != null && !matchData.Match(new ParFunctions(DataParameters, zDO), zDO))
			{
				terminal.AddString("Skipped: " + ((Object)view).name + " not matching filter.");
				return false;
			}
			if (unmatchData != null && !unmatchData.MatchNone(new ParFunctions(DataParameters, zDO), zDO))
			{
				terminal.AddString("Skipped: " + ((Object)view).name + " matching filter.");
				return false;
			}
			if (!Roll(Chance))
			{
				terminal.AddString("Skipped: " + ((Object)view).name + " (chance).");
				return false;
			}
			return true;
		}));
		return list.ToArray();
	}

	public static bool Roll(float value)
	{
		if (value >= 1f)
		{
			return true;
		}
		return Random.NextDouble() < (double)value;
	}
}
