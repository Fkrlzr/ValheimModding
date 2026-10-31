using System;
using System.Collections.Generic;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public static class Hammer
{
	public static bool AllLocationsObjects;

	public static bool RandomLocationDamage;

	private static bool OriginalUseDurability;

	private static float OriginalUseStamina;

	private static float OriginalUseEitr;

	private static bool OriginalCanRemoveFeasts;

	private static bool OriginalCanRemovePieces;

	public static int MenuRevision { get; private set; }

	public static void Place()
	{
		Player localPlayer = Player.m_localPlayer;
		if (Object.op_Implicit((Object)(object)localPlayer))
		{
			localPlayer.m_placePressedTime = Time.time;
			localPlayer.m_lastToolUseTime = 0f;
			localPlayer.UpdatePlacement(true, 0f);
		}
	}

	public static void SelectEmpty()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between I4 and Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		PieceTable buildPieces = Helper.GetPlayer().m_buildPieces;
		if (Object.op_Implicit((Object)(object)buildPieces))
		{
			PieceCategory selectedCategory = buildPieces.GetSelectedCategory();
			if (buildPieces.m_selectedPiece.Length > (int)selectedCategory)
			{
				buildPieces.m_selectedPiece[selectedCategory] = new Vector2Int(-1, -1);
			}
		}
	}

	public static void SelectRepair()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between I4 and Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		PieceTable buildPieces = Helper.GetPlayer().m_buildPieces;
		if (Object.op_Implicit((Object)(object)buildPieces))
		{
			PieceCategory selectedCategory = buildPieces.GetSelectedCategory();
			if (buildPieces.m_selectedPiece.Length > (int)selectedCategory)
			{
				buildPieces.m_selectedPiece[selectedCategory] = new Vector2Int(0, 0);
			}
		}
	}

	public static void SelectRepairIfEmpty()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between I4 and Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		PieceTable buildPieces = Helper.GetPlayer().m_buildPieces;
		if (!Object.op_Implicit((Object)(object)buildPieces))
		{
			return;
		}
		foreach (PieceCategory category in buildPieces.m_categories)
		{
			if (buildPieces.m_selectedPiece.Length <= (int)category)
			{
				break;
			}
			if (((Vector2Int)(ref buildPieces.m_selectedPiece[category])).x == -1)
			{
				buildPieces.m_selectedPiece[category] = new Vector2Int(0, 0);
			}
		}
	}

	public static void AddRemoveAnything(ItemData item)
	{
		if (item != null && Configuration.RemoveAnything && Object.op_Implicit((Object)(object)item.m_shared.m_buildPieces))
		{
			OriginalCanRemoveFeasts = item.m_shared.m_buildPieces.m_canRemoveFeasts;
			OriginalCanRemovePieces = item.m_shared.m_buildPieces.m_canRemovePieces;
			item.m_shared.m_buildPieces.m_canRemoveFeasts = true;
			item.m_shared.m_buildPieces.m_canRemovePieces = true;
		}
	}

	public static void RestoreRemoveAnything(ItemData item)
	{
		if (item != null && Configuration.RemoveAnything && Object.op_Implicit((Object)(object)item.m_shared.m_buildPieces))
		{
			item.m_shared.m_buildPieces.m_canRemoveFeasts = OriginalCanRemoveFeasts;
			item.m_shared.m_buildPieces.m_canRemovePieces = OriginalCanRemovePieces;
		}
	}

	public static void RemoveToolCosts(ItemData item)
	{
		if (item != null)
		{
			if (Configuration.NoDurabilityCost)
			{
				OriginalUseDurability = item.m_shared.m_useDurability;
				item.m_shared.m_useDurability = false;
			}
			if (Configuration.NoStaminaCost)
			{
				OriginalUseStamina = item.m_shared.m_attack.m_attackStamina;
				OriginalUseEitr = item.m_shared.m_attack.m_attackEitr;
				item.m_shared.m_attack.m_attackStamina = 0f;
				item.m_shared.m_attack.m_attackEitr = 0f;
			}
		}
	}

	public static void RestoreToolCosts(ItemData item)
	{
		if (item != null)
		{
			if (Configuration.NoDurabilityCost)
			{
				item.m_shared.m_useDurability = OriginalUseDurability;
			}
			if (Configuration.NoStaminaCost)
			{
				item.m_shared.m_attack.m_attackStamina = OriginalUseStamina;
				item.m_shared.m_attack.m_attackEitr = OriginalUseEitr;
			}
		}
	}

	public static bool IsHammer(string name)
	{
		if (!Configuration.HammerTools.Contains(name.ToLowerInvariant()))
		{
			return ToolManager.Tools.ContainsKey(name.ToLowerInvariant());
		}
		return true;
	}

	public static bool IsHammer(GameObject obj)
	{
		if (Object.op_Implicit((Object)(object)obj))
		{
			return IsHammer(Utils.GetPrefabName(obj));
		}
		return false;
	}

	public static bool IsHammer(ItemData item)
	{
		if (item != null)
		{
			if (!IsInfinityHammer(item))
			{
				return IsHammer(item.m_dropPrefab);
			}
			return true;
		}
		return false;
	}

	public static bool HasHammer(Player player)
	{
		if (Object.op_Implicit((Object)(object)player))
		{
			return IsHammer(((Humanoid)player).GetRightItem());
		}
		return false;
	}

	public static void Equip()
	{
		Player player = Helper.GetPlayer();
		if (!HasHammer(player))
		{
			ItemData val = ((Humanoid)player).GetInventory().m_inventory.Find(IsHammer) ?? throw new InvalidOperationException("Unable to find the hammer.");
			((Humanoid)player).EquipItem(val, true);
		}
	}

	public static void OpenBuildMenu()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		Player player = Helper.GetPlayer();
		bool num = EquipInfinityHammer();
		Selection.Clear();
		if (num)
		{
			((Humanoid)player).SetPlaceMode(player.m_buildPieces);
		}
		PieceTable buildPieces = player.m_buildPieces;
		if (Object.op_Implicit((Object)(object)buildPieces))
		{
			buildPieces.m_selectedCategory = (PieceCategory)((buildPieces.m_categories.Count > 0) ? ((int)buildPieces.m_categories[0]) : 0);
			buildPieces.m_selectedPiece[buildPieces.m_selectedCategory] = new Vector2Int(0, 0);
		}
		player.SetupPlacementGhost();
		MenuRevision++;
		Hud instance = Hud.instance;
		instance.m_pieceSelectionWindow.SetActive(false);
		instance.m_buildUi.Close();
		instance.m_hoveredPiece = null;
		instance.m_buildUi.m_currentBuildTool = null;
		instance.m_buildUi.OpenBuildMenu();
	}

	public static bool IsInfinityHammer(ItemData item)
	{
		if (item != null && item.m_customData != null)
		{
			return item.m_customData.ContainsKey("infinity_hammer");
		}
		return false;
	}

	public static bool IsInfinityHammer(PieceTable pt)
	{
		if (Object.op_Implicit((Object)(object)pt))
		{
			return ((Object)pt).name == "_InfinityHammerPieceTable";
		}
		return false;
	}

	public static bool EquipInfinityHammer()
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		Player player = Helper.GetPlayer();
		ItemData rightItem = ((Humanoid)player).GetRightItem();
		if (rightItem != null && rightItem.m_customData.ContainsKey("infinity_hammer"))
		{
			return true;
		}
		Inventory inventory = ((Humanoid)player).GetInventory();
		ItemData val = inventory.m_inventory.Find(IsInfinityHammer);
		if (val == null)
		{
			Vector2i val2 = inventory.FindEmptySlot(true);
			Dictionary<string, string> dictionary = new Dictionary<string, string> { ["infinity_hammer"] = "true" };
			if (!inventory.AddItem("Hammer", 1, 100f, val2, false, 1, 0, player.GetPlayerID(), Game.instance.GetPlayerProfile().GetName(), dictionary, 0, true, true, false))
			{
				throw new InvalidOperationException("Unable to add the hammer to inventory.");
			}
		}
		val = inventory.m_inventory.Find((ItemData item) => item?.m_customData.ContainsKey("infinity_hammer") ?? false);
		if (val == null)
		{
			throw new InvalidOperationException("Unable to find the hammer in inventory.");
		}
		((Humanoid)player).EquipItem(val, true);
		return false;
	}

	public static bool Is(ItemData item)
	{
		if (item != null)
		{
			return (Object)(object)item.m_shared.m_buildPieces != (Object)null;
		}
		return false;
	}

	public static bool HasAny()
	{
		Player player = Helper.GetPlayer();
		if (Object.op_Implicit((Object)(object)player))
		{
			return Is(((Humanoid)player).GetRightItem());
		}
		return false;
	}

	public static string Get()
	{
		Player player = Helper.GetPlayer();
		if (!Object.op_Implicit((Object)(object)player))
		{
			return "";
		}
		ItemData rightItem = ((Humanoid)player).GetRightItem();
		if (IsInfinityHammer(rightItem))
		{
			return "infinity_hammer";
		}
		if (rightItem == null)
		{
			return "";
		}
		return Utils.GetPrefabName(rightItem.m_dropPrefab).ToLower();
	}
}
