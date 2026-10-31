using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class CalmCommand
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
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			Vector3 position = ((Component)Helper.GetPlayer()).transform.position;
			float num = args.TryParameterFloat(1, 20f);
			int num2 = 0;
			foreach (BaseAI instance in BaseAI.Instances)
			{
				BaseAI val = instance;
				if (Vector3.Distance(position, ((Component)val).transform.position) <= num && (val.IsAlerted() || val.IsAggravated() || val.HaveTarget()))
				{
					val.m_nview.ClaimOwnership();
					val.SetAggravated(false, (AggravatedReason)1);
					val.SetAlerted(false);
					val.SetTargetInfo(ZDOID.None);
					MonsterAI val2 = (MonsterAI)(object)((val is MonsterAI) ? val : null);
					if (val2 != null)
					{
						val2.m_targetCreature = null;
						val2.m_targetStatic = null;
					}
					AnimalAI val3 = (AnimalAI)(object)((val is AnimalAI) ? val : null);
					if (val3 != null)
					{
						val3.m_target = null;
					}
					num2++;
				}
			}
			Helper.AddMessage(args.Context, $"Calmed {num2} creatures.");
		}
	}

	public CalmCommand()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		AutoComplete.Register("calm", (int index) => (index == 0) ? ParameterInfo.Create("Radius", "a positive integer") : ParameterInfo.None);
		object obj = _003C_003Ec._003C_003E9__0_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Expected O, but got Unknown
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				Vector3 position = ((Component)Helper.GetPlayer()).transform.position;
				float num = args.TryParameterFloat(1, 20f);
				int num2 = 0;
				foreach (BaseAI instance in BaseAI.Instances)
				{
					BaseAI val2 = instance;
					if (Vector3.Distance(position, ((Component)val2).transform.position) <= num && (val2.IsAlerted() || val2.IsAggravated() || val2.HaveTarget()))
					{
						val2.m_nview.ClaimOwnership();
						val2.SetAggravated(false, (AggravatedReason)1);
						val2.SetAlerted(false);
						val2.SetTargetInfo(ZDOID.None);
						MonsterAI val3 = (MonsterAI)(object)((val2 is MonsterAI) ? val2 : null);
						if (val3 != null)
						{
							val3.m_targetCreature = null;
							val3.m_targetStatic = null;
						}
						AnimalAI val4 = (AnimalAI)(object)((val2 is AnimalAI) ? val2 : null);
						if (val4 != null)
						{
							val4.m_target = null;
						}
						num2++;
					}
				}
				Helper.AddMessage(args.Context, $"Calmed {num2} creatures.");
			};
			_003C_003Ec._003C_003E9__0_1 = val;
			obj = (object)val;
		}
		Helper.Command("calm", "[radius=20] - Calms creatures within given meters.", (ConsoleEvent)obj);
	}
}
