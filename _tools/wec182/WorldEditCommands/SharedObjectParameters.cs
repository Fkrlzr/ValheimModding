using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class SharedObjectParameters
{
	public Range<Vector3>? Scale;

	public Range<int>? Level;

	public Range<float>? Health;

	public bool isHealthPercentage;

	public Range<float>? Damage;

	public Range<int>? Ammo;

	public string? AmmoType;

	public bool? Baby;

	public bool? Persist;

	public bool? Distant;

	public Item? Helmet;

	public Item? LeftHand;

	public Item? RightHand;

	public Item? Chest;

	public Item? Shoulders;

	public Item? Legs;

	public Item? Utility;

	public Range<float>? Radius;

	public Range<int>? Model;

	public Dictionary<string, object> Fields = new Dictionary<string, object>();

	public Dictionary<string, string> DataParameters = new Dictionary<string, string>();

	protected virtual void ParseArgs(string[] args)
	{
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < args.Length; i++)
		{
			string[] array = args[i].Split(new char[1] { '=' }, 2);
			string text = array[0].ToLower();
			if (text == "baby")
			{
				Baby = true;
			}
			if (text == "persist")
			{
				Persist = true;
			}
			if (text == "distant")
			{
				Distant = true;
			}
			if (array.Length < 2)
			{
				continue;
			}
			string text2 = array[1].Trim();
			if (text == "par")
			{
				KeyValuePair<string, string> keyValuePair = Parse.Kvp(text2, ',');
				if (keyValuePair.Key == "")
				{
					throw new InvalidOperationException("Invalid data parameter " + text2 + ".");
				}
				DataParameters["<" + keyValuePair.Key + ">"] = keyValuePair.Value;
			}
			if (text == "health" || text == "durability")
			{
				if (text2.EndsWith("%"))
				{
					isHealthPercentage = true;
					text2 = text2.Substring(0, text2.Length - 1);
				}
				Health = Parse.FloatRange(text2, 0f);
				if (isHealthPercentage)
				{
					Range<float>? health = Health;
					health.Max /= 100f;
					Range<float>? health2 = Health;
					health2.Min /= 100f;
				}
			}
			if (text == "damage")
			{
				Damage = Parse.FloatRange(text2, 0f);
			}
			if (text == "ammo")
			{
				Ammo = Parse.IntRange(text2, 0);
			}
			if (text == "ammotype")
			{
				AmmoType = text2;
			}
			if (text == "stars" || text == "star")
			{
				Level = Parse.IntRange(text2, 0);
				Range<int>? level = Level;
				level.Max++;
				Range<int>? level2 = Level;
				level2.Min++;
			}
			if (text == "model")
			{
				Model = Parse.IntRange(text2, 0);
			}
			if (text == "level" || text == "levels")
			{
				Level = Parse.IntRange(text2, 0);
			}
			switch (text)
			{
			case "radius":
			case "range":
			case "circle":
				Radius = Parse.FloatRange(text2, 0f);
				break;
			}
			if (text == "sc" || text == "scale")
			{
				Scale = Parse.ScaleRange(text2);
			}
			if (text == "helmet")
			{
				Helmet = new Item(text2);
			}
			if (text == "left_hand")
			{
				LeftHand = new Item(text2);
			}
			if (text == "right_hand")
			{
				RightHand = new Item(text2);
			}
			if (text == "chest")
			{
				Chest = new Item(text2);
			}
			if (text == "shoulders")
			{
				Shoulders = new Item(text2);
			}
			if (text == "legs")
			{
				Legs = new Item(text2);
			}
			if (text == "utility")
			{
				Utility = new Item(text2);
			}
			if (text == "persist")
			{
				Persist = Parse.Boolean(text2);
			}
			if (text == "distant")
			{
				Distant = Parse.Boolean(text2);
			}
			if (!(text == "field") && !(text == "f"))
			{
				continue;
			}
			string[] array2 = text2.Split(',');
			if (array2.Length < 3)
			{
				continue;
			}
			string text3 = FieldAutoComplete.RealComponent(FieldAutoComplete.PrefabFromCommand(string.Join(" ", args)), array2[0]);
			bool zdoField;
			string text4 = FieldAutoComplete.RealField(text3, array2[1], out zdoField);
			string text5 = string.Join(",", array2.Skip(2));
			Type type = FieldAutoComplete.GetType(text3, text4, zdoField);
			string text6 = (zdoField ? text4 : (text3 + "." + text4));
			if (type == typeof(int))
			{
				Fields.Add(text6, Parse.Int(text5, 0));
				continue;
			}
			if (type == typeof(long))
			{
				Fields.Add(text6, long.Parse(text5));
				continue;
			}
			if (type == typeof(float))
			{
				Fields.Add(text6, Parse.Float(text5, 0f));
				continue;
			}
			if (type == typeof(string))
			{
				Fields.Add(text6, text5);
				continue;
			}
			if (type == typeof(bool))
			{
				Fields.Add(text6, Parse.BoolNull(text5).GetValueOrDefault() ? 1 : 0);
				continue;
			}
			if (type == typeof(Vector3))
			{
				Fields.Add(text6, Parse.VectorXZY(array2, 2));
				continue;
			}
			if (type == typeof(Quaternion))
			{
				Fields.Add(text6, Parse.AngleYXZ(array2, 2));
				continue;
			}
			if (type == typeof(GameObject) || type == typeof(ItemDrop) || type == typeof(EffectList))
			{
				Fields.Add(text6, text5);
				continue;
			}
			if (type == typeof(ObjectHash) || type == typeof(LocationHash) || type == typeof(RoomHash))
			{
				Fields.Add(text6, ZDOKeys.Hash(text5));
				continue;
			}
			if (type.IsEnum)
			{
				Fields.Add(text6, ToEnum(type, text5));
				continue;
			}
			if (type == typeof(void))
			{
				throw new InvalidOperationException("Field " + text6 + " of component " + text3 + " is not supported.");
			}
			throw new InvalidOperationException($"Unhandled type {type} for field {text6}");
		}
	}

	public static int ToEnum(Type type, string str)
	{
		return ToEnum(type, ToList(str));
	}

	public static int ToEnum(Type type, List<string> list)
	{
		try
		{
			int num = 0;
			foreach (string item in list)
			{
				num += (int)Enum.Parse(type, item, ignoreCase: true);
			}
			return num;
		}
		catch
		{
			throw new InvalidOperationException("Failed to parse enum " + type.Name + " with values " + string.Join(", ", list));
		}
	}

	public static List<string> ToList(string str, bool removeEmpty = true)
	{
		string[] array = Split(str, removeEmpty);
		List<string> list = new List<string>(array.Length);
		list.AddRange(array);
		return list;
	}

	public static string[] Split(string arg, bool removeEmpty = true, char split = ',')
	{
		return (from s in arg.Split(split)
			select s.Trim() into s
			where !removeEmpty || s != ""
			select s).ToArray();
	}
}
