using HarmonyLib;
using UnityEngine;

namespace Service;

[HarmonyPatch(typeof(TextInput), "Hide")]
public class TextInput_OnCancel_Patch
{
	public static void Postfix()
	{
		if (Object.op_Implicit((Object)(object)TextInput.instance) && TextInput.instance.m_queuedSign is CommandInput commandInput)
		{
			commandInput.Hide();
		}
	}
}
