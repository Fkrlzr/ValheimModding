using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MobBattle;

public enum PickMode
{
	Faction,
	Family,
	Mob,
}

// One spawnable creature.
public sealed class MobEntry
{
	public string Prefab = "";
	public string Display = "";
	public Character.Faction Faction;
	public string Family = "";
	public bool Boss;
}

// One row in the menu: a faction, a family of mobs (Greydwarf, Troll ...) or a single mob.
public sealed class PickEntry
{
	public PickMode Mode;
	public string Key = "";
	public string Display = "";
	public List<MobEntry> Members = new();

	public string Label => $"{Display}   ({Members.Count})";
}

public static class BattleCatalog
{
	private static List<MobEntry>? mobs;
	private static int builtFor;
	private static readonly Dictionary<PickMode, List<PickEntry>> entries = new();

	public static IReadOnlyList<MobEntry> Mobs
	{
		get
		{
			Build();
			return mobs!;
		}
	}

	public static bool Available => ZNetScene.instance != null;

	public static IReadOnlyList<PickEntry> Entries(PickMode mode)
	{
		Build();
		return entries[mode];
	}

	// Splits a prefab name into the creature family: "Greydwarf_Elite" -> "Greydwarf", "Skeleton_Poison" -> "Skeleton".
	// Very short results ("gd_king" -> "gd") keep the full prefab name so they stay readable.
	public static string FamilyOf(string prefab)
	{
		string family = prefab;
		int underscore = family.IndexOf('_');
		if (underscore > 0)
		{
			family = family.Substring(0, underscore);
		}
		family = family.TrimEnd('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
		return family.Length < 4 ? prefab : family;
	}

	private static void Build()
	{
		int id = ZNetScene.instance != null ? ZNetScene.instance.GetInstanceID() : 0;
		if (mobs != null && builtFor == id)
		{
			return;
		}

		mobs = new List<MobEntry>();
		builtFor = id;
		entries[PickMode.Faction] = new List<PickEntry>();
		entries[PickMode.Family] = new List<PickEntry>();
		entries[PickMode.Mob] = new List<PickEntry>();
		if (ZNetScene.instance == null)
		{
			return;
		}

		foreach (GameObject prefab in ZNetScene.instance.m_prefabs)
		{
			if (prefab == null)
			{
				continue;
			}

			Character? character = prefab.GetComponent<Character>();
			if (character == null || character is Player || prefab.GetComponent<MonsterAI>() == null)
			{
				continue;
			}

			Character.Faction faction = character.m_faction;
			if (faction == Character.Faction.Players || faction == Character.Faction.PlayerSpawned || faction == Character.Faction.TrainingDummy)
			{
				continue;
			}

			string display = prefab.name;
			if (!string.IsNullOrEmpty(character.m_name) && Localization.instance != null)
			{
				string localized = Localization.instance.Localize(character.m_name);
				if (!string.IsNullOrEmpty(localized))
				{
					display = localized;
				}
			}

			mobs.Add(new MobEntry
			{
				Prefab = prefab.name,
				Display = display,
				Faction = faction,
				Family = FamilyOf(prefab.name),
				Boss = character.m_boss,
			});
		}

		mobs.Sort((a, b) => string.Compare(a.Display, b.Display, StringComparison.OrdinalIgnoreCase));

		foreach (IGrouping<Character.Faction, MobEntry> group in mobs.GroupBy(m => m.Faction).OrderBy(g => g.Key.ToString()))
		{
			entries[PickMode.Faction].Add(new PickEntry { Mode = PickMode.Faction, Key = group.Key.ToString(), Display = group.Key.ToString(), Members = group.ToList() });
		}

		foreach (IGrouping<string, MobEntry> group in mobs.GroupBy(m => m.Family).OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase))
		{
			MobEntry? exact = group.FirstOrDefault(m => m.Prefab == group.Key);
			entries[PickMode.Family].Add(new PickEntry { Mode = PickMode.Family, Key = group.Key, Display = exact?.Display ?? group.Key, Members = group.ToList() });
		}

		foreach (MobEntry mob in mobs)
		{
			entries[PickMode.Mob].Add(new PickEntry { Mode = PickMode.Mob, Key = mob.Prefab, Display = $"{mob.Display} [{mob.Prefab}]", Members = new List<MobEntry> { mob } });
		}
	}
}
