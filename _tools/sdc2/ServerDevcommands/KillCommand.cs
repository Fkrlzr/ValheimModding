using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class KillCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__0_0;

		public static ConsoleEvent _003C_003E9__0_1;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Expected O, but got Unknown
			List<Character> allCharacters = Character.GetAllCharacters();
			int num = 0;
			int num2 = 0;
			foreach (Character item in allCharacters)
			{
				if (!item.IsPlayer())
				{
					item.Damage(new HitData(1E+10f));
					num++;
				}
			}
			if (Settings.KillDestroySpawners)
			{
				SpawnArea[] array = Object.FindObjectsByType<SpawnArea>((FindObjectsSortMode)0);
				for (int i = 0; i < array.Length; i++)
				{
					Destructible component = ((Component)array[i]).gameObject.GetComponent<Destructible>();
					if (Object.op_Implicit((Object)(object)component))
					{
						component.Damage(new HitData(1E+10f));
						num2++;
					}
				}
			}
			((Character)Player.m_localPlayer).Message((MessageType)1, string.Format("Killed {0} monsters{1}", num, (num2 > 0) ? $" & {num2} spawners." : "."), 0, (Sprite)null, false);
		}

		internal void _003C_002Ector_003Eb__0_1(ConsoleEventArgs args)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Expected O, but got Unknown
			List<Character> allCharacters = Character.GetAllCharacters();
			int num = 0;
			int num2 = 0;
			foreach (Character item in allCharacters)
			{
				if (!item.IsPlayer() && !item.IsTamed())
				{
					item.Damage(new HitData(1E+10f));
					num++;
				}
			}
			if (Settings.KillDestroySpawners)
			{
				SpawnArea[] array = Object.FindObjectsByType<SpawnArea>((FindObjectsSortMode)0);
				for (int i = 0; i < array.Length; i++)
				{
					Destructible component = ((Component)array[i]).gameObject.GetComponent<Destructible>();
					if (Object.op_Implicit((Object)(object)component))
					{
						component.Damage(new HitData(1E+10f));
						num2++;
					}
				}
			}
			((Character)Player.m_localPlayer).Message((MessageType)1, string.Format("Killed {0} monsters{1}", num, (num2 > 0) ? $" & {num2} spawners." : "."), 0, (Sprite)null, false);
		}
	}

	public KillCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate
			{
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Expected O, but got Unknown
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_008d: Expected O, but got Unknown
				List<Character> allCharacters2 = Character.GetAllCharacters();
				int num3 = 0;
				int num4 = 0;
				foreach (Character item in allCharacters2)
				{
					if (!item.IsPlayer())
					{
						item.Damage(new HitData(1E+10f));
						num3++;
					}
				}
				if (Settings.KillDestroySpawners)
				{
					SpawnArea[] array2 = Object.FindObjectsByType<SpawnArea>((FindObjectsSortMode)0);
					for (int j = 0; j < array2.Length; j++)
					{
						Destructible component2 = ((Component)array2[j]).gameObject.GetComponent<Destructible>();
						if (Object.op_Implicit((Object)(object)component2))
						{
							component2.Damage(new HitData(1E+10f));
							num4++;
						}
					}
				}
				((Character)Player.m_localPlayer).Message((MessageType)1, string.Format("Killed {0} monsters{1}", num3, (num4 > 0) ? $" & {num4} spawners." : "."), 0, (Sprite)null, false);
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		Helper.Command("killall", "kill nearby creatures", (ConsoleEvent)obj);
		AutoComplete.RegisterEmpty("killall");
		object obj2 = _003C_003Ec._003C_003E9__0_1;
		if (obj2 == null)
		{
			ConsoleEvent val2 = delegate
			{
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Expected O, but got Unknown
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0095: Expected O, but got Unknown
				List<Character> allCharacters = Character.GetAllCharacters();
				int num = 0;
				int num2 = 0;
				foreach (Character item2 in allCharacters)
				{
					if (!item2.IsPlayer() && !item2.IsTamed())
					{
						item2.Damage(new HitData(1E+10f));
						num++;
					}
				}
				if (Settings.KillDestroySpawners)
				{
					SpawnArea[] array = Object.FindObjectsByType<SpawnArea>((FindObjectsSortMode)0);
					for (int i = 0; i < array.Length; i++)
					{
						Destructible component = ((Component)array[i]).gameObject.GetComponent<Destructible>();
						if (Object.op_Implicit((Object)(object)component))
						{
							component.Damage(new HitData(1E+10f));
							num2++;
						}
					}
				}
				((Character)Player.m_localPlayer).Message((MessageType)1, string.Format("Killed {0} monsters{1}", num, (num2 > 0) ? $" & {num2} spawners." : "."), 0, (Sprite)null, false);
			};
			_003C_003Ec._003C_003E9__0_1 = val2;
			obj2 = (object)val2;
		}
		Helper.Command("killenemies", "kill nearby enemies", (ConsoleEvent)obj2);
		AutoComplete.RegisterEmpty("killenemies");
	}
}
