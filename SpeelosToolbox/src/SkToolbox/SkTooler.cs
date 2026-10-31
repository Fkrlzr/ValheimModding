using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HarmonyLib;
using UnityEngine;

namespace SkToolbox;

// Features carried over from ValheimTooler (Yohann MARTIN / Astropilot, MIT licence) that the toolbox did not have:
// instant crafting, teleporting with restricted items, automatic map pins for ore deposits, and removing your own
// tombstones. The ESP lives in SkEsp. Each one is off until switched on from the menu.
internal static class SkTooler
{
	internal static bool InstantCraft;

	internal static bool RestrictedTeleport;

	internal static bool AutoPin;

	internal static bool PermanentPins;

	private const string PinTag = "SK";

	private static readonly Regex PinPattern = new Regex("\\[SK[0-9]{5}\\]$");

	// Crafting progress is advanced by dt every frame, so a big dt finishes the craft at once.
	[HarmonyPatch(typeof(InventoryGui), "UpdateRecipe", new Type[]
	{
		typeof(Player),
		typeof(float)
	})]
	private static class PatchInstantCraft
	{
		private static void Prefix(ref float dt)
		{
			if (InstantCraft)
			{
				dt = 2f;
			}
		}
	}

	// Some items (metals, for example) normally stop a teleport. This lets them through.
	[HarmonyPatch(typeof(Inventory), "IsTeleportable")]
	private static class PatchTeleportable
	{
		private static bool Prefix(ref bool __result)
		{
			if (RestrictedTeleport)
			{
				__result = true;
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(Destructible), "Start")]
	private static class PatchDestructibleStart
	{
		private static void Postfix(Destructible __instance)
		{
			if (AutoPin)
			{
				TryPin(__instance);
			}
		}
	}

	private static void TryPin(Destructible destructible)
	{
		try
		{
			if (destructible == null || Minimap.instance == null)
			{
				return;
			}
			HoverText hover = ((Component)destructible).GetComponent<HoverText>();
			if (hover == null || ((Component)destructible).GetComponent<SkPinned>() != null || string.IsNullOrEmpty(hover.m_text))
			{
				return;
			}
			if (hover.m_text.ToLowerInvariant().Contains("deposit"))
			{
				string name = Localization.instance.Localize(hover.GetHoverName()) + " [" + PinTag + UnityEngine.Random.Range(0, 100000).ToString("00000") + "]";
				((Component)destructible).gameObject.AddComponent<SkPinned>().Init(name);
			}
		}
		catch (Exception)
		{
		}
	}

	// Pins deposits that are already loaded, since the patch above only sees ones that load afterwards.
	internal static void PinExisting()
	{
		Destructible[] all = UnityEngine.Object.FindObjectsByType<Destructible>((FindObjectsSortMode)0);
		foreach (Destructible destructible in all)
		{
			TryPin(destructible);
		}
	}

	internal static int ClearPins()
	{
		if (Minimap.instance == null)
		{
			return 0;
		}
		int count = 0;
		foreach (SkPinned pinned in UnityEngine.Object.FindObjectsByType<SkPinned>((FindObjectsSortMode)0))
		{
			pinned.pin = null;
			UnityEngine.Object.Destroy(pinned);
		}
		foreach (Minimap.PinData pin in new List<Minimap.PinData>(Minimap.instance.m_pins))
		{
			if (pin != null && pin.m_name != null && PinPattern.IsMatch(pin.m_name))
			{
				Minimap.instance.RemovePin(pin);
				count++;
			}
		}
		return count;
	}

	// Destroys the tombstones that belong to your character. Whatever is still inside them is lost.
	internal static int RemoveOwnTombstones()
	{
		Player localPlayer = Player.m_localPlayer;
		if (localPlayer == null)
		{
			return 0;
		}
		int count = 0;
		foreach (TombStone tombstone in UnityEngine.Object.FindObjectsByType<TombStone>((FindObjectsSortMode)0))
		{
			try
			{
				if (tombstone.GetOwner() == localPlayer.GetPlayerID() && tombstone.m_nview != null && tombstone.m_nview.IsValid())
				{
					tombstone.m_nview.Destroy();
					count++;
				}
			}
			catch (Exception)
			{
			}
		}
		return count;
	}
}

// Sits on a deposit and keeps one map pin for it. The pin goes when the deposit does, unless pins are permanent.
internal class SkPinned : MonoBehaviour
{
	internal Minimap.PinData pin;

	internal void Init(string pinName)
	{
		if (Minimap.instance != null)
		{
			pin = Minimap.instance.AddPin(((Component)this).transform.position, Minimap.PinType.Icon3, pinName, SkTooler.PermanentPins, false, 0L);
		}
	}

	private void OnDestroy()
	{
		if (pin != null && Minimap.instance != null && !SkTooler.PermanentPins)
		{
			Minimap.instance.RemovePin(pin);
		}
	}
}
