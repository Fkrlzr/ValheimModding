using System;
using System.Collections;
using System.Reflection;
using HarmonyLib;
using SkToolbox.Utility;
using UnityEngine;

namespace SkToolbox;

// Writes what the game and Server Devcommands think about flying to LogOutput.log, so a fly that does nothing on
// someone else's server can be explained from the log. Read-only: it changes no state. Lines start with "FLY".
internal static class SkFlyDiag
{
	private static readonly Type managerType = AccessTools.TypeByName("ServerDevcommands.PermissionManager");

	private static void Log(string text)
	{
		SkUtilities.Logz(new string[1] { "FLY" }, new string[1] { text }, (LogType)3);
	}

	// Call right after a fly toggle. Also re-checks after 1 and 4 seconds to catch anything that undoes it.
	internal static void Toggled()
	{
		Player player = Player.m_localPlayer;
		if (player == null)
		{
			return;
		}
		try
		{
			Report("toggle", player);
			((MonoBehaviour)player).StartCoroutine(Recheck(player));
		}
		catch (Exception ex)
		{
			Log("diagnostic failed: " + ex.Message);
		}
	}

	private static IEnumerator Recheck(Player player)
	{
		yield return new WaitForSeconds(1f);
		if (player != null)
		{
			Report("after 1s", player);
		}
		yield return new WaitForSeconds(3f);
		if (player != null)
		{
			Report("after 4s", player);
		}
	}

	private static void Report(string when, Player player)
	{
		bool zdoFly = false;
		bool owner = false;
		try
		{
			ZNetView view = ((Component)player).GetComponent<ZNetView>();
			if (view != null && view.IsValid())
			{
				owner = view.IsOwner();
				zdoFly = view.GetZDO().GetBool(ZDOVars.s_debugFly, false);
			}
		}
		catch (Exception)
		{
		}
		string net = "no ZNet";
		if (ZNet.instance != null)
		{
			net = (ZNet.instance.IsServer() ? "host/server" : "client") + (ZNet.instance.IsDedicated() ? ", dedicated" : "") + (ZNet.instance.LocalPlayerIsAdminOrHost() ? ", admin-or-host" : ", NOT admin");
		}
		Log(when + ": toolbox flyEnabled=" + SkCommandProcessor.flyEnabled + ", m_debugFly=" + player.m_debugFly + ", zdo debugFly=" + zdoFly + ", owner=" + owner + ", IsDebugFlying()=" + ((Character)player).IsDebugFlying() + ", InDebugFlyMode()=" + player.InDebugFlyMode() + ", devcommands (Terminal.m_cheat)=" + Terminal.m_cheat + ", " + net + ", " + Sdc());
	}

	private static string Sdc()
	{
		if (managerType == null)
		{
			return "Server Devcommands: not installed";
		}
		try
		{
			object instance = AccessTools.Property(managerType, "Instance").GetValue(null, null);
			object admin = AccessTools.Property(managerType, "IsAdmin").GetValue(instance, null);
			object canCheat = AccessTools.Property(managerType, "CanCheat").GetValue(instance, null);
			MethodInfo permission = AccessTools.Method(managerType, "GetFeaturePermissionByHash", new Type[2]
			{
				typeof(string),
				typeof(int)
			}, (Type[])null);
			object fly = permission.Invoke(instance, new object[2] { "serverdevcommands", "fly".GetStableHashCode() });
			object wildcard = permission.Invoke(instance, new object[2] { "serverdevcommands", "*".GetStableHashCode() });
			return "SDC: isAdmin=" + admin + ", canCheat=" + canCheat + ", server fly permission=" + fly + ", server * permission=" + wildcard + " (Unknown=no rule, Yes, No, Force)";
		}
		catch (Exception ex)
		{
			return "SDC: could not read (" + ex.Message + ")";
		}
	}
}
