using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class WindCommand
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
			EnvMan instance = EnvMan.instance;
			if (Object.op_Implicit((Object)(object)instance))
			{
				if (args.Length < 3)
				{
					Helper.AddMessage(args.Context, $"Wind: {instance.GetWindIntensity()}.");
					return;
				}
				float num = Parse.Float(args[1]);
				float num2 = Parse.Float(args[2]);
				EnvMan.instance.SetDebugWind(num, num2);
			}
		}

		internal List<string> _003C_002Ector_003Eb__0_1(int index)
		{
			return index switch
			{
				0 => ParameterInfo.Create("Angle", "a number (from -360.0 to 360.0)"), 
				1 => ParameterInfo.Create("Intensity", "a positive number (from 0.0 to 1.0)"), 
				_ => ParameterInfo.None, 
			};
		}
	}

	public WindCommand()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
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
					if (args.Length < 3)
					{
						Helper.AddMessage(args.Context, $"Wind: {instance.GetWindIntensity()}.");
					}
					else
					{
						float num = Parse.Float(args[1]);
						float num2 = Parse.Float(args[2]);
						EnvMan.instance.SetDebugWind(num, num2);
					}
				}
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		new ConsoleCommand("wind", "[angle] [intensity] - Prints or overrides the wind.", (ConsoleEvent)obj, true, true, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.Register("wind", (int index) => index switch
		{
			0 => ParameterInfo.Create("Angle", "a number (from -360.0 to 360.0)"), 
			1 => ParameterInfo.Create("Intensity", "a positive number (from 0.0 to 1.0)"), 
			_ => ParameterInfo.None, 
		});
	}
}
