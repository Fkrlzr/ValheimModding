using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Player), "Update")]
public class DisableDebugModeKeys
{
	private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		return new CodeMatcher(instructions, (ILGenerator)null).MatchStartForward((CodeMatch[])(object)new CodeMatch[1]
		{
			new CodeMatch((OpCode?)OpCodes.Ldsfld, (object)AccessTools.Field(typeof(Player), "m_debugMode"), (string)null)
		}).SetAndAdvance(OpCodes.Call, Transpilers.EmitDelegate<Func<bool>>((Func<bool>)(() => Player.m_debugMode && !Settings.DisableDebugModeKeys)).operand).InstructionEnumeration();
	}
}
