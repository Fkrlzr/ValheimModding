using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ServerDevcommands;
using Service;
using UnityEngine;

namespace InfinityHammer;

public class ToolSelection : BaseSelection
{
	public Tool Tool;

	private readonly List<Vector3> Points = new List<Vector3>();

	private PathRuler? Path;

	private const float RepeatDistance = 0.05f;

	private const int MaxPoints = 64;

	public override float SnapMultiplier => 2f;

	public override bool IsTool => true;

	public override bool Continuous => Tool.Continuous;

	public override bool PlayerHeight => Tool.PlayerHeight;

	public override bool TerrainGrid => Tool.TerrainGrid;

	public ToolSelection(Tool tool)
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		Tool = tool;
		ScalingData scalingData = Scaling.Get(tool: true);
		if (tool.InitialHeight.HasValue)
		{
			scalingData.SetScaleY(tool.InitialHeight.Value);
		}
		if (tool.InitialSize.HasValue)
		{
			scalingData.SetScaleX(tool.InitialSize.Value);
			scalingData.SetScaleZ(tool.InitialSize.Value);
		}
		if (tool.InitialShape != "")
		{
			Ruler.Shape = (Enum.TryParse<RulerShape>(tool.InitialShape, ignoreCase: true, out var result) ? result : RulerShape.Circle);
		}
		SelectedPrefab = new GameObject();
		Piece obj = SelectedPrefab.AddComponent<Piece>();
		obj.m_name = tool.Name;
		obj.m_icon = tool.Icon;
		obj.m_description = tool.Description;
		obj.m_clipEverything = true;
		if (tool.SnapPiece)
		{
			Snapping.CreateSnapPoint(SelectedPrefab, Vector3.zero, "Snap");
		}
		Ruler.Create(tool);
	}

	public override float MaxPlaceDistance(float value)
	{
		return 1000f;
	}

	public override bool IsScalingSupported()
	{
		return true;
	}

	public override void AfterPlace(GameObject obj)
	{
		HandleCommand(obj);
	}

	private void HandleCommand(GameObject obj)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		PlacedCommand placedCommand = obj.AddComponent<PlacedCommand>();
		Transform transform = HammerHelper.GetPlacementGhost().transform;
		if (Tool.UsesPoints && !CapturePoint(transform.position))
		{
			return;
		}
		string newValue = transform.position.x.ToString(CultureInfo.InvariantCulture);
		string newValue2 = transform.position.y.ToString(CultureInfo.InvariantCulture);
		string newValue3 = transform.position.z.ToString(CultureInfo.InvariantCulture);
		ScalingData scalingData = Scaling.Get();
		string text = scalingData.X.ToString(CultureInfo.InvariantCulture);
		string text2 = Mathf.Min(scalingData.X, scalingData.Z).ToString(CultureInfo.InvariantCulture);
		string text3 = Mathf.Max(scalingData.X, scalingData.Z).ToString(CultureInfo.InvariantCulture);
		string newValue4 = scalingData.Z.ToString(CultureInfo.InvariantCulture);
		string text4 = scalingData.X.ToString(CultureInfo.InvariantCulture);
		RulerShape shape = Ruler.Shape;
		if (shape == RulerShape.Circle)
		{
			text2 = text;
			text3 = text;
		}
		if (shape != RulerShape.Rectangle)
		{
			newValue4 = text4;
		}
		if (shape == RulerShape.Square)
		{
			text2 = text;
			text3 = text;
		}
		if (shape == RulerShape.Rectangle)
		{
			text2 = text4;
			text3 = text4;
		}
		string newValue5 = scalingData.Y.ToString(CultureInfo.InvariantCulture);
		Quaternion rotation = transform.rotation;
		string newValue6 = ((Quaternion)(ref rotation)).eulerAngles.y.ToString(CultureInfo.InvariantCulture);
		if (TerrainGrid)
		{
			newValue6 = "0";
		}
		string text5 = Tool.GetCommand();
		if (text5.Contains("<r>") && (text5.Contains("<w>") || text5.Contains("<d>")))
		{
			text5 = RemoveUnusedShapeParameters(text5, shape);
		}
		if (Tool.UsesPoints)
		{
			Vector3[] points = Points.ToArray();
			Points.Clear();
			if (Tool.Variadic)
			{
				text5 = ExpandRepeated(text5, points.Length);
			}
			text5 = global::InfinityHammer.Tool.PointRegex.Replace(text5, delegate(Match match)
			{
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				int num = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture) - 1;
				if (num < 0 || num >= points.Length)
				{
					return match.Value;
				}
				Vector3 val = points[num];
				return ((match.Groups[1].Value == "x") ? val.x : ((match.Groups[1].Value == "y") ? val.y : val.z)).ToString(CultureInfo.InvariantCulture);
			});
		}
		if (text5.Contains("<id>"))
		{
			ZNetView hovered = Selector.GetHovered(Configuration.Range, Array.Empty<string>(), Configuration.IgnoredIds);
			if ((Object)(object)hovered == (Object)null)
			{
				Helper.AddError((Terminal)(object)Console.instance, "Nothing is being hovered.", true);
				return;
			}
			text5 = text5.Replace("<id>", Utils.GetPrefabName(((Component)hovered).gameObject));
		}
		text5 = ((shape != RulerShape.Frame) ? text5.Replace("<d>", newValue4) : text5.Replace("<d>", text2 + "-" + text3));
		text5 = text5.Replace("<r>", text);
		text5 = text5.Replace("<r2>", text3);
		text5 = text5.Replace("<w>", text4);
		text5 = text5.Replace("<w2>", text3);
		text5 = text5.Replace("<a>", newValue6);
		text5 = text5.Replace("<x>", newValue);
		text5 = text5.Replace("<y>", newValue2);
		text5 = text5.Replace("<z>", newValue3);
		text5 = text5.Replace("<h>", newValue5);
		text5 = text5.Replace("<ignore>", Configuration.configToolIgnoredIds.Value);
		text5 = text5.Replace("<include>", Configuration.configToolIncludedIds.Value);
		if (!Configuration.DisableMessages)
		{
			((Terminal)Console.instance).AddString("Hammering command: " + text5);
		}
		placedCommand.Command = text5;
	}

	private string ExpandRepeated(string command, int pointCount)
	{
		List<string> list = new List<string>();
		string[] array = command.Split(' ');
		foreach (string text in array)
		{
			if (!text.Contains("<p*>"))
			{
				list.Add(text);
				continue;
			}
			for (int j = Tool.PointCount + 1; j <= pointCount; j++)
			{
				list.Add(text.Replace("<p*>", $"<x{j}>,<z{j}>,<y{j}>"));
			}
		}
		return string.Join(" ", list);
	}

	private bool CapturePoint(Vector3 position)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		int num = Tool.PointCount + (Tool.Variadic ? 1 : 0);
		bool flag = Tool.Variadic && Points.Count > 0 && Utils.DistanceXZ(Points[Points.Count - 1], position) < 0.05f;
		if (!flag)
		{
			Points.Add(position);
		}
		bool flag2 = ((!Tool.Variadic) ? (Points.Count >= num) : (flag || Tool.Finish || Points.Count >= 64));
		if (flag2 && Points.Count < num)
		{
			Helper.AddError((Terminal)(object)Console.instance, $"At least {num} points are needed.", true);
			return false;
		}
		if (!flag2 && !Configuration.DisableMessages)
		{
			((Terminal)Console.instance).AddString($"Hammering point {Points.Count}" + (Tool.Variadic ? "" : $"/{num}"));
		}
		return flag2;
	}

	private string RemoveUnusedShapeParameters(string command, RulerShape shape)
	{
		bool flag = shape == RulerShape.Circle || shape == RulerShape.Ring;
		string[] array = MultiCommands.Split(command);
		for (int i = 0; i < array.Length; i++)
		{
			List<string> list = array[i].Split(' ').ToList();
			for (int num = list.Count - 1; num > -1; num--)
			{
				if (flag && (list[num].Contains("<w>") || list[num].Contains("<d>")))
				{
					list.RemoveAt(num);
				}
				if (!flag && list[num].Contains("<r>"))
				{
					list.RemoveAt(num);
				}
			}
			array[i] = string.Join(" ", list);
		}
		return string.Join("; ", array);
	}

	public override void Activate()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		base.Activate();
		BindCommand.SetMode("command");
		Ruler.Create(Tool);
		if (Tool.UsesPoints && (Object)(object)Path == (Object)null)
		{
			Path = new GameObject("InfinityHammerPath").AddComponent<PathRuler>();
			Path.Visible = false;
		}
	}

	public string DescriptionPoints()
	{
		if (Tool.UsesPoints)
		{
			if (!Tool.Variadic)
			{
				return $"point: {Points.Count + 1}/{Tool.PointCount}";
			}
			return $"point: {Points.Count + 1}";
		}
		return "";
	}

	public void UpdatePath(Player player)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)Path == (Object)null))
		{
			GameObject placementGhost = player.m_placementGhost;
			bool flag = Object.op_Implicit((Object)(object)placementGhost) && placementGhost.activeInHierarchy && ((Character)player).InPlaceMode() && !Hud.IsPieceSelectionVisible();
			Path.Visible = flag && Points.Count > 0;
			if (flag)
			{
				Path.Points = Points;
				Path.Cursor = placementGhost.transform.position;
				Path.Radius = ((Tool.Width || Tool.Radius) ? Scaling.Get().X : 0.5f);
				BaseRuler.SnapToGround = true;
				BaseRuler.Offset = 0f;
				Path.Refresh();
			}
		}
	}

	public override void Deactivate()
	{
		base.Deactivate();
		Ruler.Remove();
		BindCommand.SetMode("");
		Points.Clear();
		if ((Object)(object)Path != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)Path).gameObject);
		}
		Path = null;
	}
}
