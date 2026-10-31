using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;

namespace MobBattle;

[BepInPlugin(ModGUID, ModName, ModVersion)]
[BepInDependency(Jotunn.Main.ModGuid)]
// Optional: makes sure Creature Level & Loot Control is loaded first so its API can be used.
[BepInDependency(ClllcCompat.Guid, BepInDependency.DependencyFlags.SoftDependency)]
public class MobBattle : BaseUnityPlugin
{
	private const string ModName = "Mob Battle";
	private const string ModVersion = "1.0.0";
	private const string ModGUID = "org.bepinex.plugins.mobbattle";

	// Prefab name of the item that opens the battle menu.
	internal const string ItemName = "MobBattleWand";

	internal static ConfigEntry<string> baseItem = null!;
	internal static ConfigEntry<bool> disableDrops = null!;
	internal static ConfigEntry<int> defaultCount = null!;
	internal static ConfigEntry<int> maxCount = null!;
	internal static ConfigEntry<int> defaultStars = null!;
	internal static ConfigEntry<int> maxStars = null!;
	internal static ConfigEntry<float> spawnDistance = null!;
	internal static ConfigEntry<bool> keepClllcEffects = null!;
	internal static ConfigEntry<KeyboardShortcut> menuKey = null!;

	public void Awake()
	{
		baseItem = Config.Bind("1 - Item", "Base item", "Hammer",
			new ConfigDescription("Vanilla item the Mob Battle Wand borrows its model and icon from. Takes effect after a restart.",
				new AcceptableValueList<string>("Hammer", "Hoe", "Cultivator")));
		menuKey = Config.Bind("1 - Item", "Menu key", new KeyboardShortcut(KeyCode.None), "Optional key that opens the menu without holding the wand. Unset by default. The console command 'mobbattle' also opens it.");

		disableDrops = Config.Bind("2 - Battle", "Disable drops", true, "Mobs spawned for a battle drop nothing when they die.");
		defaultCount = Config.Bind("2 - Battle", "Default mobs per side", 5, new ConfigDescription("Starting value of the mobs per side slider.", new AcceptableValueRange<int>(1, 100)));
		maxCount = Config.Bind("2 - Battle", "Max mobs per side", 30, new ConfigDescription("Upper end of the mobs per side slider. Very high numbers can lag.", new AcceptableValueRange<int>(1, 100)));
		defaultStars = Config.Bind("2 - Battle", "Default stars", 0, new ConfigDescription("Starting value of the stars slider (0 stars is level 1).", new AcceptableValueRange<int>(0, 20)));
		maxStars = Config.Bind("2 - Battle", "Max stars", 10, new ConfigDescription("Upper end of the stars slider. Vanilla creatures stop at 2 stars.", new AcceptableValueRange<int>(0, 20)));
		spawnDistance = Config.Bind("2 - Battle", "Spawn distance", 25f, new ConfigDescription("How far in front of you the battle takes place, in meters.", new AcceptableValueRange<float>(10f, 60f)));

		keepClllcEffects = Config.Bind("3 - Creature Level & Loot Control", "Keep CLLC effects", false,
			"Only matters when Creature Level & Loot Control is installed. Off: battle creatures get no colored star effect and no elemental infusion, so the fight is decided by the creatures you picked. On: CLLC's random effects are kept. The Splitting effect and boss affixes are always removed because their extra creatures would attack you and fall outside the battle.");

		Harmony harmony = new(ModGUID);
		harmony.PatchAll(Assembly.GetExecutingAssembly());

		gameObject.AddComponent<BattleUi>();

		PrefabManager.OnVanillaPrefabsAvailable += CreateItem;
		CommandManager.Instance.AddConsoleCommand(new OpenMenuCommand());
	}

	private static void CreateItem()
	{
		PrefabManager.OnVanillaPrefabsAvailable -= CreateItem;

		ItemConfig config = new()
		{
			Name = "Mob Battle Wand",
			Description = "Pick two sides of creatures and watch them fight.\nLeft click: open the battle menu.\nRight click: start the last battle, or stop the running one.",
			CraftingStation = CraftingStations.Workbench,
			Amount = 1,
		};
		config.AddRequirement(new RequirementConfig("Wood", 10, 0, true));
		config.AddRequirement(new RequirementConfig("Stone", 5, 0, true));

		CustomItem item = new(ItemName, baseItem.Value, config);
		ItemDrop.ItemData.SharedData shared = item.ItemDrop.m_itemData.m_shared;
		// A tool normally switches the player into build mode. Without a piece table it behaves like a plain held item,
		// which lets the attack buttons open the menu instead.
		shared.m_buildPieces = null;
		shared.m_useDurability = false;
		ItemManager.Instance.AddItem(item);
	}

	public void Update()
	{
		if (menuKey.Value.IsDown() && Player.m_localPlayer is not null)
		{
			BattleUi.Open();
		}
	}

	private class OpenMenuCommand : ConsoleCommand
	{
		public override string Name => "mobbattle";

		public override string Help => "Open the Mob Battle menu.";

		public override void Run(string[] args)
		{
			BattleUi.Open();
		}
	}
}
