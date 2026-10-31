using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Service;

namespace ServerDevcommands;

public class UndoManager
{
	private static readonly BindingFlags Binding = BindingFlags.Instance | BindingFlags.Public;

	private static List<object> History = new List<object>();

	private static int Index = -1;

	private static bool Executing = false;

	public static int MaxSteps = 50;

	public static void Clear()
	{
		History.Clear();
		Index = -1;
		Executing = false;
	}

	public static void Add(IUndoAction action)
	{
		Add((object)action);
	}

	private static void Add(object action)
	{
		if (!Executing)
		{
			if (History.Count > MaxSteps - 1)
			{
				History = History.Skip(History.Count - MaxSteps + 1).ToList();
			}
			if (Index < History.Count - 1)
			{
				History = History.Take(Index + 1).ToList();
			}
			History.Add(action);
			Index = History.Count - 1;
		}
	}

	public static bool Undo(Terminal terminal)
	{
		if (Index < 0)
		{
			Helper.AddMessage(terminal, "Nothing to undo.");
			return false;
		}
		Executing = true;
		try
		{
			object obj = History[Index];
			object obj2 = obj.GetType().GetMethod("Undo", Binding).Invoke(obj, null);
			if (string.IsNullOrEmpty((string)obj2))
			{
				obj2 = obj.GetType().GetMethod("UndoMessage", Binding).Invoke(obj, null);
			}
			Helper.AddMessage(terminal, (string)obj2);
		}
		catch (Exception ex)
		{
			Log.Warning(ex.ToString());
		}
		Index--;
		Executing = false;
		return true;
	}

	public static bool Redo(Terminal terminal)
	{
		if (Index < History.Count - 1)
		{
			Executing = true;
			Index++;
			try
			{
				object obj = History[Index];
				object obj2 = obj.GetType().GetMethod("Redo", Binding).Invoke(obj, null);
				if (string.IsNullOrEmpty((string)obj2))
				{
					obj2 = obj.GetType().GetMethod("RedoMessage", Binding).Invoke(obj, null);
				}
				Helper.AddMessage(terminal, (string)obj2);
			}
			catch (Exception ex)
			{
				Log.Warning(ex.ToString());
			}
			Executing = false;
			return true;
		}
		Helper.AddMessage(terminal, "Nothing to redo.");
		return false;
	}
}
