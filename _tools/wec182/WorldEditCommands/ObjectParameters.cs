using System;
using System.Collections.Generic;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class ObjectParameters : SharedObjectParameters
{
	public Range<Vector3> Rotation = new Range<Vector3>(Vector3.zero);

	public Range<Vector3> Offset = new Range<Vector3>(Vector3.zero);

	public Vector3 From;

	public Vector3? Center;

	public Range<float>? Fuel;

	public string[] ConnectionIds = Array.Empty<string>();

	public string[] IncludedIds = Array.Empty<string>();

	public string[] ExcludedIds = Array.Empty<string>();

	public string Prefab = "";

	public string Origin = "player";

	public bool? Remove;

	public HashSet<string> Operations = new HashSet<string>();

	public bool ResetRotation;

	public bool Respawn;

	public Item? Visual;

	public float Angle;

	public long Creator;

	public Range<float>? Width;

	public Range<float>? Depth;

	public float Height;

	public float Chance = 1f;

	public string Match = "";

	public string Unmatch = "";

	public bool Connect;

	public HashSet<string> Components = new HashSet<string>();

	public string? StatusName;

	public Range<float>? StatusDuration;

	public Range<float>? StatusIntensity;

	public static HashSet<string> SupportedOperations = new HashSet<string>
	{
		"status", "health", "damage", "ammo", "ammotype", "durability", "stars", "tame", "wild", "level",
		"baby", "info", "data", "sleep", "remove", "visual", "model", "helmet", "left_hand", "right_hand",
		"shoulders", "legs", "utility", "move", "rotate", "scale", "chest", "fuel", "prefab", "respawn",
		"mirror", "creator", "copy", "field", "f", "components", "copy", "distant", "persist"
	};

	public ObjectParameters(ConsoleEventArgs args)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)Player.m_localPlayer))
		{
			From = ((Component)Player.m_localPlayer).transform.position;
		}
		ParseArgs(args.Args);
	}

	protected override void ParseArgs(string[] args)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		base.ParseArgs(args);
		for (int i = 0; i < args.Length; i++)
		{
			string[] array = args[i].Split('=');
			string text = array[0].ToLower();
			if (SupportedOperations.Contains(text))
			{
				if (text != "field" && text != "f" && Operations.Contains(text))
				{
					throw new InvalidOperationException("Operation " + text + " used multiple times.");
				}
				Operations.Add(text);
			}
			if (text == "center" || text == "mirror")
			{
				Center = From;
			}
			if (text == "connect")
			{
				Connect = true;
			}
			if (array.Length < 2)
			{
				continue;
			}
			string text2 = array[1];
			string[] array2 = Parse.Split(text2, ',');
			if (text == "rotate")
			{
				if (text2 == "reset")
				{
					ResetRotation = true;
				}
				else
				{
					Rotation = Parse.VectorYXZRange(text2, Vector3.zero);
				}
			}
			if (text == "connect")
			{
				ConnectionIds = array2;
			}
			if (text == "center" || text == "from")
			{
				Center = Parse.VectorXZY(array2);
			}
			if (text == "move")
			{
				Offset = Parse.VectorZXYRange(text2, Vector3.zero);
			}
			if (text == "id")
			{
				IncludedIds = array2;
			}
			if (text == "ignore")
			{
				ExcludedIds = array2;
			}
			if (text == "prefab")
			{
				Prefab = text2;
			}
			if (text == "origin")
			{
				Origin = text2.ToLower();
			}
			if (text == "visual")
			{
				Visual = new Item(text2);
			}
			if (text == "fuel")
			{
				Fuel = Parse.FloatRange(text2, 0f);
			}
			if (text == "chance")
			{
				Chance = Parse.Float(text2, 1f);
			}
			if (text == "type")
			{
				AddComponents(array2);
			}
			if (text == "match")
			{
				Match = text2;
			}
			if (text == "unmatch")
			{
				Unmatch = text2;
			}
			if (text == "rect")
			{
				Range<Vector3> val = Parse.ScaleRange(text2);
				Width = new Range<float>(val.Min.x, val.Max.x);
				Depth = new Range<float>(val.Min.z, val.Max.z);
			}
			if (text == "height")
			{
				Height = Parse.Float(text2, 0f);
			}
			if (text == "creator")
			{
				Creator = Parse.Long(text2, 0L);
			}
			if (text == "angle")
			{
				Angle = Parse.Float(text2, 0f) * (float)Math.PI / 180f;
			}
			if (text == "status")
			{
				StatusName = array2[0];
				StatusDuration = Parse.FloatRange(array2, 1, 60f);
				StatusIntensity = Parse.FloatRange(array2, 2, 100f);
			}
		}
		if (Operations.Contains("remove") && Operations.Count > 1)
		{
			throw new InvalidOperationException("Remove can't be used with other operations.");
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
}
