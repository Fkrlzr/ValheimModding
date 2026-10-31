using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkToolbox;

// Labels over players, creatures, pickables, ore deposits and dropped items. Adapted from ValheimTooler's ESP
// (Yohann MARTIN / Astropilot, MIT licence). It only draws, it changes nothing in the world.
internal class SkEsp : MonoBehaviour
{
	internal static bool Players;

	internal static bool Monsters;

	internal static bool Pickables;

	internal static bool Deposits;

	internal static bool Drops;

	internal static bool LimitRadius = true;

	internal static int Radius = 100;

	private static readonly Color PlayerColor = Color.magenta;

	private static readonly Color MonsterColor = Color.red;

	private static readonly Color TamedColor = new Color(1f, 0.3f, 0f, 1f);

	private static readonly Color PickableColor = new Color(0.13f, 0.58f, 0.89f, 1f);

	private static readonly Color DropColor = new Color(0.13f, 0.72f, 0.11f, 1f);

	private static readonly Color DepositColor = Color.yellow;

	private readonly List<Character> characters = new List<Character>();

	private readonly List<Pickable> pickables = new List<Pickable>();

	private readonly List<PickableItem> pickableItems = new List<PickableItem>();

	private readonly List<ItemDrop> drops = new List<ItemDrop>();

	private readonly List<Destructible> depositsDestructible = new List<Destructible>();

	private readonly List<MineRock5> mineRocks = new List<MineRock5>();

	private readonly Dictionary<Color, Texture2D> textures = new Dictionary<Color, Texture2D>();

	private float nextScan;

	private GUIStyle style;

	internal static bool Any => Players || Monsters || Pickables || Deposits || Drops;

	private bool InRange(Camera camera, Component target)
	{
		float distance = Vector3.Distance(((Component)camera).transform.position, target.transform.position);
		return distance > 2f && (!LimitRadius || distance < (float)Radius);
	}

	private void Update()
	{
		if (!Any || Time.time < nextScan)
		{
			return;
		}
		nextScan = Time.time + 1.5f;
		characters.Clear();
		pickables.Clear();
		pickableItems.Clear();
		drops.Clear();
		depositsDestructible.Clear();
		mineRocks.Clear();
		Camera camera = global::Utils.GetMainCamera();
		Player me = Player.m_localPlayer;
		if (camera == null || me == null)
		{
			return;
		}
		if (Players || Monsters)
		{
			List<Character> all = Character.GetAllCharacters();
			if (all != null)
			{
				foreach (Character character in all)
				{
					if (character == null || (character.IsPlayer() && ((Player)character).GetPlayerID() == me.GetPlayerID()))
					{
						continue;
					}
					if (InRange(camera, character))
					{
						characters.Add(character);
					}
				}
			}
		}
		if (Pickables)
		{
			foreach (Pickable pickable in UnityEngine.Object.FindObjectsByType<Pickable>((FindObjectsSortMode)0))
			{
				if (InRange(camera, pickable))
				{
					pickables.Add(pickable);
				}
			}
			foreach (PickableItem pickableItem in UnityEngine.Object.FindObjectsByType<PickableItem>((FindObjectsSortMode)0))
			{
				if (InRange(camera, pickableItem))
				{
					pickableItems.Add(pickableItem);
				}
			}
		}
		if (Drops)
		{
			foreach (ItemDrop drop in UnityEngine.Object.FindObjectsByType<ItemDrop>((FindObjectsSortMode)0))
			{
				if (InRange(camera, drop))
				{
					drops.Add(drop);
				}
			}
		}
		if (!Deposits)
		{
			return;
		}
		foreach (MineRock5 rock in UnityEngine.Object.FindObjectsByType<MineRock5>((FindObjectsSortMode)0))
		{
			string hover = rock.GetHoverText().ToLowerInvariant();
			if (hover.Length != 0 && !hover.Contains("rock") && InRange(camera, rock))
			{
				mineRocks.Add(rock);
			}
		}
		foreach (Destructible destructible in UnityEngine.Object.FindObjectsByType<Destructible>((FindObjectsSortMode)0))
		{
			HoverText hoverText = ((Component)destructible).GetComponent<HoverText>();
			if (hoverText == null || string.IsNullOrEmpty(hoverText.m_text))
			{
				continue;
			}
			string text = hoverText.m_text.ToLowerInvariant();
			if ((text.Contains("deposit") || text.Contains("piece_mudpile")) && InRange(camera, destructible))
			{
				depositsDestructible.Add(destructible);
			}
		}
	}

	// The game renders at a scale, so screen points have to be scaled to GUI space.
	private static Vector3 ToGui(Camera camera, Vector3 world)
	{
		Vector3 point = camera.WorldToScreenPoint(world);
		point.x *= (float)Screen.width / (float)camera.pixelWidth;
		point.y *= (float)Screen.height / (float)camera.pixelHeight;
		return point;
	}

	private Texture2D Pixel(Color color)
	{
		if (!textures.TryGetValue(color, out var texture) || texture == null)
		{
			texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
			texture.SetPixel(0, 0, color);
			texture.Apply();
			textures[color] = texture;
		}
		return texture;
	}

	private void Label(Camera camera, Component target, string text, Color color, bool box, bool eyeHeight)
	{
		if (target == null)
		{
			return;
		}
		Vector3 point = ToGui(camera, target.transform.position);
		if (point.z <= -1f)
		{
			return;
		}
		float y = (float)Screen.height - point.y;
		if (box)
		{
			float height = 60f;
			if (eyeHeight && target is Character character)
			{
				height = Math.Abs(ToGui(camera, character.GetEyePoint()).y - point.y);
			}
			float width = height * 0.65f;
			Texture2D line = Pixel(color);
			float x0 = point.x - width / 2f;
			float y0 = y - height;
			GUI.DrawTexture(new Rect(x0, y0, 1f, height), line);
			GUI.DrawTexture(new Rect(x0 + width - 1f, y0, 1f, height), line);
			GUI.DrawTexture(new Rect(x0 + 1f, y0, width - 2f, 1f), line);
			GUI.DrawTexture(new Rect(x0 + 1f, y0 + height - 1f, width - 2f, 1f), line);
		}
		style.normal.textColor = color;
		GUI.Label(new Rect(point.x - 10f, y - 5f, 220f, 40f), text + " [" + (int)point.z + "]", style);
	}

	private void OnGUI()
	{
		if (!Any || Event.current == null || (int)Event.current.type != 7)
		{
			return;
		}
		Camera camera = global::Utils.GetMainCamera();
		if (camera == null || Player.m_localPlayer == null)
		{
			return;
		}
		if (style == null)
		{
			style = new GUIStyle(GUI.skin.label)
			{
				fontSize = 12,
				fontStyle = FontStyle.Bold
			};
		}
		foreach (Character character in characters)
		{
			if (character == null)
			{
				continue;
			}
			if (character.IsPlayer())
			{
				if (Players)
				{
					Label(camera, character, ((Player)character).GetPlayerName(), PlayerColor, box: true, eyeHeight: true);
				}
			}
			else if (Monsters)
			{
				Label(camera, character, Localization.instance.Localize(character.GetHoverName()), character.IsTamed() ? TamedColor : MonsterColor, box: true, eyeHeight: true);
			}
		}
		foreach (Pickable pickable in pickables)
		{
			if (pickable != null)
			{
				Label(camera, pickable, Localization.instance.Localize(pickable.GetHoverName()), PickableColor, box: false, eyeHeight: false);
			}
		}
		foreach (PickableItem pickableItem in pickableItems)
		{
			if (pickableItem != null)
			{
				Label(camera, pickableItem, Localization.instance.Localize(pickableItem.GetHoverName()), PickableColor, box: false, eyeHeight: false);
			}
		}
		foreach (ItemDrop drop in drops)
		{
			if (drop != null)
			{
				Label(camera, drop, Localization.instance.Localize(drop.GetHoverName()), DropColor, box: false, eyeHeight: false);
			}
		}
		foreach (Destructible destructible in depositsDestructible)
		{
			if (destructible != null)
			{
				HoverText hoverText = ((Component)destructible).GetComponent<HoverText>();
				Label(camera, destructible, (hoverText != null) ? Localization.instance.Localize(hoverText.GetHoverName()) : "Deposit", DepositColor, box: false, eyeHeight: false);
			}
		}
		foreach (MineRock5 rock in mineRocks)
		{
			if (rock != null)
			{
				Label(camera, rock, Localization.instance.Localize(rock.GetHoverText()), DepositColor, box: false, eyeHeight: false);
			}
		}
	}

	private void OnDestroy()
	{
		foreach (Texture2D texture in textures.Values)
		{
			if (texture != null)
			{
				UnityEngine.Object.Destroy(texture);
			}
		}
		textures.Clear();
	}
}
