using System.Collections.Generic;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public static class Selection
{
	public static BaseSelection BaseSelection = new BaseSelection();

	public static Dictionary<string, BaseSelection> Selections = new Dictionary<string, BaseSelection>();

	public static BaseSelection Get()
	{
		if (!Selections.TryGetValue(HammerHelper.GetTool(), out BaseSelection value))
		{
			return BaseSelection;
		}
		return value;
	}

	public static void Clear()
	{
		if (Configuration.UnfreezeOnSelect)
		{
			Position.Unfreeze();
		}
		if (Selections.TryGetValue(HammerHelper.GetTool(), out BaseSelection value))
		{
			value.Deactivate();
			value.Destroy();
			Hammer.SelectRepairIfEmpty();
			Selections.Remove(HammerHelper.GetTool());
		}
	}

	public static void Destroy()
	{
		foreach (BaseSelection value in Selections.Values)
		{
			value.Deactivate();
			value.Destroy();
		}
		Selections.Clear();
	}

	public static GameObject CreateGhost(BaseSelection selection)
	{
		Clear();
		selection.Activate();
		Selections[HammerHelper.GetTool()] = selection;
		Player player = Helper.GetPlayer();
		player.SetupPlacementGhost();
		return player.m_placementGhost;
	}
}
