using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ServerDevcommands;

public class MoveSpawn
{
	public MoveSpawn()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		new ConsoleCommand("move_spawn", "[x,z,y] Moves the default spawn point to the given coordinates (player's current coordinates if not given).", (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			var (args2, val) = Helper.AddPlayerPosXZY(args.Args, 1);
			if (!Object.op_Implicit((Object)(object)Player.m_localPlayer))
			{
				Helper.ArgsCheck(args, 2, "Missing the coordinates.");
			}
			if (ZNet.instance.IsServer())
			{
				ZoneSystem instance = ZoneSystem.instance;
				if (Object.op_Implicit((Object)(object)instance))
				{
					instance.tempIconList.Clear();
					instance.GetLocationIcons(instance.tempIconList);
					Vector3[] array = (from icon in instance.tempIconList
						where icon.Value == "StartTemple"
						select icon.Key).ToArray();
					if (array.Length == 0)
					{
						Helper.AddMessage(args.Context, "Default spawn point not found. Creating a new one.");
						if (CreateSpawnLocation(val))
						{
							Helper.AddMessage(args.Context, "Spawn created at " + Helper.PrintVectorXZY(val));
							instance.SendLocationIcons(0L);
						}
						else
						{
							Helper.AddMessage(args.Context, "Failed to create spawn point. Default spawn point not found in location prefabs.");
						}
					}
					else
					{
						if (array.Length > 1)
						{
							Helper.AddMessage(args.Context, "Multiple spawn points found, moving the first one.");
						}
						if (MoveSpawnLocation(array[0], val))
						{
							Helper.AddMessage(args.Context, "Spawn moved to " + Helper.PrintVectorXZY(val));
							instance.SendLocationIcons(0L);
						}
						else
						{
							Helper.AddMessage(args.Context, "Failed to move spawn point. Default spawn point not found in location instances.");
						}
					}
				}
			}
			else
			{
				ServerExecution.Send((IEnumerable<string>)args2);
			}
		}, true, true, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.Register("move_spawn", (int index, int subIndex) => (index == 0) ? ParameterInfo.XZY("Coordinates (player's current coordinates if not given).", subIndex) : ParameterInfo.None);
	}

	private bool CreateSpawnLocation(Vector3 to)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		ZoneSystem instance = ZoneSystem.instance;
		Vector2s zone = ZoneSystem.GetZone(to);
		ZoneLocation val = instance.m_locations.FirstOrDefault((ZoneLocation l) => l.m_prefabName == "StartTemple");
		if (val == null)
		{
			return false;
		}
		LocationInstance val2 = default(LocationInstance);
		val2.m_position = to;
		val2.m_location = val;
		LocationInstance value = val2;
		instance.m_locationInstances[zone] = value;
		return true;
	}

	private bool MoveSpawnLocation(Vector3 from, Vector3 to)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		ZoneSystem instance = ZoneSystem.instance;
		Vector2s zone = ZoneSystem.GetZone(from);
		Vector2s zone2 = ZoneSystem.GetZone(to);
		if (!instance.m_locationInstances.TryGetValue(zone, out var value))
		{
			return false;
		}
		value.m_position = to;
		instance.m_locationInstances.Remove(zone);
		instance.m_locationInstances[zone2] = value;
		return true;
	}
}
