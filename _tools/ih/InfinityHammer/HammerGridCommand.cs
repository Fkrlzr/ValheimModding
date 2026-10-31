using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class HammerGridCommand
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
				0 => ParameterInfo.Create("Grid precision in meters."), 
				1 => ParameterInfo.XZY("Grid center point. If not given, uses the current placement position.", subIndex), 
				_ => ParameterInfo.None, 
			};
		}

		internal void _003C_002Ector_003Eb__0_1(ConsoleEventArgs args)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			GameObject placementGhost = HammerHelper.GetPlacementGhost();
			float num = ((args.Length < 2) ? 0f : Parse.Float(args[1], 0f));
			Vector3 center = ((args.Length < 3) ? placementGhost.transform.position : Parse.VectorXZY(args[2]));
			Grid.Toggle(num, center);
			HammerHelper.Message(args.Context, Grid.Enabled ? $"Grid {num} enabled." : "Grid disabled.");
		}
	}

	public HammerGridCommand()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		AutoComplete.Register("hammer_grid", (Func<int, int, List<string>>)((int index, int subIndex) => index switch
		{
			0 => ParameterInfo.Create("Grid precision in meters."), 
			1 => ParameterInfo.XZY("Grid center point. If not given, uses the current placement position.", subIndex), 
			_ => ParameterInfo.None, 
		}));
		object obj = _003C_003Ec._003C_003E9__0_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				GameObject placementGhost = HammerHelper.GetPlacementGhost();
				float num = ((args.Length < 2) ? 0f : Parse.Float(args[1], 0f));
				Vector3 center = ((args.Length < 3) ? placementGhost.transform.position : Parse.VectorXZY(args[2]));
				Grid.Toggle(num, center);
				HammerHelper.Message(args.Context, Grid.Enabled ? $"Grid {num} enabled." : "Grid disabled.");
			};
			_003C_003Ec._003C_003E9__0_1 = val;
			obj = (object)val;
		}
		Helper.Command("hammer_grid", " [precision] [center=current] - Restricts possible placement coordinates. Using the same command removes the restriction.", (ConsoleEvent)obj);
	}
}
