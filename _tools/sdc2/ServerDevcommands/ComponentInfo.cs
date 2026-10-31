using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using UnityEngine;

namespace ServerDevcommands;

public class ComponentInfo
{
	private static Type[]? types;

	private static Dictionary<string, Type>? nameToType;

	private static Dictionary<string, HashSet<string>> PrefabComponents = new Dictionary<string, HashSet<string>>();

	public static Type[] Types => types ?? (types = LoadTypes());

	private static Dictionary<string, Type> NameToType => nameToType ?? (nameToType = LoadNames());

	private static Dictionary<string, Type> LoadNames()
	{
		Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
		Type[] array = Types;
		foreach (Type type in array)
		{
			dictionary[type.Name.ToLowerInvariant()] = type;
		}
		return dictionary;
	}

	private static Type[] LoadTypes()
	{
		List<Assembly> list = new List<Assembly>();
		list.Add(Assembly.GetAssembly(typeof(ZNetView)));
		list.AddRange(from p in Chainloader.PluginInfos.Values
			where (Object)(object)p.Instance != (Object)null
			select ((object)p.Instance).GetType().Assembly);
		List<Assembly> source = list;
		Type baseType = typeof(MonoBehaviour);
		List<Type> list2 = new List<Type>();
		list2.AddRange(source.SelectMany(delegate(Assembly s)
		{
			try
			{
				return s.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				return ex.Types.Where((Type t) => t != null);
			}
		}).Where(delegate(Type t)
		{
			try
			{
				return baseType.IsAssignableFrom(t);
			}
			catch
			{
				return false;
			}
		}).Distinct());
		return list2.ToArray();
	}

	private static Type[] GetTypes(HashSet<string> components)
	{
		return components.Select(delegate(string c)
		{
			if (!NameToType.TryGetValue(c.ToLowerInvariant(), out Type value))
			{
				throw new InvalidOperationException("Type " + c + " not recognized.");
			}
			return value;
		}).ToArray();
	}

	private static void SearchComponents()
	{
		Dictionary<string, HashSet<string>> dictionary = new Dictionary<string, HashSet<string>>();
		foreach (KeyValuePair<int, GameObject> namedPrefab in ZNetScene.instance.m_namedPrefabs)
		{
			if (!Object.op_Implicit((Object)(object)namedPrefab.Value))
			{
				continue;
			}
			namedPrefab.Value.GetComponentsInChildren<MonoBehaviour>(ZNetView.m_tempComponents);
			Dictionary<string, HashSet<string>> dictionary2 = dictionary;
			string name = ((Object)namedPrefab.Value).name;
			HashSet<string> hashSet = new HashSet<string>();
			foreach (string item in from s in ZNetView.m_tempComponents
				where Object.op_Implicit((Object)(object)s)
				select ((object)s).GetType().Name.ToLowerInvariant())
			{
				hashSet.Add(item);
			}
			dictionary2[name] = hashSet;
		}
		PrefabComponents = dictionary;
	}

	public static string[] PrefabsByComponent(string component)
	{
		if (PrefabComponents.Count == 0)
		{
			SearchComponents();
		}
		string lower = component.ToLowerInvariant();
		return (from kvp in PrefabComponents
			where kvp.Value.Contains(lower)
			select kvp.Key).ToArray();
	}

	public static string[] PrefabsByField(string component, string field, string value)
	{
		string component2 = component;
		string field2 = field;
		string value2 = value;
		string[] source = PrefabsByComponent(component2);
		Type type = Types.FirstOrDefault((Type t) => t.Name.ToLowerInvariant() == component2);
		if (type == null)
		{
			return Array.Empty<string>();
		}
		List<string> list = new List<string>();
		list.AddRange(source.Where(delegate(string prefab)
		{
			GameObject prefab2 = ZNetScene.instance.GetPrefab(prefab);
			if (!Object.op_Implicit((Object)(object)prefab2))
			{
				return false;
			}
			Component componentInChildren = prefab2.GetComponentInChildren(type);
			if (!Object.op_Implicit((Object)(object)componentInChildren))
			{
				return false;
			}
			FieldInfo field3 = ((object)componentInChildren).GetType().GetField(field2);
			return !(field3 == null) && field3.GetValue(componentInChildren)?.ToString() == value2;
		}));
		return list.ToArray();
	}

	public static string[] Get(ZNetView view)
	{
		((Component)view).GetComponentsInChildren<MonoBehaviour>(ZNetView.m_tempComponents);
		return (from s in ZNetView.m_tempComponents
			where Object.op_Implicit((Object)(object)s)
			select ((object)s).GetType().Name).ToArray();
	}

	public static bool HasType(ZNetView view, Type[] types)
	{
		Type[] types2 = types;
		((Component)view).GetComponentsInChildren<MonoBehaviour>(ZNetView.m_tempComponents);
		return ZNetView.m_tempComponents.Any((MonoBehaviour s) => Object.op_Implicit((Object)(object)s) && types2.Contains<Type>(((object)s).GetType()));
	}

	public static IEnumerable<ZNetView> HaveComponent(IEnumerable<ZNetView> views, HashSet<string> components)
	{
		Type[] types = GetTypes(components);
		return views.Where((ZNetView view) => HasType(view, types));
	}

	public static bool HasComponent(ZNetView view, HashSet<string> components)
	{
		Type[] array = GetTypes(components);
		return HasType(view, array);
	}
}
