using System;
using System.Collections.Generic;
using SkToolbox.Configuration;
using SkToolbox.SkModules;
using SkToolbox.Utility;
using UnityEngine;

namespace SkToolbox;

public class SkMenuController : MonoBehaviour
{
	private class MenuFrame
	{
		public string Title;

		public List<SkMenuItem> Items;

		public Vector2 Scroll;

		public string Filter = "";

		public List<SkMenuSlider> Sliders;

		public List<SkMenuToggle> Toggles;

		public List<SkGridItem> Grid;

		public List<SkGridItem> GridFiltered;

		public string GridFilterKey;

		public bool ShowFilter = true;
	}

	public enum SkScope
	{
		Unset,
		Client,
		Server,
		Admin,
		HostOnly
	}

	public class SkGridItem
	{
		public string Name;

		public string Display;

		public string Tip;

		public Sprite Icon;

		public Action<string> OnClick;

		public Func<bool> IsOn;

		public string Section;

		public SkScope Scope;

		private string hoverCache;

		internal string HoverText
		{
			get
			{
				if (hoverCache == null)
				{
					string text = Tip ?? Display ?? Name ?? "";
					string text2 = ScopeMark(Scope);
					hoverCache = ((text2.Length == 0) ? text : (text + "\n" + text2));
				}
				return hoverCache;
			}
		}

		private static string ScopeMark(SkScope scope)
		{
			return scope switch
			{
				SkScope.Client => "<color=#8FB8D8>(client)</color>  <color=#9FB6CC>changes nothing for anyone else</color>", 
				SkScope.Server => "<color=#E5A959>(server)</color>  <color=#9FB6CC>changes the world for everyone on it</color>", 
				SkScope.Admin => "<color=#E5A959>(server)</color>  <color=#9FB6CC>sent to the server, which needs you to be admin</color>", 
				SkScope.HostOnly => "<color=#E08585>(host only)</color>  <color=#9FB6CC>does nothing unless you are running the world</color>", 
				_ => "", 
			};
		}
	}

	public enum SkFieldKind
	{
		Choice,
		IntSlider,
		Text,
		Toggle,
		Info,
		Checklist
	}

	public class SkFormField
	{
		public string Id = "";

		public string Label = "";

		public SkFieldKind Kind = SkFieldKind.Text;

		public List<string> Options = new List<string>();

		public List<string> OptionLabels = new List<string>();

		public int Selected;

		public List<bool> Checked = new List<bool>();

		public int Min;

		public int Max = 100;

		public int IntValue;

		public string TextValue = "";

		public bool BoolValue;

		public int Height;

		internal string Filter = "";

		internal Vector2 Scroll;

		public string SelectedOption
		{
			get
			{
				if (Options == null || Selected < 0 || Selected >= Options.Count)
				{
					return null;
				}
				return Options[Selected];
			}
		}
	}

	public class SkFormAction
	{
		public string Label = "Accept";

		public Action<SkForm> Run;
	}

	public class SkForm
	{
		public string Title = "";

		public string Note = "";

		public List<SkFormField> Fields = new List<SkFormField>();

		public List<SkFormAction> Actions = new List<SkFormAction>();

		public string Warning = "";

		public Func<SkForm, string> Validate;

		internal string Error;

		public SkFormField Field(string id)
		{
			foreach (SkFormField field in Fields)
			{
				if (field.Id == id)
				{
					return field;
				}
			}
			return null;
		}
	}

	public class SkPrompt
	{
		public string Title = "";

		public string Label = "";

		public string Accept = "Save";

		public string Value = "";

		public Action<string> OnAccept;
	}

	public class SkMenuToggle
	{
		public string Label = "Toggle";

		public Func<bool> Get;

		public Action<bool> Set;
	}

	public class SkMenuSlider
	{
		public string Label = "Value";

		public int Min = 1;

		public int Max = 100;

		public Func<int> Get;

		public Action<int> Set;
	}

	internal static System.Version SkMenuControllerVersion = new System.Version(2, 0, 0);

	internal static SkUtilities.Status SkMenuControllerStatus = SkUtilities.Status.Initialized;

	private readonly string appName = "Speelo's Toolbox";

	private bool initialCheck = true;

	internal bool logResponse;

	private bool menuOpen;

	public List<SkBaseModule> menuOptions;

	public SkModuleController SkModuleController;

	private int selectedModule = -1;

	private readonly List<MenuFrame> frames = new List<MenuFrame>();

	private Action pendingAction;

	private SkMenuSlider pendingSlider;

	private readonly Dictionary<SkMenuSlider, string> sliderText = new Dictionary<SkMenuSlider, string>();

	private readonly Dictionary<SkMenuSlider, int> sliderLast = new Dictionary<SkMenuSlider, int>();

	private string hoverTip = "";

	private const int FilterThreshold = 12;

	private const float GridCell = 54f;

	private const float GridPad = 4f;

	private SkPrompt activePrompt;

	private bool promptNeedsFocus;

	private const string PromptFieldName = "SkPromptField";

	private SkForm activeForm;

	private const int WindowId = 49000;

	private Rect windowRect = new Rect(24f, 80f, 770f, 580f);

	private bool windowPlaced;

	private bool stylesReady;

	private float stylesAlpha = -1f;

	private GUIStyle styleWindow;

	private GUIStyle styleItem;

	private GUIStyle styleHeader;

	private GUIStyle styleTip;

	private GUIStyle styleSmall;

	private GUIStyle styleBack;

	private GUIStyle styleFilter;

	private GUIStyle styleTab;

	private GUIStyle styleTabOn;

	private GUIStyle styleClose;

	private GUIStyle styleGridCell;

	private GUIStyle styleSectionBox;

	private GUIStyle styleSectionHeader;

	private GUIStyle styleTooltip;

	private GUIStyle styleError;

	private GUIStyle styleWarning;

	private GUIStyle stylePrompt;

	private static Texture2D texWindow;

	private static Texture2D texPanel;

	private static Texture2D texItem;

	private static Texture2D texItemHover;

	private static Texture2D texItemActive;

	private static Texture2D texAccent;

	private static Texture2D texWhite;

	private static Texture2D texTooltip;

	private static Texture2D texPrompt;

	private KeyCode toggleKey = (KeyCode)287;

	private static readonly HashSet<KeyCode> s_unmappedKeys = new HashSet<KeyCode>();

	private static readonly Color OnColor = new Color(0.48f, 0.95f, 0.48f);

	internal static SkMenuController Instance { get; private set; }

	public static bool IsOpen
	{
		get
		{
			if ((Object)(object)Instance != (Object)null)
			{
				return Instance.menuOpen;
			}
			return false;
		}
	}

	internal bool PromptOpen => activePrompt != null;

	private static float ConfiguredOpacity
	{
		get
		{
			if (SkConfigEntry.OMenuOpacity != null)
			{
				return Mathf.Clamp(SkConfigEntry.OMenuOpacity.Value, 0.25f, 1f);
			}
			return 0.96f;
		}
	}

	internal KeyCode ToggleKey => toggleKey;

	private static KeyCode LookKey
	{
		get
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			string value = ((SkConfigEntry.OMenuLookKey != null) ? SkConfigEntry.OMenuLookKey.Value : null);
			if (string.IsNullOrEmpty(value))
			{
				return (KeyCode)324;
			}
			try
			{
				return (KeyCode)Enum.Parse(typeof(KeyCode), value, ignoreCase: true);
			}
			catch (Exception)
			{
				return (KeyCode)324;
			}
		}
	}

	public void ShowPrompt(SkPrompt prompt)
	{
		if (prompt != null)
		{
			activePrompt = prompt;
			promptNeedsFocus = true;
			menuOpen = true;
		}
	}

	public void ClosePrompt()
	{
		activePrompt = null;
		promptNeedsFocus = false;
		GUIUtility.keyboardControl = 0;
	}

	public void ShowForm(SkForm form)
	{
		if (form != null)
		{
			activeForm = form;
			menuOpen = true;
		}
	}

	public void CloseForm()
	{
		activeForm = null;
	}

	private static Texture2D MakeTex(Color color)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Texture2D val = new Texture2D(1, 1, (TextureFormat)4, false);
		val.SetPixel(0, 0, color);
		val.Apply();
		((Texture)val).wrapMode = (TextureWrapMode)1;
		((Texture)val).filterMode = (FilterMode)0;
		((Object)val).hideFlags = (HideFlags)61;
		return val;
	}

	private static bool KeyDown(KeyCode key)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (s_unmappedKeys.Contains(key))
		{
			return false;
		}
		SkCommandPatcher.BypassInputBlock = true;
		try
		{
			return ZInput.GetKeyDown(key, false);
		}
		catch (ArgumentOutOfRangeException)
		{
			s_unmappedKeys.Add(key);
			SkUtilities.Logz(new string[2] { "CONTROLLER", "WARN" }, new string[1] { "KeyCode " + ((object)key).ToString() + " has no Input System mapping in ZInput; bind ignored." }, (LogType)3);
			return false;
		}
		finally
		{
			SkCommandPatcher.BypassInputBlock = false;
		}
	}

	private void Awake()
	{
		Instance = this;
	}

	private void OnDestroy()
	{
		if ((Object)(object)Instance == (Object)(object)this)
		{
			Instance = null;
			ReleaseTextures();
			stylesReady = false;
		}
	}

	private void Start()
	{
		SkMenuControllerStatus = SkUtilities.Status.Loading;
		SkUtilities.Logz(new string[2] { "CONTROLLER", "NOTIFY" }, new string[2] { "LOADING...", "WAITING FOR TOOLBOX." }, (LogType)3);
		SkModuleController = ((Component)this).gameObject.AddComponent<SkModuleController>();
		ApplyToggleKey();
	}

	internal void ApplyToggleKey()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		string text = SkConfigEntry.OMenuToggleKey?.Value;
		if (!string.IsNullOrWhiteSpace(text) && Enum.TryParse<KeyCode>(text.Trim(), ignoreCase: true, out KeyCode result) && Enum.IsDefined(typeof(KeyCode), result) && (int)result != 0)
		{
			toggleKey = result;
			return;
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			SkUtilities.Logz(new string[2] { "CONTROLLER", "CONFIG" }, new string[1] { "Invalid MenuToggleKey '" + text + "' in config, using F6" }, (LogType)2);
		}
		toggleKey = (KeyCode)287;
	}

	private void SampleInputState()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (!menuOpen)
		{
			SkCommandPatcher.LookHeld = false;
			SkCommandPatcher.WalkAllowed = false;
			return;
		}
		bool flag = menuOpen && !ZInput.IsGamepadActive() && (SkConfigEntry.OMenuWalk == null || SkConfigEntry.OMenuWalk.Value);
		bool flag2 = flag && ZInput.GetKey(LookKey, false);
		if (!flag2 && SkCommandPatcher.LookHeld)
		{
			GUIUtility.keyboardControl = 0;
		}
		SkCommandPatcher.LookHeld = flag2;
		bool flag3 = !flag2 && GUIUtility.keyboardControl != 0;
		SkCommandPatcher.WalkAllowed = flag && !flag3;
		if (flag)
		{
			PlayerController.SetTakeInputDelay(0.25f);
		}
	}

	private void Update()
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		SampleInputState();
		if (initialCheck)
		{
			if (menuOptions == null || menuOptions.Count == 0)
			{
				UpdateMenuOptions(SkModuleController.GetOptions());
			}
			else
			{
				SkMenuControllerStatus = SkUtilities.Status.Ready;
				if (SkModuleController.SkMainStatus == SkUtilities.Status.Ready)
				{
					initialCheck = false;
					SkUtilities.Logz(new string[2] { "CONTROLLER", "NOTIFY" }, new string[1] { "READY. Press " + ((object)toggleKey).ToString() + " to open the menu." }, (LogType)3);
				}
			}
		}
		if (Console.IsVisible() || ((Object)(object)Chat.instance != (Object)null && Chat.instance.HasFocus()) || TextInput.IsVisible() || Minimap.InTextInput())
		{
			return;
		}
		if (KeyDown(toggleKey))
		{
			if (menuOpen)
			{
				CloseMenu();
			}
			else
			{
				OpenMenu();
			}
		}
		else if (menuOpen && KeyDown((KeyCode)27))
		{
			if (activePrompt != null)
			{
				ClosePrompt();
			}
			else
			{
				CloseMenu();
			}
		}
	}

	public void OpenMenu()
	{
		if (menuOptions != null && menuOptions.Count != 0)
		{
			SkCommandPatcher.InitPatch();
			menuOpen = true;
			if (selectedModule < 0 || selectedModule >= menuOptions.Count || frames.Count == 0)
			{
				SelectModule(0);
			}
			else
			{
				RefreshRootFrame();
			}
		}
	}

	public void CloseMenu()
	{
		menuOpen = false;
		activeForm = null;
		activePrompt = null;
	}

	private void SelectModule(int index)
	{
		if (menuOptions != null && index >= 0 && index < menuOptions.Count)
		{
			selectedModule = index;
			frames.Clear();
			SkBaseModule skBaseModule = menuOptions[index];
			try
			{
				skBaseModule.CallerEntry?.ItemClass?.Invoke();
			}
			catch (Exception ex)
			{
				SkUtilities.Logz(new string[2] { "CONTROLLER", "ERROR" }, new string[1] { ex.Message }, (LogType)3);
			}
			if (frames.Count == 0)
			{
				frames.Add(new MenuFrame
				{
					Title = ModuleTitle(skBaseModule),
					Items = (skBaseModule.FlushMenu() ?? new List<SkMenuItem>())
				});
			}
			else
			{
				frames[0].Title = ModuleTitle(skBaseModule);
			}
		}
	}

	private void RefreshRootFrame()
	{
		if (frames.Count != 0 && selectedModule >= 0 && menuOptions != null && selectedModule < menuOptions.Count)
		{
			List<SkMenuItem> list = menuOptions[selectedModule].FlushMenu();
			if (list != null && list.Count > 0)
			{
				frames[0].Items = list;
			}
		}
	}

	private void PopFrame()
	{
		if (frames.Count > 1)
		{
			frames.RemoveAt(frames.Count - 1);
		}
		if (frames.Count == 1)
		{
			RefreshRootFrame();
		}
	}

	private void InvokeItem(SkMenuItem item)
	{
		if (item == null)
		{
			return;
		}
		bool flag = item.ItemText != null && item.ItemText.Contains("►");
		int count = frames.Count;
		SkCommandProcessor.MenuFeedbackActive = true;
		SkCommandProcessor.MenuFeedbackShown = false;
		try
		{
			if (item.ItemClass != null)
			{
				item.ItemClass();
			}
			else if (item.ItemClassStr != null)
			{
				item.ItemClassStr(item.ItemText);
			}
		}
		catch (Exception ex)
		{
			SkUtilities.Logz(new string[2] { "CONTROLLER", "ERROR" }, new string[1] { "Error running menu item '" + item.ItemText + "': " + ex.Message }, (LogType)0);
			SkCommandProcessor.Notify("That failed: " + ex.Message);
		}
		finally
		{
			SkCommandProcessor.MenuFeedbackActive = false;
		}
		if (frames.Count == 1)
		{
			RefreshRootFrame();
		}
		if (!SkCommandProcessor.MenuFeedbackShown && !flag && frames.Count == count)
		{
			SkCommandProcessor.Notify(CurrentLabelFor(item));
		}
	}

	private void DrawCloseButton()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (GUI.Button(new Rect(windowRect.width - 30f, 5f, 24f, 21f), "X", styleClose))
		{
			pendingAction = CloseMenu;
		}
	}

	private void DrawTabs()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		for (int i = 0; i < menuOptions.Count; i++)
		{
			SkBaseModule skBaseModule = menuOptions[i];
			if ((Object)(object)skBaseModule == (Object)null)
			{
				continue;
			}
			bool flag = i == selectedModule;
			if (GUILayout.Button(new GUIContent(ModuleTitle(skBaseModule), skBaseModule.CallerEntry?.ItemTip ?? ""), flag ? styleTabOn : styleTab, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) }) && !flag)
			{
				int captured = i;
				pendingAction = delegate
				{
					SelectModule(captured);
				};
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(8f);
	}

	private void DrawFrameHeader()
	{
		if (frames.Count > 1)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("< Back", styleBack, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(80f) }))
			{
				pendingAction = PopFrame;
			}
			GUILayout.Space(8f);
			GUILayout.Label(frames[frames.Count - 1].Title, styleHeader, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
		}
	}

	private bool CurrentFrameIsFilterable()
	{
		if (frames.Count == 0)
		{
			return false;
		}
		MenuFrame menuFrame = frames[frames.Count - 1];
		if (menuFrame.Grid != null)
		{
			if (menuFrame.ShowFilter)
			{
				return menuFrame.Grid.Count > 12;
			}
			return false;
		}
		if (menuFrame.Items != null)
		{
			return menuFrame.Items.Count > 12;
		}
		return false;
	}

	private void DrawFilterRow()
	{
		if (CurrentFrameIsFilterable())
		{
			MenuFrame menuFrame = frames[frames.Count - 1];
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Search", styleSmall, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(52f) });
			menuFrame.Filter = GUILayout.TextField(menuFrame.Filter ?? "", styleFilter, Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("x", styleBack, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(28f) }))
			{
				menuFrame.Filter = "";
			}
			GUILayout.EndHorizontal();
		}
	}

	private void DrawSliderRow()
	{
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Invalid comparison between Unknown and I4
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		if (frames.Count == 0)
		{
			return;
		}
		List<SkMenuSlider> sliders = frames[frames.Count - 1].Sliders;
		if (sliders == null)
		{
			return;
		}
		foreach (SkMenuSlider item in sliders)
		{
			if (item != null && item.Get != null && item.Set != null)
			{
				int num = Mathf.Clamp(item.Get(), item.Min, item.Max);
				int num2 = num;
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				bool typeable = item.Max > 1000;
				if (typeable)
				{
					// Wide ranges (item level goes up to 65535) are too coarse to set with the slider alone, so
					// they get a box you can type the exact value into.
					if (!sliderLast.TryGetValue(item, out var lastValue) || lastValue != num || !sliderText.ContainsKey(item))
					{
						sliderText[item] = num.ToString();
					}
					GUILayout.Label(item.Label + ":", styleSmall, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(52f) });
					string text = GUILayout.TextField(sliderText[item], 10, styleFilter, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(96f) });
					if (text != sliderText[item])
					{
						text = new string(System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Where(text, char.IsDigit)));
						sliderText[item] = text;
						if (int.TryParse(text, out var typed))
						{
							num2 = typed;
						}
					}
				}
				else
				{
					GUILayout.Label(item.Label + ": " + num, styleSmall, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(110f) });
				}
				if (GUILayout.Button("-", styleBack, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(28f) }))
				{
					num2 = num - 1;
				}
				float num3 = GUILayout.HorizontalSlider((float)num, (float)item.Min, (float)item.Max, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(120f) });
				int num4 = Mathf.RoundToInt(num3);
				if (num4 != num)
				{
					num2 = num4;
				}
				if (GUILayout.Button("+", styleBack, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(28f) }))
				{
					num2 = num + 1;
				}
				GUILayout.EndHorizontal();
				num2 = Mathf.Clamp(num2, item.Min, item.Max);
				if (num2 != num)
				{
					item.Set(num2);
					if (typeable && sliderText.TryGetValue(item, out var shown) && int.TryParse(shown, out var shownValue) && shownValue != num2)
					{
						sliderText[item] = num2.ToString();
					}
				}
				if (typeable)
				{
					sliderLast[item] = num2;
				}
			}
		}
		List<SkMenuToggle> toggles = frames[frames.Count - 1].Toggles;
		if (toggles == null)
		{
			return;
		}
		foreach (SkMenuToggle item2 in toggles)
		{
			if (item2 != null && item2.Get != null && item2.Set != null)
			{
				bool flag = item2.Get();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				string text = item2.Label + "   " + (flag ? "<color=#7CFC00>[ON]</color>" : "<color=#FF8080>[OFF]</color>");
				bool flag2 = GUILayout.Button(new GUIContent(text, item2.Label), styleBack, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(190f) });
				if ((int)Event.current.type == 7 && flag)
				{
					DrawOutline(GUILayoutUtility.GetLastRect(), OnColor, 2f);
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				if (flag2)
				{
					item2.Set(!flag);
				}
			}
		}
	}

	private void DrawItemList()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Invalid comparison between Unknown and I4
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		if (frames.Count == 0)
		{
			GUILayout.FlexibleSpace();
			GUILayout.Label("Pick a tab above.", styleTip, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			return;
		}
		MenuFrame menuFrame = frames[frames.Count - 1];
		if (menuFrame.Grid != null)
		{
			DrawGrid(menuFrame);
			return;
		}
		menuFrame.Scroll = GUILayout.BeginScrollView(menuFrame.Scroll, false, true, Array.Empty<GUILayoutOption>());
		string text = (CurrentFrameIsFilterable() ? (menuFrame.Filter ?? "").Trim() : "");
		int num = 0;
		if (menuFrame.Items != null)
		{
			for (int i = 0; i < menuFrame.Items.Count; i++)
			{
				SkMenuItem skMenuItem = menuFrame.Items[i];
				if (skMenuItem == null || string.IsNullOrEmpty(skMenuItem.ItemText) || (text.Length > 0 && skMenuItem.ItemText.IndexOf(text, StringComparison.OrdinalIgnoreCase) < 0))
				{
					continue;
				}
				num++;
				string text2 = Display(skMenuItem.ItemText);
				if ((Object)(object)skMenuItem.Icon != (Object)null)
				{
					text2 = "        " + text2;
				}
				if (GUILayout.Button(new GUIContent(text2, skMenuItem.ItemTip ?? ""), styleItem, Array.Empty<GUILayoutOption>()))
				{
					SkMenuItem captured = skMenuItem;
					pendingAction = delegate
					{
						InvokeItem(captured);
					};
				}
				if ((Object)(object)skMenuItem.Icon != (Object)null && (int)Event.current.type == 7)
				{
					Rect lastRect = GUILayoutUtility.GetLastRect();
					DrawSprite(new Rect(lastRect.x + 3f, lastRect.y, lastRect.height, lastRect.height), skMenuItem.Icon, 2f);
				}
			}
		}
		if (num == 0)
		{
			GUILayout.Label("No matches.", styleTip, Array.Empty<GUILayoutOption>());
		}
		GUILayout.EndScrollView();
	}

	private void DrawGrid(MenuFrame frame)
	{
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Invalid comparison between Unknown and I4
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		string text = (frame.Filter ?? "").Trim();
		if (frame.GridFiltered == null || frame.GridFilterKey != text)
		{
			frame.GridFilterKey = text;
			if (text.Length == 0)
			{
				frame.GridFiltered = frame.Grid;
			}
			else
			{
				List<SkGridItem> list = new List<SkGridItem>();
				foreach (SkGridItem item in frame.Grid)
				{
					if (item != null && ((item.Display != null && item.Display.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0) || (item.Name != null && item.Name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)))
					{
						list.Add(item);
					}
				}
				frame.GridFiltered = list;
			}
		}
		List<SkGridItem> gridFiltered = frame.GridFiltered;
		float num = Mathf.Max(80f, windowRect.width - 42f);
		float num2 = 58f;
		int num3 = Mathf.Max(1, Mathf.FloorToInt(num / num2));
		int num4 = Mathf.CeilToInt((float)gridFiltered.Count / (float)num3);
		bool flag = false;
		foreach (SkGridItem item2 in gridFiltered)
		{
			if (item2 != null && !string.IsNullOrEmpty(item2.Section))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			DrawSectionedGrid(frame, gridFiltered);
			return;
		}
		frame.Scroll = GUILayout.BeginScrollView(frame.Scroll, false, true, Array.Empty<GUILayoutOption>());
		if (gridFiltered.Count == 0)
		{
			GUILayout.Label("No matches.", styleTip, Array.Empty<GUILayoutOption>());
			GUILayout.EndScrollView();
			return;
		}
		float num5 = Mathf.Max(120f, windowRect.height - 240f);
		int num6 = Mathf.Max(0, Mathf.FloorToInt(frame.Scroll.y / num2) - 1);
		int num7 = Mathf.Min(num4 - 1, Mathf.CeilToInt((frame.Scroll.y + num5) / num2) + 1);
		if (num6 > 0)
		{
			GUILayout.Space((float)num6 * num2);
		}
		for (int i = num6; i <= num7; i++)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			for (int j = 0; j < num3; j++)
			{
				int num8 = i * num3 + j;
				if (num8 >= gridFiltered.Count)
				{
					GUILayout.Space(num2);
					continue;
				}
				SkGridItem skGridItem = gridFiltered[num8];
				string text2 = (((Object)(object)skGridItem.Icon == (Object)null) ? ShortLabel(skGridItem.Display ?? skGridItem.Name) : "");
				if (GUILayout.Button(new GUIContent(text2, skGridItem.HoverText), styleGridCell, (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.Width(54f),
					GUILayout.Height(54f)
				}))
				{
					SkGridItem captured = skGridItem;
					pendingAction = delegate
					{
						InvokeItem(new SkMenuItem(captured.Name, captured.OnClick, captured.Tip));
					};
				}
				if ((int)Event.current.type == 7)
				{
					Rect lastRect = GUILayoutUtility.GetLastRect();
					if ((Object)(object)skGridItem.Icon != (Object)null)
					{
						DrawSprite(lastRect, skGridItem.Icon);
					}
					if (skGridItem.IsOn != null && skGridItem.IsOn())
					{
						DrawOutline(lastRect, OnColor, 2f);
					}
				}
			}
			GUILayout.EndHorizontal();
		}
		if (num7 < num4 - 1)
		{
			GUILayout.Space((float)(num4 - 1 - num7) * num2);
		}
		GUILayout.EndScrollView();
	}

	private void DrawSectionedGrid(MenuFrame frame, List<SkGridItem> shown)
	{
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		float num = ((styleSectionBox != null) ? ((float)(styleSectionBox.padding.left + styleSectionBox.padding.right) + 8f) : 24f);
		float num2 = Mathf.Max(80f, windowRect.width - 42f - num);
		int num3 = Mathf.Max(1, Mathf.FloorToInt((num2 - 4f) / 58f));
		List<string> list = new List<string>();
		Dictionary<string, List<SkGridItem>> dictionary = new Dictionary<string, List<SkGridItem>>();
		foreach (SkGridItem item in shown)
		{
			if (item != null)
			{
				string text = (string.IsNullOrEmpty(item.Section) ? "Other" : item.Section);
				if (!dictionary.ContainsKey(text))
				{
					dictionary[text] = new List<SkGridItem>();
					list.Add(text);
				}
				dictionary[text].Add(item);
			}
		}
		frame.Scroll = GUILayout.BeginScrollView(frame.Scroll, false, true, Array.Empty<GUILayoutOption>());
		foreach (string item2 in list)
		{
			List<SkGridItem> list2 = dictionary[item2];
			GUILayout.BeginVertical(styleSectionBox, Array.Empty<GUILayoutOption>());
			GUILayout.Label(item2, styleSectionHeader, Array.Empty<GUILayoutOption>());
			for (int i = 0; i < list2.Count; i += num3)
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				for (int j = 0; j < num3; j++)
				{
					int num4 = i + j;
					if (num4 >= list2.Count)
					{
						GUILayout.Space(58f);
					}
					else
					{
						DrawGridCell(list2[num4]);
					}
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndScrollView();
	}

	private void DrawGridCell(SkGridItem cell)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Invalid comparison between Unknown and I4
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		string text = (((Object)(object)cell.Icon == (Object)null) ? ShortLabel(cell.Display ?? cell.Name) : "");
		if (GUILayout.Button(new GUIContent(text, cell.HoverText), styleGridCell, (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.Width(54f),
			GUILayout.Height(54f)
		}))
		{
			SkGridItem captured = cell;
			pendingAction = delegate
			{
				InvokeItem(new SkMenuItem(captured.Name, captured.OnClick, captured.Tip));
			};
		}
		if ((int)Event.current.type == 7)
		{
			Rect lastRect = GUILayoutUtility.GetLastRect();
			if ((Object)(object)cell.Icon != (Object)null)
			{
				DrawSprite(lastRect, cell.Icon);
			}
			if (cell.IsOn != null && cell.IsOn())
			{
				DrawOutline(lastRect, OnColor, 2f);
			}
		}
	}

	private static void DrawSprite(Rect area, Sprite sprite, float pad = 5f)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)sprite == (Object)null) && !((Object)(object)sprite.texture == (Object)null) && !(area.width <= 0f))
		{
			Rect textureRect = sprite.textureRect;
			Rect val = default(Rect);
			val = new Rect(textureRect.x / (float)((Texture)sprite.texture).width, textureRect.y / (float)((Texture)sprite.texture).height, textureRect.width / (float)((Texture)sprite.texture).width, textureRect.height / (float)((Texture)sprite.texture).height);
			Rect val2 = default(Rect);
			val2 = new Rect(area.x + pad, area.y + pad, area.width - pad * 2f, area.height - pad * 2f);
			GUI.DrawTextureWithTexCoords(val2, (Texture)(object)sprite.texture, val, true);
		}
	}

	private static void DrawOutline(Rect area, Color color, float thickness)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)texWhite == (Object)null))
		{
			Color color2 = GUI.color;
			GUI.color = color;
			GUI.DrawTexture(new Rect(area.x, area.y, area.width, thickness), (Texture)(object)texWhite);
			GUI.DrawTexture(new Rect(area.x, area.yMax - thickness, area.width, thickness), (Texture)(object)texWhite);
			GUI.DrawTexture(new Rect(area.x, area.y, thickness, area.height), (Texture)(object)texWhite);
			GUI.DrawTexture(new Rect(area.xMax - thickness, area.y, thickness, area.height), (Texture)(object)texWhite);
			GUI.color = color2;
		}
	}

	private static string ShortLabel(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return "?";
		}
		if (text.Length > 7)
		{
			return text.Substring(0, 7);
		}
		return text;
	}

	private void DrawPrompt()
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Invalid comparison between Unknown and I4
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Invalid comparison between Unknown and I4
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Invalid comparison between Unknown and I4
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Invalid comparison between Unknown and I4
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Invalid comparison between Unknown and I4
		SkPrompt skPrompt = activePrompt;
		float num = Mathf.Min(380f, Mathf.Max(220f, windowRect.width - 80f));
		float num2 = 152f;
		Rect val = default(Rect);
		val = new Rect((windowRect.width - num) / 2f, (windowRect.height - num2) / 2f, num, num2);
		GUI.Box(val, GUIContent.none, stylePrompt);
		DrawOutline(val, new Color(0.42f, 0.54f, 0.7f), 2f);
		GUILayout.BeginArea(new Rect(val.x + 14f, val.y + 12f, val.width - 28f, val.height - 24f));
		GUILayout.Label(skPrompt.Title, styleHeader, Array.Empty<GUILayoutOption>());
		if (!string.IsNullOrEmpty(skPrompt.Label))
		{
			GUILayout.Label(skPrompt.Label, styleSmall, Array.Empty<GUILayoutOption>());
		}
		GUILayout.Space(4f);
		GUI.SetNextControlName("SkPromptField");
		skPrompt.Value = GUILayout.TextField(skPrompt.Value ?? "", styleFilter, Array.Empty<GUILayoutOption>());
		if (promptNeedsFocus && (int)Event.current.type == 7)
		{
			GUI.FocusControl("SkPromptField");
			promptNeedsFocus = false;
		}
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		bool flag = GUILayout.Button(skPrompt.Accept, styleItem, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(27f) });
		GUILayout.Space(8f);
		bool flag2 = GUILayout.Button("Cancel", styleBack, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(27f) });
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		Event current = Event.current;
		if (current != null && (int)current.type == 4)
		{
			if ((int)current.keyCode == 13 || (int)current.keyCode == 271)
			{
				flag = true;
				current.Use();
			}
			else if ((int)current.keyCode == 27)
			{
				flag2 = true;
				current.Use();
			}
		}
		if (flag)
		{
			SkPrompt captured = skPrompt;
			string typed = (skPrompt.Value ?? "").Trim();
			pendingAction = delegate
			{
				ClosePrompt();
				if (captured.OnAccept != null)
				{
					try
					{
						captured.OnAccept(typed);
					}
					catch (Exception ex)
					{
						SkUtilities.Logz(new string[2] { "PROMPT", "ERROR" }, new string[1] { ex.Message }, (LogType)0);
						SkCommandProcessor.Notify("That failed: " + ex.Message);
					}
				}
			};
		}
		else if (flag2)
		{
			pendingAction = ClosePrompt;
		}
	}

	private void DrawForm()
	{
		SkForm skForm = activeForm;
		string text = null;
		if (skForm.Validate != null)
		{
			try
			{
				text = skForm.Validate(skForm);
			}
			catch (Exception)
			{
				text = null;
			}
		}
		GUILayout.Label(skForm.Title, styleHeader, Array.Empty<GUILayoutOption>());
		if (!string.IsNullOrEmpty(skForm.Warning))
		{
			GUILayout.Space(2f);
			GUILayout.Label(skForm.Warning, styleWarning, Array.Empty<GUILayoutOption>());
			GUILayout.Space(2f);
		}
		if (!string.IsNullOrEmpty(skForm.Note))
		{
			GUILayout.Label(skForm.Note, styleTip, Array.Empty<GUILayoutOption>());
		}
		string text2 = ((!string.IsNullOrEmpty(skForm.Error)) ? skForm.Error : text);
		if (!string.IsNullOrEmpty(text2))
		{
			GUILayout.Label(text2, styleError, Array.Empty<GUILayoutOption>());
		}
		GUILayout.Space(6f);
		foreach (SkFormField field in skForm.Fields)
		{
			if (field == null)
			{
				continue;
			}
			GUILayout.BeginVertical(styleSectionBox, Array.Empty<GUILayoutOption>());
			GUILayout.Label(field.Label, styleSectionHeader, Array.Empty<GUILayoutOption>());
			switch (field.Kind)
			{
			case SkFieldKind.Choice:
				DrawChoiceField(field);
				break;
			case SkFieldKind.IntSlider:
			{
				int num = Mathf.Clamp(field.IntValue, field.Min, field.Max);
				int num2 = num;
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label(num.ToString(), styleSmall, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(42f) });
				if (GUILayout.Button("-", styleBack, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(28f) }))
				{
					num2 = num - 1;
				}
				int num3 = Mathf.RoundToInt(GUILayout.HorizontalSlider((float)num, (float)field.Min, (float)field.Max, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(140f) }));
				if (num3 != num)
				{
					num2 = num3;
				}
				if (GUILayout.Button("+", styleBack, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(28f) }))
				{
					num2 = num + 1;
				}
				GUILayout.EndHorizontal();
				field.IntValue = Mathf.Clamp(num2, field.Min, field.Max);
				break;
			}
			case SkFieldKind.Text:
				field.TextValue = GUILayout.TextField(field.TextValue ?? "", styleFilter, Array.Empty<GUILayoutOption>());
				break;
			case SkFieldKind.Info:
				DrawInfoField(field);
				break;
			case SkFieldKind.Checklist:
				DrawChecklistField(field);
				break;
			case SkFieldKind.Toggle:
			{
				string text3 = (field.BoolValue ? "<color=#7CFC00>[ON]</color>" : "<color=#FF8080>[OFF]</color>");
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button(text3, styleBack, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(90f) }))
				{
					field.BoolValue = !field.BoolValue;
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				break;
			}
			}
			GUILayout.EndVertical();
		}
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		bool enabled = GUI.enabled;
		if (!string.IsNullOrEmpty(text))
		{
			GUI.enabled = false;
		}
		foreach (SkFormAction action in skForm.Actions)
		{
			if (action == null || action.Run == null)
			{
				continue;
			}
			if (GUILayout.Button(action.Label, styleItem, (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(150f),
				GUILayout.Height(28f)
			}))
			{
				SkFormAction captured = action;
				SkForm capturedForm = skForm;
				pendingAction = delegate
				{
					if (capturedForm.Validate != null)
					{
						string text4 = capturedForm.Validate(capturedForm);
						if (!string.IsNullOrEmpty(text4))
						{
							capturedForm.Error = text4;
							return;
						}
					}
					capturedForm.Error = null;
					try
					{
						captured.Run(capturedForm);
					}
					catch (Exception ex2)
					{
						SkUtilities.Logz(new string[2] { "FORM", "ERROR" }, new string[1] { ex2.Message }, (LogType)0);
						SkCommandProcessor.Notify("That failed: " + ex2.Message);
					}
					CloseForm();
				};
			}
			GUILayout.Space(6f);
		}
		GUI.enabled = enabled;
		GUILayout.FlexibleSpace();
		if (GUILayout.Button((skForm.Actions.Count == 0) ? "Close" : "Cancel", styleBack, (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.Width(110f),
			GUILayout.Height(28f)
		}))
		{
			pendingAction = CloseForm;
		}
		GUILayout.EndHorizontal();
	}

	private void DrawChoiceField(SkFormField field)
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		if (field.Options.Count > 12)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Search", styleSmall, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(52f) });
			field.Filter = GUILayout.TextField(field.Filter ?? "", styleFilter, Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("x", styleBack, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(28f) }))
			{
				field.Filter = "";
			}
			GUILayout.EndHorizontal();
		}
		string text = (field.Filter ?? "").Trim();
		field.Scroll = GUILayout.BeginScrollView(field.Scroll, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height((float)((field.Height > 0) ? field.Height : 196)) });
		int num = 0;
		for (int i = 0; i < field.Options.Count; i++)
		{
			string text2 = ((field.OptionLabels != null && i < field.OptionLabels.Count && !string.IsNullOrEmpty(field.OptionLabels[i])) ? field.OptionLabels[i] : field.Options[i]);
			if (text.Length <= 0 || field.Options[i].IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 || text2.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				num++;
				bool flag = i == field.Selected;
				if (GUILayout.Button(text2, flag ? styleTabOn : styleItem, Array.Empty<GUILayoutOption>()))
				{
					field.Selected = i;
				}
			}
		}
		if (num == 0)
		{
			GUILayout.Label("Nothing matches that.", styleTip, Array.Empty<GUILayoutOption>());
		}
		GUILayout.EndScrollView();
	}

	private void DrawChecklistField(SkFormField field)
	{
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		while (field.Checked.Count < field.Options.Count)
		{
			field.Checked.Add(item: false);
		}
		if (field.Options.Count > 12)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Search", styleSmall, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(52f) });
			field.Filter = GUILayout.TextField(field.Filter ?? "", styleFilter, Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("x", styleBack, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(28f) }))
			{
				field.Filter = "";
			}
			GUILayout.EndHorizontal();
		}
		string text = (field.Filter ?? "").Trim();
		field.Scroll = GUILayout.BeginScrollView(field.Scroll, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height((float)((field.Height > 0) ? field.Height : 196)) });
		int num = 0;
		for (int i = 0; i < field.Options.Count; i++)
		{
			string text2 = ((field.OptionLabels != null && i < field.OptionLabels.Count && !string.IsNullOrEmpty(field.OptionLabels[i])) ? field.OptionLabels[i] : field.Options[i]);
			if (text.Length <= 0 || field.Options[i].IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 || text2.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				num++;
				bool flag = field.Checked[i];
				string text3 = (flag ? "<color=#7CFC00>[ON]</color>  " : "<color=#7A8796>[OFF]</color>  ");
				if (GUILayout.Button(text3 + text2, flag ? styleTabOn : styleItem, Array.Empty<GUILayoutOption>()))
				{
					field.Checked[i] = !flag;
				}
			}
		}
		if (num == 0)
		{
			GUILayout.Label("Nothing matches that.", styleTip, Array.Empty<GUILayoutOption>());
		}
		GUILayout.EndScrollView();
	}

	private void DrawInfoField(SkFormField field)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		field.Scroll = GUILayout.BeginScrollView(field.Scroll, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height((float)((field.Height > 0) ? field.Height : 260)) });
		GUILayout.Label(field.TextValue ?? "", styleTip, Array.Empty<GUILayoutOption>());
		GUILayout.EndScrollView();
	}

	private void DrawFooter()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.FlexibleSpace();
		GUILayout.Label(((object)toggleKey).ToString() + " opens / closes, Escape closes", styleSmall, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(18f) });
		GUILayout.EndHorizontal();
	}

	private void DrawCursorTooltip()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		if ((int)Event.current.type != 7)
		{
			return;
		}
		hoverTip = GUI.tooltip ?? "";
		if (!string.IsNullOrEmpty(hoverTip))
		{
			GUIContent val = new GUIContent(hoverTip);
			float num = Mathf.Min(320f, Mathf.Max(120f, windowRect.width - 32f));
			float num2 = Mathf.Min(num, styleTooltip.CalcSize(val).x);
			float num3 = styleTooltip.CalcHeight(val, num2);
			Vector2 mousePosition = Event.current.mousePosition;
			float num4 = mousePosition.x + 16f;
			float num5 = mousePosition.y + 18f;
			if (num4 + num2 > windowRect.width - 6f)
			{
				num4 = mousePosition.x - num2 - 10f;
			}
			if (num5 + num3 > windowRect.height - 6f)
			{
				num5 = mousePosition.y - num3 - 10f;
			}
			num4 = Mathf.Max(6f, num4);
			num5 = Mathf.Max(6f, num5);
			Rect val2 = default(Rect);
			val2 = new Rect(num4, num5, num2, num3);
			GUI.Label(val2, val, styleTooltip);
			DrawOutline(val2, new Color(0.35f, 0.42f, 0.55f), 1f);
		}
	}

	private string CurrentLabelFor(SkMenuItem item)
	{
		string text = ToggleFree(item.ItemText);
		if (frames.Count > 0 && frames[frames.Count - 1].Items != null)
		{
			foreach (SkMenuItem item2 in frames[frames.Count - 1].Items)
			{
				if (item2 != null && ToggleFree(item2.ItemText) == text)
				{
					return CleanText(item2.ItemText);
				}
			}
		}
		return CleanText(item.ItemText);
	}

	public void RequestGridMenu(List<SkGridItem> grid, SkMenuSlider slider = null, string title = "Items", bool showFilter = true)
	{
		RequestGridMenu(grid, (slider == null) ? null : new List<SkMenuSlider> { slider }, title, showFilter);
	}

	public void RequestGridMenu(List<SkGridItem> grid, List<SkMenuSlider> sliders, string title = "Items", bool showFilter = true, List<SkMenuToggle> toggles = null)
	{
		if (grid != null && grid.Count != 0)
		{
			frames.Add(new MenuFrame
			{
				Title = title,
				Items = null,
				Grid = grid,
				Sliders = sliders,
				ShowFilter = showFilter,
				Toggles = toggles
			});
			pendingSlider = null;
			menuOpen = true;
		}
	}

	public void RequestSubMenu(List<SkMenuItem> subMenuOptions, SkMenuSlider slider)
	{
		pendingSlider = slider;
		RequestSubMenu(subMenuOptions);
	}

	public void RequestSubMenu(List<SkMenuItem> subMenuOptions, float refreshTime = 0f, int subWidth = 0)
	{
		if (subMenuOptions == null || subMenuOptions.Count == 0)
		{
			pendingSlider = null;
			return;
		}
		if (frames.Count > 0 && SameMenu(frames[frames.Count - 1].Items, subMenuOptions))
		{
			frames[frames.Count - 1].Items = subMenuOptions;
			if (pendingSlider != null)
			{
				frames[frames.Count - 1].Sliders = new List<SkMenuSlider> { pendingSlider };
			}
		}
		else
		{
			string title = ((frames.Count != 0) ? "Submenu" : ((selectedModule >= 0 && menuOptions != null && selectedModule < menuOptions.Count) ? ModuleTitle(menuOptions[selectedModule]) : appName));
			frames.Add(new MenuFrame
			{
				Title = title,
				Items = subMenuOptions,
				Sliders = ((pendingSlider == null) ? null : new List<SkMenuSlider> { pendingSlider })
			});
		}
		pendingSlider = null;
		menuOpen = true;
		if (logResponse)
		{
			SkUtilities.Logz(new string[2] { "CONTROLLER", "RESP" }, new string[1] { "Submenu created." }, (LogType)3);
		}
	}

	public void RequestSubMenu(SkMenu subMenuOptions, float refreshTime = 0f, int subWidth = 0)
	{
		if (subMenuOptions != null)
		{
			RequestSubMenu(subMenuOptions.FlushMenu(), refreshTime, subWidth);
		}
	}

	public void UpdateMenuOptions(List<SkBaseModule> newMenuOptions)
	{
		menuOptions = newMenuOptions;
		frames.Clear();
		selectedModule = -1;
		menuOpen = false;
	}

	private static string CleanText(string s)
	{
		if (s != null)
		{
			return s.Replace("\t►", "").Replace("►", "").Replace("\t", " ")
				.Trim();
		}
		return "";
	}

	private static string ToggleFree(string s)
	{
		return CleanText(s).Replace("[ON]", "").Replace("[OFF]", "").Trim();
	}

	private static string Display(string s)
	{
		string text = CleanText(s);
		if (text.EndsWith("[ON]"))
		{
			return text.Substring(0, text.Length - 4) + "<color=#7CFC00>[ON]</color>";
		}
		if (text.EndsWith("[OFF]"))
		{
			return text.Substring(0, text.Length - 5) + "<color=#FF8080>[OFF]</color>";
		}
		return text;
	}

	private static string ModuleTitle(SkBaseModule m)
	{
		string text = CleanText(m?.CallerEntry?.ItemText);
		object obj;
		if (text.Length <= 0)
		{
			obj = m?.ModuleName;
			if (obj == null)
			{
				return "Menu";
			}
		}
		else
		{
			obj = text;
		}
		return (string)obj;
	}

	private static bool SameMenu(List<SkMenuItem> a, List<SkMenuItem> b)
	{
		if (a == null || b == null || a.Count != b.Count)
		{
			return false;
		}
		for (int i = 0; i < a.Count; i++)
		{
			if (ToggleFree(a[i]?.ItemText) != ToggleFree(b[i]?.ItemText))
			{
				return false;
			}
		}
		return true;
	}

	private void EnsureStyles()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Expected O, but got Unknown
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Expected O, but got Unknown
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Expected O, but got Unknown
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Expected O, but got Unknown
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Expected O, but got Unknown
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Expected O, but got Unknown
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Expected O, but got Unknown
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Expected O, but got Unknown
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Expected O, but got Unknown
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Expected O, but got Unknown
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Expected O, but got Unknown
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0608: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Expected O, but got Unknown
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Expected O, but got Unknown
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Expected O, but got Unknown
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Expected O, but got Unknown
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Expected O, but got Unknown
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Expected O, but got Unknown
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_075f: Expected O, but got Unknown
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Unknown result type (might be due to invalid IL or missing references)
		//IL_0783: Expected O, but got Unknown
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bb: Expected O, but got Unknown
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Expected O, but got Unknown
		//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Expected O, but got Unknown
		//IL_0804: Unknown result type (might be due to invalid IL or missing references)
		//IL_080e: Expected O, but got Unknown
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Unknown result type (might be due to invalid IL or missing references)
		float configuredOpacity = ConfiguredOpacity;
		if (!stylesReady || !(Mathf.Abs(configuredOpacity - stylesAlpha) < 0.001f))
		{
			stylesAlpha = configuredOpacity;
			ReleaseTextures();
			texWindow = MakeTex(new Color(0.06f, 0.07f, 0.09f, configuredOpacity));
			texPanel = MakeTex(new Color(0.11f, 0.12f, 0.15f, configuredOpacity));
			texItem = MakeTex(new Color(0.17f, 0.19f, 0.23f, configuredOpacity));
			texItemHover = MakeTex(new Color(0.27f, 0.31f, 0.38f, configuredOpacity));
			texItemActive = MakeTex(new Color(0.13f, 0.42f, 0.55f, configuredOpacity));
			texAccent = MakeTex(new Color(0.1f, 0.3f, 0.4f, configuredOpacity));
			texWhite = MakeTex(Color.white);
			Color val = default(Color);
			val = new Color(0.93f, 0.94f, 0.96f);
			Color val2 = default(Color);
			val2 = new Color(0.72f, 0.75f, 0.8f);
			styleWindow = new GUIStyle(GUI.skin.window)
			{
				fontSize = 14,
				fontStyle = (FontStyle)1
			};
			styleWindow.padding = new RectOffset(12, 12, 28, 12);
			styleWindow.normal.background = texWindow;
			styleWindow.onNormal.background = texWindow;
			styleWindow.border = new RectOffset(0, 0, 0, 0);
			styleWindow.normal.textColor = val;
			styleWindow.onNormal.textColor = val;
			styleItem = new GUIStyle(GUI.skin.button)
			{
				alignment = (TextAnchor)3,
				fontSize = 13,
				fixedHeight = 27f,
				richText = true
			};
			styleItem.padding = new RectOffset(10, 8, 3, 3);
			styleItem.border = new RectOffset(0, 0, 0, 0);
			styleItem.margin = new RectOffset(0, 0, 1, 1);
			Paint(styleItem, texItem, texItemHover, texItemActive, val);
			styleBack = new GUIStyle(styleItem)
			{
				alignment = (TextAnchor)4,
				fontSize = 12,
				fixedHeight = 25f,
				richText = true
			};
			styleHeader = new GUIStyle(GUI.skin.label)
			{
				fontSize = 14,
				fontStyle = (FontStyle)1
			};
			styleHeader.normal.textColor = val;
			styleTip = new GUIStyle(GUI.skin.label)
			{
				fontSize = 12,
				wordWrap = true
			};
			styleError = new GUIStyle(GUI.skin.label)
			{
				fontSize = 12,
				wordWrap = true,
				fontStyle = (FontStyle)1
			};
			styleError.normal.textColor = new Color(1f, 0.55f, 0.45f);
			styleWarning = new GUIStyle(GUI.skin.label)
			{
				fontSize = 14,
				wordWrap = true,
				fontStyle = (FontStyle)1
			};
			styleWarning.normal.textColor = new Color(1f, 0.38f, 0.32f);
			styleWarning.padding = new RectOffset(0, 0, 4, 4);
			styleTip.normal.textColor = new Color(0.95f, 0.9f, 0.65f);
			styleSmall = new GUIStyle(GUI.skin.label)
			{
				fontSize = 12
			};
			styleSmall.normal.textColor = val2;
			styleFilter = new GUIStyle(GUI.skin.textField)
			{
				fontSize = 13,
				fixedHeight = 25f
			};
			styleFilter.normal.background = texPanel;
			styleFilter.focused.background = texPanel;
			styleFilter.hover.background = texPanel;
			styleFilter.border = new RectOffset(0, 0, 0, 0);
			styleFilter.padding = new RectOffset(6, 6, 4, 4);
			styleFilter.normal.textColor = val;
			styleFilter.focused.textColor = Color.white;
			styleTab = new GUIStyle(GUI.skin.button)
			{
				alignment = (TextAnchor)4,
				fontSize = 13,
				fixedHeight = 30f,
				richText = true
			};
			styleTab.padding = new RectOffset(10, 10, 4, 4);
			styleTab.border = new RectOffset(0, 0, 0, 0);
			styleTab.margin = new RectOffset(0, 2, 0, 0);
			Paint(styleTab, texPanel, texItemHover, texItemActive, val2);
			styleTabOn = new GUIStyle(styleTab)
			{
				fontStyle = (FontStyle)1,
				fontSize = 14
			};
			Paint(styleTabOn, texAccent, texAccent, texItemActive, Color.white);
			styleGridCell = new GUIStyle(GUI.skin.button)
			{
				alignment = (TextAnchor)4,
				fontSize = 10,
				richText = false,
				wordWrap = true
			};
			styleGridCell.border = new RectOffset(0, 0, 0, 0);
			styleGridCell.padding = new RectOffset(2, 2, 2, 2);
			styleGridCell.margin = new RectOffset(0, 4, 4, 0);
			Paint(styleGridCell, texItem, texItemHover, texItemActive, val2);
			styleTooltip = new GUIStyle(GUI.skin.label)
			{
				fontSize = 12,
				wordWrap = true,
				richText = true
			};
			texPrompt = MakeTex(new Color(0.09f, 0.11f, 0.15f, 1f));
			texTooltip = MakeTex(new Color(0.03f, 0.04f, 0.06f, 0.98f));
			styleTooltip.normal.background = texTooltip;
			styleTooltip.border = new RectOffset(0, 0, 0, 0);
			styleTooltip.padding = new RectOffset(8, 8, 6, 6);
			styleTooltip.normal.textColor = new Color(0.96f, 0.93f, 0.72f);
			stylePrompt = new GUIStyle(GUI.skin.box);
			stylePrompt.normal.background = texPrompt;
			stylePrompt.border = new RectOffset(0, 0, 0, 0);
			styleSectionBox = new GUIStyle(GUI.skin.box);
			styleSectionBox.normal.background = texPanel;
			styleSectionBox.border = new RectOffset(0, 0, 0, 0);
			styleSectionBox.padding = new RectOffset(8, 8, 6, 8);
			styleSectionBox.margin = new RectOffset(0, 0, 0, 8);
			styleSectionHeader = new GUIStyle(GUI.skin.label)
			{
				fontSize = 12,
				fontStyle = (FontStyle)1
			};
			styleSectionHeader.normal.textColor = new Color(0.62f, 0.78f, 0.95f);
			styleSectionHeader.padding = new RectOffset(2, 2, 0, 4);
			styleClose = new GUIStyle(GUI.skin.button)
			{
				alignment = (TextAnchor)4,
				fontSize = 14,
				fontStyle = (FontStyle)1
			};
			styleClose.border = new RectOffset(0, 0, 0, 0);
			styleClose.padding = new RectOffset(0, 0, 0, 0);
			Paint(styleClose, texPanel, MakeTex(new Color(0.65f, 0.16f, 0.16f, configuredOpacity)), texItemActive, val);
			stylesReady = true;
		}
	}

	private static void ReleaseTextures()
	{
		Texture2D[] array = (Texture2D[])(object)new Texture2D[9] { texWindow, texPanel, texItem, texItemHover, texItemActive, texAccent, texWhite, texTooltip, texPrompt };
		foreach (Texture2D val in array)
		{
			if ((Object)(object)val != (Object)null)
			{
				Object.DestroyImmediate((Object)(object)val);
			}
		}
		texWindow = (texPanel = (texItem = (texItemHover = (texItemActive = (texAccent = (texWhite = (texTooltip = (texPrompt = null))))))));
	}

	private static void Paint(GUIStyle style, Texture2D normal, Texture2D hover, Texture2D active, Color text)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		style.normal.background = normal;
		style.hover.background = hover;
		style.active.background = active;
		style.focused.background = normal;
		style.onNormal.background = normal;
		style.onHover.background = hover;
		style.onActive.background = active;
		style.normal.textColor = text;
		style.hover.textColor = Color.white;
		style.active.textColor = Color.white;
		style.focused.textColor = text;
		style.onNormal.textColor = text;
		style.onHover.textColor = Color.white;
		style.onActive.textColor = Color.white;
	}

	private void OnGUI()
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Invalid comparison between Unknown and I4
		if (menuOptions == null || menuOptions.Count == 0)
		{
			UpdateMenuOptions(SkModuleController.GetOptions());
		}
		else if (menuOpen)
		{
			EnsureStyles();
			if (!windowPlaced)
			{
				windowRect.x = 24f;
				windowRect.y = Mathf.Max(24f, ((float)Screen.height - windowRect.height) / 2f);
				windowPlaced = true;
			}
			GUI.color = Color.white;
			windowRect = GUILayout.Window(49000, windowRect, new WindowFunction(DrawWindow), appName + "  v1.1.7", styleWindow, Array.Empty<GUILayoutOption>());
			windowRect.x = Mathf.Clamp(windowRect.x, 0f, Mathf.Max(0f, (float)Screen.width - windowRect.width));
			windowRect.y = Mathf.Clamp(windowRect.y, 0f, Mathf.Max(0f, (float)Screen.height - windowRect.height));
			Event current = Event.current;
			if (current != null && ((int)current.type == 0 || (int)current.type == 1) && !windowRect.Contains(current.mousePosition))
			{
				current.Use();
			}
		}
	}

	private void DrawWindow(int windowID)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)Event.current.type == 7)
			{
				GUI.tooltip = string.Empty;
			}
			bool flag = activePrompt != null;
			bool enabled = GUI.enabled;
			if (flag)
			{
				GUI.enabled = false;
			}
			DrawCloseButton();
			if (activeForm != null)
			{
				DrawForm();
				if (flag)
				{
					GUI.enabled = enabled;
					DrawPrompt();
				}
				DrawCursorTooltip();
				if (!flag)
				{
					GUI.DragWindow(new Rect(0f, 0f, Mathf.Max(0f, windowRect.width - 36f), 26f));
				}
				return;
			}
			DrawTabs();
			DrawFrameHeader();
			DrawFilterRow();
			DrawSliderRow();
			DrawItemList();
			DrawFooter();
			if (flag)
			{
				GUI.enabled = enabled;
				DrawPrompt();
			}
			DrawCursorTooltip();
			if (!flag)
			{
				GUI.DragWindow(new Rect(0f, 0f, Mathf.Max(0f, windowRect.width - 36f), 26f));
			}
		}
		catch (ArgumentException)
		{
		}
		catch (Exception ex2)
		{
			SkUtilities.Logz(new string[2] { "CONTROLLER", "ERROR" }, new string[1] { ex2.Message }, (LogType)3);
		}
		finally
		{
			if (pendingAction != null)
			{
				Action action = pendingAction;
				pendingAction = null;
				try
				{
					action();
				}
				catch (Exception ex3)
				{
					SkUtilities.Logz(new string[2] { "CONTROLLER", "ERROR" }, new string[1] { ex3.Message }, (LogType)3);
				}
			}
		}
	}
}
