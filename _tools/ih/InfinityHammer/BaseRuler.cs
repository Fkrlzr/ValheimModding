using System;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityHammer;

public abstract class BaseRuler : MonoBehaviour
{
	private static LayerMask Mask = LayerMask.op_Implicit(LayerMask.GetMask(new string[1] { "terrain" }));

	private static GameObject? basePrefab;

	public static bool SnapToGround = true;

	public bool Visible = true;

	private GameObject? Center;

	protected List<GameObject> Segments = new List<GameObject>();

	public static float Offset = 0f;

	private readonly List<GameObject> OffsetSegments = new List<GameObject>();

	private static GameObject BasePrefab
	{
		get
		{
			if (!Object.op_Implicit((Object)(object)basePrefab))
			{
				return basePrefab = GetBasePrefab();
			}
			return basePrefab;
		}
	}

	protected static Vector3 BaseScale => BasePrefab.transform.localScale;

	protected virtual bool HasCenter => true;

	private static GameObject GetBasePrefab()
	{
		GameObject prefab = ZNetScene.instance.GetPrefab("piece_workbench");
		if (!Object.op_Implicit((Object)(object)prefab))
		{
			throw new InvalidOperationException("Error: Unable to find the workbench object.");
		}
		return prefab.GetComponentInChildren<CircleProjector>(true).m_prefab;
	}

	public void OnDestroy()
	{
		foreach (GameObject segment in Segments)
		{
			Object.Destroy((Object)(object)segment);
		}
		foreach (GameObject offsetSegment in OffsetSegments)
		{
			Object.Destroy((Object)(object)offsetSegment);
		}
		Object.Destroy((Object)(object)Center);
		Center = null;
		Segments.Clear();
		OffsetSegments.Clear();
	}

	public void Refresh()
	{
		if (!((Component)this).gameObject.activeInHierarchy)
		{
			return;
		}
		if (!Visible)
		{
			CreateSegments(0);
			CreateOffsetSegments(0);
			return;
		}
		CreateLines();
		if (SnapToGround)
		{
			Snap();
		}
		CreateOffsetSegments(Segments.Count);
		CreateOffsetLines();
	}

	protected void CreateSegments(int count)
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		if (Segments.Count == count)
		{
			return;
		}
		foreach (GameObject segment in Segments)
		{
			Object.Destroy((Object)(object)segment);
		}
		Segments.Clear();
		Object.Destroy((Object)(object)Center);
		if (count != 0)
		{
			if (HasCenter)
			{
				Center = CreateMarker();
				Center.transform.localPosition = Vector3.zero;
				Center.transform.localRotation = Quaternion.identity;
				Center.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			}
			for (int i = 0; i < count; i++)
			{
				Segments.Add(CreateMarker());
			}
		}
	}

	private GameObject CreateMarker()
	{
		GameObject val = Object.Instantiate<GameObject>(BasePrefab, ((Component)this).transform);
		val.SetActive(true);
		Renderer[] componentsInChildren = val.GetComponentsInChildren<Renderer>(true);
		foreach (Renderer obj in componentsInChildren)
		{
			((Component)obj).gameObject.SetActive(true);
			obj.enabled = true;
		}
		return val;
	}

	protected abstract void CreateLines();

	private void Snap()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		foreach (GameObject segment in Segments)
		{
			segment.transform.position = Snap(segment.transform.position);
		}
	}

	private Vector3 Snap(Vector3 pos)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		RaycastHit val = default(RaycastHit);
		if (Physics.Raycast(pos + Vector3.up * 500f, Vector3.down, ref val, 1000f, ((LayerMask)(ref Mask)).value))
		{
			pos.y = ((RaycastHit)(ref val)).point.y + 0.05f;
		}
		return pos;
	}

	private void CreateOffsetSegments(int count)
	{
		if (Mathf.Approximately(Offset, 0f))
		{
			count = 0;
		}
		if (OffsetSegments.Count == count)
		{
			return;
		}
		foreach (GameObject offsetSegment in OffsetSegments)
		{
			Object.Destroy((Object)(object)offsetSegment);
		}
		OffsetSegments.Clear();
		for (int i = 0; i < count; i++)
		{
			OffsetSegments.Add(CreateMarker());
		}
	}

	private void CreateOffsetLines()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < OffsetSegments.Count; i++)
		{
			GameObject val = Segments[i];
			GameObject obj = OffsetSegments[i];
			obj.transform.localPosition = val.transform.localPosition + Vector3.up * Offset;
			obj.transform.localRotation = val.transform.localRotation;
			obj.transform.localScale = val.transform.localScale;
			obj.SetActive(val.activeSelf);
		}
	}

	protected Transform Get(int index)
	{
		return Segments[index].transform;
	}

	protected void Set(int index, Vector3 pos)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		Get(index).localPosition = pos;
	}

	protected void SetRot(int index, Vector3 rot)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Get(index).localRotation = Quaternion.LookRotation(rot, Vector3.up);
	}
}
