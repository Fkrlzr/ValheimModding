using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class DungeonCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<ZNetView, bool> _003C_003E9__1_1;

		public static ConsoleEvent _003C_003E9__1_0;

		internal void _003C_002Ector_003Eb__1_0(ConsoleEventArgs args)
		{
			_003C_003Ec__DisplayClass1_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass1_0
			{
				player = Helper.GetPlayer()
			};
			ZNetView val = (from view in ZNetScene.instance.m_instances.Values
				where Object.op_Implicit((Object)(object)((Component)view).GetComponent<DungeonGenerator>())
				orderby Vector3.Distance(((Component)CS_0024_003C_003E8__locals0.player).transform.position, ((Component)view).transform.position)
				select view).FirstOrDefault();
			if ((Object)(object)val == (Object)null)
			{
				Helper.AddError(args.Context, "No dungeon found.", false);
			}
			else if (args.Length < 2)
			{
				PrintRooms(val);
			}
		}

		internal bool _003C_002Ector_003Eb__1_1(ZNetView view)
		{
			return Object.op_Implicit((Object)(object)((Component)view).GetComponent<DungeonGenerator>());
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass1_0
	{
		public Player player;

		internal float _003C_002Ector_003Eb__2(ZNetView view)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return Vector3.Distance(((Component)player).transform.position, ((Component)view).transform.position);
		}
	}

	public const string Name = "dungeon";

	public DungeonCommand()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		new ObjectAutoComplete();
		object obj = _003C_003Ec._003C_003E9__1_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				Player player = Helper.GetPlayer();
				ZNetView val2 = (from view in ZNetScene.instance.m_instances.Values
					where Object.op_Implicit((Object)(object)((Component)view).GetComponent<DungeonGenerator>())
					orderby Vector3.Distance(((Component)player).transform.position, ((Component)view).transform.position)
					select view).FirstOrDefault();
				if ((Object)(object)val2 == (Object)null)
				{
					Helper.AddError(args.Context, "No dungeon found.", false);
				}
				else if (args.Length < 2)
				{
					PrintRooms(val2);
				}
			};
			_003C_003Ec._003C_003E9__1_0 = val;
			obj = (object)val;
		}
		Helper.Command("dungeon", "Modifies the nearest dungeon.", (ConsoleEvent)obj);
	}

	private static RoomData FindRoom(string type)
	{
		throw new NotImplementedException();
	}

	private static string EditRoom(ZNetView obj, int i, string type, Vector3 pos, Quaternion rot)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		string text = "room" + i;
		RoomData val = FindRoom(type);
		ZDO zDO = obj.GetZDO();
		zDO.Set(text, ((object)val).GetHashCode());
		zDO.Set(text, pos);
		zDO.Set(text, rot);
		return "";
	}

	private static string PrintRooms(ZNetView obj)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		List<string> list = new List<string>();
		ZDO zDO = obj.GetZDO();
		DungeonGenerator component = ((Component)obj).GetComponent<DungeonGenerator>();
		list.Add($"Dungeon {obj.GetPrefabName()} {component.m_generatedSeed} {Helper.PrintVectorXZY(((Component)obj).transform.position)} {Helper.PrintAngleYXZ(((Component)obj).transform.rotation)}");
		int @int = zDO.GetInt("rooms", 0);
		for (int i = 0; i < @int; i++)
		{
			string text = "room" + i;
			int int2 = zDO.GetInt(text, 0);
			Vector3 vec = zDO.GetVec3(text + "_pos", Vector3.zero);
			Quaternion quaternion = zDO.GetQuaternion(text + "_rot", Quaternion.identity);
			list.Add($"Room {i}: {((Object)DungeonDB.instance.GetRoom(int2).RoomInPrefab).name} {Helper.PrintVectorXZY(vec)} {Helper.PrintAngleYXZ(quaternion)}");
		}
		return string.Join(", ", list);
	}
}
