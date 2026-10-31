using System;
using System.Collections.Generic;
using System.Linq;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public abstract class TweakCommand
{
	public static Random Random = new Random();

	protected Type Component = typeof(int);

	protected string ComponentName = "";

	public bool AddComponentAutomatically = true;

	public Dictionary<string, Type> SupportedOperations = new Dictionary<string, Type>();

	public Dictionary<string, Func<int, List<string>>> AutoComplete = new Dictionary<string, Func<int, List<string>>>();

	private TweakParameters Parameters = new TweakParameters(new Dictionary<string, Type>());

	public static bool Roll(float value)
	{
		if (value >= 1f)
		{
			return true;
		}
		return Random.NextDouble() < (double)value;
	}

	protected abstract string DoOperation(ZNetView view, string operation, string? value);

	protected abstract string DoOperation(ZNetView view, string operation, float? value);

	protected abstract string DoOperation(ZNetView view, string operation, string[] value);

	protected abstract string DoOperation(ZNetView view, string operation, int? value);

	protected abstract string DoOperation(ZNetView view, string operation, bool? value);

	protected abstract string DoOperation(ZNetView view, string operation, long? value);

	protected virtual Dictionary<string, object?> Postprocess(ZNetView view, Dictionary<string, object?> operations)
	{
		return operations;
	}

	protected virtual void Postprocess(GameObject? obj)
	{
	}

	protected virtual ZNetView Preprocess(Terminal context, ZNetView view)
	{
		return view;
	}

	private void Execute(Terminal context, float chance, bool force, Dictionary<string, object?> operations, ZNetView[] views)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		Terminal context2 = context;
		ZNetView[] views2 = views;
		_ = ZNetScene.instance;
		Dictionary<ZDOID, long> dictionary = new Dictionary<ZDOID, long>();
		views2 = (from view in views2.Where(delegate(ZNetView view)
			{
				if (!Object.op_Implicit((Object)(object)view))
				{
					return false;
				}
				if (!view.GetZDO().IsValid())
				{
					context2.AddString("Skipped: " + ((Object)view).name + " is not loaded.");
					return false;
				}
				if (!Roll(chance))
				{
					context2.AddString("Skipped: " + ((Object)view).name + " (chance).");
					return false;
				}
				return true;
			})
			select Preprocess(context2, view)).Where(delegate(ZNetView view)
		{
			if (!Object.op_Implicit((Object)(object)view))
			{
				return false;
			}
			if (ComponentName != "" && !Object.op_Implicit((Object)(object)((Component)view).GetComponentInChildren(Component)))
			{
				if (force || (views2.Length == 1 && AddComponentAutomatically))
				{
					return true;
				}
				context2.AddString("Skipped: " + ((Object)view).name + " doesn't have the component. Use <color=yellow>force</color> to add it.");
				return false;
			}
			return true;
		}).ToArray();
		UndoHelper.BeginAction();
		ZNetView[] array = views2;
		foreach (ZNetView obj in array)
		{
			ZDO zDO = obj.GetZDO();
			UndoHelper.AddEditAction(zDO);
			dictionary.Add(zDO.m_uid, zDO.GetOwner());
			obj.ClaimOwnership();
		}
		int num = views2.Count();
		array = views2;
		foreach (ZNetView val in array)
		{
			if (ComponentName != "" && !Object.op_Implicit((Object)(object)((Component)val).GetComponentInChildren(Component)) && (force || (views2.Length == 1 && AddComponentAutomatically)) && !Object.op_Implicit((Object)(object)((Component)val).GetComponent<DungeonGenerator>()))
			{
				TweakActions.AddComponent(val, ComponentName);
			}
			foreach (KeyValuePair<string, object> item in Postprocess(val, operations))
			{
				Type type = SupportedOperations[item.Key];
				string prefabName = Utils.GetPrefabName(((Component)val).gameObject);
				string text = "";
				text = ((type == typeof(int)) ? DoOperation(val, item.Key, (int?)item.Value) : ((type == typeof(long)) ? DoOperation(val, item.Key, (long?)item.Value) : ((type == typeof(float)) ? DoOperation(val, item.Key, (float?)item.Value) : ((type == typeof(string[])) ? DoOperation(val, item.Key, (string[])item.Value) : ((!(type == typeof(bool))) ? DoOperation(val, item.Key, (string)item.Value) : DoOperation(val, item.Key, (bool?)item.Value))))));
				if (!(text == ""))
				{
					string text2 = text.Replace("¤", prefabName);
					if (num == 1)
					{
						Helper.AddMessage(context2, text2, false);
					}
					else
					{
						context2.AddString(text2);
					}
				}
			}
		}
		array = views2;
		foreach (ZNetView val2 in array)
		{
			val2.GetZDO().SetOwner(dictionary[val2.GetZDO().m_uid]);
		}
		UndoHelper.EndAction();
		array = views2;
		foreach (ZNetView view2 in array)
		{
			Postprocess(Actions.Refresh(view2));
		}
	}

	protected void Init(string name, string description)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		List<string> namedParameters = TweakAutoComplete.WithFilters(AutoComplete.Keys.ToList());
		AutoComplete.Register(name, (Func<int, List<string>>)((int index) => namedParameters), TweakAutoComplete.WithFilters(AutoComplete));
		Helper.Command(name, description, (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			Parameters = new TweakParameters(SupportedOperations);
			Parameters.ParseCommand(args);
			ZNetView[] objects = Parameters.GetObjects();
			Execute(args.Context, Parameters.Chance, Parameters.Force, Parameters.Operations, objects);
		});
	}

	public TweakCommand()
	{
	}
}
