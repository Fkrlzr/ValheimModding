using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using SkToolbox.Configuration;
using SkToolbox.SkModules;
using SkToolbox.Utility;
using Splatform;
using TMPro;
using UnityEngine;

namespace SkToolbox;

internal static class SkCommandProcessor
{
	[Flags]
	public enum LogTo
	{
		Console = 0,
		Chat = 1,
		DebugConsole = 2
	}

	internal static class TerrainModification
	{
		private static void CreateTerrain(GameObject prefab, Vector3 position, ZNetView component)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			float levelOffset = prefab.GetComponent<TerrainModifier>().m_levelOffset;
			GameObject val = Object.Instantiate<GameObject>(prefab, position - Vector3.up * levelOffset, Quaternion.identity);
		}

		public static void ModifyTerrain(Vector3 centerLocation, GameObject prefab, float radius)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			if (radius > 30f)
			{
				PrintOut("Radius clamped to 30 max!", LogTo.Console);
			}
			radius = Mathf.Clamp(radius, 0f, 30f);
			ZNetView component = ((Component)Player.m_localPlayer).gameObject.GetComponent<ZNetView>();
			int num = Mathf.CeilToInt(radius / 3f) * 3;
			Vector3 val = default(Vector3);
			for (int i = -num; i <= num; i += 3)
			{
				for (int j = -num; j <= num; j += 3)
				{
					val = new Vector3(centerLocation.x + (float)i, centerLocation.y, centerLocation.z + (float)j);
					if (Utils.DistanceXZ(centerLocation, val) <= radius)
					{
						CreateTerrain(prefab, val, component);
					}
				}
			}
			int num2 = int.MaxValue;
			int num3 = int.MaxValue;
			Vector3 val2 = default(Vector3);
			for (int k = -num; k <= 0; k++)
			{
				for (int l = -num; l <= -num / 2; l++)
				{
					val2 = new Vector3(centerLocation.x + (float)k, centerLocation.y, centerLocation.z + (float)l);
					if (!(Utils.DistanceXZ(centerLocation, val2) <= radius))
					{
						continue;
					}
					if (k < l || (l >= num3 && k <= num2 + 2 && k != 0))
					{
						break;
					}
					num2 = k;
					num3 = l;
					if (k / 3 * 3 == k && l / 3 * 3 == l)
					{
						break;
					}
					CreateTerrain(prefab, new Vector3(centerLocation.x + (float)k, centerLocation.y, centerLocation.z + (float)l), component);
					CreateTerrain(prefab, new Vector3(centerLocation.x + (float)k, centerLocation.y, centerLocation.z - (float)l), component);
					if (k != 0)
					{
						CreateTerrain(prefab, new Vector3(centerLocation.x - (float)k, centerLocation.y, centerLocation.z + (float)l), component);
						CreateTerrain(prefab, new Vector3(centerLocation.x - (float)k, centerLocation.y, centerLocation.z - (float)l), component);
					}
					if (k != l)
					{
						CreateTerrain(prefab, new Vector3(centerLocation.x + (float)l, centerLocation.y, centerLocation.z + (float)k), component);
						CreateTerrain(prefab, new Vector3(centerLocation.x - (float)l, centerLocation.y, centerLocation.z + (float)k), component);
						if (k != 0)
						{
							CreateTerrain(prefab, new Vector3(centerLocation.x + (float)l, centerLocation.y, centerLocation.z - (float)k), component);
							CreateTerrain(prefab, new Vector3(centerLocation.x - (float)l, centerLocation.y, centerLocation.z - (float)k), component);
						}
					}
					break;
				}
			}
		}

		public static void ResetTerrain(Vector3 centerLocation, float radius)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			if (radius > 50f)
			{
				PrintOut("Radius clamped to 50 max!", LogTo.Console);
			}
			radius = Mathf.Clamp(radius, 2f, 50f);
			try
			{
				List<TerrainModifier> allInstances = TerrainModifier.GetAllInstances();
				foreach (TerrainModifier item in allInstances)
				{
					if ((Object)(object)item != (Object)null && Utils.DistanceXZ(((Component)Player.m_localPlayer).transform.position, ((Component)item).transform.position) < radius)
					{
						ZNetView component = ((Component)item).GetComponent<ZNetView>();
						if ((Object)(object)component != (Object)null && component.IsValid())
						{
							component.ClaimOwnership();
							component.Destroy();
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<KeyValuePair<string, string>, string> _003C_003E9__50_0;

		public static Func<KeyValuePair<string, string>, string> _003C_003E9__50_1;

		public static Func<KeyValuePair<string, string>, string> _003C_003E9__50_2;

		public static ConsoleEvent _003C_003E9__53_0;

		public static ConsoleEvent _003C_003E9__53_1;

		public static ConsoleEvent _003C_003E9__53_2;

		public static ConsoleEvent _003C_003E9__53_3;

		public static ConsoleEvent _003C_003E9__53_4;

		public static ConsoleEvent _003C_003E9__53_5;

		public static ConsoleEvent _003C_003E9__53_6;

		public static ConsoleEvent _003C_003E9__53_7;

		public static ConsoleEvent _003C_003E9__53_8;

		public static ConsoleEvent _003C_003E9__53_9;

		public static ConsoleEvent _003C_003E9__53_10;

		public static ConsoleEvent _003C_003E9__53_11;

		public static Func<EnvSetup, string> _003C_003E9__53_49;

		public static Func<string, string> _003C_003E9__53_50;

		public static ConsoleEvent _003C_003E9__53_12;

		public static ConsoleEvent _003C_003E9__53_13;

		public static ConsoleEvent _003C_003E9__53_14;

		public static ConsoleEvent _003C_003E9__53_15;

		public static ConsoleEvent _003C_003E9__53_16;

		public static ConsoleEvent _003C_003E9__53_17;

		public static ConsoleEvent _003C_003E9__53_18;

		public static ConsoleEvent _003C_003E9__53_19;

		public static ConsoleEvent _003C_003E9__53_20;

		public static ConsoleEvent _003C_003E9__53_21;

		public static ConsoleEvent _003C_003E9__53_22;

		public static ConsoleEvent _003C_003E9__53_23;

		public static ConsoleEvent _003C_003E9__53_24;

		public static ConsoleEvent _003C_003E9__53_25;

		public static ConsoleEvent _003C_003E9__53_26;

		public static ConsoleEvent _003C_003E9__53_27;

		public static ConsoleEvent _003C_003E9__53_28;

		public static ConsoleEvent _003C_003E9__53_29;

		public static ConsoleEvent _003C_003E9__53_30;

		public static ConsoleEvent _003C_003E9__53_31;

		public static ConsoleEvent _003C_003E9__53_32;

		public static ConsoleEvent _003C_003E9__53_33;

		public static ConsoleEvent _003C_003E9__53_34;

		public static ConsoleEvent _003C_003E9__53_35;

		public static ConsoleEvent _003C_003E9__53_36;

		public static ConsoleEvent _003C_003E9__53_37;

		public static ConsoleEvent _003C_003E9__53_38;

		public static ConsoleEvent _003C_003E9__53_39;

		public static ConsoleEvent _003C_003E9__53_40;

		public static ConsoleEvent _003C_003E9__53_41;

		public static ConsoleEvent _003C_003E9__53_42;

		public static ConsoleEvent _003C_003E9__53_43;

		public static ConsoleEvent _003C_003E9__53_44;

		public static ConsoleEvent _003C_003E9__53_45;

		public static ConsoleEvent _003C_003E9__53_46;

		public static ConsoleEvent _003C_003E9__53_47;

		public static ConsoleEvent _003C_003E9__53_48;

		public static Predicate<ZDO> _003C_003E9__63_0;

		internal string _003CAnnounce_003Eb__50_0(KeyValuePair<string, string> obj)
		{
			return obj.Key;
		}

		internal string _003CAnnounce_003Eb__50_1(KeyValuePair<string, string> obj)
		{
			return obj.Key;
		}

		internal string _003CAnnounce_003Eb__50_2(KeyValuePair<string, string> obj)
		{
			return obj.Value;
		}

		internal void _003CInitCommands_003Eb__53_0(ConsoleEventArgs args)
		{
			if (args.Length > 1)
			{
				string text = string.Empty;
				for (int i = 0; i < args.Length; i++)
				{
					text = text + args[i] + " ";
				}
				PrintOut(text, LogTo.Console);
			}
		}

		internal void _003CInitCommands_003Eb__53_1(ConsoleEventArgs args)
		{
			if (args.Length > 1)
			{
				if (args[1].Equals("1"))
				{
					((Component)((Terminal)Console.instance).m_chatWindow).gameObject.SetActive(true);
					SkConfigEntry.CConsoleEnabled.Value = true;
				}
				else
				{
					((Component)((Terminal)Console.instance).m_chatWindow).gameObject.SetActive(false);
				}
			}
			else
			{
				((Component)((Terminal)Console.instance).m_chatWindow).gameObject.SetActive(!((Component)((Terminal)Console.instance).m_chatWindow).gameObject.activeSelf);
				if (((Component)((Terminal)Console.instance).m_chatWindow).gameObject.activeInHierarchy)
				{
					SkConfigEntry.CConsoleEnabled.Value = true;
				}
			}
		}

		internal void _003CInitCommands_003Eb__53_2(ConsoleEventArgs args)
		{
			PrintOut("Quitting game...", LogTo.DebugConsole);
			Application.Quit();
		}

		internal void _003CInitCommands_003Eb__53_3(ConsoleEventArgs args)
		{
			((TMP_Text)((Terminal)Console.instance).m_output).text = string.Empty;
			try
			{
				Console.instance.SetPrivateField("m_chatBuffer", new List<string>());
				ConsoleOpt.consoleOutputHistory.Clear();
			}
			catch (Exception)
			{
			}
		}

		internal void _003CInitCommands_003Eb__53_4(ConsoleEventArgs args)
		{
			List<ItemData> list = new List<ItemData>();
			((Humanoid)Player.m_localPlayer).GetInventory().GetWornItems(list);
			foreach (ItemData item in list)
			{
				try
				{
					item.m_durability = item.GetMaxDurability();
				}
				catch (Exception)
				{
				}
			}
			PrintOut("All items repaired!");
		}

		internal void _003CInitCommands_003Eb__53_5(ConsoleEventArgs args)
		{
			PrintOut(ListPortals(), LogTo.Console);
		}

		internal void _003CInitCommands_003Eb__53_6(ConsoleEventArgs args)
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			GameObject prefab = ZNetScene.instance.GetPrefab("digg_v2");
			if ((Object)(object)prefab == (Object)null)
			{
				PrintOut("Terrain level failed. Report to mod author - terrain level error 1");
				return;
			}
			float radius = 5f;
			if (args.Length > 1)
			{
				try
				{
					radius = int.Parse(args[1]);
				}
				catch (Exception)
				{
				}
			}
			TerrainModification.ModifyTerrain(((Component)Player.m_localPlayer).transform.position, prefab, radius);
			PrintOut("Terrain levelled!");
		}

		internal void _003CInitCommands_003Eb__53_7(ConsoleEventArgs args)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			float radius = 5f;
			if (args.Length > 1)
			{
				try
				{
					radius = int.Parse(args[1]);
				}
				catch (Exception)
				{
				}
			}
			TerrainModification.ResetTerrain(((Component)Player.m_localPlayer).transform.position, radius);
			PrintOut("Terrain reset!");
		}

		internal void _003CInitCommands_003Eb__53_8(ConsoleEventArgs args)
		{
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			GameObject prefab = ZNetScene.instance.GetPrefab("raise");
			if ((Object)(object)prefab == (Object)null)
			{
				PrintOut("Terrain raise failed. Report to mod author - terrain raise error 1");
				return;
			}
			float radius = 5f;
			float num = 2f;
			if (args.Length > 1)
			{
				try
				{
					radius = int.Parse(args[1]);
				}
				catch (Exception)
				{
				}
			}
			if (args.Length > 2)
			{
				try
				{
					num = int.Parse(args[2]);
				}
				catch (Exception)
				{
				}
			}
			TerrainModification.ModifyTerrain(((Component)Player.m_localPlayer).transform.position + Vector3.up * num, prefab, radius);
			PrintOut("Terrain raised!");
		}

		internal void _003CInitCommands_003Eb__53_9(ConsoleEventArgs args)
		{
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			GameObject prefab = ZNetScene.instance.GetPrefab("digg_v2");
			if ((Object)(object)prefab == (Object)null)
			{
				PrintOut("Terrain dig failed. Report to mod author - terrain dig error 1");
				return;
			}
			float radius = 5f;
			float num = 1f;
			if (args.Length > 1)
			{
				try
				{
					radius = int.Parse(args[1]);
				}
				catch (Exception)
				{
				}
			}
			if (args.Length > 2)
			{
				try
				{
					num = int.Parse(args[2]);
				}
				catch (Exception)
				{
				}
			}
			TerrainModification.ModifyTerrain(((Component)Player.m_localPlayer).transform.position - Vector3.up * num, prefab, radius);
			PrintOut("Terrain dug!");
		}

		internal void _003CInitCommands_003Eb__53_10(ConsoleEventArgs args)
		{
			EnvMan.instance.ResetDebugWind();
			PrintOut("Wind unlocked and under game control.");
		}

		internal void _003CInitCommands_003Eb__53_11(ConsoleEventArgs args)
		{
			if (args.Length == 3)
			{
				float num = float.Parse(args[1]);
				float num2 = float.Parse(args[2]);
				EnvMan.instance.SetDebugWind(num, num2);
			}
			else
			{
				PrintOut("Failed to set wind. Check parameters! Ex. /wind 240 5");
			}
		}

		internal void _003CInitCommands_003Eb__53_12(ConsoleEventArgs args)
		{
			if (args.Length == 1)
			{
				if (!((Object)(object)EnvMan.instance == (Object)null))
				{
					PrintOut("Current environment: " + (EnvMan.instance.GetCurrentEnvironment()?.m_name ?? "(none)"));
					{
						foreach (string item in from e in EnvMan.instance.m_environments
							select e.m_name into q
							orderby q
							select q)
						{
							PrintOut(item);
						}
						return;
					}
				}
				PrintOut("Can't find environment manager.");
			}
			else if (args.Length >= 2)
			{
				if (args[1].Equals("-1"))
				{
					if ((Object)(object)EnvMan.instance != (Object)null)
					{
						EnvMan.instance.m_debugEnv = "";
						PrintOut("Weather unlocked and under game control.");
					}
					return;
				}
				_003C_003Ec__DisplayClass53_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass53_0
				{
					finalWeatherName = string.Empty
				};
				if (args.Length > 2)
				{
					string[] args2 = args.Args;
					foreach (string text in args2)
					{
						if (!text.Equals(args[0]))
						{
							CS_0024_003C_003E8__locals0.finalWeatherName = CS_0024_003C_003E8__locals0.finalWeatherName + " " + text;
						}
					}
					CS_0024_003C_003E8__locals0.finalWeatherName = CS_0024_003C_003E8__locals0.finalWeatherName.Trim();
				}
				else
				{
					CS_0024_003C_003E8__locals0.finalWeatherName = args[1];
				}
				EnvSetup val = (((Object)(object)EnvMan.instance != (Object)null) ? EnvMan.instance.m_environments.FirstOrDefault((EnvSetup e) => string.Equals(e.m_name, CS_0024_003C_003E8__locals0.finalWeatherName, StringComparison.OrdinalIgnoreCase)) : null);
				if (val != null)
				{
					EnvMan.instance.m_debugEnv = val.m_name;
					PrintOut("Weather set to: " + EnvMan.instance.m_debugEnv);
				}
				else if ((Object)(object)EnvMan.instance == (Object)null)
				{
					PrintOut("Failed to set weather to '" + CS_0024_003C_003E8__locals0.finalWeatherName + "'. Can't find environment manager.");
				}
				else
				{
					PrintOut("Failed to set weather to '" + CS_0024_003C_003E8__locals0.finalWeatherName + "'. Unknown environment. Use /env to list, /env -1 to unlock.");
				}
			}
			else
			{
				PrintOut("Failed to set weather. Check parameters! Ex. /env, /env -1, /env Misty");
			}
		}

		internal string _003CInitCommands_003Eb__53_49(EnvSetup e)
		{
			return e.m_name;
		}

		internal string _003CInitCommands_003Eb__53_50(string q)
		{
			return q;
		}

		internal void _003CInitCommands_003Eb__53_13(ConsoleEventArgs args)
		{
			Player localPlayer = Player.m_localPlayer;
			if ((Object)(object)localPlayer == (Object)null || (Object)(object)((Component)localPlayer).GetComponent<ZNetView>() == (Object)null || !((Component)localPlayer).GetComponent<ZNetView>().IsValid())
			{
				PrintOut("Fly: no local player yet. Spawn into a world first.");
				return;
			}
			flyEnabled = localPlayer.ToggleDebugFly();
			Player.m_debugMode = flyEnabled || noCostEnabled || SkCommandPatcher.BCheat;
			PrintOut("Fly toggled! (" + flyEnabled + ")");
SkFlyDiag.Toggled();
		}

		internal void _003CInitCommands_003Eb__53_14(ConsoleEventArgs args)
		{
			if ((Object)(object)ObjectDB.instance == (Object)null || ObjectDB.instance.m_items == null)
			{
				PrintOut("Item database is not loaded yet.");
				return;
			}
			string text = ((args.Length > 1) ? args[1].ToLower() : string.Empty);
			int num = 0;
			foreach (GameObject item in ObjectDB.instance.m_items)
			{
				if ((Object)(object)item == (Object)null)
				{
					continue;
				}
				ItemDrop component = item.GetComponent<ItemDrop>();
				if (!((Object)(object)component == (Object)null) && component.m_itemData != null && component.m_itemData.m_shared != null)
				{
					Sprite[] icons = component.m_itemData.m_shared.m_icons;
					if (icons != null && icons.Length != 0 && !((Object)(object)icons[0] == (Object)null) && (text.Length <= 0 || ((Object)item).name.ToLower().IndexOf(text) >= 0))
					{
						PrintOut(((Object)item).name);
						num++;
					}
				}
			}
			PrintOut(num + " item icon(s) listed.");
		}

		internal void _003CInitCommands_003Eb__53_15(ConsoleEventArgs args)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			SkMenuController skMenuController = (((Object)(object)SkLoader.Load != (Object)null) ? SkLoader.Load.GetComponent<SkMenuController>() : null);
			object obj;
			if (!((Object)(object)skMenuController != (Object)null))
			{
				obj = "F6";
			}
			else
			{
				KeyCode toggleKey = skMenuController.ToggleKey;
				obj = ((object)toggleKey).ToString();
			}
			PrintOut("The menu is mouse-driven now. Press " + (string)obj + " to open it; the /alt keyboard controls were removed.");
		}

		internal void _003CInitCommands_003Eb__53_16(ConsoleEventArgs args)
		{
			if ((Object)(object)ZNet.instance == (Object)null || !ZNet.instance.IsServer())
			{
				PrintOut("Events can only be stopped by the host/server (the server re-syncs the active event every 2s).");
				return;
			}
			RandEventSystem.instance.ResetRandomEvent();
			PrintOut("Event stopped!");
		}

		internal void _003CInitCommands_003Eb__53_17(ConsoleEventArgs args)
		{
			Minimap.instance.ExploreAll();
			PrintOut("Map revealed!");
		}

		internal void _003CInitCommands_003Eb__53_18(ConsoleEventArgs args)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			string text = string.Empty;
			List<PlayerInfo> playerList = ZNet.instance.GetPlayerList();
			foreach (PlayerInfo item in playerList)
			{
				string[] obj = new string[6] { text, ", ", item.m_name, "(", null, null };
				ZDOID characterID = item.m_characterID;
				obj[4] = characterID.UserID.ToString();
				obj[5] = ")";
				text = string.Concat(obj);
			}
			if (text.Length > 2)
			{
				text = text.Remove(0, 2);
			}
			PrintOut("Active Players (" + playerList.Count + ") - " + text);
		}

		internal void _003CInitCommands_003Eb__53_19(ConsoleEventArgs args)
		{
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Expected O, but got Unknown
			string text = Player.m_localPlayer.GetPlayerName();
			string empty = string.Empty;
			int result = 1;
			int result2 = 1;
			if (args.Length < 2)
			{
				PrintOut("Failed. No item provided. /give [Item] [Qty=1] [Player] [Level=1]");
				return;
			}
			empty = args[1];
			if (args.Length >= 3 && !int.TryParse(args[2], out result))
			{
				result = 1;
			}
			if (args.Length >= 4)
			{
				text = args[3];
			}
			if (args.Length >= 5 && !int.TryParse(args[4], out result2))
			{
				result2 = 1;
			}
			Player val = null;
			foreach (Player allPlayer in Player.GetAllPlayers())
			{
				if ((Object)(object)allPlayer != (Object)null && allPlayer.GetPlayerName().ToLower().StartsWith(text.ToLower()))
				{
					val = allPlayer;
					break;
				}
			}
			if ((Object)(object)val == (Object)null)
			{
				PrintOut("Failed. Player does not exist. /give [Item] [Qty=1] [Player] [Level=1]");
				return;
			}
			GameObject prefab = ZNetScene.instance.GetPrefab(empty);
			if (Compat.Truthy((Object)(object)prefab))
			{
				PrintOut("Spawning " + result + " of item " + empty + "(" + result2 + ") on " + text);
				try
				{
					GameObject val2 = Object.Instantiate<GameObject>(prefab, ((Component)val).transform.position + ((Component)val).transform.forward * 1.5f + Vector3.up, Quaternion.identity);
					ItemDrop val3 = (ItemDrop)val2.GetComponent(typeof(ItemDrop));
					if ((Object)(object)val3 != (Object)null && val3.m_itemData != null)
					{
						val3.SetQuality(Mathf.Clamp(result2, 1, SkToolbox.SkModules.ModGive.MaxLevel));
						val3.m_itemData.m_stack = result;
						val3.m_itemData.m_durability = val3.m_itemData.GetMaxDurability();
					}
					return;
				}
				catch (Exception ex)
				{
					PrintOut("Something unexpected failed.");
					SkUtilities.Logz(new string[2] { "ERR", "/give" }, new string[2] { ex.Message, ex.Source }, (LogType)2);
					return;
				}
			}
			PrintOut("Failed. Check parameters. /give [Item] [Qty=1] [Player] [Level=1]");
		}

		internal void _003CInitCommands_003Eb__53_20(ConsoleEventArgs args)
		{
			if ((Object)(object)Player.m_localPlayer == (Object)null)
			{
				PrintOut("No local player.");
				return;
			}
			godEnabled = !Player.m_localPlayer.m_godMode;
			Player.m_localPlayer.SetGodMode(godEnabled);
			PrintOut("Godmode " + (godEnabled ? "ON" : "OFF") + ((godEnabled && (SkConfigEntry.CGodModeBlocksDamage == null || SkConfigEntry.CGodModeBlocksDamage.Value)) ? " - damage is blocked outright." : "."));
		}

		internal void _003CInitCommands_003Eb__53_21(ConsoleEventArgs args)
		{
			((Humanoid)Player.m_localPlayer).UnequipAllItems();
			((Humanoid)Player.m_localPlayer).GetInventory().RemoveAll();
			PrintOut("All items removed from inventory.");
		}

		internal void _003CInitCommands_003Eb__53_22(ConsoleEventArgs args)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			TombStone[] array = Object.FindObjectsByType<TombStone>((FindObjectsSortMode)0);
			if (array.Length != 0)
			{
				TombStone[] array2 = array;
				foreach (TombStone val in array2)
				{
					if ((Object)(object)val != (Object)null && ((Behaviour)val).enabled)
					{
						Minimap.instance.AddPin(((Component)val).transform.position, (PinType)12, "TS", true, true, 0L, default(PlatformUserID));
					}
				}
			}
			PrintOut("Tombstone sought out! Potentially " + array.Length + " found.");
		}

		internal void _003CInitCommands_003Eb__53_23(ConsoleEventArgs args)
		{
			World privateField = WorldGenerator.instance.GetPrivateField<World>("m_world");
			PrintOut("Map seed: " + privateField.m_seedName);
		}

		internal void _003CInitCommands_003Eb__53_24(ConsoleEventArgs args)
		{
			GameCamera.instance.ToggleFreeFly();
			PrintOut("Free cam toggled " + GameCamera.InFreeFly());
		}

		internal void _003CInitCommands_003Eb__53_25(ConsoleEventArgs args)
		{
			if (args.Length > 1)
			{
				foreach (Player allPlayer in Player.GetAllPlayers())
				{
					if ((Object)(object)allPlayer != (Object)null && allPlayer.GetPlayerName().ToLower().Equals(args[1].ToLower()))
					{
						((Character)allPlayer).Heal(((Character)allPlayer).GetMaxHealth(), true);
						PrintOut("Player healed: " + allPlayer.GetPlayerName());
					}
				}
				return;
			}
			((Character)Player.m_localPlayer).Heal(((Character)Player.m_localPlayer).GetMaxHealth(), true);
			PrintOut("Self healed.");
		}

		internal void _003CInitCommands_003Eb__53_26(ConsoleEventArgs args)
		{
			SkCommandPatcher.InitPatch();
			SkCommandPatcher.bBuildAnywhere = !SkCommandPatcher.bBuildAnywhere;
			PrintOut("No build restrictions toggled! (" + SkCommandPatcher.bBuildAnywhere + ")");
		}

		internal void _003CInitCommands_003Eb__53_27(ConsoleEventArgs args)
		{
			Player localPlayer = Player.m_localPlayer;
			if ((Object)(object)localPlayer == (Object)null)
			{
				PrintOut("No local player.");
				return;
			}
			noCostEnabled = !localPlayer.NoCostCheat();
			localPlayer.SetNoPlacementCost(noCostEnabled);
			Player.m_debugMode = flyEnabled || noCostEnabled || SkCommandPatcher.BCheat;
			PrintOut("No build cost/requirements toggled! (" + noCostEnabled + ")");
		}

		internal void _003CInitCommands_003Eb__53_28(ConsoleEventArgs args)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			if (args.Length != 2 || !args[1].Contains(","))
			{
				PrintOut("Syntax /tp X,Z");
				return;
			}
			try
			{
				string[] array = args[1].Split(',');
				float num = float.Parse(array[0]);
				float num2 = float.Parse(array[1]);
				float groundHeight = ZoneSystem.instance.GetGroundHeight(new Vector3(num, 750f, num2));
				bool flag = groundHeight < 749f;
				Player localPlayer = Player.m_localPlayer;
				if (Compat.Truthy((Object)(object)localPlayer))
				{
					float num3;
					if (flag)
					{
						num3 = Mathf.Max(groundHeight + 0.5f, ZoneSystem.instance.m_waterLevel);
					}
					else
					{
						float num4 = (localPlayer.m_debugFly ? 400f : 30f);
						num3 = Mathf.Clamp(((Component)localPlayer).transform.position.y, 30f, num4);
					}
					Vector3 val = default(Vector3);
					val = new Vector3(num, num3, num2);
					if (((Character)localPlayer).TeleportTo(val, ((Component)localPlayer).transform.rotation, true))
					{
						PrintOut("Teleporting...");
					}
					else
					{
						PrintOut("Teleport refused (already teleporting or <2s since last teleport). Try again in a moment.");
					}
				}
			}
			catch (Exception)
			{
				PrintOut("Syntax /tp X,Z");
			}
		}

		internal void _003CInitCommands_003Eb__53_29(ConsoleEventArgs args)
		{
			bDetectEnemies = !bDetectEnemies;
			if (args.Length > 0)
			{
				try
				{
					bDetectRange = int.Parse(args[1]);
					bDetectRange = ((bDetectRange < 5) ? 5 : bDetectRange);
				}
				catch (Exception)
				{
					bDetectRange = 20;
				}
			}
			PrintOut("Detect enemies toggled! (" + bDetectEnemies + ", range: " + bDetectRange + ")");
		}

		internal void _003CInitCommands_003Eb__53_30(ConsoleEventArgs args)
		{
			SkCommandPatcher.InitPatch();
			SkCommandPatcher.BCheat = !SkCommandPatcher.BCheat;
			try
			{
				Player.m_debugMode = SkCommandPatcher.BCheat;
				Terminal.m_cheat = SkCommandPatcher.BCheat;
				if (SkCommandPatcher.BCheat && (Object)(object)Game.instance != (Object)null)
				{
					PlayerProfile playerProfile = Game.instance.GetPlayerProfile();
					if (playerProfile != null && !playerProfile.m_usedCheats)
					{
						playerProfile.m_usedCheats = true;
						PrintOut(Localization.instance.Localize("$achievements_permanently_cheated_character"));
					}
				}
			}
			catch (Exception)
			{
			}
			if ((Object)(object)args.Context != (Object)null)
			{
				args.Context.InvokePrivateMethod("updateCommandList", null);
			}
			if ((Object)(object)Console.instance != (Object)null && Console.instance != args.Context)
			{
				Console.instance.InvokePrivateMethod("updateCommandList", null);
			}
			PrintOut("Cheats toggled! (" + SkCommandPatcher.BCheat + ")");
			if (SkCommandPatcher.BCheat)
			{
				PrintOut("Vanilla cheat commands (god/fly/ghost/nocost/debugmode...) are now valid on this client, including on joined servers.");
			}
		}

		internal void _003CInitCommands_003Eb__53_31(ConsoleEventArgs args)
		{
			SkCommandPatcher.InitPatch();
			SkCommandPatcher.BFreeSupport = !SkCommandPatcher.BFreeSupport;
			PrintOut("No build support requirements toggled! (" + SkCommandPatcher.BFreeSupport + ")");
		}

		internal void _003CInitCommands_003Eb__53_32(ConsoleEventArgs args)
		{
			bCoords = !bCoords;
			PrintOut("Show coords toggled! (" + bCoords + ")");
		}

		internal void _003CInitCommands_003Eb__53_33(ConsoleEventArgs args)
		{
			Minimap.instance.Reset();
		}

		internal void _003CInitCommands_003Eb__53_34(ConsoleEventArgs args)
		{
			Player localPlayer = Player.m_localPlayer;
			if ((Object)(object)localPlayer == (Object)null)
			{
				PrintOut("Infinite stamina: no local player yet. Spawn into a world first.");
				return;
			}
			infStamina = !infStamina;
			ApplyInfStamina(localPlayer, applyOffValues: true);
			PrintOut("Infinite stamina toggled! (" + infStamina + ")");
		}

		internal void _003CInitCommands_003Eb__53_35(ConsoleEventArgs args)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			Tameable.TameAllInArea(((Component)Player.m_localPlayer).transform.position, 20f);
			PrintOut("Creatures tamed!");
		}

		internal void _003CInitCommands_003Eb__53_36(ConsoleEventArgs args)
		{
			Player localPlayer = Player.m_localPlayer;
			if ((Object)(object)localPlayer == (Object)null)
			{
				PrintOut("Far interactions: no local player yet. Spawn into a world first.");
				return;
			}
			farInteract = !farInteract;
			if (farInteract && args.Length > 1)
			{
				try
				{
					farInteractDistance = ((int.Parse(args[1]) < 20) ? 20 : int.Parse(args[1]));
				}
				catch (Exception)
				{
					PrintOut("Failed to set far interaction distance. Check params. /farinteract 50");
				}
			}
			ApplyFarInteract(localPlayer, applyOffValues: true);
			PrintOut(farInteract ? ("Far interactions toggled! (True Distance: " + farInteractDistance + ")") : "Far interactions toggled! (False)");
		}

		internal void _003CInitCommands_003Eb__53_37(ConsoleEventArgs args)
		{
			Player localPlayer = Player.m_localPlayer;
			if ((Object)(object)localPlayer == (Object)null)
			{
				PrintOut("Ghost: no local player yet. Spawn into a world first.");
				return;
			}
			localPlayer.SetGhostMode(!localPlayer.m_ghostMode);
			ghostEnabled = localPlayer.m_ghostMode;
			PrintOut("Ghost mode toggled! (" + ghostEnabled + ")");
		}

		internal void _003CInitCommands_003Eb__53_38(ConsoleEventArgs args)
		{
			if (args.Length > 1)
			{
				if (float.TryParse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
				{
					if (result < 0f)
					{
						EnvMan.instance.m_debugTimeOfDay = false;
						PrintOut("Time unlocked and under game control.");
					}
					else
					{
						EnvMan.instance.m_debugTimeOfDay = true;
						EnvMan.instance.m_debugTime = Mathf.Clamp01(result);
						PrintOut("Setting time of day:" + result);
					}
				}
			}
			else
			{
				PrintOut("Failed. Syntax /tod [0-1] Ex. /tod 0.5");
			}
		}

		internal void _003CInitCommands_003Eb__53_39(ConsoleEventArgs args)
		{
			TerrainComp.UpgradeTerrain();
			Heightmap.UpdateTerrainAlpha();
		}

		internal void _003CInitCommands_003Eb__53_40(ConsoleEventArgs args)
		{
			if (args.Length <= 1)
			{
				return;
			}
			if (args[1].Equals("cw"))
			{
				try
				{
					int num = int.Parse(args[2]);
					Player.m_localPlayer.m_maxCarryWeight = num;
					PrintOut("New carry weight set to: " + num);
					return;
				}
				catch (Exception)
				{
					PrintOut("Failed to set new carry weight. Check params.");
					return;
				}
			}
			if (args[1].Equals("skill"))
			{
				if (args.Length == 4 && !args[2].Contains("None") && !args[2].Contains("All") && !args[2].Contains("FireMagic") && !args[2].Contains("FrostMagic"))
				{
					try
					{
						string text = args[2];
						int num2 = int.Parse(args[3]);
						((Character)Player.m_localPlayer).GetSkills().CheatResetSkill(text.ToLower());
						((Character)Player.m_localPlayer).GetSkills().CheatRaiseSkill(text.ToLower(), (float)num2, true);
						return;
					}
					catch (Exception)
					{
						PrintOut("Failed to set skill. Check params / skill name. See /listskills. /set skill [skill] [level]");
						return;
					}
				}
				PrintOut("Failed to set skill. Check params / skill name. See /listskills.  /set skill [skill] [level]");
			}
			else if (args[1].Equals("pickup"))
			{
				if (args.Length >= 3)
				{
					try
					{
						int num3 = int.Parse(args[2]);
						Player.m_localPlayer.m_autoPickupRange = num3;
						PrintOut("New range set to: " + num3);
						return;
					}
					catch (Exception)
					{
						PrintOut("Failed to set pickup range. Check params. /set pickup 2");
						return;
					}
				}
				PrintOut("Failed to set pickup range. Check params.  /set pickup 2");
			}
			else if (args[1].Equals("jumpforce"))
			{
				if (args.Length >= 3)
				{
					try
					{
						int num4 = int.Parse(args[2]);
						((Character)Player.m_localPlayer).m_jumpForce = num4;
						PrintOut("New range set to: " + num4);
						return;
					}
					catch (Exception)
					{
						PrintOut("Failed to set jump force. Check params. /set jumpforce 10");
						return;
					}
				}
				PrintOut("Failed to set jump force. Check params.  /set jumpforce 10");
			}
			else if (args[1].Equals("exploreradius"))
			{
				if (args.Length >= 3)
				{
					try
					{
						int num5 = int.Parse(args[2]);
						Minimap.instance.m_exploreRadius = num5;
						PrintOut("New range set to: " + num5);
						return;
					}
					catch (Exception)
					{
						PrintOut("Failed to set explore radius. Check params. /set exploreradius 100");
						return;
					}
				}
				PrintOut("Failed to set explore radius. Check params.  /set exploreradius 100");
			}
			else if (args[1].Equals("speed"))
			{
				if (args.Length >= 4)
				{
					string text2 = args[2];
					string[] source = new string[3] { "crouch", "run", "swim" };
					if (source.Contains(text2))
					{
						float num6 = 0f;
						try
						{
							int num7 = int.Parse(args[3]);
							switch (text2)
							{
							case "crouch":
								num6 = ((Character)Player.m_localPlayer).m_crouchSpeed;
								((Character)Player.m_localPlayer).m_crouchSpeed = num7;
								break;
							case "run":
								num6 = ((Character)Player.m_localPlayer).m_runSpeed;
								((Character)Player.m_localPlayer).m_runSpeed = num7;
								break;
							case "swim":
								num6 = ((Character)Player.m_localPlayer).m_swimSpeed;
								((Character)Player.m_localPlayer).m_swimSpeed = num7;
								break;
							}
							PrintOut("New " + text2 + " speed set to: " + num7 + " (was: " + num6 + ")");
							return;
						}
						catch (Exception)
						{
							PrintOut("Failed to set speed. Check params name. Ex. /set speed crouch 2");
							return;
						}
					}
					PrintOut("Failed to set speed. Check params name. Ex.  /set speed crouch 2");
				}
				else
				{
					PrintOut("Failed to set speed. Check params name. Ex.  /set speed crouch 2");
				}
			}
			else
			{
				if (!args[1].Equals("difficulty"))
				{
					return;
				}
				if (args.Length >= 3)
				{
					try
					{
						int forcePlayerDifficulty = int.Parse(args[2]);
						Game.instance.SetForcePlayerDifficulty(forcePlayerDifficulty);
						PrintOut("Difficulty set to " + forcePlayerDifficulty);
						return;
					}
					catch (Exception)
					{
						PrintOut("Failed to set difficulty. Check params. /set difficulty 5");
						return;
					}
				}
				PrintOut("Failed to set difficulty. Check params.  /set difficulty 5");
			}
		}

		internal void _003CInitCommands_003Eb__53_41(ConsoleEventArgs args)
		{
			ItemDrop[] array = Object.FindObjectsByType<ItemDrop>((FindObjectsSortMode)0);
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				Fish component = ((Component)array[i]).GetComponent<Fish>();
				if ((Compat.Truthy((Object)(object)component) && !component.IsOutOfWater()) || array[i].IsPiece())
				{
					continue;
				}
				ZNetView component2 = ((Component)array[i]).GetComponent<ZNetView>();
				if (Compat.Truthy((Object)(object)component2) && component2.IsValid())
				{
					if (!component2.IsOwner())
					{
						component2.ClaimOwnership();
					}
					component2.Destroy();
					num++;
				}
			}
			PrintOut("Items cleared (" + num + ").");
		}

		internal void _003CInitCommands_003Eb__53_42(ConsoleEventArgs args)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			GameObject val = SpawnPrefab(args);
			if ((Object)(object)val == (Object)null)
			{
				return;
			}
			MonsterAI component = val.GetComponent<MonsterAI>();
			if ((Object)(object)component == (Object)null)
			{
				PrintOut("Spawned, but " + args[1] + " has no MonsterAI and cannot be tamed.");
				return;
			}
			component.MakeTame();
			Tameable component2 = val.GetComponent<Tameable>();
			if ((Object)(object)component2 != (Object)null)
			{
				component2.m_tamedEffect.Create(val.transform.position, val.transform.rotation, (Transform)null, 1f, -1, default(ZDOID));
			}
			PrintOut("Spawned tamed - " + args[1]);
		}

		internal void _003CInitCommands_003Eb__53_43(ConsoleEventArgs args)
		{
			GameObject val = SpawnPrefab(args);
			if ((Object)(object)val != (Object)null)
			{
				PrintOut("Spawned - " + args[1]);
			}
		}

		internal void _003CInitCommands_003Eb__53_44(ConsoleEventArgs args)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			List<Character> list = new List<Character>();
			Character.GetCharactersInRange(((Component)Player.m_localPlayer).transform.position, 50f, list);
			foreach (Character item in list)
			{
				if (!item.IsPlayer())
				{
					HitData val = new HitData();
					val.m_damage.m_damage = 1E+10f;
					item.Damage(val);
				}
			}
			PrintOut("Nearby creatures killed! (50m)");
		}

		internal void _003CInitCommands_003Eb__53_45(ConsoleEventArgs args)
		{
			if (args.Length > 1)
			{
				foreach (GameObject item in ObjectDB.instance.m_items)
				{
					ItemDrop component = item.GetComponent<ItemDrop>();
					if (((Object)component).name.ToLower().Contains(args[1].ToLower()))
					{
						PrintOut("Item: '" + ((Object)component).name + "'", LogTo.DebugConsole);
					}
				}
				return;
			}
			foreach (GameObject item2 in ObjectDB.instance.m_items)
			{
				ItemDrop component2 = item2.GetComponent<ItemDrop>();
				PrintOut("Item: '" + ((Object)component2).name + "'", LogTo.DebugConsole);
			}
		}

		internal void _003CInitCommands_003Eb__53_46(ConsoleEventArgs args)
		{
			ConsoleOpt.BuildPrefabs();
			if (args.Length > 1)
			{
				foreach (string prefab in ConsoleOpt.PrefabList)
				{
					if (prefab.ToLower().StartsWith(args[1].ToLower()))
					{
						PrintOut("Prefab: '" + prefab + "'", LogTo.DebugConsole);
					}
				}
				return;
			}
			foreach (string prefab2 in ConsoleOpt.PrefabList)
			{
				PrintOut("Prefab: '" + prefab2 + "'", LogTo.DebugConsole);
			}
		}

		internal void _003CInitCommands_003Eb__53_47(ConsoleEventArgs args)
		{
			string text = "Skills found: ";
			foreach (object value in Enum.GetValues(typeof(SkillType)))
			{
				if (!value.ToString().Contains("None") && !value.ToString().Contains("All") && !value.ToString().Contains("FireMagic") && !value.ToString().Contains("FrostMagic"))
				{
					text = text + value.ToString() + ", ";
				}
			}
			text = text.Remove(text.Length - 2);
			PrintOut(text, LogTo.DebugConsole);
		}

		internal void _003CInitCommands_003Eb__53_48(ConsoleEventArgs args)
		{
			((Terminal)Console.instance).TryRunCommand("help", false, false);
		}

		internal bool _003CListPortals_003Eb__63_0(ZDO z)
		{
			return !z.IsValid();
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass53_0
	{
		public string finalWeatherName;

		internal bool _003CInitCommands_003Eb__51(EnvSetup e)
		{
			return string.Equals(e.m_name, finalWeatherName, StringComparison.OrdinalIgnoreCase);
		}
	}

	public static bool flyEnabled = false;

	public static bool godEnabled = false;

	public static bool farInteract = false;

	public static float farInteractDistance = 50f;

	public static bool infStamina = false;

	public static bool noCostEnabled = false;

	public static bool ghostEnabled = false;

	public static bool infEitr = false;

	public static bool noCarryLimit = false;

	public static readonly HashSet<int> immuneTo = new HashSet<int>();

	public static int carryWeight = 0;

	public static int pickupRange = 0;

	public static int jumpForce = 0;

	public static int runSpeed = 0;

	public static int swimSpeed = 0;

	public static int exploreRadius = 0;

	public static bool bTeleport = false;

	public static bool bDebugTime = false;

	public static bool bDetectEnemies = false;

	public static bool btDetectEnemiesSwitch = true;

	public static int bDetectRange = 20;

	public static bool altOnScreenControls = false;

	public static bool bCoords = false;

	public static int pageSize = 11;

	private static Vector3 chatPos = new Vector3(0f, -99f);

	private static ModConsole consoleOpt = null;

	private static bool eitrBaselineTaken = false;

	private static float baseEitrRegen;

	private static float baseEitrRegenDelay;

	private static bool tuningBaselineTaken = false;

	private static float baseCarry;

	private static float basePickup;

	private static float baseJump;

	private static float baseRun;

	private static float baseSwim;

	private static float baseExplore;

	public static Dictionary<string, string> commandList = new Dictionary<string, string>
	{
		{ "/alt", "- (Removed) The menu is mouse-driven now. Press the MenuToggleKey (default F6) to open it." },
		{ "/listicons", "[Filter] - List item prefabs that have an icon, for picking menu icons" },
		{ "/console", "[1/0] - Toggle the console. No parameter with toggle. 1 = Open, 0 = Closed. Intended for use with hotkeys and aliases." },
		{ "/coords", "- Show coords in corner of the screen" },
		{ "/clear", "- Clear the current output shown in the console" },
		{ "/clearinventory", "- Removes all items from your inventory. There is no confirmation, be careful." },
		{ "/detect", "[Range=20] - Toggle enemy detection" },
		{ "/farinteract", "[Distance=50] - Toggles far interactions (building as well). To change distance, toggle this off then back on with new distance" },
		{ "/echo", "[Text] - Echo the text back to the console. This is intended for use with aliases and the autorun features." },
		{ "/env", "[Weather] - Change the weather. No parameter provided will list all weather. -1 will allow the game to control the weather again." },
		{ "/findtomb", "- Pin nearby dead player tombstones on the map if any currently exist" },
		{ "/fly", "- Toggle flying" },
		{ "/freecam", "- Toggle freecam" },
		{ "/ghost", "- Toggle Ghostmode (enemy creatures cannot see you)" },
		{ "/give", "[Item] [Qty=1], OR /give [Item] [Qty=1] [Player] [Level=1] - Gives item to player. If player has a space in name, only provide name before the space. Capital letters matter in item / player name!" },
		{ "/god", "- Toggle Godmode" },
		{ "/heal", "[Player=local] - Heal Player" },
		{ "/imacheater", "- Use the toolbox to force enable standard cheats on any server" },
		{ "/infstam", "- Toggles infinite stamina" },
		{ "/killall", "- Kills all nearby creatures" },
		{ "/listitems", "[Name Contains] - List all items. Optionally include name starts with. Ex. /listitems Woo returns any item that contains the letters 'Woo'" },
		{ "/listprefabs", "[Name Contains] - Lists all prefabs - List all prefabs/creatures. Optionally include name starts with. Ex. /listprefabs Troll returns any prefab that starts with the letters 'Troll'" },
		{ "/listskills", "- Lists all skills" },
		{ "/nocost", "- Toggle no requirement building" },
		{ "/nores", "- Toggle no restrictions to where you can build (except ward zones)" },
		{ "/nosup", "- Toggle no supports required for buildings - WARNING! - IF YOU REJOIN AND THIS IS DISABLED, YOUR STRUCTURES MAY FALL APART - USE WITH CARE. Maybe use the AutoRun functionality?" },
		{ "/optterrain", "- Optimize old terrain modifications" },
		{ "/portals", "- List all portal tags" },
		{ "/removedrops", "- Removes items from the ground" },
		{ "/resetwind", "- If wind has been set, this will allow the game to take control of the wind again" },
		{ "/repair", "- Repair your inventory" },
		{ "/resetmap", "- Reset the map exploration" },
		{ "/revealmap", "- Reveals the entire minimap" },
		{ "/q", "- Quickly exit the game. Commands are sometimes just more convenient." },
		{ "/seed", "- Reveals the map seed" },
		{ "/set cw", "[Weight] - Set your weight limit (default 300)" },
		{ "/set difficulty", "[Player Count] - Set the difficulty (default is number of connected players)" },
		{ "/set exploreradius", "[Radius=100] - Set the explore radius" },
		{ "/set jumpforce", "[Force] - Set jump force (default 10). Careful if you fall too far!" },
		{ "/set pickup", "[Radius] - Set your auto pickup radius (default 2)" },
		{ "/set skill", "[Skill] [Level] - Set your skill level" },
		{ "/set speed", "[Speed Type] [Speed] - Speed Types: crouch (def: 2), run (def: 7), swim (def: 2)" },
		{ "/td", "[Radius=5] [Height=1] - Dig nearby terrain. Radius 30 max." },
		{ "/tl", "[Radius=5] - Level nearby terrain. Radius 30 max." },
		{ "/tr", "[Radius=5] [Height=1] - Raise nearby terrain. Radius 30 max." },
		{ "/tu", "[Radius=5] - Undo terrain modifications around you. Radius 50 max." },
		{ "/spawn", "[Creature Name] [Level=1] - Spawns a creature or prefab in front of you. Capitals in name matter! Ex. /spawn Boar 3 (use /give for items!)" },
		{ "/spawntamed", "[Creature Name] [Level=1] - Spawns a tamed creature in front of you. Capitals in name matter! Ex. /spawntamed Boar 3" },
		{ "/stopevent", "- Stops a current event" },
		{ "/tame", "- Tame all nearby creatures" },
		{ "/tod", "[0-1] - Set (and lock) time of day (-1 to unlock time) - Ex. /tod 0.5" },
		{ "/tp", "[X,Y] - Teleport you to the coords provided" },
		{ "/wind", "[Angle] [Intensity] - Set the wind direction and intensity" },
		{ "/whois", "- List all players" }
	};

	internal static bool MenuFeedbackActive = false;

	internal static bool MenuFeedbackShown = false;

	internal static ModConsole ConsoleOpt
	{
		get
		{
			return consoleOpt;
		}
		set
		{
			consoleOpt = value;
		}
	}

	internal static void ApplyInfStamina(Player lp, bool applyOffValues)
	{
		if (!((Object)(object)lp == (Object)null))
		{
			if (infStamina)
			{
				lp.m_staminaRegenDelay = 0.05f;
				lp.m_staminaRegen = 999f;
				lp.m_runStaminaDrain = 0f;
				lp.m_encumberedStaminaDrain = 0f;
				lp.m_swimStaminaDrainMinSkill = 0f;
				lp.m_swimStaminaDrainMaxSkill = 0f;
				lp.m_sneakStaminaDrain = 0f;
			}
			else if (applyOffValues)
			{
				lp.m_staminaRegenDelay = 1f;
				lp.m_staminaRegen = 5f;
				lp.m_runStaminaDrain = 10f;
				lp.m_encumberedStaminaDrain = 10f;
				lp.m_swimStaminaDrainMinSkill = 5f;
				lp.m_swimStaminaDrainMaxSkill = 2f;
				lp.m_sneakStaminaDrain = 5f;
			}
		}
	}

	internal static void ApplyInfEitr(Player lp, bool applyOffValues)
	{
		if (!((Object)(object)lp == (Object)null))
		{
			if (!eitrBaselineTaken)
			{
				eitrBaselineTaken = true;
				baseEitrRegen = lp.m_eiterRegen;
				baseEitrRegenDelay = lp.m_eitrRegenDelay;
			}
			if (infEitr)
			{
				lp.m_eiterRegen = 999f;
				lp.m_eitrRegenDelay = 0.05f;
			}
			else if (applyOffValues)
			{
				lp.m_eiterRegen = baseEitrRegen;
				lp.m_eitrRegenDelay = baseEitrRegenDelay;
			}
		}
	}

	internal static void CaptureTuningBaseline(Player lp)
	{
		if (!((Object)(object)lp == (Object)null) && !tuningBaselineTaken)
		{
			tuningBaselineTaken = true;
			baseCarry = lp.m_maxCarryWeight;
			basePickup = lp.m_autoPickupRange;
			baseJump = ((Character)lp).m_jumpForce;
			baseRun = ((Character)lp).m_runSpeed;
			baseSwim = ((Character)lp).m_swimSpeed;
			baseExplore = (((Object)(object)Minimap.instance != (Object)null) ? Minimap.instance.m_exploreRadius : 100f);
		}
	}

	internal static void ApplyTuning(Player lp)
	{
		if (!((Object)(object)lp == (Object)null))
		{
			CaptureTuningBaseline(lp);
			if (noCarryLimit)
			{
				lp.m_maxCarryWeight = 99999f;
			}
			else if (carryWeight > 0)
			{
				lp.m_maxCarryWeight = carryWeight;
			}
			if (pickupRange > 0)
			{
				lp.m_autoPickupRange = pickupRange;
			}
			if (jumpForce > 0)
			{
				((Character)lp).m_jumpForce = jumpForce;
			}
			if (runSpeed > 0)
			{
				((Character)lp).m_runSpeed = runSpeed;
			}
			if (swimSpeed > 0)
			{
				((Character)lp).m_swimSpeed = swimSpeed;
			}
			if (exploreRadius > 0 && (Object)(object)Minimap.instance != (Object)null)
			{
				Minimap.instance.m_exploreRadius = exploreRadius;
			}
		}
	}

	internal static void RestoreCarryBaseline(Player lp)
	{
		if (!((Object)(object)lp == (Object)null) && tuningBaselineTaken)
		{
			lp.m_maxCarryWeight = baseCarry;
		}
	}

	internal static void ResetTuning(Player lp)
	{
		noCarryLimit = false;
		carryWeight = 0;
		pickupRange = 0;
		jumpForce = 0;
		runSpeed = 0;
		swimSpeed = 0;
		exploreRadius = 0;
		if (!((Object)(object)lp == (Object)null) && tuningBaselineTaken)
		{
			lp.m_maxCarryWeight = baseCarry;
			lp.m_autoPickupRange = basePickup;
			((Character)lp).m_jumpForce = baseJump;
			((Character)lp).m_runSpeed = baseRun;
			((Character)lp).m_swimSpeed = baseSwim;
			if ((Object)(object)Minimap.instance != (Object)null)
			{
				Minimap.instance.m_exploreRadius = baseExplore;
			}
		}
	}

	internal static void ApplyFarInteract(Player lp, bool applyOffValues)
	{
		if (!((Object)(object)lp == (Object)null))
		{
			if (farInteract)
			{
				lp.m_maxInteractDistance = farInteractDistance;
				lp.m_maxPlaceDistance = farInteractDistance;
			}
			else if (applyOffValues)
			{
				lp.m_maxInteractDistance = 5f;
				lp.m_maxPlaceDistance = 5f;
			}
		}
	}

	internal static void SnapshotOnDeath(Player lp)
	{
		if (!((Object)(object)lp == (Object)null))
		{
			godEnabled = lp.m_godMode;
			flyEnabled = lp.m_debugFly;
			noCostEnabled = lp.NoCostCheat();
			ghostEnabled = lp.m_ghostMode;
		}
	}

	internal static void ReapplyOnSpawn(Player lp)
	{
		if ((Object)(object)lp == (Object)null)
		{
			return;
		}
		if (SkConfigEntry.CPersistCheatsOnDeath != null && !SkConfigEntry.CPersistCheatsOnDeath.Value)
		{
			infStamina = false;
			farInteract = false;
			godEnabled = false;
			flyEnabled = false;
			noCostEnabled = false;
			ghostEnabled = false;
			infEitr = false;
			immuneTo.Clear();
			return;
		}
		ApplyInfStamina(lp, applyOffValues: false);
		ApplyInfEitr(lp, applyOffValues: false);
		ApplyFarInteract(lp, applyOffValues: false);
		ApplyTuning(lp);
		if (ghostEnabled && !lp.m_ghostMode)
		{
			lp.SetGhostMode(true);
		}
		if (godEnabled && !lp.m_godMode)
		{
			lp.SetGodMode(true);
		}
		if (noCostEnabled && !lp.NoCostCheat())
		{
			lp.SetNoPlacementCost(true);
		}
		if (flyEnabled && !lp.m_debugFly)
		{
			ZNetView component = ((Component)lp).GetComponent<ZNetView>();
			if ((Object)(object)component != (Object)null && component.IsValid())
			{
				lp.ToggleDebugFly();
			}
		}
		Player.m_debugMode = Player.m_debugMode || flyEnabled || noCostEnabled || SkCommandPatcher.BCheat;
	}

	public static void Announce()
	{
		if ((SkConfigEntry.CScrollable != null) & !SkConfigEntry.CScrollable.Value)
		{
			if (SkVersionChecker.VersionCurrent())
			{
				PrintOut("====  Toolbox (" + SkVersionChecker.currentVersion?.ToString() + ") by Skrip (DS) is enabled.\t\t====", LogTo.Console);
			}
			else
			{
				PrintOut("Toolbox by Skrip (DS) is enabled.", LogTo.Console);
				PrintOut("New Version Available on NexusMods!\t► Current: " + SkVersionChecker.currentVersion?.ToString() + " Latest: " + SkVersionChecker.latestVersion, LogTo.DebugConsole);
			}
			PrintOut("====  Press numpad 0 to open on-screen menu or type /? 1\t====", LogTo.Console);
		}
		try
		{
			commandList = commandList.OrderBy((KeyValuePair<string, string> obj) => obj.Key).ToDictionary((KeyValuePair<string, string> obj) => obj.Key, (KeyValuePair<string, string> obj) => obj.Value);
			SkCommandPatcher.InitPatch();
		}
		catch (Exception)
		{
		}
	}

	public static string DecomposeAlias(string inCommand)
	{
		string[] array = inCommand.Split(';');
		string text = inCommand;
		if ((Object)(object)ConsoleOpt != (Object)null && ConsoleOpt.AliasList != null && ConsoleOpt.AliasList.Count > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]))
				{
					array[i] = array[i].Trim();
				}
				if (ConsoleOpt.AliasList.Keys.Contains(array[i]))
				{
					string value = string.Empty;
					ConsoleOpt.AliasList.TryGetValue(array[i], out value);
					value = value.Trim();
					if (value.EndsWith(";"))
					{
						value = value.Substring(0, value.Length - 1);
					}
					array[i] = value;
				}
			}
			text = string.Empty;
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				text = ((array.Length <= 1) ? (text + text2) : (text + text2 + ";"));
			}
		}
		return text;
	}

	public static void ProcessCommands(string inCommand, LogTo source, GameObject go = null)
	{
		if (string.IsNullOrEmpty(inCommand))
		{
			return;
		}
		if ((Object)(object)Console.instance != (Object)null)
		{
			List<string> privateField = Console.instance.GetPrivateField<List<string>>("m_history");
			if (privateField != null)
			{
				if (privateField.Count == 0 || privateField[privateField.Count - 1] != inCommand)
				{
					privateField.Add(inCommand);
				}
				Console.instance.SetPrivateField("m_historyPosition", privateField.Count);
			}
		}
		inCommand = inCommand.Trim();
		inCommand = DecomposeAlias(inCommand);
		string[] array = inCommand.Split(';');
		string[] array2 = array;
		foreach (string text in array2)
		{
			string text2 = text.Trim();
			if (!string.IsNullOrEmpty(text2))
			{
				((Terminal)Console.instance).TryRunCommand(text2, false, false);
			}
		}
		((TMP_InputField)((Terminal)Console.instance).m_input).text = string.Empty;
	}

	public static void InitCommands()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Expected O, but got Unknown
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Expected O, but got Unknown
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Expected O, but got Unknown
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Expected O, but got Unknown
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Expected O, but got Unknown
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Expected O, but got Unknown
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Expected O, but got Unknown
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Expected O, but got Unknown
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Expected O, but got Unknown
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Expected O, but got Unknown
		//IL_067f: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Expected O, but got Unknown
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		//IL_072a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Expected O, but got Unknown
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Expected O, but got Unknown
		//IL_079c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0787: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Expected O, but got Unknown
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Expected O, but got Unknown
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Expected O, but got Unknown
		//IL_0847: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Expected O, but got Unknown
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Expected O, but got Unknown
		//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08af: Expected O, but got Unknown
		//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e8: Expected O, but got Unknown
		//IL_092b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0916: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0921: Expected O, but got Unknown
		//IL_0964: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Expected O, but got Unknown
		//IL_099d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_098d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0993: Expected O, but got Unknown
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cc: Expected O, but got Unknown
		//IL_0a0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a05: Expected O, but got Unknown
		//IL_0a48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3e: Expected O, but got Unknown
		//IL_0a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a77: Expected O, but got Unknown
		//IL_0aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Expected O, but got Unknown
		//IL_0af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae9: Expected O, but got Unknown
		if (!((Object)(object)Console.instance != (Object)null))
		{
			return;
		}
		object obj = _003C_003Ec._003C_003E9__53_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				if (args.Length > 1)
				{
					string text8 = string.Empty;
					for (int l = 0; l < args.Length; l++)
					{
						text8 = text8 + args[l] + " ";
					}
					PrintOut(text8, LogTo.Console);
				}
			};
			_003C_003Ec._003C_003E9__53_0 = val;
			obj = (object)val;
		}
		new ConsoleCommand("echo", "Echo the text back to the console. This is intended for use with aliases and the autorun features. (Speelo's Toolbox)", (ConsoleEvent)obj, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj2 = _003C_003Ec._003C_003E9__53_1;
		if (obj2 == null)
		{
			ConsoleEvent val2 = delegate(ConsoleEventArgs args)
			{
				if (args.Length > 1)
				{
					if (args[1].Equals("1"))
					{
						((Component)((Terminal)Console.instance).m_chatWindow).gameObject.SetActive(true);
						SkConfigEntry.CConsoleEnabled.Value = true;
					}
					else
					{
						((Component)((Terminal)Console.instance).m_chatWindow).gameObject.SetActive(false);
					}
				}
				else
				{
					((Component)((Terminal)Console.instance).m_chatWindow).gameObject.SetActive(!((Component)((Terminal)Console.instance).m_chatWindow).gameObject.activeSelf);
					if (((Component)((Terminal)Console.instance).m_chatWindow).gameObject.activeInHierarchy)
					{
						SkConfigEntry.CConsoleEnabled.Value = true;
					}
				}
			};
			_003C_003Ec._003C_003E9__53_1 = val2;
			obj2 = (object)val2;
		}
		new ConsoleCommand("/console", "[1/0] - Toggle the console. No parameter with toggle. 1 = Open, 0 = Closed. Intended for use with hotkeys and aliases. (Speelo's Toolbox)", (ConsoleEvent)obj2, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj3 = _003C_003Ec._003C_003E9__53_2;
		if (obj3 == null)
		{
			ConsoleEvent val3 = delegate
			{
				PrintOut("Quitting game...", LogTo.DebugConsole);
				Application.Quit();
			};
			_003C_003Ec._003C_003E9__53_2 = val3;
			obj3 = (object)val3;
		}
		new ConsoleCommand("/q", "Quickly exit the game. Commands are sometimes just more convenient. (Speelo's Toolbox)", (ConsoleEvent)obj3, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj4 = _003C_003Ec._003C_003E9__53_3;
		if (obj4 == null)
		{
			ConsoleEvent val4 = delegate
			{
				((TMP_Text)((Terminal)Console.instance).m_output).text = string.Empty;
				try
				{
					Console.instance.SetPrivateField("m_chatBuffer", new List<string>());
					ConsoleOpt.consoleOutputHistory.Clear();
				}
				catch (Exception)
				{
				}
			};
			_003C_003Ec._003C_003E9__53_3 = val4;
			obj4 = (object)val4;
		}
		new ConsoleCommand("/clear", "Clear the current output shown in the console. (Speelo's Toolbox)", (ConsoleEvent)obj4, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj5 = _003C_003Ec._003C_003E9__53_4;
		if (obj5 == null)
		{
			ConsoleEvent val5 = delegate
			{
				List<ItemData> list2 = new List<ItemData>();
				((Humanoid)Player.m_localPlayer).GetInventory().GetWornItems(list2);
				foreach (ItemData item in list2)
				{
					try
					{
						item.m_durability = item.GetMaxDurability();
					}
					catch (Exception)
					{
					}
				}
				PrintOut("All items repaired!");
			};
			_003C_003Ec._003C_003E9__53_4 = val5;
			obj5 = (object)val5;
		}
		new ConsoleCommand("/repair", "Repair your inventory. (Speelo's Toolbox)", (ConsoleEvent)obj5, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj6 = _003C_003Ec._003C_003E9__53_5;
		if (obj6 == null)
		{
			ConsoleEvent val6 = delegate
			{
				PrintOut(ListPortals(), LogTo.Console);
			};
			_003C_003Ec._003C_003E9__53_5 = val6;
			obj6 = (object)val6;
		}
		new ConsoleCommand("/portals", "List all portal tags. (Speelo's Toolbox)", (ConsoleEvent)obj6, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj7 = _003C_003Ec._003C_003E9__53_6;
		if (obj7 == null)
		{
			ConsoleEvent val7 = delegate(ConsoleEventArgs args)
			{
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				GameObject prefab4 = ZNetScene.instance.GetPrefab("digg_v2");
				if ((Object)(object)prefab4 == (Object)null)
				{
					PrintOut("Terrain level failed. Report to mod author - terrain level error 1");
				}
				else
				{
					float radius4 = 5f;
					if (args.Length > 1)
					{
						try
						{
							radius4 = int.Parse(args[1]);
						}
						catch (Exception)
						{
						}
					}
					TerrainModification.ModifyTerrain(((Component)Player.m_localPlayer).transform.position, prefab4, radius4);
					PrintOut("Terrain levelled!");
				}
			};
			_003C_003Ec._003C_003E9__53_6 = val7;
			obj7 = (object)val7;
		}
		new ConsoleCommand("/tl", "[Radius=5] - Level nearby terrain. Radius 30 max. (Speelo's Toolbox)", (ConsoleEvent)obj7, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj8 = _003C_003Ec._003C_003E9__53_7;
		if (obj8 == null)
		{
			ConsoleEvent val8 = delegate(ConsoleEventArgs args)
			{
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				float radius3 = 5f;
				if (args.Length > 1)
				{
					try
					{
						radius3 = int.Parse(args[1]);
					}
					catch (Exception)
					{
					}
				}
				TerrainModification.ResetTerrain(((Component)Player.m_localPlayer).transform.position, radius3);
				PrintOut("Terrain reset!");
			};
			_003C_003Ec._003C_003E9__53_7 = val8;
			obj8 = (object)val8;
		}
		new ConsoleCommand("/tu", "[Radius=5] - Undo terrain modifications around you. Radius 50 max. (Speelo's Toolbox)", (ConsoleEvent)obj8, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj9 = _003C_003Ec._003C_003E9__53_8;
		if (obj9 == null)
		{
			ConsoleEvent val9 = delegate(ConsoleEventArgs args)
			{
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_0077: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				GameObject prefab3 = ZNetScene.instance.GetPrefab("raise");
				if ((Object)(object)prefab3 == (Object)null)
				{
					PrintOut("Terrain raise failed. Report to mod author - terrain raise error 1");
				}
				else
				{
					float radius2 = 5f;
					float num17 = 2f;
					if (args.Length > 1)
					{
						try
						{
							radius2 = int.Parse(args[1]);
						}
						catch (Exception)
						{
						}
					}
					if (args.Length > 2)
					{
						try
						{
							num17 = int.Parse(args[2]);
						}
						catch (Exception)
						{
						}
					}
					TerrainModification.ModifyTerrain(((Component)Player.m_localPlayer).transform.position + Vector3.up * num17, prefab3, radius2);
					PrintOut("Terrain raised!");
				}
			};
			_003C_003Ec._003C_003E9__53_8 = val9;
			obj9 = (object)val9;
		}
		new ConsoleCommand("/tr", "[Radius=5] [Height=1] - Raise nearby terrain. Radius 30 max. (Speelo's Toolbox)", (ConsoleEvent)obj9, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj10 = _003C_003Ec._003C_003E9__53_9;
		if (obj10 == null)
		{
			ConsoleEvent val10 = delegate(ConsoleEventArgs args)
			{
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_0077: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				GameObject prefab2 = ZNetScene.instance.GetPrefab("digg_v2");
				if ((Object)(object)prefab2 == (Object)null)
				{
					PrintOut("Terrain dig failed. Report to mod author - terrain dig error 1");
				}
				else
				{
					float radius = 5f;
					float num16 = 1f;
					if (args.Length > 1)
					{
						try
						{
							radius = int.Parse(args[1]);
						}
						catch (Exception)
						{
						}
					}
					if (args.Length > 2)
					{
						try
						{
							num16 = int.Parse(args[2]);
						}
						catch (Exception)
						{
						}
					}
					TerrainModification.ModifyTerrain(((Component)Player.m_localPlayer).transform.position - Vector3.up * num16, prefab2, radius);
					PrintOut("Terrain dug!");
				}
			};
			_003C_003Ec._003C_003E9__53_9 = val10;
			obj10 = (object)val10;
		}
		new ConsoleCommand("/td", "[Radius=5] [Height=1] - Dig nearby terrain. Radius 30 max. (Speelo's Toolbox)", (ConsoleEvent)obj10, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj11 = _003C_003Ec._003C_003E9__53_10;
		if (obj11 == null)
		{
			ConsoleEvent val11 = delegate
			{
				EnvMan.instance.ResetDebugWind();
				PrintOut("Wind unlocked and under game control.");
			};
			_003C_003Ec._003C_003E9__53_10 = val11;
			obj11 = (object)val11;
		}
		new ConsoleCommand("/resetwind", "If wind has been set, this will allow the game to take control of the wind again. (Speelo's Toolbox)", (ConsoleEvent)obj11, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj12 = _003C_003Ec._003C_003E9__53_11;
		if (obj12 == null)
		{
			ConsoleEvent val12 = delegate(ConsoleEventArgs args)
			{
				if (args.Length == 3)
				{
					float num14 = float.Parse(args[1]);
					float num15 = float.Parse(args[2]);
					EnvMan.instance.SetDebugWind(num14, num15);
				}
				else
				{
					PrintOut("Failed to set wind. Check parameters! Ex. /wind 240 5");
				}
			};
			_003C_003Ec._003C_003E9__53_11 = val12;
			obj12 = (object)val12;
		}
		new ConsoleCommand("/wind", "[Angle] [Intensity] - Set the wind direction and intensity. (Speelo's Toolbox)", (ConsoleEvent)obj12, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj13 = _003C_003Ec._003C_003E9__53_12;
		if (obj13 == null)
		{
			ConsoleEvent val13 = delegate(ConsoleEventArgs args)
			{
				if (args.Length == 1)
				{
					if (!((Object)(object)EnvMan.instance == (Object)null))
					{
						PrintOut("Current environment: " + (EnvMan.instance.GetCurrentEnvironment()?.m_name ?? "(none)"));
						{
							foreach (string item2 in from e in EnvMan.instance.m_environments
								select e.m_name into q
								orderby q
								select q)
							{
								PrintOut(item2);
							}
							return;
						}
					}
					PrintOut("Can't find environment manager.");
				}
				else if (args.Length >= 2)
				{
					if (args[1].Equals("-1"))
					{
						if ((Object)(object)EnvMan.instance != (Object)null)
						{
							EnvMan.instance.m_debugEnv = "";
							PrintOut("Weather unlocked and under game control.");
						}
					}
					else
					{
						string finalWeatherName = string.Empty;
						if (args.Length > 2)
						{
							string[] args2 = args.Args;
							foreach (string text7 in args2)
							{
								if (!text7.Equals(args[0]))
								{
									finalWeatherName = finalWeatherName + " " + text7;
								}
							}
							finalWeatherName = finalWeatherName.Trim();
						}
						else
						{
							finalWeatherName = args[1];
						}
						EnvSetup val58 = (((Object)(object)EnvMan.instance != (Object)null) ? EnvMan.instance.m_environments.FirstOrDefault((EnvSetup e) => string.Equals(e.m_name, finalWeatherName, StringComparison.OrdinalIgnoreCase)) : null);
						if (val58 != null)
						{
							EnvMan.instance.m_debugEnv = val58.m_name;
							PrintOut("Weather set to: " + EnvMan.instance.m_debugEnv);
						}
						else if ((Object)(object)EnvMan.instance == (Object)null)
						{
							PrintOut("Failed to set weather to '" + finalWeatherName + "'. Can't find environment manager.");
						}
						else
						{
							PrintOut("Failed to set weather to '" + finalWeatherName + "'. Unknown environment. Use /env to list, /env -1 to unlock.");
						}
					}
				}
				else
				{
					PrintOut("Failed to set weather. Check parameters! Ex. /env, /env -1, /env Misty");
				}
			};
			_003C_003Ec._003C_003E9__53_12 = val13;
			obj13 = (object)val13;
		}
		new ConsoleCommand("/env", "[Weather] - Change the weather. No parameter provided will list all weather. -1 will allow the game to control the weather again. (Speelo's Toolbox)", (ConsoleEvent)obj13, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj14 = _003C_003Ec._003C_003E9__53_13;
		if (obj14 == null)
		{
			ConsoleEvent val14 = delegate
			{
				Player localPlayer6 = Player.m_localPlayer;
				if ((Object)(object)localPlayer6 == (Object)null || (Object)(object)((Component)localPlayer6).GetComponent<ZNetView>() == (Object)null || !((Component)localPlayer6).GetComponent<ZNetView>().IsValid())
				{
					PrintOut("Fly: no local player yet. Spawn into a world first.");
				}
				else
				{
					flyEnabled = localPlayer6.ToggleDebugFly();
					Player.m_debugMode = flyEnabled || noCostEnabled || SkCommandPatcher.BCheat;
					PrintOut("Fly toggled! (" + flyEnabled + ")");
SkFlyDiag.Toggled();
				}
			};
			_003C_003Ec._003C_003E9__53_13 = val14;
			obj14 = (object)val14;
		}
		new ConsoleCommand("/fly", "Toggle flying. (Speelo's Toolbox)", (ConsoleEvent)obj14, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj15 = _003C_003Ec._003C_003E9__53_14;
		if (obj15 == null)
		{
			ConsoleEvent val15 = delegate(ConsoleEventArgs args)
			{
				if ((Object)(object)ObjectDB.instance == (Object)null || ObjectDB.instance.m_items == null)
				{
					PrintOut("Item database is not loaded yet.");
				}
				else
				{
					string text6 = ((args.Length > 1) ? args[1].ToLower() : string.Empty);
					int num13 = 0;
					foreach (GameObject item3 in ObjectDB.instance.m_items)
					{
						if (!((Object)(object)item3 == (Object)null))
						{
							ItemDrop component7 = item3.GetComponent<ItemDrop>();
							if (!((Object)(object)component7 == (Object)null) && component7.m_itemData != null && component7.m_itemData.m_shared != null)
							{
								Sprite[] icons = component7.m_itemData.m_shared.m_icons;
								if (icons != null && icons.Length != 0 && !((Object)(object)icons[0] == (Object)null) && (text6.Length <= 0 || ((Object)item3).name.ToLower().IndexOf(text6) >= 0))
								{
									PrintOut(((Object)item3).name);
									num13++;
								}
							}
						}
					}
					PrintOut(num13 + " item icon(s) listed.");
				}
			};
			_003C_003Ec._003C_003E9__53_14 = val15;
			obj15 = (object)val15;
		}
		new ConsoleCommand("/listicons", "[Filter] - List item prefabs that have an icon, for picking menu icons. (Speelo's Toolbox)", (ConsoleEvent)obj15, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj16 = _003C_003Ec._003C_003E9__53_15;
		if (obj16 == null)
		{
			ConsoleEvent val16 = delegate
			{
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				SkMenuController skMenuController = (((Object)(object)SkLoader.Load != (Object)null) ? SkLoader.Load.GetComponent<SkMenuController>() : null);
				object obj51;
				if (!((Object)(object)skMenuController != (Object)null))
				{
					obj51 = "F6";
				}
				else
				{
					KeyCode toggleKey = skMenuController.ToggleKey;
					obj51 = ((object)toggleKey).ToString();
				}
				PrintOut("The menu is mouse-driven now. Press " + (string)obj51 + " to open it; the /alt keyboard controls were removed.");
			};
			_003C_003Ec._003C_003E9__53_15 = val16;
			obj16 = (object)val16;
		}
		new ConsoleCommand("/alt", "(Removed) The menu is mouse-driven now; press the MenuToggleKey (default F6). (Speelo's Toolbox)", (ConsoleEvent)obj16, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj17 = _003C_003Ec._003C_003E9__53_16;
		if (obj17 == null)
		{
			ConsoleEvent val17 = delegate
			{
				if ((Object)(object)ZNet.instance == (Object)null || !ZNet.instance.IsServer())
				{
					PrintOut("Events can only be stopped by the host/server (the server re-syncs the active event every 2s).");
				}
				else
				{
					RandEventSystem.instance.ResetRandomEvent();
					PrintOut("Event stopped!");
				}
			};
			_003C_003Ec._003C_003E9__53_16 = val17;
			obj17 = (object)val17;
		}
		new ConsoleCommand("/stopevent", "Stops a current event. (Speelo's Toolbox)", (ConsoleEvent)obj17, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj18 = _003C_003Ec._003C_003E9__53_17;
		if (obj18 == null)
		{
			ConsoleEvent val18 = delegate
			{
				Minimap.instance.ExploreAll();
				PrintOut("Map revealed!");
			};
			_003C_003Ec._003C_003E9__53_17 = val18;
			obj18 = (object)val18;
		}
		new ConsoleCommand("/revealmap", "Reveals the entire minimap. (Speelo's Toolbox)", (ConsoleEvent)obj18, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj19 = _003C_003Ec._003C_003E9__53_18;
		if (obj19 == null)
		{
			ConsoleEvent val19 = delegate
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				string text5 = string.Empty;
				List<PlayerInfo> playerList = ZNet.instance.GetPlayerList();
				foreach (PlayerInfo item4 in playerList)
				{
					string[] obj50 = new string[6] { text5, ", ", item4.m_name, "(", null, null };
					ZDOID characterID = item4.m_characterID;
					obj50[4] = characterID.UserID.ToString();
					obj50[5] = ")";
					text5 = string.Concat(obj50);
				}
				if (text5.Length > 2)
				{
					text5 = text5.Remove(0, 2);
				}
				PrintOut("Active Players (" + playerList.Count + ") - " + text5);
			};
			_003C_003Ec._003C_003E9__53_18 = val19;
			obj19 = (object)val19;
		}
		new ConsoleCommand("/whois", "List all players. (Speelo's Toolbox)", (ConsoleEvent)obj19, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj20 = _003C_003Ec._003C_003E9__53_19;
		if (obj20 == null)
		{
			ConsoleEvent val20 = delegate(ConsoleEventArgs args)
			{
				//IL_0157: Unknown result type (might be due to invalid IL or missing references)
				//IL_0163: Unknown result type (might be due to invalid IL or missing references)
				//IL_016d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0172: Unknown result type (might be due to invalid IL or missing references)
				//IL_0177: Unknown result type (might be due to invalid IL or missing references)
				//IL_017c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0181: Unknown result type (might be due to invalid IL or missing references)
				//IL_019e: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a5: Expected O, but got Unknown
				string text4 = Player.m_localPlayer.GetPlayerName();
				string empty = string.Empty;
				int result2 = 1;
				int result3 = 1;
				if (args.Length < 2)
				{
					PrintOut("Failed. No item provided. /give [Item] [Qty=1] [Player] [Level=1]");
				}
				else
				{
					empty = args[1];
					if (args.Length >= 3 && !int.TryParse(args[2], out result2))
					{
						result2 = 1;
					}
					if (args.Length >= 4)
					{
						text4 = args[3];
					}
					if (args.Length >= 5 && !int.TryParse(args[4], out result3))
					{
						result3 = 1;
					}
					Player val55 = null;
					foreach (Player allPlayer in Player.GetAllPlayers())
					{
						if ((Object)(object)allPlayer != (Object)null && allPlayer.GetPlayerName().ToLower().StartsWith(text4.ToLower()))
						{
							val55 = allPlayer;
							break;
						}
					}
					if ((Object)(object)val55 == (Object)null)
					{
						PrintOut("Failed. Player does not exist. /give [Item] [Qty=1] [Player] [Level=1]");
					}
					else
					{
						GameObject prefab = ZNetScene.instance.GetPrefab(empty);
						if (Compat.Truthy((Object)(object)prefab))
						{
							PrintOut("Spawning " + result2 + " of item " + empty + "(" + result3 + ") on " + text4);
							try
							{
								GameObject val56 = Object.Instantiate<GameObject>(prefab, ((Component)val55).transform.position + ((Component)val55).transform.forward * 1.5f + Vector3.up, Quaternion.identity);
								ItemDrop val57 = (ItemDrop)val56.GetComponent(typeof(ItemDrop));
								if ((Object)(object)val57 != (Object)null && val57.m_itemData != null)
								{
									val57.SetQuality(Mathf.Clamp(result3, 1, SkToolbox.SkModules.ModGive.MaxLevel));
									val57.m_itemData.m_stack = result2;
									val57.m_itemData.m_durability = val57.m_itemData.GetMaxDurability();
								}
								return;
							}
							catch (Exception ex12)
							{
								PrintOut("Something unexpected failed.");
								SkUtilities.Logz(new string[2] { "ERR", "/give" }, new string[2] { ex12.Message, ex12.Source }, (LogType)2);
								return;
							}
						}
						PrintOut("Failed. Check parameters. /give [Item] [Qty=1] [Player] [Level=1]");
					}
				}
			};
			_003C_003Ec._003C_003E9__53_19 = val20;
			obj20 = (object)val20;
		}
		new ConsoleCommand("/give", "[Item] [Qty=1], OR /give [Item] [Qty=1] [Player] [Level=1] - Gives item to player. If player has a space in name, only provide name before the space. Capital letters matter in item / player name! (Speelo's Toolbox)", (ConsoleEvent)obj20, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj21 = _003C_003Ec._003C_003E9__53_20;
		if (obj21 == null)
		{
			ConsoleEvent val21 = delegate
			{
				if ((Object)(object)Player.m_localPlayer == (Object)null)
				{
					PrintOut("No local player.");
				}
				else
				{
					godEnabled = !Player.m_localPlayer.m_godMode;
					Player.m_localPlayer.SetGodMode(godEnabled);
					PrintOut("Godmode " + (godEnabled ? "ON" : "OFF") + ((godEnabled && (SkConfigEntry.CGodModeBlocksDamage == null || SkConfigEntry.CGodModeBlocksDamage.Value)) ? " - damage is blocked outright." : "."));
				}
			};
			_003C_003Ec._003C_003E9__53_20 = val21;
			obj21 = (object)val21;
		}
		new ConsoleCommand("/god", "Toggle Godmode. (Speelo's Toolbox)", (ConsoleEvent)obj21, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj22 = _003C_003Ec._003C_003E9__53_21;
		if (obj22 == null)
		{
			ConsoleEvent val22 = delegate
			{
				((Humanoid)Player.m_localPlayer).UnequipAllItems();
				((Humanoid)Player.m_localPlayer).GetInventory().RemoveAll();
				PrintOut("All items removed from inventory.");
			};
			_003C_003Ec._003C_003E9__53_21 = val22;
			obj22 = (object)val22;
		}
		new ConsoleCommand("/clearinventory", "Removes all items from your inventory. There is no confirmation, be careful. (Speelo's Toolbox)", (ConsoleEvent)obj22, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj23 = _003C_003Ec._003C_003E9__53_22;
		if (obj23 == null)
		{
			ConsoleEvent val23 = delegate
			{
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				TombStone[] array3 = Object.FindObjectsByType<TombStone>((FindObjectsSortMode)0);
				if (array3.Length != 0)
				{
					TombStone[] array4 = array3;
					foreach (TombStone val54 in array4)
					{
						if ((Object)(object)val54 != (Object)null && ((Behaviour)val54).enabled)
						{
							Minimap.instance.AddPin(((Component)val54).transform.position, (PinType)12, "TS", true, true, 0L, default(PlatformUserID));
						}
					}
				}
				PrintOut("Tombstone sought out! Potentially " + array3.Length + " found.");
			};
			_003C_003Ec._003C_003E9__53_22 = val23;
			obj23 = (object)val23;
		}
		new ConsoleCommand("/findtomb", "Pin nearby dead player tombstones on the map if any currently exist. (Speelo's Toolbox)", (ConsoleEvent)obj23, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj24 = _003C_003Ec._003C_003E9__53_23;
		if (obj24 == null)
		{
			ConsoleEvent val24 = delegate
			{
				World privateField = WorldGenerator.instance.GetPrivateField<World>("m_world");
				PrintOut("Map seed: " + privateField.m_seedName);
			};
			_003C_003Ec._003C_003E9__53_23 = val24;
			obj24 = (object)val24;
		}
		new ConsoleCommand("/seed", "Reveals the map seed. (Speelo's Toolbox)", (ConsoleEvent)obj24, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj25 = _003C_003Ec._003C_003E9__53_24;
		if (obj25 == null)
		{
			ConsoleEvent val25 = delegate
			{
				GameCamera.instance.ToggleFreeFly();
				PrintOut("Free cam toggled " + GameCamera.InFreeFly());
			};
			_003C_003Ec._003C_003E9__53_24 = val25;
			obj25 = (object)val25;
		}
		new ConsoleCommand("/freecam", "Toggle freecam. (Speelo's Toolbox)", (ConsoleEvent)obj25, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj26 = _003C_003Ec._003C_003E9__53_25;
		if (obj26 == null)
		{
			ConsoleEvent val26 = delegate(ConsoleEventArgs args)
			{
				if (args.Length > 1)
				{
					foreach (Player allPlayer2 in Player.GetAllPlayers())
					{
						if ((Object)(object)allPlayer2 != (Object)null && allPlayer2.GetPlayerName().ToLower().Equals(args[1].ToLower()))
						{
							((Character)allPlayer2).Heal(((Character)allPlayer2).GetMaxHealth(), true);
							PrintOut("Player healed: " + allPlayer2.GetPlayerName());
						}
					}
					return;
				}
				((Character)Player.m_localPlayer).Heal(((Character)Player.m_localPlayer).GetMaxHealth(), true);
				PrintOut("Self healed.");
			};
			_003C_003Ec._003C_003E9__53_25 = val26;
			obj26 = (object)val26;
		}
		new ConsoleCommand("/heal", "[Player=local] - Heal Player. (Speelo's Toolbox)", (ConsoleEvent)obj26, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj27 = _003C_003Ec._003C_003E9__53_26;
		if (obj27 == null)
		{
			ConsoleEvent val27 = delegate
			{
				SkCommandPatcher.InitPatch();
				SkCommandPatcher.bBuildAnywhere = !SkCommandPatcher.bBuildAnywhere;
				PrintOut("No build restrictions toggled! (" + SkCommandPatcher.bBuildAnywhere + ")");
			};
			_003C_003Ec._003C_003E9__53_26 = val27;
			obj27 = (object)val27;
		}
		new ConsoleCommand("/nores", "Toggle no restrictions to where you can build (except ward zones). (Speelo's Toolbox)", (ConsoleEvent)obj27, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj28 = _003C_003Ec._003C_003E9__53_27;
		if (obj28 == null)
		{
			ConsoleEvent val28 = delegate
			{
				Player localPlayer5 = Player.m_localPlayer;
				if ((Object)(object)localPlayer5 == (Object)null)
				{
					PrintOut("No local player.");
				}
				else
				{
					noCostEnabled = !localPlayer5.NoCostCheat();
					localPlayer5.SetNoPlacementCost(noCostEnabled);
					Player.m_debugMode = flyEnabled || noCostEnabled || SkCommandPatcher.BCheat;
					PrintOut("No build cost/requirements toggled! (" + noCostEnabled + ")");
				}
			};
			_003C_003Ec._003C_003E9__53_27 = val28;
			obj28 = (object)val28;
		}
		new ConsoleCommand("/nocost", "Toggle no requirement building. (Speelo's Toolbox)", (ConsoleEvent)obj28, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj29 = _003C_003Ec._003C_003E9__53_28;
		if (obj29 == null)
		{
			ConsoleEvent val29 = delegate(ConsoleEventArgs args)
			{
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
				if (args.Length != 2 || !args[1].Contains(","))
				{
					PrintOut("Syntax /tp X,Z");
					return;
				}
				try
				{
					string[] array2 = args[1].Split(',');
					float num9 = float.Parse(array2[0]);
					float num10 = float.Parse(array2[1]);
					float groundHeight = ZoneSystem.instance.GetGroundHeight(new Vector3(num9, 750f, num10));
					bool flag = groundHeight < 749f;
					Player localPlayer4 = Player.m_localPlayer;
					if (Compat.Truthy((Object)(object)localPlayer4))
					{
						float num11;
						if (flag)
						{
							num11 = Mathf.Max(groundHeight + 0.5f, ZoneSystem.instance.m_waterLevel);
						}
						else
						{
							float num12 = (localPlayer4.m_debugFly ? 400f : 30f);
							num11 = Mathf.Clamp(((Component)localPlayer4).transform.position.y, 30f, num12);
						}
						Vector3 val53 = default(Vector3);
						val53 = new Vector3(num9, num11, num10);
						if (((Character)localPlayer4).TeleportTo(val53, ((Component)localPlayer4).transform.rotation, true))
						{
							PrintOut("Teleporting...");
						}
						else
						{
							PrintOut("Teleport refused (already teleporting or <2s since last teleport). Try again in a moment.");
						}
					}
				}
				catch (Exception)
				{
					PrintOut("Syntax /tp X,Z");
				}
			};
			_003C_003Ec._003C_003E9__53_28 = val29;
			obj29 = (object)val29;
		}
		new ConsoleCommand("/tp", "[X,Y] - Teleport you to the coords provided. (Speelo's Toolbox)", (ConsoleEvent)obj29, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj30 = _003C_003Ec._003C_003E9__53_29;
		if (obj30 == null)
		{
			ConsoleEvent val30 = delegate(ConsoleEventArgs args)
			{
				bDetectEnemies = !bDetectEnemies;
				if (args.Length > 0)
				{
					try
					{
						bDetectRange = int.Parse(args[1]);
						bDetectRange = ((bDetectRange < 5) ? 5 : bDetectRange);
					}
					catch (Exception)
					{
						bDetectRange = 20;
					}
				}
				PrintOut("Detect enemies toggled! (" + bDetectEnemies + ", range: " + bDetectRange + ")");
			};
			_003C_003Ec._003C_003E9__53_29 = val30;
			obj30 = (object)val30;
		}
		new ConsoleCommand("/detect", "[Range=20] - Toggle enemy detection. (Speelo's Toolbox)", (ConsoleEvent)obj30, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj31 = _003C_003Ec._003C_003E9__53_30;
		if (obj31 == null)
		{
			ConsoleEvent val31 = delegate(ConsoleEventArgs args)
			{
				SkCommandPatcher.InitPatch();
				SkCommandPatcher.BCheat = !SkCommandPatcher.BCheat;
				try
				{
					Player.m_debugMode = SkCommandPatcher.BCheat;
					Terminal.m_cheat = SkCommandPatcher.BCheat;
					if (SkCommandPatcher.BCheat && (Object)(object)Game.instance != (Object)null)
					{
						PlayerProfile playerProfile = Game.instance.GetPlayerProfile();
						if (playerProfile != null && !playerProfile.m_usedCheats)
						{
							playerProfile.m_usedCheats = true;
							PrintOut(Localization.instance.Localize("$achievements_permanently_cheated_character"));
						}
					}
				}
				catch (Exception)
				{
				}
				if ((Object)(object)args.Context != (Object)null)
				{
					args.Context.InvokePrivateMethod("updateCommandList", null);
				}
				if ((Object)(object)Console.instance != (Object)null && Console.instance != args.Context)
				{
					Console.instance.InvokePrivateMethod("updateCommandList", null);
				}
				PrintOut("Cheats toggled! (" + SkCommandPatcher.BCheat + ")");
				if (SkCommandPatcher.BCheat)
				{
					PrintOut("Vanilla cheat commands (god/fly/ghost/nocost/debugmode...) are now valid on this client, including on joined servers.");
				}
			};
			_003C_003Ec._003C_003E9__53_30 = val31;
			obj31 = (object)val31;
		}
		new ConsoleCommand("/imacheater", "Use the toolbox to force enable standard cheats on any server. (Speelo's Toolbox)", (ConsoleEvent)obj31, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj32 = _003C_003Ec._003C_003E9__53_31;
		if (obj32 == null)
		{
			ConsoleEvent val32 = delegate
			{
				SkCommandPatcher.InitPatch();
				SkCommandPatcher.BFreeSupport = !SkCommandPatcher.BFreeSupport;
				PrintOut("No build support requirements toggled! (" + SkCommandPatcher.BFreeSupport + ")");
			};
			_003C_003Ec._003C_003E9__53_31 = val32;
			obj32 = (object)val32;
		}
		new ConsoleCommand("/nosup", "Toggle no supports required for buildings - WARNING! - IF YOU REJOIN AND THIS IS DISABLED, YOUR STRUCTURES MAY FALL APART - USE WITH CARE. Maybe use the AutoRun functionality? (Speelo's Toolbox)", (ConsoleEvent)obj32, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj33 = _003C_003Ec._003C_003E9__53_32;
		if (obj33 == null)
		{
			ConsoleEvent val33 = delegate
			{
				bCoords = !bCoords;
				PrintOut("Show coords toggled! (" + bCoords + ")");
			};
			_003C_003Ec._003C_003E9__53_32 = val33;
			obj33 = (object)val33;
		}
		new ConsoleCommand("/coords", "Show coords in corner of the screen. (Speelo's Toolbox)", (ConsoleEvent)obj33, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj34 = _003C_003Ec._003C_003E9__53_33;
		if (obj34 == null)
		{
			ConsoleEvent val34 = delegate
			{
				Minimap.instance.Reset();
			};
			_003C_003Ec._003C_003E9__53_33 = val34;
			obj34 = (object)val34;
		}
		new ConsoleCommand("/resetmap", "Reset the map exploration. (Speelo's Toolbox)", (ConsoleEvent)obj34, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj35 = _003C_003Ec._003C_003E9__53_34;
		if (obj35 == null)
		{
			ConsoleEvent val35 = delegate
			{
				Player localPlayer3 = Player.m_localPlayer;
				if ((Object)(object)localPlayer3 == (Object)null)
				{
					PrintOut("Infinite stamina: no local player yet. Spawn into a world first.");
				}
				else
				{
					infStamina = !infStamina;
					ApplyInfStamina(localPlayer3, applyOffValues: true);
					PrintOut("Infinite stamina toggled! (" + infStamina + ")");
				}
			};
			_003C_003Ec._003C_003E9__53_34 = val35;
			obj35 = (object)val35;
		}
		new ConsoleCommand("/infstam", "Toggles infinite stamina. (Speelo's Toolbox)", (ConsoleEvent)obj35, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj36 = _003C_003Ec._003C_003E9__53_35;
		if (obj36 == null)
		{
			ConsoleEvent val36 = delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				Tameable.TameAllInArea(((Component)Player.m_localPlayer).transform.position, 20f);
				PrintOut("Creatures tamed!");
			};
			_003C_003Ec._003C_003E9__53_35 = val36;
			obj36 = (object)val36;
		}
		new ConsoleCommand("/tame", "Tame all nearby creatures. (Speelo's Toolbox)", (ConsoleEvent)obj36, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj37 = _003C_003Ec._003C_003E9__53_36;
		if (obj37 == null)
		{
			ConsoleEvent val37 = delegate(ConsoleEventArgs args)
			{
				Player localPlayer2 = Player.m_localPlayer;
				if ((Object)(object)localPlayer2 == (Object)null)
				{
					PrintOut("Far interactions: no local player yet. Spawn into a world first.");
				}
				else
				{
					farInteract = !farInteract;
					if (farInteract && args.Length > 1)
					{
						try
						{
							farInteractDistance = ((int.Parse(args[1]) < 20) ? 20 : int.Parse(args[1]));
						}
						catch (Exception)
						{
							PrintOut("Failed to set far interaction distance. Check params. /farinteract 50");
						}
					}
					ApplyFarInteract(localPlayer2, applyOffValues: true);
					PrintOut(farInteract ? ("Far interactions toggled! (True Distance: " + farInteractDistance + ")") : "Far interactions toggled! (False)");
				}
			};
			_003C_003Ec._003C_003E9__53_36 = val37;
			obj37 = (object)val37;
		}
		new ConsoleCommand("/farinteract", "[Distance=50] - Toggles far interactions (building as well). To change distance, toggle this off then back on with new distance. (Speelo's Toolbox)", (ConsoleEvent)obj37, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj38 = _003C_003Ec._003C_003E9__53_37;
		if (obj38 == null)
		{
			ConsoleEvent val38 = delegate
			{
				Player localPlayer = Player.m_localPlayer;
				if ((Object)(object)localPlayer == (Object)null)
				{
					PrintOut("Ghost: no local player yet. Spawn into a world first.");
				}
				else
				{
					localPlayer.SetGhostMode(!localPlayer.m_ghostMode);
					ghostEnabled = localPlayer.m_ghostMode;
					PrintOut("Ghost mode toggled! (" + ghostEnabled + ")");
				}
			};
			_003C_003Ec._003C_003E9__53_37 = val38;
			obj38 = (object)val38;
		}
		new ConsoleCommand("/ghost", "Toggle Ghostmode (enemy creatures cannot see you). (Speelo's Toolbox)", (ConsoleEvent)obj38, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj39 = _003C_003Ec._003C_003E9__53_38;
		if (obj39 == null)
		{
			ConsoleEvent val39 = delegate(ConsoleEventArgs args)
			{
				if (args.Length > 1)
				{
					if (float.TryParse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
					{
						if (result < 0f)
						{
							EnvMan.instance.m_debugTimeOfDay = false;
							PrintOut("Time unlocked and under game control.");
						}
						else
						{
							EnvMan.instance.m_debugTimeOfDay = true;
							EnvMan.instance.m_debugTime = Mathf.Clamp01(result);
							PrintOut("Setting time of day:" + result);
						}
					}
				}
				else
				{
					PrintOut("Failed. Syntax /tod [0-1] Ex. /tod 0.5");
				}
			};
			_003C_003Ec._003C_003E9__53_38 = val39;
			obj39 = (object)val39;
		}
		new ConsoleCommand("/tod", "[0-1] - Set (and lock) time of day (-1 to unlock time). Ex. /tod 0.5 (Speelo's Toolbox)", (ConsoleEvent)obj39, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj40 = _003C_003Ec._003C_003E9__53_39;
		if (obj40 == null)
		{
			ConsoleEvent val40 = delegate
			{
				TerrainComp.UpgradeTerrain();
				Heightmap.UpdateTerrainAlpha();
			};
			_003C_003Ec._003C_003E9__53_39 = val40;
			obj40 = (object)val40;
		}
		new ConsoleCommand("/optterrain", "Optimize old terrain modifications. (Speelo's Toolbox)", (ConsoleEvent)obj40, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj41 = _003C_003Ec._003C_003E9__53_40;
		if (obj41 == null)
		{
			ConsoleEvent val41 = delegate(ConsoleEventArgs args)
			{
				if (args.Length > 1)
				{
					if (args[1].Equals("cw"))
					{
						try
						{
							int num2 = int.Parse(args[2]);
							Player.m_localPlayer.m_maxCarryWeight = num2;
							PrintOut("New carry weight set to: " + num2);
							return;
						}
						catch (Exception)
						{
							PrintOut("Failed to set new carry weight. Check params.");
							return;
						}
					}
					if (args[1].Equals("skill"))
					{
						if (args.Length == 4 && !args[2].Contains("None") && !args[2].Contains("All") && !args[2].Contains("FireMagic") && !args[2].Contains("FrostMagic"))
						{
							try
							{
								string text2 = args[2];
								int num3 = int.Parse(args[3]);
								((Character)Player.m_localPlayer).GetSkills().CheatResetSkill(text2.ToLower());
								((Character)Player.m_localPlayer).GetSkills().CheatRaiseSkill(text2.ToLower(), (float)num3, true);
								return;
							}
							catch (Exception)
							{
								PrintOut("Failed to set skill. Check params / skill name. See /listskills. /set skill [skill] [level]");
								return;
							}
						}
						PrintOut("Failed to set skill. Check params / skill name. See /listskills.  /set skill [skill] [level]");
					}
					else if (args[1].Equals("pickup"))
					{
						if (args.Length >= 3)
						{
							try
							{
								int num4 = int.Parse(args[2]);
								Player.m_localPlayer.m_autoPickupRange = num4;
								PrintOut("New range set to: " + num4);
								return;
							}
							catch (Exception)
							{
								PrintOut("Failed to set pickup range. Check params. /set pickup 2");
								return;
							}
						}
						PrintOut("Failed to set pickup range. Check params.  /set pickup 2");
					}
					else if (args[1].Equals("jumpforce"))
					{
						if (args.Length >= 3)
						{
							try
							{
								int num5 = int.Parse(args[2]);
								((Character)Player.m_localPlayer).m_jumpForce = num5;
								PrintOut("New range set to: " + num5);
								return;
							}
							catch (Exception)
							{
								PrintOut("Failed to set jump force. Check params. /set jumpforce 10");
								return;
							}
						}
						PrintOut("Failed to set jump force. Check params.  /set jumpforce 10");
					}
					else if (args[1].Equals("exploreradius"))
					{
						if (args.Length >= 3)
						{
							try
							{
								int num6 = int.Parse(args[2]);
								Minimap.instance.m_exploreRadius = num6;
								PrintOut("New range set to: " + num6);
								return;
							}
							catch (Exception)
							{
								PrintOut("Failed to set explore radius. Check params. /set exploreradius 100");
								return;
							}
						}
						PrintOut("Failed to set explore radius. Check params.  /set exploreradius 100");
					}
					else if (args[1].Equals("speed"))
					{
						if (args.Length >= 4)
						{
							string text3 = args[2];
							string[] source = new string[3] { "crouch", "run", "swim" };
							if (source.Contains(text3))
							{
								float num7 = 0f;
								try
								{
									int num8 = int.Parse(args[3]);
									switch (text3)
									{
									case "crouch":
										num7 = ((Character)Player.m_localPlayer).m_crouchSpeed;
										((Character)Player.m_localPlayer).m_crouchSpeed = num8;
										break;
									case "run":
										num7 = ((Character)Player.m_localPlayer).m_runSpeed;
										((Character)Player.m_localPlayer).m_runSpeed = num8;
										break;
									case "swim":
										num7 = ((Character)Player.m_localPlayer).m_swimSpeed;
										((Character)Player.m_localPlayer).m_swimSpeed = num8;
										break;
									}
									PrintOut("New " + text3 + " speed set to: " + num8 + " (was: " + num7 + ")");
									return;
								}
								catch (Exception)
								{
									PrintOut("Failed to set speed. Check params name. Ex. /set speed crouch 2");
									return;
								}
							}
							PrintOut("Failed to set speed. Check params name. Ex.  /set speed crouch 2");
						}
						else
						{
							PrintOut("Failed to set speed. Check params name. Ex.  /set speed crouch 2");
						}
					}
					else if (args[1].Equals("difficulty"))
					{
						if (args.Length >= 3)
						{
							try
							{
								int forcePlayerDifficulty = int.Parse(args[2]);
								Game.instance.SetForcePlayerDifficulty(forcePlayerDifficulty);
								PrintOut("Difficulty set to " + forcePlayerDifficulty);
								return;
							}
							catch (Exception)
							{
								PrintOut("Failed to set difficulty. Check params. /set difficulty 5");
								return;
							}
						}
						PrintOut("Failed to set difficulty. Check params.  /set difficulty 5");
					}
				}
			};
			_003C_003Ec._003C_003E9__53_40 = val41;
			obj41 = (object)val41;
		}
		new ConsoleCommand("/set", "[Option] [value] [value]. Option can be one of [cw,difficulty,exploreradius,jumpforce,pickup,skill,speed]. All options take 1 value except [skill,speed]. (Speelo's Toolbox)", (ConsoleEvent)obj41, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj42 = _003C_003Ec._003C_003E9__53_41;
		if (obj42 == null)
		{
			ConsoleEvent val42 = delegate
			{
				ItemDrop[] array = Object.FindObjectsByType<ItemDrop>((FindObjectsSortMode)0);
				int num = 0;
				for (int i = 0; i < array.Length; i++)
				{
					Fish component5 = ((Component)array[i]).GetComponent<Fish>();
					if ((!Compat.Truthy((Object)(object)component5) || component5.IsOutOfWater()) && !array[i].IsPiece())
					{
						ZNetView component6 = ((Component)array[i]).GetComponent<ZNetView>();
						if (Compat.Truthy((Object)(object)component6) && component6.IsValid())
						{
							if (!component6.IsOwner())
							{
								component6.ClaimOwnership();
							}
							component6.Destroy();
							num++;
						}
					}
				}
				PrintOut("Items cleared (" + num + ").");
			};
			_003C_003Ec._003C_003E9__53_41 = val42;
			obj42 = (object)val42;
		}
		new ConsoleCommand("/removedrops", "Removes items from the ground. (Speelo's Toolbox)", (ConsoleEvent)obj42, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj43 = _003C_003Ec._003C_003E9__53_42;
		if (obj43 == null)
		{
			ConsoleEvent val43 = delegate(ConsoleEventArgs args)
			{
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				GameObject val52 = SpawnPrefab(args);
				if (!((Object)(object)val52 == (Object)null))
				{
					MonsterAI component3 = val52.GetComponent<MonsterAI>();
					if ((Object)(object)component3 == (Object)null)
					{
						PrintOut("Spawned, but " + args[1] + " has no MonsterAI and cannot be tamed.");
					}
					else
					{
						component3.MakeTame();
						Tameable component4 = val52.GetComponent<Tameable>();
						if ((Object)(object)component4 != (Object)null)
						{
							component4.m_tamedEffect.Create(val52.transform.position, val52.transform.rotation, (Transform)null, 1f, -1, default(ZDOID));
						}
						PrintOut("Spawned tamed - " + args[1]);
					}
				}
			};
			_003C_003Ec._003C_003E9__53_42 = val43;
			obj43 = (object)val43;
		}
		new ConsoleCommand("/spawntamed", "[Creature Name] [Level=1] - Spawns a tamed creature in front of you. Capitals in name matter! Ex. /spawntamed Boar 3. (Speelo's Toolbox)", (ConsoleEvent)obj43, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj44 = _003C_003Ec._003C_003E9__53_43;
		if (obj44 == null)
		{
			ConsoleEvent val44 = delegate(ConsoleEventArgs args)
			{
				GameObject val51 = SpawnPrefab(args);
				if ((Object)(object)val51 != (Object)null)
				{
					PrintOut("Spawned - " + args[1]);
				}
			};
			_003C_003Ec._003C_003E9__53_43 = val44;
			obj44 = (object)val44;
		}
		new ConsoleCommand("/spawn", "[Creature Name] [Level=1] - Spawns a creature or prefab in front of you. Capitals in name matter! Ex. /spawn Boar 3 (use /give for items!) (Speelo's Toolbox)", (ConsoleEvent)obj44, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj45 = _003C_003Ec._003C_003E9__53_44;
		if (obj45 == null)
		{
			ConsoleEvent val45 = delegate
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Expected O, but got Unknown
				List<Character> list = new List<Character>();
				Character.GetCharactersInRange(((Component)Player.m_localPlayer).transform.position, 50f, list);
				foreach (Character item5 in list)
				{
					if (!item5.IsPlayer())
					{
						HitData val50 = new HitData();
						val50.m_damage.m_damage = 1E+10f;
						item5.Damage(val50);
					}
				}
				PrintOut("Nearby creatures killed! (50m)");
			};
			_003C_003Ec._003C_003E9__53_44 = val45;
			obj45 = (object)val45;
		}
		new ConsoleCommand("/killall", "Kills all nearby creatures. (Speelo's Toolbox)", (ConsoleEvent)obj45, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj46 = _003C_003Ec._003C_003E9__53_45;
		if (obj46 == null)
		{
			ConsoleEvent val46 = delegate(ConsoleEventArgs args)
			{
				if (args.Length > 1)
				{
					foreach (GameObject item6 in ObjectDB.instance.m_items)
					{
						ItemDrop component = item6.GetComponent<ItemDrop>();
						if (((Object)component).name.ToLower().Contains(args[1].ToLower()))
						{
							PrintOut("Item: '" + ((Object)component).name + "'", LogTo.DebugConsole);
						}
					}
					return;
				}
				foreach (GameObject item7 in ObjectDB.instance.m_items)
				{
					ItemDrop component2 = item7.GetComponent<ItemDrop>();
					PrintOut("Item: '" + ((Object)component2).name + "'", LogTo.DebugConsole);
				}
			};
			_003C_003Ec._003C_003E9__53_45 = val46;
			obj46 = (object)val46;
		}
		new ConsoleCommand("/listitems", "[Name Contains] - List all items. Optionally include name starts with. Ex. /listitems Woo returns any item that contains the letters 'Woo'. (Speelo's Toolbox)", (ConsoleEvent)obj46, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj47 = _003C_003Ec._003C_003E9__53_46;
		if (obj47 == null)
		{
			ConsoleEvent val47 = delegate(ConsoleEventArgs args)
			{
				ConsoleOpt.BuildPrefabs();
				if (args.Length > 1)
				{
					foreach (string prefab5 in ConsoleOpt.PrefabList)
					{
						if (prefab5.ToLower().StartsWith(args[1].ToLower()))
						{
							PrintOut("Prefab: '" + prefab5 + "'", LogTo.DebugConsole);
						}
					}
					return;
				}
				foreach (string prefab6 in ConsoleOpt.PrefabList)
				{
					PrintOut("Prefab: '" + prefab6 + "'", LogTo.DebugConsole);
				}
			};
			_003C_003Ec._003C_003E9__53_46 = val47;
			obj47 = (object)val47;
		}
		new ConsoleCommand("/listprefabs", "[Name Contains] - Lists all prefabs - List all prefabs / creatures. Optionally include name starts with.Ex. / listprefabs Troll returns any prefab that starts with the letters 'Troll'. (Speelo's Toolbox)", (ConsoleEvent)obj47, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj48 = _003C_003Ec._003C_003E9__53_47;
		if (obj48 == null)
		{
			ConsoleEvent val48 = delegate
			{
				string text = "Skills found: ";
				foreach (object value in Enum.GetValues(typeof(SkillType)))
				{
					if (!value.ToString().Contains("None") && !value.ToString().Contains("All") && !value.ToString().Contains("FireMagic") && !value.ToString().Contains("FrostMagic"))
					{
						text = text + value.ToString() + ", ";
					}
				}
				text = text.Remove(text.Length - 2);
				PrintOut(text, LogTo.DebugConsole);
			};
			_003C_003Ec._003C_003E9__53_47 = val48;
			obj48 = (object)val48;
		}
		new ConsoleCommand("/listskills", "Lists all skills. (Speelo's Toolbox)", (ConsoleEvent)obj48, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		object obj49 = _003C_003Ec._003C_003E9__53_48;
		if (obj49 == null)
		{
			ConsoleEvent val49 = delegate
			{
				((Terminal)Console.instance).TryRunCommand("help", false, false);
			};
			_003C_003Ec._003C_003E9__53_48 = val49;
			obj49 = (object)val49;
		}
		new ConsoleCommand("/?", "(Speelo's Toolbox)", (ConsoleEvent)obj49, false, false, false, false, true, false, (ConsoleOptionsFetcher)null, false, false, false);
	}

	public static bool ProcessCommand(string inCommand, LogTo source, GameObject go = null)
	{
		((Terminal)Console.instance).TryRunCommand(inCommand, false, false);
		return true;
	}

	public static bool ProcessCommand(string inCommand, LogTo source, GameObject go, bool TEMP)
	{
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(inCommand) || string.IsNullOrWhiteSpace(inCommand))
		{
			return true;
		}
		inCommand = inCommand.Trim();
		string[] array = inCommand.Split(' ');
		if (inCommand.StartsWith("help") && source.HasFlag(LogTo.Console))
		{
			Console.instance.Print("devcommands - Enable standard developer/cheat commands");
			Console.instance.Print("/? [Page] - Speelo's Toolbox Commands - Pages 1 through " + (Mathf.Ceil((float)(commandList.Count / pageSize)) + (float)((commandList.Count % pageSize != 0) ? 1 : 0)) + " - Example /? 1");
			Console instance = Console.instance;
			object obj;
			if (!((Object)(object)SkLoader.Load != (Object)null) || !((Object)(object)SkLoader.Load.GetComponent<SkMenuController>() != (Object)null))
			{
				obj = "F6";
			}
			else
			{
				KeyCode toggleKey = SkLoader.Load.GetComponent<SkMenuController>().ToggleKey;
				obj = ((object)toggleKey).ToString();
			}
			instance.Print("Speelo's Toolbox - Close the console and press " + (string)obj + " to open the menu.");
			return false;
		}
		if (array[0].Equals("/?"))
		{
			int result = 1;
			if (array.Length > 1 && int.TryParse(array[1], out result))
			{
				if ((float)result > Mathf.Ceil((float)(commandList.Count / pageSize)) + (float)((commandList.Count % pageSize != 0) ? 1 : 0))
				{
					result = Mathf.RoundToInt(Mathf.Ceil((float)(commandList.Count / pageSize)) + (float)((commandList.Count % pageSize != 0) ? 1 : 0));
				}
				List<string> list = new List<string>(commandList.Keys);
				List<string> list2 = new List<string>(commandList.Values);
				PrintOut("Command List Page " + result + " / " + (Mathf.Ceil((float)(commandList.Count / pageSize)) + (float)((commandList.Count % pageSize != 0) ? 1 : 0)));
				for (int i = pageSize * result - pageSize; i < ((commandList.Count > pageSize * (result + 1) - pageSize) ? (pageSize * (result + 1) - pageSize) : commandList.Count); i++)
				{
					PrintOut(list[i] + " " + list2[i]);
				}
			}
			else
			{
				PrintOut("Type /? # to see the help for that page number. Ex. /? 1", source);
			}
			return true;
		}
		return false;
	}

	private static GameObject SpawnPrefab(ConsoleEventArgs args)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		if (args.Length <= 1)
		{
			PrintOut("Usage: [Creature Name] [Level=1]");
			return null;
		}
		if ((Object)(object)ZNetScene.instance == (Object)null || (Object)(object)Player.m_localPlayer == (Object)null)
		{
			PrintOut("Not in a world.");
			return null;
		}
		GameObject prefab = ZNetScene.instance.GetPrefab(args[1]);
		if ((Object)(object)prefab == (Object)null)
		{
			PrintOut("Creature not found.");
			return null;
		}
		Transform transform = ((Component)Player.m_localPlayer).transform;
		GameObject val = Object.Instantiate<GameObject>(prefab, transform.position + transform.forward * 1.5f, Quaternion.identity);
		if (args.Length > 2)
		{
			try
			{
				Character component = val.GetComponent<Character>();
				if ((Object)(object)component != (Object)null)
				{
					int num = int.Parse(args[2]);
					if (num > 10)
					{
						num = 10;
					}
					component.SetLevel(num);
				}
				else
				{
					ItemDrop component2 = val.GetComponent<ItemDrop>();
					if ((Object)(object)component2 != (Object)null && component2.m_itemData != null)
					{
						component2.m_itemData.m_quality = 1;
						component2.m_itemData.m_stack = int.Parse(args[2]);
						component2.m_itemData.m_durability = component2.m_itemData.GetMaxDurability();
					}
				}
			}
			catch (Exception)
			{
			}
		}
		return val;
	}

	public static void PrintOut(string text)
	{
		PrintOut(text, LogTo.Console);
	}

	internal static void Notify(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		try
		{
			int num = text.IndexOf('\n');
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
			if (text.Length > 160)
			{
				text = text.Substring(0, 160);
			}
			if ((Object)(object)Player.m_localPlayer != (Object)null)
			{
				((Character)Player.m_localPlayer).Message((MessageType)1, text, 0, (Sprite)null, false);
				MenuFeedbackShown = true;
			}
			else if ((Object)(object)MessageHud.instance != (Object)null)
			{
				MessageHud.instance.ShowMessage((MessageType)1, text, 0, (Sprite)null, false, true);
				MenuFeedbackShown = true;
			}
		}
		catch (Exception)
		{
		}
	}

	public static void PrintOut(string text, LogTo source, bool playerSay = false)
	{
		if (text.Equals(string.Empty) || text.Equals(" "))
		{
			return;
		}
		if ((source.HasFlag(LogTo.Console) || (SkConfigEntry.CAllowChatOutput != null && !SkConfigEntry.CAllowChatOutput.Value)) && (Object)(object)Console.instance != (Object)null)
		{
			Console.instance.Print("(Speelo's Toolbox) " + text);
		}
		if (source.HasFlag(LogTo.Chat) && (Object)(object)Chat.instance != (Object)null && SkConfigEntry.CAllowChatOutput != null && SkConfigEntry.CAllowChatOutput.Value)
		{
			if (playerSay && (Object)(object)Player.m_localPlayer != (Object)null && SkConfigEntry.CAllowPublicChatOutput != null && SkConfigEntry.CAllowPublicChatOutput.Value)
			{
				((Component)Player.m_localPlayer).GetComponent<Talker>().Say((Talker.Type)1, text);
			}
			else
			{
				ChatPrint(text);
			}
		}
		if (source.HasFlag(LogTo.DebugConsole))
		{
			SkUtilities.Logz(new string[1] { "TOOLBOX" }, new string[1] { text }, (LogType)3);
		}
		if (MenuFeedbackActive)
		{
			Notify(text);
		}
	}

	public static void ChatPrint(string ln, string source = "(Speelo's Toolbox) ")
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)Chat.instance != (Object)null))
		{
			return;
		}
		Chat.instance.OnNewChatMessage((GameObject)null, 999L, chatPos, (Talker.Type)1, UserInfo.GetLocalUser(), source + ln);
		Chat.instance.SetPrivateField("m_hideTimer", 0f);
		((Component)((Terminal)Chat.instance).m_chatWindow).gameObject.SetActive(true);
		((Component)((Terminal)Chat.instance).m_input).gameObject.SetActive(true);
		List<WorldTextInstance> privateField = Chat.instance.GetPrivateField<List<WorldTextInstance>>("m_worldTexts");
		foreach (WorldTextInstance item in privateField)
		{
			if (item.m_talkerID == 999)
			{
				item.m_timer = 999f;
			}
		}
	}

	public static string ListPortals(bool printToChat = false)
	{
		List<ZDO> portalList = ZDOMan.instance.GetPortalList();
		portalList.RemoveAll((ZDO z) => !z.IsValid());
		string text = string.Empty;
		foreach (ZDO item in portalList)
		{
			string @string = item.GetString(ZDOVars.s_tag, "");
			if (!@string.Equals(string.Empty) && !@string.Equals(" "))
			{
				text = text + "'" + @string + "', ";
			}
		}
		if (!text.Equals(string.Empty))
		{
			text = text.Substring(0, text.Length - 2);
			return "Portals found: " + text;
		}
		return "No portals found.";
	}
}
