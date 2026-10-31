using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEngine;

namespace Data;

public class Vector3Value : DynamicValue, IVector3Value
{
	public Vector3Value(string[] values)
		: base(values)
	{
	}

	public Vector3? Get(Functions f)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		string value = GetValue(f);
		if (value != null)
		{
			return Calculator.EvaluateVector3(value);
		}
		return null;
	}

	public bool? Match(Functions f, Vector3 value)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		List<string> allValues = GetAllValues(f);
		if (allValues.Count == 0)
		{
			return null;
		}
		return allValues.Any(delegate(string v)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			Vector3? val = Parse.VectorXZYNull(v);
			Vector3 val2 = value;
			return val.HasValue && val.GetValueOrDefault() == val2;
		});
	}
}
