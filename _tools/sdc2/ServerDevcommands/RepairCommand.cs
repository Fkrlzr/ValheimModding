using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class RepairCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<int, List<string>> _003C_003E9__0_0;

		public static ConsoleEvent _003C_003E9__0_1;

		internal List<string> _003C_002Ector_003Eb__0_0(int index)
		{
			if (index == 0)
			{
				return ParameterInfo.Create("Radius", "a positive integer");
			}
			return ParameterInfo.None;
		}

		internal void _003C_002Ector_003Eb__0_1(ConsoleEventArgs args)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			Vector3 position = ((Component)Helper.GetPlayer()).transform.position;
			int num = 0;
			float num2 = args.TryParameterFloat(1, 20f);
			new List<Piece>();
			foreach (Piece s_allPiece in Piece.s_allPieces)
			{
				if (!(Vector3.Distance(position, ((Component)s_allPiece).transform.position) > num2))
				{
					WearNTear component = ((Component)s_allPiece).GetComponent<WearNTear>();
					if (component != null && component.Repair())
					{
						num++;
					}
				}
			}
			Helper.AddMessage(args.Context, $"Repaired {num} structures.");
		}
	}

	public RepairCommand()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		AutoComplete.Register("repair", (int index) => (index == 0) ? ParameterInfo.Create("Radius", "a positive integer") : ParameterInfo.None);
		object obj = _003C_003Ec._003C_003E9__0_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				Vector3 position = ((Component)Helper.GetPlayer()).transform.position;
				int num = 0;
				float num2 = args.TryParameterFloat(1, 20f);
				new List<Piece>();
				foreach (Piece s_allPiece in Piece.s_allPieces)
				{
					if (!(Vector3.Distance(position, ((Component)s_allPiece).transform.position) > num2))
					{
						WearNTear component = ((Component)s_allPiece).GetComponent<WearNTear>();
						if (component != null && component.Repair())
						{
							num++;
						}
					}
				}
				Helper.AddMessage(args.Context, $"Repaired {num} structures.");
			};
			_003C_003Ec._003C_003E9__0_1 = val;
			obj = (object)val;
		}
		Helper.Command("repair", "[radius=20] - Repair structures within given  meters.", (ConsoleEvent)obj);
	}
}
