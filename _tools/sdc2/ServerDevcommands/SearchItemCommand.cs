using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class SearchItemCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<GameObject, string> _003C_003E9__0_2;

		public static ConsoleEvent _003C_003E9__0_0;

		public static Func<FieldInfo, bool> _003C_003E9__2_0;

		public static Func<FieldInfo, string> _003C_003E9__2_1;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			Helper.ArgsCheck(args, 2, "Missing the search term");
			List<GameObject> list = ObjectDB.instance.m_items;
			for (int i = 1; i < args.Length; i++)
			{
				string[] array = args[i].Split(',');
				if (array.Length != 2)
				{
					args.Context.AddString("Invalid search term: " + args[i]);
					continue;
				}
				string field = array[0];
				string value = array[1].ToLowerInvariant();
				list = ItemDropsByField(list, field, value);
			}
			List<string> list2 = list.Select((GameObject item) => ((Object)item).name).ToList();
			if (list2.Count > 50)
			{
				args.Context.AddString("Over 50 results, printing to the log file.");
				Debug.Log((object)("Search item results:\n" + string.Join("\n", list2)));
				return;
			}
			foreach (string item in list2)
			{
				args.Context.AddString(item);
			}
		}

		internal string _003C_002Ector_003Eb__0_2(GameObject item)
		{
			return ((Object)item).name;
		}

		internal bool _003CGenerateAutocomplete_003Eb__2_0(FieldInfo f)
		{
			if (!validTypes.Contains(f.FieldType))
			{
				return f.FieldType.IsEnum;
			}
			return true;
		}

		internal string _003CGenerateAutocomplete_003Eb__2_1(FieldInfo p)
		{
			return p.Name;
		}
	}

	private static readonly HashSet<Type> validTypes = new HashSet<Type>
	{
		typeof(bool),
		typeof(int),
		typeof(float),
		typeof(string)
	};

	public SearchItemCommand()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				Helper.ArgsCheck(args, 2, "Missing the search term");
				List<GameObject> list = ObjectDB.instance.m_items;
				for (int i = 1; i < args.Length; i++)
				{
					string[] array = args[i].Split(',');
					if (array.Length != 2)
					{
						args.Context.AddString("Invalid search term: " + args[i]);
					}
					else
					{
						string field = array[0];
						string value = array[1].ToLowerInvariant();
						list = ItemDropsByField(list, field, value);
					}
				}
				List<string> list2 = list.Select((GameObject item) => ((Object)item).name).ToList();
				if (list2.Count > 50)
				{
					args.Context.AddString("Over 50 results, printing to the log file.");
					Debug.Log((object)("Search item results:\n" + string.Join("\n", list2)));
					return;
				}
				foreach (string item in list2)
				{
					args.Context.AddString(item);
				}
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		Helper.Command("search_item", "[field,value] [field,value] ...  - Searches items.", (ConsoleEvent)obj);
		List<string> itemFields = new List<string>();
		AutoComplete.Register("search_item", delegate(int index, int subIndex)
		{
			if (itemFields.Count == 0)
			{
				itemFields = GenerateAutocomplete();
			}
			return subIndex switch
			{
				0 => itemFields, 
				1 => ParameterInfo.Create("Value"), 
				_ => ParameterInfo.None, 
			};
		});
	}

	private static List<string> GenerateAutocomplete()
	{
		return (from f in typeof(SharedData).GetFields(BindingFlags.Instance | BindingFlags.Public)
			where validTypes.Contains(f.FieldType) || f.FieldType.IsEnum
			select f into p
			select p.Name).ToList();
	}

	private static List<GameObject> ItemDropsByField(List<GameObject> items, string field, string value)
	{
		string field2 = field;
		string value2 = value;
		return items.Where(delegate(GameObject item)
		{
			ItemDrop component = item.GetComponent<ItemDrop>();
			if ((Object)(object)component == (Object)null)
			{
				return false;
			}
			ItemData itemData = component.m_itemData;
			if (itemData == null)
			{
				return false;
			}
			SharedData shared = itemData.m_shared;
			if (shared == null)
			{
				return false;
			}
			FieldInfo field3 = typeof(SharedData).GetField(field2, BindingFlags.Instance | BindingFlags.Public);
			return !(field3 == null) && field3.GetValue(shared).ToString().ToLowerInvariant() == value2;
		}).ToList();
	}
}
