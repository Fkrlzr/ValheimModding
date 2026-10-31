using System;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Terminal), "UpdateChat")]
public class ConsoleScrollBar
{
	private static RectTransform? track;

	private static RectTransform? thumb;

	public static void Scroll(Terminal terminal, int height)
	{
		height = Mathf.Clamp(height, 0, Mathf.Max(0, terminal.m_chatBuffer.Count - 5));
		if (height != terminal.m_scrollHeight)
		{
			terminal.m_scrollHeight = height;
			terminal.UpdateChat();
		}
	}

	public static void GetRange(Terminal terminal, out int start, out int end)
	{
		int count = terminal.m_chatBuffer.Count;
		end = Mathf.Min(count, Mathf.Max(5, count - terminal.m_scrollHeight));
		start = Mathf.Max(0, end - terminal.m_maxVisibleBufferLength);
	}

	private static void Postfix(Terminal __instance)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)__instance != (Object)(object)Console.instance))
		{
			if (!Object.op_Implicit((Object)(object)track))
			{
				Create(__instance);
			}
			int count = __instance.m_chatBuffer.Count;
			((Component)track).gameObject.SetActive(count > __instance.m_maxVisibleBufferLength);
			if (count > __instance.m_maxVisibleBufferLength)
			{
				GetRange(__instance, out var start, out var end);
				thumb.anchorMin = new Vector2(0f, 1f - (float)end / (float)count);
				thumb.anchorMax = new Vector2(1f, 1f - (float)start / (float)count);
			}
		}
	}

	private static void Create(Terminal terminal)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = new GameObject("ScrollBar", new Type[2]
		{
			typeof(RectTransform),
			typeof(Image)
		});
		track = val.GetComponent<RectTransform>();
		((Transform)track).SetParent((Transform)(object)((TMP_Text)terminal.m_output).rectTransform, false);
		track.anchorMin = new Vector2(1f, 0f);
		track.anchorMax = Vector2.one;
		track.pivot = new Vector2(1f, 0.5f);
		track.sizeDelta = new Vector2(10f, 0f);
		((Graphic)val.GetComponent<Image>()).color = new Color(1f, 1f, 1f, 0.1f);
		val.AddComponent<ScrollBarDrag>();
		GameObject val2 = new GameObject("Thumb", new Type[2]
		{
			typeof(RectTransform),
			typeof(Image)
		});
		thumb = val2.GetComponent<RectTransform>();
		((Transform)thumb).SetParent((Transform)(object)track, false);
		thumb.offsetMin = Vector2.zero;
		thumb.offsetMax = Vector2.zero;
		((Graphic)val2.GetComponent<Image>()).color = new Color(1f, 1f, 1f, 0.5f);
	}
}
