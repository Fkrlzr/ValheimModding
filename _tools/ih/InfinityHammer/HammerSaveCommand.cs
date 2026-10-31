using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class HammerSaveCommand : TextReceiver
{
	private HammerSavePars? Pars;

	private ConsoleEventArgs? Args;

	private static string GetExtraInfo(GameObject obj, DataEntry data)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string result = "";
		string text = default(string);
		if (DataEntryExtensions.TryGetString(data, dictionary, ZDOVars.s_text, ref text))
		{
			result = text;
		}
		string text2 = default(string);
		if (DataEntryExtensions.TryGetString(data, dictionary, ZDOVars.s_tamedName, ref text2))
		{
			result = text2;
		}
		string text3 = default(string);
		if (DataEntryExtensions.TryGetString(data, dictionary, ZDOVars.s_tag, ref text3))
		{
			result = text3;
		}
		if (Object.op_Implicit((Object)(object)obj.GetComponent<ItemStand>()) && HammerHelper.TryGetName(data, ZDOVars.s_item, out string name))
		{
			int num = default(int);
			int num2 = (DataEntryExtensions.TryGetInt(data, dictionary, ZDOVars.s_variant, ref num) ? num : 0);
			int num3 = default(int);
			int num4 = ((!DataEntryExtensions.TryGetInt(data, dictionary, ZDOVars.s_quality, ref num3)) ? 1 : num3);
			int num5 = default(int);
			int num6 = (DataEntryExtensions.TryGetInt(data, dictionary, ZDOVars.s_type, ref num5) ? num5 : 0);
			result = ((num4 != 1 || num6 != 0) ? $"{name}:{num2}:{num4}:{num6}" : ((num2 == 0) ? (name ?? "") : $"{name}:{num2}"));
		}
		ArmorStand val = default(ArmorStand);
		if (obj.TryGetComponent<ArmorStand>(ref val))
		{
			result = $"{val.m_pose}:";
			result += $"{val.m_slots.Count}:";
			IEnumerable<string> values = val.m_slots.Select((ArmorStandSlot slot) => $"{HammerHelper.ConvertHashToName(slot.m_visualHash)}:{slot.m_visualVariant}");
			result += string.Join(":", values);
		}
		return result;
	}

	private static Blueprint BuildBluePrint(Player player, GameObject obj, string centerPiece, string snapPiece, bool saveData)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		Blueprint obj2 = new Blueprint
		{
			Name = Utils.GetPrefabName(obj),
			Creator = player.GetPlayerName()
		};
		Quaternion rotation = HammerHelper.GetPlacementGhost().transform.rotation;
		obj2.Rotation = ((Quaternion)(ref rotation)).eulerAngles;
		Blueprint blueprint = obj2;
		Piece component = obj.GetComponent<Piece>();
		if (Object.op_Implicit((Object)(object)component))
		{
			blueprint.Name = Localization.instance.Localize(component.m_name);
		}
		if (!(Selection.Get() is ObjectSelection objectSelection))
		{
			return blueprint;
		}
		if (objectSelection.TerrainHeightInfo != null)
		{
			blueprint.TerrainHeight = objectSelection.TerrainHeightInfo.Clone();
		}
		if (objectSelection.TerrainPaintInfo != null)
		{
			blueprint.TerrainPaint = objectSelection.TerrainPaintInfo.Clone();
		}
		List<GameObject> children = Snapping.GetChildren(obj);
		Dictionary<string, string> pars = new Dictionary<string, string>();
		if (!objectSelection.UsesSelectionRoot)
		{
			AddSingleObject(blueprint, pars, obj, saveData);
			foreach (GameObject snapPoint in Snapping.GetSnapPoints(obj))
			{
				blueprint.SnapPoints.Add(snapPoint.transform.localPosition);
			}
		}
		else
		{
			int num = 0;
			foreach (GameObject item in children)
			{
				if (snapPiece != "" && Utils.GetPrefabName(item) == snapPiece)
				{
					blueprint.SnapPoints.Add(item.transform.localPosition);
				}
				else
				{
					AddObject(blueprint, pars, item, saveData, num);
				}
				num++;
			}
			if (snapPiece == "")
			{
				foreach (GameObject snapPoint2 in Snapping.GetSnapPoints(obj))
				{
					blueprint.SnapPoints.Add(snapPoint2.transform.localPosition);
				}
			}
		}
		Vector3 val = blueprint.Center(centerPiece);
		blueprint.Coordinates = player.m_placementGhost.transform.position - val;
		return blueprint;
	}

	private static void AddSingleObject(Blueprint bp, Dictionary<string, string> pars, GameObject obj, bool saveData)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		string prefabName = Utils.GetPrefabName(obj);
		DataEntry val = ((saveData || Configuration.SavedObjectData.Contains(prefabName.ToLowerInvariant())) ? Selection.Get().GetData() : null);
		string info = ((val == null) ? "" : GetExtraInfo(obj, val));
		bp.Objects.Add(new BlueprintObject(prefabName, Vector3.zero, Quaternion.identity, obj.transform.localScale, info, ((val != null) ? DataEntryExtensions.GetBase64(val, pars) : null) ?? "", 1f));
	}

	private static void AddObject(Blueprint bp, Dictionary<string, string> pars, GameObject obj, bool saveData, int index = 0)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		string prefabName = Utils.GetPrefabName(obj);
		DataEntry val = ((saveData || Configuration.SavedObjectData.Contains(prefabName.ToLowerInvariant())) ? Selection.Get().GetData(index) : null);
		string info = ((val == null) ? "" : GetExtraInfo(obj, val));
		bp.Objects.Add(new BlueprintObject(prefabName, obj.transform.localPosition, obj.transform.localRotation, obj.transform.localScale, info, ((val != null) ? DataEntryExtensions.GetBase64(val, pars) : null) ?? "", 1f));
	}

	private static string[] GetPlanBuildFile(Blueprint bp)
	{
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		if (Configuration.SimplerBlueprints)
		{
			List<string> list = new List<string>();
			list.Add("#Center:" + bp.CenterPiece);
			list.Add("#Pieces");
			list.AddRange(bp.Objects.OrderBy((BlueprintObject o) => o.Prefab).Select(GetPlanBuildObject));
			return list.ToArray();
		}
		List<string> list2 = new List<string>
		{
			"#Name:" + bp.Name,
			"#Creator:" + bp.Creator,
			"#Description:" + bp.Description,
			"#Category:InfinityHammer",
			"#Center:" + bp.CenterPiece,
			"#Coordinates:" + HammerHelper.PrintXZY(bp.Coordinates),
			"#Rotation:" + HammerHelper.PrintYXZ(bp.Rotation),
			"#SnapPoints"
		};
		list2.AddRange(bp.SnapPoints.Select(GetPlanBuildSnapPoint));
		list2.Add("#Pieces");
		list2.AddRange(bp.Objects.OrderBy((BlueprintObject o) => o.Prefab).Select(GetPlanBuildObject));
		if (bp.TerrainHeight != null)
		{
			list2.Add("#TerrainHeight:" + HammerHelper.PrintXZY(bp.TerrainHeight.CenterPosition) + ";" + HammerHelper.Format(((Quaternion)(ref bp.TerrainHeight.CenterRotation)).eulerAngles.y) + ";" + HammerHelper.Format(bp.TerrainHeight.DistanceBetweenNodes));
			for (int i = 0; i < bp.TerrainHeight.Height; i++)
			{
				List<string> list3 = new List<string>();
				for (int j = 0; j < bp.TerrainHeight.Width; j++)
				{
					float? num = bp.TerrainHeight.Get(j, i);
					list3.Add(num.HasValue ? HammerHelper.Format(num.Value) : "");
				}
				list2.Add(string.Join(";", list3));
			}
		}
		if (bp.TerrainPaint != null)
		{
			list2.Add("#TerrainPaint:" + HammerHelper.PrintXZY(bp.TerrainPaint.CenterPosition) + ";" + HammerHelper.Format(((Quaternion)(ref bp.TerrainPaint.CenterRotation)).eulerAngles.y) + ";" + HammerHelper.Format(bp.TerrainPaint.DistanceBetweenNodes));
			for (int k = 0; k < bp.TerrainPaint.Height; k++)
			{
				List<string> list4 = new List<string>();
				for (int l = 0; l < bp.TerrainPaint.Width; l++)
				{
					Color? val = bp.TerrainPaint.Get(l, k);
					if (val.HasValue)
					{
						Color value = val.Value;
						list4.Add(HammerHelper.Format(value.r) + ":" + HammerHelper.Format(value.g) + ":" + HammerHelper.Format(value.b) + ":" + HammerHelper.Format(value.a));
					}
					else
					{
						list4.Add("");
					}
				}
				list2.Add(string.Join(";", list4));
			}
		}
		return list2.ToArray();
	}

	private static string GetPlanBuildSnapPoint(Vector3 pos)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		string text = HammerHelper.Format(pos.x);
		string text2 = HammerHelper.Format(pos.y);
		string text3 = HammerHelper.Format(pos.z);
		return text + ";" + text2 + ";" + text3;
	}

	private static string GetPlanBuildObject(BlueprintObject obj)
	{
		string prefab = obj.Prefab;
		string text = HammerHelper.Format(obj.Pos.x);
		string text2 = HammerHelper.Format(obj.Pos.y);
		string text3 = HammerHelper.Format(obj.Pos.z);
		string text4 = HammerHelper.Format(obj.Rot.x);
		string text5 = HammerHelper.Format(obj.Rot.y);
		string text6 = HammerHelper.Format(obj.Rot.z);
		string text7 = HammerHelper.Format(obj.Rot.w);
		string text8 = HammerHelper.Format(obj.Scale.x);
		string text9 = HammerHelper.Format(obj.Scale.y);
		string text10 = HammerHelper.Format(obj.Scale.z);
		string extraInfo = obj.ExtraInfo;
		string data = obj.Data;
		return prefab + ";;" + text + ";" + text2 + ";" + text3 + ";" + text4 + ";" + text5 + ";" + text6 + ";" + text7 + ";" + extraInfo + ";" + text8 + ";" + text9 + ";" + text10 + ";" + data;
	}

	public HammerSaveCommand()
	{
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		AutoComplete.Register("hammer_save", (Func<int, List<string>>)((int index) => (index == 0) ? HammerBlueprintCommand.GetBlueprints() : new List<string>(8) { "c", "center", "d", "data", "p", "profile", "s", "snap" }), new Dictionary<string, Func<int, List<string>>>
		{
			{
				"c",
				(int index) => ParameterInfo.ObjectIds
			},
			{
				"center",
				(int index) => ParameterInfo.ObjectIds
			},
			{
				"d",
				(int index) => new List<string>(2) { "true", "false" }
			},
			{
				"data",
				(int index) => new List<string>(2) { "true", "false" }
			},
			{
				"p",
				(int index) => new List<string>(2) { "true", "false" }
			},
			{
				"profile",
				(int index) => new List<string>(2) { "true", "false" }
			},
			{
				"s",
				(int index) => ParameterInfo.ObjectIds
			},
			{
				"snap",
				(int index) => ParameterInfo.ObjectIds
			}
		});
		Helper.Command("hammer_save", "[file name] [center=piece] [snap=piece] [data=true/false] [profile=true/false] - Saves the selection to a blueprint.", (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			Args = args;
			Pars = new HammerSavePars(args);
			if (Pars.Name != "")
			{
				SaveBlueprint(args, Pars);
			}
			else
			{
				TextInput.instance.RequestText((TextReceiver)(object)this, "Name", 1000);
			}
		});
	}

	private void SaveBlueprint(ConsoleEventArgs args, HammerSavePars pars)
	{
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		if (pars.Name != "")
		{
			Player player = Helper.GetPlayer();
			GameObject placementGhost = HammerHelper.GetPlacementGhost();
			Blueprint blueprint = BuildBluePrint(player, placementGhost, pars.CenterPiece, pars.SnapPiece, pars.SaveData);
			string[] planBuildFile = GetPlanBuildFile(blueprint);
			string path = Path.GetFileNameWithoutExtension(pars.Name) + ".blueprint";
			string text = Path.Combine(pars.Profile ? Configuration.BlueprintLocalFolder : Configuration.BlueprintGlobalFolder, path);
			Directory.CreateDirectory(Path.GetDirectoryName(text));
			File.WriteAllLines(text, planBuildFile);
			args.Context.AddString("Blueprint saved to " + text.Replace("\\", "\\\\") + " (pos: " + HammerHelper.PrintXZY(blueprint.Coordinates) + " rot: " + HammerHelper.PrintYXZ(blueprint.Rotation) + ").");
			Blueprint planBuild = HammerBlueprintCommand.GetPlanBuild(new Blueprint
			{
				Name = blueprint.Name
			}, planBuildFile, loadData: true);
			Selection.CreateGhost(new ObjectSelection(args.Context, planBuild, Vector3.one));
		}
		Pars = null;
		Args = null;
	}

	public string GetText()
	{
		return "";
	}

	public void SetText(string text)
	{
		if (Pars != null && Args != null)
		{
			Pars.Name = text;
			SaveBlueprint(Args, Pars);
			Pars = null;
			Args = null;
		}
	}
}
