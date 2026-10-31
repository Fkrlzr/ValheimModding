using System;
using System.Linq.Expressions;
using System.Reflection;
using HarmonyLib;
using Service;

namespace ServerDevcommands;

public class GizmoReloadedPatcher
{
	public static void DoPatching(Assembly assembly)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		if (!(assembly == null))
		{
			Log.Info("\"GizmoReloaded\" detected. Patching \"HandleAxisInput\" for mouse wheel binding.");
			Harmony val = new Harmony("valheim.jerekuusela.server_devcommand.m3to.mods.GizmoReloaded");
			MethodInfo methodInfo = AccessTools.Method(assembly.GetType("GizmoReloaded.Plugin"), "HandleAxisInput", (Type[])null, (Type[])null);
			MethodInfo methodInfo2 = SymbolExtensions.GetMethodInfo((Expression<Action>)(() => Prefix()));
			val.Patch((MethodBase)methodInfo, new HarmonyMethod(methodInfo2), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
		}
	}

	private static bool Prefix()
	{
		return !MouseWheelBinding.CouldExecute();
	}
}
