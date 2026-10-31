using System;
using System.Collections.Generic;
using System.Linq;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class TerrainParameters
{
	public Vector3 Position = Vector3.zero;

	public Vector3 Offset = Vector3.zero;

	public Vector3 Step = Vector3.zero;

	public float Size;

	public Range<float>? Radius;

	public Range<float>? Width;

	public Range<float>? Depth;

	public string[] IncludedIds = Array.Empty<string>();

	public string[] ExcludedIds = Array.Empty<string>();

	public float Angle;

	public float? Set;

	public bool Reset;

	public bool Void;

	public float? Delta;

	public float? Level;

	public float? Min;

	public float? Max;

	public float Smooth;

	public float? Slope;

	public float SlopeAngle;

	public string Paint = "";

	public bool FixedPosition;

	public bool FixedAngle;

	public BlockCheck BlockCheck;

	public Range<float>? Within;

	public float Chance = 1f;

	public bool Path;

	public bool Arc;

	public bool Curve;

	public List<List<Vector3>>? Paths;

	public Range<float>? PathRadius;

	public bool LevelToPath;

	public TerrainParameters(ConsoleEventArgs args)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)Player.m_localPlayer))
		{
			float num = (float)Math.PI / 4f;
			Position = ((Component)Player.m_localPlayer).transform.position;
			Quaternion rotation = ((Component)Player.m_localPlayer).transform.rotation;
			Angle = num * Mathf.Round(((Quaternion)(ref rotation)).eulerAngles.y / 45f);
		}
		ParseArgs(args.Args);
	}

	private float ParseAngle(string value)
	{
		return value switch
		{
			"n" => 0f, 
			"ne" => 45f, 
			"e" => 90f, 
			"se" => 135f, 
			"s" => 180f, 
			"sw" => 225f, 
			"w" => 270f, 
			"nw" => 315f, 
			_ => Parse.Float(value, 0f), 
		} * ((float)Math.PI / 180f);
	}

	protected void ParseArgs(string[] args)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_084d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0892: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_0698: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		string[] array = args;
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split('=');
			string text = array2[0].ToLower();
			if (array2.Length >= 2)
			{
				string text2 = array2[1].ToLower();
				if (text == "from")
				{
					FixedPosition = true;
					flag = Parse.Split(text2, ',').Length < 3;
					Position = Parse.VectorXZY(Parse.Split(text2, ','));
				}
			}
		}
		if (flag)
		{
			if (!ZoneSystem.instance.IsZoneLoaded(Position))
			{
				throw new InvalidOperationException("Unable to find the ground height. Use <color=yellow>from</color> with the y coordinate.");
			}
			Position.y = ZoneSystem.instance.GetGroundHeight(Position);
		}
		array = args;
		for (int i = 0; i < array.Length; i++)
		{
			string[] array3 = array[i].Split('=');
			string text3 = array3[0].ToLower();
			if (text3 == "reset")
			{
				Reset = true;
			}
			if (text3 == "void")
			{
				Void = true;
			}
			if (text3 == "delta")
			{
				Set = 0f;
			}
			if (text3 == "level")
			{
				Level = Position.y;
				LevelToPath = true;
			}
			if (text3 == "path")
			{
				Path = true;
			}
			if (text3 == "arc")
			{
				Arc = true;
			}
			if (text3 == "curve")
			{
				Curve = true;
			}
			if (text3 == "blockcheck")
			{
				BlockCheck = BlockCheck.On;
			}
			if (text3 == "circle")
			{
				Radius = new Range<float>(0f);
			}
			if (text3 == "rect")
			{
				Width = new Range<float>(0f);
				Depth = new Range<float>(0f);
			}
			if (text3 == "slope")
			{
				Slope = 0f;
			}
			if (array3.Length < 2)
			{
				continue;
			}
			string text4 = array3[1].ToLower();
			string[] array4 = Parse.Split(text4, ',');
			if (text3 == "chance")
			{
				Range<float> val = Parse.FloatRange(text4, 1f);
				Chance = Helper.RandomValue(val);
			}
			if (text3 == "id")
			{
				IncludedIds = array4;
			}
			if (text3 == "ignore")
			{
				ExcludedIds = array4;
			}
			if (text3 == "circle")
			{
				Radius = Parse.FloatRange(text4, 0f);
			}
			if (text3 == "rect")
			{
				Range<Vector3> val2 = Parse.ScaleRange(text4);
				Width = new Range<float>(val2.Min.x, val2.Max.x);
				Depth = new Range<float>(val2.Min.z, val2.Max.z);
			}
			if (text3 == "paint")
			{
				Paint = text4;
			}
			if (text3 == "angle")
			{
				FixedAngle = true;
				Angle = ParseAngle(text4);
			}
			if (text3 == "delta")
			{
				Set = Parse.Float(text4, 0f);
			}
			if (text3 == "min")
			{
				Min = Parse.Float(text4, float.MinValue);
			}
			if (text3 == "max")
			{
				Max = Parse.Float(text4, float.MaxValue);
			}
			if (text3 == "raise")
			{
				Delta = Parse.Float(text4, 0f);
			}
			if (text3 == "lower")
			{
				Delta = 0f - Parse.Float(text4, 0f);
			}
			if (text3 == "smooth")
			{
				Smooth = Parse.Float(text4, 0f);
			}
			if (text3 == "slope")
			{
				Slope = Parse.Float(array4, 0, 0f);
				if (array4.Length > 1)
				{
					SlopeAngle = ParseAngle(array4[1]);
				}
			}
			if (text3 == "offset")
			{
				Offset = Parse.VectorZXY(array4);
			}
			if (text3 == "within")
			{
				Within = Parse.FloatRange(text4, 0f);
			}
			if (text3 == "level")
			{
				Level = Parse.Float(text4, Position.y);
				LevelToPath = false;
			}
			if (text3 == "step")
			{
				Step = Parse.VectorZXY(array4);
			}
			if (text3 == "blockcheck")
			{
				switch (text4)
				{
				case "on":
					BlockCheck = BlockCheck.On;
					break;
				case "inverse":
					BlockCheck = BlockCheck.Inverse;
					break;
				case "off":
					BlockCheck = BlockCheck.Off;
					break;
				default:
					throw new InvalidOperationException("Invalid value " + text4 + " for blockcheck.");
				}
			}
		}
		if (Path || Arc || Curve)
		{
			HandlePath(args);
		}
		else
		{
			HandleTo(args);
		}
		if (Radius != null && Depth != null)
		{
			throw new InvalidOperationException("<color=yellow>circle</color> and <color=yellow>rect</color> parameters can't be used together.");
		}
		if (Radius == null && Depth == null)
		{
			throw new InvalidOperationException("<color=yellow>circle</color> or <color=yellow>rect</color> parameter must be used.");
		}
		if (Radius != null)
		{
			Size = Radius.Max;
		}
		if (Depth != null && Width != null)
		{
			Size = Mathf.Max(Depth.Max, Width.Max);
		}
		if (Paths != null)
		{
			if (Slope.HasValue)
			{
				throw new InvalidOperationException("<color=yellow>slope</color> can't be used with <color=yellow>path</color>. Use <color=yellow>level</color> to follow the path height.");
			}
			PathRadius = Radius ?? Width;
			List<Vector3> list = Paths.SelectMany((List<Vector3> path) => path).ToList();
			Vector3 val3 = list[0];
			Vector3 val4 = list[0];
			foreach (Vector3 item in list)
			{
				val3 = Vector3.Min(val3, item);
				val4 = Vector3.Max(val4, item);
			}
			Position = (val3 + val4) / 2f;
			Size = Utils.DistanceXZ(val3, val4) / 2f + PathRadius.Max;
			return;
		}
		if (Step != Vector3.zero)
		{
			float num = Size;
			float num2 = Size;
			if (Width != null)
			{
				num = Width.Max;
			}
			if (Depth != null)
			{
				num2 = Depth.Max;
			}
			Offset.x += Step.x * num * 2f;
			Offset.z += Step.z * num2 * 2f;
			if (Slope.HasValue)
			{
				Offset.y = Slope.Value * (Step.z + Step.y);
				if (Level.HasValue)
				{
					Level += Offset.y - 0.5f * Slope.Value;
				}
			}
		}
		if (Offset != Vector3.zero)
		{
			Vector3 offset = Offset;
			Offset.x = Mathf.Cos(Angle) * offset.x + Mathf.Sin(Angle) * offset.z;
			Offset.z = Mathf.Cos(Angle) * offset.z - Mathf.Sin(Angle) * offset.x;
			Position += Offset;
		}
		if (Radius != null)
		{
			SlopeAngle += Angle;
		}
	}

	private void HandlePath(string[] args)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < args.Length; i++)
		{
			string[] array = args[i].Split('=');
			if (array.Length < 2)
			{
				continue;
			}
			string text = array[0].ToLower();
			if (text != "point" && text != "p")
			{
				continue;
			}
			string[] array2 = Parse.Split(array[1].ToLower(), ',');
			Vector3 val = Parse.VectorXZY(array2);
			if (array2.Length < 3)
			{
				if (!ZoneSystem.instance.IsZoneLoaded(val))
				{
					throw new InvalidOperationException("Unable to find the ground height. Use <color=yellow>" + text + "</color> with the y coordinate.");
				}
				val.y = ZoneSystem.instance.GetGroundHeight(val);
			}
			list.Add(val);
		}
		if (list.Count < 2)
		{
			throw new InvalidOperationException("<color=yellow>path</color> requires at least two points. Use <color=yellow>point</color> (or <color=yellow>p</color>) parameters.");
		}
		Paths = new List<List<Vector3>>();
		if (Path)
		{
			Paths.Add(TerrainPath.Sample(list, PathMode.Line));
		}
		if (Arc)
		{
			Paths.Add(TerrainPath.Sample(list, PathMode.Arc));
		}
		if (Curve)
		{
			Paths.Add(TerrainPath.Sample(list, PathMode.Curve));
		}
	}

	private void HandleTo(string[] args)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < args.Length; i++)
		{
			string[] array = args[i].Split('=');
			string text = array[0].ToLower();
			if (array.Length < 2)
			{
				continue;
			}
			string text2 = array[1].ToLower();
			if (!(text == "to"))
			{
				continue;
			}
			Vector3 val = Parse.VectorXZY(Parse.Split(text2, ','));
			if (Slope == 0f && Parse.Split(text2, ',').Length < 3)
			{
				if (!ZoneSystem.instance.IsZoneLoaded(val))
				{
					throw new InvalidOperationException("Unable to find the ground height. Use <color=yellow>to</color> with the y coordinate.");
				}
				val.y = ZoneSystem.instance.GetGroundHeight(val);
			}
			float num = Utils.DistanceXZ(Position, val);
			if (Radius != null)
			{
				Radius = new Range<float>(num / 2f);
			}
			Range<float>? width = Width;
			if (width != null && width.Max == 0f)
			{
				Width = new Range<float>(num / 2f);
			}
			if (Depth != null)
			{
				Depth = new Range<float>(num / 2f);
			}
			FixedAngle = true;
			Angle = Vector3.SignedAngle(Vector3.forward, Utils.DirectionXZ(val - Position), Vector3.up) * (float)Math.PI / 180f;
			Position.x = (Position.x + val.x) / 2f;
			Position.z = (Position.z + val.z) / 2f;
			if (Slope.HasValue)
			{
				if (Slope == 0f)
				{
					Slope = val.y - Position.y;
				}
				Position.y += Slope.Value / 2f;
			}
		}
	}
}
