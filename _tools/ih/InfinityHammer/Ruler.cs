using System.Globalization;
using ServerDevcommands;
using Service;
using UnityEngine;

namespace InfinityHammer;

public class Ruler
{
	public static RulerShape Shape;

	public static GameObject? Projector;

	public static GameObject? Circle;

	public static GameObject? Ring;

	public static GameObject? Rectangle;

	public static GameObject? Square;

	public static GameObject? Frame;

	public static bool IsActive => (Object)(object)Projector != (Object)null;

	public static void SanityCheckShape()
	{
		if (Shape == RulerShape.Circle && (!Object.op_Implicit((Object)(object)Circle) || !Configuration.ShapeCircle))
		{
			Shape = RulerShape.Ring;
		}
		if (Shape == RulerShape.Ring && (!Object.op_Implicit((Object)(object)Ring) || !Configuration.ShapeRing))
		{
			Shape = RulerShape.Square;
		}
		if (Shape == RulerShape.Square && (!Object.op_Implicit((Object)(object)Square) || !Configuration.ShapeSquare))
		{
			Shape = RulerShape.Frame;
		}
		if (Shape == RulerShape.Frame && (!Object.op_Implicit((Object)(object)Frame) || !Configuration.ShapeFrame))
		{
			Shape = RulerShape.Rectangle;
		}
		if (Shape == RulerShape.Rectangle && (!Object.op_Implicit((Object)(object)Rectangle) || !Configuration.ShapeRectangle))
		{
			Shape = RulerShape.Circle;
		}
	}

	private static void SetTerrainGrid(float width, float height)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		float num = width - Mathf.Floor(width);
		float num2 = height - Mathf.Floor(height);
		bool flag = 0.25f < num && num < 0.75f;
		bool flag2 = 0.25f < num2 && num2 < 0.75f;
		Grid.SetPreciseMode(new Vector3(flag ? 0f : 0.5f, 0f, flag2 ? 0f : 0.5f));
	}

	private static void BuildCircle(GameObject obj, float radius)
	{
		CircleRuler component = obj.GetComponent<CircleRuler>();
		component.Visible = obj.activeSelf;
		component.Radius = radius;
		component.Refresh();
	}

	private static void BuildRectangle(GameObject obj, float width, float depth)
	{
		RectangleRuler component = obj.GetComponent<RectangleRuler>();
		component.Visible = obj.activeSelf;
		component.Width = width;
		component.Depth = depth;
		component.Refresh();
	}

	public static void Update()
	{
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Player.m_localPlayer;
		if (Object.op_Implicit((Object)(object)localPlayer) && Selection.Get() is ToolSelection toolSelection)
		{
			toolSelection.UpdatePath(localPlayer);
		}
		if ((Object)(object)Projector == (Object)null || !Object.op_Implicit((Object)(object)localPlayer))
		{
			return;
		}
		if (!(Selection.Get() is ToolSelection toolSelection2))
		{
			Projector.SetActive(false);
			return;
		}
		GameObject placementGhost = localPlayer.m_placementGhost;
		Transform transform = ((Component)localPlayer).transform;
		bool flag = Object.op_Implicit((Object)(object)placementGhost) && placementGhost.activeInHierarchy && ((Character)localPlayer).InPlaceMode() && !Hud.IsPieceSelectionVisible();
		Projector.SetActive(flag);
		if (!flag)
		{
			return;
		}
		Transform transform2 = placementGhost.transform;
		ScalingData scalingData = Scaling.Get();
		Tool tool = toolSelection2.Tool;
		if (tool.IsTargetEdge)
		{
			Vector3 position = default(Vector3);
			((Vector3)(ref position))._002Ector((transform.position.x + transform2.position.x) / 2f, transform2.position.y, (transform.position.z + transform2.position.z) / 2f);
			Projector.transform.position = position;
		}
		else
		{
			Projector.transform.position = transform2.position;
		}
		Quaternion rotation = transform2.rotation;
		float num = ((Quaternion)(ref rotation)).eulerAngles.y;
		if (tool.RotateWithPlayer)
		{
			num = Vector3.SignedAngle(Vector3.forward, Utils.DirectionXZ(transform2.position - transform.position), Vector3.up);
		}
		if (toolSelection2.TerrainGrid)
		{
			num = 0f;
		}
		Projector.transform.rotation = Quaternion.Euler(0f, num, 0f);
		if (tool.IsTargetEdge)
		{
			float scaleZ = Utils.DistanceXZ(transform.position, transform2.position) / 2f;
			scalingData.SetScaleZ(scaleZ);
		}
		BaseRuler.SnapToGround = toolSelection2.Tool.SnapGround;
		BaseRuler.Offset = (toolSelection2.Tool.Height ? scalingData.Y : 0f);
		SanityCheckShape();
		RulerShape shape = Shape;
		if (toolSelection2.TerrainGrid)
		{
			scalingData.SetPrecisionXZ(0.5f, 0.5f);
		}
		if ((Object)(object)Circle != (Object)null)
		{
			Circle.SetActive(shape == RulerShape.Circle || shape == RulerShape.Ring);
			BuildCircle(Circle, scalingData.X);
			if (Circle.activeSelf && toolSelection2.TerrainGrid)
			{
				SetTerrainGrid(scalingData.X, scalingData.X);
			}
		}
		if ((Object)(object)Circle != (Object)null && (Object)(object)Ring != (Object)null)
		{
			Ring.SetActive(shape == RulerShape.Ring);
			BuildCircle(Ring, scalingData.Z);
		}
		if ((Object)(object)Square != (Object)null)
		{
			Square.SetActive(shape == RulerShape.Square || shape == RulerShape.Frame);
			BuildRectangle(Square, scalingData.X, scalingData.X);
			if (Square.activeSelf && toolSelection2.TerrainGrid)
			{
				SetTerrainGrid(scalingData.X, scalingData.X);
			}
		}
		if ((Object)(object)Square != (Object)null && (Object)(object)Frame != (Object)null)
		{
			Frame.SetActive(shape == RulerShape.Frame);
			BuildRectangle(Frame, scalingData.Z, scalingData.Z);
			if (Frame.activeSelf && toolSelection2.TerrainGrid)
			{
				SetTerrainGrid(scalingData.Z, scalingData.Z);
			}
		}
		if ((Object)(object)Rectangle != (Object)null)
		{
			Rectangle.SetActive(shape == RulerShape.Rectangle);
			BuildRectangle(Rectangle, scalingData.X, scalingData.Z);
			if (Rectangle.activeSelf && toolSelection2.TerrainGrid)
			{
				SetTerrainGrid(scalingData.X, scalingData.Z);
			}
		}
		if (tool.Highlight)
		{
			if (shape == RulerShape.Circle)
			{
				HighlightCircle(Projector.transform.position, scalingData.X, scalingData.Y);
			}
			if (shape == RulerShape.Ring)
			{
				HighlightRing(Projector.transform.position, scalingData.X, scalingData.Z, scalingData.Y);
			}
			if (shape == RulerShape.Square)
			{
				HighlightRectangle(Projector.transform.position, num, scalingData.X, scalingData.X, scalingData.Y);
			}
			if (shape == RulerShape.Frame)
			{
				HighlightFrame(Projector.transform.position, num, scalingData.X, scalingData.Z, scalingData.Y);
			}
			if (shape == RulerShape.Rectangle)
			{
				HighlightRectangle(Projector.transform.position, num, scalingData.X, scalingData.Z, scalingData.Y);
			}
		}
	}

	public static string DescriptionScale(ToolSelection selection)
	{
		if ((Object)(object)Projector == (Object)null)
		{
			return "";
		}
		Tool tool = selection.Tool;
		ScalingData scalingData = Scaling.Get();
		string text = (tool.Height ? (", h: " + Format2(scalingData.Y)) : "");
		switch (Shape)
		{
		case RulerShape.Rectangle:
			return "w: " + Format2(scalingData.X) + ", d: " + Format2(scalingData.Z) + text;
		case RulerShape.Square:
			return "w: " + Format2(scalingData.X) + text;
		case RulerShape.Circle:
			return "r: " + Format2(scalingData.X) + text;
		case RulerShape.Ring:
		{
			float f3 = Mathf.Min(scalingData.X, scalingData.Z);
			float f4 = Mathf.Max(scalingData.X, scalingData.Z);
			return "r: " + Format2(f3) + "-" + Format2(f4) + text;
		}
		case RulerShape.Frame:
		{
			float f = Mathf.Min(scalingData.X, scalingData.Z);
			float f2 = Mathf.Max(scalingData.X, scalingData.Z);
			return "w: " + Format2(f) + "-" + Format2(f2) + text;
		}
		default:
			return "";
		}
	}

	public static string DescriptionPosition()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)Projector == (Object)null)
		{
			return "";
		}
		Vector3 position = Projector.transform.position;
		return "x: " + Format1(position.x) + ", z: " + Format1(position.z) + ", y: " + Format1(position.y);
	}

	private static string Format1(float f)
	{
		return f.ToString("F1", CultureInfo.InvariantCulture);
	}

	private static string Format2(float f)
	{
		return f.ToString("F2", CultureInfo.InvariantCulture);
	}

	private static GameObject InitializeGameObject()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		Projector = new GameObject
		{
			layer = LayerMask.NameToLayer("character_trigger")
		};
		return Projector;
	}

	private static GameObject CreateCircle(GameObject obj)
	{
		GameObject obj2 = CreateChild(obj);
		obj2.AddComponent<CircleRuler>();
		return obj2;
	}

	private static GameObject CreateRectangle(GameObject obj)
	{
		GameObject obj2 = CreateChild(obj);
		obj2.AddComponent<RectangleRuler>();
		return obj2;
	}

	private static GameObject CreateChild(GameObject obj)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		GameObject val = new GameObject();
		val.transform.SetParent(obj.transform);
		val.transform.localPosition = Vector3.zero;
		val.transform.localRotation = Quaternion.identity;
		return val;
	}

	public static void Constrain(Range<float?> size, Range<float?> height)
	{
		ScalingData scalingData = Scaling.Get();
		if (size.Min.HasValue && size.Max.HasValue)
		{
			scalingData.SetScaleX(Mathf.Clamp(scalingData.X, size.Min.Value, size.Max.Value));
			scalingData.SetScaleZ(Mathf.Clamp(scalingData.Z, size.Min.Value, size.Max.Value));
		}
		if (height.Min.HasValue && height.Max.HasValue)
		{
			scalingData.SetScaleY(Mathf.Clamp(scalingData.Y, height.Min.Value, height.Max.Value));
		}
	}

	public static void InitializeProjector(Tool tool, GameObject obj)
	{
		if (tool.Width)
		{
			Square = CreateRectangle(obj);
		}
		if (tool.Frame)
		{
			Frame = CreateRectangle(obj);
		}
		if (tool.Width && tool.Depth)
		{
			Rectangle = CreateRectangle(obj);
		}
		if (tool.Radius)
		{
			Circle = CreateCircle(obj);
		}
		if (tool.Ring)
		{
			Ring = CreateCircle(obj);
		}
	}

	public static void Create(Tool tool)
	{
		Remove();
		if (tool.Radius || tool.Width || tool.Depth)
		{
			InitializeProjector(tool, InitializeGameObject());
		}
	}

	public static void Remove()
	{
		if ((Object)(object)Projector != (Object)null)
		{
			Projector.SetActive(false);
			Object.Destroy((Object)(object)Projector);
		}
		Projector = null;
		if ((Object)(object)Circle != (Object)null)
		{
			Object.Destroy((Object)(object)Circle);
		}
		Circle = null;
		if ((Object)(object)Ring != (Object)null)
		{
			Object.Destroy((Object)(object)Ring);
		}
		Ring = null;
		if ((Object)(object)Frame != (Object)null)
		{
			Object.Destroy((Object)(object)Frame);
		}
		Frame = null;
		if ((Object)(object)Rectangle != (Object)null)
		{
			Object.Destroy((Object)(object)Rectangle);
		}
		Rectangle = null;
		if ((Object)(object)Square != (Object)null)
		{
			Object.Destroy((Object)(object)Square);
		}
		Square = null;
	}

	private static void HighlightCircle(Vector3 center, float radius, float height)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Range<float> val = new Range<float>(radius);
		foreach (WearNTear s_allInstance in WearNTear.s_allInstances)
		{
			if (Object.op_Implicit((Object)(object)s_allInstance) && Object.op_Implicit((Object)(object)s_allInstance.m_nview) && s_allInstance.m_nview.GetZDO() != null && Selector.Within(s_allInstance.m_nview.GetZDO().GetPosition(), center, val, height))
			{
				s_allInstance.Highlight();
			}
		}
	}

	private static void HighlightRing(Vector3 center, float radius1, float radius2, float height)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.Min(radius1, radius2);
		float num2 = Mathf.Max(radius1, radius2);
		Range<float> val = new Range<float>(num, num2);
		foreach (WearNTear s_allInstance in WearNTear.s_allInstances)
		{
			if (Object.op_Implicit((Object)(object)s_allInstance) && Object.op_Implicit((Object)(object)s_allInstance.m_nview) && s_allInstance.m_nview.GetZDO() != null && Selector.Within(s_allInstance.m_nview.GetZDO().GetPosition(), center, val, height))
			{
				s_allInstance.Highlight();
			}
		}
	}

	private static void HighlightRectangle(Vector3 center, float angle, float width, float depth, float height)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		Range<float> val = new Range<float>(width);
		Range<float> val2 = new Range<float>(depth);
		foreach (WearNTear s_allInstance in WearNTear.s_allInstances)
		{
			if (Object.op_Implicit((Object)(object)s_allInstance) && Object.op_Implicit((Object)(object)s_allInstance.m_nview) && s_allInstance.m_nview.GetZDO() != null && Selector.Within(s_allInstance.m_nview.GetZDO().GetPosition(), center, angle, val, val2, height))
			{
				s_allInstance.Highlight();
			}
		}
	}

	private static void HighlightFrame(Vector3 center, float angle, float size1, float size2, float height)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.Min(size1, size2);
		Mathf.Max(size1, size2);
		Range<float> val = new Range<float>(num);
		foreach (WearNTear s_allInstance in WearNTear.s_allInstances)
		{
			if (Object.op_Implicit((Object)(object)s_allInstance) && Object.op_Implicit((Object)(object)s_allInstance.m_nview) && s_allInstance.m_nview.GetZDO() != null && Selector.Within(s_allInstance.m_nview.GetZDO().GetPosition(), center, angle, val, val, height))
			{
				s_allInstance.Highlight();
			}
		}
	}
}
