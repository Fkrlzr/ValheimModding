using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class HammerPosCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<int, int, List<string>> _003C_003E9__0_0;

		public static ConsoleEvent _003C_003E9__0_1;

		internal List<string> _003C_002Ector_003Eb__0_0(int index, int subIndex)
		{
			return index switch
			{
				1 => ParameterInfo.YXZ("Rotation", subIndex), 
				0 => ParameterInfo.XZY("Coordinates", subIndex), 
				_ => ParameterInfo.None, 
			};
		}

		internal void _003C_002Ector_003Eb__0_1(ConsoleEventArgs args)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			Hammer.Equip();
			Vector3 value = Vector3.zero;
			if (args.Length > 1)
			{
				value = Parse.VectorXZY(args[1]);
			}
			Position.Override = value;
			if (args.Length > 2)
			{
				value = Parse.VectorYXZ(args[2]);
				PlaceRotation.Set(Quaternion.Euler(value));
			}
		}
	}

	public HammerPosCommand()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		AutoComplete.Register("hammer_pos", (Func<int, int, List<string>>)((int index, int subIndex) => index switch
		{
			1 => ParameterInfo.YXZ("Rotation", subIndex), 
			0 => ParameterInfo.XZY("Coordinates", subIndex), 
			_ => ParameterInfo.None, 
		}));
		object obj = _003C_003Ec._003C_003E9__0_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				Hammer.Equip();
				Vector3 value = Vector3.zero;
				if (args.Length > 1)
				{
					value = Parse.VectorXZY(args[1]);
				}
				Position.Override = value;
				if (args.Length > 2)
				{
					value = Parse.VectorYXZ(args[2]);
					PlaceRotation.Set(Quaternion.Euler(value));
				}
			};
			_003C_003Ec._003C_003E9__0_1 = val;
			obj = (object)val;
		}
		Helper.Command("hammer_pos", "[posX,posZ,posY=0,0,0] [rotY,rotX,rotZ=0,0,0] - Sets the hammer position.", (ConsoleEvent)obj);
	}
}
