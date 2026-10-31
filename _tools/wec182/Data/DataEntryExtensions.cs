using System.Collections.Generic;

namespace Data;

public static class DataEntryExtensions
{
	public static void Set(this DataEntry data, int key, string value)
	{
		if (data.Strings == null)
		{
			data.Strings = new Dictionary<int, IStringValue>();
		}
		data.Strings[key] = DataValue.Constant(value);
	}

	public static void Set(this DataEntry data, int key, float value)
	{
		if (data.Floats == null)
		{
			data.Floats = new Dictionary<int, IFloatValue>();
		}
		data.Floats[key] = DataValue.Constant(value);
	}

	public static void Set(this DataEntry data, int key, int value)
	{
		if (data.Ints == null)
		{
			data.Ints = new Dictionary<int, IIntValue>();
		}
		data.Ints[key] = DataValue.Constant(value);
	}

	public static void Set(this DataEntry data, int key, long value)
	{
		if (data.Longs == null)
		{
			data.Longs = new Dictionary<int, ILongValue>();
		}
		data.Longs[key] = DataValue.Constant(value);
	}

	public static void Set(this DataEntry data, int key, bool value)
	{
		if (data.Bools == null)
		{
			data.Bools = new Dictionary<int, IBoolValue>();
		}
		data.Bools[key] = new ConstantBoolValue(value);
	}

	public static bool TryGetString(this DataEntry data, Dictionary<string, string> pars, int key, out string value)
	{
		value = "";
		if (data.Strings == null || !data.Strings.TryGetValue(key, out IStringValue value2))
		{
			return false;
		}
		string text = value2.Get(new ParFunctions(pars, null));
		if (text == null)
		{
			return false;
		}
		value = text;
		return true;
	}

	public static bool TryGetFloat(this DataEntry data, Dictionary<string, string> pars, int key, out float value)
	{
		value = 0f;
		if (data.Floats == null || !data.Floats.TryGetValue(key, out IFloatValue value2))
		{
			return false;
		}
		float? num = value2.Get(new ParFunctions(pars, null));
		if (!num.HasValue)
		{
			return false;
		}
		value = num.Value;
		return true;
	}

	public static bool TryGetInt(this DataEntry data, Dictionary<string, string> pars, int key, out int value)
	{
		value = 0;
		if (data.Ints == null || !data.Ints.TryGetValue(key, out IIntValue value2))
		{
			return false;
		}
		int? num = value2.Get(new ParFunctions(pars, null));
		if (!num.HasValue)
		{
			return false;
		}
		value = num.Value;
		return true;
	}

	public static bool TryGetHash(this DataEntry data, Dictionary<string, string> pars, int key, out int value)
	{
		value = 0;
		if (data.Hashes == null || !data.Hashes.TryGetValue(key, out IHashValue value2))
		{
			return false;
		}
		int? num = value2.Get(new ParFunctions(pars, null));
		if (!num.HasValue)
		{
			return false;
		}
		value = num.Value;
		return true;
	}

	public static string GetBase64(this DataEntry data, Dictionary<string, string> pars)
	{
		return DataHelper.Resolve(data, pars, null).GetBase64();
	}
}
