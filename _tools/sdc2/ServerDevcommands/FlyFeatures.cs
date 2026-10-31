using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch]
public class FlyFeatures
{
	private static bool NoClipTurnedOn;

	private static bool NoClipPlayerEnabled()
	{
		if (Object.op_Implicit((Object)(object)Player.m_localPlayer) && ((Character)Player.m_localPlayer).IsDebugFlying())
		{
			return Settings.FlyNoClip;
		}
		return false;
	}

	private static bool NoClipCameraEnabled()
	{
		if (!Settings.NoClipView)
		{
			return NoClipPlayerEnabled();
		}
		return true;
	}

	private static bool CheckKeys(List<KeyCode> required, List<KeyCode> banned)
	{
		if (required.All(BindManager.GetKey))
		{
			return !banned.Any(BindManager.GetKey);
		}
		return false;
	}

	private static bool IsFlyUp()
	{
		if (!CheckKeys(Settings.FlyUpRequiredKeys, Settings.FlyUpBannedKeys))
		{
			return false;
		}
		if (Settings.FlyUpRequiredKeys.Count > Settings.FlyDownRequiredKeys.Count)
		{
			return true;
		}
		return !CheckKeys(Settings.FlyDownRequiredKeys, Settings.FlyDownBannedKeys);
	}

	private static bool IsFlyDown()
	{
		if (!CheckKeys(Settings.FlyDownRequiredKeys, Settings.FlyDownBannedKeys))
		{
			return false;
		}
		if (Settings.FlyDownRequiredKeys.Count > Settings.FlyUpRequiredKeys.Count)
		{
			return true;
		}
		return !CheckKeys(Settings.FlyUpRequiredKeys, Settings.FlyUpBannedKeys);
	}

	private static Vector2 CheckInvert(Vector2 vector)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		if (!Settings.FreeFlyCameraInvert)
		{
			return vector;
		}
		if (ZInput.IsGamepadActive())
		{
			if (PlayerController.m_invertCameraX)
			{
				vector.x *= -1f;
			}
			if (PlayerController.m_invertCameraY)
			{
				vector.y *= -1f;
			}
		}
		else if (PlayerController.m_invertMouse)
		{
			vector.y *= -1f;
		}
		return vector;
	}

	[HarmonyPatch(typeof(Character), "UpdateDebugFly")]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> UpdateDebugFlyTranspiler(IEnumerable<CodeInstruction> instructions)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		return new CodeMatcher(instructions, (ILGenerator)null).MatchStartForward((CodeMatch[])(object)new CodeMatch[1]
		{
			new CodeMatch((OpCode?)OpCodes.Ldstr, (object)"Jump", (string)null)
		}).SetOpcodeAndAdvance(OpCodes.Nop).Set(OpCodes.Call, Transpilers.EmitDelegate<Func<bool>>((Func<bool>)IsFlyUp).operand)
			.MatchStartForward((CodeMatch[])(object)new CodeMatch[1]
			{
				new CodeMatch((OpCode?)OpCodes.Ldc_I4, (object)306, (string)null)
			})
			.SetOpcodeAndAdvance(OpCodes.Nop)
			.SetOpcodeAndAdvance(OpCodes.Nop)
			.Set(OpCodes.Call, Transpilers.EmitDelegate<Func<bool>>((Func<bool>)IsFlyDown).operand)
			.InstructionEnumeration();
	}

	[HarmonyPatch(typeof(GameCamera), "UpdateFreeFly")]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> UpdateFreeFlyTranspiler(IEnumerable<CodeInstruction> instructions)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		return new CodeMatcher(instructions, (ILGenerator)null).MatchStartForward((CodeMatch[])(object)new CodeMatch[1]
		{
			new CodeMatch((OpCode?)OpCodes.Ldfld, (object)AccessTools.Field(typeof(GameCamera), "m_freeFlyYaw"), (string)null)
		}).Advance(-2).InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[3]
		{
			new CodeInstruction(OpCodes.Ldloc_0, (object)null),
			new CodeInstruction(OpCodes.Call, (object)AccessTools.Method(typeof(FlyFeatures), "CheckInvert", (Type[])null, (Type[])null)),
			new CodeInstruction(OpCodes.Stloc_0, (object)null)
		})
			.InstructionEnumeration();
	}

	[HarmonyPatch(typeof(Player), "SetControls")]
	[HarmonyPrefix]
	private static void SetControlsPrefix(Player __instance, ref bool jump, ref bool crouch)
	{
		if (!((Object)(object)__instance != (Object)(object)Player.m_localPlayer))
		{
			jump = jump && !((Character)__instance).IsDebugFlying();
			crouch = crouch && !((Character)__instance).IsDebugFlying();
		}
	}

	[HarmonyPatch(typeof(Player), "LateUpdate")]
	[HarmonyPostfix]
	private static void LateUpdatePostfix(Player __instance)
	{
		if ((Object)(object)__instance != (Object)(object)Player.m_localPlayer)
		{
			return;
		}
		if (((Character)__instance).IsDebugFlying())
		{
			__instance.m_crouchToggled = false;
			((Character)__instance).m_zanim.SetBool(Character.s_onGround, true);
			((Character)__instance).m_zanim.SetBool(Character.s_inWater, false);
			((Character)__instance).m_zanim.SetBool(Character.s_encumbered, false);
			((Character)__instance).m_zanim.SetFloat(Character.s_sidewaySpeed, 0f);
			((Character)__instance).m_zanim.SetFloat(Character.s_forwardSpeed, 0f);
			((Character)__instance).m_zanim.SetFloat(Character.s_turnSpeed, 0f);
		}
		if (NoClipPlayerEnabled())
		{
			if (!NoClipTurnedOn)
			{
				if (Settings.NoClipClearEnvironment)
				{
					EnvMan.instance.SetForceEnvironment("");
				}
				Ship localShip = Ship.GetLocalShip();
				if (localShip != null)
				{
					localShip.OnTriggerExit((Collider)(object)((Character)__instance).m_collider);
				}
				IWaterInteractable component = ((Component)__instance).GetComponent<IWaterInteractable>();
				LiquidSurface[] array = Object.FindObjectsByType<LiquidSurface>((FindObjectsInactive)1, (FindObjectsSortMode)0);
				foreach (LiquidSurface val in array)
				{
					if (val.m_inWater != null && val.m_inWater.Contains(component))
					{
						val.OnTriggerExit((Collider)(object)((Character)__instance).m_collider);
					}
				}
				WaterVolume[] array2 = Object.FindObjectsByType<WaterVolume>((FindObjectsInactive)1, (FindObjectsSortMode)0);
				foreach (WaterVolume val2 in array2)
				{
					if (val2.m_inWater != null && val2.m_inWater.Contains(component))
					{
						val2.OnTriggerExit((Collider)(object)((Character)__instance).m_collider);
					}
				}
			}
			NoClipTurnedOn = true;
			((Collider)((Character)__instance).m_collider).enabled = false;
		}
		else if (NoClipTurnedOn)
		{
			NoClipTurnedOn = false;
			((Collider)((Character)__instance).m_collider).enabled = true;
		}
	}

	[HarmonyPatch(typeof(Character), "UpdateEyeRotation")]
	[HarmonyPrefix]
	private static void UpdateEyeRotationPrefix(Character __instance)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		__instance.m_lookDir = ((Vector3)(ref __instance.m_lookDir)).normalized;
	}

	[HarmonyPatch(typeof(Player), "IsDebugFlying")]
	[HarmonyPostfix]
	private static bool IsDebugFlyingPostfix(bool result, Player __instance)
	{
		if (!((Object)(object)__instance == (Object)(object)Player.m_localPlayer))
		{
			return result;
		}
		return Settings.IsEnabled(PermissionHash.Fly, result);
	}

	[HarmonyPatch(typeof(Player), "InDebugFlyMode")]
	[HarmonyPostfix]
	private static bool InDebugFlyModePostfix(bool result, Player __instance)
	{
		if (!((Object)(object)__instance == (Object)(object)Player.m_localPlayer))
		{
			return result;
		}
		return Settings.IsEnabled(PermissionHash.Fly, result);
	}

	[HarmonyPatch(typeof(Character), "UnderWorldCheck")]
	[HarmonyPrefix]
	private static bool UnderWorldCheckPrefix(Character __instance)
	{
		if (!((Object)(object)__instance != (Object)(object)Player.m_localPlayer))
		{
			return !NoClipPlayerEnabled();
		}
		return true;
	}

	[HarmonyPatch(typeof(GameCamera), "GetCameraPosition")]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> GetCameraPositionTranspiler(IEnumerable<CodeInstruction> instructions)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		return new CodeMatcher(instructions, (ILGenerator)null).MatchStartForward((CodeMatch[])(object)new CodeMatch[1]
		{
			new CodeMatch((OpCode?)OpCodes.Ldfld, (object)AccessTools.Field(typeof(GameCamera), "m_minWaterDistance"), (string)null)
		}).Repeat((Action<CodeMatcher>)delegate(CodeMatcher matcher)
		{
			matcher.SetAndAdvance(OpCodes.Call, Transpilers.EmitDelegate<Func<GameCamera, float>>((Func<GameCamera, float>)((GameCamera instance) => (!NoClipCameraEnabled()) ? instance.m_minWaterDistance : float.MinValue)).operand);
		}, (Action<string>)null).InstructionEnumeration();
	}

	[HarmonyPatch(typeof(GameCamera), "CollideRay2")]
	[HarmonyPrefix]
	private static bool CollideRay2Prefix()
	{
		return !NoClipCameraEnabled();
	}
}
