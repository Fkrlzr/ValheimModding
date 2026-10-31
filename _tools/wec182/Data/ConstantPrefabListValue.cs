using System.Collections.Generic;
using UnityEngine;

namespace Data;

public class ConstantPrefabListValue : IPrefabValue
{
	private readonly List<int> Values;

	public ConstantPrefabListValue(List<int> value)
	{
		Values = value;
		base._002Ector();
	}

	public int? Get(Functions f)
	{
		return RollValue();
	}

	public bool? Match(Functions f, int value)
	{
		return Values.Contains(value);
	}

	private int RollValue()
	{
		return Values[Random.Range(0, Values.Count)];
	}
}
