using System;
using System.Reflection;
using BepInEx.Bootstrap;
using HarmonyLib;

namespace EWP;

public static class Api
{
	public const string GUID = "expand_world_prefabs";

	private static bool isSetup;

	private static MethodInfo? registerGroupHandlerMethod;

	private static void SetupIfNeeded()
	{
		if (!isSetup)
		{
			isSetup = true;
			if (Chainloader.PluginInfos.TryGetValue("expand_world_prefabs", out var value))
			{
				Setup(((object)value.Instance).GetType().Assembly);
			}
		}
	}

	private static void Setup(Assembly assembly)
	{
		if (!(assembly == null))
		{
			Type type = assembly.GetType("ExpandWorld.Prefab.Api");
			if (!(type == null))
			{
				registerGroupHandlerMethod = AccessTools.Method(type, "RegisterGroupHandler", new Type[2]
				{
					typeof(string),
					typeof(Func<string, long, string, bool>)
				}, (Type[])null);
			}
		}
	}

	public static void RegisterGroupHandler(string key, Func<string, long, string, bool> handler)
	{
		SetupIfNeeded();
		registerGroupHandlerMethod?.Invoke(null, new object[2] { key, handler });
	}
}
