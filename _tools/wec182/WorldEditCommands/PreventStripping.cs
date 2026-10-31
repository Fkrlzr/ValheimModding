using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace WorldEditCommands;

[HarmonyPatch(typeof(ZDO))]
public class PreventStripping
{
	private static readonly HashSet<int> FieldZdoKeys = new HashSet<int>();

	public static void AddKey(Type type, FieldInfo fieldInfo)
	{
		if (fieldInfo.FieldType == typeof(string) || fieldInfo.FieldType == typeof(int) || fieldInfo.FieldType == typeof(bool))
		{
			string text = type.Name + "." + fieldInfo.Name;
			FieldZdoKeys.Add(StringExtensionMethods.GetStableHashCode(text));
		}
	}

	[HarmonyPatch("Strip", new Type[]
	{
		typeof(int),
		typeof(string)
	})]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> String(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
	{
		return Transpile(instructions, generator);
	}

	[HarmonyPatch("Strip", new Type[]
	{
		typeof(int),
		typeof(int)
	})]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> Int(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
	{
		return Transpile(instructions, generator);
	}

	private static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		Label label = default(Label);
		return new CodeMatcher(instructions, generator).CreateLabelAt(0, ref label).Start().InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
		{
			new CodeInstruction(OpCodes.Ldsfld, (object)AccessTools.Field(typeof(PreventStripping), "FieldZdoKeys"))
		})
			.InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
			{
				new CodeInstruction(OpCodes.Ldarg_0, (object)null)
			})
			.InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
			{
				new CodeInstruction(OpCodes.Call, (object)AccessTools.Method(typeof(HashSet<int>), "Contains", (Type[])null, (Type[])null))
			})
			.InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
			{
				new CodeInstruction(OpCodes.Brfalse, (object)label)
			})
			.InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
			{
				new CodeInstruction(OpCodes.Ldc_I4_0, (object)null)
			})
			.InsertAndAdvance((CodeInstruction[])(object)new CodeInstruction[1]
			{
				new CodeInstruction(OpCodes.Ret, (object)null)
			})
			.InstructionEnumeration();
	}
}
