using System;
using UnityEngine;

namespace InfinityHammer;

public class RectangleRuler : BaseRuler
{
	public float Width = 20f;

	public float Depth = 20f;

	private void EdgeFix(int index, float percent, float max, float start, float end, Vector3 direction)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		float num = percent * max;
		Transform val = Get(index);
		Vector3 baseScale = BaseRuler.BaseScale;
		if (num - start < 0.5f)
		{
			baseScale.z *= num - start + 0.5f;
			val.localPosition += direction * (0.5f - baseScale.z / 2f);
		}
		if (end - num < 0.5f)
		{
			baseScale.z *= Mathf.Max(0f, end - num + 0.5f);
			val.localPosition -= direction * (0.5f - baseScale.z / 2f);
		}
		if (baseScale.z == 0f)
		{
			((Component)val).gameObject.SetActive(false);
		}
		else
		{
			((Component)val).gameObject.SetActive(true);
		}
		val.localScale = baseScale;
	}

	protected override void CreateLines()
	{
		float num = 2f * Width + 2f * Depth;
		int num2 = Math.Max(3, (int)num);
		int num3 = (int)Mathf.Max(2f, Mathf.Ceil((float)num2 * Depth / num));
		int num4 = (int)Mathf.Max(2f, Mathf.Ceil((float)num2 * Width / num));
		num2 = 2 * (num3 + num4);
		CreateSegments(num2);
		UpdateLines(num4, num3);
	}

	private void UpdateLines(int w, int d)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		int count = Segments.Count;
		int num = 0;
		int num2 = 0;
		while (num2 < d)
		{
			SetRot(num, Vector3.forward);
			num2++;
			num++;
		}
		int num3 = 0;
		while (num3 < w)
		{
			SetRot(num, Vector3.right);
			num3++;
			num++;
		}
		int num4 = 0;
		while (num4 < d)
		{
			SetRot(num, Vector3.back);
			num4++;
			num++;
		}
		int num5 = 0;
		while (num5 < w)
		{
			SetRot(num, Vector3.left);
			num5++;
			num++;
		}
		num = 0;
		float num6 = Time.time * 0.025f * (float)(count - 4);
		float num7 = 0.5f;
		Vector3 val = Width * Vector3.left - (Depth + num7) * Vector3.forward;
		float num8 = 0.5f;
		float end = num8 + 2f * Depth;
		float num9 = 2f * Depth * (float)d / (float)(d - 1);
		float num10 = num6 / (float)d;
		int num11 = 0;
		while (num11 < d)
		{
			float num12 = ((float)num11 / (float)d + num10) % 1f;
			Vector3 pos = val + num12 * num9 * Vector3.forward;
			Set(num, pos);
			EdgeFix(num, num12, num9, num8, end, Vector3.forward);
			num11++;
			num++;
		}
		val = Depth * Vector3.forward - (Width + num7) * Vector3.right;
		end = num8 + 2f * Width;
		num9 = 2f * Width * (float)w / (float)(w - 1);
		num10 = num6 / (float)w;
		int num13 = 0;
		while (num13 < w)
		{
			float num14 = ((float)num13 / (float)w + num10) % 1f;
			Vector3 pos2 = val + num14 * num9 * Vector3.right;
			Set(num, pos2);
			EdgeFix(num, num14, num9, num8, end, Vector3.right);
			num13++;
			num++;
		}
		val = Width * Vector3.right - (Depth + num7) * Vector3.back;
		end = num8 + 2f * Depth;
		num9 = 2f * Depth * (float)d / (float)(d - 1);
		num10 = num6 / (float)d;
		int num15 = 0;
		while (num15 < d)
		{
			float num16 = ((float)num15 / (float)d + num10) % 1f;
			Vector3 pos3 = val + num16 * num9 * Vector3.back;
			Set(num, pos3);
			EdgeFix(num, num16, num9, num8, end, Vector3.back);
			num15++;
			num++;
		}
		val = Depth * Vector3.back - (Width + num7) * Vector3.left;
		end = num8 + 2f * Width;
		num9 = 2f * Width * (float)w / (float)(w - 1);
		num10 = num6 / (float)w;
		int num17 = 0;
		while (num17 < w)
		{
			float num18 = ((float)num17 / (float)w + num10) % 1f;
			Vector3 pos4 = val + num18 * num9 * Vector3.left;
			Set(num, pos4);
			EdgeFix(num, num18, num9, num8, end, Vector3.left);
			num17++;
			num++;
		}
	}
}
