using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Service;
using UnityEngine;

namespace Data;

public class DataValue
{
	public static IZdoIdValue ZdoId(string values)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		string[] array = SplitWithValues(values);
		ZDOID val = Parse.ZdoId(array[0]);
		if (array.Length == 1 && val != ZDOID.None)
		{
			return new ConstantZdoIdValue(val);
		}
		return new ZdoIdValue(array);
	}

	public static IIntValue Constant(int value)
	{
		return new ConstantIntValue(value);
	}

	public static IStringValue Constant(string value)
	{
		return new ConstantStringValue(value);
	}

	public static IFloatValue Constant(float value)
	{
		return new ConstantFloatValue(value);
	}

	public static ILongValue Constant(long value)
	{
		return new ConstantLongValue(value);
	}

	public static IVector3Value Constant(Vector3 value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return new ConstantVector3Value(value);
	}

	public static IQuaternionValue Constant(Quaternion value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return new ConstantQuaternionValue(value);
	}

	public static IBytesValue Constant(byte[]? value)
	{
		return new ConstantBytesValue(value);
	}

	public static IIntValue Int(string values)
	{
		string[] array = SplitWithValues(values);
		if (array.Length == 1 && Parse.TryInt(array[0], out var result))
		{
			return new ConstantIntValue(result);
		}
		return new IntValue(array);
	}

	public static IRangeIntValue RangeInt(string values)
	{
		string[] array = SplitWithValues(values);
		if (array.Length == 1 && !HasFunctions(array[0]))
		{
			if (Parse.TryInt(array[0], out var result))
			{
				return new ConstantRangeIntValue(new ValueRange<int>(result, 0));
			}
			return new ConstantRangeIntValue(Parse.IntRange(array[0]));
		}
		return new RangeIntValue(array);
	}

	public static IFloatValue Float(string values)
	{
		string[] array = SplitWithValues(values);
		if (array.Length == 1 && Parse.TryFloat(array[0], out var result))
		{
			return new ConstantFloatValue(result);
		}
		return new FloatValue(array);
	}

	public static ILongValue Long(string values)
	{
		string[] array = SplitWithValues(values);
		if (array.Length == 1 && Parse.TryLong(array[0], out var result))
		{
			return new ConstantLongValue(result);
		}
		return new LongValue(array);
	}

	public static IStringValue String(string values)
	{
		if (values.Length > 2 && values[0] == '"' && values[values.Length - 1] == '"')
		{
			return new ConstantStringValue(values.Substring(1, values.Length - 2));
		}
		string[] array = SplitWithValues(values);
		if (array.Length == 1 && !HasFunctions(array[0]))
		{
			return new ConstantStringValue(array[0]);
		}
		return new StringValue(array);
	}

	public static IBoolValue Bool(string values)
	{
		string[] array = SplitWithValues(values);
		if (array.Length == 1 && bool.TryParse(array[0], out var result))
		{
			return new ConstantBoolValue(result);
		}
		return new BoolValue(array);
	}

	public static IBytesValue Bytes(string values)
	{
		string[] array = SplitWithValues(values);
		if (array.Length == 1 && !HasFunctions(array[0]))
		{
			if (string.IsNullOrEmpty(array[0]))
			{
				return new ConstantBytesValue(null);
			}
			try
			{
				return new ConstantBytesValue(Convert.FromBase64String(array[0]));
			}
			catch (FormatException)
			{
			}
		}
		return new BytesValue(array);
	}

	public static IHashValue Hash(string values)
	{
		string[] array = SplitWithValues(values);
		if (array.Length == 1 && !HasFunctions(array[0]))
		{
			return new ConstantHashValue(array[0]);
		}
		return new HashValue(array);
	}

	public static IPrefabValue Prefab(string values)
	{
		if (HasFunctions(values))
		{
			return new PrefabValue(SplitWithValues(values));
		}
		List<int> prefabs = PrefabHelper.GetPrefabs(values, "");
		if (prefabs.Count == 0)
		{
			return new ConstantPrefabValue(null);
		}
		if (prefabs.Count == 1)
		{
			return new ConstantPrefabValue(prefabs[0]);
		}
		return new ConstantPrefabListValue(prefabs);
	}

	public static IVector3Value Vector3(string values)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		string[] array = SplitWithValues(values);
		if (HasFunctions(values) || array.Length > 3)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < array.Length; i += 3)
			{
				string[] array2;
				if (i + 3 >= array.Length)
				{
					List<string> list2 = new List<string>();
					list2.AddRange(array.Skip(i));
					array2 = list2.ToArray();
				}
				else
				{
					array2 = array.Skip(i).Take(3).ToArray();
				}
				string[] value = array2;
				list.Add(string.Join(",", value));
			}
			return new Vector3Value(list.ToArray());
		}
		if (Parse.TryDistanceAngle(array, out var vector))
		{
			return new ConstantVector3Value(vector);
		}
		Vector3? val = Parse.VectorXZYNull(array);
		return new ConstantVector3Value(val.HasValue ? val.Value : Vector3.zero);
	}

	public static IQuaternionValue Quaternion(string values)
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		string[] array = SplitWithValues(values);
		if (HasFunctions(values) || array.Length > 3)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < array.Length; i += 3)
			{
				string[] array2;
				if (i + 3 >= array.Length)
				{
					List<string> list2 = new List<string>();
					list2.AddRange(array.Skip(i));
					array2 = list2.ToArray();
				}
				else
				{
					array2 = array.Skip(i).Take(3).ToArray();
				}
				string[] value = array2;
				list.Add(string.Join(",", value));
			}
			return new QuaternionValue(list.ToArray());
		}
		Quaternion? val = Parse.AngleYXZNull(array);
		return new ConstantQuaternionValue(val.HasValue ? val.Value : Quaternion.identity);
	}

	private static bool HasFunctions(string value)
	{
		if (value.Contains("<"))
		{
			return value.Contains(">");
		}
		return false;
	}

	private static string[] SplitWithValues(string str)
	{
		List<string> list = new List<string>();
		string[] array = Parse.SplitWithEmpty(str);
		foreach (string text in array)
		{
			if (!HasFunctions(text))
			{
				list.Add(text);
				continue;
			}
			string[] array2 = text.Split('<', '>');
			List<string> list2 = new List<string>();
			List<int> list3 = new List<int>();
			for (int j = 1; j < array2.Length; j += 2)
			{
				int num = ValueGroups.Hash(array2[j]);
				if (ValueGroups.Contains(num))
				{
					list2.Add("<" + array2[j] + ">");
					list3.Add(num);
				}
			}
			if (list2.Count == 0)
			{
				list.Add(text);
				if (list.Count > 1000)
				{
					break;
				}
			}
			else
			{
				SubstituteValues(list, text, list2, list3, 0);
			}
		}
		if (list.Count > 1000)
		{
			Log.Warning("Too many values loaded for " + str);
		}
		if (list.Count <= 1000)
		{
			return list.ToArray();
		}
		return new string[1] { str };
	}

	private static void SubstituteValues(List<string> result, string format, List<string> parameters, List<int> hashes, int index)
	{
		if (!ValueGroups.TryGet(hashes[index], out List<string> values))
		{
			return;
		}
		foreach (string item in values)
		{
			string text = format.Replace(parameters[index], item);
			if (index == parameters.Count - 1)
			{
				result.Add(text);
				if (result.Count > 1000)
				{
					break;
				}
			}
			else
			{
				SubstituteValues(result, text, parameters, hashes, index + 1);
			}
		}
	}
}
