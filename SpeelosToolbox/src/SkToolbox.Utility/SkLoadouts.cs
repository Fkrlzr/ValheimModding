using System;
using System.Collections.Generic;
using SkToolbox.Configuration;
using UnityEngine;

namespace SkToolbox.Utility;

internal static class SkLoadouts
{
	private const char EntrySeparator = ';';

	private const char FieldSeparator = '|';

	internal static List<string> Names()
	{
		List<string> list = new List<string>();
		string[] array = Raw().Split(';');
		foreach (string text in array)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string[] array2 = text.Split('|');
				if (array2.Length == 2 && array2[0].Length > 0)
				{
					list.Add(array2[0]);
				}
			}
		}
		return list;
	}

	internal static int CountOf(string name)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		string text = BlobOf(name);
		if (string.IsNullOrEmpty(text))
		{
			return -1;
		}
		try
		{
			Inventory val = new Inventory("scratch", (Sprite)null, 8, 4);
			val.Load(new ZPackage(text));
			return val.GetAllItems().Count;
		}
		catch (Exception)
		{
			return -1;
		}
	}

	internal static int Save(string name, Player player)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		if ((Object)(object)player == (Object)null)
		{
			return 0;
		}
		name = Clean(name);
		if (name.Length == 0)
		{
			return 0;
		}
		Inventory inventory = ((Humanoid)player).GetInventory();
		if (inventory == null || inventory.GetAllItems().Count == 0)
		{
			return 0;
		}
		ZPackage val = new ZPackage();
		inventory.Save(val);
		string @base = val.GetBase64();
		List<string> list = new List<string>();
		string[] array = Raw().Split(';');
		foreach (string text in array)
		{
			if (!string.IsNullOrEmpty(text) && !NameOf(text).Equals(name, StringComparison.OrdinalIgnoreCase))
			{
				list.Add(text);
			}
		}
		list.Add(name + "|" + @base);
		Write(list);
		return inventory.GetAllItems().Count;
	}

	internal static bool Restore(string name, Player player, out int stacks, out int equipped)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		stacks = 0;
		equipped = 0;
		if ((Object)(object)player == (Object)null)
		{
			return false;
		}
		string text = BlobOf(name);
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		Inventory inventory = ((Humanoid)player).GetInventory();
		if (inventory == null)
		{
			return false;
		}
		((Humanoid)player).UnequipAllItems();
		inventory.Load(new ZPackage(text));
		foreach (ItemData item in new List<ItemData>(inventory.GetAllItems()))
		{
			if (item == null)
			{
				continue;
			}
			stacks++;
			if (item.m_equipped)
			{
				item.m_equipped = false;
				if (((Humanoid)player).EquipItem(item, false))
				{
					equipped++;
				}
			}
		}
		return true;
	}

	internal static void Delete(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return;
		}
		List<string> list = new List<string>();
		string[] array = Raw().Split(';');
		foreach (string text in array)
		{
			if (!string.IsNullOrEmpty(text) && !NameOf(text).Equals(name, StringComparison.OrdinalIgnoreCase))
			{
				list.Add(text);
			}
		}
		Write(list);
	}

	private static string BlobOf(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		string[] array = Raw().Split(';');
		foreach (string text in array)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string[] array2 = text.Split('|');
				if (array2.Length == 2 && array2[0].Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return array2[1];
				}
			}
		}
		return null;
	}

	private static string NameOf(string entry)
	{
		string[] array = entry.Split('|');
		if (array.Length == 0)
		{
			return "";
		}
		return array[0];
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
		if (SkConfigEntry.OLoadouts == null)
		{
			return "";
		}
		return SkConfigEntry.OLoadouts.Value ?? "";
	}

	private static void Write(List<string> entries)
	{
		if (SkConfigEntry.OLoadouts != null)
		{
			SkConfigEntry.OLoadouts.Value = string.Join(';'.ToString(), entries.ToArray());
		}
	}
}
