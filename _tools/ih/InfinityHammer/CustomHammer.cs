using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch]
public class CustomHammer
{
	private static Sprite? cachedSprite;

	private static PieceTable? cachedPieceTable;

	[HarmonyPatch(typeof(ItemData), "GetIcon")]
	[HarmonyPostfix]
	public static Sprite GetIcon(Sprite result, ItemData __instance)
	{
		if (!Hammer.IsInfinityHammer(__instance))
		{
			return result;
		}
		if ((Object)(object)cachedSprite == (Object)null)
		{
			cachedSprite = SpriteHelper.FindSprite("+\ud83d\udee0\ufe0f");
		}
		return cachedSprite ?? result;
	}

	[HarmonyPatch(typeof(ItemDrop), "GetHoverName")]
	[HarmonyPostfix]
	public static string GetHoverName(string result, ItemDrop __instance)
	{
		if (!Hammer.IsInfinityHammer(__instance.m_itemData))
		{
			return result;
		}
		return "Infinity Hammer";
	}

	[HarmonyPatch(typeof(Player), "SetPlaceMode")]
	[HarmonyPrefix]
	public static void SetPlaceModePrefix(Player __instance, ref PieceTable buildPieces)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		ItemData rightItem = ((Humanoid)__instance).GetRightItem();
		if ((Object)(object)__instance != (Object)(object)Player.m_localPlayer || !Object.op_Implicit((Object)(object)buildPieces) || !Hammer.IsInfinityHammer(rightItem))
		{
			return;
		}
		if (HammerMenuCommand.CurrentMode == MenuMode.Builds && HammerMenuCommand.CurrentFilter != "")
		{
			ItemDrop val = CustomMenu.GetBuildItems().FirstOrDefault((ItemDrop i) => ((Object)i).name.Equals(HammerMenuCommand.CurrentFilter, StringComparison.InvariantCultureIgnoreCase));
			if ((Object)(object)val != (Object)null)
			{
				buildPieces = val.m_itemData.m_shared.m_buildPieces;
				return;
			}
		}
		if ((Object)(object)cachedPieceTable == (Object)null)
		{
			GameObject val2 = new GameObject("_InfinityHammerPieceTable");
			Object.DontDestroyOnLoad((Object)val2);
			PieceTable obj = val2.AddComponent<PieceTable>();
			obj.m_canRemoveFeasts = true;
			obj.m_canRemovePieces = true;
			obj.m_skill = (SkillType)0;
			cachedPieceTable = obj;
		}
		buildPieces = cachedPieceTable;
	}
}
