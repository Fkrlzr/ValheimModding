using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class StartEventCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__1_0;

		public static Func<RandomEvent, string> _003C_003E9__1_2;

		public static Func<int, List<string>> _003C_003E9__1_1;

		internal void _003C_002Ector_003Eb__1_0(ConsoleEventArgs args)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (args.Length >= 2)
			{
				var (args2, position) = Helper.AddPlayerPosXZ(args.Args, 2);
				if (ZNet.instance.IsServer())
				{
					DoStartEvent(args2, position, args.Context);
				}
				else
				{
					ServerExecution.Send((IEnumerable<string>)args2);
				}
			}
		}

		internal List<string> _003C_002Ector_003Eb__1_1(int index)
		{
			return index switch
			{
				0 => RandEventSystem.instance.m_events.Select((RandomEvent ev) => ev.m_name).ToList(), 
				1 => ParameterInfo.Create("X coordinate", "number (default is the current position)"), 
				2 => ParameterInfo.Create("Z coordinate", "number (default is the current position)"), 
				_ => ParameterInfo.None, 
			};
		}

		internal string _003C_002Ector_003Eb__1_2(RandomEvent ev)
		{
			return ev.m_name;
		}
	}

	private static void DoStartEvent(string[] args, Vector3 position, Terminal context)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (args.Length >= 2)
		{
			string text = args[1];
			if (!RandEventSystem.instance.HaveEvent(text))
			{
				context.AddString("Random event not found:" + text);
			}
			else
			{
				RandEventSystem.instance.SetRandomEventByName(text, new Vector3(position.x, 0f, position.z));
			}
		}
	}

	public StartEventCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__1_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				if (args.Length >= 2)
				{
					var (args2, position) = Helper.AddPlayerPosXZ(args.Args, 2);
					if (ZNet.instance.IsServer())
					{
						DoStartEvent(args2, position, args.Context);
					}
					else
					{
						ServerExecution.Send((IEnumerable<string>)args2);
					}
				}
			};
			_003C_003Ec._003C_003E9__1_0 = val;
			obj = (object)val;
		}
		Helper.Command("event", "[name] [x] [z] - start event.", (ConsoleEvent)obj);
		AutoComplete.Register("event", (int index) => index switch
		{
			0 => RandEventSystem.instance.m_events.Select((RandomEvent ev) => ev.m_name).ToList(), 
			1 => ParameterInfo.Create("X coordinate", "number (default is the current position)"), 
			2 => ParameterInfo.Create("Z coordinate", "number (default is the current position)"), 
			_ => ParameterInfo.None, 
		});
	}
}
