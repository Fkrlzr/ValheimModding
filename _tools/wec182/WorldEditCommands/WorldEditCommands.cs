using BepInEx;
using BepInEx.Bootstrap;
using Data;
using HarmonyLib;

namespace WorldEditCommands;

[BepInPlugin("world_edit_commands", "World Edit Commands", "1.82")]
[BepInDependency("server_devcommands", "1.110")]
public class WorldEditCommands : BaseUnityPlugin
{
	public const string GUID = "world_edit_commands";

	public const string NAME = "World Edit Commands";

	public const string VERSION = "1.82";

	public static bool IsSpawnerTweaks;

	public static bool IsStructureTweaks;

	public static bool IsCLLC;

	public static bool IsTweaks;

	public static bool IsEWFactions;

	public void Awake()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		new Harmony("world_edit_commands").PatchAll();
	}

	public void Start()
	{
		IsSpawnerTweaks = Chainloader.PluginInfos.ContainsKey("spawner_tweaks") || Chainloader.PluginInfos.ContainsKey("logic_tweaks");
		IsStructureTweaks = Chainloader.PluginInfos.ContainsKey("structure_tweaks") || Chainloader.PluginInfos.ContainsKey("logic_tweaks");
		IsCLLC = Chainloader.PluginInfos.ContainsKey("rg.bepinex.plugins.creaturelevelcontrol");
		IsTweaks = Chainloader.PluginInfos.ContainsKey("world_edit_tweaks");
		IsEWFactions = Chainloader.PluginInfos.ContainsKey("expand_world_factions");
		DataLoading.SetupWatcher();
	}
}
