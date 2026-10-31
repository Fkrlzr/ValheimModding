using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class PullCommand
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
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			Player player = Helper.GetPlayer();
			float num = args.TryParameterFloat(1, player.m_autoPickupRange);
			Collider[] array = Physics.OverlapSphere(((Component)player).transform.position + Vector3.up, num, player.m_autoPickupMask);
			foreach (Collider val in array)
			{
				if (Object.op_Implicit((Object)(object)val.attachedRigidbody))
				{
					ItemDrop component = ((Component)val.attachedRigidbody).GetComponent<ItemDrop>();
					if (Object.op_Implicit((Object)(object)component))
					{
						component.m_autoPickup = true;
						((Component)component).transform.position = ((Component)player).transform.position + Vector3.up * 0.5f;
					}
				}
			}
			Helper.AddMessage(args.Context, $"Pulled items within {num:F0} meters.");
		}

		internal List<string> _003C_002Ector_003Eb__0_1(int index)
		{
			if (index == 0)
			{
				return ParameterInfo.Create("Radius", "a positive integer");
			}
			return ParameterInfo.None;
		}
	}

	public PullCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				Player player = Helper.GetPlayer();
				float num = args.TryParameterFloat(1, player.m_autoPickupRange);
				Collider[] array = Physics.OverlapSphere(((Component)player).transform.position + Vector3.up, num, player.m_autoPickupMask);
				foreach (Collider val2 in array)
				{
					if (Object.op_Implicit((Object)(object)val2.attachedRigidbody))
					{
						ItemDrop component = ((Component)val2.attachedRigidbody).GetComponent<ItemDrop>();
						if (Object.op_Implicit((Object)(object)component))
						{
							component.m_autoPickup = true;
							((Component)component).transform.position = ((Component)player).transform.position + Vector3.up * 0.5f;
						}
					}
				}
				Helper.AddMessage(args.Context, $"Pulled items within {num:F0} meters.");
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		Helper.Command("pull", "[radius] - Pulls all items within the radius.", (ConsoleEvent)obj);
		AutoComplete.Register("pull", (int index) => (index == 0) ? ParameterInfo.Create("Radius", "a positive integer") : ParameterInfo.None);
	}
}
