using System.Linq;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class HammerBlueprintPars
{
	public string Name = "";

	public string CenterPiece = "";

	public string SnapPiece = "";

	public bool LoadData = true;

	public Vector3 Scale = Vector3.one;

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

	public HammerBlueprintPars(ConsoleEventArgs args)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		string[] array = args.Args.Skip(1).ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i];
			string[] array2 = text.Split('=');
			if (array2.Length < 2)
			{
				if (i == 0)
				{
					Name = text;
				}
				if (i == 1)
				{
					CenterPiece = text;
				}
				if (i == 2)
				{
					Scale = Parse.Scale(Parse.Split(text, ','));
				}
				continue;
			}
			if (array2[0] == "center" || array2[0] == "c")
			{
				CenterPiece = array2[1];
			}
			if (array2[0] == "snap" || array2[0] == "s")
			{
				SnapPiece = array2[1];
			}
			if (array2[0] == "data" || array2[0] == "d")
			{
				LoadData = Parse.BoolNull(array2[1]).GetValueOrDefault(true);
			}
			if (array2[0] == "scale" || array2[0] == "sc")
			{
				Scale = Parse.Scale(Parse.Split(array2[1], ','));
			}
			if (array2[0] == "smooth" || array2[0] == "sm")
			{
				string[] array3 = array2[1].Split(',');
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
	}
}
