using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using SkToolbox.Configuration;
using SkToolbox.Utility;
using TMPro;
using UnityEngine;

namespace SkToolbox;

internal static class SkCommandPatcher
{
	[HarmonyPatch(typeof(Console), "IsConsoleEnabled")]
	public static class PatchIsConsoleEnabled
	{
		[HarmonyPriority(0)]
		private static void Postfix(ref bool __result)
		{
			if (SkConfigEntry.CConsoleEnabled != null && SkConfigEntry.CConsoleEnabled.Value)
			{
				__result = true;
			}
		}
	}

	[HarmonyPatch(typeof(Terminal), "IsCheatsEnabled")]
	private static class PatchIsCheatsEnabled
	{
		private static void Postfix(ref bool __result)
		{
			if (BCheat)
			{
				__result = true;
			}
		}
	}

	[HarmonyPatch(typeof(ConsoleCommand), "IsValid")]
	private static class PatchConsoleCommandIsValid
	{
		private static readonly MethodInfo s_isAllowedCommand = AccessTools.Method(typeof(Terminal), "isAllowedCommand", (Type[])null, (Type[])null);

		private static void Postfix(ConsoleCommand __instance, Terminal context, bool skipAllowedCheck, ref bool __result)
		{
			if (!__result && BCheat && !((Object)(object)context == (Object)null) && __instance.OnlyServer && !__instance.OnlyAdmin && (!__instance.RemoteCommand || string.Equals(__instance.Command, "confirmcheats", StringComparison.OrdinalIgnoreCase)) && (!__instance.IsNetwork || !((Object)(object)ZNet.instance == (Object)null)) && (skipAllowedCheck || !(s_isAllowedCommand != null) || (bool)s_isAllowedCommand.Invoke(context, new object[1] { __instance })))
			{
				__result = true;
			}
		}
	}

	[HarmonyPatch(typeof(Chat), "InputText")]
	private static class PatchChatInputText
	{
		private static readonly FieldInfo commandsField = AccessTools.Field(typeof(Terminal), "commands");

		private static bool Prefix(Chat __instance)
		{
			if (SkConfigEntry.CAllowChatCommandInput != null && !SkConfigEntry.CAllowChatCommandInput.Value)
			{
				return true;
			}
			string text = ((TMP_InputField)((Terminal)__instance).m_input).text;
			if (!string.IsNullOrEmpty(text) && text.Length > 1 && text[0] == '/' && commandsField?.GetValue(null) is Dictionary<string, ConsoleCommand> dictionary && dictionary.ContainsKey(text.Split(' ')[0].ToLower()))
			{
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(WearNTear), "UpdateSupport")]
	private static class PatchUpdateSupport
	{
		private static bool Prefix(ref float ___m_support, ref ZNetView ___m_nview)
		{
			if (BFreeSupport)
			{
				if (___m_support != float.MaxValue)
				{
					___m_support = float.MaxValue;
					___m_nview.GetZDO().Set(ZDOVars.s_support, ___m_support);
				}
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
	private static class PatchUpdatePlacementGhost
	{
		private static void Postfix(bool flashGuardStone)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Invalid comparison between Unknown and I4
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			if (!bBuildAnywhere)
			{
				return;
			}
			try
			{
				if (!((Object)(object)Player.m_localPlayer != (Object)null))
				{
					return;
				}
				PlacementStatus privateField = Player.m_localPlayer.GetPrivateField<PlacementStatus>("m_placementStatus");
				if ((int)privateField == 0 || (int)privateField == 4 || (int)privateField == 12)
				{
					return;
				}
				GameObject privateField2 = Player.m_localPlayer.GetPrivateField<GameObject>("m_placementGhost");
				if ((Object)(object)privateField2 != (Object)null && privateField2.activeSelf)
				{
					Player.m_localPlayer.SetPrivateField("m_placementStatus", (object)(PlacementStatus)0);
					Piece component = privateField2.GetComponent<Piece>();
					if (component != null)
					{
						component.SetInvalidPlacementHeightlight(false);
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	[HarmonyPatch(typeof(Location), "IsInsideNoBuildLocation")]
	private static class PatchIsInsideNoBuildLocation
	{
		private static void Postfix(ref bool __result)
		{
			if (bBuildAnywhere)
			{
				__result = false;
			}
		}
	}

	[HarmonyPatch(typeof(GameCamera), "UpdateMouseCapture")]
	private static class PatchMenuMouseCapture
	{
		private static Vector2 savedPointer;

		private static bool cursorWasHeld;

		private static void Prefix(ref bool ___m_mouseCapture, out bool __state)
		{
			__state = ___m_mouseCapture;
			if (SkMenuController.IsOpen)
			{
				___m_mouseCapture = LookHeld;
			}
		}

		private static void Postfix(ref bool ___m_mouseCapture, bool __state)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			if (!SkMenuController.IsOpen)
			{
				return;
			}
			if (LookHeld)
			{
				if (!cursorWasHeld)
				{
					cursorWasHeld = true;
					savedPointer = (Vector2)ZInput.pointerPosition;
				}
				___m_mouseCapture = __state;
				return;
			}
			___m_mouseCapture = (___m_mouseCapture ? (!__state) : __state);
			ZCursor.LockState = (CursorLockMode)0;
			ZCursor.Show();
			if (cursorWasHeld)
			{
				cursorWasHeld = false;
				ZInput.SetMousePosition(savedPointer);
			}
		}
	}

	[HarmonyPatch(typeof(Player), "TakeInput")]
	private static class PatchMenuTakeInput
	{
		private static void Postfix(ref bool __result)
		{
			if (SkMenuController.IsOpen && !InDebugFlyMotion)
			{
				__result = false;
			}
		}
	}

	[HarmonyPatch(typeof(PlayerController), "TakeInput", new Type[] { typeof(bool) })]
	private static class PatchMenuPlayerControllerInput
	{
		private static void Postfix(bool look, ref bool __result)
		{
			if (SkMenuController.IsOpen && __result)
			{
				__result = (look ? LookHeld : WalkAllowed);
			}
		}
	}

	[HarmonyPatch(typeof(ZInput), "GetMouseScrollWheel")]
	private static class PatchMenuScrollWheel
	{
		private static void Postfix(ref float __result)
		{
			if (SkMenuController.IsOpen)
			{
				__result = 0f;
			}
		}
	}

	[HarmonyPatch(typeof(ZInput), "GetKeyDown", new Type[]
	{
		typeof(KeyCode),
		typeof(bool)
	})]
	private static class PatchMenuKeyDown
	{
		private static void Postfix(ref bool __result)
		{
			if (__result && SkMenuController.IsOpen && !BypassInputBlock)
			{
				__result = false;
			}
		}
	}

	[HarmonyPatch(typeof(ZInput), "GetButtonDown", new Type[] { typeof(string) })]
	private static class PatchMenuButtonDown
	{
		private static void Postfix(ref bool __result)
		{
			if (__result && SkMenuController.IsOpen && !BypassInputBlock)
			{
				__result = false;
			}
		}
	}

	[HarmonyPatch(typeof(PlayerProfile), "s_bypassCheatChecks", MethodType.Getter)]
	private static class PatchBypassCheatChecks
	{
		private static void Postfix(ref bool __result)
		{
			if (KeepAchievementsEnabled)
			{
				__result = true;
			}
		}
	}

	[HarmonyPatch(typeof(Player), "UseStamina")]
	private static class PatchInfiniteStamina
	{
		private static void Prefix(Player __instance, ref float v)
		{
			if (SkCommandProcessor.infStamina && !((Object)(object)__instance == (Object)null) && __instance == Player.m_localPlayer)
			{
				v = 0f;
			}
		}
	}

	[HarmonyPatch(typeof(Player), "UseEitr")]
	private static class PatchInfiniteEitr
	{
		private static void Prefix(Player __instance, ref float v)
		{
			if (SkCommandProcessor.infEitr && !((Object)(object)__instance == (Object)null) && __instance == Player.m_localPlayer)
			{
				v = 0f;
			}
		}
	}

	[HarmonyPatch(typeof(Player), "SetControls")]
	private static class PatchMenuJumpCrouch
	{
		private static bool lastJump;

		private static bool lastCrouch;

		private static void Prefix(Player __instance, ref bool jump, ref bool crouch)
		{
			if ((Object)(object)__instance == (Object)null || __instance != Player.m_localPlayer)
			{
				return;
			}
			bool button = ZInput.GetButton("Jump");
			bool flag = ZInput.GetButton("Crouch") || ZInput.GetButton("JoyCrouch");
			bool flag2 = button && !lastJump;
			bool flag3 = flag && !lastCrouch;
			lastJump = button;
			lastCrouch = flag;
			if (SkMenuController.IsOpen && WalkAllowed && !__instance.m_debugFly)
			{
				if (flag2)
				{
					jump = true;
				}
				if (flag3)
				{
					crouch = true;
				}
			}
		}
	}

	[HarmonyPatch(typeof(Character), "UpdateDebugFly")]
	private static class PatchMenuFlyUpDown
	{
		private static void Prefix(Character __instance)
		{
			InDebugFlyMotion = SkMenuController.IsOpen && WalkAllowed && (Object)(object)Player.m_localPlayer != (Object)null && __instance == Player.m_localPlayer;
		}

		private static void Finalizer()
		{
			InDebugFlyMotion = false;
		}
	}

	[HarmonyPatch(typeof(Player), "OnSpawned")]
	private static class PatchReapplyToggles
	{
		private static void Postfix(Player __instance)
		{
			if ((Object)(object)__instance == (Object)null || __instance != Player.m_localPlayer)
			{
				return;
			}
			try
			{
				SkCommandProcessor.ReapplyOnSpawn(__instance);
			}
			catch (Exception ex)
			{
				SkUtilities.Logz(new string[2] { "RESPAWN", "ERROR" }, new string[1] { ex.Message }, (LogType)2);
			}
		}
	}

	[HarmonyPatch(typeof(Player), "OnDeath")]
	private static class PatchSnapshotOnDeath
	{
		private static void Prefix(Player __instance)
		{
			if ((Object)(object)__instance == (Object)null || __instance != Player.m_localPlayer)
			{
				return;
			}
			try
			{
				SkCommandProcessor.SnapshotOnDeath(__instance);
			}
			catch (Exception ex)
			{
				SkUtilities.Logz(new string[2] { "RESPAWN", "ERROR" }, new string[1] { ex.Message }, (LogType)2);
			}
		}
	}

	[HarmonyPatch(typeof(Character), "ApplyDamage")]
	private static class PatchGodModeBlocksDamage
	{
		private static bool Prefix(Character __instance, HitData hit)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected I4, but got Unknown
			Player localPlayer = Player.m_localPlayer;
			if ((Object)(object)localPlayer == (Object)null || __instance != localPlayer)
			{
				return true;
			}
			if (hit != null && SkCommandProcessor.immuneTo.Contains((int)hit.m_hitType))
			{
				return false;
			}
			if (SkConfigEntry.CGodModeBlocksDamage != null && !SkConfigEntry.CGodModeBlocksDamage.Value)
			{
				return true;
			}
			if (localPlayer.m_godMode)
			{
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(Achievements), "CanGetAchievements", new Type[] { typeof(bool) })]
	private static class PatchCanGetAchievements
	{
		private static void Postfix(ref bool __result)
		{
			if (KeepAchievementsEnabled)
			{
				ApplyAchievementBypass();
				__result = true;
			}
		}
	}

	[HarmonyPatch(typeof(ConsoleCommand), "RunAction")]
	private static class PatchRunActionKeepAchievements
	{
		private static void Prefix(out bool __state)
		{
			__state = false;
			try
			{
				if ((Object)(object)Game.instance != (Object)null)
				{
					__state = Game.instance.GetPlayerProfile()?.m_usedCheats ?? false;
				}
			}
			catch (Exception)
			{
			}
		}

		private static void Postfix(bool __state)
		{
			if (!KeepAchievementsEnabled || __state)
			{
				return;
			}
			try
			{
				if ((Object)(object)Game.instance != (Object)null)
				{
					PlayerProfile playerProfile = Game.instance.GetPlayerProfile();
					if (playerProfile != null)
					{
						playerProfile.m_usedCheats = false;
					}
				}
			}
			catch (Exception)
			{
			}
			ApplyAchievementBypass();
		}
	}

	private static Harmony harmony;

	private static bool initComplete;

	internal static bool BypassInputBlock;

	internal static bool LookHeld;

	internal static bool WalkAllowed;

	internal static bool InDebugFlyMotion;

	private static bool bCheat;

	private static bool bFreeSupport;

	public static bool bBuildAnywhere;

	private static bool clearedUsedCheats;

	public static Harmony Harmony
	{
		get
		{
			return harmony;
		}
		set
		{
			harmony = value;
		}
	}

	public static bool BCheat
	{
		get
		{
			return bCheat;
		}
		set
		{
			bCheat = value;
		}
	}

	public static bool BFreeSupport
	{
		get
		{
			return bFreeSupport;
		}
		set
		{
			bFreeSupport = value;
		}
	}

	public static bool InitComplete
	{
		get
		{
			return initComplete;
		}
		set
		{
			initComplete = value;
		}
	}

	internal static bool KeepAchievementsEnabled
	{
		get
		{
			if (SkConfigEntry.CKeepAchievements != null)
			{
				return SkConfigEntry.CKeepAchievements.Value;
			}
			return true;
		}
	}

	public static void InitPatch()
	{
		if (InitComplete)
		{
			return;
		}
		try
		{
			// Patch one class at a time so a single patch that cannot apply (for example because another mod changed
			// the target) is reported by name and does not stop every patch after it from being applied.
			harmony = new Harmony("com.speelo.speelostoolbox");
			int applied = 0;
			int failed = 0;
			foreach (Type type in typeof(SkCommandPatcher).Assembly.GetTypes())
			{
				if (!type.IsDefined(typeof(HarmonyAttribute), true))
				{
					continue;
				}
				try
				{
					harmony.CreateClassProcessor(type).Patch();
					applied++;
				}
				catch (Exception ex2)
				{
					failed++;
					Exception inner = ex2;
					while (inner.InnerException != null)
					{
						inner = inner.InnerException;
					}
					SkUtilities.Logz(new string[2] { "SkCommandPatcher", "PATCH" }, new string[2] { "Patch class failed: " + type.FullName, inner.GetType().Name + ": " + inner.Message }, (LogType)0);
				}
			}
			SkUtilities.Logz(new string[2] { "SkCommandPatcher", "PATCH" }, new string[1] { applied + " patch classes applied, " + failed + " failed." }, (LogType)3);
		}
		catch (Exception ex)
		{
			SkCommandProcessor.PrintOut("Something failed, there is a strong possibility another mod blocked this operation.", SkCommandProcessor.LogTo.Console);
			SkUtilities.Logz(new string[2] { "SkCommandPatcher", "PATCH" }, new string[3] { "PATCH => FAILED. CHECK FOR OTHER MODS BLOCKING PATCHES.\n", ex.Message, ex.StackTrace }, (LogType)0);
		}
		finally
		{
			InitComplete = true;
		}
	}

	internal static void ApplyAchievementBypass()
	{
		if (!KeepAchievementsEnabled)
		{
			return;
		}
		try
		{
			if (clearedUsedCheats || !((Object)(object)Game.instance != (Object)null))
			{
				return;
			}
			PlayerProfile playerProfile = Game.instance.GetPlayerProfile();
			if (playerProfile != null)
			{
				clearedUsedCheats = true;
				if (playerProfile.m_usedCheats)
				{
					playerProfile.m_usedCheats = false;
					SkUtilities.Logz(new string[1] { "ACHIEVEMENTS" }, new string[1] { "Cleared the permanent 'used cheats' flag on this character (KeepAchievements is on)." }, (LogType)3);
				}
			}
		}
		catch (Exception ex)
		{
			SkUtilities.Logz(new string[2] { "ACHIEVEMENTS", "ERROR" }, new string[1] { ex.Message }, (LogType)0);
		}
	}
}
