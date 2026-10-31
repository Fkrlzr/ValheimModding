using System;
using System.Collections.Generic;
using HarmonyLib;
using SkToolbox.Configuration;
using SkToolbox.Utility;
using UnityEngine;

namespace SkToolbox;

// Puts building pieces that the game never lets you craft (wall and floor parts for Ashlands ruins, dungeon and village
// parts, old test pieces) into the Hammer's build menu. Pieces that have no snap points get corner snap points built from
// their mesh bounds, so they can still snap to each other and to normal pieces.
//
// Nothing is saved: the pieces are added to the Hammer's piece list in memory, so turning it off or restarting the game
// puts the menu back the way it was.
internal static class SkHiddenPieces
{
	internal static int Added => addedPieces.Count;

	internal static int SnapPointsMade;

	private static readonly List<GameObject> addedPieces = new List<GameObject>();

	internal static bool Enabled
	{
		get
		{
			if (SkConfigEntry.CHiddenPieces != null)
			{
				return SkConfigEntry.CHiddenPieces.Value;
			}
			return false;
		}
	}

	private static PieceTable HammerTable()
	{
		if (ObjectDB.instance == null)
		{
			return null;
		}
		GameObject hammer = ObjectDB.instance.GetItemPrefab("Hammer");
		ItemDrop drop = (hammer != null) ? hammer.GetComponent<ItemDrop>() : null;
		return (drop != null && drop.m_itemData != null && drop.m_itemData.m_shared != null) ? drop.m_itemData.m_shared.m_buildPieces : null;
	}

	// Every piece some build menu already offers, by prefab name.
	private static HashSet<string> KnownPieces()
	{
		HashSet<string> known = new HashSet<string>();
		foreach (GameObject item in ObjectDB.instance.m_items)
		{
			ItemDrop drop = (item != null) ? item.GetComponent<ItemDrop>() : null;
			PieceTable table = (drop != null && drop.m_itemData != null && drop.m_itemData.m_shared != null) ? drop.m_itemData.m_shared.m_buildPieces : null;
			if (table == null)
			{
				continue;
			}
			foreach (GameObject piece in table.m_pieces)
			{
				if (piece != null)
				{
					known.Add(piece.name);
				}
			}
		}
		return known;
	}

	private static bool IsCandidate(GameObject prefab, HashSet<string> known)
	{
		if (prefab == null || prefab.name.StartsWith("_") || known.Contains(prefab.name))
		{
			return false;
		}
		if (prefab.GetComponent<ZNetView>() == null || prefab.GetComponent<Character>() != null || prefab.GetComponent<ItemDrop>() != null || prefab.GetComponent<Projectile>() != null)
		{
			return false;
		}
		return prefab.GetComponent<Piece>() != null || prefab.GetComponent<WearNTear>() != null;
	}

	// Adds the pieces. Returns how many were added by this call, or -1 when the game is not ready.
	internal static int Apply()
	{
		PieceTable table = HammerTable();
		if (table == null || ZNetScene.instance == null || ObjectDB.instance == null)
		{
			return -1;
		}
		HashSet<string> known = KnownPieces();
		List<GameObject> candidates = new List<GameObject>();
		foreach (GameObject prefab in ZNetScene.instance.m_prefabs)
		{
			if (IsCandidate(prefab, known) && !table.m_pieces.Contains(prefab))
			{
				candidates.Add(prefab);
			}
		}
		candidates.Sort((GameObject a, GameObject b) => string.Compare(a.name, b.name, StringComparison.OrdinalIgnoreCase));
		Sprite placeholder = SkIcons.First("Hammer", "Wood");
		int count = 0;
		foreach (GameObject prefab in candidates)
		{
			try
			{
				Piece piece = prefab.GetComponent<Piece>();
				if (piece == null)
				{
					piece = prefab.AddComponent<Piece>();
				}
				piece.m_enabled = true;
				if (!table.m_categories.Contains(piece.m_category))
				{
					piece.m_category = Piece.PieceCategory.Misc;
				}
				if (string.IsNullOrEmpty(piece.m_name) || Localization.instance.Localize(piece.m_name).StartsWith("["))
				{
					piece.m_name = prefab.name;
				}
				if (piece.m_icon == null)
				{
					piece.m_icon = placeholder;
				}
				if (piece.m_resources == null)
				{
					piece.m_resources = new Piece.Requirement[0];
				}
				SnapPointsMade += AddCornerSnapPoints(prefab);
				table.m_pieces.Add(prefab);
				addedPieces.Add(prefab);
				count++;
			}
			catch (Exception ex)
			{
				SkUtilities.Logz(new string[2] { "HIDDENPIECES", "SKIP" }, new string[2] { prefab.name, ex.Message }, (LogType)2);
			}
		}
		Refresh();
		return count;
	}

	internal static int Remove()
	{
		PieceTable table = HammerTable();
		int count = 0;
		if (table != null)
		{
			foreach (GameObject prefab in addedPieces)
			{
				if (table.m_pieces.Remove(prefab))
				{
					count++;
				}
			}
		}
		addedPieces.Clear();
		Refresh();
		return count;
	}

	private static void Refresh()
	{
		Player player = Player.m_localPlayer;
		if (player != null)
		{
			player.UpdateAvailablePiecesList();
		}
	}

	// A piece snaps to other pieces through child objects tagged "snappoint". Pieces that have none get one on each corner of
	// their mesh bounds. Returns how many it made.
	private static int AddCornerSnapPoints(GameObject prefab)
	{
		Transform root = prefab.transform;
		foreach (Transform child in root)
		{
			if (child.CompareTag("snappoint"))
			{
				return 0;
			}
		}
		Bounds bounds = default(Bounds);
		bool any = false;
		foreach (MeshFilter filter in prefab.GetComponentsInChildren<MeshFilter>(true))
		{
			Mesh mesh = filter.sharedMesh;
			if (mesh == null)
			{
				continue;
			}
			Matrix4x4 toRoot = root.worldToLocalMatrix * filter.transform.localToWorldMatrix;
			Bounds meshBounds = mesh.bounds;
			for (int i = 0; i < 8; i++)
			{
				Vector3 sign = new Vector3((i & 1) == 0 ? -1 : 1, (i & 2) == 0 ? -1 : 1, (i & 4) == 0 ? -1 : 1);
				Vector3 corner = toRoot.MultiplyPoint3x4(meshBounds.center + Vector3.Scale(meshBounds.extents, sign));
				if (!any)
				{
					bounds = new Bounds(corner, Vector3.zero);
					any = true;
				}
				else
				{
					bounds.Encapsulate(corner);
				}
			}
		}
		if (!any)
		{
			return 0;
		}
		int made = 0;
		for (int j = 0; j < 8; j++)
		{
			Vector3 sign2 = new Vector3((j & 1) == 0 ? -1 : 1, (j & 2) == 0 ? -1 : 1, (j & 4) == 0 ? -1 : 1);
			GameObject point = new GameObject("_sk_snap");
			try
			{
				point.tag = "snappoint";
				point.transform.SetParent(root, false);
				point.transform.localPosition = bounds.center + Vector3.Scale(bounds.extents, sign2);
				made++;
			}
			catch (Exception)
			{
				UnityEngine.Object.Destroy(point);
				break;
			}
		}
		return made;
	}

	// With the HiddenPieces option on, the pieces are added every time you spawn in.
	[HarmonyPatch(typeof(Player), "OnSpawned")]
	private static class PatchApplyOnSpawn
	{
		private static void Postfix(Player __instance)
		{
			if (Enabled && __instance != null && __instance == Player.m_localPlayer)
			{
				try
				{
					Apply();
				}
				catch (Exception ex)
				{
					SkUtilities.Logz(new string[2] { "HIDDENPIECES", "ERROR" }, new string[1] { ex.Message }, (LogType)2);
				}
			}
		}
	}
}
