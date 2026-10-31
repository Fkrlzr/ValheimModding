using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Data;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class SpawnObjectCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__3_0;

		internal void _003C_002Ector_003Eb__3_0(ConsoleEventArgs args)
		{
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			Helper.ArgsCheck(args, 2, "Missing object id.");
			string text = args[1];
			GameObject prefab = Helper.GetPrefab(text);
			if ((Object)(object)prefab == (Object)null)
			{
				throw new InvalidOperationException("Unable to find the object.");
			}
			SpawnObjectParameters spawnObjectParameters = new SpawnObjectParameters(args);
			ItemDrop component = prefab.GetComponent<ItemDrop>();
			int num = ((spawnObjectParameters.Amount == null) ? 1 : Helper.RandomValue(spawnObjectParameters.Amount));
			int num2 = num;
			if (Object.op_Implicit((Object)(object)component))
			{
				num2 = (int)Math.Ceiling((double)num2 / (double)component.m_itemData.m_shared.m_maxStackSize);
			}
			UndoHelper.BeginAction();
			List<GameObject> list = SpawnObject(spawnObjectParameters, prefab, num2);
			Manipulate(list, spawnObjectParameters, num);
			UndoHelper.EndAction();
			Player localPlayer = Player.m_localPlayer;
			if (localPlayer != null)
			{
				((Character)localPlayer).Message((MessageType)1, "Spawning object " + text, list.Count, (Sprite)null, false);
			}
			args.Context.AddString("Spawned: " + text + " at " + Helper.PrintVectorXZY(spawnObjectParameters.GetPosition()));
		}
	}

	public const string Name = "spawn_object";

	private static List<GameObject> SpawnObject(SpawnObjectParameters pars, GameObject prefab, int count)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		List<GameObject> list = new List<GameObject>();
		float num = (float)(count - 1) * 0.5f;
		float num3 = default(float);
		for (int i = 0; i < count; i++)
		{
			Vector3 position;
			if (pars.To.HasValue)
			{
				position = pars.GetPosition(i, count);
			}
			else
			{
				position = pars.GetPosition();
				Vector2 val = Random.insideUnitCircle * (pars.Radius?.Max ?? num);
				if (pars.Radius != null && pars.Radius.Min != pars.Radius.Max)
				{
					float num2 = Random.Range(0f, (float)Math.PI * 2f);
					Vector2 val2 = new Vector2(Mathf.Sin(num2), Mathf.Cos(num2));
					val = ((Vector2)(ref val2)).normalized * Helper.RandomValue(pars.Radius);
				}
				position.x += val.x;
				position.z += val.y;
			}
			if (pars.Snap)
			{
				ZoneSystem.instance.FindFloor(position, ref num3);
				if (num3 == 0f)
				{
					Vector3 val3 = position;
					val3.y += 100f;
					ZoneSystem.instance.FindFloor(position, ref num3);
				}
				position.y = num3;
			}
			Quaternion val4 = pars.BaseRotation * Quaternion.Euler(Helper.RandomValue(pars.Rotation));
			Vector3? scale = ((pars.Scale == null) ? null : new Vector3?(Helper.RandomValue(pars.Scale)));
			DataHelper.Init(prefab, position, val4, scale, pars.Data, pars.DataParameters);
			try
			{
				GameObject val5 = Object.Instantiate<GameObject>(prefab, position, val4);
				list.Add(val5);
				if (!ZNet.instance.IsServer())
				{
					ZDOMan.instance.ClientChanged(val5.GetComponent<ZNetView>().GetZDO().m_uid);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)ex);
			}
			DataHelper.CleanUp();
		}
		return list;
	}

	private static void Manipulate(IEnumerable<GameObject> spawned, SpawnObjectParameters pars, int total)
	{
		foreach (GameObject item in spawned)
		{
			ZNetView component = item.GetComponent<ZNetView>();
			if (pars.Baby.GetValueOrDefault())
			{
				Actions.SetBaby(item);
			}
			if (pars.Level != null)
			{
				Actions.SetLevel(item, Helper.RandomValue(pars.Level));
			}
			if (pars.Damage != null)
			{
				Actions.SetFloat(component, Helper.RandomValue(pars.Damage), Hash.Damage);
			}
			if (pars.Ammo != null)
			{
				Actions.SetInt(component, Helper.RandomValue(pars.Ammo), Hash.Ammo);
			}
			if (pars.AmmoType != null)
			{
				Actions.SetString(component, pars.AmmoType, Hash.AmmoType);
			}
			if (pars.Health != null)
			{
				Actions.SetHealth(item, Helper.RandomValue(pars.Health), pars.isHealthPercentage);
			}
			if (pars.Variant != null)
			{
				Actions.SetVariant(item, Helper.RandomValue(pars.Variant));
			}
			if (pars.Name != null)
			{
				Actions.SetName(item, pars.Name);
			}
			if (pars.CrafterId.HasValue)
			{
				Actions.SetCrafterId(item.GetComponent<ItemDrop>(), pars.CrafterId.Value);
			}
			if (pars.Hunt.HasValue)
			{
				Actions.SetHunt(item, pars.Hunt.Value);
			}
			if (pars.Tamed.HasValue)
			{
				Actions.SetTame(item, pars.Tamed.Value);
			}
			if (pars.Amount != null)
			{
				total -= Actions.SetStack(item, total);
			}
			Actions.SetVisual(item, (VisSlot)6, pars.Helmet);
			Actions.SetVisual(item, (VisSlot)4, pars.Chest);
			Actions.SetVisual(item, (VisSlot)7, pars.Shoulders);
			Actions.SetVisual(item, (VisSlot)5, pars.Legs);
			Actions.SetVisual(item, (VisSlot)8, pars.Utility);
			Actions.SetVisual(item, (VisSlot)0, pars.LeftHand);
			Actions.SetVisual(item, (VisSlot)1, pars.RightHand);
			if (pars.Model != null)
			{
				Actions.SetModel(item, Helper.RandomValue(pars.Model));
			}
			if (pars.Helmet != null || pars.Chest != null || pars.Shoulders != null || pars.Legs != null || pars.Utility != null || pars.LeftHand != null || pars.RightHand != null)
			{
				ZNetView component2 = item.GetComponent<ZNetView>();
				ZDO obj = ((component2 != null) ? component2.GetZDO() : null);
				if (obj != null)
				{
					obj.SetOwner(0L);
				}
			}
			if (pars.Distant.HasValue)
			{
				Actions.SetDistant(item, pars.Distant.Value);
			}
			if (pars.Persist.HasValue)
			{
				Actions.SetPersistent(item, pars.Persist.Value);
			}
			if (pars.Fields.Count > 0)
			{
				Actions.SetFields(item, pars.Fields);
			}
		}
	}

	public SpawnObjectCommand()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		new SpawnObjectAutoComplete();
		object obj = _003C_003Ec._003C_003E9__3_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
				Helper.ArgsCheck(args, 2, "Missing object id.");
				string text = args[1];
				GameObject prefab = Helper.GetPrefab(text);
				if ((Object)(object)prefab == (Object)null)
				{
					throw new InvalidOperationException("Unable to find the object.");
				}
				SpawnObjectParameters spawnObjectParameters = new SpawnObjectParameters(args);
				ItemDrop component = prefab.GetComponent<ItemDrop>();
				int num = ((spawnObjectParameters.Amount == null) ? 1 : Helper.RandomValue(spawnObjectParameters.Amount));
				int num2 = num;
				if (Object.op_Implicit((Object)(object)component))
				{
					num2 = (int)Math.Ceiling((double)num2 / (double)component.m_itemData.m_shared.m_maxStackSize);
				}
				UndoHelper.BeginAction();
				List<GameObject> list = SpawnObject(spawnObjectParameters, prefab, num2);
				Manipulate(list, spawnObjectParameters, num);
				UndoHelper.EndAction();
				Player localPlayer = Player.m_localPlayer;
				if (localPlayer != null)
				{
					((Character)localPlayer).Message((MessageType)1, "Spawning object " + text, list.Count, (Sprite)null, false);
				}
				args.Context.AddString("Spawned: " + text + " at " + Helper.PrintVectorXZY(spawnObjectParameters.GetPosition()));
			};
			_003C_003Ec._003C_003E9__3_0 = val;
			obj = (object)val;
		}
		Helper.Command("spawn_object", "Spawns objects", (ConsoleEvent)obj);
	}
}
