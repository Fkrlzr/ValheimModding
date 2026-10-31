using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Terminal), "InitTerminal")]
[HarmonyPriority(500)]
public class InitializeTerminal
{
	private static bool Initialized;

	private static void Postfix()
	{
		if (!Initialized)
		{
			Initialized = true;
			new DevcommandsCommand();
			new ConfigCommand();
			new StartEventCommand();
			new PosCommand();
			new AliasCommand();
			new SearchIdCommand();
			new UndoRedoCommand();
			new ResolutionCommand();
			new WaitCommand();
			new ServerCommand();
			new HUDCommand();
			new NoMapCommand();
			new BindCommand();
			new BroadcastCommand();
			new SeedCommand();
			new MoveSpawn();
			new MappingCommand();
			new WindCommand();
			new EnvCommand();
			new GotoCommand();
			new InventoryCommand();
			new CalmCommand();
			new RepairCommand();
			new AddStatusCommand();
			new PlayerListCommand();
			new FindCommand();
			new PullCommand();
			new ResetDungeonCommand();
			new TeleportCommand();
			new SearchComponentCommand();
			new SearchItemCommand();
			new RPCCommand();
			new DmgCommand();
			new KillCommand();
			new PermissionsCommand();
			new ShutdownCommand();
			DefaultAutoComplete.Register();
			AliasManager.Init();
			BindManager.Init();
		}
	}
}
