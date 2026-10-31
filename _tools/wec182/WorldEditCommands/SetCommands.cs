using HarmonyLib;

namespace WorldEditCommands;

[HarmonyPatch(typeof(Terminal), "InitTerminal")]
public class SetCommands
{
	private static void Postfix()
	{
		new SpawnLocationCommand();
		new SpawnObjectCommand();
		new ObjectCommand();
		new TerrainCommand();
		new AliasesCommand();
		new DungeonCommand();
		new DataCommand();
		new DataRawCommand();
		if (WorldEditCommands.IsSpawnerTweaks || WorldEditCommands.IsStructureTweaks)
		{
			new TweakObjectCommand();
		}
		if (WorldEditCommands.IsSpawnerTweaks)
		{
			new TweakAltarCommand();
			new TweakBeehiveCommand();
			new TweakPickableCommand();
			new TweakSpawnerCommand();
			new TweakSpawnPointCommand();
			new TweakItemStandCommand();
			new TweakChestCommand();
			new TweakSmelterCommand();
			new TweakFermenterCommand();
		}
		if (WorldEditCommands.IsSpawnerTweaks || WorldEditCommands.IsEWFactions)
		{
			new TweakCreatureCommand();
		}
		if (WorldEditCommands.IsStructureTweaks)
		{
			new TweakDungeonCommand();
			new TweakRunestoneCommand();
			new TweakPortalCommand();
			new TweakFireplaceCommand();
			new TweakChestCommand();
			new TweakDoorCommand();
		}
	}
}
