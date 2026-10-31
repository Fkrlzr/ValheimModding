using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(ZNet), "WritePlayerInfo")]
public class AddExtraPlayer
{
	private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		return new CodeMatcher(instructions, (ILGenerator)null).MatchStartForward((CodeMatch[])(object)new CodeMatch[1]
		{
			new CodeMatch((OpCode?)OpCodes.Callvirt, (object)AccessTools.Method(typeof(ZPackage), "Write", new Type[1] { typeof(int) }, (Type[])null), (string)null)
		}).Advance(1).InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
		{
			new CodeInstruction(OpCodes.Ldarg_0, (object)null)
		})
			.InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
			{
				new CodeInstruction(OpCodes.Ldloc_0, (object)null)
			})
			.InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
			{
				new CodeInstruction(OpCodes.Call, (object)AccessTools.Method(typeof(AddExtraPlayer), "AddServer", (Type[])null, (Type[])null))
			})
			.InstructionEnumeration();
	}

	private static void AddServer(ZNet net, ZPackage pkg)
	{
		if (Settings.IsServerChat)
		{
			int pos = pkg.GetPos();
			pkg.SetPos(0);
			if (IsExtraPlayerAdded(net, pkg.ReadInt()))
			{
				pkg.SetPos(pos);
				return;
			}
			pkg.SetPos(0);
			pkg.Write(net.m_players.Count + 1);
			ServerChat.Write(pkg);
		}
	}

	private static bool IsExtraPlayerAdded(ZNet net, int count)
	{
		return count >= net.m_players.Count + 1;
	}
}
