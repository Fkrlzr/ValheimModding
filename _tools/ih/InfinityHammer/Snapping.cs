using System;
using System.Collections.Generic;
using System.Linq;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public static class Snapping
{
	public static bool IsSnapPoint(GameObject obj)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			return obj.CompareTag("snappoint");
		}
		return false;
	}

	public static bool IsSnapPoint(Transform tr)
	{
		return IsSnapPoint(((Component)tr).gameObject);
	}

	public static List<GameObject> GetChildren(GameObject obj)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		List<GameObject> list = new List<GameObject>();
		foreach (Transform item in obj.transform)
		{
			Transform val = item;
			if (!IsSnapPoint(val))
			{
				list.Add(((Component)val).gameObject);
			}
		}
		return list;
	}

	public static List<GameObject> GetSnapPoints(GameObject obj)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		List<GameObject> list = new List<GameObject>();
		foreach (Transform item in obj.transform)
		{
			Transform val = item;
			if (IsSnapPoint(val))
			{
				list.Add(((Component)val).gameObject);
			}
		}
		return list;
	}

	public static int CountActiveChildren(GameObject obj)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		int num = 0;
		foreach (Transform item in obj.transform)
		{
			Transform val = item;
			if (((Component)val).gameObject.activeSelf && !IsSnapPoint(((Component)val).gameObject))
			{
				num++;
			}
		}
		return num;
	}

	public static int CountSnapPoints(GameObject obj)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		foreach (Transform item in obj.transform)
		{
			if (IsSnapPoint(((Component)item).gameObject))
			{
				num++;
			}
		}
		return num;
	}

	public static void CreateSnapPoint(GameObject parent, Vector3 pos, string name)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = new GameObject
		{
			name = name,
			layer = LayerMask.NameToLayer("piece"),
			tag = "snappoint"
		};
		val.SetActive(false);
		val.transform.parent = parent.transform;
		val.transform.localPosition = pos;
		val.transform.localRotation = Quaternion.identity;
	}

	public static void CreateSnapPoints(GameObject obj, List<Vector3> points)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		foreach (Vector3 point in points)
		{
			CreateSnapPoint(obj, point, $"Snap {++num}");
		}
	}

	public static void RegenerateSnapPoints(GameObject obj)
	{
		RemoveSnapPoints(obj);
		GenerateSnapPoints(obj);
	}

	public static void RemoveSnapPoints(GameObject obj)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (Transform item in obj.transform)
		{
			Transform val = item;
			if (IsSnapPoint(val))
			{
				Object.Destroy((Object)(object)((Component)val).gameObject);
			}
		}
	}

	public static void BuildSnaps(GameObject obj)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		if (Configuration.Dimensions.TryGetValue(Utils.GetPrefabName(obj).ToLower(), out var value))
		{
			Vector3 val = value / 2f;
			List<Vector3> list = ((Configuration.Snapping == "Edges" || Configuration.Snapping == "All") ? new List<Vector3>(6)
			{
				new Vector3(val.x, val.y, 0f),
				new Vector3(0f - val.x, val.y, 0f),
				new Vector3(0f, value.y, 0f),
				new Vector3(0f, 0f, 0f),
				new Vector3(0f, val.y, val.z),
				new Vector3(0f, val.y, 0f - val.z)
			} : new List<Vector3>());
			bool num = Configuration.Snapping == "Corners" || Configuration.Snapping == "All";
			if (val.x < 0.5f)
			{
				val.x = 0f;
			}
			if (val.y < 0.5f)
			{
				val.y = 0f;
			}
			if (val.z < 0.5f)
			{
				val.z = 0f;
			}
			List<Vector3> obj2 = (num ? new List<Vector3>(8)
			{
				new Vector3(val.x, 0f, val.z),
				new Vector3(val.x, 0f, 0f - val.z),
				new Vector3(val.x, value.y, val.z),
				new Vector3(val.x, value.y, 0f - val.z),
				new Vector3(0f - val.x, 0f, val.z),
				new Vector3(0f - val.x, 0f, 0f - val.z),
				new Vector3(0f - val.x, value.y, val.z),
				new Vector3(0f - val.x, value.y, 0f - val.z)
			} : new List<Vector3>());
			List<Vector3> list2 = list;
			List<Vector3> list3 = obj2;
			List<Vector3> list4 = new List<Vector3>(list2.Count + list3.Count);
			list4.AddRange(list2);
			list4.AddRange(list3);
			List<Vector3> points = UniquePoints(list4);
			CreateSnapPoints(obj.gameObject, points);
		}
	}

	public static void GenerateSnapPoints(GameObject obj)
	{
		CreateSnapPoints(obj, GenerateSnapPoints(GetChildren(obj)));
	}

	public static List<Vector3> GenerateSnapPoints(List<GameObject> objects)
	{
		if (objects.Count == 0)
		{
			return new List<Vector3>();
		}
		if (Configuration.Snapping == "Off")
		{
			List<Vector3> list = new List<Vector3>();
			GetSnapPoints(objects[0], list);
			return list;
		}
		List<Vector3> list2 = FindOuterPoints(SearchSnapPoints(objects));
		if (list2.Count == 0)
		{
			return list2;
		}
		if (Configuration.Snapping == "Corners")
		{
			return CornerSnap(list2);
		}
		if (Configuration.Snapping == "Edges")
		{
			return EdgeSnap(list2);
		}
		return list2;
	}

	private static List<Vector3> SearchSnapPoints(List<GameObject> objects)
	{
		List<Vector3> list = new List<Vector3>();
		foreach (GameObject @object in objects)
		{
			if (!IsSnapPoint(@object))
			{
				GetSnapPoints(@object, list);
			}
		}
		return list;
	}

	private static void GetSnapPoints(GameObject obj, List<Vector3> snapPoints)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			Transform child = obj.transform.GetChild(i);
			if (IsSnapPoint(child))
			{
				Vector3 item = obj.transform.localPosition + obj.transform.localRotation * child.localPosition;
				snapPoints.Add(item);
			}
		}
	}

	private static List<Vector3> FindOuterPoints(List<Vector3> snapPoints)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		if (snapPoints.Count == 0)
		{
			return new List<Vector3>();
		}
		float num = float.MaxValue;
		float num2 = float.MinValue;
		float num3 = float.MaxValue;
		float num4 = float.MinValue;
		float num5 = float.MinValue;
		float num6 = float.MaxValue;
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<Vector3> list3 = new List<Vector3>();
		List<Vector3> list4 = new List<Vector3>();
		List<Vector3> list5 = new List<Vector3>();
		List<Vector3> list6 = new List<Vector3>();
		foreach (Vector3 snapPoint in snapPoints)
		{
			if (Helper.Approx(snapPoint.x, num))
			{
				list.Add(snapPoint);
			}
			else if (snapPoint.x < num)
			{
				num = snapPoint.x;
				list = new List<Vector3>(1) { snapPoint };
			}
			if (Helper.Approx(snapPoint.x, num2))
			{
				list2.Add(snapPoint);
			}
			else if (snapPoint.x > num2)
			{
				num2 = snapPoint.x;
				list2 = new List<Vector3>(1) { snapPoint };
			}
			if (Helper.Approx(snapPoint.z, num3))
			{
				list3.Add(snapPoint);
			}
			else if (snapPoint.z < num3)
			{
				num3 = snapPoint.z;
				list3 = new List<Vector3>(1) { snapPoint };
			}
			if (Helper.Approx(snapPoint.z, num4))
			{
				list4.Add(snapPoint);
			}
			else if (snapPoint.z > num4)
			{
				num4 = snapPoint.z;
				list4 = new List<Vector3>(1) { snapPoint };
			}
			if (Helper.Approx(snapPoint.y, num5))
			{
				list5.Add(snapPoint);
			}
			else if (snapPoint.y > num5)
			{
				num5 = snapPoint.y;
				list5 = new List<Vector3>(1) { snapPoint };
			}
			if (Helper.Approx(snapPoint.y, num6))
			{
				list6.Add(snapPoint);
			}
			else if (snapPoint.y < num6)
			{
				num6 = snapPoint.y;
				list6 = new List<Vector3>(1) { snapPoint };
			}
		}
		List<Vector3> list7 = new List<Vector3>();
		list7.AddRange(list);
		list7.AddRange(list2);
		list7.AddRange(list3);
		list7.AddRange(list4);
		list7.AddRange(list5);
		list7.AddRange(list6);
		return UniquePoints(list7);
	}

	private static List<Vector3> CornerSnap(List<Vector3> snapPoints)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		Vector3 center = FindCenter(snapPoints);
		List<Tuple<Vector3, Vector3>> source = snapPoints.Select((Vector3 p) => Tuple.Create<Vector3, Vector3>(p - center, p)).ToList();
		Vector3 item = source.OrderBy((Tuple<Vector3, Vector3> p) => p.Item1.x + p.Item1.y + p.Item1.z).First().Item2;
		Vector3 item2 = source.OrderBy((Tuple<Vector3, Vector3> p) => p.Item1.x + p.Item1.y - p.Item1.z).First().Item2;
		Vector3 item3 = source.OrderBy((Tuple<Vector3, Vector3> p) => p.Item1.x - p.Item1.y + p.Item1.z).First().Item2;
		Vector3 item4 = source.OrderBy((Tuple<Vector3, Vector3> p) => p.Item1.x - p.Item1.y - p.Item1.z).First().Item2;
		Vector3 item5 = source.OrderBy((Tuple<Vector3, Vector3> p) => 0f - p.Item1.x + p.Item1.y + p.Item1.z).First().Item2;
		Vector3 item6 = source.OrderBy((Tuple<Vector3, Vector3> p) => 0f - p.Item1.x + p.Item1.y - p.Item1.z).First().Item2;
		Vector3 item7 = source.OrderBy((Tuple<Vector3, Vector3> p) => 0f - p.Item1.x - p.Item1.y + p.Item1.z).First().Item2;
		Vector3 item8 = source.OrderBy((Tuple<Vector3, Vector3> p) => 0f - p.Item1.x - p.Item1.y - p.Item1.z).First().Item2;
		return UniquePoints(new List<Vector3>(8) { item, item2, item3, item4, item5, item6, item7, item8 });
	}

	private static List<Vector3> EdgeSnap(List<Vector3> snapPoints)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		float num = float.MaxValue;
		float num2 = float.MinValue;
		float num3 = float.MaxValue;
		float num4 = float.MinValue;
		float num5 = float.MinValue;
		float num6 = float.MaxValue;
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<Vector3> list3 = new List<Vector3>();
		List<Vector3> list4 = new List<Vector3>();
		List<Vector3> list5 = new List<Vector3>();
		List<Vector3> list6 = new List<Vector3>();
		foreach (Vector3 snapPoint in snapPoints)
		{
			if (Helper.Approx(snapPoint.x, num))
			{
				list.Add(snapPoint);
			}
			else if (snapPoint.x < num)
			{
				num = snapPoint.x;
				list = new List<Vector3>(1) { snapPoint };
			}
			if (Helper.Approx(snapPoint.x, num2))
			{
				list2.Add(snapPoint);
			}
			else if (snapPoint.x > num2)
			{
				num2 = snapPoint.x;
				list2 = new List<Vector3>(1) { snapPoint };
			}
			if (Helper.Approx(snapPoint.z, num3))
			{
				list3.Add(snapPoint);
			}
			else if (snapPoint.z < num3)
			{
				num3 = snapPoint.z;
				list3 = new List<Vector3>(1) { snapPoint };
			}
			if (Helper.Approx(snapPoint.z, num4))
			{
				list4.Add(snapPoint);
			}
			else if (snapPoint.z > num4)
			{
				num4 = snapPoint.z;
				list4 = new List<Vector3>(1) { snapPoint };
			}
			if (Helper.Approx(snapPoint.y, num5))
			{
				list5.Add(snapPoint);
			}
			else if (snapPoint.y > num5)
			{
				num5 = snapPoint.y;
				list5 = new List<Vector3>(1) { snapPoint };
			}
			if (Helper.Approx(snapPoint.y, num6))
			{
				list6.Add(snapPoint);
			}
			else if (snapPoint.y < num6)
			{
				num6 = snapPoint.y;
				list6 = new List<Vector3>(1) { snapPoint };
			}
		}
		Vector3 item = FindCenter(list);
		Vector3 item2 = FindCenter(list2);
		Vector3 item3 = FindCenter(list3);
		Vector3 item4 = FindCenter(list4);
		Vector3 item5 = FindCenter(list5);
		Vector3 item6 = FindCenter(list6);
		List<Vector3> obj = new List<Vector3>(6) { item, item2, item3, item4, item5, item6 };
		Bounds val = default(Bounds);
		foreach (Vector3 item7 in obj)
		{
			((Bounds)(ref val)).Encapsulate(item7);
		}
		bool num7 = Helper.Approx(((Bounds)(ref val)).size.x, 0f);
		bool flag = Helper.Approx(((Bounds)(ref val)).size.z, 0f);
		bool flag2 = Helper.Approx(((Bounds)(ref val)).size.y, 0f);
		if (num7)
		{
			return UniquePoints(new List<Vector3>(4) { item3, item4, item5, item6 });
		}
		if (flag)
		{
			return UniquePoints(new List<Vector3>(4) { item, item2, item5, item6 });
		}
		if (flag2)
		{
			return UniquePoints(new List<Vector3>(4) { item, item2, item3, item4 });
		}
		return UniquePoints(new List<Vector3>(6) { item, item2, item3, item4, item5, item6 });
	}

	private static List<Vector3> UniquePoints(List<Vector3> points)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		List<Vector3> list = new List<Vector3>();
		foreach (Vector3 pos in points)
		{
			if (!list.Any((Vector3 p) => Helper.Approx(p.x, pos.x) && Helper.Approx(p.y, pos.y) && Helper.Approx(p.z, pos.z)))
			{
				list.Add(pos);
			}
		}
		return list;
	}

	private static Vector3 FindCenter(List<Vector3> snapPoints)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		Vector3 center = Vector3.zero;
		foreach (Vector3 snapPoint in snapPoints)
		{
			center += snapPoint;
		}
		center /= (float)snapPoints.Count;
		return snapPoints.OrderBy(delegate(Vector3 p)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = p - center;
			return ((Vector3)(ref val)).sqrMagnitude;
		}).First();
	}
}
