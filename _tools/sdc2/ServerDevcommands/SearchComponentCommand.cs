using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class SearchComponentCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<ZoneLocation, string> _003C_003E9__0_3;

		public static ConsoleEvent _003C_003E9__0_0;

		public static Func<int, List<string>> _003C_003E9__0_1;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			_003C_003Ec__DisplayClass0_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass0_0();
			Helper.ArgsCheck(args, 2, "Missing the object/location");
			Helper.ArgsCheck(args, 3, "Missing the component");
			CS_0024_003C_003E8__locals0.component = args[2].ToLowerInvariant();
			string text = "";
			string value = "";
			string[] array = CS_0024_003C_003E8__locals0.component.Split(',');
			if (array.Length == 3)
			{
				CS_0024_003C_003E8__locals0.component = array[0];
				text = array[1];
				value = array[2];
			}
			int num = args.TryParameterInt(2, 5);
			IEnumerable<string> source = from l in ZoneSystem.instance.m_locations.Where(Helper.IsValid).Where(delegate(ZoneLocation l)
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					l.m_prefab.Load();
					l.m_prefab.Asset.GetComponentsInChildren<MonoBehaviour>(ZNetView.m_tempComponents);
					l.m_prefab.Release();
					return ZNetView.m_tempComponents.Any((MonoBehaviour s) => ((object)s).GetType().Name.ToLowerInvariant() == CS_0024_003C_003E8__locals0.component);
				})
				select l.m_prefab.Name;
			string[] array2 = ((args[1] == "location") ? source.ToArray() : ((text == "") ? ComponentInfo.PrefabsByComponent(CS_0024_003C_003E8__locals0.component) : ComponentInfo.PrefabsByField(CS_0024_003C_003E8__locals0.component, text, value)));
			if (array2.Length > 100)
			{
				args.Context.AddString("Over 100 results, printing to the log file.");
				ZLog.Log((object)("\n" + string.Join("\n", array2)));
				return;
			}
			int num2 = (int)Math.Ceiling((float)array2.Length / (float)num);
			string[] array3 = new string[num2];
			for (int i = 0; i < array2.Length; i += num2)
			{
				if (array2.Length - i < num2)
				{
					num2 = array2.Length - i;
					array3 = new string[num2];
				}
				Array.Copy(array2, i, array3, 0, num2);
				args.Context.AddString(string.Join(", ", array3));
			}
		}

		internal string _003C_002Ector_003Eb__0_3(ZoneLocation l)
		{
			return l.m_prefab.Name;
		}

		internal List<string> _003C_002Ector_003Eb__0_1(int index)
		{
			return index switch
			{
				0 => new List<string>(2) { "object", "location" }, 
				1 => ParameterInfo.Create("Search component"), 
				2 => ParameterInfo.Create("Max lines", "number (default 5)"), 
				_ => ParameterInfo.None, 
			};
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_0
	{
		public string component;

		public Func<MonoBehaviour, bool> _003C_003E9__4;

		internal bool _003C_002Ector_003Eb__2(ZoneLocation l)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			l.m_prefab.Load();
			l.m_prefab.Asset.GetComponentsInChildren<MonoBehaviour>(ZNetView.m_tempComponents);
			l.m_prefab.Release();
			return ZNetView.m_tempComponents.Any((MonoBehaviour s) => ((object)s).GetType().Name.ToLowerInvariant() == component);
		}

		internal bool _003C_002Ector_003Eb__4(MonoBehaviour s)
		{
			return ((object)s).GetType().Name.ToLowerInvariant() == component;
		}
	}

	public SearchComponentCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				Helper.ArgsCheck(args, 2, "Missing the object/location");
				Helper.ArgsCheck(args, 3, "Missing the component");
				string component = args[2].ToLowerInvariant();
				string text = "";
				string value = "";
				string[] array = component.Split(',');
				if (array.Length == 3)
				{
					component = array[0];
					text = array[1];
					value = array[2];
				}
				int num = args.TryParameterInt(2, 5);
				IEnumerable<string> source = from l in ZoneSystem.instance.m_locations.Where(Helper.IsValid).Where(delegate(ZoneLocation l)
					{
						//IL_0006: Unknown result type (might be due to invalid IL or missing references)
						l.m_prefab.Load();
						l.m_prefab.Asset.GetComponentsInChildren<MonoBehaviour>(ZNetView.m_tempComponents);
						l.m_prefab.Release();
						return ZNetView.m_tempComponents.Any((MonoBehaviour s) => ((object)s).GetType().Name.ToLowerInvariant() == component);
					})
					select l.m_prefab.Name;
				string[] array2 = ((args[1] == "location") ? source.ToArray() : ((text == "") ? ComponentInfo.PrefabsByComponent(component) : ComponentInfo.PrefabsByField(component, text, value)));
				if (array2.Length > 100)
				{
					args.Context.AddString("Over 100 results, printing to the log file.");
					ZLog.Log((object)("\n" + string.Join("\n", array2)));
				}
				else
				{
					int num2 = (int)Math.Ceiling((float)array2.Length / (float)num);
					string[] array3 = new string[num2];
					for (int i = 0; i < array2.Length; i += num2)
					{
						if (array2.Length - i < num2)
						{
							num2 = array2.Length - i;
							array3 = new string[num2];
						}
						Array.Copy(array2, i, array3, 0, num2);
						args.Context.AddString(string.Join(", ", array3));
					}
				}
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		Helper.Command("search_component", "[object/location] [component] [max_lines=5] - Prints object or location ids having the component.", (ConsoleEvent)obj);
		AutoComplete.Register("search_component", (int index) => index switch
		{
			0 => new List<string>(2) { "object", "location" }, 
			1 => ParameterInfo.Create("Search component"), 
			2 => ParameterInfo.Create("Max lines", "number (default 5)"), 
			_ => ParameterInfo.None, 
		});
	}
}
