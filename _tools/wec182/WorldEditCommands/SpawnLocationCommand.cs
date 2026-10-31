using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Data;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class SpawnLocationCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__1_0;

		internal void _003C_002Ector_003Eb__1_0(ConsoleEventArgs args)
		{
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			Helper.ArgsCheck(args, 2, "Missing location id.");
			ZoneSystem instance = ZoneSystem.instance;
			string text = args[1];
			ZoneLocation val = instance.GetLocation(ZDOKeys.Hash(text)) ?? throw new InvalidOperationException("Can't find location " + text + ".");
			if (!val.m_prefab.IsValid)
			{
				throw new InvalidOperationException("Can't find prefab for location " + text + ".");
			}
			int num = Random.Range(0, 99999);
			int forceSeed = int.MinValue;
			float num2 = (float)Random.Range(0, 16) * 22.5f;
			float num3 = 0f;
			Vector3 val2 = Vector3.zero;
			Vector3 val3 = Vector3.zero;
			Transform val4 = (Object.op_Implicit((Object)(object)Player.m_localPlayer) ? ((Component)Player.m_localPlayer).transform : null);
			if (Object.op_Implicit((Object)(object)val4))
			{
				val3 = val4.position;
				val2 = 2f * ((Component)val4).transform.forward;
				Quaternion rotation = ((Component)val4).transform.rotation;
				num3 = ((Quaternion)(ref rotation)).eulerAngles.y;
			}
			bool flag = true;
			string[] args2 = args.Args;
			for (int i = 0; i < args2.Length; i++)
			{
				string[] array = args2[i].Split('=');
				string text2 = array[0].ToLower();
				if (array.Length >= 2)
				{
					if (text2 == "seed")
					{
						num = Parse.Int(array[1], 0);
					}
					if (text2 == "dungeonseed")
					{
						forceSeed = Parse.Int(array[1], 0);
					}
					if (text2 == "rot" || text2 == "rotation")
					{
						num2 = Parse.Float(array[1], 0f);
					}
					if (text2 == "pos" || text2 == "position")
					{
						val2 = Parse.VectorXZY(array[1].Split(','));
						flag = array[1].Split(',').Length < 3;
					}
					if (text2 == "refrot" || text2 == "refrotation")
					{
						num3 = Parse.Float(array[1], num3);
					}
					if (text2 == "from" || text2 == "refpos")
					{
						val3 = Parse.VectorXZY(array[1].Split(','), val3);
					}
				}
			}
			Quaternion val5 = Quaternion.Euler(0f, num3, 0f);
			Vector3 val6 = val3;
			val6 += val5 * Vector3.forward * val2.x;
			val6 += val5 * Vector3.right * val2.z;
			val6 += val5 * Vector3.up * val2.y;
			Quaternion val7 = val5 * Quaternion.Euler(0f, num2, 0f);
			float y = default(float);
			if (flag && ZoneSystem.instance.FindFloor(val6, ref y))
			{
				val6.y = y;
			}
			DungeonGenerator.m_forceSeed = forceSeed;
			UndoHelper.BeginAction();
			ZoneSystem.instance.SpawnLocation(val, num, val6, val7, (SpawnMode)0, new List<GameObject>(), false);
			UndoHelper.EndAction();
			args.Context.AddString("Spawned: " + text + " at " + Helper.PrintVectorXZY(val6));
		}
	}

	public const string Name = "spawn_location";

	public SpawnLocationCommand()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		new SpawnLocationAutoComplete();
		object obj = _003C_003Ec._003C_003E9__1_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0091: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Unknown result type (might be due to invalid IL or missing references)
				//IL_0098: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
				//IL_0269: Unknown result type (might be due to invalid IL or missing references)
				//IL_026e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0270: Unknown result type (might be due to invalid IL or missing references)
				//IL_0272: Unknown result type (might be due to invalid IL or missing references)
				//IL_0274: Unknown result type (might be due to invalid IL or missing references)
				//IL_0276: Unknown result type (might be due to invalid IL or missing references)
				//IL_0278: Unknown result type (might be due to invalid IL or missing references)
				//IL_027d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0282: Unknown result type (might be due to invalid IL or missing references)
				//IL_0289: Unknown result type (might be due to invalid IL or missing references)
				//IL_028e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0293: Unknown result type (might be due to invalid IL or missing references)
				//IL_0295: Unknown result type (might be due to invalid IL or missing references)
				//IL_0297: Unknown result type (might be due to invalid IL or missing references)
				//IL_0299: Unknown result type (might be due to invalid IL or missing references)
				//IL_029e: Unknown result type (might be due to invalid IL or missing references)
				//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_02af: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
				//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
				//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
				//IL_0320: Unknown result type (might be due to invalid IL or missing references)
				//IL_0322: Unknown result type (might be due to invalid IL or missing references)
				//IL_0347: Unknown result type (might be due to invalid IL or missing references)
				//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0243: Unknown result type (might be due to invalid IL or missing references)
				//IL_0245: Unknown result type (might be due to invalid IL or missing references)
				//IL_024a: Unknown result type (might be due to invalid IL or missing references)
				Helper.ArgsCheck(args, 2, "Missing location id.");
				ZoneSystem instance = ZoneSystem.instance;
				string text = args[1];
				ZoneLocation val2 = instance.GetLocation(ZDOKeys.Hash(text)) ?? throw new InvalidOperationException("Can't find location " + text + ".");
				if (!val2.m_prefab.IsValid)
				{
					throw new InvalidOperationException("Can't find prefab for location " + text + ".");
				}
				int num = Random.Range(0, 99999);
				int forceSeed = int.MinValue;
				float num2 = (float)Random.Range(0, 16) * 22.5f;
				float num3 = 0f;
				Vector3 val3 = Vector3.zero;
				Vector3 val4 = Vector3.zero;
				Transform val5 = (Object.op_Implicit((Object)(object)Player.m_localPlayer) ? ((Component)Player.m_localPlayer).transform : null);
				if (Object.op_Implicit((Object)(object)val5))
				{
					val4 = val5.position;
					val3 = 2f * ((Component)val5).transform.forward;
					Quaternion rotation = ((Component)val5).transform.rotation;
					num3 = ((Quaternion)(ref rotation)).eulerAngles.y;
				}
				bool flag = true;
				string[] args2 = args.Args;
				for (int i = 0; i < args2.Length; i++)
				{
					string[] array = args2[i].Split('=');
					string text2 = array[0].ToLower();
					if (array.Length >= 2)
					{
						if (text2 == "seed")
						{
							num = Parse.Int(array[1], 0);
						}
						if (text2 == "dungeonseed")
						{
							forceSeed = Parse.Int(array[1], 0);
						}
						if (text2 == "rot" || text2 == "rotation")
						{
							num2 = Parse.Float(array[1], 0f);
						}
						if (text2 == "pos" || text2 == "position")
						{
							val3 = Parse.VectorXZY(array[1].Split(','));
							flag = array[1].Split(',').Length < 3;
						}
						if (text2 == "refrot" || text2 == "refrotation")
						{
							num3 = Parse.Float(array[1], num3);
						}
						if (text2 == "from" || text2 == "refpos")
						{
							val4 = Parse.VectorXZY(array[1].Split(','), val4);
						}
					}
				}
				Quaternion val6 = Quaternion.Euler(0f, num3, 0f);
				Vector3 val7 = val4;
				val7 += val6 * Vector3.forward * val3.x;
				val7 += val6 * Vector3.right * val3.z;
				val7 += val6 * Vector3.up * val3.y;
				Quaternion val8 = val6 * Quaternion.Euler(0f, num2, 0f);
				float y = default(float);
				if (flag && ZoneSystem.instance.FindFloor(val7, ref y))
				{
					val7.y = y;
				}
				DungeonGenerator.m_forceSeed = forceSeed;
				UndoHelper.BeginAction();
				ZoneSystem.instance.SpawnLocation(val2, num, val7, val8, (SpawnMode)0, new List<GameObject>(), false);
				UndoHelper.EndAction();
				args.Context.AddString("Spawned: " + text + " at " + Helper.PrintVectorXZY(val7));
			};
			_003C_003Ec._003C_003E9__1_0 = val;
			obj = (object)val;
		}
		Helper.Command("spawn_location", "Spawns locations", (ConsoleEvent)obj);
	}
}
