using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class ResetDungeonCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<ZDO, bool> _003C_003E9__2_2;

		public static ConsoleEvent _003C_003E9__2_0;

		internal void _003C_002Ector_003Eb__2_0(ConsoleEventArgs args)
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Invalid comparison between Unknown and I4
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			_003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass2_0();
			DungeonGenerator[] source = Object.FindObjectsByType<DungeonGenerator>((FindObjectsSortMode)0);
			CS_0024_003C_003E8__locals0.player = Helper.GetPlayer();
			DungeonGenerator val = source.OrderBy((DungeonGenerator dg) => Utils.DistanceXZ(((Component)dg).transform.position, ((Component)CS_0024_003C_003E8__locals0.player).transform.position)).FirstOrDefault();
			if (!Object.op_Implicit((Object)(object)val))
			{
				throw new Exception("No dungeon found.");
			}
			if (Utils.DistanceXZ(((Component)val).transform.position, ((Component)CS_0024_003C_003E8__locals0.player).transform.position) > 20f)
			{
				throw new Exception("The dungeon is too far.");
			}
			int sector = (int)ZoneSystem.SectorToIndex(ZoneSystem.GetZone(((Component)val).transform.position)).Sector;
			if (sector < 0 || sector >= ZDOMan.instance.m_objectsBySector.Length)
			{
				throw new Exception("No objects found.");
			}
			List<ZDO> obj = ZDOMan.instance.m_objectsBySector[sector] ?? throw new Exception("No objects found.");
			ZDO val2 = obj.FirstOrDefault((ZDO x) => x.GetPrefab() == ProxyHash);
			if (val2 != null)
			{
				int @int = val2.GetInt(ZDOVars.s_location, 0);
				if (ZoneSystem.instance.m_locationsByHash.TryGetValue(@int, out var value))
				{
					value.m_prefab.Load();
					Location component = value.m_prefab.Asset.GetComponent<Location>();
					if (Object.op_Implicit((Object)(object)component) && Object.op_Implicit((Object)(object)component.m_generator) && component.m_useCustomInteriorTransform && Object.op_Implicit((Object)(object)component.m_interiorTransform))
					{
						val.m_originalPosition = ((Component)component.m_generator).transform.localPosition;
					}
					value.m_prefab.Release();
				}
			}
			foreach (ZDO item in obj)
			{
				if (((int)val.m_algorithm != 0 || !(item.GetPosition().y < 3000f)) && ((int)val.m_algorithm != 1 || !(Utils.DistanceXZ(item.GetPosition(), ((Component)val).transform.position) > val.m_campRadiusMax)) && item.GetPrefab() != PlayerHash && item.GetPrefab() != ProxyHash && item != val.m_nview.GetZDO())
				{
					ZDOMan.instance.DestroyZDO(item);
				}
			}
			val.m_nview.ClaimOwnership();
			val.Generate((SpawnMode)0);
			Helper.AddMessage(args.Context, $"Reset dungeon {((Object)val).name} at {Helper.PrintVectorXZY(((Component)val).transform.position)} with m_originalPosition {val.m_originalPosition.y}.");
		}

		internal bool _003C_002Ector_003Eb__2_2(ZDO x)
		{
			return x.GetPrefab() == ProxyHash;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public Player player;

		internal float _003C_002Ector_003Eb__1(DungeonGenerator dg)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return Utils.DistanceXZ(((Component)dg).transform.position, ((Component)player).transform.position);
		}
	}

	private static readonly int ProxyHash = StringExtensionMethods.GetStableHashCode("LocationProxy");

	private static readonly int PlayerHash = StringExtensionMethods.GetStableHashCode("Player");

	public ResetDungeonCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__2_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_011c: Unknown result type (might be due to invalid IL or missing references)
				//IL_019f: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c0: Invalid comparison between Unknown and I4
				//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_025d: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
				//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
				//IL_0173: Unknown result type (might be due to invalid IL or missing references)
				//IL_0178: Unknown result type (might be due to invalid IL or missing references)
				DungeonGenerator[] source = Object.FindObjectsByType<DungeonGenerator>((FindObjectsSortMode)0);
				Player player = Helper.GetPlayer();
				DungeonGenerator val2 = source.OrderBy((DungeonGenerator dg) => Utils.DistanceXZ(((Component)dg).transform.position, ((Component)player).transform.position)).FirstOrDefault();
				if (!Object.op_Implicit((Object)(object)val2))
				{
					throw new Exception("No dungeon found.");
				}
				if (Utils.DistanceXZ(((Component)val2).transform.position, ((Component)player).transform.position) > 20f)
				{
					throw new Exception("The dungeon is too far.");
				}
				int sector = (int)ZoneSystem.SectorToIndex(ZoneSystem.GetZone(((Component)val2).transform.position)).Sector;
				if (sector < 0 || sector >= ZDOMan.instance.m_objectsBySector.Length)
				{
					throw new Exception("No objects found.");
				}
				List<ZDO> obj2 = ZDOMan.instance.m_objectsBySector[sector] ?? throw new Exception("No objects found.");
				ZDO val3 = obj2.FirstOrDefault((ZDO x) => x.GetPrefab() == ProxyHash);
				if (val3 != null)
				{
					int @int = val3.GetInt(ZDOVars.s_location, 0);
					if (ZoneSystem.instance.m_locationsByHash.TryGetValue(@int, out var value))
					{
						value.m_prefab.Load();
						Location component = value.m_prefab.Asset.GetComponent<Location>();
						if (Object.op_Implicit((Object)(object)component) && Object.op_Implicit((Object)(object)component.m_generator) && component.m_useCustomInteriorTransform && Object.op_Implicit((Object)(object)component.m_interiorTransform))
						{
							val2.m_originalPosition = ((Component)component.m_generator).transform.localPosition;
						}
						value.m_prefab.Release();
					}
				}
				foreach (ZDO item in obj2)
				{
					if (((int)val2.m_algorithm != 0 || !(item.GetPosition().y < 3000f)) && ((int)val2.m_algorithm != 1 || !(Utils.DistanceXZ(item.GetPosition(), ((Component)val2).transform.position) > val2.m_campRadiusMax)) && item.GetPrefab() != PlayerHash && item.GetPrefab() != ProxyHash && item != val2.m_nview.GetZDO())
					{
						ZDOMan.instance.DestroyZDO(item);
					}
				}
				val2.m_nview.ClaimOwnership();
				val2.Generate((SpawnMode)0);
				Helper.AddMessage(args.Context, $"Reset dungeon {((Object)val2).name} at {Helper.PrintVectorXZY(((Component)val2).transform.position)} with m_originalPosition {val2.m_originalPosition.y}.");
			};
			_003C_003Ec._003C_003E9__2_0 = val;
			obj = (object)val;
		}
		Helper.Command("resetdungeon", "- Resets the nearest camp or dungeon within 20 meters.", (ConsoleEvent)obj);
		AutoComplete.RegisterEmpty("resetdungeon");
	}
}
