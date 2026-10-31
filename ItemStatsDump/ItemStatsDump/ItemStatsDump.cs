using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;

namespace ItemStatsDump;

[BepInPlugin(ModGUID, ModName, ModVersion)]
public class ItemStatsDump : BaseUnityPlugin
{
	private const string ModName = "Item Stats Dump";
	private const string ModVersion = "1.0.0";
	private const string ModGUID = "org.bepinex.plugins.itemstatsdump";

	private static ConfigEntry<string> outputPath = null!;
	private static ConfigEntry<bool> dumpOnSpawn = null!;
	private static bool dumpedThisSession;

	public void Awake()
	{
		outputPath = Config.Bind("1 - General", "Output file", @"E:\Documents\Sekolah\ValheimModding\ItemStats\items.csv", "CSV file the item stats are written to.");
		dumpOnSpawn = Config.Bind("1 - General", "Dump on spawn", true, "Write the CSV once per game session, the first time the player spawns in a world.");

		Harmony harmony = new(ModGUID);
		harmony.PatchAll(Assembly.GetExecutingAssembly());
	}

	private static readonly string[] Header =
	{
		"Prefab", "Name", "Type", "Skill", "MaxQuality",
		"Blunt", "Slash", "Pierce", "Chop", "Pickaxe", "Fire", "Frost", "Lightning", "Poison", "Spirit", "Damage",
		"BluntPL", "SlashPL", "PiercePL", "ChopPL", "PickaxePL", "FirePL", "FrostPL", "LightningPL", "PoisonPL", "SpiritPL", "DamagePL",
		"Armor", "ArmorPL", "BlockPower", "BlockPowerPL", "Deflection", "DeflectionPL", "TimedBlockBonus",
		"Durability", "DurabilityPL", "UseDurability", "Weight", "WeightScaleByQuality", "Knockback", "Backstab", "MovementModifier", "Resistances"
	};

	private static string F(float v) => v.ToString("0.######", CultureInfo.InvariantCulture);

	private static string Csv(string s)
	{
		s ??= "";
		return s.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0 ? "\"" + s.Replace("\"", "\"\"") + "\"" : s;
	}

	private static bool IsRelevant(ItemDrop.ItemData.SharedData s)
	{
		switch (s.m_itemType)
		{
			case ItemDrop.ItemData.ItemType.OneHandedWeapon:
			case ItemDrop.ItemData.ItemType.TwoHandedWeapon:
			case ItemDrop.ItemData.ItemType.TwoHandedWeaponLeft:
			case ItemDrop.ItemData.ItemType.Bow:
			case ItemDrop.ItemData.ItemType.Shield:
			case ItemDrop.ItemData.ItemType.Helmet:
			case ItemDrop.ItemData.ItemType.Chest:
			case ItemDrop.ItemData.ItemType.Legs:
			case ItemDrop.ItemData.ItemType.Shoulder:
			case ItemDrop.ItemData.ItemType.Tool:
			case ItemDrop.ItemData.ItemType.Torch:
				return true;
			default:
				return s.m_maxQuality > 1;
		}
	}

	private static void Dump()
	{
		if (ObjectDB.instance == null)
		{
			return;
		}

		StringBuilder sb = new();
		sb.AppendLine(string.Join(",", Header));
		int count = 0;
		foreach (UnityEngine.GameObject go in ObjectDB.instance.m_items)
		{
			if (go == null || !go.TryGetComponent(out ItemDrop drop))
			{
				continue;
			}

			ItemDrop.ItemData.SharedData s = drop.m_itemData.m_shared;
			if (!IsRelevant(s))
			{
				continue;
			}

			HitData.DamageTypes d = s.m_damages;
			HitData.DamageTypes p = s.m_damagesPerLevel;
			List<string> resist = new();
			foreach (HitData.DamageModPair m in s.m_damageModifiers)
			{
				resist.Add($"{m.m_type}:{m.m_modifier}");
			}

			string[] row =
			{
				go.name,
				Localization.instance.Localize(s.m_name),
				s.m_itemType.ToString(),
				s.m_skillType.ToString(),
				s.m_maxQuality.ToString(CultureInfo.InvariantCulture),
				F(d.m_blunt), F(d.m_slash), F(d.m_pierce), F(d.m_chop), F(d.m_pickaxe), F(d.m_fire), F(d.m_frost), F(d.m_lightning), F(d.m_poison), F(d.m_spirit), F(d.m_damage),
				F(p.m_blunt), F(p.m_slash), F(p.m_pierce), F(p.m_chop), F(p.m_pickaxe), F(p.m_fire), F(p.m_frost), F(p.m_lightning), F(p.m_poison), F(p.m_spirit), F(p.m_damage),
				F(s.m_armor), F(s.m_armorPerLevel), F(s.m_blockPower), F(s.m_blockPowerPerLevel), F(s.m_deflectionForce), F(s.m_deflectionForcePerLevel), F(s.m_timedBlockBonus),
				F(s.m_maxDurability), F(s.m_durabilityPerLevel), s.m_useDurability ? "1" : "0", F(s.m_weight), F(s.m_scaleWeightByQuality), F(s.m_attackForce), F(s.m_backstabBonus), F(s.m_movementModifier),
				string.Join(" ", resist)
			};
			sb.AppendLine(string.Join(",", Array.ConvertAll(row, Csv)));
			++count;
		}

		string path = outputPath.Value;
		Directory.CreateDirectory(Path.GetDirectoryName(path)!);
		File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
		UnityEngine.Debug.Log($"[{ModName}] Wrote {count} items to {path}");
	}

	[HarmonyPatch(typeof(Player), nameof(Player.OnSpawned))]
	private static class DumpWhenSpawned
	{
		[UsedImplicitly]
		private static void Postfix()
		{
			if (dumpOnSpawn.Value && !dumpedThisSession)
			{
				dumpedThisSession = true;
				try
				{
					Dump();
				}
				catch (Exception e)
				{
					UnityEngine.Debug.LogError($"[{ModName}] Dump failed: {e}");
				}
			}
		}
	}
}
