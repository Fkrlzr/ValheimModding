using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(InventoryGui), "UpdateRecipeList")]
internal static class LimitRecipesToStationPatch
{
	[HarmonyPriority(800)]
	private static void Prefix(ref List<Recipe> recipes)
	{
		if (!Settings.configNoCostLimitRecipesToStation.Value)
		{
			return;
		}
		Player localPlayer = Player.m_localPlayer;
		if (Object.op_Implicit((Object)(object)localPlayer) && Object.op_Implicit((Object)(object)localPlayer.m_currentStation) && (ZoneSystem.instance.GetGlobalKey((GlobalKeys)25) || localPlayer.NoCostCheat()))
		{
			recipes.RemoveAll((Recipe recipe) => !localPlayer.RequiredCraftingStation(recipe, 1, Settings.configNoCostRespectStationLevel.Value));
		}
	}
}
