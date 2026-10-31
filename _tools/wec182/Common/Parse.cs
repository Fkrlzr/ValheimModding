using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Common;

public static class Parse
{
	private static Dictionary<string, Color> Paints = new Dictionary<string, Color>
	{
		{
			"grass",
			Color.black
		},
		{
			"patches",
			new Color(0f, 0.75f, 0f)
		},
		{
			"grass_dark",
			new Color(0.6f, 0.5f, 0f)
		},
		{
			"dirt",
			Color.red
		},
		{
			"cultivated",
			Color.green
		},
		{
			"paved",
			Color.blue
		},
		{
			"paved_moss",
			new Color(0f, 0f, 0.5f)
		},
		{
			"paved_dirt",
			new Color(1f, 0f, 0.5f)
		},
		{
			"paved_dark",
			new Color(0f, 1f, 0.5f)
		}
	};

	public static List<string> ToList(string str, bool removeEmpty = true)
	{
		string[] array = Split(str, removeEmpty);
		List<string> list = new List<string>(array.Length);
		list.AddRange(array);
		return list;
	}

	public static string[] ToArr(string str, bool removeEmpty = true)
	{
		string[] array = Split(str, removeEmpty);
		int num = 0;
		string[] array2 = new string[array.Length];
		string[] array3 = array;
		foreach (string text in array3)
		{
			array2[num] = text;
			num++;
		}
		return array2;
	}

	public static Vector2i Vector2Int(string arg)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		string[] array = SplitWithEmpty(arg);
		return new Vector2i(Int(array[0]), (array.Length > 1) ? Int(array[1]) : 0);
	}

	public static int Int(string arg, int defaultValue = 0)
	{
		if (!TryInt(arg, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static uint UInt(string arg, uint defaultValue = 0u)
	{
		if (!TryUInt(arg, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static long Long(string arg, long defaultValue = 0L)
	{
		if (!long.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static bool TryLong(string arg, out long result)
	{
		return long.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
	}

	public static int Int(string[] args, int index, int defaultValue = 0)
	{
		if (args.Length <= index)
		{
			return defaultValue;
		}
		return Int(args[index], defaultValue);
	}

	public static bool TryUInt(string arg, out uint result)
	{
		return uint.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
	}

	public static bool TryInt(string arg, out int result)
	{
		return int.TryParse(arg, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
	}

	public static float Float(string arg, float defaultValue = 0f)
	{
		if (!TryFloat(arg, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static float Float(string[] args, int index, float defaultValue = 0f)
	{
		if (args.Length <= index)
		{
			return defaultValue;
		}
		return Float(args[index], defaultValue);
	}

	public static bool TryFloat(string arg, out float result)
	{
		return float.TryParse(arg, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
	}

	public static bool TryDistanceAngle(string arg, out Vector3 vector)
	{
		return TryDistanceAngle(Split(arg), out vector);
	}

	public static bool TryDistanceAngle(string[] values, out Vector3 vector)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		vector = Vector3.zero;
		if (values.Length < 2 || values.Length > 3)
		{
			return false;
		}
		if (values[1] == null || !HasAngleSuffix(values[1].Trim()))
		{
			return false;
		}
		if (!TryDistanceAngle(values[0], values[1], out vector))
		{
			return false;
		}
		if (values.Length > 2 && !TryFloat(values[2], out vector.y))
		{
			return false;
		}
		return true;
	}

	private static bool TryDistanceAngle(string distanceValue, string angleValue, out Vector3 vector)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		vector = Vector3.zero;
		if (!TryFloat(distanceValue, out var result))
		{
			return false;
		}
		if (!TryAngleRadians(angleValue, out var radians))
		{
			return false;
		}
		vector = new Vector3(Mathf.Cos(radians) * result, 0f, Mathf.Sin(radians) * result);
		return true;
	}

	private static bool HasAngleSuffix(string arg)
	{
		if (!arg.EndsWith("deg", StringComparison.OrdinalIgnoreCase))
		{
			return arg.EndsWith("rad", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	public static bool TryAngleRadians(string arg, out float radians)
	{
		radians = 0f;
		if (arg == null)
		{
			return false;
		}
		string text = arg.Trim();
		if (text.EndsWith("deg", StringComparison.OrdinalIgnoreCase))
		{
			if (!TryFloat(text.Substring(0, text.Length - 3).Trim(), out var result))
			{
				return false;
			}
			radians = result * ((float)Math.PI / 180f);
			return true;
		}
		if (text.EndsWith("rad", StringComparison.OrdinalIgnoreCase))
		{
			if (!TryFloat(text.Substring(0, text.Length - 3).Trim(), out radians))
			{
				return false;
			}
			return true;
		}
		return TryFloat(text, out radians);
	}

	public static bool TryAngleDegrees(string arg, out float degrees)
	{
		degrees = 0f;
		if (arg == null)
		{
			return false;
		}
		string text = arg.Trim();
		if (text.EndsWith("deg", StringComparison.OrdinalIgnoreCase))
		{
			return TryFloat(text.Substring(0, text.Length - 3).Trim(), out degrees);
		}
		if (text.EndsWith("rad", StringComparison.OrdinalIgnoreCase))
		{
			if (!TryFloat(text.Substring(0, text.Length - 3).Trim(), out var result))
			{
				return false;
			}
			degrees = result * 57.29578f;
			return true;
		}
		return TryFloat(text, out degrees);
	}

	public static bool TryBoolean(string arg, out bool result)
	{
		result = false;
		if (arg.ToLowerInvariant() == "true")
		{
			result = true;
			return true;
		}
		if (arg.ToLowerInvariant() == "false")
		{
			return true;
		}
		return false;
	}

	public static Quaternion AngleYXZ(string arg)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return AngleYXZ(Split(arg), 0, Vector3.zero);
	}

	public static Quaternion AngleYXZ(string[] args, int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return AngleYXZ(args, index, Vector3.zero);
	}

	public static Quaternion AngleYXZ(string[] args, int index, Vector3 defaultValue)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zero = Vector3.zero;
		zero.y = Float(args, index, defaultValue.y);
		zero.x = Float(args, index + 1, defaultValue.x);
		zero.z = Float(args, index + 2, defaultValue.z);
		return Quaternion.Euler(zero);
	}

	public static string[] Split(string arg, bool removeEmpty = true, char split = ',')
	{
		List<string> list;
		if (!removeEmpty)
		{
			list = new List<string>();
			list.AddRange(from s in arg.Split(split)
				select s.Trim());
			return list.ToArray();
		}
		list = new List<string>();
		list.AddRange(from s in arg.Split(split)
			select s.Trim() into s
			where s != ""
			select s);
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

	public static bool TryKvp(string str, out KeyValuePair<string, string> kvp, char separator = ',')
	{
		kvp = Kvp(str, separator);
		return kvp.Value != "";
	}

	public static string[] SplitWithEmpty(string arg, char split = ',')
	{
		List<string> list = new List<string>();
		list.AddRange(from s in arg.Split(split)
			select s.Trim());
		return list.ToArray();
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

	public static string Name(string arg)
	{
		return arg.Split(':')[0];
	}

	public static Vector3 VectorXZY(string arg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return VectorXZY(arg, Vector3.zero);
	}

	public static Vector3 VectorXZY(string arg, Vector3 defaultValue)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		string[] array = Split(arg);
		if (!TryDistanceAngle(array, out var vector))
		{
			return VectorXZY(array, 0, defaultValue);
		}
		return vector;
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
		zero.z = Float(args, index + 1, defaultValue.z);
		zero.y = Float(args, index + 2, defaultValue.y);
		return zero;
	}

	public static Vector2 VectorXY(string arg)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		Vector2 zero = Vector2.zero;
		string[] args = Split(arg);
		zero.x = Float(args, 0);
		zero.y = Float(args, 1);
		return zero;
	}

	public static Vector3 Scale(string args)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		return Scale(Split(args), 0);
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

	public static ValueRange<string> StringRange(string arg)
	{
		string[] array = Split(arg, removeEmpty: true, ';');
		if (array.Length > 1)
		{
			return new ValueRange<string>(array[0], array[1]);
		}
		return new ValueRange<string>(array[0], array[0]);
	}

	public static ValueRange<int> IntRange(string arg)
	{
		ValueRange<string> valueRange = StringRange(arg);
		return new ValueRange<int>(Int(valueRange.Min), Int(valueRange.Max));
	}

	public static ValueRange<float> FloatRange(string arg)
	{
		ValueRange<string> valueRange = StringRange(arg);
		return new ValueRange<float>(Float(valueRange.Min), Float(valueRange.Max));
	}

	public static ValueRange<long> LongRange(string arg)
	{
		ValueRange<string> valueRange = StringRange(arg);
		return new ValueRange<long>(Long(valueRange.Min, 0L), Long(valueRange.Max, 0L));
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

	public static Vector3? VectorXZYNull(string? arg)
	{
		if (arg != null)
		{
			return VectorXZYNull(Split(arg));
		}
		return null;
	}

	public static Vector3? VectorXZYNull(string[] args)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		if (TryDistanceAngle(args, out var vector))
		{
			return vector;
		}
		float? num = FloatNull(args, 0);
		float? num2 = FloatNull(args, 2);
		float? num3 = FloatNull(args, 1);
		if (!num.HasValue && !num2.HasValue && !num3.HasValue)
		{
			return null;
		}
		return new Vector3(num.GetValueOrDefault(), num2.GetValueOrDefault(), num3.GetValueOrDefault());
	}

	public static Quaternion? AngleYXZNull(string? arg)
	{
		if (arg != null)
		{
			return AngleYXZNull(Split(arg));
		}
		return null;
	}

	public static Quaternion? AngleYXZNull(string[] values)
	{
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		if (values.Length >= 4)
		{
			float? num = FloatNull(values, 0);
			float? num2 = FloatNull(values, 1);
			float? num3 = FloatNull(values, 2);
			float? num4 = FloatNull(values, 3);
			if (!num.HasValue && !num2.HasValue && !num3.HasValue && !num4.HasValue)
			{
				return null;
			}
			return new Quaternion(num.GetValueOrDefault(), num2.GetValueOrDefault(), num3.GetValueOrDefault(), num4.GetValueOrDefault());
		}
		float? num5 = FloatNull(values, 0);
		float? num6 = FloatNull(values, 1);
		float? num7 = FloatNull(values, 2);
		if (!num5.HasValue && !num6.HasValue && !num7.HasValue)
		{
			return null;
		}
		return Quaternion.Euler(new Vector3(num6.GetValueOrDefault(), num5.GetValueOrDefault(), num7.GetValueOrDefault()));
	}

	public static string String(string[] args, int index)
	{
		if (args.Length <= index)
		{
			return "";
		}
		return args[index];
	}

	public static int Hash(string[] args, int index)
	{
		if (args.Length <= index)
		{
			return 0;
		}
		return StringExtensionMethods.GetStableHashCode(args[index]);
	}

	public static bool Boolean(string[] args, int index)
	{
		if (args.Length > index)
		{
			return Boolean(args[index]);
		}
		return false;
	}

	public static bool Boolean(string arg)
	{
		return arg.ToLowerInvariant() == "true";
	}

	public static ZDOID ZdoId(string arg)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrWhiteSpace(arg))
		{
			return ZDOID.None;
		}
		string text = arg.Trim().Trim('(', ')', '[', ']', '{', '}');
		string[] array = (from value in text.Split(new char[2] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
			select value.Trim()).ToArray();
		if (array.Length < 2)
		{
			array = text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
		}
		if (array.Length < 2 || !long.TryParse(array[0], out var result) || !uint.TryParse(array[1], out var result2))
		{
			return ZDOID.None;
		}
		return new ZDOID(result, result2);
	}

	public static HitData Hit(ZDO? zdo, string arg)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		HitData val = new HitData
		{
			m_point = (zdo?.m_position ?? Vector3.zero)
		};
		string[] array = Split(arg, removeEmpty: true, ' ');
		for (int i = 0; i < array.Length; i++)
		{
			KeyValuePair<string, string> keyValuePair = Kvp(array[i], '=');
			string key = keyValuePair.Key;
			string value = keyValuePair.Value;
			if (key == "damage")
			{
				val.m_damage.m_damage = Float(value);
			}
			if (key == "blunt")
			{
				val.m_damage.m_blunt = Float(value);
			}
			if (key == "slash")
			{
				val.m_damage.m_slash = Float(value);
			}
			if (key == "pierce")
			{
				val.m_damage.m_pierce = Float(value);
			}
			if (key == "chop")
			{
				val.m_damage.m_chop = Float(value);
			}
			if (key == "pickaxe")
			{
				val.m_damage.m_pickaxe = Float(value);
			}
			if (key == "fire")
			{
				val.m_damage.m_fire = Float(value);
			}
			if (key == "frost")
			{
				val.m_damage.m_frost = Float(value);
			}
			if (key == "lightning")
			{
				val.m_damage.m_lightning = Float(value);
			}
			if (key == "poison")
			{
				val.m_damage.m_poison = Float(value);
			}
			if (key == "spirit")
			{
				val.m_damage.m_spirit = Float(value);
			}
			if (key == "tier")
			{
				val.m_toolTier = (short)Int(value);
			}
			if (key == "force")
			{
				val.m_pushForce = Float(value);
			}
			if (key == "backstab")
			{
				val.m_backstabBonus = Float(value);
			}
			if (key == "stagger")
			{
				val.m_staggerMultiplier = Float(value);
			}
			if (key == "dodge")
			{
				val.m_dodgeable = Boolean(value);
			}
			if (key == "block")
			{
				val.m_blockable = Boolean(value);
			}
			if (key == "dir")
			{
				val.m_dir = VectorXZY(value);
			}
			if (key == "ranged")
			{
				val.m_ranged = Boolean(value);
			}
			if (key == "pvp")
			{
				val.m_ignorePVP = Boolean(value);
			}
			if (key == "pos")
			{
				val.m_point = VectorXZY(value);
			}
			if (key == "status")
			{
				val.m_statusEffectHash = StringExtensionMethods.GetStableHashCode(value);
			}
			if (key == "attacker")
			{
				val.m_attacker = ZdoId(value);
			}
			if (key == "skill")
			{
				val.m_skillLevel = Float(value);
			}
			if (key == "level")
			{
				val.m_itemLevel = (short)Int(value);
			}
			if (key == "world")
			{
				val.m_itemWorldLevel = (byte)Int(value);
			}
			if (key == "type")
			{
				val.m_hitType = (HitType)(Enum.TryParse<HitType>(value, ignoreCase: true, out HitType result) ? ((int)result) : 0);
			}
			if (key == "spot")
			{
				val.m_weakSpot = (short)Int(value);
			}
		}
		return val;
	}

	public static int EnumMessage(string arg)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected I4, but got Unknown
		if (!Enum.TryParse<MessageType>(arg, ignoreCase: true, out MessageType result))
		{
			return Int(arg, 2);
		}
		return (int)result;
	}

	public static int EnumReason(string arg)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected I4, but got Unknown
		if (!Enum.TryParse<AggravatedReason>(arg, ignoreCase: true, out AggravatedReason result))
		{
			return Int(arg);
		}
		return (int)result;
	}

	public static int EnumTrap(string arg)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected I4, but got Unknown
		if (!Enum.TryParse<TrapState>(arg, ignoreCase: true, out TrapState result))
		{
			return Int(arg);
		}
		return (int)result;
	}

	public static int EnumDamageText(string arg)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected I4, but got Unknown
		if (!Enum.TryParse<TextType>(arg, ignoreCase: true, out TextType result))
		{
			return Int(arg);
		}
		return (int)result;
	}

	public static int EnumTerrainPaint(string arg)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected I4, but got Unknown
		if (!Enum.TryParse<PaintType>(arg, ignoreCase: true, out PaintType result))
		{
			return Int(arg);
		}
		return (int)result;
	}

	public static Color? Color(string arg, float defaultAlpha)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		string key = arg.ToLowerInvariant();
		if (Paints.TryGetValue(key, out var value))
		{
			return value;
		}
		string[] array = Split(arg);
		if (array.Length < 3)
		{
			return null;
		}
		return new Color(Float(array[0]), Float(array[1]), Float(array[2]), (array.Length > 3) ? Float(array[3]) : defaultAlpha);
	}
}
