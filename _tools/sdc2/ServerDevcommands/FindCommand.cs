using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Service;
using UnityEngine;

namespace ServerDevcommands;

public class FindCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<LocationInstance, Tuple<string, Vector3>> _003C_003E9__0_3;

		public static Func<ZDO, Tuple<string, Vector3>> _003C_003E9__0_5;

		public static Func<Tuple<string, Vector3>, string> _003C_003E9__0_8;

		public static Func<Tuple<string, Vector3>, string> _003C_003E9__0_9;

		public static ConsoleEvent _003C_003E9__0_0;

		public static Func<int, List<string>> _003C_003E9__0_1;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			_003C_003Ec__DisplayClass0_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass0_0
			{
				args = args
			};
			Helper.ArgsCheck(CS_0024_003C_003E8__locals0.args, 2, "Missing the search term.");
			if (CS_0024_003C_003E8__locals0.args.Length < 3)
			{
				ConsoleEventArgs args2 = CS_0024_003C_003E8__locals0.args;
				string[] args3 = CS_0024_003C_003E8__locals0.args.Args;
				int num = 0;
				string[] array = new string[1 + args3.Length];
				string[] array2 = args3;
				foreach (string text in array2)
				{
					array[num] = text;
					num++;
				}
				array[num] = "10";
				num++;
				args2.Args = array;
			}
			string[] args4;
			(args4, CS_0024_003C_003E8__locals0.pos) = Helper.AddPlayerPosXZY(CS_0024_003C_003E8__locals0.args.Args, 3);
			if (!ZNet.instance.IsServer())
			{
				ServerExecution.Send((IEnumerable<string>)args4);
				return;
			}
			CS_0024_003C_003E8__locals0.prefabs = Selector.GetAllPrefabs(new string[1] { CS_0024_003C_003E8__locals0.args[1] });
			List<LocationInstance> list = (from l in ZoneSystem.instance.GetLocationList()
				where Helper.IsValid(l.m_location) && l.m_location.m_prefab.Name == CS_0024_003C_003E8__locals0.args[1]
				select l).ToList();
			if (CS_0024_003C_003E8__locals0.prefabs.Count <= 0 && list.Count <= 0)
			{
				_003C_003Ec__DisplayClass0_1 CS_0024_003C_003E8__locals1 = new _003C_003Ec__DisplayClass0_1
				{
					lower = CS_0024_003C_003E8__locals0.args[1].ToLower()
				};
				list = (from l in ZoneSystem.instance.GetLocationList()
					where Helper.IsValid(l.m_location) && l.m_location.m_prefab.Name.ToLower().Contains(CS_0024_003C_003E8__locals1.lower)
					select l).ToList();
				CS_0024_003C_003E8__locals0.prefabs = Selector.GetAllPrefabs(new string[1] { "*" + CS_0024_003C_003E8__locals0.args[1] + "*" });
			}
			List<Tuple<string, Vector3>> list2 = list.Select((LocationInstance l) => Tuple.Create<string, Vector3>(l.m_location.m_prefab.Name, l.m_position)).ToList();
			IEnumerable<ZDO> source = ZDOMan.instance.m_objectsByID.Values.Where((ZDO zdo) => zdo.IsValid() && CS_0024_003C_003E8__locals0.prefabs.Contains(zdo.GetPrefab()));
			list2.AddRange(source.Select((ZDO zdo) => Tuple.Create<string, Vector3>(((Object)ZNetScene.instance.GetPrefab(zdo.m_prefab)).name, zdo.GetPosition())));
			list2.Sort((Tuple<string, Vector3> a, Tuple<string, Vector3> b) => Vector3.Distance(a.Item2, CS_0024_003C_003E8__locals0.pos).CompareTo(Vector3.Distance(b.Item2, CS_0024_003C_003E8__locals0.pos)));
			int count = list2.Count;
			list2 = list2.Take(Parse.Int(CS_0024_003C_003E8__locals0.args.Args, 2, 10)).ToList();
			List<string> values = list2.Select((Tuple<string, Vector3> p) => Format(CS_0024_003C_003E8__locals0.pos, p.Item2, p.Item1)).ToList();
			CS_0024_003C_003E8__locals0.args.Context.AddString($"Found {count} of {CS_0024_003C_003E8__locals0.args[1]}. Showing {list2.Count} closest:");
			CS_0024_003C_003E8__locals0.args.Context.AddString(string.Join("\n", values));
			if (RedirectOutput.Target == null)
			{
				ServerExecution.RPC_Do_Pins(null, string.Join("|", list2.Select((Tuple<string, Vector3> item) => Helper.PrintVectorXZY(item.Item2))));
				return;
			}
			RedirectOutput.Target.Invoke(ServerExecution.RPC_Pins, new object[1] { string.Join("|", list2.Select((Tuple<string, Vector3> item) => Helper.PrintVectorXZY(item.Item2))) });
		}

		internal Tuple<string, Vector3> _003C_002Ector_003Eb__0_3(LocationInstance l)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			return Tuple.Create<string, Vector3>(l.m_location.m_prefab.Name, l.m_position);
		}

		internal Tuple<string, Vector3> _003C_002Ector_003Eb__0_5(ZDO zdo)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return Tuple.Create<string, Vector3>(((Object)ZNetScene.instance.GetPrefab(zdo.m_prefab)).name, zdo.GetPosition());
		}

		internal string _003C_002Ector_003Eb__0_8(Tuple<string, Vector3> item)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Helper.PrintVectorXZY(item.Item2);
		}

		internal string _003C_002Ector_003Eb__0_9(Tuple<string, Vector3> item)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Helper.PrintVectorXZY(item.Item2);
		}

		internal List<string> _003C_002Ector_003Eb__0_1(int index)
		{
			return index switch
			{
				0 => ParameterInfo.Ids, 
				1 => ParameterInfo.Create("Max amount", "a positive integer (default 10)"), 
				2 => ParameterInfo.Create("X coordinate", "if not specified, the current position is used"), 
				3 => ParameterInfo.Create("Z coordinate", "if not specified, the current position is used"), 
				_ => ParameterInfo.None, 
			};
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_0
	{
		public ConsoleEventArgs args;

		public HashSet<int> prefabs;

		public Vector3 pos;

		internal bool _003C_002Ector_003Eb__2(LocationInstance l)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (Helper.IsValid(l.m_location))
			{
				return l.m_location.m_prefab.Name == args[1];
			}
			return false;
		}

		internal bool _003C_002Ector_003Eb__4(ZDO zdo)
		{
			if (zdo.IsValid())
			{
				return prefabs.Contains(zdo.GetPrefab());
			}
			return false;
		}

		internal int _003C_002Ector_003Eb__6(Tuple<string, Vector3> a, Tuple<string, Vector3> b)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			return Vector3.Distance(a.Item2, pos).CompareTo(Vector3.Distance(b.Item2, pos));
		}

		internal string _003C_002Ector_003Eb__7(Tuple<string, Vector3> p)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return Format(pos, p.Item2, p.Item1);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_1
	{
		public string lower;

		internal bool _003C_002Ector_003Eb__10(LocationInstance l)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (Helper.IsValid(l.m_location))
			{
				return l.m_location.m_prefab.Name.ToLower().Contains(lower);
			}
			return false;
		}
	}

	public FindCommand()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		ConsoleCommand val = Terminal.commands["find"];
		string description = val.Description;
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val2 = delegate(ConsoleEventArgs args)
			{
				//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
				Helper.ArgsCheck(args, 2, "Missing the search term.");
				if (args.Length < 3)
				{
					ConsoleEventArgs val3 = args;
					string[] args2 = args.Args;
					int num = 0;
					string[] array = new string[1 + args2.Length];
					string[] array2 = args2;
					foreach (string text in array2)
					{
						array[num] = text;
						num++;
					}
					array[num] = "10";
					num++;
					val3.Args = array;
				}
				var (args3, pos) = Helper.AddPlayerPosXZY(args.Args, 3);
				if (!ZNet.instance.IsServer())
				{
					ServerExecution.Send((IEnumerable<string>)args3);
				}
				else
				{
					HashSet<int> prefabs = Selector.GetAllPrefabs(new string[1] { args[1] });
					List<LocationInstance> list = (from l in ZoneSystem.instance.GetLocationList()
						where Helper.IsValid(l.m_location) && l.m_location.m_prefab.Name == args[1]
						select l).ToList();
					if (prefabs.Count <= 0 && list.Count <= 0)
					{
						string lower = args[1].ToLower();
						list = (from l in ZoneSystem.instance.GetLocationList()
							where Helper.IsValid(l.m_location) && l.m_location.m_prefab.Name.ToLower().Contains(lower)
							select l).ToList();
						prefabs = Selector.GetAllPrefabs(new string[1] { "*" + args[1] + "*" });
					}
					List<Tuple<string, Vector3>> list2 = list.Select((LocationInstance l) => Tuple.Create<string, Vector3>(l.m_location.m_prefab.Name, l.m_position)).ToList();
					IEnumerable<ZDO> source = ZDOMan.instance.m_objectsByID.Values.Where((ZDO zdo) => zdo.IsValid() && prefabs.Contains(zdo.GetPrefab()));
					list2.AddRange(source.Select((ZDO zdo) => Tuple.Create<string, Vector3>(((Object)ZNetScene.instance.GetPrefab(zdo.m_prefab)).name, zdo.GetPosition())));
					list2.Sort((Tuple<string, Vector3> a, Tuple<string, Vector3> b) => Vector3.Distance(a.Item2, pos).CompareTo(Vector3.Distance(b.Item2, pos)));
					int count = list2.Count;
					list2 = list2.Take(Parse.Int(args.Args, 2, 10)).ToList();
					List<string> values = list2.Select((Tuple<string, Vector3> p) => Format(pos, p.Item2, p.Item1)).ToList();
					args.Context.AddString($"Found {count} of {args[1]}. Showing {list2.Count} closest:");
					args.Context.AddString(string.Join("\n", values));
					if (RedirectOutput.Target == null)
					{
						ServerExecution.RPC_Do_Pins(null, string.Join("|", list2.Select((Tuple<string, Vector3> item) => Helper.PrintVectorXZY(item.Item2))));
					}
					else
					{
						RedirectOutput.Target.Invoke(ServerExecution.RPC_Pins, new object[1] { string.Join("|", list2.Select((Tuple<string, Vector3> item) => Helper.PrintVectorXZY(item.Item2))) });
					}
				}
			};
			_003C_003Ec._003C_003E9__0_0 = val2;
			obj = (object)val2;
		}
		Helper.Command("find", description, (ConsoleEvent)obj);
		AutoComplete.Register("find", (int index) => index switch
		{
			0 => ParameterInfo.Ids, 
			1 => ParameterInfo.Create("Max amount", "a positive integer (default 10)"), 
			2 => ParameterInfo.Create("X coordinate", "if not specified, the current position is used"), 
			3 => ParameterInfo.Create("Z coordinate", "if not specified, the current position is used"), 
			_ => ParameterInfo.None, 
		});
	}

	private static string Format(Vector3 pos, Vector3 p, string name)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector3.Distance(pos, p);
		return string.Format(Settings.FindFormat.Replace("{pos_x", "{0").Replace("{pos_y", "{1").Replace("{pos_z", "{2")
			.Replace("{distance", "{3")
			.Replace("{name", "{4"), p.x, p.y, p.z, num, name);
	}
}
