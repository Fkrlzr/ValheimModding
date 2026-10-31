using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class PosCommand
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
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			Vector3 position = ((Component)Helper.GetPlayer()).transform.position;
			int num = 0;
			if (args.Length >= 2)
			{
				num = args.TryParameterInt(1, 0);
				if (num == 0)
				{
					position = Helper.FindPlayer(args[1]).m_position;
				}
			}
			if (args.Length >= 3)
			{
				num = args.TryParameterInt(2, 0);
			}
			Helper.AddMessage(args.Context, "Player position (X,Z,Y): (" + position.x.ToString($"F{num}") + ", " + position.z.ToString($"F{num}") + ", " + position.y.ToString($"F{num}") + ")");
		}

		internal List<string> _003C_002Ector_003Eb__0_1(int index)
		{
			return index switch
			{
				0 => ParameterInfo.PlayerNames, 
				1 => ParameterInfo.Create("Precision", "a positive integer"), 
				_ => ParameterInfo.None, 
			};
		}
	}

	public PosCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				Vector3 position = ((Component)Helper.GetPlayer()).transform.position;
				int num = 0;
				if (args.Length >= 2)
				{
					num = args.TryParameterInt(1, 0);
					if (num == 0)
					{
						position = Helper.FindPlayer(args[1]).m_position;
					}
				}
				if (args.Length >= 3)
				{
					num = args.TryParameterInt(2, 0);
				}
				Helper.AddMessage(args.Context, "Player position (X,Z,Y): (" + position.x.ToString($"F{num}") + ", " + position.z.ToString($"F{num}") + ", " + position.y.ToString($"F{num}") + ")");
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		Helper.Command("pos", "[name/precision] [precision] - Prints the position of a player. If name is not given, prints the current position.", (ConsoleEvent)obj);
		AutoComplete.Register("pos", (int index) => index switch
		{
			0 => ParameterInfo.PlayerNames, 
			1 => ParameterInfo.Create("Precision", "a positive integer"), 
			_ => ParameterInfo.None, 
		});
	}
}
