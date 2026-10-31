using HarmonyLib;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch(typeof(BuildUi), "UpdateSearch")]
internal static class KeepPipettesVisible
{
	private static void Postfix(BuildUi __instance)
	{
		PieceTable val = (Object.op_Implicit((Object)(object)Player.m_localPlayer) ? Player.m_localPlayer.m_buildPieces : null);
		if (!Object.op_Implicit((Object)(object)val))
		{
			return;
		}
		bool flag = false;
		foreach (BuildUiPieceButton pieceButton in __instance.m_pieceButtons)
		{
			if (Object.op_Implicit((Object)(object)pieceButton) && PinnedPieceList.PinIndex(pieceButton.Piece) >= 0 && val.m_availablePieces.Contains(pieceButton.Piece) && !((Component)pieceButton).gameObject.activeSelf)
			{
				((Component)pieceButton).gameObject.SetActive(true);
				flag = true;
			}
		}
		if (flag)
		{
			__instance.ConfigureButtonNavigation();
		}
	}
}
