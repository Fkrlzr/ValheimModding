using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MobBattle;

// The battle menu and the small on-screen counter, both drawn with IMGUI so no asset bundle is needed.
public class BattleUi : MonoBehaviour
{
	private static BattleUi? instance;

	private const int WindowId = 0x4D42;

	private PickMode tab = PickMode.Faction;
	private string filter = "";
	private Vector2 scroll;
	private PickEntry? picked;
	private PickEntry? opponent;
	private bool assignToB;
	private int countA;
	private int countB;
	private int starsA;
	private int starsB;
	private readonly ClllcChoice choiceA = new();
	private readonly ClllcChoice choiceB = new();
	private bool initialized;
	private Rect window;
	private string notice = "";
	private float hudHideAt;
	private bool resultShown;

	private PickEntry? lastA;
	private PickEntry? lastB;
	private int lastCountA;
	private int lastCountB;
	private int lastStarsA;
	private int lastStarsB;

	internal static bool IsOpen { get; private set; }

	public void Awake()
	{
		instance = this;
	}

	internal static void Open()
	{
		if (instance == null || Player.m_localPlayer == null)
		{
			return;
		}

		if (!BattleCatalog.Available)
		{
			Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, "Mob Battle: load into a world first");
			return;
		}

		instance.OpenMenu();
	}

	internal static void Close()
	{
		IsOpen = false;
		GUIUtility.keyboardControl = 0;
	}

	// Right click with the wand: stop a running battle, otherwise repeat the last one.
	internal static void QuickToggle()
	{
		if (instance == null)
		{
			return;
		}

		if (BattleManager.Running)
		{
			BattleManager.Stop();
			return;
		}

		if (instance.lastA == null || instance.lastB == null)
		{
			Open();
			return;
		}

		instance.StartBattle(instance.lastA, instance.lastB, instance.lastCountA, instance.lastCountB, instance.lastStarsA, instance.lastStarsB, false);
	}

	private void OpenMenu()
	{
		if (!initialized)
		{
			countA = Mathf.Clamp(MobBattle.defaultCount.Value, 1, MobBattle.maxCount.Value);
			countB = countA;
			starsA = Mathf.Clamp(MobBattle.defaultStars.Value, 0, MobBattle.maxStars.Value);
			starsB = starsA;
			initialized = true;
		}

		float scale = Scale();
		float width = 600f;
		float height = Mathf.Min(Screen.height / scale - 40f, 760f);
		window = new Rect((Screen.width / scale - width) / 2f, (Screen.height / scale - height) / 2f, width, height);
		notice = "";
		IsOpen = true;
	}

	public void Update()
	{
		BattleManager.Tick();

		if (!IsOpen)
		{
			return;
		}

		if (Player.m_localPlayer == null || Input.GetKeyDown(KeyCode.Escape))
		{
			Close();
			return;
		}

		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	private static float Scale()
	{
		return Mathf.Clamp(Screen.height / 1080f, 1f, 2.5f);
	}

	public void OnGUI()
	{
		Battle? battle = BattleManager.Current;
		if (battle != null && battle.Finished && !resultShown)
		{
			resultShown = true;
			hudHideAt = Time.time + 25f;
		}
		else if (battle != null && !battle.Finished)
		{
			resultShown = false;
		}

		float scale = Scale();
		Matrix4x4 previous = GUI.matrix;
		GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1f));

		if (IsOpen)
		{
			GUI.depth = -100;
			window = GUILayout.Window(WindowId, window, DrawWindow, "Mob Battle");
			window.x = Mathf.Clamp(window.x, 0f, Mathf.Max(0f, Screen.width / scale - window.width));
			window.y = Mathf.Clamp(window.y, 0f, Mathf.Max(0f, Screen.height / scale - window.height));
		}
		else
		{
			DrawHud(scale);
		}

		GUI.matrix = previous;
	}

	private void DrawHud(float scale)
	{
		Battle? battle = BattleManager.Current;
		if (battle == null || Player.m_localPlayer == null)
		{
			return;
		}

		bool showResult = battle.Finished && Time.time < hudHideAt;
		if (battle.Finished && !showResult)
		{
			return;
		}

		string text = battle.Finished ? BattleManager.LastResult : BattleManager.Status();
		float width = Mathf.Min(Screen.width / scale - 20f, battle.Finished ? 700f : 640f);
		GUIStyle style = new(GUI.skin.box) { alignment = battle.Finished ? TextAnchor.UpperLeft : TextAnchor.MiddleCenter, wordWrap = true, fontSize = 15, richText = false };
		float height = style.CalcHeight(new GUIContent(text), width - 10f) + 14f;
		GUI.Box(new Rect((Screen.width / scale - width) / 2f, 12f, width, height), text, style);
	}

	private void DrawWindow(int id)
	{
		GUILayout.BeginVertical();

		// Mode tabs.
		GUILayout.BeginHorizontal();
		TabButton("Faction", PickMode.Faction);
		TabButton("Mob type", PickMode.Family);
		TabButton("Single mob", PickMode.Mob);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Search", GUILayout.Width(52f));
		filter = GUILayout.TextField(filter ?? "");
		if (GUILayout.Button("x", GUILayout.Width(26f)))
		{
			filter = "";
			GUIUtility.keyboardControl = 0;
		}

		GUILayout.EndHorizontal();

		// Both sides are always chosen by hand: these buttons say which side the next click in the list fills.
		GUILayout.BeginHorizontal();
		Color assignColor = GUI.backgroundColor;
		GUI.backgroundColor = !assignToB ? new Color(0.45f, 0.85f, 0.45f) : assignColor;
		if (GUILayout.Button("Next click sets side A", GUILayout.Height(24f)))
		{
			assignToB = false;
		}

		GUI.backgroundColor = assignToB ? new Color(0.95f, 0.55f, 0.45f) : assignColor;
		if (GUILayout.Button("Next click sets side B", GUILayout.Height(24f)))
		{
			assignToB = true;
		}

		GUI.backgroundColor = assignColor;
		GUILayout.EndHorizontal();

		// Pick list.
		scroll = GUILayout.BeginScrollView(scroll, GUI.skin.box, GUILayout.Height(window.height * (ClllcCompat.Present ? 0.27f : 0.34f)));
		var entries = BattleCatalog.Entries(tab);
		string needle = (filter ?? "").Trim();
		int shown = 0;
		foreach (PickEntry entry in entries)
		{
			if (needle.Length > 0 && entry.Display.IndexOf(needle, StringComparison.OrdinalIgnoreCase) < 0 && entry.Key.IndexOf(needle, StringComparison.OrdinalIgnoreCase) < 0)
			{
				continue;
			}

			shown++;
			Color previousColor = GUI.backgroundColor;
			bool selected = picked != null && picked.Mode == entry.Mode && picked.Key == entry.Key;
			bool selectedB = opponent != null && opponent.Mode == entry.Mode && opponent.Key == entry.Key;
			if (selected)
			{
				GUI.backgroundColor = new Color(0.45f, 0.85f, 0.45f);
			}
			else if (selectedB)
			{
				GUI.backgroundColor = new Color(0.95f, 0.55f, 0.45f);
			}

			string label = entry.Mode == PickMode.Mob ? entry.Display : entry.Label;
			if (GUILayout.Button(label, GUILayout.Height(24f)))
			{
				SelectSide(entry);
			}

			GUI.backgroundColor = previousColor;
		}

		if (shown == 0)
		{
			GUILayout.Label(entries.Count == 0 ? "No creatures found yet. Load into a world." : "Nothing matches the search.");
		}

		GUILayout.EndScrollView();

		// Selection summary.
		GUILayout.Space(4f);
		GUILayout.Label($"Side A:  {(picked != null ? SideText(picked) : "pick something from the list")}");
		GUILayout.Label($"Side B:  {(opponent != null ? SideText(opponent) : "pick something from the list")}");

		// Settings.
		GUILayout.Space(4f);
		int maxCount = MobBattle.maxCount.Value;
		int maxStars = MobBattle.maxStars.Value;
		GUILayout.BeginHorizontal();
		GUILayout.Label($"Side A mobs: {countA}", GUILayout.Width(150f));
		countA = Mathf.Clamp(Mathf.RoundToInt(GUILayout.HorizontalSlider(countA, 1, maxCount)), 1, maxCount);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label($"Side B mobs: {countB}", GUILayout.Width(150f));
		countB = Mathf.Clamp(Mathf.RoundToInt(GUILayout.HorizontalSlider(countB, 1, maxCount)), 1, maxCount);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label($"Side A stars: {starsA}  (lvl {starsA + 1})", GUILayout.Width(150f));
		starsA = maxStars > 0 ? Mathf.Clamp(Mathf.RoundToInt(GUILayout.HorizontalSlider(starsA, 0, maxStars)), 0, maxStars) : 0;
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label($"Side B stars: {starsB}  (lvl {starsB + 1})", GUILayout.Width(150f));
		starsB = maxStars > 0 ? Mathf.Clamp(Mathf.RoundToInt(GUILayout.HorizontalSlider(starsB, 0, maxStars)), 0, maxStars) : 0;
		GUILayout.EndHorizontal();
		if (ClllcCompat.Present)
		{
			GUILayout.Label(MobBattle.keepClllcEffects.Value
				? "CLLC detected: its health/damage scaling applies. Default keeps its random effects (Splitting is always removed). Click a button to change it."
				: "CLLC detected: its health/damage scaling applies. Default removes random effects, infusions and boss affixes. Click a button to pick one instead.");
			ClllcRow("Side A  CLLC", choiceA);
			ClllcRow("Side B  CLLC", choiceB);
		}

		// Buttons.
		GUILayout.Space(6f);
		GUILayout.BeginHorizontal();
		GUI.enabled = picked != null && opponent != null;
		if (GUILayout.Button("Start battle", GUILayout.Height(30f)))
		{
			StartBattle(picked!, opponent!, countA, countB, starsA, starsB, true);
		}

		GUI.enabled = true;
		if (GUILayout.Button("Stop and clear", GUILayout.Height(30f)))
		{
			BattleManager.Stop();
			notice = "All battle creatures removed.";
		}

		if (GUILayout.Button("Close", GUILayout.Height(30f), GUILayout.Width(80f)))
		{
			Close();
		}

		GUILayout.EndHorizontal();

		if (!string.IsNullOrEmpty(notice))
		{
			GUILayout.Label(notice);
		}

		// Live status and last result.
		if (BattleManager.Current != null)
		{
			GUILayout.Label(BattleManager.Running ? "Running:  " + BattleManager.Status() : "Last battle:");
			if (!BattleManager.Running && !string.IsNullOrEmpty(BattleManager.LastResult))
			{
				GUILayout.Label(BattleManager.LastResult, GUI.skin.box);
			}
		}

		GUILayout.EndVertical();
		GUI.DragWindow(new Rect(0f, 0f, 10000f, 22f));
	}

	private string SideText(PickEntry entry)
	{
		return entry.Mode == PickMode.Mob ? entry.Display : $"{entry.Display}  [{string.Join(", ", entry.Members.Take(4).Select(m => m.Display))}{(entry.Members.Count > 4 ? ", ..." : "")}]";
	}

	private void TabButton(string title, PickMode mode)
	{
		Color previousColor = GUI.backgroundColor;
		if (tab == mode)
		{
			GUI.backgroundColor = new Color(0.55f, 0.75f, 1f);
		}

		if (GUILayout.Button(title, GUILayout.Height(26f)) && tab != mode)
		{
			tab = mode;
			picked = null;
			opponent = null;
			assignToB = false;
			filter = "";
			scroll = Vector2.zero;
		}

		GUI.backgroundColor = previousColor;
	}

	private void SelectSide(PickEntry entry)
	{
		// Same entry on both sides is allowed: a mirror match.
		if (assignToB)
		{
			opponent = entry;
		}
		else
		{
			picked = entry;
			// After choosing side A the next click naturally fills side B.
			assignToB = true;
		}

		notice = "";
	}

	private static string Cycle(string current, List<string> options)
	{
		int index = options.IndexOf(current);
		return options[(index + 1) % options.Count];
	}

	// One row of buttons for what CLLC gives a side. Each click steps to the next value.
	private static void ClllcRow(string title, ClllcChoice choice)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(title, GUILayout.Width(150f));
		if (GUILayout.Button("Effect: " + choice.Effect))
		{
			choice.Effect = Cycle(choice.Effect, ClllcCompat.EffectOptions());
		}

		if (GUILayout.Button("Infusion: " + choice.Infusion))
		{
			choice.Infusion = Cycle(choice.Infusion, ClllcCompat.InfusionOptions());
		}

		if (GUILayout.Button("Boss affix: " + choice.Affix))
		{
			choice.Affix = Cycle(choice.Affix, ClllcCompat.AffixOptions());
		}

		GUILayout.EndHorizontal();
	}

	private static ClllcChoice Copy(ClllcChoice choice)
	{
		return new ClllcChoice { Effect = choice.Effect, Infusion = choice.Infusion, Affix = choice.Affix };
	}

	private void StartBattle(PickEntry a, PickEntry b, int mobCountA, int mobCountB, int mobStarsA, int mobStarsB, bool closeMenu)
	{
		if (BattleManager.Start(a, b, mobCountA, mobCountB, mobStarsA, mobStarsB, Copy(choiceA), Copy(choiceB)))
		{
			lastA = a;
			lastB = b;
			lastCountA = mobCountA;
			lastCountB = mobCountB;
			lastStarsA = mobStarsA;
			lastStarsB = mobStarsB;
			notice = "";
			if (closeMenu)
			{
				Close();
			}
		}
		else
		{
			notice = string.IsNullOrEmpty(BattleManager.LastResult) ? "Could not start the battle." : BattleManager.LastResult;
		}
	}
}
