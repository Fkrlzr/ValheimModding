using System.Collections.Generic;
using UnityEngine;

namespace InfinityHammer;

public static class ResourceCost
{
	public static Requirement[] Calculate(List<SelectedObject> objects)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		Dictionary<ItemDrop, Requirement> dictionary = new Dictionary<ItemDrop, Requirement>();
		Piece val = default(Piece);
		Requirement[] resources;
		foreach (SelectedObject @object in objects)
		{
			GameObject prefab = ZNetScene.instance.GetPrefab(@object.Prefab);
			if (!Object.op_Implicit((Object)(object)prefab) || !prefab.TryGetComponent<Piece>(ref val) || val.m_resources == null)
			{
				continue;
			}
			resources = val.m_resources;
			foreach (Requirement val2 in resources)
			{
				if (Object.op_Implicit((Object)(object)val2.m_resItem))
				{
					if (dictionary.TryGetValue(val2.m_resItem, out var value))
					{
						Requirement obj = value;
						obj.m_amount += val2.m_amount;
						continue;
					}
					dictionary[val2.m_resItem] = new Requirement
					{
						m_resItem = val2.m_resItem,
						m_amount = val2.m_amount,
						m_recover = val2.m_recover
					};
				}
			}
		}
		Dictionary<ItemDrop, Requirement>.ValueCollection values = dictionary.Values;
		int i = 0;
		resources = (Requirement[])(object)new Requirement[values.Count];
		foreach (Requirement item in values)
		{
			resources[i] = item;
			i++;
		}
		return resources;
	}
}
