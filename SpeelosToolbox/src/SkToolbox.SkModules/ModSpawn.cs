using System;
using System.Collections.Generic;
using SkToolbox.Utility;
using UnityEngine;

namespace SkToolbox.SkModules;

internal class ModSpawn : SkBaseModule, IModule
{
	private const int MaxCount = 20;

	private const int MaxLevel = 3;

	private int count = 1;

	private int level = 1;

	private bool tamed;

	private List<SkMenuController.SkGridItem> cache;

	private int cachedFor;

	private static readonly Dictionary<string, string> TrophyAliases = new Dictionary<string, string>
	{
		{ "Troll", "TrophyForestTroll" },
		{ "gd_king", "TrophyTheElder" },
		{ "Dragon", "TrophyDragonQueen" },
		{ "StoneGolem", "TrophySGolem" },
		{ "Greyling", "TrophyGreydwarf" },
		{ "Greydwarf_Elite", "TrophyGreydwarfBrute" },
		{ "Greydwarf_Shaman", "TrophyGreydwarfShaman" },
		{ "Draugr_Elite", "TrophyDraugrElite" },
		{ "BlobTar", "TrophyBlobTar" },
		{ "Hen", "TrophyHen" },
		{ "Chicken", "TrophyHen" }
	};

	public ModSpawn()
	{
		ModuleName = "Spawn";
		Loading();
	}

	public void Start()
	{
		BeginMenu();
		Ready();
	}

	public void BeginMenu()
	{
		base.MenuOptions = new SkMenu();
	}

	internal void ShowGrid()
	{
		int num = (((Object)(object)ZNetScene.instance != (Object)null) ? ((Object)ZNetScene.instance).GetInstanceID() : 0);
		if (cache == null || cache.Count == 0 || cachedFor != num)
		{
			cache = BuildCreatures();
			cachedFor = num;
		}
		if (cache.Count == 0)
		{
			SkCommandProcessor.Notify("Creature list is not available yet. Load into a world first.");
			return;
		}
		List<SkMenuController.SkMenuSlider> sliders = new List<SkMenuController.SkMenuSlider>
		{
			new SkMenuController.SkMenuSlider
			{
				Label = "How many",
				Min = 1,
				Max = 20,
				Get = () => count,
				Set = delegate(int value)
				{
					count = value;
				}
			},
			new SkMenuController.SkMenuSlider
			{
				Label = "Level",
				Min = 1,
				Max = 3,
				Get = () => level,
				Set = delegate(int value)
				{
					level = value;
				}
			}
		};
		List<SkMenuController.SkMenuToggle> toggles = new List<SkMenuController.SkMenuToggle>
		{
			new SkMenuController.SkMenuToggle
			{
				Label = "Spawn tamed",
				Get = () => tamed,
				Set = delegate(bool value)
				{
					tamed = value;
				}
			}
		};
		SkMC.RequestGridMenu(cache, sliders, "Spawn", showFilter: true, toggles);
	}

	private List<SkMenuController.SkGridItem> BuildCreatures()
	{
		List<SkMenuController.SkGridItem> list = new List<SkMenuController.SkGridItem>();
		if ((Object)(object)ZNetScene.instance == (Object)null || ZNetScene.instance.m_prefabs == null)
		{
			return list;
		}
		foreach (GameObject prefab in ZNetScene.instance.m_prefabs)
		{
			if ((Object)(object)prefab == (Object)null || string.IsNullOrEmpty(((Object)prefab).name))
			{
				continue;
			}
			Character component = prefab.GetComponent<Character>();
			if ((Object)(object)component == (Object)null)
			{
				continue;
			}
			string text = ((Object)prefab).name;
			try
			{
				if (Localization.instance != null && !string.IsNullOrEmpty(component.m_name))
				{
					string text2 = Localization.instance.Localize(component.m_name);
					if (!string.IsNullOrEmpty(text2) && !text2.StartsWith("["))
					{
						text = text2;
					}
				}
			}
			catch (Exception)
			{
			}
			list.Add(new SkMenuController.SkGridItem
			{
				Name = ((Object)prefab).name,
				Display = text,
				Tip = text + "   (" + ((Object)prefab).name + ")" + (component.m_boss ? "   [boss]" : ""),
				Icon = TrophyIcon(((Object)prefab).name),
				Scope = SkMenuController.SkScope.Server,
				OnClick = Spawn
			});
		}
		list.Sort((SkMenuController.SkGridItem a, SkMenuController.SkGridItem b) => string.Compare(a.Display, b.Display, StringComparison.OrdinalIgnoreCase));
		return list;
	}

	private static Sprite TrophyIcon(string prefabName)
	{
		if (TrophyAliases.TryGetValue(prefabName, out var value))
		{
			Sprite val = SkIcons.Item(value);
			if ((Object)(object)val != (Object)null)
			{
				return val;
			}
		}
		return SkIcons.Item("Trophy" + prefabName);
	}

	private void Spawn(string prefabName)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Player.m_localPlayer;
		if ((Object)(object)localPlayer == (Object)null)
		{
			SkCommandProcessor.Notify("No player yet.");
			return;
		}
		GameObject val = (((Object)(object)ZNetScene.instance != (Object)null) ? ZNetScene.instance.GetPrefab(prefabName) : null);
		if ((Object)(object)val == (Object)null)
		{
			SkCommandProcessor.Notify("Creature not found: " + prefabName);
			return;
		}
		int num = Mathf.Clamp(count, 1, 20);
		int num2 = 0;
		Vector3 val2 = ((Component)localPlayer).transform.position + ((Component)localPlayer).transform.forward * 2.5f + Vector3.up;
		for (int i = 0; i < num; i++)
		{
			try
			{
				float num3 = ((num <= 1) ? 0f : (360f / (float)num * (float)i));
				float num4 = ((num <= 1) ? 0f : Mathf.Clamp((float)num * 0.25f, 1f, 4f));
				Vector3 val3 = Quaternion.Euler(0f, num3, 0f) * Vector3.forward * num4;
				GameObject val4 = Object.Instantiate<GameObject>(val, val2 + val3, Quaternion.identity);
				if ((Object)(object)val4 == (Object)null)
				{
					continue;
				}
				num2++;
				Character component = val4.GetComponent<Character>();
				if ((Object)(object)component != (Object)null && level > 1)
				{
					component.SetLevel(Mathf.Clamp(level, 1, 3));
				}
				if (tamed)
				{
					MonsterAI component2 = val4.GetComponent<MonsterAI>();
					if ((Object)(object)component2 != (Object)null)
					{
						component2.MakeTame();
					}
					else if ((Object)(object)component != (Object)null)
					{
						component.SetTamed(true);
					}
				}
			}
			catch (Exception ex)
			{
				SkUtilities.Logz(new string[2] { "SPAWN", "ERROR" }, new string[1] { ex.Message }, (LogType)0);
			}
		}
		SkCommandProcessor.Notify((num2 > 0) ? ("Spawned " + num2 + "x " + prefabName + ((level > 1) ? (" (level " + level + ")") : "") + (tamed ? ", tamed" : "")) : ("Could not spawn " + prefabName));
	}
}
