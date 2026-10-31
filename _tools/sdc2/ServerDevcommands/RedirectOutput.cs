using System;
using HarmonyLib;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Terminal), "AddString", new Type[] { typeof(string) })]
public class RedirectOutput
{
	public static ZRpc? Target;

	private static void Postfix(string text)
	{
		if (ZNet.m_isServer && Target != null && !text.StartsWith("["))
		{
			ZNet.instance.RemotePrint(Target, text);
		}
	}
}
