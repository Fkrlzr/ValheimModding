using UnityEngine;

namespace InfinityHammer;

public class Scaling
{
	private static readonly ScalingData PieceScaling = new ScalingData(sanityY: true, minXZ: false, printChanges: true, Vector3.one);

	private static readonly ScalingData ToolScaling = new ScalingData(sanityY: false, minXZ: true, printChanges: false, new Vector3(10f, 0f, 10f));

	public static Vector3 Value => Get().Vec3;

	public static void Set(Vector3 value)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		Get().SetScale(value);
	}

	public static void Set(GameObject value)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Get().SetScale(value.transform.localScale);
	}

	public static void Print(Terminal terminal)
	{
		Get().Print(terminal);
	}

	public static ScalingData Get()
	{
		if (!Selection.Get().IsTool)
		{
			return PieceScaling;
		}
		return ToolScaling;
	}

	public static ScalingData Get(bool tool)
	{
		if (!tool)
		{
			return PieceScaling;
		}
		return ToolScaling;
	}
}
