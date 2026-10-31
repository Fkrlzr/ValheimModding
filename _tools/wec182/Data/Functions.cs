using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Common;
using UnityEngine;

namespace Data;

public class Functions
{
	[CompilerGenerated]
	private string _003Cprefab_003EP;

	[CompilerGenerated]
	private string[] _003Cargs_003EP;

	[CompilerGenerated]
	private Vector3 _003Cpos_003EP;

	protected const char Separator = '_';

	public static Func<string, string?> ExecuteCode = (string key) => (string?)null;

	public static Func<string, string, string?> ExecuteCodeWithValue = (string key, string value) => (string?)null;

	private readonly double time;

	public int Amount;

	private Dictionary<int, List<ZDOID>>? pokeTargets;

	private Dictionary<int, Dictionary<string, int>>? pokeCounts;

	private string HandleRank(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length < 2)
		{
			return defaultValue;
		}
		if (!Parse.TryFloat(array[0], out var result))
		{
			return defaultValue;
		}
		List<float> list = (from v in array.Skip(1)
			select Parse.Float(v, float.MaxValue)).ToList();
		int num = 0;
		foreach (float item in list)
		{
			if (item > result)
			{
				num++;
			}
		}
		return num.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleSmall(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length < 2)
		{
			return defaultValue;
		}
		if (!Parse.TryInt(array[0], out var result))
		{
			return defaultValue;
		}
		List<float> list = (from v in array.Skip(1)
			select Parse.Float(v, float.MaxValue)).ToList();
		list.Sort();
		if (result < 1)
		{
			return list[0].ToString(CultureInfo.InvariantCulture);
		}
		if (result > list.Count)
		{
			return list[list.Count - 1].ToString(CultureInfo.InvariantCulture);
		}
		return list[result - 1].ToString(CultureInfo.InvariantCulture);
	}

	private string HandleLarge(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length < 2)
		{
			return defaultValue;
		}
		if (!Parse.TryInt(array[0], out var result))
		{
			return defaultValue;
		}
		List<float> list = (from v in array.Skip(1)
			select Parse.Float(v, float.MinValue)).ToList();
		list.Sort();
		if (result < 1)
		{
			return list[list.Count - 1].ToString(CultureInfo.InvariantCulture);
		}
		if (result > list.Count)
		{
			return list[0].ToString(CultureInfo.InvariantCulture);
		}
		return list[list.Count - result].ToString(CultureInfo.InvariantCulture);
	}

	private string HandleEqual(string value, string defaultValue)
	{
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, '_');
		if (keyValuePair.Value == "")
		{
			return defaultValue;
		}
		if (Parse.TryFloat(keyValuePair.Key, out var result) && Parse.TryFloat(keyValuePair.Value, out var result2))
		{
			if (!(Math.Abs(result - result2) < float.Epsilon))
			{
				return "false";
			}
			return "true";
		}
		if (!string.Equals(keyValuePair.Key, keyValuePair.Value, StringComparison.OrdinalIgnoreCase))
		{
			return "false";
		}
		return "true";
	}

	private string HandleNotEqual(string value, string defaultValue)
	{
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, '_');
		if (keyValuePair.Value == "")
		{
			return defaultValue;
		}
		if (Parse.TryFloat(keyValuePair.Key, out var result) && Parse.TryFloat(keyValuePair.Value, out var result2))
		{
			if (!(Math.Abs(result - result2) >= float.Epsilon))
			{
				return "false";
			}
			return "true";
		}
		if (string.Equals(keyValuePair.Key, keyValuePair.Value, StringComparison.OrdinalIgnoreCase))
		{
			return "false";
		}
		return "true";
	}

	private string HandleGreater(string value, string defaultValue)
	{
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, '_');
		if (keyValuePair.Value == "" || !Parse.TryFloat(keyValuePair.Key, out var result) || !Parse.TryFloat(keyValuePair.Value, out var result2))
		{
			return defaultValue;
		}
		if (!(result > result2))
		{
			return "false";
		}
		return "true";
	}

	private string HandleGreaterOrEqual(string value, string defaultValue)
	{
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, '_');
		if (keyValuePair.Value == "" || !Parse.TryFloat(keyValuePair.Key, out var result) || !Parse.TryFloat(keyValuePair.Value, out var result2))
		{
			return defaultValue;
		}
		if (!(result >= result2))
		{
			return "false";
		}
		return "true";
	}

	private string HandleLess(string value, string defaultValue)
	{
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, '_');
		if (keyValuePair.Value == "" || !Parse.TryFloat(keyValuePair.Key, out var result) || !Parse.TryFloat(keyValuePair.Value, out var result2))
		{
			return defaultValue;
		}
		if (!(result < result2))
		{
			return "false";
		}
		return "true";
	}

	private string HandleLessOrEqual(string value, string defaultValue)
	{
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, '_');
		if (keyValuePair.Value == "" || !Parse.TryFloat(keyValuePair.Key, out var result) || !Parse.TryFloat(keyValuePair.Value, out var result2))
		{
			return defaultValue;
		}
		if (!(result <= result2))
		{
			return "false";
		}
		return "true";
	}

	private string HandleEven(string value, string defaultValue)
	{
		if (!Parse.TryInt(value, out var result))
		{
			return defaultValue;
		}
		if (result % 2 != 0)
		{
			return "false";
		}
		return "true";
	}

	private string HandleOdd(string value, string defaultValue)
	{
		if (!Parse.TryInt(value, out var result))
		{
			return defaultValue;
		}
		if (result % 2 == 0)
		{
			return "false";
		}
		return "true";
	}

	private string HandleFindUpper(string value, string defaultValue)
	{
		if (string.IsNullOrEmpty(value))
		{
			return defaultValue;
		}
		List<char> list = new List<char>();
		list.AddRange(value.Where(char.IsUpper));
		return new string(list.ToArray());
	}

	private string HandleFindLower(string value, string defaultValue)
	{
		if (string.IsNullOrEmpty(value))
		{
			return defaultValue;
		}
		List<char> list = new List<char>();
		list.AddRange(value.Where(char.IsLower));
		return new string(list.ToArray());
	}

	public Functions(string prefab, string[] args, Vector3 pos)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		_003Cprefab_003EP = prefab;
		_003Cargs_003EP = args;
		_003Cpos_003EP = pos;
		time = ZNet.instance.GetTimeSeconds();
		base._002Ector();
	}

	private string LoadStored(string key, string defaultValue)
	{
		return null ?? defaultValue;
	}

	private string IncrementStored(string key, long amount)
	{
		return null ?? "";
	}

	public void SetPokeTargets(int slot, List<ZDOID> targets)
	{
		if (pokeTargets == null)
		{
			pokeTargets = new Dictionary<int, List<ZDOID>>();
		}
		if (!pokeTargets.TryGetValue(slot, out List<ZDOID> value))
		{
			value = (pokeTargets[slot] = new List<ZDOID>());
		}
		value.AddRange(targets);
		pokeCounts = null;
		Amount += targets.Count;
	}

	public string Replace(string str)
	{
		return Replace(str, preventInjections: false, allValues: false);
	}

	public string Replace(string str, bool preventInjections, bool allValues)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == '<')
			{
				if (num == 0)
				{
					stringBuilder.Append(str.Substring(num2, i - num2));
					num2 = i;
				}
				num++;
			}
			if (str[i] != '>')
			{
				continue;
			}
			if (num == 1)
			{
				string str2 = str.Substring(num2, i - num2 + 1);
				string text = ResolveFunctions(str2, allValues);
				if (preventInjections && text.Contains(";"))
				{
					text = text.Replace(";", ",");
				}
				stringBuilder.Append(text);
				num2 = i + 1;
			}
			if (num > 0)
			{
				num--;
			}
		}
		if (num2 < str.Length)
		{
			stringBuilder.Append(str.Substring(num2));
		}
		return stringBuilder.ToString();
	}

	private string ResolveFunctions(string str, bool allValues)
	{
		int num;
		for (num = 0; num < str.Length; num++)
		{
			int num2 = str.IndexOf(">", num);
			if (num2 == -1)
			{
				break;
			}
			num = num2;
			int num3 = str.LastIndexOf("<", num2);
			if (num3 != -1)
			{
				int num4 = num2 - num3 + 1;
				if (TryReplaceFunction(str.Substring(num3, num4), allValues, out string resolved))
				{
					str = str.Remove(num3, num4);
					str = str.Insert(num3, resolved);
					num = num3 - 1;
				}
				else
				{
					num = num2;
				}
			}
		}
		return str;
	}

	private bool TryReplaceFunction(string rawKey, bool allValues, out string? resolved)
	{
		string text = rawKey.Substring(1, rawKey.Length - 2);
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(text, '=');
		string text2 = keyValuePair.Value;
		if (text2.All((char c) => c == '='))
		{
			text2 = "";
		}
		else
		{
			text = keyValuePair.Key;
		}
		resolved = GetFunction(text, text2);
		if (resolved == null)
		{
			resolved = (allValues ? ResolveConditionValue(rawKey) : ResolveValue(rawKey));
		}
		return resolved != rawKey;
	}

	protected virtual string? GetFunction(string key, string defaultValue)
	{
		string text = null;
		if (text != null)
		{
			return text;
		}
		text = ExecuteCode(key);
		if (text != null)
		{
			return text;
		}
		text = GetGeneralFunction(key, defaultValue);
		if (text != null)
		{
			return text;
		}
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(key, '_');
		if (keyValuePair.Value == "")
		{
			return null;
		}
		key = keyValuePair.Key;
		string value = keyValuePair.Value;
		if (text != null)
		{
			return text;
		}
		text = ExecuteCodeWithValue(key, value);
		if (text != null)
		{
			return text;
		}
		return GetValueFunction(key, value, defaultValue);
	}

	private string? GetGeneralFunction(string key, string defaultValue)
	{
		return key switch
		{
			"prefab" => _003Cprefab_003EP, 
			"safeprefab" => _003Cprefab_003EP.Replace('_', '-'), 
			"par" => string.Join(" ", _003Cargs_003EP), 
			"par0" => GetArg(0, defaultValue), 
			"par1" => GetArg(1, defaultValue), 
			"par2" => GetArg(2, defaultValue), 
			"par3" => GetArg(3, defaultValue), 
			"par4" => GetArg(4, defaultValue), 
			"par5" => GetArg(5, defaultValue), 
			"par6" => GetArg(6, defaultValue), 
			"par7" => GetArg(7, defaultValue), 
			"par8" => GetArg(8, defaultValue), 
			"par9" => GetArg(9, defaultValue), 
			"day" => EnvMan.instance.GetDay(time).ToString(), 
			"ticks" => ((long)(time * 10000000.0)).ToString(), 
			"x" => Formatting.Format(_003Cpos_003EP.x), 
			"y" => Formatting.Format(_003Cpos_003EP.y), 
			"z" => Formatting.Format(_003Cpos_003EP.z), 
			"snap" => Formatting.Format(WorldGenerator.instance.GetHeight(_003Cpos_003EP.x, _003Cpos_003EP.z)), 
			"pokecount" => (_003Cargs_003EP.Length < 2) ? Amount.ToString() : null, 
			"time" => Formatting.Format(time), 
			"realtime" => DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), 
			_ => null, 
		};
	}

	protected virtual string? GetValueFunction(string key, string value, string defaultValue)
	{
		float result;
		float result2;
		float result3;
		float result4;
		float result5;
		float radians;
		float radians2;
		float radians3;
		float result6;
		float result7;
		KeyValuePair<string, string> kvp;
		float result8;
		float result9;
		float result10;
		KeyValuePair<string, string> kvp2;
		float result11;
		float result12;
		KeyValuePair<string, string> kvp3;
		int result13;
		int result14;
		KeyValuePair<string, string> kvp4;
		float result15;
		float result16;
		KeyValuePair<string, string> kvp5;
		int result17;
		int result18;
		int result19;
		int result20;
		int result21;
		string text = default(string);
		return key switch
		{
			"pokecount" => GetPokeCount(value, defaultValue), 
			"sqrt" => Parse.TryFloat(value, out result) ? Mathf.Sqrt(result).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"round" => Parse.TryFloat(value, out result2) ? Mathf.Round(result2).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"ceil" => Parse.TryFloat(value, out result3) ? Mathf.Ceil(result3).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"floor" => Parse.TryFloat(value, out result4) ? Mathf.Floor(result4).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"abs" => Parse.TryFloat(value, out result5) ? Mathf.Abs(result5).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"sin" => Parse.TryAngleRadians(value, out radians) ? Mathf.Sin(radians).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"cos" => Parse.TryAngleRadians(value, out radians2) ? Mathf.Cos(radians2).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"tan" => Parse.TryAngleRadians(value, out radians3) ? Mathf.Tan(radians3).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"asin" => Parse.TryFloat(value, out result6) ? Mathf.Asin(result6).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"acos" => Parse.TryFloat(value, out result7) ? Mathf.Acos(result7).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"rad2deg" => Rad2Deg(value) ?? defaultValue, 
			"deg2rad" => Deg2Rad(value) ?? defaultValue, 
			"rad2vec" => Rad2Vec(value) ?? defaultValue, 
			"deg2vec" => Deg2Vec(value) ?? defaultValue, 
			"vec2deg" => Vec2Deg(value) ?? defaultValue, 
			"vec2rad" => Vec2Rad(value) ?? defaultValue, 
			"angle" => HandleAngle(value, defaultValue), 
			"distance" => HandleDistance(value, defaultValue), 
			"dot" => HandleDot(value, defaultValue), 
			"cross" => HandleCross(value, defaultValue), 
			"normalize" => HandleNormalize(value, defaultValue), 
			"magnitude" => HandleMagnitude(value, defaultValue), 
			"sqrmagnitude" => HandleSqrMagnitude(value, defaultValue), 
			"project" => HandleProject(value, defaultValue), 
			"reflect" => HandleReflect(value, defaultValue), 
			"lerp" => HandleLerp(value, defaultValue), 
			"vecx" => HandleVecX(value, defaultValue), 
			"vecy" => HandleVecY(value, defaultValue), 
			"vecz" => HandleVecZ(value, defaultValue), 
			"atan" => Atan(value, defaultValue), 
			"pow" => (Parse.TryKvp(value, out kvp, '_') && Parse.TryFloat(kvp.Key, out result8) && Parse.TryFloat(kvp.Value, out result9)) ? Mathf.Pow(result8, result9).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"log" => Loga(value, defaultValue), 
			"exp" => Parse.TryFloat(value, out result10) ? Mathf.Exp(result10).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"min" => HandleMin(value, defaultValue), 
			"max" => HandleMax(value, defaultValue), 
			"add" => HandleAdd(value, defaultValue), 
			"sub" => HandleSub(value, defaultValue), 
			"mul" => HandleMul(value, defaultValue), 
			"div" => HandleDiv(value, defaultValue), 
			"mod" => HandleMod(value, defaultValue), 
			"iter" => HandleIter(value, defaultValue), 
			"iter2" => HandleIter2(value, defaultValue), 
			"addlong" => HandleAddLong(value, defaultValue), 
			"sublong" => HandleSubLong(value, defaultValue), 
			"mullong" => HandleMulLong(value, defaultValue), 
			"divlong" => HandleDivLong(value, defaultValue), 
			"modlong" => HandleModLong(value, defaultValue), 
			"randf" => (Parse.TryKvp(value, out kvp2, '_') && Parse.TryFloat(kvp2.Key, out result11) && Parse.TryFloat(kvp2.Value, out result12)) ? Random.Range(result11, result12).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"randi" => (Parse.TryKvp(value, out kvp3, '_') && Parse.TryInt(kvp3.Key, out result13) && Parse.TryInt(kvp3.Value, out result14)) ? Random.Range(result13, result14).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"random" => HandleRandom(value, defaultValue), 
			"randomfloat" => (Parse.TryKvp(value, out kvp4, '_') && Parse.TryFloat(kvp4.Key, out result15) && Parse.TryFloat(kvp4.Value, out result16)) ? Random.Range(result15, result16).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"randomint" => (Parse.TryKvp(value, out kvp5, '_') && Parse.TryInt(kvp5.Key, out result17) && Parse.TryInt(kvp5.Value, out result18)) ? Random.Range(result17, result18).ToString(CultureInfo.InvariantCulture) : defaultValue, 
			"hashof" => ZdoHelper.Hash(value).ToString(), 
			"textof" => Parse.TryInt(value, out result19) ? ZdoHelper.ReverseHash(result19) : defaultValue, 
			"len" => value.Length.ToString(CultureInfo.InvariantCulture), 
			"lower" => value.ToLowerInvariant(), 
			"upper" => value.ToUpperInvariant(), 
			"trim" => value.Trim(), 
			"left" => HandleLeft(value, defaultValue), 
			"right" => HandleRight(value, defaultValue), 
			"mid" => HandleMid(value, defaultValue), 
			"proper" => HandleProper(value, defaultValue), 
			"search" => HandleSearch(value, defaultValue), 
			"calcf" => Calculator.EvaluateFloat(value)?.ToString(CultureInfo.InvariantCulture) ?? defaultValue, 
			"calci" => Calculator.EvaluateInt(value)?.ToString(CultureInfo.InvariantCulture) ?? defaultValue, 
			"calcfloat" => Calculator.EvaluateFloat(value)?.ToString(CultureInfo.InvariantCulture) ?? defaultValue, 
			"calcint" => Calculator.EvaluateInt(value)?.ToString(CultureInfo.InvariantCulture) ?? defaultValue, 
			"calclong" => Calculator.EvaluateLong(value)?.ToString(CultureInfo.InvariantCulture) ?? defaultValue, 
			"par" => Parse.TryInt(value, out result20) ? GetArg(result20, defaultValue) : defaultValue, 
			"rest" => Parse.TryInt(value, out result21) ? GetRest(result21, defaultValue) : defaultValue, 
			"load" => LoadStored(value, defaultValue), 
			"save" => SetValue(value), 
			"save++" => IncrementStored(value, 1L), 
			"save--" => IncrementStored(value, -1L), 
			"clear" => RemoveValue(value), 
			"rank" => HandleRank(value, defaultValue), 
			"small" => HandleSmall(value, defaultValue), 
			"large" => HandleLarge(value, defaultValue), 
			"eq" => HandleEqual(value, defaultValue), 
			"ne" => HandleNotEqual(value, defaultValue), 
			"gt" => HandleGreater(value, defaultValue), 
			"ge" => HandleGreaterOrEqual(value, defaultValue), 
			"lt" => HandleLess(value, defaultValue), 
			"le" => HandleLessOrEqual(value, defaultValue), 
			"even" => HandleEven(value, defaultValue), 
			"odd" => HandleOdd(value, defaultValue), 
			"findupper" => HandleFindUpper(value, defaultValue), 
			"findlower" => HandleFindLower(value, defaultValue), 
			"time" => HandleTime(value), 
			"realtime" => HandleRealtime(value), 
			"key" => LoadStored(value, defaultValue), 
			"globalkey" => ZoneSystem.instance.GetGlobalKey(value, ref text) ? text.ToString() : defaultValue, 
			_ => null, 
		};
	}

	private string GetPokeCount(string value, string defaultValue)
	{
		if (pokeTargets != null)
		{
			return GetPokeCount(GetPokeCounts(), value, defaultValue);
		}
		return defaultValue;
	}

	private static string GetPokeCount(Dictionary<int, Dictionary<string, int>> slots, string value, string defaultValue)
	{
		return GetCount(slots, (from c in slots.Values.SelectMany((Dictionary<string, int> s) => s)
			group c by c.Key).ToDictionary((IGrouping<string, KeyValuePair<string, int>> g) => g.Key, (IGrouping<string, KeyValuePair<string, int>> g) => g.Sum((KeyValuePair<string, int> c) => c.Value)), value, defaultValue);
	}

	protected static string GetCount(Dictionary<int, Dictionary<string, int>> slots, Dictionary<string, int> counts, string value, string defaultValue)
	{
		string value2 = value;
		if (value2 == "")
		{
			return defaultValue;
		}
		if (Parse.TryInt(value2, out var result))
		{
			if (!slots.TryGetValue(result, out Dictionary<string, int> value3))
			{
				return defaultValue;
			}
			return value3.Values.Sum().ToString(CultureInfo.InvariantCulture);
		}
		if (counts.TryGetValue(value2, out var value4))
		{
			return value4.ToString(CultureInfo.InvariantCulture);
		}
		if (ValueGroups.TryGet(value2, out List<string> values))
		{
			return SumCounts(counts, values);
		}
		if (!Wildcard.IsPattern(value2))
		{
			return defaultValue;
		}
		return counts.Where<KeyValuePair<string, int>>((KeyValuePair<string, int> kv) => Wildcard.Match(kv.Key, value2)).Sum((KeyValuePair<string, int> kv) => kv.Value).ToString(CultureInfo.InvariantCulture);
	}

	protected static string SumCounts(Dictionary<string, int> counts, List<string> members)
	{
		List<string> members2 = members;
		return counts.Where<KeyValuePair<string, int>>((KeyValuePair<string, int> kv) => members2.Contains(kv.Key)).Sum((KeyValuePair<string, int> kv) => kv.Value).ToString(CultureInfo.InvariantCulture);
	}

	private Dictionary<int, Dictionary<string, int>> GetPokeCounts()
	{
		return pokeCounts ?? (pokeCounts = pokeTargets.ToDictionary<KeyValuePair<int, List<ZDOID>>, int, Dictionary<string, int>>((KeyValuePair<int, List<ZDOID>> kv) => kv.Key, (KeyValuePair<int, List<ZDOID>> kv) => GetPokeCounts(kv.Value)));
	}

	private static Dictionary<string, int> GetPokeCounts(List<ZDOID> targets)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (ZDOID target in targets)
		{
			ZDO zDO = ZDOMan.instance.GetZDO(target);
			object obj;
			if (zDO != null)
			{
				GameObject prefab = ZNetScene.instance.GetPrefab(zDO.GetPrefab());
				obj = ((prefab != null) ? ((Object)prefab).name : null);
			}
			else
			{
				obj = null;
			}
			string text = (string)obj;
			if (text != null)
			{
				dictionary.TryGetValue(text, out var value);
				dictionary[text] = value + 1;
			}
		}
		return dictionary;
	}

	private string SetValue(string value)
	{
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, '_');
		if (keyValuePair.Value == "")
		{
			return "";
		}
		return keyValuePair.Value;
	}

	private string RemoveValue(string value)
	{
		return "";
	}

	private string GetRest(int index, string defaultValue = "")
	{
		if (index < 0 || index >= _003Cargs_003EP.Length)
		{
			return defaultValue;
		}
		return string.Join(" ", _003Cargs_003EP, index, _003Cargs_003EP.Length - index);
	}

	private string GetArg(int index, string defaultValue = "")
	{
		if (_003Cargs_003EP.Length > index && !(_003Cargs_003EP[index] == ""))
		{
			return _003Cargs_003EP[index];
		}
		return defaultValue;
	}

	private static string ResolveValue(string value)
	{
		if (!value.StartsWith("<", StringComparison.OrdinalIgnoreCase))
		{
			return value;
		}
		if (!value.EndsWith(">", StringComparison.OrdinalIgnoreCase))
		{
			return value;
		}
		if (TryGetValueFromGroup(value.Substring(1, value.Length - 2), out string value2))
		{
			return value2;
		}
		return value;
	}

	private static string ResolveConditionValue(string value)
	{
		if (!value.StartsWith("<", StringComparison.OrdinalIgnoreCase))
		{
			return value;
		}
		if (!value.EndsWith(">", StringComparison.OrdinalIgnoreCase))
		{
			return value;
		}
		if (TryGetValuesFromGroup(value.Substring(1, value.Length - 2), out List<string> values))
		{
			return string.Join(",", values);
		}
		return value;
	}

	private static bool TryGetValueFromGroup(string group, out string value)
	{
		if (!ValueGroups.TryGetRandom(group, out value))
		{
			return false;
		}
		value = ResolveValue(value);
		return true;
	}

	private static bool TryGetValuesFromGroup(string group, out List<string> values)
	{
		values = new List<string>();
		HashSet<int> handled = new HashSet<int>();
		AddGroupValues(group, values, handled);
		return values.Count > 0;
	}

	private static void AddGroupValues(string group, List<string> values, HashSet<int> handled)
	{
		if (!handled.Add(ValueGroups.Hash(group)) || !ValueGroups.TryGet(group, out List<string> values2))
		{
			return;
		}
		foreach (string item in values2)
		{
			if (item.StartsWith("<", StringComparison.OrdinalIgnoreCase) && item.EndsWith(">", StringComparison.OrdinalIgnoreCase))
			{
				AddGroupValues(item.Substring(1, item.Length - 2), values, handled);
			}
			else
			{
				values.Add(item);
			}
		}
	}

	private string HandleMin(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length == 0)
		{
			return defaultValue;
		}
		return array.Min((string v) => Parse.Float(v, float.MaxValue)).ToString(CultureInfo.InvariantCulture);
	}

	private string HandleMax(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length == 0)
		{
			return defaultValue;
		}
		return array.Max((string v) => Parse.Float(v, float.MinValue)).ToString(CultureInfo.InvariantCulture);
	}

	private string HandleIter(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length < 4)
		{
			return defaultValue;
		}
		string operation = array[0];
		if (!Parse.TryInt(array[1], out var result))
		{
			return defaultValue;
		}
		if (!Parse.TryInt(array[2], out var result2))
		{
			return defaultValue;
		}
		string template = BuildIteratorTemplate(string.Join('_'.ToString(), array.Skip(3)), defaultValue);
		return BuildIteratorReduceExpression(operation, template, result, result2, null, null, defaultValue);
	}

	private string HandleIter2(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length < 6)
		{
			return defaultValue;
		}
		string operation = array[0];
		if (!Parse.TryInt(array[1], out var result))
		{
			return defaultValue;
		}
		if (!Parse.TryInt(array[2], out var result2))
		{
			return defaultValue;
		}
		if (!Parse.TryInt(array[3], out var result3))
		{
			return defaultValue;
		}
		if (!Parse.TryInt(array[4], out var result4))
		{
			return defaultValue;
		}
		string template = BuildIteratorTemplate(string.Join('_'.ToString(), array.Skip(5)), defaultValue);
		return BuildIteratorReduceExpression(operation, template, result, result2, result3, result4, defaultValue);
	}

	private static string BuildIteratorTemplate(string template, string defaultValue)
	{
		if (defaultValue == "")
		{
			return template;
		}
		if (template.Contains("="))
		{
			return template;
		}
		return template + "=" + defaultValue;
	}

	private string BuildIteratorReduceExpression(string operation, string template, int minI, int maxI, int? minJ, int? maxJ, string defaultValue)
	{
		if (operation == "" || template == "")
		{
			return defaultValue;
		}
		if (minI > maxI)
		{
			return defaultValue;
		}
		if (minJ.HasValue && maxJ.HasValue && minJ.Value > maxJ.Value)
		{
			return defaultValue;
		}
		List<string> list = new List<string>();
		if (minJ.HasValue && maxJ.HasValue)
		{
			for (int i = minJ.Value; i <= maxJ.Value; i++)
			{
				for (int j = minI; j <= maxI; j++)
				{
					list.Add(RenderIteratorTemplate(template, j, i));
				}
			}
		}
		else
		{
			for (int k = minI; k <= maxI; k++)
			{
				list.Add(RenderIteratorTemplate(template, k, null));
			}
		}
		if (list.Count == 0)
		{
			return defaultValue;
		}
		if (list.Count == 1)
		{
			return list[0];
		}
		return "<" + operation + "_" + string.Join('_'.ToString(), list) + ">";
	}

	private static string RenderIteratorTemplate(string template, int i, int? j)
	{
		string text = ReplaceIteratorToken(template, "i", i.ToString(CultureInfo.InvariantCulture));
		if (j.HasValue)
		{
			text = ReplaceIteratorToken(text, "j", j.Value.ToString(CultureInfo.InvariantCulture));
		}
		if (text.StartsWith("<", StringComparison.Ordinal) && text.EndsWith(">", StringComparison.Ordinal))
		{
			return text;
		}
		if (IsIteratorLiteral(text))
		{
			return text;
		}
		return "<" + text + ">";
	}

	private static bool IsIteratorLiteral(string value)
	{
		float result;
		return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
	}

	private static string ReplaceIteratorToken(string input, string token, string replacement)
	{
		if (input == "")
		{
			return input;
		}
		int length = token.Length;
		StringBuilder stringBuilder = new StringBuilder(input.Length);
		int num = 0;
		while (num < input.Length)
		{
			if (num <= input.Length - length && string.CompareOrdinal(input, num, token, 0, length) == 0 && (num == 0 || !char.IsLetterOrDigit(input[num - 1])) && (num + length == input.Length || !char.IsLetterOrDigit(input[num + length])))
			{
				stringBuilder.Append(replacement);
				num += length;
			}
			else
			{
				stringBuilder.Append(input[num]);
				num++;
			}
		}
		return stringBuilder.ToString();
	}

	private string Atan(string value, string defaultValue)
	{
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, '_');
		if (!Parse.TryFloat(keyValuePair.Key, out var result))
		{
			return defaultValue;
		}
		if (keyValuePair.Value == "")
		{
			return Mathf.Atan(result).ToString(CultureInfo.InvariantCulture);
		}
		if (!Parse.TryFloat(keyValuePair.Value, out var result2))
		{
			return defaultValue;
		}
		return Mathf.Atan2(result, result2).ToString(CultureInfo.InvariantCulture);
	}

	private string Loga(string value, string defaultValue)
	{
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, '_');
		if (!Parse.TryFloat(keyValuePair.Key, out var result))
		{
			return defaultValue;
		}
		if (keyValuePair.Value == "")
		{
			return Mathf.Log(result).ToString(CultureInfo.InvariantCulture);
		}
		if (!Parse.TryFloat(keyValuePair.Value, out var result2))
		{
			return defaultValue;
		}
		return Mathf.Log(result, result2).ToString(CultureInfo.InvariantCulture);
	}

	private string HandleAdd(string value, string defaultValue)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		string[] array = value.Split('_');
		if (array.Length == 0)
		{
			return defaultValue;
		}
		if (TryGetStrictVectorOperands(array, out Vector3?[] vectors))
		{
			Vector3 val = Vector3.zero;
			for (int i = 0; i < array.Length; i++)
			{
				if (!TryGetVectorMathOperand(array[i], vectors[i], out var operand))
				{
					return defaultValue;
				}
				val += operand;
			}
			return Formatting.FormatPos(val);
		}
		float num = 0f;
		string[] array2 = array;
		foreach (string arg in array2)
		{
			num += Parse.Float(arg);
		}
		return num.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleSub(string value, string defaultValue)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		string[] array = value.Split('_');
		if (array.Length == 0)
		{
			return defaultValue;
		}
		if (TryGetStrictVectorOperands(array, out Vector3?[] vectors))
		{
			if (!TryGetVectorMathOperand(array[0], vectors[0], out var operand))
			{
				return defaultValue;
			}
			for (int i = 1; i < array.Length; i++)
			{
				if (!TryGetVectorMathOperand(array[i], vectors[i], out var operand2))
				{
					return defaultValue;
				}
				operand -= operand2;
			}
			return Formatting.FormatPos(operand);
		}
		float num = Parse.Float(array[0]);
		for (int j = 1; j < array.Length; j++)
		{
			num -= Parse.Float(array[j]);
		}
		return num.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleMul(string value, string defaultValue)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		string[] array = value.Split('_');
		if (array.Length == 0)
		{
			return defaultValue;
		}
		if (TryGetStrictVectorOperands(array, out Vector3?[] vectors))
		{
			if (!TryGetVectorMathOperand(array[0], vectors[0], out var operand))
			{
				return defaultValue;
			}
			for (int i = 1; i < array.Length; i++)
			{
				if (vectors[i].HasValue)
				{
					Vector3 value2 = vectors[i].Value;
					operand = Vector3.Scale(operand, value2);
					continue;
				}
				float? num = Calculator.EvaluateFloat(array[i]);
				if (!num.HasValue)
				{
					return defaultValue;
				}
				operand *= num.Value;
			}
			return Formatting.FormatPos(operand);
		}
		float num2 = 1f;
		string[] array2 = array;
		foreach (string arg in array2)
		{
			num2 *= Parse.Float(arg, 1f);
		}
		return num2.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleDiv(string value, string defaultValue)
	{
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		string[] array = value.Split('_');
		if (array.Length == 0)
		{
			return defaultValue;
		}
		if (TryGetStrictVectorOperands(array, out Vector3?[] vectors))
		{
			if (!TryGetVectorMathOperand(array[0], vectors[0], out var operand))
			{
				return defaultValue;
			}
			for (int i = 1; i < array.Length; i++)
			{
				if (vectors[i].HasValue)
				{
					Vector3 value2 = vectors[i].Value;
					if (value2.x == 0f || value2.y == 0f || value2.z == 0f)
					{
						return defaultValue;
					}
					((Vector3)(ref operand))._002Ector(operand.x / value2.x, operand.y / value2.y, operand.z / value2.z);
				}
				else
				{
					float? num = Calculator.EvaluateFloat(array[i]);
					if (!num.HasValue || num.Value == 0f)
					{
						return defaultValue;
					}
					operand /= num.Value;
				}
			}
			return Formatting.FormatPos(operand);
		}
		float num2 = Parse.Float(array[0]);
		for (int j = 1; j < array.Length; j++)
		{
			float num3 = Parse.Float(array[j], 1f);
			if (num3 == 0f)
			{
				return defaultValue;
			}
			num2 /= num3;
		}
		return num2.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleMod(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length == 0)
		{
			return defaultValue;
		}
		float num = Parse.Float(array[0]);
		for (int i = 1; i < array.Length; i++)
		{
			float num2 = Parse.Float(array[i], 1f);
			if (num2 == 0f)
			{
				return defaultValue;
			}
			num %= num2;
		}
		return num.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleAddLong(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length == 0)
		{
			return defaultValue;
		}
		long num = 0L;
		string[] array2 = array;
		foreach (string arg in array2)
		{
			num += Parse.Long(arg, 0L);
		}
		return num.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleSubLong(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length == 0)
		{
			return defaultValue;
		}
		long num = Parse.Long(array[0], 0L);
		for (int i = 1; i < array.Length; i++)
		{
			num -= Parse.Long(array[i], 0L);
		}
		return num.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleMulLong(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length == 0)
		{
			return defaultValue;
		}
		long num = 1L;
		string[] array2 = array;
		foreach (string arg in array2)
		{
			num *= Parse.Long(arg, 1L);
		}
		return num.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleDivLong(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length == 0)
		{
			return defaultValue;
		}
		long num = Parse.Long(array[0], 0L);
		for (int i = 1; i < array.Length; i++)
		{
			long num2 = Parse.Long(array[i], 1L);
			if (num2 == 0L)
			{
				return defaultValue;
			}
			num /= num2;
		}
		return num.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleModLong(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length == 0)
		{
			return defaultValue;
		}
		long num = Parse.Long(array[0], 0L);
		for (int i = 1; i < array.Length; i++)
		{
			long num2 = Parse.Long(array[i], 1L);
			if (num2 == 0L)
			{
				return defaultValue;
			}
			num %= num2;
		}
		return num.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleRandom(string value, string defaultValue)
	{
		if (!Parse.TryKvp(value, out KeyValuePair<string, string> kvp, '_'))
		{
			return defaultValue;
		}
		if (HasFractionalMarker(kvp.Key) || HasFractionalMarker(kvp.Value))
		{
			if (!Parse.TryFloat(kvp.Key, out var result) || !Parse.TryFloat(kvp.Value, out var result2))
			{
				return defaultValue;
			}
			return Random.Range(result, result2).ToString(CultureInfo.InvariantCulture);
		}
		if (!Parse.TryInt(kvp.Key, out var result3) || !Parse.TryInt(kvp.Value, out var result4))
		{
			return defaultValue;
		}
		return Random.Range(result3, result4).ToString(CultureInfo.InvariantCulture);
	}

	private static bool HasFractionalMarker(string value)
	{
		return value.Contains('.');
	}

	private string HandleLeft(string value, string defaultValue)
	{
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, '_');
		string key = keyValuePair.Key;
		int num = Parse.Int(keyValuePair.Value, 1);
		if (key.Length == 0)
		{
			return defaultValue;
		}
		if (num <= 0)
		{
			return "";
		}
		if (num >= key.Length)
		{
			return key;
		}
		return key.Substring(0, num);
	}

	private string HandleRight(string value, string defaultValue)
	{
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, '_');
		string key = keyValuePair.Key;
		int num = Parse.Int(keyValuePair.Value, 1);
		if (key.Length == 0)
		{
			return defaultValue;
		}
		if (num <= 0)
		{
			return "";
		}
		if (num >= key.Length)
		{
			return key;
		}
		return key.Substring(key.Length - num);
	}

	private string HandleMid(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length < 3)
		{
			return defaultValue;
		}
		string text = array[0];
		if (!Parse.TryInt(array[1], out var result) || !Parse.TryInt(array[2], out var result2))
		{
			return defaultValue;
		}
		if (text.Length == 0 || result >= text.Length || result2 <= 0)
		{
			return "";
		}
		int num = Math.Min(result + result2, text.Length);
		return text.Substring(result, num - result);
	}

	private string HandleProper(string value, string defaultValue)
	{
		if (string.IsNullOrEmpty(value))
		{
			return defaultValue;
		}
		string[] array = value.Split(' ');
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Length > 0)
			{
				array[i] = char.ToUpper(array[i][0]) + ((array[i].Length > 1) ? array[i].Substring(1).ToLower() : "");
			}
		}
		return string.Join(" ", array);
	}

	private string HandleSearch(string value, string defaultValue)
	{
		string[] array = value.Split('_');
		if (array.Length < 2)
		{
			return defaultValue;
		}
		string value2 = array[0];
		string text = array[1];
		int num = ((array.Length >= 3) ? Parse.Int(array[2]) : 0);
		if (num >= text.Length)
		{
			return defaultValue;
		}
		int num2 = text.IndexOf(value2, num, StringComparison.OrdinalIgnoreCase);
		if (num2 < 0)
		{
			return defaultValue;
		}
		return num2.ToString();
	}

	private string HandleTime(string value)
	{
		long dayLengthSec = EnvMan.instance.m_dayLengthSec;
		double num = (double)dayLengthSec / 24.0;
		double num2 = num / 60.0;
		int num3 = (int)(time / (double)dayLengthSec);
		double num4 = time - (double)(num3 * dayLengthSec);
		int num5 = (int)(num4 / num);
		int num6 = (int)((num4 - (double)num5 * num) / num2);
		int num7 = (int)((num4 - (double)num5 * num - (double)num6 * num2) / (num2 / 60.0));
		return FormatTime(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero).AddDays(num3).AddHours(num5).AddMinutes(num6)
			.AddSeconds(num7), value);
	}

	private string HandleRealtime(string value)
	{
		string[] array = value.Split('_');
		string format = array[0];
		float num = ((array.Length > 1) ? Parse.Float(array[1]) : ((float)TimeZoneInfo.Local.BaseUtcOffset.TotalHours));
		return FormatTime(DateTimeOffset.UtcNow.AddHours(num), format);
	}

	private static string FormatTime(DateTimeOffset time, string format)
	{
		return time.ToString((format.Length == 1) ? ("%" + format) : format, CultureInfo.InvariantCulture);
	}

	internal static string? Rad2Deg(string value)
	{
		if (!Parse.TryAngleRadians(value, out var radians))
		{
			return null;
		}
		return (radians * 57.29578f).ToString(CultureInfo.InvariantCulture);
	}

	internal static string? Deg2Rad(string value)
	{
		if (!Parse.TryAngleDegrees(value, out var degrees))
		{
			return null;
		}
		return (degrees * ((float)Math.PI / 180f)).ToString(CultureInfo.InvariantCulture);
	}

	internal static string? Rad2Vec(string value)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (!Parse.TryAngleRadians(value, out var radians))
		{
			return null;
		}
		return Formatting.FormatPos(new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)));
	}

	internal static string? Deg2Vec(string value)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (!Parse.TryAngleDegrees(value, out var degrees))
		{
			return null;
		}
		float num = degrees * ((float)Math.PI / 180f);
		return Formatting.FormatPos(new Vector3(Mathf.Cos(num), 0f, Mathf.Sin(num)));
	}

	internal static string? Vec2Deg(string value)
	{
		if (!Parse.TryKvp(value, out KeyValuePair<string, string> kvp, '_'))
		{
			return null;
		}
		if (!Parse.TryFloat(kvp.Key, out var result) || !Parse.TryFloat(kvp.Value, out var result2))
		{
			return null;
		}
		return (Mathf.Atan2(result2, result) * 57.29578f).ToString(CultureInfo.InvariantCulture);
	}

	internal static string? Vec2Rad(string value)
	{
		if (!Parse.TryKvp(value, out KeyValuePair<string, string> kvp, '_'))
		{
			return null;
		}
		if (!Parse.TryFloat(kvp.Key, out var result) || !Parse.TryFloat(kvp.Value, out var result2))
		{
			return null;
		}
		return Mathf.Atan2(result2, result).ToString(CultureInfo.InvariantCulture);
	}

	private string HandleAngle(string value, string defaultValue)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (!TryGetTwoVectors(value, out var first, out var second))
		{
			return defaultValue;
		}
		return Vector3.Angle(first, second).ToString(CultureInfo.InvariantCulture);
	}

	private string HandleDistance(string value, string defaultValue)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (!TryGetTwoVectors(value, out var first, out var second))
		{
			return defaultValue;
		}
		return Vector3.Distance(first, second).ToString(CultureInfo.InvariantCulture);
	}

	private string HandleDot(string value, string defaultValue)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (!TryGetTwoVectors(value, out var first, out var second))
		{
			return defaultValue;
		}
		return Vector3.Dot(first, second).ToString(CultureInfo.InvariantCulture);
	}

	private string HandleCross(string value, string defaultValue)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (!TryGetTwoVectors(value, out var first, out var second))
		{
			return defaultValue;
		}
		return Formatting.FormatPos(Vector3.Cross(first, second));
	}

	private string HandleNormalize(string value, string defaultValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (!TryEvaluateVector3(value, out var vector))
		{
			return defaultValue;
		}
		return Formatting.FormatPos(((Vector3)(ref vector)).normalized);
	}

	private string HandleMagnitude(string value, string defaultValue)
	{
		if (!TryEvaluateVector3(value, out var vector))
		{
			return defaultValue;
		}
		return ((Vector3)(ref vector)).magnitude.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleSqrMagnitude(string value, string defaultValue)
	{
		if (!TryEvaluateVector3(value, out var vector))
		{
			return defaultValue;
		}
		return ((Vector3)(ref vector)).sqrMagnitude.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleProject(string value, string defaultValue)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (!TryGetTwoVectors(value, out var first, out var second))
		{
			return defaultValue;
		}
		return Formatting.FormatPos(Vector3.Project(first, second));
	}

	private string HandleReflect(string value, string defaultValue)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (!TryGetTwoVectors(value, out var first, out var second))
		{
			return defaultValue;
		}
		return Formatting.FormatPos(Vector3.Reflect(first, second));
	}

	private string HandleLerp(string value, string defaultValue)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		string[] array = value.Split('_');
		if (array.Length != 3)
		{
			return defaultValue;
		}
		if (!TryEvaluateVector3(array[0], out var vector))
		{
			return defaultValue;
		}
		if (!TryEvaluateVector3(array[1], out var vector2))
		{
			return defaultValue;
		}
		float? num = Calculator.EvaluateFloat(array[2]);
		if (!num.HasValue)
		{
			return defaultValue;
		}
		return Formatting.FormatPos(Vector3.LerpUnclamped(vector, vector2, num.Value));
	}

	private string HandleVecX(string value, string defaultValue)
	{
		if (!TryEvaluateVector3(value, out var vector))
		{
			return defaultValue;
		}
		return vector.x.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleVecY(string value, string defaultValue)
	{
		if (!TryEvaluateVector3(value, out var vector))
		{
			return defaultValue;
		}
		return vector.y.ToString(CultureInfo.InvariantCulture);
	}

	private string HandleVecZ(string value, string defaultValue)
	{
		if (!TryEvaluateVector3(value, out var vector))
		{
			return defaultValue;
		}
		return vector.z.ToString(CultureInfo.InvariantCulture);
	}

	private bool TryGetTwoVectors(string value, out Vector3 first, out Vector3 second)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, '_');
		if (keyValuePair.Value == "")
		{
			first = Vector3.zero;
			second = Vector3.zero;
			return false;
		}
		if (!TryEvaluateVector3(keyValuePair.Key, out first))
		{
			second = Vector3.zero;
			return false;
		}
		return TryEvaluateVector3(keyValuePair.Value, out second);
	}

	private static bool TryEvaluateVector3(string value, out Vector3 vector)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		vector = Vector3.zero;
		if (value == "")
		{
			return false;
		}
		string[] array = Parse.Split(value.Replace(" ", ","));
		if (array.Length == 0 || array.Length > 3)
		{
			return false;
		}
		float? num = Calculator.EvaluateFloat(array[0]);
		if (!num.HasValue)
		{
			return false;
		}
		vector.x = num.Value;
		if (array.Length > 1)
		{
			float? num2 = Calculator.EvaluateFloat(array[1]);
			if (!num2.HasValue)
			{
				return false;
			}
			vector.z = num2.Value;
		}
		if (array.Length > 2)
		{
			float? num3 = Calculator.EvaluateFloat(array[2]);
			if (!num3.HasValue)
			{
				return false;
			}
			vector.y = num3.Value;
		}
		return true;
	}

	private static bool TryEvaluateVector3Strict(string value, out Vector3 vector)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		vector = Vector3.zero;
		if (value == "")
		{
			return false;
		}
		string[] array = Parse.Split(value);
		if (array.Length < 2 || array.Length > 3)
		{
			return false;
		}
		return TryEvaluateVector3(value, out vector);
	}

	private static bool TryGetStrictVectorOperands(string[] values, out Vector3?[] vectors)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		vectors = new Vector3?[values.Length];
		bool result = false;
		for (int i = 0; i < values.Length; i++)
		{
			if (TryEvaluateVector3Strict(values[i], out var vector))
			{
				vectors[i] = vector;
				result = true;
			}
		}
		return result;
	}

	private static bool TryGetVectorMathOperand(string value, Vector3? parsedVector, out Vector3 operand)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (parsedVector.HasValue)
		{
			operand = parsedVector.Value;
			return true;
		}
		float? num = Calculator.EvaluateFloat(value);
		if (!num.HasValue)
		{
			operand = Vector3.zero;
			return false;
		}
		operand = new Vector3(num.Value, num.Value, num.Value);
		return true;
	}
}
