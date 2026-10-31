using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace MobBattle;

internal static class BattlePatches
{
	// Creatures in a battle only fight each other. Everything else (the player, wild creatures) is neutral to them,
	// so the player is a pure spectator. Outside battles nothing changes.
	[HarmonyPatch(typeof(BaseAI), nameof(BaseAI.IsEnemy), typeof(Character), typeof(Character))]
	private static class BattleEnemies
	{
		[UsedImplicitly]
		private static void Postfix(Character a, Character b, ref bool __result)
		{
			int sideA = BattleManager.GetSide(a);
			int sideB = BattleManager.GetSide(b);
			if (sideA == 0 && sideB == 0)
			{
				return;
			}

			__result = sideA != 0 && sideB != 0 && sideA != sideB;
		}
	}

	// Backs up CharacterDrop.SetDropsEnabled for creatures that moved to a client where it was never called.
	[HarmonyPatch(typeof(CharacterDrop), nameof(CharacterDrop.OnDeath))]
	private static class NoBattleDrops
	{
		[UsedImplicitly]
		private static bool Prefix(CharacterDrop __instance)
		{
			return !MobBattle.disableDrops.Value || BattleManager.GetSide(__instance.m_character) == 0;
		}
	}

	// Left click with the wand opens the menu, right click starts or stops a battle.
	[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.StartAttack))]
	private static class WandUse
	{
		private static float lastCall = -10f;

		[UsedImplicitly]
		private static bool Prefix(Humanoid __instance, bool secondaryAttack, ref bool __result)
		{
			if (__instance != Player.m_localPlayer)
			{
				return true;
			}

			ItemDrop.ItemData? weapon = __instance.m_rightItem;
			if (weapon?.m_dropPrefab == null || weapon.m_dropPrefab.name != MobBattle.ItemName)
			{
				return true;
			}

			__result = false;

			// The game asks again every tick while the button is held. Only act on a fresh press.
			bool held = Time.unscaledTime - lastCall < 0.15f;
			lastCall = Time.unscaledTime;
			if (held || BattleUi.IsOpen)
			{
				return false;
			}

			if (secondaryAttack)
			{
				BattleUi.QuickToggle();
			}
			else
			{
				BattleUi.Open();
			}

			return false;
		}
	}

	// While the menu is open the game must not react to clicks or keys behind it.
	[HarmonyPatch(typeof(Player), "TakeInput")]
	private static class MenuBlocksPlayerInput
	{
		[UsedImplicitly]
		private static void Postfix(ref bool __result)
		{
			if (BattleUi.IsOpen)
			{
				__result = false;
			}
		}
	}

	[HarmonyPatch(typeof(PlayerController), "TakeInput", typeof(bool))]
	private static class MenuBlocksCamera
	{
		[UsedImplicitly]
		private static void Postfix(ref bool __result)
		{
			if (BattleUi.IsOpen)
			{
				__result = false;
			}
		}
	}

	// Without this the camera code locks the cursor again every frame.
	[HarmonyPatch(typeof(GameCamera), "UpdateMouseCapture")]
	private static class MenuFreesMouse
	{
		[UsedImplicitly]
		private static bool Prefix()
		{
			if (!BattleUi.IsOpen)
			{
				return true;
			}

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			return false;
		}
	}

	[HarmonyPatch(typeof(ZInput), "GetKeyDown", typeof(KeyCode), typeof(bool))]
	private static class TypingBlocksKeys
	{
		[UsedImplicitly]
		private static void Postfix(ref bool __result)
		{
			if (__result && BattleUi.IsOpen)
			{
				__result = false;
			}
		}
	}

	[HarmonyPatch(typeof(ZInput), "GetButtonDown", typeof(string))]
	private static class TypingBlocksButtons
	{
		[UsedImplicitly]
		private static void Postfix(ref bool __result)
		{
			if (__result && BattleUi.IsOpen)
			{
				__result = false;
			}
		}
	}
}
