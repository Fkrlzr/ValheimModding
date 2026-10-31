using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Data;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class DataRawCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<int, List<string>> _003C_003E9__0_0;

		public static ConsoleEvent _003C_003E9__0_1;

		public static Func<KeyValuePair<int, Vector3>, int> _003C_003E9__2_0;

		public static Func<KeyValuePair<int, Vector3>, Vector3> _003C_003E9__2_1;

		public static Func<KeyValuePair<int, int>, int> _003C_003E9__2_2;

		public static Func<KeyValuePair<int, int>, int> _003C_003E9__2_3;

		public static Func<KeyValuePair<int, float>, int> _003C_003E9__2_4;

		public static Func<KeyValuePair<int, float>, float> _003C_003E9__2_5;

		public static Func<KeyValuePair<int, Quaternion>, int> _003C_003E9__2_6;

		public static Func<KeyValuePair<int, Quaternion>, Quaternion> _003C_003E9__2_7;

		public static Func<KeyValuePair<int, string>, int> _003C_003E9__2_8;

		public static Func<KeyValuePair<int, string>, string> _003C_003E9__2_9;

		public static Func<KeyValuePair<int, long>, int> _003C_003E9__2_10;

		public static Func<KeyValuePair<int, long>, long> _003C_003E9__2_11;

		public static Func<KeyValuePair<int, byte[]>, int> _003C_003E9__2_12;

		public static Func<KeyValuePair<int, byte[]>, byte[]> _003C_003E9__2_13;

		internal List<string> _003C_002Ector_003Eb__0_0(int index)
		{
			if (index != 0)
			{
				return ParameterInfo.None;
			}
			return DataLoading.DataKeys;
		}

		internal void _003C_002Ector_003Eb__0_1(ConsoleEventArgs args)
		{
			Helper.ArgsCheck(args, 2, "Missing data entry name.");
			if (!DataLoading.Data.TryGetValue(StringExtensionMethods.GetStableHashCode(args[1]), out DataEntry value))
			{
				throw new InvalidOperationException("Data entry " + args[1] + " not found.");
			}
			Dictionary<string, string> pars = ParseCommand(args);
			string @base = DataHelper.Resolve(value, pars, null).GetBase64();
			if (@base == "AAAAAA==")
			{
				throw new InvalidOperationException("Data entry " + args[1] + " is empty.");
			}
			GUIUtility.systemCopyBuffer = @base;
			Helper.AddMessage(args.Context, "Data entry " + args[1] + " copied to clipboard.", false);
		}

		internal int _003CLegacySerialize_003Eb__2_0(KeyValuePair<int, Vector3> kvp)
		{
			return kvp.Key;
		}

		internal Vector3 _003CLegacySerialize_003Eb__2_1(KeyValuePair<int, Vector3> kvp)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return kvp.Value;
		}

		internal int _003CLegacySerialize_003Eb__2_2(KeyValuePair<int, int> kvp)
		{
			return kvp.Key;
		}

		internal int _003CLegacySerialize_003Eb__2_3(KeyValuePair<int, int> kvp)
		{
			return kvp.Value;
		}

		internal int _003CLegacySerialize_003Eb__2_4(KeyValuePair<int, float> kvp)
		{
			return kvp.Key;
		}

		internal float _003CLegacySerialize_003Eb__2_5(KeyValuePair<int, float> kvp)
		{
			return kvp.Value;
		}

		internal int _003CLegacySerialize_003Eb__2_6(KeyValuePair<int, Quaternion> kvp)
		{
			return kvp.Key;
		}

		internal Quaternion _003CLegacySerialize_003Eb__2_7(KeyValuePair<int, Quaternion> kvp)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return kvp.Value;
		}

		internal int _003CLegacySerialize_003Eb__2_8(KeyValuePair<int, string> kvp)
		{
			return kvp.Key;
		}

		internal string _003CLegacySerialize_003Eb__2_9(KeyValuePair<int, string> kvp)
		{
			return kvp.Value;
		}

		internal int _003CLegacySerialize_003Eb__2_10(KeyValuePair<int, long> kvp)
		{
			return kvp.Key;
		}

		internal long _003CLegacySerialize_003Eb__2_11(KeyValuePair<int, long> kvp)
		{
			return kvp.Value;
		}

		internal int _003CLegacySerialize_003Eb__2_12(KeyValuePair<int, byte[]> kvp)
		{
			return kvp.Key;
		}

		internal byte[] _003CLegacySerialize_003Eb__2_13(KeyValuePair<int, byte[]> kvp)
		{
			return kvp.Value;
		}
	}

	public DataRawCommand()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		AutoComplete.Register("data_raw", (Func<int, List<string>>)((int index) => (index != 0) ? ParameterInfo.None : DataLoading.DataKeys));
		object obj = _003C_003Ec._003C_003E9__0_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				Helper.ArgsCheck(args, 2, "Missing data entry name.");
				if (!DataLoading.Data.TryGetValue(StringExtensionMethods.GetStableHashCode(args[1]), out DataEntry value))
				{
					throw new InvalidOperationException("Data entry " + args[1] + " not found.");
				}
				Dictionary<string, string> pars = ParseCommand(args);
				string @base = DataHelper.Resolve(value, pars, null).GetBase64();
				if (@base == "AAAAAA==")
				{
					throw new InvalidOperationException("Data entry " + args[1] + " is empty.");
				}
				GUIUtility.systemCopyBuffer = @base;
				Helper.AddMessage(args.Context, "Data entry " + args[1] + " copied to clipboard.", false);
			};
			_003C_003Ec._003C_003E9__0_1 = val;
			obj = (object)val;
		}
		Helper.Command("data_raw", "[name] - Copies data entry to clipboard as base64 encoded string.", (ConsoleEvent)obj);
	}

	private static Dictionary<string, string> ParseCommand(ConsoleEventArgs args)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string[] args2 = args.Args;
		for (int i = 0; i < args2.Length; i++)
		{
			string[] array = args2[i].Split(new char[1] { '=' }, 2);
			if (array.Length >= 2 && array[0].ToLower() == "par")
			{
				KeyValuePair<string, string> keyValuePair = Parse.Kvp(array[1], ',');
				if (keyValuePair.Key == "")
				{
					throw new InvalidOperationException("Invalid data parameter " + array[1] + ".");
				}
				dictionary["<" + keyValuePair.Key + ">"] = keyValuePair.Value;
			}
		}
		return dictionary;
	}

	public static void LegacySerialize(ZDO zdo, ZPackage pkg, string filter)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_070d: Expected I4, but got Unknown
		ZDOID uid = zdo.m_uid;
		Dictionary<int, Vector3> dictionary = (ZDOExtraData.s_vec3.ContainsKey(uid) ? ((IEnumerable<KeyValuePair<int, Vector3>>)ZDOExtraData.s_vec3[uid]).ToDictionary((KeyValuePair<int, Vector3> kvp) => kvp.Key, (KeyValuePair<int, Vector3> kvp) => kvp.Value) : new Dictionary<int, Vector3>());
		Dictionary<int, int> dictionary2 = (ZDOExtraData.s_ints.ContainsKey(uid) ? ((IEnumerable<KeyValuePair<int, int>>)ZDOExtraData.s_ints[uid]).ToDictionary((KeyValuePair<int, int> kvp) => kvp.Key, (KeyValuePair<int, int> kvp) => kvp.Value) : new Dictionary<int, int>());
		Dictionary<int, float> dictionary3 = (ZDOExtraData.s_floats.ContainsKey(uid) ? ((IEnumerable<KeyValuePair<int, float>>)ZDOExtraData.s_floats[uid]).ToDictionary((KeyValuePair<int, float> kvp) => kvp.Key, (KeyValuePair<int, float> kvp) => kvp.Value) : new Dictionary<int, float>());
		Dictionary<int, Quaternion> dictionary4 = (ZDOExtraData.s_quats.ContainsKey(uid) ? ((IEnumerable<KeyValuePair<int, Quaternion>>)ZDOExtraData.s_quats[uid]).ToDictionary((KeyValuePair<int, Quaternion> kvp) => kvp.Key, (KeyValuePair<int, Quaternion> kvp) => kvp.Value) : new Dictionary<int, Quaternion>());
		Dictionary<int, string> dictionary5 = (ZDOExtraData.s_strings.ContainsKey(uid) ? ((IEnumerable<KeyValuePair<int, string>>)ZDOExtraData.s_strings[uid]).ToDictionary((KeyValuePair<int, string> kvp) => kvp.Key, (KeyValuePair<int, string> kvp) => kvp.Value) : new Dictionary<int, string>());
		Dictionary<int, long> dictionary6 = (ZDOExtraData.s_longs.ContainsKey(uid) ? ((IEnumerable<KeyValuePair<int, long>>)ZDOExtraData.s_longs[uid]).ToDictionary((KeyValuePair<int, long> kvp) => kvp.Key, (KeyValuePair<int, long> kvp) => kvp.Value) : new Dictionary<int, long>());
		Dictionary<int, byte[]> dictionary7 = (ZDOExtraData.s_byteArrays.ContainsKey(uid) ? ((IEnumerable<KeyValuePair<int, byte[]>>)ZDOExtraData.s_byteArrays[uid]).ToDictionary((KeyValuePair<int, byte[]> kvp) => kvp.Key, (KeyValuePair<int, byte[]> kvp) => kvp.Value) : new Dictionary<int, byte[]>());
		if (filter == "")
		{
			dictionary.Remove(Hash.Scale);
			dictionary.Remove(Hash.SpawnPoint);
			dictionary2.Remove(Hash.Seed);
			dictionary2.Remove(Hash.Location);
			if (dictionary5.ContainsKey(Hash.OverrideItems))
			{
				dictionary2.Remove(Hash.AddedDefaultItems);
				dictionary5.Remove(Hash.Items);
			}
		}
		else if (filter != "all")
		{
			HashSet<int> filters = Parse.Split(filter, ',').Select(ZDOKeys.Hash).ToHashSet();
			dictionary = FilterZdo(dictionary, filters);
			dictionary4 = FilterZdo(dictionary4, filters);
			dictionary3 = FilterZdo(dictionary3, filters);
			dictionary2 = FilterZdo(dictionary2, filters);
			dictionary6 = FilterZdo(dictionary6, filters);
			dictionary7 = FilterZdo(dictionary7, filters);
		}
		int num = 0;
		if (dictionary3.Count() > 0)
		{
			num |= 1;
		}
		if (dictionary.Count() > 0)
		{
			num |= 2;
		}
		if (dictionary4.Count() > 0)
		{
			num |= 4;
		}
		if (dictionary2.Count() > 0)
		{
			num |= 8;
		}
		if (dictionary5.Count() > 0)
		{
			num |= 0x10;
		}
		if (dictionary6.Count() > 0)
		{
			num |= 0x40;
		}
		if (dictionary7.Count() > 0)
		{
			num |= 0x80;
		}
		ZDOConnectionHashData value;
		ZDOConnectionHashData val = (ZDOExtraData.s_connectionsHashData.TryGetValue(uid, out value) ? value : null);
		if (val != null && filter == "all" && (int)val.m_type != 0 && val.m_hash != 0)
		{
			num |= 0x100;
		}
		pkg.Write(num);
		if (dictionary3.Count() > 0)
		{
			pkg.Write((byte)dictionary3.Count());
			foreach (KeyValuePair<int, float> item in dictionary3)
			{
				pkg.Write(item.Key);
				pkg.Write(item.Value);
			}
		}
		if (dictionary.Count() > 0)
		{
			pkg.Write((byte)dictionary.Count());
			foreach (KeyValuePair<int, Vector3> item2 in dictionary)
			{
				pkg.Write(item2.Key);
				pkg.Write(item2.Value);
			}
		}
		if (dictionary4.Count() > 0)
		{
			pkg.Write((byte)dictionary4.Count());
			foreach (KeyValuePair<int, Quaternion> item3 in dictionary4)
			{
				pkg.Write(item3.Key);
				pkg.Write(item3.Value);
			}
		}
		if (dictionary2.Count() > 0)
		{
			pkg.Write((byte)dictionary2.Count());
			foreach (KeyValuePair<int, int> item4 in dictionary2)
			{
				pkg.Write(item4.Key);
				pkg.Write(item4.Value);
			}
		}
		if (dictionary6.Count() > 0)
		{
			pkg.Write((byte)dictionary6.Count());
			foreach (KeyValuePair<int, long> item5 in dictionary6)
			{
				pkg.Write(item5.Key);
				pkg.Write(item5.Value);
			}
		}
		if (dictionary5.Count() > 0)
		{
			pkg.Write((byte)dictionary5.Count());
			foreach (KeyValuePair<int, string> item6 in dictionary5)
			{
				pkg.Write(item6.Key);
				pkg.Write(item6.Value);
			}
		}
		if (dictionary7.Count() > 0)
		{
			pkg.Write((byte)dictionary7.Count());
			foreach (KeyValuePair<int, byte[]> item7 in dictionary7)
			{
				pkg.Write(item7.Key);
				pkg.Write(item7.Value);
			}
		}
		if (val != null && ((uint)num & 0x100u) != 0)
		{
			pkg.Write((byte)(int)val.m_type);
			pkg.Write(val.m_hash);
		}
	}

	private static Dictionary<int, T> FilterZdo<T>(Dictionary<int, T> dict, HashSet<int> filters)
	{
		HashSet<int> filters2 = filters;
		return dict.Where<KeyValuePair<int, T>>((KeyValuePair<int, T> kvp) => filters2.Contains(kvp.Key)).ToDictionary((KeyValuePair<int, T> kvp) => kvp.Key, (KeyValuePair<int, T> pair) => pair.Value);
	}
}
