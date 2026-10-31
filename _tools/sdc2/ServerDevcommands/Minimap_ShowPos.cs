using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace ServerDevcommands;

[HarmonyPatch(typeof(Minimap), "UpdateBiome")]
public class Minimap_ShowPos
{
	private static string PreviousSmallText = "";

	private static string PreviousLargeText = "";

	private static void AddText(TMP_Text input, string text)
	{
		if (!input.text.Contains(text))
		{
			input.text += text;
		}
	}

	private static void CleanUp(TMP_Text input, string text)
	{
		if (!(text == "") && input.text.Contains(text))
		{
			input.text = input.text.Replace(text, "");
		}
	}

	private static string Format(Vector3 position)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		string text = (Object.op_Implicit((Object)(object)Player.m_localPlayer) ? Player.m_localPlayer.GetPlayerName() : "Unknown");
		long num;
		if (!Object.op_Implicit((Object)(object)Player.m_localPlayer))
		{
			ZDOID none = ZDOID.None;
			num = ((ZDOID)(ref none)).UserID;
		}
		else
		{
			num = Player.m_localPlayer.GetPlayerID();
		}
		long num2 = num;
		return string.Format(Settings.Format(Settings.MinimapFormat), "Not available", text, num2, position.x, position.y, position.z);
	}

	private static string GetText(Vector3 position, float? distance = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		Vector2s zone = ZoneSystem.GetZone(position);
		string text = Format(position);
		string text2 = "zone: " + zone.x + "/" + zone.y;
		string text3 = (distance.HasValue ? $"\ndistance: {distance.Value:F0}" : "");
		HashSet<string> hashSet = WorldGenerator.instance.GetBiomeSector(position.x, position.z, false).AltBiomes.Select((AltBiome b) => b.m_name).ToHashSet();
		string text4 = ((Settings.ShowAlternativeBiomes && hashSet.Count > 0) ? ("\n" + string.Join("\n", hashSet)) : "");
		return "\n" + text2 + "\n" + text + text3 + text4;
	}

	private static void Postfix(Minimap __instance, Player player)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Invalid comparison between Unknown and I4
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Invalid comparison between Unknown and I4
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		MapMode mode = __instance.m_mode;
		if (PreviousSmallText != "")
		{
			CleanUp(__instance.m_biomeNameSmall, PreviousSmallText);
			PreviousSmallText = "";
		}
		if (PreviousLargeText != "")
		{
			CleanUp(__instance.m_biomeNameLarge, PreviousLargeText);
			PreviousLargeText = "";
		}
		Vector3 position = ((GameCamera.InFreeFly() && Object.op_Implicit((Object)(object)Utils.GetMainCamera())) ? ((Component)Utils.GetMainCamera()).transform.position : ((Component)player).transform.position);
		if ((int)mode == 1 && Settings.MiniMapCoordinates)
		{
			string text = GetText(position);
			AddText(__instance.m_biomeNameSmall, text);
			PreviousSmallText = text;
		}
		if ((int)mode == 2 && Settings.MapCoordinates)
		{
			position = __instance.ScreenToWorldPoint((Vector3)(ZInput.IsMouseActive() ? Input.mousePosition : new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f)));
			position.y = WorldGenerator.instance.GetHeight(position.x, position.z);
			string text2 = GetText(position, Utils.DistanceXZ(((Component)player).transform.position, position));
			AddText(__instance.m_biomeNameLarge, text2);
			PreviousLargeText = text2;
		}
	}
}
