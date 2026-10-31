using System;
using System.Collections.Generic;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class HammerParameters
{
	public Vector3 Position = Vector3.zero;

	public Vector3? Scale;

	public string? Text;

	public int? Level;

	public float? Health;

	public float Angle;

	public bool Connect;

	public bool Freeze;

	public bool Pick;

	public string? Data;

	public Range<float>? Radius;

	public Range<float>? Width;

	public Range<float>? Depth;

	public bool Show = true;

	public bool Collision = true;

	public bool Interact = true;

	public bool Restrict = true;

	public float Height;

	public HashSet<string> Components = new HashSet<string>();

	public Wear Wear;

	public Growth Growth;

	public Fall Fall;

	public string[] Ignored = Array.Empty<string>();

	public string[] Included = Array.Empty<string>();

	public float? Terrain;

	public float? Paint;

	public float? HeightSmooth;

	public float? PaintSmooth;

	private static float ClampSmooth(string value)
	{
		float num = Parse.Float(value, 0f);
		if (num < 0f)
		{
			return 0f;
		}
		if (num > 1f)
		{
			return 1f;
		}
		return num;
	}

	public HammerParameters(ConsoleEventArgs args)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)Player.m_localPlayer))
		{
			Position = ((Component)Player.m_localPlayer).transform.position;
		}
		ParseArgs(args.Args);
	}

	protected void ParseArgs(string[] args)
	{
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		string[] array = args;
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split('=');
			string text = array2[0].ToLower();
			if (text == "connect")
			{
				Connect = true;
			}
			if (text == "freeze")
			{
				Freeze = true;
			}
			if (text == "pick")
			{
				Pick = true;
			}
			if (text == "terrain")
			{
				Terrain = 0f;
			}
			if (text == "paint")
			{
				Paint = 0f;
			}
			if (array2.Length < 2)
			{
				continue;
			}
			string text2 = array2[1].ToLower();
			string[] array3 = Parse.Split(text2, ',');
			if (text == "data")
			{
				Data = array2[1];
			}
			if (text == "connect")
			{
				Connect = HammerHelper.IsDown(text2);
			}
			if (text == "freeze")
			{
				Freeze = HammerHelper.IsDown(text2);
			}
			if (text == "pick")
			{
				Pick = HammerHelper.IsDown(text2);
			}
			if (text == "level")
			{
				Level = Parse.Int(text2, 1);
			}
			if (text == "stars")
			{
				Level = Parse.Int(text2, 0) + 1;
			}
			if (text == "text")
			{
				Text = array2[1].Replace("_", " ");
			}
			if (text == "health")
			{
				Health = Parse.Float(text2, 0f);
			}
			if (text == "from")
			{
				Position = Parse.VectorXZY(array3, Position);
			}
			if (text == "scale")
			{
				Scale = Parse.Scale(array3);
			}
			if (text == "circle")
			{
				Radius = Parse.FloatRange(text2, 0f);
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
			if (text == "angle")
			{
				Angle = Parse.Float(text2, 0f) * (float)Math.PI / 180f;
			}
			if (text == "type")
			{
				AddComponents(array3);
			}
			if (text == "wear" && text2 == "broken")
			{
				Wear = Wear.Broken;
			}
			if (text == "wear" && text2 == "damaged")
			{
				Wear = Wear.Damaged;
			}
			if (text == "wear" && text2 == "healthy")
			{
				Wear = Wear.Healthy;
			}
			if (text == "growth" && text2 == "big")
			{
				Growth = Growth.HealthyGrown;
			}
			if (text == "growth" && text2 == "big_bad")
			{
				Growth = Growth.UnhealthyGrown;
			}
			if (text == "growth" && text2 == "small")
			{
				Growth = Growth.Healthy;
			}
			if (text == "growth" && text2 == "small_bad")
			{
				Growth = Growth.Unhealthy;
			}
			if (text == "fall" && text2 == "solid")
			{
				Fall = Fall.Solid;
			}
			if (text == "fall" && text2 == "terrain")
			{
				Fall = Fall.Terrain;
			}
			if (text == "fall" && text2 == "off")
			{
				Fall = Fall.Off;
			}
			if (text == "show")
			{
				Show = Parse.Boolean(text2).GetValueOrDefault(true);
			}
			if (text == "collision")
			{
				Collision = Parse.Boolean(text2).GetValueOrDefault(true);
			}
			if (text == "interact")
			{
				Interact = Parse.Boolean(text2).GetValueOrDefault(true);
			}
			if (text == "restrict")
			{
				Restrict = Parse.Boolean(text2).GetValueOrDefault(true);
			}
			if (text == "include")
			{
				Included = array3;
			}
			if (text == "id")
			{
				Included = array3;
			}
			if (text == "ignore")
			{
				Ignored = array3;
			}
			if (text == "terrain")
			{
				Terrain = Parse.Float(text2, 0f);
			}
			if (text == "paint")
			{
				Paint = Parse.Float(text2, 0f);
			}
			if (text == "smooth")
			{
				if (array3.Length != 0 && !string.IsNullOrWhiteSpace(array3[0]))
				{
					HeightSmooth = ClampSmooth(array3[0]);
				}
				if (array3.Length > 1 && !string.IsNullOrWhiteSpace(array3[1]))
				{
					PaintSmooth = ClampSmooth(array3[1]);
				}
			}
		}
		if (Ignored.Length == 0)
		{
			Ignored = Configuration.IgnoredIds;
		}
		else if (Configuration.IgnoredIds.Length != 0)
		{
			string[] ignored = Ignored;
			array = Configuration.IgnoredIds;
			int i = 0;
			string[] array4 = new string[ignored.Length + array.Length];
			string[] array5 = ignored;
			foreach (string text3 in array5)
			{
				array4[i] = text3;
				i++;
			}
			array5 = array;
			foreach (string text3 in array5)
			{
				array4[i] = text3;
				i++;
			}
			Ignored = array4;
		}
		if (Radius != null && Depth != null)
		{
			throw new InvalidOperationException("<color=yellow>circle</color> and <color=yellow>rect</color> parameters can't be used together.");
		}
	}

	private void AddComponents(string[] values)
	{
		foreach (string item in values)
		{
			Components.Add(item);
		}
	}
}
