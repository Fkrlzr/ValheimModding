using System.Globalization;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public static class Position
{
	public static Vector3? Override = null;

	public static Vector3 Offset = Vector3.zero;

	public static void ToggleFreeze()
	{
		if (Override.HasValue)
		{
			Unfreeze();
		}
		else
		{
			Freeze();
		}
	}

	public static void Freeze(Vector3 position)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		Override = position;
	}

	public static void Freeze()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		Player player = Helper.GetPlayer();
		GameObject placementGhost = player.m_placementGhost;
		Override = (Object.op_Implicit((Object)(object)placementGhost) ? Deapply(placementGhost.transform.position, placementGhost.transform.rotation) : ((Component)player).transform.position);
	}

	public static void Unfreeze()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		Override = null;
		if (Configuration.ResetOffsetOnUnfreeze)
		{
			Offset = Vector3.zero;
		}
	}

	public static void Apply(GameObject ghost)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		if (Selection.Get().PlayerHeight)
		{
			Player player = Helper.GetPlayer();
			ghost.transform.position = new Vector3(ghost.transform.position.x, ((Component)player).transform.position.y, ghost.transform.position.z);
		}
		ghost.transform.position = Apply(ghost.transform.position, ghost.transform.rotation);
	}

	public static Vector3 Apply(Vector3 point, Quaternion rotation)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (Override.HasValue)
		{
			point = Override.Value;
		}
		if (Selection.Get().TerrainGrid)
		{
			rotation = Quaternion.identity;
		}
		point += rotation * Vector3.right * Offset.x;
		point += rotation * Vector3.up * Offset.y;
		point += rotation * Vector3.forward * Offset.z;
		return point;
	}

	public static Vector3 Deapply(Vector3 point, Quaternion rotation)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (Override.HasValue)
		{
			point = Override.Value;
		}
		if (Selection.Get().TerrainGrid)
		{
			rotation = Quaternion.identity;
		}
		point -= rotation * Vector3.right * Offset.x;
		point -= rotation * Vector3.up * Offset.y;
		point -= rotation * Vector3.forward * Offset.z;
		return point;
	}

	public static void SetX(float value)
	{
		Offset.x = value;
	}

	public static void SetY(float value)
	{
		Offset.y = value;
	}

	public static void SetZ(float value)
	{
		Offset.z = value;
	}

	public static void MoveLeft(float value)
	{
		if (Selection.Get().TerrainGrid)
		{
			value = Mathf.Max(value, 1f);
		}
		Offset.x = Helper.Round(Offset.x - value);
	}

	public static void MoveRight(float value)
	{
		if (Selection.Get().TerrainGrid)
		{
			value = Mathf.Max(value, 1f);
		}
		Offset.x = Helper.Round(Offset.x + value);
	}

	public static void MoveDown(float value)
	{
		Offset.y = Helper.Round(Offset.y - value);
	}

	public static void MoveUp(float value)
	{
		Offset.y = Helper.Round(Offset.y + value);
	}

	public static void MoveBackward(float value)
	{
		if (Selection.Get().TerrainGrid)
		{
			value = Mathf.Max(value, 1f);
		}
		Offset.z = Helper.Round(Offset.z - value);
	}

	public static void MoveForward(float value)
	{
		if (Selection.Get().TerrainGrid)
		{
			value = Mathf.Max(value, 1f);
		}
		Offset.z = Helper.Round(Offset.z + value);
	}

	public static void Set(Vector3 value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Offset = value;
	}

	public static void Move(Vector3 value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Offset += value;
	}

	public static void Print(Terminal terminal)
	{
		if (!Configuration.DisableOffsetMessages)
		{
			HammerHelper.Message(terminal, "Offset set to forward: " + Offset.z.ToString("F1", CultureInfo.InvariantCulture) + ", up: " + Offset.y.ToString("F1", CultureInfo.InvariantCulture) + ", right: " + Offset.x.ToString("F1", CultureInfo.InvariantCulture) + ".");
		}
	}
}
