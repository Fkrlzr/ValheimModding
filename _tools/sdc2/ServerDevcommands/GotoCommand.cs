using System;
using System.Linq;
using UnityEngine;

namespace ServerDevcommands;

public class GotoCommand
{
	private Vector3? LastPosition;

	private Quaternion? LastRotation;

	private void ParseArgs(ConsoleEventArgs args, Player player, ref Vector3 pos, ref Quaternion rot)
	{
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		if (args.Length < 2)
		{
			pos.y = WorldGenerator.instance.GetHeight(pos.x, pos.z);
			return;
		}
		if (args.Length == 2 && args[1] == "last")
		{
			if (!LastPosition.HasValue)
			{
				throw new InvalidOperationException("No last position");
			}
			pos = (Vector3)(((_003F?)LastPosition) ?? pos);
			rot = (Quaternion)(((_003F?)LastRotation) ?? rot);
			return;
		}
		string[] array = Parse.Split(args[1]);
		if (args.Length > 2)
		{
			array = args.Args.Skip(1).ToArray();
		}
		if (Parse.TryFloat(array[0], out var value))
		{
			if (array.Length < 2)
			{
				pos.y = value;
				return;
			}
			Vector3 val = Parse.VectorXZY(array);
			float num = (((Character)player).IsDebugFlying() ? ((Component)player).transform.position.y : WorldGenerator.instance.GetHeight(val.x, val.z));
			pos = Parse.VectorXZY(array, new Vector3(pos.x, num, pos.z));
		}
		else
		{
			PlayerInfo val2 = Helper.FindPlayer(string.Join(" ", args.Args.Skip(1)));
			pos = val2.m_position;
		}
	}

	public GotoCommand()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		AutoComplete.Register("goto", delegate(int index, int subIndex)
		{
			if (index == 0 && subIndex == 0)
			{
				return ParameterInfo.PublicPlayerNames;
			}
			return (index == 0) ? ParameterInfo.XZY("Coordinates", subIndex) : ParameterInfo.XZY("Coordinates", index);
		});
		Helper.Command("goto", "[x,z,y or altitude or last or player or no parameter] - Teleports to the coordinates. If y is not given, teleports to the ground level.", (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			Player player = Helper.GetPlayer();
			Vector3 pos = ((Component)player).transform.position;
			Quaternion rot = ((Component)player).transform.rotation;
			ParseArgs(args, player, ref pos, ref rot);
			LastPosition = ((Component)player).transform.position;
			LastRotation = ((Component)player).transform.rotation;
			ZRoutedRpc.instance.InvokeRoutedRPC(0L, ((Character)player).GetZDOID(), "RPC_TeleportTo", new object[3] { pos, rot, true });
			Helper.AddMessage(args.Context, $"Teleported to (X,Z,Y): {pos.x}, {pos.z}, {pos.y}.");
		});
	}
}
