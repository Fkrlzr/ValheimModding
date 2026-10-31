using System;
using System.Collections.Generic;
using System.Linq;
using ServerDevcommands;
using UnityEngine;

namespace Service;

public static class Selector
{
	private static readonly int PlayerHash = StringExtensionMethods.GetStableHashCode("Player");

	private static KeyValuePair<int, int> RaftParent = ZDO.GetHashZDOID("MBParent");

	public static bool IsValid(ZNetView view)
	{
		if (Object.op_Implicit((Object)(object)view))
		{
			return IsValid(view.GetZDO());
		}
		return false;
	}

	public static bool IsValid(ZDO zdo)
	{
		if (zdo != null)
		{
			return zdo.IsValid();
		}
		return false;
	}

	public static ZNetView? GetHovered(float range, string[] included, string[] excluded)
	{
		return GetHovered(range, included, new HashSet<string>(), excluded);
	}

	public static ZNetView? GetHovered(float range, string[] included, HashSet<string> types, string[] excluded)
	{
		return GetHovered(Player.m_localPlayer, range, included, types, excluded)?.Obj;
	}

	public static int GetPrefabFromHit(RaycastHit hit)
	{
		return ((Component)((RaycastHit)(ref hit)).collider).GetComponentInParent<ZNetView>().GetZDO().GetPrefab();
	}

	public static Hovered? GetHovered(Player obj, float maxDistance, string[] included, string[] excluded, bool allowOtherPlayers = false)
	{
		return GetHovered(obj, maxDistance, included, new HashSet<string>(), excluded, allowOtherPlayers);
	}

	public static Hovered? GetHovered(Player obj, float maxDistance, string[] included, HashSet<string> types, string[] excluded, bool allowOtherPlayers = false)
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		allowOtherPlayers |= included.Contains<string>("Player");
		HashSet<int> allPrefabs = GetAllPrefabs(included);
		if (included.Length != 0 && allPrefabs.Count == 0)
		{
			throw new InvalidOperationException("No valid prefabs found.");
		}
		HashSet<int> allPrefabs2 = GetAllPrefabs(excluded);
		float num = Math.Max(maxDistance + 5f, 50f);
		int mask = LayerMask.GetMask(new string[11]
		{
			"item", "piece", "piece_nonsolid", "Default", "static_solid", "Default_small", "character", "character_net", "terrain", "vehicle",
			"character_trigger"
		});
		RaycastHit[] array = Physics.RaycastAll(((Component)GameCamera.instance).transform.position, ((Component)GameCamera.instance).transform.forward, num, mask);
		Array.Sort(array, (RaycastHit x, RaycastHit y) => ((RaycastHit)(ref x)).distance.CompareTo(((RaycastHit)(ref y)).distance));
		RaycastHit[] array2 = array;
		MineRock5 val2 = default(MineRock5);
		MineRock val3 = default(MineRock);
		for (int i = 0; i < array2.Length; i++)
		{
			RaycastHit val = array2[i];
			if (Vector3.Distance(((RaycastHit)(ref val)).point, ((Character)obj).m_eye.position) >= maxDistance)
			{
				continue;
			}
			ZNetView componentInParent = ((Component)((RaycastHit)(ref val)).collider).GetComponentInParent<ZNetView>();
			if (!IsValid(componentInParent))
			{
				continue;
			}
			ZDO zDO = componentInParent.GetZDO();
			int prefab = zDO.GetPrefab();
			if ((allPrefabs.Count > 0 && !allPrefabs.Contains(prefab)) || allPrefabs2.Contains(prefab) || Object.op_Implicit((Object)(object)((Component)((RaycastHit)(ref val)).collider).GetComponent<EffectArea>()) || (Object)(object)((Component)componentInParent).gameObject == (Object)(object)((Component)obj).gameObject || (Object)(object)((Component)componentInParent).GetComponentInChildren<Player>() == (Object)(object)obj || (!allowOtherPlayers && prefab == PlayerHash && !zDO.Persistent) || (types.Count > 0 && !ComponentInfo.HasComponent(componentInParent, types)))
			{
				continue;
			}
			int index = -1;
			if (((Component)componentInParent).TryGetComponent<MineRock5>(ref val2))
			{
				index = val2.GetAreaIndex(((RaycastHit)(ref val)).collider);
			}
			if (((Component)componentInParent).TryGetComponent<MineRock>(ref val3))
			{
				index = val3.GetAreaIndex(((RaycastHit)(ref val)).collider);
			}
			Room componentInParent2 = ((Component)((RaycastHit)(ref val)).collider).GetComponentInParent<Room>();
			if (Object.op_Implicit((Object)(object)componentInParent2))
			{
				Transform transform = ((Component)componentInParent).transform;
				for (int j = 0; j < transform.childCount; j++)
				{
					if ((Object)(object)((Component)transform.GetChild(j)).gameObject == (Object)(object)((Component)componentInParent2).gameObject)
					{
						index = j;
						break;
					}
				}
			}
			return new Hovered(componentInParent, index);
		}
		return null;
	}

	private static float GetX(float x, float y, float angle)
	{
		return Mathf.Cos(angle) * x - Mathf.Sin(angle) * y;
	}

	private static float GetY(float x, float y, float angle)
	{
		return Mathf.Sin(angle) * x + Mathf.Cos(angle) * y;
	}

	public static bool Within(Vector3 position, Vector3 center, float angle, Range<float> width, Range<float> depth, float height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		float x = position.x - center.x;
		float y = position.z - center.z;
		float x2 = GetX(x, y, angle);
		float y2 = GetY(x, y, angle);
		if (!WithinHeight(position, center, height))
		{
			return false;
		}
		return Helper.Within(width, depth, Mathf.Abs(x2), Mathf.Abs(y2));
	}

	public static bool Within(Vector3 position, Vector3 center, Range<float> radius, float height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (!WithinHeight(position, center, height))
		{
			return false;
		}
		return Helper.Within(radius, Utils.DistanceXZ(position, center));
	}

	private static bool WithinHeight(Vector3 position, Vector3 center, float height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		float y = center.y;
		float y2 = position.y;
		if (Helper.IsZero(height))
		{
			return Mathf.Abs(y2 - y) <= 1000f;
		}
		float num = Mathf.Min(y, y + height);
		float num2 = Mathf.Max(y, y + height);
		if (y2 >= num)
		{
			return y2 <= num2;
		}
		return false;
	}

	private static bool IsIncluded(string id, string name)
	{
		if (id.StartsWith("*", StringComparison.Ordinal) && id.EndsWith("*", StringComparison.Ordinal))
		{
			return name.Contains(id.Substring(1, id.Length - 2));
		}
		if (id.StartsWith("*", StringComparison.Ordinal))
		{
			return name.EndsWith(id.Substring(1), StringComparison.Ordinal);
		}
		if (id.EndsWith("*", StringComparison.Ordinal))
		{
			return name.StartsWith(id.Substring(0, id.Length - 1), StringComparison.Ordinal);
		}
		return id == name;
	}

	public static HashSet<int> GetPrefabs(string[] ids)
	{
		if (ids.Length == 0)
		{
			return GetSafePrefabs("");
		}
		HashSet<int> hashSet = new HashSet<int>();
		foreach (string id in ids)
		{
			hashSet.UnionWith(GetSafePrefabs(id));
		}
		return hashSet;
	}

	public static HashSet<int> GetAllPrefabs(string[] ids)
	{
		if (ids.Length == 0)
		{
			return new HashSet<int>();
		}
		HashSet<int> hashSet = new HashSet<int>();
		foreach (string id in ids)
		{
			hashSet.UnionWith(GetAllPrefabs(id));
		}
		return hashSet;
	}

	private static HashSet<int> GetSafePrefabs(string id)
	{
		string id2 = id;
		id2 = id2.ToLower();
		IEnumerable<GameObject> source = ZNetScene.instance.m_namedPrefabs.Values;
		if (id2 != "player")
		{
			source = source.Where((GameObject prefab) => ((Object)prefab).name != "Player");
		}
		source = ((id2 == "*" || id2 == "") ? source.Where((GameObject prefab) => !((Object)prefab).name.StartsWith("_", StringComparison.Ordinal)) : ((!id2.Contains("*")) ? source.Where((GameObject prefab) => ((Object)prefab).name.ToLower() == id2) : source.Where((GameObject prefab) => IsIncluded(id2, ((Object)prefab).name.ToLower()))));
		HashSet<int> hashSet = new HashSet<int>();
		foreach (int item in source.Select((GameObject prefab) => StringExtensionMethods.GetStableHashCode(((Object)prefab).name)))
		{
			hashSet.Add(item);
		}
		return hashSet;
	}

	private static HashSet<int> GetAllPrefabs(string id)
	{
		string id2 = id;
		id2 = id2.ToLower();
		IEnumerable<GameObject> source = ZNetScene.instance.m_namedPrefabs.Values;
		if (!(id2 == "*") && !(id2 == ""))
		{
			source = ((!id2.Contains("*")) ? source.Where((GameObject prefab) => ((Object)prefab).name.ToLower() == id2) : source.Where((GameObject prefab) => IsIncluded(id2, ((Object)prefab).name.ToLower())));
		}
		HashSet<int> hashSet = new HashSet<int>();
		foreach (int item in source.Select((GameObject prefab) => StringExtensionMethods.GetStableHashCode(((Object)prefab).name)))
		{
			hashSet.Add(item);
		}
		return hashSet;
	}

	public static ZNetView[] GetNearby(string[] included, HashSet<string> types, string[] excluded, Vector3 center, Range<float> radius, float height)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		Range<float> radius2 = radius;
		HashSet<int> prefabs = GetPrefabs(included);
		if (included.Length != 0 && prefabs.Count == 0)
		{
			throw new InvalidOperationException("No valid prefabs found.");
		}
		HashSet<int> allPrefabs = GetAllPrefabs(excluded);
		return GetNearby(prefabs, types, allPrefabs, checker);
		bool checker(Vector3 pos)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return Within(pos, center, radius2, height);
		}
	}

	public static ZNetView[] GetNearby(string[] included, HashSet<string> types, string[] excluded, Vector3 center, float angle, Range<float> width, Range<float> depth, float height)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		Range<float> width2 = width;
		Range<float> depth2 = depth;
		HashSet<int> prefabs = GetPrefabs(included);
		if (included.Length != 0 && prefabs.Count == 0)
		{
			throw new InvalidOperationException("No valid prefabs found.");
		}
		HashSet<int> allPrefabs = GetAllPrefabs(excluded);
		return GetNearby(prefabs, types, allPrefabs, checker);
		bool checker(Vector3 pos)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return Within(pos, center, angle, width2, depth2, height);
		}
	}

	public static ZNetView[] GetNearby(HashSet<int> included, HashSet<string> types, HashSet<int> excluded, Func<Vector3, bool> checker)
	{
		HashSet<int> included2 = included;
		HashSet<int> excluded2 = excluded;
		Func<Vector3, bool> checker2 = checker;
		_ = ZNetScene.instance;
		IEnumerable<ZNetView> source = ZNetScene.instance.m_instances.Values.Where(IsValid);
		if (included2.Count > 0)
		{
			source = source.Where((ZNetView view) => included2.Contains(view.GetZDO().GetPrefab()));
		}
		if (excluded2.Count > 0)
		{
			source = source.Where((ZNetView view) => !excluded2.Contains(view.GetZDO().GetPrefab()));
		}
		source = source.Where((ZNetView view) => checker2(view.GetZDO().GetPosition()));
		if (types.Count > 0)
		{
			source = ComponentInfo.HaveComponent(source, types);
		}
		ZNetView[] array = source.ToArray();
		if (array.Length == 0)
		{
			throw new InvalidOperationException("Nothing is nearby.");
		}
		return array;
	}

	public static ZNetView[] GetConnectedRaft(ZNetView baseView, HashSet<int> included, HashSet<int> excluded)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		HashSet<int> included2 = included;
		HashSet<int> excluded2 = excluded;
		ZDOID id = baseView.GetZDO().GetZDOID(RaftParent);
		IEnumerable<ZNetView> source = ZNetScene.instance.m_instances.Values.Where(IsValid);
		if (included2.Count > 0)
		{
			source = source.Where((ZNetView view) => included2.Contains(view.GetZDO().GetPrefab()));
		}
		if (excluded2.Count > 0)
		{
			source = source.Where((ZNetView view) => !excluded2.Contains(view.GetZDO().GetPrefab()));
		}
		source = source.Where((ZNetView view) => view.GetZDO().m_uid == id || view.GetZDO().GetZDOID(RaftParent) == id);
		List<ZNetView> list = new List<ZNetView>();
		list.AddRange(source);
		return list.ToArray();
	}

	public static ZNetView[] GetConnected(ZNetView baseView, string[] take, string[] include, string[] exclude)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		HashSet<int> allPrefabs = GetAllPrefabs(take);
		HashSet<int> allPrefabs2 = GetAllPrefabs(include);
		HashSet<int> allPrefabs3 = GetAllPrefabs(exclude);
		if (baseView.GetZDO().GetZDOID(RaftParent) != ZDOID.None)
		{
			return GetConnectedRaft(baseView, allPrefabs2, allPrefabs3);
		}
		WearNTear item = ((Component)baseView).GetComponent<WearNTear>() ?? throw new InvalidOperationException("Connected doesn't work for this object.");
		List<ZNetView> list = new List<ZNetView>();
		int prefab = baseView.GetZDO().GetPrefab();
		if (allPrefabs.Contains(prefab))
		{
			list.Add(baseView);
		}
		else if (allPrefabs.Count == 0 && !allPrefabs3.Contains(prefab) && (allPrefabs2.Count == 0 || allPrefabs2.Contains(prefab)))
		{
			list.Add(baseView);
		}
		HashSet<ZNetView> hashSet = new HashSet<ZNetView> { baseView };
		Queue<WearNTear> queue = new Queue<WearNTear>();
		queue.Enqueue(item);
		while (queue.Count > 0)
		{
			WearNTear val = queue.Dequeue();
			if (val.m_colliders == null)
			{
				val.SetupColliders();
			}
			foreach (BoundData bound in val.m_bounds)
			{
				int num = Physics.OverlapBoxNonAlloc(bound.m_pos, bound.m_size, WearNTear.s_tempColliders, bound.m_rot, WearNTear.s_rayMask);
				for (int i = 0; i < num; i++)
				{
					Collider val2 = WearNTear.s_tempColliders[i];
					if (val2.isTrigger || (Object)(object)val2.attachedRigidbody != (Object)null || val.m_colliders.Contains(val2))
					{
						continue;
					}
					WearNTear componentInParent = ((Component)val2).GetComponentInParent<WearNTear>();
					if (!Object.op_Implicit((Object)(object)componentInParent) || !IsValid(componentInParent.m_nview) || hashSet.Contains(componentInParent.m_nview))
					{
						continue;
					}
					hashSet.Add(componentInParent.m_nview);
					prefab = componentInParent.m_nview.GetZDO().GetPrefab();
					if (allPrefabs.Contains(prefab))
					{
						list.Add(componentInParent.m_nview);
					}
					if ((allPrefabs2.Count <= 0 || allPrefabs2.Contains(prefab)) && !allPrefabs3.Contains(prefab))
					{
						if (allPrefabs.Count == 0)
						{
							list.Add(componentInParent.m_nview);
						}
						queue.Enqueue(componentInParent);
					}
				}
			}
		}
		return list.ToArray();
	}

	public static ZNetView[] GetConnected(ZNetView baseView, string[] included, string[] excluded)
	{
		return GetConnected(baseView, Array.Empty<string>(), included, excluded);
	}
}
