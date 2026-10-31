using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ServerDevcommands;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InfinityHammer;

public static class SpriteHelper
{
	private static Dictionary<string, int> PrefabNames = new Dictionary<string, int>();

	private static readonly Dictionary<string, Sprite> GeneratedSpriteCache = new Dictionary<string, Sprite>();

	private static GameObject _cachedCameraObj;

	private static GameObject CameraObj => _cachedCameraObj ?? (_cachedCameraObj = CreateCamera());

	private static Camera Camera => CameraObj.GetComponent<Camera>();

	private static GameObject CreateCamera()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = new GameObject("CachedTextRenderCamera");
		Camera val2 = val.AddComponent<Camera>();
		val2.orthographic = true;
		val2.nearClipPlane = 0.1f;
		val2.farClipPlane = 10f;
		val2.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.4f);
		val2.clearFlags = (CameraClearFlags)2;
		((Behaviour)val2).enabled = false;
		val2.cullingMask = LayerMask.GetMask(new string[1] { "ghost" });
		val2.depth = -1000f;
		Object.DontDestroyOnLoad((Object)(object)val);
		return val;
	}

	public static Sprite? FindSprite(string name)
	{
		if (!Object.op_Implicit((Object)(object)ZNetScene.instance))
		{
			return null;
		}
		if (PrefabNames.Count == 0)
		{
			PrefabNames = (from kvp in ZNetScene.instance.m_namedPrefabs
				group kvp by ((Object)kvp.Value).name.ToLower()).ToDictionary((IGrouping<string, KeyValuePair<int, GameObject>> kvp) => kvp.Key, (IGrouping<string, KeyValuePair<int, GameObject>> kvp) => kvp.First().Key);
		}
		string lower = name.ToLower();
		int num = 1;
		string[] array = Parse.Split(lower, ',');
		if (array.Length > 1)
		{
			lower = array[0];
			num = ((!int.TryParse(array[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var result)) ? 1 : result);
		}
		Sprite val;
		if (PrefabNames.TryGetValue(lower, out var value))
		{
			GameObject prefab = ZNetScene.instance.GetPrefab(value);
			val = ((prefab == null) ? null : prefab.GetComponent<Piece>()?.m_icon);
			if (Object.op_Implicit((Object)(object)val))
			{
				return val;
			}
			val = ((prefab == null) ? null : prefab.GetComponent<ItemDrop>()?.m_itemData?.m_shared?.m_icons.ElementAtOrDefault(num - 1));
			if (Object.op_Implicit((Object)(object)val))
			{
				return val;
			}
		}
		val = ObjectDB.instance.m_StatusEffects.Find((StatusEffect se) => ((Object)se).name.ToLower() == lower)?.m_icon;
		if (Object.op_Implicit((Object)(object)val))
		{
			return val;
		}
		val = Player.m_localPlayer.m_skills.m_skills.Find((SkillDef skill) => ((object)(SkillType)(ref skill.m_skill)).ToString().ToLowerInvariant() == lower)?.m_icon;
		if (Object.op_Implicit((Object)(object)val))
		{
			return val;
		}
		return GetOrCreateTextSprite(name);
	}

	private static Sprite? GetOrCreateTextSprite(string text)
	{
		if (text.Length == 0)
		{
			return null;
		}
		if (GeneratedSpriteCache.TryGetValue(text, out Sprite value))
		{
			return value;
		}
		Sprite val = CreateTextSprite(text);
		GeneratedSpriteCache[text] = val;
		return val;
	}

	private static Sprite CreateTextSprite(string text)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		bool flag = text.StartsWith("+");
		if (flag)
		{
			text = text.Substring(1);
		}
		GameObject val = new GameObject("TempTextSprite");
		try
		{
			val.transform.position = Vector3.zero;
			val.SetActive(false);
			TextMeshPro val2 = val.AddComponent<TextMeshPro>();
			((TMP_Text)val2).font = Hud.instance.m_buildSelection.font;
			((TMP_Text)val2).fontSize = 8f;
			((Graphic)val2).color = Color.white;
			((TMP_Text)val2).alignment = (TextAlignmentOptions)514;
			((TMP_Text)val2).text = GenerateDisplayName(text);
			val.SetActive(true);
			val.layer = LayerMask.NameToLayer("ghost");
			((TMP_Text)val2).ForceMeshUpdate(false, false);
			Bounds bounds = ((TMP_Text)val2).bounds;
			((Behaviour)Camera).enabled = true;
			Camera.orthographicSize = ((((TMP_Text)val2).text.Length <= 2) ? 0.75f : 2f);
			if (flag)
			{
				Camera camera = Camera;
				camera.orthographicSize *= 0.25f;
			}
			CameraObj.transform.position = new Vector3(((Bounds)(ref bounds)).center.x, ((Bounds)(ref bounds)).center.y, ((Bounds)(ref bounds)).center.z - 5f);
			CameraObj.transform.LookAt(((Bounds)(ref bounds)).center);
			int num = 64;
			RenderTexture val3 = new RenderTexture(num, num, 24)
			{
				format = (RenderTextureFormat)0
			};
			RenderTexture targetTexture = Camera.targetTexture;
			Camera.targetTexture = val3;
			Camera.Render();
			RenderTexture.active = val3;
			Texture2D val4 = new Texture2D(num, num, (TextureFormat)4, false);
			val4.ReadPixels(new Rect(0f, 0f, (float)num, (float)num), 0, 0);
			val4.Apply();
			RenderTexture.active = null;
			Sprite result = Sprite.Create(val4, new Rect(0f, 0f, (float)num, (float)num), new Vector2(0.5f, 0.5f));
			val3.Release();
			Camera.targetTexture = targetTexture;
			((Behaviour)Camera).enabled = false;
			return result;
		}
		finally
		{
			Object.DestroyImmediate((Object)(object)val);
		}
	}

	public static string GenerateDisplayName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return "";
		}
		if (name.StartsWith("_"))
		{
			name = name.Substring(1);
		}
		List<string> list = new List<string>();
		string text = "";
		for (int i = 0; i < name.Length; i++)
		{
			char c = name[i];
			if (c == '_' || c == ' ' || c == ':')
			{
				if (text.Length > 0)
				{
					list.Add(text);
					text = "";
				}
			}
			else if (char.IsUpper(c) && text.Length > 0)
			{
				if (i > 0 && char.IsUpper(name[i - 1]))
				{
					text += c;
					continue;
				}
				list.Add(text);
				text = c.ToString();
			}
			else
			{
				text += c;
			}
		}
		if (text.Length > 0)
		{
			list.Add(text);
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].Length > 10)
			{
				list[j] = list[j].Substring(0, 10);
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			if (list[k].Length > 0)
			{
				list[k] = char.ToUpper(list[k][0]) + list[k].Substring(1);
			}
		}
		return string.Join("\n", list.Take(3));
	}
}
