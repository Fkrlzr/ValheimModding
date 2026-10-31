using System;
using Common;

namespace Data;

public class RangeIntValue : DynamicValue, IRangeIntValue
{
	public RangeIntValue(string[] values)
		: base(values)
	{
	}

	public ValueRange<int>? Get(Functions f)
	{
		string value = GetValue(f);
		if (value == null)
		{
			return null;
		}
		if (!value.Contains(";"))
		{
			int? num = Parse.IntNull(value);
			if (num.HasValue)
			{
				return new ValueRange<int>(num.Value, 0);
			}
			return null;
		}
		string[] array = value.Split(';');
		if (array.Length < 2)
		{
			throw new InvalidOperationException("Invalid range format: " + value);
		}
		int? num2 = Parse.IntNull(array[0]);
		int? num3 = Parse.IntNull(array[1]);
		if (!num2.HasValue || !num3.HasValue)
		{
			return null;
		}
		return new ValueRange<int>(num2.Value, num3.Value);
	}
}
