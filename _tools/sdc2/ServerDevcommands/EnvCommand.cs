using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class EnvCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__0_0;

		public static Func<int, List<string>> _003C_003E9__0_1;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			_003C_003Ec__DisplayClass0_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass0_0();
			EnvMan instance = EnvMan.instance;
			if (!Object.op_Implicit((Object)(object)instance))
			{
				return;
			}
			if (args.Length < 2)
			{
				Helper.AddMessage(args.Context, $"Environment: {instance.GetCurrentEnvironment()}.");
				return;
			}
			CS_0024_003C_003E8__locals0.text = string.Join(" ", args.Args, 1, args.Args.Length - 1);
			if (!EnvMan.instance.m_environments.Any((EnvSetup env) => env.m_name == CS_0024_003C_003E8__locals0.text))
			{
				CS_0024_003C_003E8__locals0.text = CS_0024_003C_003E8__locals0.text.Replace("_", " ");
			}
			Helper.AddMessage(args.Context, "Setting debug environment: " + CS_0024_003C_003E8__locals0.text);
			instance.m_debugEnv = CS_0024_003C_003E8__locals0.text;
		}

		internal List<string> _003C_002Ector_003Eb__0_1(int index)
		{
			if (index != 0)
			{
				return ParameterInfo.None;
			}
			return ParameterInfo.Environments;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_0
	{
		public string text;

		internal bool _003C_002Ector_003Eb__2(EnvSetup env)
		{
			return env.m_name == text;
		}
	}

	public EnvCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				EnvMan instance = EnvMan.instance;
				if (Object.op_Implicit((Object)(object)instance))
				{
					if (args.Length < 2)
					{
						Helper.AddMessage(args.Context, $"Environment: {instance.GetCurrentEnvironment()}.");
					}
					else
					{
						string text = string.Join(" ", args.Args, 1, args.Args.Length - 1);
						if (!EnvMan.instance.m_environments.Any((EnvSetup env) => env.m_name == text))
						{
							text = text.Replace("_", " ");
						}
						Helper.AddMessage(args.Context, "Setting debug environment: " + text);
						instance.m_debugEnv = text;
					}
				}
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		Helper.Command("env", "[value] - Prints or overrides the environment.", (ConsoleEvent)obj);
		AutoComplete.Register("env", (int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Environments);
	}
}
