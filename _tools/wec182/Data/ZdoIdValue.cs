using System.Collections.Generic;
using System.Linq;
using Common;

namespace Data;

public class ZdoIdValue : DynamicValue, IZdoIdValue
{
	public ZdoIdValue(string[] values)
		: base(values)
	{
	}

	public ZDOID? Get(Functions f)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		string value = GetValue(f);
		if (value == null)
		{
			return null;
		}
		return Parse.ZdoId(value);
	}

	public bool? Match(Functions f, ZDOID value)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		List<string> allValues = GetAllValues(f);
		if (allValues.Count == 0)
		{
			return null;
		}
		return allValues.Any((string v) => Parse.ZdoId(v) == value);
	}
}
