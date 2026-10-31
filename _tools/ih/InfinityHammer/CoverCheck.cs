using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InfinityHammer;

public static class CoverCheck
{
	public const float CoverRayCastLength = 30f;

	public const float CoverRaycastStart = 0.5f;

	public const float CoverBedLimit = 0.8f;

	public const float CoverCraftingStationLimit = 0.7f;

	public const float CoverFermenterLimit = 0.7f;

	public const float CoverFireplaceLimit = 0.7f;

	public const string FORMAT = "0.##";

	public static string CurrentCover = "";

	public static Vector3 GetCoverPoint(CraftingStation obj)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return obj.m_roofCheckPoint.position;
	}

	public static Vector3 GetCoverPoint(Fermenter obj)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return obj.m_roofCheckPoint.position;
	}

	public static Vector3 GetCoverPoint(Beehive obj)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return obj.m_coverPoint.position;
	}

	public static Vector3 GetCoverPoint(Fireplace obj)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		return ((Component)obj).transform.position + Vector3.up * obj.m_coverCheckOffset;
	}

	public static Vector3 GetCoverPoint(Bed obj)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return obj.GetSpawnPoint();
	}

	public static Vector3 GetCoverPoint(Windmill obj)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		return ((Component)obj.m_propeller).transform.position;
	}

	public static bool ChecksCover(Fireplace obj)
	{
		if ((Object)(object)obj.m_enabledObjectLow != (Object)null)
		{
			return (Object)(object)obj.m_enabledObjectHigh != (Object)null;
		}
		return false;
	}

	public static string String(string value, string color = "white")
	{
		return "<color=" + color + ">" + value + "</color>";
	}

	public static string Percent(double value, string color = "white")
	{
		return String(value.ToString("P0"), color);
	}

	public static string JoinLines(IEnumerable<string> lines)
	{
		return string.Join(". ", lines.Where((string line) => line != ""));
	}

	public static void CheckCover(GameObject obj)
	{
		CurrentCover = "";
		GetCover(obj.GetComponent<Beehive>());
		GetCover(obj.GetComponent<Fermenter>());
		GetCover(obj.GetComponent<Fireplace>());
		GetCover(obj.GetComponent<Bed>());
		GetCover(obj.GetComponent<CraftingStation>());
		GetCover(obj.GetComponent<Windmill>());
	}

	public static void GetCover(Beehive obj)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)obj))
		{
			GetCover(GetCoverPoint(obj), obj.m_maxCover, checkRoof: false, minLimit: false);
		}
	}

	public static void GetCover(Fermenter obj)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)obj))
		{
			GetCover(GetCoverPoint(obj), 0.7f);
		}
	}

	public static void GetCover(Fireplace obj)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)obj))
		{
			GetCover(GetCoverPoint(obj), 0.7f, checkRoof: false);
		}
	}

	public static void GetCover(Bed obj)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)obj))
		{
			GetCover(GetCoverPoint(obj), 0.8f);
		}
	}

	public static void GetCover(CraftingStation obj)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)obj))
		{
			GetCover(GetCoverPoint(obj), 0.7f);
		}
	}

	public static void GetCover(Windmill obj)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)obj))
		{
			GetCover(GetCoverPoint(obj), 0f, checkRoof: false);
		}
	}

	public static void GetCover(Vector3 position, float limit, bool checkRoof = true, bool minLimit = true)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		List<string> list = new List<string>();
		float num = default(float);
		bool flag = default(bool);
		Cover.GetCoverForPoint(position, ref num, ref flag, 0.5f);
		string text = Percent(num) + " cover";
		if (limit > 0f)
		{
			bool flag2 = (minLimit ? (num < limit) : (num > limit));
			text = text + " (" + Percent(limit, flag2 ? "yellow" : "white") + ")";
		}
		list.Add(text);
		if (checkRoof && !flag)
		{
			list.Add(String("Missing roof", "yellow"));
		}
		CurrentCover = JoinLines(list);
	}
}
