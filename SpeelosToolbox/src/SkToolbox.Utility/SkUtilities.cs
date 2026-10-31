using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SkToolbox.Utility;

public static class SkUtilities
{
	public enum Status
	{
		Initialized,
		Loading,
		Ready,
		Error,
		Unload
	}

	public static bool ConvertInternalWarningsErrors = false;

	public static BindingFlags BindFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	private static readonly HashSet<string> reportedMissing = new HashSet<string>();

	private static void ReportMissing(string kind, Type type, string name)
	{
		string item = kind + ":" + type.FullName + "." + name;
		if (reportedMissing.Add(item))
		{
			Logz(new string[1] { "REFLECTION" }, new string[1] { kind + " '" + name + "' not found on " + type.FullName + " (renamed in this Valheim version?)" }, (LogType)0);
		}
	}

	public static FieldInfo FindField(Type type, string fieldName)
	{
		Type type2 = type;
		while (type2 != null)
		{
			FieldInfo field = type2.GetField(fieldName, BindFlags | BindingFlags.DeclaredOnly);
			if (field != null)
			{
				return field;
			}
			type2 = type2.BaseType;
		}
		return null;
	}

	public static PropertyInfo FindProperty(Type type, string propertyName)
	{
		Type type2 = type;
		while (type2 != null)
		{
			PropertyInfo property = type2.GetProperty(propertyName, BindFlags | BindingFlags.DeclaredOnly);
			if (property != null)
			{
				return property;
			}
			type2 = type2.BaseType;
		}
		return null;
	}

	public static MethodInfo FindMethod(Type type, string methodName)
	{
		Type type2 = type;
		while (type2 != null)
		{
			MethodInfo method = type2.GetMethod(methodName, BindFlags | BindingFlags.DeclaredOnly);
			if (method != null)
			{
				return method;
			}
			type2 = type2.BaseType;
		}
		return null;
	}

	public static void SetPrivateField(this object obj, string fieldName, object value)
	{
		FieldInfo fieldInfo = FindField(obj.GetType(), fieldName);
		if (fieldInfo == null)
		{
			ReportMissing("Field", obj.GetType(), fieldName);
		}
		else
		{
			fieldInfo.SetValue(obj, value);
		}
	}

	public static T GetPrivateField<T>(this object obj, string fieldName)
	{
		FieldInfo fieldInfo = FindField(obj.GetType(), fieldName);
		if (fieldInfo == null)
		{
			ReportMissing("Field", obj.GetType(), fieldName);
			return default(T);
		}
		object value = fieldInfo.GetValue(obj);
		if (!(value is T))
		{
			return default(T);
		}
		return (T)value;
	}

	public static void SetPrivateProperty(this object obj, string propertyName, object value)
	{
		PropertyInfo propertyInfo = FindProperty(obj.GetType(), propertyName);
		if (propertyInfo == null)
		{
			ReportMissing("Property", obj.GetType(), propertyName);
		}
		else
		{
			propertyInfo.SetValue(obj, value, null);
		}
	}

	public static T GetPrivateProperty<T>(this object obj, string propertyName)
	{
		PropertyInfo propertyInfo = FindProperty(obj.GetType(), propertyName);
		if (propertyInfo == null)
		{
			ReportMissing("Property", obj.GetType(), propertyName);
			return default(T);
		}
		object value = propertyInfo.GetValue(obj, null);
		if (!(value is T))
		{
			return default(T);
		}
		return (T)value;
	}

	public static void InvokePrivateMethod(this object obj, string methodName, object[] methodParams)
	{
		MethodInfo methodInfo = FindMethod(obj.GetType(), methodName);
		if (methodInfo == null)
		{
			ReportMissing("Method", obj.GetType(), methodName);
		}
		else
		{
			methodInfo.Invoke(obj, methodParams);
		}
	}

	public static Component CopyComponent(Component original, Type originalType, Type overridingType, GameObject destination)
	{
		Component val = destination.AddComponent(overridingType);
		FieldInfo[] fields = originalType.GetFields(BindFlags);
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			object value = fieldInfo.GetValue(original);
			fieldInfo.SetValue(val, value);
		}
		return val;
	}

	public static void Logz(string[] categories, string[] messages, LogType logType = (LogType)3)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Invalid comparison between Unknown and I4
		string text = string.Empty;
		if (categories != null)
		{
			foreach (string text2 in categories)
			{
				text = text + " (" + text2 + ") -> ";
			}
		}
		if (messages != null)
		{
			foreach (string text3 in messages)
			{
				text = ((text3 == null) ? (text + "NULL | ") : (text + text3 + " | "));
			}
			text = text.Remove(text.Length - 2, 1);
		}
		if (!ConvertInternalWarningsErrors)
		{
			if ((int)logType != 0)
			{
				if ((int)logType == 2)
				{
					Debug.LogWarning((object)("(Speelo's Toolbox) -> " + text));
				}
				else
				{
					Debug.Log((object)("(Speelo's Toolbox) -> " + text));
				}
			}
			else
			{
				Debug.LogError((object)("(Speelo's Toolbox) -> " + text));
			}
		}
		else
		{
			Debug.Log((object)("(Speelo's Toolbox) -> " + text));
		}
	}

	public static string Logr(string[] categories, string[] messages)
	{
		string text = string.Empty;
		if (categories != null)
		{
			foreach (string text2 in categories)
			{
				text = text + " (" + text2 + ") -> ";
			}
		}
		if (messages != null)
		{
			foreach (string text3 in messages)
			{
				text = ((text3 == null) ? (text + "NULL | ") : (text + text3 + " | "));
			}
			text = text.Remove(text.Length - 2, 1);
		}
		return "(Speelo's Toolbox) -> " + text;
	}

	public static void Logz(string message)
	{
		string empty = string.Empty;
		empty += " (OUT) -> ";
		empty = empty + message + " ";
		Debug.Log((object)("(Speelo's Toolbox) -> " + empty));
	}

	public static void RectFilled(float x, float y, float width, float height, Texture2D text)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		GUI.DrawTexture(new Rect(x, y, width, height), (Texture)(object)text);
	}

	public static void RectOutlined(float x, float y, float width, float height, Texture2D text, float thickness = 1f)
	{
		RectFilled(x, y, thickness, height, text);
		RectFilled(x + width - thickness, y, thickness, height, text);
		RectFilled(x + thickness, y, width - thickness * 2f, thickness, text);
		RectFilled(x + thickness, y + height - thickness, width - thickness * 2f, thickness, text);
	}
}
