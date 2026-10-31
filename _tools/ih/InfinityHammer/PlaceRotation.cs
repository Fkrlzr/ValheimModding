using System;
using System.Reflection;
using HarmonyLib;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public static class PlaceRotation
{
	public static Assembly? Comfy;

	public static Assembly? Reloaded;

	public static void Set(GameObject obj)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		Player player = Helper.GetPlayer();
		Quaternion rotation = obj.transform.rotation;
		player.m_placeRotation = Mathf.RoundToInt(((Quaternion)(ref rotation)).eulerAngles.y / 22.5f);
		if (Comfy != null)
		{
			ComfySet(obj.GetComponent<Piece>());
		}
		if (Reloaded != null)
		{
			SetReloaded(obj.transform.rotation);
		}
	}

	public static void Set(Quaternion rotation)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		Helper.GetPlayer().m_placeRotation = Mathf.RoundToInt(((Quaternion)(ref rotation)).eulerAngles.y / 22.5f);
		if (Comfy == null && Reloaded == null)
		{
			return;
		}
		GameObject placementGhost = HammerHelper.GetPlacementGhost();
		if (Object.op_Implicit((Object)(object)placementGhost))
		{
			placementGhost.transform.rotation = rotation;
			if (Comfy != null)
			{
				ComfySet(placementGhost.GetComponent<Piece>());
			}
			if (Reloaded != null)
			{
				SetReloaded(placementGhost.transform.rotation);
			}
		}
	}

	public static void RotateX(float rotation)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Rotate(new Vector3(rotation, 0f, 0f));
	}

	public static void RotateY(float rotation)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		Player player = Helper.GetPlayer();
		player.m_placeRotation = Mathf.RoundToInt(((float)player.m_placeRotation * 22.5f + rotation) / 22.5f);
		Rotate(new Vector3(0f, rotation, 0f));
	}

	public static void RotateZ(float rotation)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Rotate(new Vector3(0f, 0f, rotation));
	}

	private static void Rotate(Vector3 rotation)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Rotate(Quaternion.Euler(rotation));
	}

	private static void Rotate(Quaternion rotation)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		GameObject placementGhost = HammerHelper.GetPlacementGhost();
		if (Object.op_Implicit((Object)(object)placementGhost))
		{
			Set(placementGhost.transform.rotation * rotation);
		}
	}

	private static Type ComfyType()
	{
		return Comfy.GetType("ComfyGizmo.RotationManager");
	}

	private static void ComfySet(Piece piece)
	{
		AccessTools.Method(ComfyType(), "MatchPieceRotation", (Type[])null, (Type[])null).Invoke(null, new object[1] { piece });
	}

	private static GameObject GetReloaded()
	{
		GameObject val = GameObject.Find("GizmoRoot(Clone)");
		if (Object.op_Implicit((Object)(object)val))
		{
			return val;
		}
		Player localPlayer = Player.m_localPlayer;
		if (Object.op_Implicit((Object)(object)localPlayer))
		{
			localPlayer.UpdatePlacementGhost(false);
			localPlayer.UpdatePlacement(false, 0f);
		}
		return GameObject.Find("GizmoRoot(Clone)");
	}

	private static void SetReloaded(Quaternion rotation)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		GameObject reloaded = GetReloaded();
		if (Object.op_Implicit((Object)(object)reloaded))
		{
			reloaded.transform.rotation = rotation;
		}
	}
}
