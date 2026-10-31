using UnityEngine;

namespace InfinityHammer;

public class CustomHealth
{
	public static float SetHealth(ZNetView obj, bool isRepair)
	{
		ZDO zDO = obj.GetZDO();
		if (Configuration.Invulnerability == "Off" && Configuration.OverwriteHealth == 0f)
		{
			return SetDefaultHealth(obj, zDO, isRepair);
		}
		if (Configuration.Invulnerability == "Off" || Configuration.Invulnerability == "Legacy")
		{
			return SetCustomHealth(obj, zDO);
		}
		return SetInfiniteHealth(obj, zDO);
	}

	private static float SetDefaultHealth(ZNetView obj, ZDO zdo, bool isRepair)
	{
		if (!isRepair)
		{
			return 0f;
		}
		float num = 0f;
		Character character = default(Character);
		if (((Component)obj).TryGetComponent<Character>(ref character))
		{
			num += SetDefaultHealth(zdo, character);
		}
		Destructible destructible = default(Destructible);
		if (((Component)obj).TryGetComponent<Destructible>(ref destructible))
		{
			num += SetDefaultHealth(zdo, destructible);
		}
		TreeBase treeBase = default(TreeBase);
		if (((Component)obj).TryGetComponent<TreeBase>(ref treeBase))
		{
			num += SetDefaultHealth(zdo, treeBase);
		}
		TreeLog treeLog = default(TreeLog);
		if (((Component)obj).TryGetComponent<TreeLog>(ref treeLog))
		{
			num += SetDefaultHealth(zdo, treeLog);
		}
		MineRock mineRock = default(MineRock);
		if (((Component)obj).TryGetComponent<MineRock>(ref mineRock))
		{
			num += SetDefaultHealth(zdo, mineRock);
		}
		MineRock5 mineRock2 = default(MineRock5);
		if (((Component)obj).TryGetComponent<MineRock5>(ref mineRock2))
		{
			num += SetDefaultHealth(zdo, mineRock2);
		}
		WearNTear wear = default(WearNTear);
		if (((Component)obj).TryGetComponent<WearNTear>(ref wear))
		{
			num += SetDefaultHealth(zdo, wear);
		}
		return num;
	}

	private static float SetDefaultHealth(ZDO zdo, Destructible destructible)
	{
		float @float = zdo.GetFloat(ZDOVars.s_health, destructible.m_health);
		zdo.RemoveFloat(ZDOVars.s_health);
		if (RemoveToolTier(destructible, zdo))
		{
			return float.NegativeInfinity;
		}
		return destructible.m_health - @float;
	}

	private static float SetDefaultHealth(ZDO zdo, TreeBase treeBase)
	{
		float @float = zdo.GetFloat(ZDOVars.s_health, treeBase.m_health);
		zdo.RemoveFloat(ZDOVars.s_health);
		if (RemoveToolTier(treeBase, zdo))
		{
			return float.NegativeInfinity;
		}
		return treeBase.m_health - @float;
	}

	private static float SetDefaultHealth(ZDO zdo, TreeLog treeLog)
	{
		float @float = zdo.GetFloat(ZDOVars.s_health, treeLog.m_health);
		zdo.RemoveFloat(ZDOVars.s_health);
		if (RemoveToolTier(treeLog, zdo))
		{
			return float.NegativeInfinity;
		}
		return treeLog.m_health - @float;
	}

	private static float SetDefaultHealth(ZDO zdo, MineRock mineRock)
	{
		MineRock component = ZNetScene.instance.GetPrefab(zdo.GetPrefab()).GetComponent<MineRock>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return 0f;
		}
		float num = component.m_health - mineRock.m_health;
		mineRock.m_health = component.m_health;
		zdo.RemoveFloat(Hashes.HealthMineRock);
		float result = RemoveCurrentHealth(mineRock, zdo);
		if (RemoveToolTier(mineRock, zdo))
		{
			return float.NegativeInfinity;
		}
		if (num == 0f)
		{
			return result;
		}
		return num;
	}

	private static float SetDefaultHealth(ZDO zdo, MineRock5 mineRock)
	{
		float num = 0f;
		foreach (HitArea hitArea in mineRock.m_hitAreas)
		{
			if (!(hitArea.m_health <= 0f))
			{
				num += mineRock.m_health - hitArea.m_health;
				hitArea.m_health = mineRock.m_health;
			}
		}
		mineRock.SaveHealth();
		if (RemoveToolTier(mineRock, zdo))
		{
			return float.NegativeInfinity;
		}
		return num;
	}

	private static float SetDefaultHealth(ZDO zdo, WearNTear wear)
	{
		float @float = zdo.GetFloat(ZDOVars.s_health, wear.m_health);
		zdo.RemoveFloat(ZDOVars.s_health);
		float float2 = zdo.GetFloat(Hashes.MaxHealth, 0f);
		if (float2 != 0f)
		{
			wear.m_health = ZNetScene.instance.GetPrefab(zdo.GetPrefab()).GetComponent<WearNTear>().m_health;
			zdo.RemoveFloat(Hashes.MaxHealth);
		}
		UpdateVisual(zdo, wear);
		if (!(float2 < 0f))
		{
			return wear.m_health - @float;
		}
		return float.NegativeInfinity;
	}

	private static float SetDefaultHealth(ZDO zdo, Character character)
	{
		float @float = zdo.GetFloat(ZDOVars.s_health, zdo.GetFloat(ZDOVars.s_maxHealth, character.m_health));
		zdo.RemoveFloat(ZDOVars.s_health);
		character.SetupMaxHealth();
		return zdo.GetFloat(ZDOVars.s_maxHealth, 0f) - @float;
	}

	private static float SetCustomHealth(ZNetView obj, ZDO zdo)
	{
		float num = 0f;
		float num2 = ((Configuration.Invulnerability == "Legacy") ? 1E+30f : Configuration.OverwriteHealth);
		if (num2 <= 0f)
		{
			return 0f;
		}
		Character character = default(Character);
		if (((Component)obj).TryGetComponent<Character>(ref character))
		{
			num += SetCustomHealth(zdo, character, num2);
		}
		Destructible destructible = default(Destructible);
		if (((Component)obj).TryGetComponent<Destructible>(ref destructible))
		{
			num += SetCustomHealth(zdo, destructible, num2);
		}
		TreeBase treeBase = default(TreeBase);
		if (((Component)obj).TryGetComponent<TreeBase>(ref treeBase))
		{
			num += SetCustomHealth(zdo, treeBase, num2);
		}
		TreeLog treeLog = default(TreeLog);
		if (((Component)obj).TryGetComponent<TreeLog>(ref treeLog))
		{
			num += SetCustomHealth(zdo, treeLog, num2);
		}
		MineRock mineRock = default(MineRock);
		if (((Component)obj).TryGetComponent<MineRock>(ref mineRock))
		{
			num += SetCustomHealth(zdo, mineRock, num2);
		}
		MineRock5 mineRock2 = default(MineRock5);
		if (((Component)obj).TryGetComponent<MineRock5>(ref mineRock2))
		{
			num += SetCustomHealth(zdo, mineRock2, num2);
		}
		WearNTear wear = default(WearNTear);
		if (((Component)obj).TryGetComponent<WearNTear>(ref wear))
		{
			num += SetCustomHealth(zdo, wear, num2);
		}
		return num;
	}

	private static float SetCustomHealth(ZDO zdo, Destructible destructible, float health)
	{
		float @float = zdo.GetFloat(ZDOVars.s_health, destructible.m_health);
		zdo.Set(ZDOVars.s_health, health);
		if (RemoveToolTier(destructible, zdo))
		{
			return float.NegativeInfinity;
		}
		return health - @float;
	}

	private static float SetCustomHealth(ZDO zdo, TreeBase treeBase, float health)
	{
		float @float = zdo.GetFloat(ZDOVars.s_health, treeBase.m_health);
		zdo.Set(ZDOVars.s_health, health);
		if (RemoveToolTier(treeBase, zdo))
		{
			return float.NegativeInfinity;
		}
		return health - @float;
	}

	private static float SetCustomHealth(ZDO zdo, TreeLog treeLog, float health)
	{
		float @float = zdo.GetFloat(ZDOVars.s_health, treeLog.m_health);
		zdo.Set(ZDOVars.s_health, health);
		if (RemoveToolTier(treeLog, zdo))
		{
			return float.NegativeInfinity;
		}
		return health - @float;
	}

	private static float SetCustomHealth(ZDO zdo, MineRock mineRock, float health)
	{
		zdo.Set(Hashes.HasFields, true);
		zdo.Set(Hashes.HashFieldsMineRock, true);
		zdo.Set(Hashes.HealthMineRock, health);
		float num = health - mineRock.m_health;
		mineRock.m_health = health;
		float result = RemoveCurrentHealth(mineRock, zdo);
		if (RemoveToolTier(mineRock, zdo))
		{
			return float.NegativeInfinity;
		}
		if (num == 0f)
		{
			return result;
		}
		return num;
	}

	private static float SetCustomHealth(ZDO zdo, MineRock5 mineRock, float health)
	{
		float num = 0f;
		foreach (HitArea hitArea in mineRock.m_hitAreas)
		{
			if (!(hitArea.m_health <= 0f))
			{
				num += health - hitArea.m_health;
				hitArea.m_health = health;
			}
		}
		mineRock.SaveHealth();
		if (RemoveToolTier(mineRock, zdo))
		{
			return float.NegativeInfinity;
		}
		return num;
	}

	private static float SetCustomHealth(ZDO zdo, WearNTear wear, float health)
	{
		float @float = zdo.GetFloat(ZDOVars.s_health, wear.m_health);
		zdo.Set(ZDOVars.s_health, health);
		float float2 = zdo.GetFloat(Hashes.MaxHealth, 0f);
		if (float2 != 0f)
		{
			wear.m_health = ZNetScene.instance.GetPrefab(zdo.GetPrefab()).GetComponent<WearNTear>().m_health;
			zdo.RemoveFloat(Hashes.MaxHealth);
		}
		zdo.RemoveFloat(Hashes.BuildingSkillLevel);
		UpdateVisual(zdo, wear);
		if (!(float2 < 0f))
		{
			return health - @float;
		}
		return float.NegativeInfinity;
	}

	private static float SetCustomHealth(ZDO zdo, Character character, float health)
	{
		float @float = zdo.GetFloat(ZDOVars.s_health, zdo.GetFloat(ZDOVars.s_maxHealth, character.m_health));
		zdo.Set(ZDOVars.s_maxHealth, health);
		zdo.Set(ZDOVars.s_health, health * 1.000001f);
		return health - @float;
	}

	private static float SetInfiniteHealth(ZNetView obj, ZDO zdo)
	{
		bool flag = false;
		Character character = default(Character);
		if (((Component)obj).TryGetComponent<Character>(ref character))
		{
			flag |= SetInfiniteHealth(zdo, character);
		}
		Destructible destructible = default(Destructible);
		if (((Component)obj).TryGetComponent<Destructible>(ref destructible))
		{
			flag |= SetInfiniteHealth(zdo, destructible);
		}
		TreeBase treeBase = default(TreeBase);
		if (((Component)obj).TryGetComponent<TreeBase>(ref treeBase))
		{
			flag |= SetInfiniteHealth(zdo, treeBase);
		}
		TreeLog treeLog = default(TreeLog);
		if (((Component)obj).TryGetComponent<TreeLog>(ref treeLog))
		{
			flag |= SetInfiniteHealth(zdo, treeLog);
		}
		MineRock mineRock = default(MineRock);
		if (((Component)obj).TryGetComponent<MineRock>(ref mineRock))
		{
			flag |= SetInfiniteHealth(zdo, mineRock);
		}
		MineRock5 mineRock2 = default(MineRock5);
		if (((Component)obj).TryGetComponent<MineRock5>(ref mineRock2))
		{
			flag |= SetInfiniteHealth(zdo, mineRock2);
		}
		WearNTear wear = default(WearNTear);
		if (((Component)obj).TryGetComponent<WearNTear>(ref wear))
		{
			flag |= SetInfiniteHealth(zdo, wear);
		}
		if (!flag)
		{
			return 0f;
		}
		return float.PositiveInfinity;
	}

	private static bool SetInfiniteHealth(ZDO zdo, Destructible destructible)
	{
		bool result = zdo.GetInt(Hashes.ToolTierDestructible, 0) != 1073741823;
		zdo.RemoveFloat(ZDOVars.s_health);
		zdo.Set(Hashes.HasFields, true);
		zdo.Set(Hashes.HashFieldsDestructible, true);
		zdo.Set(Hashes.ToolTierDestructible, 1073741823, false);
		destructible.m_minToolTier = 1073741823;
		return result;
	}

	private static bool SetInfiniteHealth(ZDO zdo, TreeBase treeBase)
	{
		bool result = zdo.GetInt(Hashes.ToolTierTreeBase, 0) != 1073741823;
		zdo.RemoveFloat(ZDOVars.s_health);
		zdo.Set(Hashes.HasFields, true);
		zdo.Set(Hashes.HashFieldsTreeBase, true);
		zdo.Set(Hashes.ToolTierTreeBase, 1073741823, false);
		treeBase.m_minToolTier = 1073741823;
		return result;
	}

	private static bool SetInfiniteHealth(ZDO zdo, TreeLog treeLog)
	{
		bool result = zdo.GetInt(Hashes.ToolTierTreeLog, 0) != 1073741823;
		zdo.RemoveFloat(ZDOVars.s_health);
		zdo.Set(Hashes.HasFields, true);
		zdo.Set(Hashes.HashFieldsTreeLog, true);
		zdo.Set(Hashes.ToolTierTreeLog, 1073741823, false);
		treeLog.m_minToolTier = 1073741823;
		return result;
	}

	private static bool SetInfiniteHealth(ZDO zdo, MineRock mineRock)
	{
		bool num = zdo.GetInt(Hashes.ToolTierMineRock, 0) != 1073741823;
		float num2 = RemoveCurrentHealth(mineRock, zdo);
		mineRock.m_minToolTier = 1073741823;
		zdo.Set(Hashes.HasFields, true);
		zdo.Set(Hashes.HashFieldsMineRock, true);
		zdo.Set(Hashes.ToolTierMineRock, 1073741823, false);
		if (!num)
		{
			return num2 != 0f;
		}
		return true;
	}

	private static bool SetInfiniteHealth(ZDO zdo, MineRock5 mineRock)
	{
		bool result = zdo.GetInt(Hashes.ToolTierMineRock5, 0) != 1073741823;
		zdo.Set(Hashes.HasFields, true);
		zdo.Set(Hashes.HashFieldsMineRock5, true);
		zdo.Set(Hashes.ToolTierMineRock5, 1073741823, false);
		mineRock.m_minToolTier = 1073741823;
		foreach (HitArea hitArea in mineRock.m_hitAreas)
		{
			if (!(hitArea.m_health <= 0f))
			{
				hitArea.m_health = 1.0737418E+09f;
			}
		}
		mineRock.SaveHealth();
		return result;
	}

	private static bool SetInfiniteHealth(ZDO zdo, WearNTear wear)
	{
		float @float = zdo.GetFloat(Hashes.MaxHealth, 0f);
		zdo.Set(ZDOVars.s_health, -1f);
		float num = -1f;
		if (Configuration.Invulnerability == "Worn")
		{
			num = -2f;
		}
		if (Configuration.Invulnerability == "Damaged")
		{
			num = -4f;
		}
		if (Configuration.PreserveWear && Configuration.Invulnerability != "Off")
		{
			if (@float < 0f)
			{
				num = @float;
			}
			else if (wear.GetHealthPercentage() <= 0.25f)
			{
				num = -4f;
			}
			else if (wear.GetHealthPercentage() <= 0.75f)
			{
				num = -2f;
			}
		}
		bool result = @float != num;
		zdo.Set(Hashes.HasFields, true);
		zdo.Set(Hashes.HashFieldsWearNTear, true);
		zdo.Set(Hashes.MaxHealth, num);
		zdo.RemoveFloat(Hashes.BuildingSkillLevel);
		wear.m_health = num;
		UpdateVisual(zdo, wear);
		return result;
	}

	private static bool SetInfiniteHealth(ZDO zdo, Character character)
	{
		bool result = zdo.GetFloat(ZDOVars.s_maxHealth, 0f) != 1E+30f;
		zdo.Set(ZDOVars.s_health, 1.000001E+30f);
		zdo.Set(ZDOVars.s_maxHealth, 1E+30f);
		return result;
	}

	private static void UpdateVisual(ZDO zdo, WearNTear wear)
	{
		wear.m_healthPercentage = zdo.GetFloat(ZDOVars.s_health, wear.m_health) / wear.m_health;
		wear.SetHealthVisual(wear.m_healthPercentage, false);
	}

	private static float RemoveCurrentHealth(MineRock mineRock, ZDO zdo)
	{
		if (mineRock.m_hitAreas == null)
		{
			return 0f;
		}
		float num = 0f;
		for (int i = 0; i < mineRock.m_hitAreas.Length; i++)
		{
			string text = "Health" + i;
			float @float = zdo.GetFloat(text, 0f);
			if (!(@float <= 0f))
			{
				num += mineRock.m_health - @float;
				zdo.RemoveFloat(text);
			}
		}
		return num;
	}

	private static bool RemoveToolTier(MineRock5 obj, ZDO zdo)
	{
		if (zdo.GetInt(Hashes.ToolTierMineRock5, -1) < 0)
		{
			return false;
		}
		zdo.RemoveInt(Hashes.ToolTierMineRock5);
		obj.m_minToolTier = ZNetScene.instance.GetPrefab(zdo.GetPrefab()).GetComponent<MineRock5>()?.m_minToolTier ?? 0;
		return true;
	}

	private static bool RemoveToolTier(MineRock obj, ZDO zdo)
	{
		if (zdo.GetInt(Hashes.ToolTierMineRock, -1) < 0)
		{
			return false;
		}
		zdo.RemoveInt(Hashes.ToolTierMineRock);
		obj.m_minToolTier = ZNetScene.instance.GetPrefab(zdo.GetPrefab()).GetComponent<MineRock>()?.m_minToolTier ?? 0;
		return true;
	}

	private static bool RemoveToolTier(TreeBase obj, ZDO zdo)
	{
		if (zdo.GetInt(Hashes.ToolTierTreeBase, -1) < 0)
		{
			return false;
		}
		zdo.RemoveInt(Hashes.ToolTierTreeBase);
		obj.m_minToolTier = ZNetScene.instance.GetPrefab(zdo.GetPrefab()).GetComponent<TreeBase>()?.m_minToolTier ?? 0;
		return true;
	}

	private static bool RemoveToolTier(TreeLog obj, ZDO zdo)
	{
		if (zdo.GetInt(Hashes.ToolTierTreeLog, -1) < 0)
		{
			return false;
		}
		zdo.RemoveInt(Hashes.ToolTierTreeLog);
		obj.m_minToolTier = ZNetScene.instance.GetPrefab(zdo.GetPrefab()).GetComponent<TreeLog>()?.m_minToolTier ?? 0;
		return true;
	}

	private static bool RemoveToolTier(Destructible obj, ZDO zdo)
	{
		if (zdo.GetInt(Hashes.ToolTierDestructible, -1) < 0)
		{
			return false;
		}
		zdo.RemoveInt(Hashes.ToolTierDestructible);
		obj.m_minToolTier = ZNetScene.instance.GetPrefab(zdo.GetPrefab()).GetComponent<Destructible>()?.m_minToolTier ?? 0;
		return true;
	}
}
