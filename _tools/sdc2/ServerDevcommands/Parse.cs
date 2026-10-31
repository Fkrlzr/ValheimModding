using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace ServerDevcommands;

public static class Parse
{
	private static readonly HashSet<string> Truthies = new HashSet<string> { "1", "t", "true", "yes", "on" };

	private static readonly HashSet<string> Falsies = new HashSet<string> { "0", "f", "false", "no", "off" };

	private static Range<string> Range(string arg)
	{
		List<string> list = arg.Split(';').ToList();
		if (list.Count == 2)
		{
			return new Range<string>(list[0], list[1]);
		}
		List<string> list2 = arg.Split('-').ToList();
		List<string> list3 = new List<string>();
		for (int i = 0; i < list2.Count; i++)
		{
			string text = list2[i];
			if (text == "")
			{
				if (i + 1 < list2.Count && list2[i + 1] != "")
				{
					list3.Add("-" + list2[i + 1]);
					i++;
				}
			}
			else
			{
				list3.Add(text);
			}
		}
		if (list3.Count == 1)
		{
			return new Range<string>(list3[0]);
		}
		if (list3.Count >= 2)
		{
			return new Range<string>(list3[0], list3[1]);
		}
		return new Range<string>(arg);
	}

	public static int Int(string arg, int defaultValue = 0)
	{
		if (int.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			return result;
		}
		return defaultValue;
	}

	public static int Int(string[] args, int index, int defaultValue = 0)
	{
		if (args.Length <= index)
		{
			return defaultValue;
		}
		return Int(args[index], defaultValue);
	}

	public static int? IntNull(string arg)
	{
		if (int.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			return result;
		}
		return null;
	}

	public static int? IntNull(string[] args, int index)
	{
		if (args.Length <= index)
		{
			return null;
		}
		return IntNull(args[index]);
	}

	public static Range<int> IntRange(string arg, int defaultValue = 0)
	{
		Range<string> range = Range(arg);
		return new Range<int>(Int(range.Min, defaultValue), Int(range.Max, defaultValue));
	}

	public static Range<int> IntRange(string[] args, int index, int defaultValue = 0)
	{
		if (args.Length <= index)
		{
			return new Range<int>(defaultValue);
		}
		return IntRange(args[index], defaultValue);
	}

	public static uint UInt(string arg, uint defaultValue = 0u)
	{
		if (uint.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			return result;
		}
		return defaultValue;
	}

	public static uint UInt(string[] args, int index, uint defaultValue = 0u)
	{
		if (args.Length <= index)
		{
			return defaultValue;
		}
		return UInt(args[index], defaultValue);
	}

	public static Range<uint> UIntRange(string arg, uint defaultValue = 0u)
	{
		Range<string> range = Range(arg);
		return new Range<uint>(UInt(range.Min, defaultValue), UInt(range.Max, defaultValue));
	}

	public static Range<uint> UIntRange(string[] args, int index, uint defaultValue = 0u)
	{
		if (args.Length <= index)
		{
			return new Range<uint>(defaultValue);
		}
		return UIntRange(args[index], defaultValue);
	}

	public static long Long(string arg, long defaultValue = 0L)
	{
		if (long.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			return result;
		}
		return defaultValue;
	}

	public static long Long(string[] args, int index, long defaultValue = 0L)
	{
		if (args.Length <= index)
		{
			return defaultValue;
		}
		return Long(args[index], defaultValue);
	}

	public static long? LongNull(string[] args, int index)
	{
		if (args.Length <= index)
		{
			return null;
		}
		return LongNull(args[index]);
	}

	public static long? LongNull(string arg)
	{
		if (long.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			return result;
		}
		return null;
	}

	public static Range<long> LongRange(string arg, long defaultValue = 0L)
	{
		Range<string> range = Range(arg);
		return new Range<long>(Long(range.Min, defaultValue), Long(range.Max, defaultValue));
	}

	public static Range<long> LongRange(string[] args, int index, long defaultValue = 0L)
	{
		if (args.Length <= index)
		{
			return new Range<long>(defaultValue);
		}
		return LongRange(args[index], defaultValue);
	}

	public static float Float(string arg, float defaultValue = 0f)
	{
		if (!float.TryParse(arg, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static bool TryFloat(string arg, out float value)
	{
		return float.TryParse(arg, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
	}

	public static float Float(string[] args, int index, float defaultValue = 0f)
	{
		if (args.Length <= index)
		{
			return defaultValue;
		}
		return Float(args[index], defaultValue);
	}

	public static float? FloatNull(string arg)
	{
		if (float.TryParse(arg, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
		{
			return result;
		}
		return null;
	}

	public static float? FloatNull(string[] args, int index)
	{
		if (args.Length <= index)
		{
			return null;
		}
		return FloatNull(args[index]);
	}

	public static Range<float> FloatRange(string arg, float defaultValue = 0f)
	{
		Range<string> range = Range(arg);
		return new Range<float>(Float(range.Min, defaultValue), Float(range.Max, defaultValue));
	}

	public static Range<float> FloatRange(string[] args, int index, float defaultValue = 0f)
	{
		if (args.Length <= index)
		{
			return new Range<float>(defaultValue);
		}
		return FloatRange(args[index], defaultValue);
	}

	public static string String(string[] args, int index, string defaultValue = "")
	{
		if (args.Length <= index)
		{
			return defaultValue;
		}
		return args[index];
	}

	public static Quaternion AngleYXZ(string arg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return AngleYXZ(arg, Quaternion.identity);
	}

	public static Quaternion AngleYXZ(string[] values, int index)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return AngleYXZ(values, Quaternion.identity, index);
	}

	public static Quaternion AngleYXZ(string arg, Quaternion defaultValue)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		return AngleYXZ(Split(arg), defaultValue);
	}

	public static Quaternion AngleYXZ(string[] values, Quaternion defaultValue, int index = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zero = Vector3.zero;
		zero.y = Float(values, index, ((Quaternion)(ref defaultValue)).eulerAngles.y);
		zero.x = Float(values, 1 + index, ((Quaternion)(ref defaultValue)).eulerAngles.x);
		zero.z = Float(values, 2 + index, ((Quaternion)(ref defaultValue)).eulerAngles.z);
		return Quaternion.Euler(zero);
	}

	public static Quaternion? AngleYXZNull(string arg)
	{
		return AngleYXZNull(Split(arg));
	}

	public static Quaternion? AngleYXZNull(string[] values)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		float? num = FloatNull(values, 0);
		float? num2 = FloatNull(values, 1);
		float? num3 = FloatNull(values, 2);
		if (!num.HasValue || !num2.HasValue || !num3.HasValue)
		{
			return null;
		}
		return Quaternion.Euler(new Vector3(num2.Value, num.Value, num3.Value));
	}

	public static Range<Quaternion> AngleYXZRange(string arg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return AngleYXZRange(arg, Quaternion.identity);
	}

	public static Range<Quaternion> AngleYXZRange(string arg, Quaternion defaultValue)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		string[] args = Split(arg);
		Range<float> y = FloatRange(args, 0, defaultValue.y);
		Range<float> x = FloatRange(args, 1, defaultValue.x);
		Range<float> z = FloatRange(args, 2, defaultValue.z);
		return ToAngleRange(x, y, z);
	}

	private static Range<Quaternion> ToAngleRange(Range<float> x, Range<float> y, Range<float> z)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		Quaternion min = Quaternion.Euler(new Vector3(x.Min, y.Min, z.Min));
		Quaternion max = Quaternion.Euler(new Vector3(x.Max, y.Max, z.Max));
		return new Range<Quaternion>(min, max);
	}

	public static Vector3 VectorXZY(string arg)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return VectorXZY(Split(arg), 0, Vector3.zero);
	}

	public static Vector3 VectorXZY(string[] args)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return VectorXZY(args, 0, Vector3.zero);
	}

	public static Vector3 VectorXZY(string[] args, Vector3 defaultValue)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return VectorXZY(args, 0, defaultValue);
	}

	public static Vector3 VectorXZY(string[] args, int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return VectorXZY(args, index, Vector3.zero);
	}

	public static Vector3 VectorXZY(string[] args, int index, Vector3 defaultValue)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zero = Vector3.zero;
		zero.x = Float(args, index, defaultValue.x);
		zero.y = Float(args, index + 2, defaultValue.y);
		zero.z = Float(args, index + 1, defaultValue.z);
		return zero;
	}

	public static Vector3? VectorXZYNull(string arg)
	{
		return VectorXZYNull(Split(arg));
	}

	public static Vector3? VectorXZYNull(string[] args)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		float? num = FloatNull(args, 0);
		float? num2 = FloatNull(args, 2);
		float? num3 = FloatNull(args, 1);
		if (!num.HasValue || !num2.HasValue || !num3.HasValue)
		{
			return null;
		}
		return new Vector3(num.Value, num2.Value, num3.Value);
	}

	public static Vector3 VectorZYX(string arg)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return VectorZYX(Split(arg), 0, Vector3.zero);
	}

	public static Vector3 VectorZYX(string[] args, int index, Vector3 defaultValue)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zero = Vector3.zero;
		zero.x = Float(args, index + 2, defaultValue.z);
		zero.y = Float(args, index + 1, defaultValue.y);
		zero.z = Float(args, index, defaultValue.x);
		return zero;
	}

	public static Range<Vector3> VectorZYXRange(string arg, Vector3 defaultValue)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		string[] args = Split(arg);
		Range<float> x = FloatRange(args, 2, defaultValue.x);
		Range<float> y = FloatRange(args, 1, defaultValue.y);
		Range<float> z = FloatRange(args, 0, defaultValue.z);
		return ToVectorRange(x, y, z);
	}

	public static Range<Vector3Int> VectorIntZYXRange(string arg, Vector3Int defaultValue)
	{
		string[] args = Split(arg);
		Range<int> x = IntRange(args, 2, ((Vector3Int)(ref defaultValue)).x);
		Range<int> y = IntRange(args, 1, ((Vector3Int)(ref defaultValue)).y);
		Range<int> z = IntRange(args, 0, ((Vector3Int)(ref defaultValue)).z);
		return ToVectorRange(x, y, z);
	}

	public static Range<Vector3> VectorXZYRange(string arg, Vector3 defaultValue)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		string[] args = Split(arg);
		Range<float> x = FloatRange(args, 0, defaultValue.x);
		Range<float> y = FloatRange(args, 2, defaultValue.y);
		Range<float> z = FloatRange(args, 1, defaultValue.z);
		return ToVectorRange(x, y, z);
	}

	public static Vector2i Vector2Int(string arg)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		string[] array = SplitWithEmpty(arg);
		return new Vector2i(Int(array[0]), (array.Length > 1) ? Int(array[1]) : 0);
	}

	public static Vector3 VectorZXY(string[] args)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return VectorZXY(args, 0, Vector3.zero);
	}

	public static Vector3 VectorZXY(string[] args, Vector3 defaultValue)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return VectorZXY(args, 0, defaultValue);
	}

	public static Vector3 VectorZXY(string[] args, int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return VectorZXY(args, index, Vector3.zero);
	}

	public static Vector3 VectorZXY(string[] args, int index, Vector3 defaultValue)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zero = Vector3.zero;
		zero.x = Float(args, index + 1, defaultValue.x);
		zero.y = Float(args, index + 2, defaultValue.y);
		zero.z = Float(args, index, defaultValue.z);
		return zero;
	}

	public static Range<Vector3> VectorZXYRange(string arg, Vector3 defaultValue)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		string[] args = Split(arg);
		Range<float> x = FloatRange(args, 1, defaultValue.x);
		Range<float> y = FloatRange(args, 2, defaultValue.y);
		Range<float> z = FloatRange(args, 0, defaultValue.z);
		return ToVectorRange(x, y, z);
	}

	private static Range<Vector3> ToVectorRange(Range<float> x, Range<float> y, Range<float> z)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		Vector3 min = new Vector3(x.Min, y.Min, z.Min);
		Vector3 max = default(Vector3);
		((Vector3)(ref max))._002Ector(x.Max, y.Max, z.Max);
		return new Range<Vector3>(min, max);
	}

	private static Range<Vector3Int> ToVectorRange(Range<int> x, Range<int> y, Range<int> z)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		Vector3Int min = new Vector3Int(x.Min, y.Min, z.Min);
		Vector3Int max = default(Vector3Int);
		((Vector3Int)(ref max))._002Ector(x.Max, y.Max, z.Max);
		return new Range<Vector3Int>(min, max);
	}

	public static Vector3 VectorYXZ(string arg)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return VectorYXZ(Split(arg), 0, Vector3.zero);
	}

	public static Vector3 VectorYXZ(string[] args)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return VectorYXZ(args, 0, Vector3.zero);
	}

	public static Vector3 VectorYXZ(string[] args, Vector3 defaultValue)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return VectorYXZ(args, 0, defaultValue);
	}

	public static Vector3 VectorYXZ(string[] args, int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return VectorYXZ(args, index, Vector3.zero);
	}

	public static Vector3 VectorYXZ(string[] args, int index, Vector3 defaultValue)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zero = Vector3.zero;
		zero.y = Float(args, index, defaultValue.y);
		zero.x = Float(args, index + 1, defaultValue.x);
		zero.z = Float(args, index + 2, defaultValue.z);
		return zero;
	}

	public static Range<Vector3> VectorYXZRange(string arg, Vector3 defaultValue)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		string[] args = Split(arg);
		Range<float> x = FloatRange(args, 1, defaultValue.x);
		Range<float> y = FloatRange(args, 0, defaultValue.y);
		Range<float> z = FloatRange(args, 2, defaultValue.z);
		return ToVectorRange(x, y, z);
	}

	public static Vector3 Scale(string[] args)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return Scale(args, 0);
	}

	public static Vector3 Scale(string[] args, int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return SanityCheck(VectorXZY(args, index));
	}

	private static Vector3 SanityCheck(Vector3 scale)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		if (scale.x == 0f)
		{
			scale.x = 1f;
		}
		if (scale.y == 0f)
		{
			scale.y = scale.x;
		}
		if (scale.z == 0f)
		{
			scale.z = scale.x;
		}
		return scale;
	}

	public static Range<Vector3> ScaleRange(string arg)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		string[] array = Split(arg);
		Range<float> x = FloatRange(array, 0);
		Range<float> y = FloatRange(array, 2);
		Range<float> z = FloatRange(array, 1);
		Range<Vector3> range = ToVectorRange(x, y, z);
		range.Min = SanityCheck(range.Min);
		range.Max = SanityCheck(range.Max);
		range.Uniform = array.Length < 4;
		return range;
	}

	public static string[] SplitWithEscape(string arg, char separator = ',')
	{
		List<string> list = new List<string>();
		string[] array = arg.Split(separator);
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].TrimStart();
			if (text.StartsWith("\""))
			{
				array[i] = text.Substring(1);
				int j;
				for (j = i; j < array.Length; j++)
				{
					text = array[j].TrimEnd();
					if (text.EndsWith("\""))
					{
						array[j] = text.Substring(0, text.Length - 1);
						break;
					}
				}
				list.Add(string.Join(separator.ToString(), array.Skip(i).Take(j - i + 1)));
				i = j;
			}
			else
			{
				list.Add(array[i].Trim());
			}
		}
		return list.ToArray();
	}

	public static KeyValuePair<string, string> Kvp(string str, char separator = ',')
	{
		int num = str.IndexOf(separator);
		if (num < 0)
		{
			return new KeyValuePair<string, string>(str, "");
		}
		return new KeyValuePair<string, string>(str.Substring(0, num), str.Substring(num + 1).Trim());
	}

	public static string[] SplitWithEmpty(string arg, char separator = ',')
	{
		return (from s in arg.Split(separator)
			select s.Trim()).ToArray();
	}

	public static string[] Split(string arg, char separator = ',')
	{
		return (from s in arg.Split(separator)
			select s.Trim() into s
			where s != ""
			select s).ToArray();
	}

	public static string[] Split(string[] args, int index, char separator)
	{
		if (args.Length <= index)
		{
			return Array.Empty<string>();
		}
		return Split(args[index], separator);
	}

	private static bool IsTruthy(string value)
	{
		return Truthies.Contains(value);
	}

	private static bool IsFalsy(string value)
	{
		return Falsies.Contains(value);
	}

	public static bool? Boolean(string value)
	{
		if (IsTruthy(value))
		{
			return true;
		}
		if (IsFalsy(value))
		{
			return false;
		}
		return Helper.IsDown(value);
	}

	public static bool? BoolNull(string? arg)
	{
		if (arg == null)
		{
			return null;
		}
		arg = arg.ToLower();
		if (IsTruthy(arg))
		{
			return true;
		}
		if (IsFalsy(arg))
		{
			return false;
		}
		return null;
	}

	public static string Logic(string value)
	{
		if (!value.Contains("?") || !value.Contains(":"))
		{
			return value;
		}
		string[] array = value.Split('?');
		string value2 = array[0];
		array = array[1].Split(':');
		if (!Boolean(value2).GetValueOrDefault())
		{
			return array[1];
		}
		return array[0];
	}

	public static float Multiplier(string value)
	{
		float num = 1f;
		string[] array = value.Split('*');
		foreach (string arg in array)
		{
			num *= Float(arg, 1f);
		}
		return num;
	}

	public static float TryMultiplier(string[] args, int index, float defaultValue = 1f)
	{
		if (args.Length <= index)
		{
			return defaultValue;
		}
		return Multiplier(args[index]);
	}

	public static float Direction(string[] args, int index)
	{
		if (args.Length > index)
		{
			return Direction(args[index]);
		}
		return 1f;
	}

	public static float Direction(string arg)
	{
		if (!(Float(arg, 1f) > 0f))
		{
			return -1f;
		}
		return 1f;
	}
}
