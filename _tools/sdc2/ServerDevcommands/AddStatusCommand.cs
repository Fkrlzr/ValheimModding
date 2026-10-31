using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class AddStatusCommand
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
			return index switch
			{
				0 => ParameterInfo.StatusEffects, 
				1 => ParameterInfo.Create("Effect duration in seconds."), 
				2 => ParameterInfo.Create("Effect intensity."), 
				_ => ParameterInfo.None, 
			};
		}

		internal void _003C_002Ector_003Eb__0_1(ConsoleEventArgs args)
		{
			Helper.ArgsCheck(args, 2, "Missing status name");
			Player player = Helper.GetPlayer();
			int stableHashCode = StringExtensionMethods.GetStableHashCode(args[1]);
			((Character)player).GetSEMan().AddStatusEffect(stableHashCode, true, 0, 0f, (short)(-1));
			StatusEffect statusEffect = ((Character)player).GetSEMan().GetStatusEffect(stableHashCode);
			if ((Object)(object)statusEffect == (Object)null)
			{
				return;
			}
			float ttl = default(float);
			if (args.TryParameterFloat(2, ref ttl))
			{
				statusEffect.m_ttl = ttl;
			}
			float num = default(float);
			if (!args.TryParameterFloat(3, ref num))
			{
				return;
			}
			SE_Shield val = (SE_Shield)(object)((statusEffect is SE_Shield) ? statusEffect : null);
			if (val != null)
			{
				val.m_absorbDamage = num;
			}
			SE_Burning val2 = (SE_Burning)(object)((statusEffect is SE_Burning) ? statusEffect : null);
			if (val2 != null)
			{
				if (args[1] == "Burning")
				{
					val2.m_fireDamageLeft = 0f;
					val2.AddFireDamage(num);
				}
				else
				{
					val2.m_spiritDamageLeft = 0f;
					val2.AddSpiritDamage(num);
				}
			}
			SE_Poison val3 = (SE_Poison)(object)((statusEffect is SE_Poison) ? statusEffect : null);
			if (val3 != null)
			{
				val3.m_damageLeft = num;
				val3.m_damagePerHit = num / statusEffect.m_ttl * val3.m_damageInterval;
			}
		}
	}

	public AddStatusCommand()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		AutoComplete.Register("addstatus", (int index) => index switch
		{
			0 => ParameterInfo.StatusEffects, 
			1 => ParameterInfo.Create("Effect duration in seconds."), 
			2 => ParameterInfo.Create("Effect intensity."), 
			_ => ParameterInfo.None, 
		});
		object obj = _003C_003Ec._003C_003E9__0_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				Helper.ArgsCheck(args, 2, "Missing status name");
				Player player = Helper.GetPlayer();
				int stableHashCode = StringExtensionMethods.GetStableHashCode(args[1]);
				((Character)player).GetSEMan().AddStatusEffect(stableHashCode, true, 0, 0f, (short)(-1));
				StatusEffect statusEffect = ((Character)player).GetSEMan().GetStatusEffect(stableHashCode);
				if (!((Object)(object)statusEffect == (Object)null))
				{
					float ttl = default(float);
					if (args.TryParameterFloat(2, ref ttl))
					{
						statusEffect.m_ttl = ttl;
					}
					float num = default(float);
					if (args.TryParameterFloat(3, ref num))
					{
						SE_Shield val2 = (SE_Shield)(object)((statusEffect is SE_Shield) ? statusEffect : null);
						if (val2 != null)
						{
							val2.m_absorbDamage = num;
						}
						SE_Burning val3 = (SE_Burning)(object)((statusEffect is SE_Burning) ? statusEffect : null);
						if (val3 != null)
						{
							if (args[1] == "Burning")
							{
								val3.m_fireDamageLeft = 0f;
								val3.AddFireDamage(num);
							}
							else
							{
								val3.m_spiritDamageLeft = 0f;
								val3.AddSpiritDamage(num);
							}
						}
						SE_Poison val4 = (SE_Poison)(object)((statusEffect is SE_Poison) ? statusEffect : null);
						if (val4 != null)
						{
							val4.m_damageLeft = num;
							val4.m_damagePerHit = num / statusEffect.m_ttl * val4.m_damageInterval;
						}
					}
				}
			};
			_003C_003Ec._003C_003E9__0_1 = val;
			obj = (object)val;
		}
		Helper.Command("addstatus", "[name] [duration] [intensity] - Adds a status effect.", (ConsoleEvent)obj);
	}
}
