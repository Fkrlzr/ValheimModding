using System;
using System.Linq;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Minimap))]
public class MinimapTeleport
{
	private static bool TryTeleport(Minimap map, KeyCode mainKey)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		if (!Object.op_Implicit((Object)(object)Console.instance) || !((Terminal)Console.instance).IsCheatsEnabled())
		{
			return false;
		}
		KeyboardShortcut mapTeleport = Settings.MapTeleport;
		if (mainKey == ((KeyboardShortcut)(ref mapTeleport)).MainKey)
		{
			mapTeleport = Settings.MapTeleport;
			if (((KeyboardShortcut)(ref mapTeleport)).Modifiers.All((KeyCode k) => ZInput.GetKey(k, true)))
			{
				Vector3 val = map.ScreenToWorldPoint(ZInput.pointerPosition);
				val.y = ((Component)Player.m_localPlayer).transform.position.y;
				float val2 = default(float);
				Heightmap.GetHeight(val, ref val2);
				val.y = Math.Max(0f, val2);
				ZRoutedRpc.instance.InvokeRoutedRPC(0L, ((Character)Player.m_localPlayer).GetZDOID(), "RPC_TeleportTo", new object[3]
				{
					val,
					((Component)Player.m_localPlayer).transform.rotation,
					true
				});
				return true;
			}
		}
		return false;
	}

	[HarmonyPatch("OnMapLeftClick")]
	[HarmonyPrefix]
	private static bool OnMapLeftClick(Minimap __instance)
	{
		return !TryTeleport(__instance, (KeyCode)323);
	}

	[HarmonyPatch("RemovePinUnderPointer")]
	[HarmonyPrefix]
	private static bool RemovePinUnderPointer(Minimap __instance)
	{
		return !TryTeleport(__instance, (KeyCode)324);
	}

	[HarmonyPatch("OnMapMiddleClick")]
	[HarmonyPrefix]
	private static bool OnMapMiddleClick(Minimap __instance)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (TryTeleport(__instance, (KeyCode)325))
		{
			return false;
		}
		Vector3 val = __instance.ScreenToWorldPoint(ZInput.pointerPosition);
		Chat.instance.SendPing(val);
		return false;
	}
}
