using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Data;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class DataCommand
{
	public const string Name = "data";

	private DataParameters Parameters = new DataParameters();

	protected string DoOperation(ZNetView view, string operation, string? value)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		switch (operation)
		{
		case "copy_raw":
		{
			ZPackage val = new ZPackage();
			DataRawCommand.LegacySerialize(view.GetZDO(), val, value ?? "");
			string @base = val.GetBase64();
			if (@base == "AAAAAA==")
			{
				return "¤ raw data is empty.";
			}
			GUIUtility.systemCopyBuffer = @base;
			return "¤ raw data copied to clipboard.";
		}
		case "save":
			if (value == null || value == "")
			{
				throw new InvalidOperationException("Save: Missing data entry name.");
			}
			DataLoading.Save(new PlainDataEntry(view.GetZDO()), value, profile: false, dump: false);
			return "¤ data saved.";
		case "dump":
			if (value == null || value == "")
			{
				throw new InvalidOperationException("Save: Missing data entry name.");
			}
			DataLoading.Save(AddDefaultFields(view.GetZDO(), new PlainDataEntry(view.GetZDO())), value, profile: false, dump: true);
			return "¤ data saved.";
		default:
			throw new NotImplementedException();
		}
	}

	protected string DoOperation(ZNetView view, string operation, float? value)
	{
		throw new NotImplementedException();
	}

	protected string DoOperation(ZNetView view, string operation, string[] value)
	{
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		switch (operation)
		{
		case "merge":
		{
			if (value.Length == 0)
			{
				throw new InvalidOperationException("Merge: Missing data entry name.");
			}
			UndoHelper.AddEditAction(view);
			string[] array4 = (from s in value.SelectMany((string str) => str.Split(','))
				select s.Trim()).ToArray();
			DataEntry dataEntry2 = DataHelper.Merge(array4.Select(DataHelper.Get).ToArray());
			if (dataEntry2 != null)
			{
				DataHelper.Write(dataEntry2, Parameters.DataParameters, view.GetZDO());
			}
			Actions.Refresh(view);
			return "¤ data merged from " + string.Join(", ", array4) + ".";
		}
		case "load":
		{
			if (value.Length == 0)
			{
				throw new InvalidOperationException("Load: Missing data entry name.");
			}
			UndoHelper.AddEditAction(view);
			ZDO zdo = DataHelper.CloneBase(view.GetZDO());
			string[] array3 = (from s in value.SelectMany((string str) => str.Split(','))
				select s.Trim()).ToArray();
			DataEntry dataEntry = DataHelper.Merge(array3.Select(DataHelper.Get).ToArray());
			if (dataEntry != null)
			{
				DataHelper.Write(dataEntry, Parameters.DataParameters, zdo);
			}
			Regen(view, zdo);
			return "¤ data loaded from " + string.Join(", ", array3) + ".";
		}
		case "keep":
		{
			if (value.Length == 0)
			{
				throw new InvalidOperationException("Keep: Missing data keys.");
			}
			string[] array5 = (from s in value.SelectMany((string str) => str.Split(','))
				select s.Trim()).ToArray();
			if (DataHelper.HasKey(view.GetZDO(), array5))
			{
				UndoHelper.AddEditAction(view);
				Regen(view, DataHelper.CloneWithKeys(view.GetZDO(), array5));
				return "¤ data cleaned except " + string.Join(", ", array5) + ".";
			}
			return "¤ skipped, keys " + string.Join(", ", array5) + " not found.";
		}
		case "remove":
		{
			if (value.Length == 0)
			{
				throw new InvalidOperationException("Remove: Missing data keys.");
			}
			UndoHelper.AddEditAction(view);
			string[] array2 = (from s in value.SelectMany((string str) => str.Split(','))
				select s.Trim()).ToArray();
			Regen(view, DataHelper.CloneWithoutKeys(view.GetZDO(), array2));
			return "¤ data keys removed " + string.Join(", ", array2) + ".";
		}
		case "set":
		{
			if (value.Length == 0)
			{
				throw new InvalidOperationException("Set: Missing values.");
			}
			UndoHelper.AddEditAction(view);
			ZDO zDO = view.GetZDO();
			ZDOID uid = zDO.m_uid;
			for (int i = 0; i < value.Length; i++)
			{
				string[] array = value[i].Split(new char[1] { ',' }, 3);
				if (array.Length < 2)
				{
					throw new InvalidOperationException("Set: Missing value for " + array[0] + ".");
				}
				if (array.Length < 3)
				{
					throw new InvalidOperationException("Set: Missing key for " + array[1] + ".");
				}
				string text = array[0].ToLowerInvariant();
				int result;
				int num = (int.TryParse(array[1], out result) ? result : ZDOKeys.Hash(array[1]));
				string text2 = array[2];
				switch (text)
				{
				case "float":
					ZDOExtraData.Set(uid, num, Parse.Float(text2, 0f));
					break;
				case "vec3":
					ZDOExtraData.Set(uid, num, Parse.VectorXZY(text2));
					break;
				case "quat":
					ZDOExtraData.Set(uid, num, Parse.AngleYXZ(text2));
					break;
				case "int":
					ZDOExtraData.Set(uid, num, Parse.Int(text2, 0));
					break;
				case "hash":
					ZDOExtraData.Set(uid, num, ZDOKeys.Hash(text2));
					break;
				case "bool":
					ZDOExtraData.Set(uid, num, Parse.Boolean(text2).GetValueOrDefault() ? 1 : 0);
					break;
				case "long":
					ZDOExtraData.Set(uid, num, Parse.Long(text2, 0L));
					break;
				case "string":
					ZDOExtraData.Set(uid, num, text2);
					break;
				case "array":
					ZDOExtraData.Set(uid, num, Convert.FromBase64String(text2));
					break;
				default:
					throw new InvalidOperationException("Unknown type " + text + ".");
				}
			}
			zDO.IncreaseDataRevision();
			Actions.Refresh(view);
			return "¤ data set.";
		}
		default:
			throw new NotImplementedException();
		}
	}

	protected string DoOperation(ZNetView view, string operation, int? value)
	{
		throw new NotImplementedException();
	}

	protected string DoOperation(ZNetView view, string operation, bool? value)
	{
		if (value == false)
		{
			return "";
		}
		if (operation == "clear")
		{
			UndoHelper.AddEditAction(view);
			Regen(view, DataHelper.CloneBase(view.GetZDO()));
			return "¤ data cleared.";
		}
		if (operation == "print")
		{
			return "¤\n" + string.Join("\n", DataHelper.Print(view.GetZDO()));
		}
		throw new NotImplementedException();
	}

	protected string DoOperation(ZNetView view, string operation, long? value)
	{
		throw new NotImplementedException();
	}

	private void Regen(ZNetView view, ZDO zdo)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (zdo.GetPrefab() != Hash.Player)
		{
			ZDOID uid = view.GetZDO().m_uid;
			ZDO zdo2 = DataHelper.Regen(view.GetZDO(), zdo);
			UndoHelper.RefreshEditAction(uid, zdo2);
		}
	}

	private void Execute(Terminal context, ZNetView[] views)
	{
		UndoHelper.BeginAction();
		int num = views.Count();
		bool flag = Parameters.Operations.Count > 1 || !Parameters.Operations.ContainsKey("print");
		foreach (ZNetView val in views)
		{
			ZDO zDO = val.GetZDO();
			if (flag && zDO.GetPrefab() != Hash.Player)
			{
				val.ClaimOwnership();
			}
			foreach (KeyValuePair<string, object> operation in Parameters.Operations)
			{
				Type type = DataParameters.SupportedOperations[operation.Key];
				string prefabName = Utils.GetPrefabName(((Component)val).gameObject);
				string text = ((type == typeof(int)) ? DoOperation(val, operation.Key, (int?)operation.Value) : ((type == typeof(long)) ? DoOperation(val, operation.Key, (long?)operation.Value) : ((type == typeof(float)) ? DoOperation(val, operation.Key, (float?)operation.Value) : ((type == typeof(string[])) ? DoOperation(val, operation.Key, (string[])operation.Value) : ((!(type == typeof(bool))) ? DoOperation(val, operation.Key, (string)operation.Value) : DoOperation(val, operation.Key, (bool?)operation.Value))))));
				if (!(text == ""))
				{
					string text2 = text.Replace("¤", prefabName);
					if (num == 1)
					{
						Helper.AddMessage(context, text2, false);
					}
					else
					{
						context.AddString(text2);
					}
				}
			}
		}
		UndoHelper.EndAction();
	}

	private static PlainDataEntry AddDefaultFields(ZDO zdo, PlainDataEntry entry)
	{
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		GameObject prefab = ZNetScene.instance.GetPrefab(zdo.GetPrefab());
		if (!Object.op_Implicit((Object)(object)prefab))
		{
			return entry;
		}
		prefab.GetComponentsInChildren<MonoBehaviour>(ZNetView.m_tempComponents);
		foreach (MonoBehaviour tempComponent in ZNetView.m_tempComponents)
		{
			FieldInfo[] fields = ((object)tempComponent).GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				int key = ZDOKeys.Hash(((object)tempComponent).GetType().Name + "." + fieldInfo.Name);
				Type fieldType = fieldInfo.FieldType;
				if (fieldType == typeof(int))
				{
					PlainDataEntry plainDataEntry = entry;
					if (plainDataEntry.Ints == null)
					{
						plainDataEntry.Ints = new Dictionary<int, int>();
					}
					if (!entry.Ints.ContainsKey(key))
					{
						entry.Ints[key] = (int)fieldInfo.GetValue(tempComponent);
					}
				}
				if (fieldType == typeof(float))
				{
					PlainDataEntry plainDataEntry = entry;
					if (plainDataEntry.Floats == null)
					{
						plainDataEntry.Floats = new Dictionary<int, float>();
					}
					if (!entry.Floats.ContainsKey(key))
					{
						entry.Floats[key] = (float)fieldInfo.GetValue(tempComponent);
					}
				}
				if (fieldType == typeof(string))
				{
					PlainDataEntry plainDataEntry = entry;
					if (plainDataEntry.Strings == null)
					{
						plainDataEntry.Strings = new Dictionary<int, string>();
					}
					if (!entry.Strings.ContainsKey(key))
					{
						entry.Strings[key] = (string)fieldInfo.GetValue(tempComponent);
					}
				}
				if (fieldType == typeof(bool))
				{
					PlainDataEntry plainDataEntry = entry;
					if (plainDataEntry.Ints == null)
					{
						plainDataEntry.Ints = new Dictionary<int, int>();
					}
					if (!entry.Ints.ContainsKey(key))
					{
						entry.Ints[key] = (((bool)fieldInfo.GetValue(tempComponent)) ? 1 : 0);
					}
				}
				if (fieldType == typeof(Vector3))
				{
					PlainDataEntry plainDataEntry = entry;
					if (plainDataEntry.Vecs == null)
					{
						plainDataEntry.Vecs = new Dictionary<int, Vector3>();
					}
					if (!entry.Vecs.ContainsKey(key))
					{
						entry.Vecs[key] = (Vector3)fieldInfo.GetValue(tempComponent);
					}
				}
				if (fieldType == typeof(GameObject))
				{
					PlainDataEntry plainDataEntry = entry;
					if (plainDataEntry.Strings == null)
					{
						plainDataEntry.Strings = new Dictionary<int, string>();
					}
					if (!entry.Strings.ContainsKey(key))
					{
						Dictionary<int, string>? strings = entry.Strings;
						GameObject val = (GameObject)fieldInfo.GetValue(tempComponent);
						strings[key] = (((int)val != 0) ? ((Object)val).name : null) ?? "";
					}
				}
				if (fieldType == typeof(ItemDrop))
				{
					PlainDataEntry plainDataEntry = entry;
					if (plainDataEntry.Strings == null)
					{
						plainDataEntry.Strings = new Dictionary<int, string>();
					}
					if (!entry.Strings.ContainsKey(key))
					{
						Dictionary<int, string>? strings2 = entry.Strings;
						ItemDrop val2 = (ItemDrop)fieldInfo.GetValue(tempComponent);
						strings2[key] = (((int)val2 != 0) ? ((Object)val2).name : null) ?? "";
					}
				}
			}
		}
		ZNetView.m_tempComponents.Clear();
		return entry;
	}

	public DataCommand()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		AutoComplete.Register("data", (Func<int, List<string>>)((int index) => DataAutoComplete.GetOptions()), DataAutoComplete.GetNamedOptions());
		Helper.Command("data", "Modifies object data.", (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			Parameters = new DataParameters();
			Parameters.ParseCommand(args);
			ZNetView[] objects = Parameters.GetObjects();
			if (Parameters.Operations.ContainsKey("save") && objects.Length > 1)
			{
				throw new InvalidOperationException("Can't save multiple objects at once.");
			}
			Execute(args.Context, objects);
		});
	}
}
