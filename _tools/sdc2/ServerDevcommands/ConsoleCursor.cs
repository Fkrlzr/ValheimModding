using HarmonyLib;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(GameCamera), "UpdateMouseCapture")]
public class ConsoleCursor
{
	private static void Postfix()
	{
		if (Console.IsVisible())
		{
			ZCursor.LockState = (CursorLockMode)(!ZInput.IsMouseActive());
			ZCursor.Show();
		}
	}
}
