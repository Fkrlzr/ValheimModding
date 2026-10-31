using System;
using System.Collections.Generic;
using System.Globalization;
using SkToolbox.Configuration;
using UnityEngine;

namespace SkToolbox.Utility;

internal static class SkBookmarks
{
	internal class Bookmark
	{
		public string Name = "";

		public Vector3 Pos;
	}

	private const char EntrySeparator = ';';

	private const char FieldSeparator = '|';

	private static int CurrentSeed
	{
		get
		{
			try
			{
				return (((Object)(object)ZNet.instance != (Object)null) ? ZNet.instance.GetWorld() : null)?.m_seed ?? 0;
			}
			catch (Exception)
			{
				return 0;
			}
		}
	}

	internal static List<Bookmark> ForThisWorld()
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		List<Bookmark> list = new List<Bookmark>();
		int currentSeed = CurrentSeed;
		if (currentSeed == 0)
		{
			return list;
		}
		string[] array = Raw().Split(';');
		foreach (string text in array)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string[] array2 = text.Split('|');
				if (array2.Length == 5 && int.TryParse(array2[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) && result == currentSeed && float.TryParse(array2[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var result2) && float.TryParse(array2[3], NumberStyles.Float, CultureInfo.InvariantCulture, out var result3) && float.TryParse(array2[4], NumberStyles.Float, CultureInfo.InvariantCulture, out var result4))
				{
					list.Add(new Bookmark
					{
						Name = array2[1],
						Pos = new Vector3(result2, result3, result4)
					});
				}
			}
		}
		return list;
	}

	internal static bool Save(string name, Vector3 pos)
	{
		int currentSeed = CurrentSeed;
		if (currentSeed == 0)
		{
			return false;
		}
		name = Clean(name);
		if (name.Length == 0)
		{
			return false;
		}
		List<string> list = new List<string>();
		string[] array = Raw().Split(';');
		foreach (string text in array)
		{
			if (!string.IsNullOrEmpty(text) && !Matches(text, currentSeed, name))
			{
				list.Add(text);
			}
		}
		list.Add(string.Join('|'.ToString(), currentSeed.ToString(CultureInfo.InvariantCulture), name, pos.x.ToString("0.##", CultureInfo.InvariantCulture), pos.y.ToString("0.##", CultureInfo.InvariantCulture), pos.z.ToString("0.##", CultureInfo.InvariantCulture)));
		Write(list);
		return true;
	}

	internal static void Delete(string name)
	{
		int currentSeed = CurrentSeed;
		if (currentSeed == 0 || string.IsNullOrEmpty(name))
		{
			return;
		}
		List<string> list = new List<string>();
		string[] array = Raw().Split(';');
		foreach (string text in array)
		{
			if (!string.IsNullOrEmpty(text) && !Matches(text, currentSeed, name))
			{
				list.Add(text);
			}
		}
		Write(list);
	}

	private static bool Matches(string entry, int seed, string name)
	{
		string[] array = entry.Split('|');
		if (array.Length != 5)
		{
			return false;
		}
		if (!int.TryParse(array[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			return false;
		}
		if (result == seed)
		{
			return string.Equals(array[1], name, StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private static string Clean(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return "";
		}
		return name.Replace(';', ' ').Replace('|', ' ').Trim();
	}

	private static string Raw()
	{
		if (SkConfigEntry.OBookmarks == null)
		{
			return "";
		}
		return SkConfigEntry.OBookmarks.Value ?? "";
	}

	private static void Write(List<string> entries)
	{
		if (SkConfigEntry.OBookmarks != null)
		{
			SkConfigEntry.OBookmarks.Value = string.Join(';'.ToString(), entries.ToArray());
		}
	}
}
