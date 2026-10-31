using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ServerDevcommands;

public class ScrollBarDrag : MonoBehaviour
{
	private static readonly List<RaycastResult> hits = new List<RaycastResult>();

	private bool dragging;

	private float grab;

	private void OnGUI()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		GUI.depth = int.MaxValue;
		Event current = Event.current;
		if ((int)current.type == 0 && current.button == 0)
		{
			Press(current);
		}
		else if (dragging && (int)current.type == 3)
		{
			Drag(current);
		}
		else if (dragging && (int)current.type == 1)
		{
			dragging = false;
			current.Use();
			((Terminal)Console.instance).m_input.ActivateInputField();
		}
	}

	private void OnDisable()
	{
		dragging = false;
	}

	private Camera? GetCamera()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Canvas rootCanvas = ((Component)this).GetComponentInParent<Canvas>().rootCanvas;
		if ((int)rootCanvas.renderMode != 0)
		{
			return rootCanvas.worldCamera;
		}
		return null;
	}

	private static Vector2 ToScreen(Event e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(e.mousePosition.x, (float)Screen.height - e.mousePosition.y);
	}

	private float GetFraction(Event e, out bool inside)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		RectTransform val = (RectTransform)((Component)this).transform;
		Camera camera = GetCamera();
		Vector2 val2 = ToScreen(e);
		inside = RectTransformUtility.RectangleContainsScreenPoint(val, val2, camera);
		Vector3[] array = (Vector3[])(object)new Vector3[4];
		val.GetWorldCorners(array);
		float y = RectTransformUtility.WorldToScreenPoint(camera, array[0]).y;
		return Mathf.InverseLerp(RectTransformUtility.WorldToScreenPoint(camera, array[1]).y, y, val2.y);
	}

	private bool IsTopmost(Event e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		if (!Object.op_Implicit((Object)(object)EventSystem.current))
		{
			return true;
		}
		hits.Clear();
		EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current)
		{
			position = ToScreen(e)
		}, hits);
		if (hits.Count > 0)
		{
			RaycastResult val = hits[0];
			return ((RaycastResult)(ref val)).gameObject.transform.IsChildOf(((Component)this).transform);
		}
		return false;
	}

	private void Press(Event e)
	{
		bool inside;
		float fraction = GetFraction(e, out inside);
		if (inside && IsTopmost(e))
		{
			Console instance = Console.instance;
			int count = ((Terminal)instance).m_chatBuffer.Count;
			ConsoleScrollBar.GetRange((Terminal)(object)instance, out var start, out var end);
			float num = (float)(start + end) / 2f / (float)count;
			grab = ((Mathf.Abs(fraction - num) <= (float)(end - start) / 2f / (float)count) ? (fraction - num) : 0f);
			dragging = true;
			e.Use();
			((Terminal)instance).m_input.ActivateInputField();
			Drag(e);
		}
	}

	private void Drag(Event e)
	{
		Console instance = Console.instance;
		int count = ((Terminal)instance).m_chatBuffer.Count;
		ConsoleScrollBar.GetRange((Terminal)(object)instance, out var start, out var end);
		bool inside;
		int num = Mathf.RoundToInt((GetFraction(e, out inside) - grab) * (float)count + (float)(end - start) / 2f);
		ConsoleScrollBar.Scroll((Terminal)(object)instance, count - num);
		e.Use();
	}
}
