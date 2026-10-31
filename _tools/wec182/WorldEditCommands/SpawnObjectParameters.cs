using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

internal class SpawnObjectParameters : SharedObjectParameters
{
	public Quaternion BaseRotation = Quaternion.identity;

	public Range<Vector3> Rotation = new Range<Vector3>(Vector3.zero);

	public Range<Vector3> RelativePosition = new Range<Vector3>(Vector3.zero);

	public Vector3 From = Vector3.zero;

	public Vector3? To;

	public Range<int>? Amount;

	public string? Name;

	public long? CrafterId;

	public Range<int>? Variant;

	public bool Snap = true;

	public bool? Tamed;

	public bool? Hunt;

	private bool UseDefaultRelativePosition;

	public DataEntry? Data;

	public SpawnObjectParameters(ConsoleEventArgs args)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)Player.m_localPlayer))
		{
			From = ((Component)Player.m_localPlayer).transform.position;
			BaseRotation = ((Component)Player.m_localPlayer).transform.rotation;
			UseDefaultRelativePosition = true;
		}
		ParseArgs(args.Args);
	}

	protected override void ParseArgs(string[] args)
	{
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		base.ParseArgs(args);
		for (int i = 0; i < args.Length; i++)
		{
			string[] array = args[i].Split(new char[1] { '=' }, 2);
			string text = array[0].ToLower();
			if (text == "tame" || text == "tamed")
			{
				Tamed = true;
			}
			if (text == "hunt")
			{
				Hunt = true;
			}
			if (array.Length < 2)
			{
				continue;
			}
			string text2 = array[1];
			if (text == "name" || text == "crafter")
			{
				Name = text2.Replace("_", " ");
			}
			if (text == "crafterid")
			{
				CrafterId = Parse.Long(text2, 0L);
			}
			if (text == "variant")
			{
				Variant = Parse.IntRange(text2, 0);
			}
			if (text == "amount")
			{
				Amount = Parse.IntRange(text2, 0);
			}
			if (text == "hunt")
			{
				Hunt = Parse.Boolean(text2);
			}
			if (text == "tame" || text == "tamed")
			{
				Tamed = Parse.Boolean(text2);
			}
			if (text == "refrot" || text == "refrotation")
			{
				BaseRotation = Parse.AngleYXZ(text2, BaseRotation);
			}
			if (text == "pos" || text == "position")
			{
				UseDefaultRelativePosition = false;
				string[] array2 = text2.Split(',');
				RelativePosition = Parse.VectorZXYRange(text2, Vector3.zero);
				if (array2.Length >= 3)
				{
					Snap = false;
				}
			}
			if (text == "rot" || text == "rotation")
			{
				Rotation = Parse.VectorYXZRange(text2, Vector3.zero);
			}
			if (text == "from" || text == "refpos")
			{
				UseDefaultRelativePosition = false;
				string[] array3 = text2.Split(',');
				From = Parse.VectorXZY(array3, From);
				if (array3.Length >= 3)
				{
					Snap = false;
				}
			}
			if (text == "to")
			{
				UseDefaultRelativePosition = false;
				To = Parse.VectorXZY(text2.Split(','), From);
			}
			if (text == "data")
			{
				List<DataEntry> list = new List<DataEntry>();
				list.AddRange(Parse.Split(text2, ',').Select(DataHelper.Get));
				Data = DataHelper.Merge(list.ToArray());
			}
			if (text == "refplayer")
			{
				UseDefaultRelativePosition = false;
				PlayerInfo val = Helper.FindPlayer(text2, false);
				if (((ZDOID)(ref val.m_characterID)).IsNone())
				{
					throw new InvalidOperationException("Unable to find the player.");
				}
				if (!val.m_publicPosition)
				{
					throw new InvalidOperationException("Player doesn't have a public position.");
				}
				From = val.m_position;
			}
		}
		if (UseDefaultRelativePosition)
		{
			From += BaseRotation * Vector3.forward * 2f;
		}
		if (To.HasValue && Radius != null)
		{
			throw new InvalidOperationException("<color=yellow>radius</color> can't be used with <color=yellow>to</color>.");
		}
	}

	public Vector3 GetPosition()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		return From + BaseRotation * Helper.RandomValue(RelativePosition);
	}

	public Vector3 GetPosition(int index, int max)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		if (To.HasValue)
		{
			return From + (To.Value - From) * (float)index / (float)(max - 1);
		}
		return From;
	}
}
