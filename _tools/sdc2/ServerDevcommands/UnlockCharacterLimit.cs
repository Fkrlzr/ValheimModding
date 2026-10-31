using HarmonyLib;
using TMPro;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Terminal), "Awake")]
public class UnlockCharacterLimit
{
	private static void Postfix(Terminal __instance)
	{
		if (Object.op_Implicit((Object)(object)__instance.m_input))
		{
			((TMP_InputField)__instance.m_input).characterLimit = 0;
		}
	}
}
