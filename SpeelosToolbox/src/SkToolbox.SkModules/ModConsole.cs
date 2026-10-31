using System;
using System.Collections.Generic;
using System.Globalization;
using BepInEx.Configuration;
using SkToolbox.Configuration;
using SkToolbox.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SkToolbox.SkModules;

internal class ModConsole : SkBaseModule, IModule
{
	private class History
	{
		public List<string> history = new List<string>();

		private int index;

		private string current;

		public void Add(string item)
		{
			history.Add(item);
			index = 0;
		}

		public string Fetch(string current, bool next)
		{
			if (index == 0)
			{
				this.current = current;
			}
			if (history.Count == 0)
			{
				return current;
			}
			index += ((!next) ? 1 : (-1));
			if (history.Count + index < 0 || history.Count + index > history.Count - 1)
			{
				index = 0;
				return this.current;
			}
			return history[history.Count + index];
		}
	}

	internal bool conWriteToFile;

	private string consoleLastMessage = string.Empty;

	private string lastOutput = string.Empty;

	private string chatInLastMessage = string.Empty;

	private Rect EnemyWindow;

	private Rect ConsoleOutput;

	private GUIStyle ConsoleOutputStyle;

	private History consoleHistory = new History();

	public List<string> consoleOutputHistory = new List<string>();

	private Vector2 scrollPosition;

	private Rect rectCoords = new Rect(5f, 20f, 125f, 20f);

	private Rect rectEnemy = new Rect(5f, 415f, 375f, 50f);

	private Rect rectConsoleOutput = new Rect(0f, 0f, 500f, 500f);

	private bool anncounced1;

	private bool anncounced2;

	private int ConsoleOutputMaxHistory = 250;

	public Dictionary<string, string> AliasList = new Dictionary<string, string>();

	public List<string> PrefabList = new List<string>();

	public List<string> ItemList = new List<string>();

	private readonly Dictionary<char, string> hotkeys = new Dictionary<char, string>();

	private readonly List<char> typedChars = new List<char>();

	private Keyboard typedFrom;

	private List<Character> nearbyCharacters = new List<Character>();

	private Terminal terminal;

	public ModConsole()
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		ModuleName = "CC Controller";
		Loading();
	}

	public void Start()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		SkCommandProcessor.ConsoleOpt = this;
		SkCommandPatcher.InitPatch();
		BeginMenu();
		try
		{
			ConsoleOutputStyle = new GUIStyle();
			ConsoleOutputStyle.wordWrap = true;
			BuildAliases();
			BuildHotkeys();
			if (SkConfigEntry.CScrollableLimit != null)
			{
				ConsoleOutputMaxHistory = SkConfigEntry.CScrollableLimit.Value;
			}
		}
		catch (Exception)
		{
		}
		Ready();
	}

	public void BeginMenu()
	{
		SkMenu skMenu = new SkMenu();
		skMenu.AddItem("Reload Toolbox", ReloadMenu);
		skMenu.AddItem("Unload Toolbox", UnloadMenu);
		skMenu.AddItem("Open Log Folder", OpenLogFolder);
		base.MenuOptions = skMenu;
	}

	private static SkMenuController.SkGridItem Cell(string section, string label, string tip, Sprite icon, Action run)
	{
		return new SkMenuController.SkGridItem
		{
			Name = label,
			Display = label,
			Tip = label + "  -  " + tip,
			Icon = icon,
			Section = section,
			Scope = SkMenuController.SkScope.Client,
			OnClick = delegate
			{
				run();
			}
		};
	}

	internal void ShowGrid()
	{
		List<SkMenuController.SkGridItem> list = new List<SkMenuController.SkGridItem>();
		list.Add(Cell("Display", "Field of View", "Widen or narrow the camera", SkIcons.First("Torch"), ShowFovForm));
		list.Add(Cell("Display", "Frame Limit", "Cap the frame rate", SkIcons.First("Feathers"), ShowFpsForm));
		list.Add(Cell("Display", "Detail Distance", "How far away the game keeps full detail", SkIcons.First("Stone", "Wood"), ShowLodForm));
		list.Add(Cell("Display", "Snow Buildup", "Add or clear snow on nearby building pieces", SkIcons.First("FreezeGland", "WolfPelt"), ShowSnowForm));
		list.Add(Cell("Display", "Free Fly Camera", "Detach the camera for screenshots. Run it again to come back", SkIcons.First("FeatherCape", "Feathers"), delegate
		{
			SkRun.CmdNotify("freefly", "Toggled the free fly camera.");
		}));
		list.Add(new SkMenuController.SkGridItem
		{
			Name = "Debug Mode",
			Display = "Debug Mode",
			Tip = "Debug Mode  -  the game's own developer mode, which some other cheats rely on",
			Icon = SkIcons.First("Wishbone"),
			Section = "Display",
			Scope = SkMenuController.SkScope.Client,
			OnClick = delegate
			{
				SkRun.CmdNotify("debugmode", "Toggled debug mode.");
			},
			IsOn = () => Player.m_debugMode
		});
		list.Add(Cell("Toolbox", "Reload Toolbox", "Unload and load the toolbox again", SkIcons.First("Hammer"), ReloadMenu));
		list.Add(Cell("Toolbox", "Unload Toolbox", "Shut the toolbox down until the next launch", SkIcons.First("Flint", "Stone"), UnloadMenu));
		list.Add(Cell("Toolbox", "Open Log Folder", "Open the folder holding the log files", SkIcons.First("Coins"), OpenLogFolder));
		List<SkMenuController.SkGridItem> grid = list;
		SkMC.RequestGridMenu(grid, null, "System", showFilter: false, null);
	}

	private void ShowFovForm()
	{
		SkMenuController.SkForm skForm = new SkMenuController.SkForm
		{
			Title = "Field of View",
			Note = "The game's default is 65. Higher shows more of the world and feels faster."
		};
		skForm.Fields.Add(new SkMenuController.SkFormField
		{
			Id = "fov",
			Label = "Degrees",
			Kind = SkMenuController.SkFieldKind.IntSlider,
			Min = 40,
			Max = 120,
			IntValue = CurrentFov()
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Apply",
			Run = delegate(SkMenuController.SkForm f)
			{
				SkRun.CmdNotify("fov " + f.Field("fov").IntValue, "Field of view: " + f.Field("fov").IntValue);
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Back to 65",
			Run = delegate
			{
				SkRun.CmdNotify("fov 65", "Field of view back to 65.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private static int CurrentFov()
	{
		Camera main = Camera.main;
		if (!((Object)(object)main != (Object)null))
		{
			return 65;
		}
		return Mathf.Clamp(Mathf.RoundToInt(main.fieldOfView), 40, 120);
	}

	private void ShowFpsForm()
	{
		SkMenuController.SkForm skForm = new SkMenuController.SkForm
		{
			Title = "Frame Limit",
			Note = "Caps the frame rate. This is saved with the graphics settings, the same as changing it in the options screen."
		};
		skForm.Fields.Add(new SkMenuController.SkFormField
		{
			Id = "fps",
			Label = "Frames per second",
			Kind = SkMenuController.SkFieldKind.IntSlider,
			Min = 30,
			Max = 360,
			IntValue = 60
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Apply",
			Run = delegate(SkMenuController.SkForm f)
			{
				SkRun.CmdNotify("maxfps " + f.Field("fps").IntValue, "Frame limit: " + f.Field("fps").IntValue);
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Uncapped",
			Run = delegate
			{
				SkRun.CmdNotify("maxfps 0", "Frame limit removed.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowLodForm()
	{
		SkMenuController.SkForm skForm = new SkMenuController.SkForm
		{
			Title = "Detail Distance",
			Note = "How far away the game keeps full detail. 1 is normal; higher looks better and costs performance."
		};
		skForm.Fields.Add(new SkMenuController.SkFormField
		{
			Id = "bias",
			Label = "Bias",
			Kind = SkMenuController.SkFieldKind.IntSlider,
			Min = 1,
			Max = 10,
			IntValue = Mathf.Clamp(Mathf.RoundToInt(QualitySettings.lodBias), 1, 10)
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Apply",
			Run = delegate(SkMenuController.SkForm f)
			{
				SkRun.CmdNotify("lodbias " + f.Field("bias").IntValue, "Detail distance: " + f.Field("bias").IntValue);
			}
		});
		SkMC.ShowForm(skForm);
	}

	private void ShowSnowForm()
	{
		SkMenuController.SkForm skForm = new SkMenuController.SkForm
		{
			Title = "Snow Buildup",
			Note = "Adds or removes snow on every building piece nearby. Adding only affects pieces that can hold snow."
		};
		skForm.Fields.Add(new SkMenuController.SkFormField
		{
			Id = "amount",
			Label = "Amount %",
			Kind = SkMenuController.SkFieldKind.IntSlider,
			Min = 5,
			Max = 100,
			IntValue = 100
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Add snow",
			Run = delegate(SkMenuController.SkForm f)
			{
				SkRun.Cmd("snow " + Fraction(f.Field("amount").IntValue));
				SkCommandProcessor.Notify("Added snow to nearby pieces.");
			}
		});
		skForm.Actions.Add(new SkMenuController.SkFormAction
		{
			Label = "Clear snow",
			Run = delegate
			{
				SkRun.Cmd("snow -1");
				SkCommandProcessor.Notify("Cleared snow from nearby pieces.");
			}
		});
		SkMC.ShowForm(skForm);
	}

	private static string Fraction(int percent)
	{
		return ((float)percent / 100f).ToString("0.00", CultureInfo.InvariantCulture);
	}

	public void OpenLogFolder()
	{
		SkUtilities.Logz(new string[2] { "CMD", "REQ" }, new string[1] { "Opening Log Directory" }, (LogType)3);
		Application.OpenURL(Application.persistentDataPath);
	}

	public void ReloadMenu()
	{
		SkUtilities.Logz(new string[3] { "BASE", "CMD", "REQ" }, new string[2] { "UNLOADING CONTROLLERS AND MODULES...", "SKTOOLBOX RELOAD REQUESTED." }, (LogType)3);
		SkLoader.Reload();
	}

	public void UnloadMenu()
	{
		SkUtilities.Logz(new string[3] { "BASE", "CMD", "REQ" }, new string[2] { "SKTOOLBOX UNLOAD REQUESTED.", "UNLOADING CONTROLLERS AND MODULES..." }, (LogType)3);
		if (((SkConfigEntry.CScrollable != null) & SkConfigEntry.CScrollable.Value) && (Object)(object)Console.instance != (Object)null)
		{
			((Component)((Terminal)Console.instance).m_output).gameObject.SetActive(true);
		}
		SkLoader.Unload();
	}

	public void HandleConsole()
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)terminal == (Object)null && (Object)(object)Console.instance != (Object)null)
		{
			terminal = Console.instance.GetPrivateProperty<Terminal>("m_terminalInstance");
		}
		if ((Object)(object)Console.instance != (Object)null && (Object)(object)terminal != (Object)null)
		{
			if ((SkConfigEntry.CScrollable != null) & SkConfigEntry.CScrollable.Value)
			{
				List<string> privateField = Console.instance.GetPrivateField<List<string>>("m_chatBuffer");
				if (privateField != null && privateField.Count > 0)
				{
					try
					{
						while (privateField.Count > 0)
						{
							consoleOutputHistory.Add(privateField[0]);
							privateField.RemoveAt(0);
						}
					}
					catch (Exception)
					{
					}
					if (privateField.Count == 0)
					{
						((TMP_Text)((Terminal)Console.instance).m_output).text = string.Empty;
					}
					scrollPosition = new Vector2(0f, 2.1474836E+09f);
					while (consoleOutputHistory.Count > ConsoleOutputMaxHistory)
					{
						consoleOutputHistory.RemoveAt(0);
					}
				}
			}
			if (((Component)((Terminal)Console.instance).m_chatWindow).gameObject.activeInHierarchy)
			{
				string text = ((TMP_InputField)((Terminal)Console.instance).m_input).text;
				if (Input.GetKeyDown((KeyCode)127) || Input.GetKeyDown((KeyCode)8) || Input.GetKeyDown((KeyCode)27))
				{
					consoleLastMessage = string.Empty;
				}
				if (!text.Equals(string.Empty) && !text.Equals(consoleLastMessage))
				{
					consoleLastMessage = text;
				}
				if ((Input.GetKeyDown((KeyCode)13) || Input.GetKeyDown((KeyCode)271)) && text.Equals(string.Empty) && !consoleLastMessage.Equals(string.Empty))
				{
					consoleHistory.Add(consoleLastMessage);
					consoleLastMessage = string.Empty;
				}
			}
			if (Input.GetKeyDown((KeyCode)47) && SkConfigEntry.COpenConsoleWithSlash != null && SkConfigEntry.COpenConsoleWithSlash.Value && !Console.IsVisible() && ((Object)(object)Chat.instance == (Object)null || !Chat.instance.IsChatDialogWindowVisible()) && !TextInput.IsVisible())
			{
				((Component)((Terminal)Console.instance).m_chatWindow).gameObject.SetActive(true);
				((Terminal)Console.instance).m_input.ActivateInputField();
				((TMP_InputField)((Terminal)Console.instance).m_input).caretPosition = ((TMP_InputField)((Terminal)Console.instance).m_input).text.Length;
			}
		}
		base.ModuleStatus = SkUtilities.Status.Ready;
	}

	public void HandleChat()
	{
		if (!((Object)(object)Chat.instance != (Object)null))
		{
			return;
		}
		if (((Component)((Terminal)Chat.instance).m_chatWindow).gameObject.activeInHierarchy)
		{
			string text = ((TMP_InputField)((Terminal)Chat.instance).m_input).text;
			string text2 = ((TMP_Text)((Terminal)Chat.instance).m_output).text;
			if ((Input.GetKeyDown((KeyCode)127) || Input.GetKeyDown((KeyCode)8) || Input.GetKeyDown((KeyCode)27)) && ((SkConfigEntry.CAllowExecuteOnClear != null && !SkConfigEntry.CAllowExecuteOnClear.Value) || SkConfigEntry.CAllowExecuteOnClear == null))
			{
				chatInLastMessage = string.Empty;
			}
			if (!text.Equals(string.Empty) && !text.Equals(consoleLastMessage))
			{
				chatInLastMessage = text;
			}
			if (text.Equals(string.Empty) && !chatInLastMessage.Equals(string.Empty))
			{
				SkCommandProcessor.ProcessCommands(chatInLastMessage, SkCommandProcessor.LogTo.Chat);
				chatInLastMessage = string.Empty;
			}
		}
		if (Input.GetKeyDown((KeyCode)47) && (Object)(object)Player.m_localPlayer != (Object)null && SkConfigEntry.COpenConsoleWithSlash != null && !SkConfigEntry.COpenConsoleWithSlash.Value && SkConfigEntry.COpenChatWithSlash != null && SkConfigEntry.COpenChatWithSlash.Value && !Console.IsVisible() && !Chat.instance.IsChatDialogWindowVisible() && !TextInput.IsVisible() && !Minimap.InTextInput() && !Menu.IsVisible())
		{
			Chat.instance.SetPrivateField("m_hideTimer", 0f);
			((Component)((Terminal)Chat.instance).m_chatWindow).gameObject.SetActive(true);
			((Component)((Terminal)Chat.instance).m_input).gameObject.SetActive(true);
			((Terminal)Chat.instance).m_input.ActivateInputField();
		}
	}

	public void LoadConsoleCustomizations()
	{
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		if (SkConfigEntry.CAllowLookCustomizations != null && SkConfigEntry.CAllowLookCustomizations.Value)
		{
			try
			{
				ConsoleOutputStyle = new GUIStyle();
				ConsoleOutputStyle.wordWrap = true;
				int num = (int)((TMP_Text)((Terminal)Console.instance).m_output).fontSize;
				string value = SkConfigEntry.CConsoleFont.Value;
				string text = string.Empty;
				string[] pathsToOSFonts = Font.GetPathsToOSFonts();
				string[] array = pathsToOSFonts;
				foreach (string text2 in array)
				{
					if (text2.IndexOf(value, StringComparison.OrdinalIgnoreCase) != -1)
					{
						text = text2;
						break;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					Font val = new Font(text);
					Font font = Font.CreateDynamicFontFromOSFont(value, num);
					TMP_FontAsset font2 = TMP_FontAsset.CreateFontAsset(val);
					((TMP_InputField)((Terminal)Console.instance).m_input).textComponent.font = font2;
					((TMP_Text)((Terminal)Console.instance).m_output).font = font2;
					ConsoleOutputStyle.font = font;
				}
				Color color = ((Graphic)((Terminal)Console.instance).m_output).color;
				Color color2 = ((Graphic)((TMP_InputField)((Terminal)Console.instance).m_input).textComponent).color;
				Color selectionColor = ((TMP_InputField)((Terminal)Console.instance).m_input).selectionColor;
				Color white = Color.white;
				try
				{
					num = SkConfigEntry.CConsoleFontSize.Value;
					value = SkConfigEntry.CConsoleFont.Value;
					ColorUtility.TryParseHtmlString(SkConfigEntry.CConsoleOutputTextColor.Value, out color);
					ColorUtility.TryParseHtmlString(SkConfigEntry.CConsoleInputTextColor.Value, out color2);
					ColorUtility.TryParseHtmlString(SkConfigEntry.CConsoleSelectionColor.Value, out selectionColor);
					ColorUtility.TryParseHtmlString(SkConfigEntry.CConsoleCaretColor.Value, out white);
				}
				catch (Exception ex)
				{
					SkUtilities.Logz(new string[1] { "ERR" }, new string[2] { "Failed to load something from the config.", ex.Message }, (LogType)2);
				}
				((Graphic)((TMP_InputField)((Terminal)Console.instance).m_input).textComponent).color = color2;
				((TMP_InputField)((Terminal)Console.instance).m_input).selectionColor = selectionColor;
				((TMP_InputField)((Terminal)Console.instance).m_input).caretColor = white;
				((TMP_InputField)((Terminal)Console.instance).m_input).customCaretColor = true;
				((TMP_Text)((Terminal)Console.instance).m_output).fontSize = num;
				((Graphic)((Terminal)Console.instance).m_output).color = color;
				if ((SkConfigEntry.CScrollable != null) & SkConfigEntry.CScrollable.Value)
				{
					ConsoleOutputStyle.fontSize = num;
					ConsoleOutputStyle.normal.textColor = color;
				}
			}
			catch (Exception ex2)
			{
				SkUtilities.Logz(new string[1] { "ERR" }, new string[2] { "Failed to set when customizing console style.", ex2.Message }, (LogType)2);
			}
		}
		else if ((SkConfigEntry.CScrollable != null) & SkConfigEntry.CScrollable.Value)
		{
			Font font3 = Font.CreateDynamicFontFromOSFont("Consolas", 18);
			ConsoleOutputStyle.fontSize = 18;
			ConsoleOutputStyle.font = font3;
			ConsoleOutputStyle.normal.textColor = Color.white;
		}
		((Component)((Terminal)Console.instance).m_output).gameObject.SetActive(false);
		SkCommandProcessor.ProcessCommand("/clear", SkCommandProcessor.LogTo.Console);
	}

	private void OnGUI()
	{
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)Player.m_localPlayer != (Object)null)
		{
			if (SkCommandProcessor.bDetectEnemies && nearbyCharacters.Count > 0)
			{
				EnemyWindow = GUILayout.Window(39999, rectEnemy, new WindowFunction(ProcessEnemies), "Enemy Information", Array.Empty<GUILayoutOption>());
			}
			if (SkCommandProcessor.bCoords)
			{
				Vector3 position = ((Component)Player.m_localPlayer).transform.position;
				GUI.Label(rectCoords, "Coords: " + Mathf.RoundToInt(position.x) + "/" + Mathf.RoundToInt(position.z));
			}
		}
		if ((Object)(object)Console.instance != (Object)null && Console.IsVisible())
		{
			Rect val = default(Rect);
			val.x = 7f;
			val.y = 1f;
			Rect rect = ((TMP_Text)((Terminal)Console.instance).m_output).rectTransform.rect;
			val.width = rect.width - 7f;
			rect = ((TMP_Text)((Terminal)Console.instance).m_output).rectTransform.rect;
			val.height = rect.height;
			rectConsoleOutput = val;
			if (SkVersionChecker.VersionCurrent())
			{
				ConsoleOutput = GUILayout.Window(39979, rectConsoleOutput, new WindowFunction(ProcessConsoleOutput), " Speelo's Toolbox (" + SkVersionChecker.currentVersion?.ToString() + ") by Skrip (DS)", ConsoleOutputStyle, Array.Empty<GUILayoutOption>());
			}
			else
			{
				ConsoleOutput = GUILayout.Window(39979, rectConsoleOutput, new WindowFunction(ProcessConsoleOutput), " Speelo's Toolbox, based on SkToolbox by Skrip ► New Version Available on NexusMods!\t► Current: " + SkVersionChecker.currentVersion?.ToString() + " Latest: " + SkVersionChecker.latestVersion, ConsoleOutputStyle, Array.Empty<GUILayoutOption>());
			}
		}
	}

	private void ProcessConsoleOutput(int windowID)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		if (!Cursor.visible)
		{
			Cursor.visible = true;
			Cursor.lockState = (CursorLockMode)0;
		}
		if (SkConfigEntry.CConsoleFontSize != null)
		{
			GUILayout.Space((float)SkConfigEntry.CConsoleFontSize.Value);
		}
		else
		{
			GUILayout.Space(18f);
		}
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, Array.Empty<GUILayoutOption>());
		foreach (string item in consoleOutputHistory)
		{
			GUILayout.Label(item, ConsoleOutputStyle, Array.Empty<GUILayoutOption>());
			GUILayout.Space(7f);
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	public void BuildPrefabs()
	{
		if (PrefabList.Count != 0 || !((Object)(object)ZNetScene.instance != (Object)null))
		{
			return;
		}
		try
		{
			foreach (GameObject value in ZNetScene.instance.GetPrivateField<Dictionary<int, GameObject>>("m_namedPrefabs").Values)
			{
				PrefabList.Add(Utils.GetPrefabName(value));
				ItemDrop component = value.GetComponent<ItemDrop>();
				if ((Object)(object)component != (Object)null)
				{
					ItemList.Add(Utils.GetPrefabName(value));
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void Update()
	{
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)Console.instance != (Object)null && SkConfigEntry.CConsoleEnabled.Value && !Console.instance.IsConsoleEnabled())
		{
			Console.SetConsoleEnabled(true);
		}
		try
		{
			HandleConsole();
		}
		catch (Exception ex)
		{
			base.ModuleStatus = SkUtilities.Status.Error;
			SkUtilities.Logz(new string[1] { "ERR" }, new string[3] { "Error detected!", ex.Message, ex.Source }, (LogType)2);
			SkCommandProcessor.PrintOut("An error was detected. You may need to restart your game.", SkCommandProcessor.LogTo.Console);
		}
		if ((SkConfigEntry.CAllowChatCommandInput != null && SkConfigEntry.CAllowChatCommandInput.Value) || SkConfigEntry.CAllowChatCommandInput == null)
		{
			try
			{
				HandleChat();
			}
			catch (Exception)
			{
				base.ModuleStatus = SkUtilities.Status.Error;
			}
		}
		if (!anncounced1)
		{
			if ((Object)(object)Console.instance != (Object)null && (Object)(object)Player.m_localPlayer == (Object)null)
			{
				SkCommandProcessor.Announce();
				LoadConsoleCustomizations();
				SkCommandProcessor.InitCommands();
				anncounced1 = true;
			}
		}
		else if ((Object)(object)Player.m_localPlayer != (Object)null)
		{
			anncounced1 = false;
		}
		if (!anncounced2)
		{
			if ((Object)(object)Chat.instance != (Object)null)
			{
				SkCommandProcessor.Announce();
				LoadConsoleCustomizations();
				anncounced2 = true;
			}
		}
		else if ((Object)(object)Chat.instance == (Object)null)
		{
			anncounced2 = false;
		}
		ProcessAutoRun();
		ProcessHotkeys();
		if (SkCommandProcessor.bTeleport && Input.GetKeyDown((KeyCode)96))
		{
			try
			{
				Ray val = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit val2 = default(RaycastHit);
				if (Physics.Raycast(val, out val2))
				{
					Vector3 point = val2.point;
					((Component)Player.m_localPlayer).transform.position = point;
					((Character)Player.m_localPlayer).Message((MessageType)1, "Warp!", 0, (Sprite)null, false);
				}
			}
			catch (Exception)
			{
			}
		}
		if (!SkCommandProcessor.bDetectEnemies)
		{
			return;
		}
		try
		{
			List<Character> allCharacters = Character.GetAllCharacters();
			if (allCharacters.Count > 0)
			{
				foreach (Character item in allCharacters)
				{
					if ((Object)(object)item != (Object)null && (Object)(object)Player.m_localPlayer != (Object)null && !item.IsDead() && !item.IsPlayer())
					{
						if (Vector3.Distance(((Component)item).transform.position, ((Component)Player.m_localPlayer).transform.position) < (float)SkCommandProcessor.bDetectRange)
						{
							if (!nearbyCharacters.Contains(item))
							{
								nearbyCharacters.Add(item);
							}
						}
						else if (nearbyCharacters.Contains(item))
						{
							nearbyCharacters.Remove(item);
						}
					}
					else if (nearbyCharacters.Contains(item))
					{
						nearbyCharacters.Remove(item);
					}
				}
				if (nearbyCharacters.Count > 0)
				{
					List<Character> list = new List<Character>(nearbyCharacters);
					foreach (Character item2 in list)
					{
						if (!allCharacters.Contains(item2))
						{
							nearbyCharacters.Remove(item2);
						}
					}
				}
			}
			if (nearbyCharacters.Count > 0 && SkCommandProcessor.btDetectEnemiesSwitch)
			{
				((Character)Player.m_localPlayer).Message((MessageType)2, "Enemy nearby!", 0, (Sprite)null, false);
				SkCommandProcessor.btDetectEnemiesSwitch = false;
			}
			else if (nearbyCharacters.Count == 0)
			{
				SkCommandProcessor.btDetectEnemiesSwitch = true;
			}
		}
		catch (Exception)
		{
		}
	}

	private void OnDestroy()
	{
		if (typedFrom != null)
		{
			typedFrom.onTextInput -= OnTextInput;
			typedFrom = null;
		}
		if (((SkConfigEntry.CScrollable != null) & SkConfigEntry.CScrollable.Value) && (Object)(object)Console.instance != (Object)null)
		{
			((Component)((Terminal)Console.instance).m_output).gameObject.SetActive(true);
		}
		if (SkCommandPatcher.Harmony == null)
		{
			return;
		}
		try
		{
			SkCommandPatcher.Harmony.UnpatchSelf();
		}
		catch (Exception ex)
		{
			SkUtilities.Logz(new string[2] { "CONSOLE", "UNPATCH" }, new string[1] { "UnpatchSelf failed: " + ex.Message }, (LogType)2);
		}
		finally
		{
			SkCommandPatcher.Harmony = null;
			SkCommandPatcher.InitComplete = false;
		}
	}

	private void ProcessHotkeys()
	{
		try
		{
			SyncTextInput();
			if (typedChars.Count == 0)
			{
				return;
			}
			if (hotkeys.Count == 0 || !HotkeysAllowed())
			{
				typedChars.Clear();
				return;
			}
			char[] array = typedChars.ToArray();
			typedChars.Clear();
			char[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				char key = array2[i];
				if (hotkeys.TryGetValue(key, out var value))
				{
					SkUtilities.Logz(new string[2] { "CONTROLLER", "HOTKEY" }, new string[1] { key + " -> " + value }, (LogType)3);
					SkCommandProcessor.ProcessCommands(value, SkCommandProcessor.LogTo.Console);
				}
			}
		}
		catch (Exception ex)
		{
			typedChars.Clear();
			SkUtilities.Logz(new string[2] { "CONTROLLER", "HOTKEY" }, new string[1] { "Hotkey failed: " + ex.Message }, (LogType)2);
		}
	}

	private void SyncTextInput()
	{
		Keyboard current = Keyboard.current;
		if (current != typedFrom)
		{
			if (typedFrom != null)
			{
				typedFrom.onTextInput -= OnTextInput;
			}
			typedFrom = current;
			if (current != null)
			{
				current.onTextInput += OnTextInput;
			}
		}
	}

	private void OnTextInput(char c)
	{
		if (typedChars.Count < 32)
		{
			typedChars.Add(c);
		}
	}

	private static bool HotkeysAllowed()
	{
		if ((Object)(object)Player.m_localPlayer == (Object)null)
		{
			return false;
		}
		if (Console.IsVisible())
		{
			return false;
		}
		if ((Object)(object)Chat.instance != (Object)null && Chat.instance.HasFocus())
		{
			return false;
		}
		if (TextInput.IsVisible())
		{
			return false;
		}
		if (Minimap.InTextInput())
		{
			return false;
		}
		if (Menu.IsVisible())
		{
			return false;
		}
		if (InventoryGui.IsVisible())
		{
			return false;
		}
		if (StoreGui.IsVisible())
		{
			return false;
		}
		if (ZInput.VirtualKeyboardOpen)
		{
			return false;
		}
		if (SkMenuController.IsOpen)
		{
			return false;
		}
		return true;
	}

	private void ProcessAutoRun()
	{
		if (SkConfigEntry.CAutoRunComplete)
		{
			return;
		}
		if (SkConfigEntry.CAutoRun != null && SkConfigEntry.CAutoRun.Value)
		{
			if (!((Object)(object)Player.m_localPlayer != (Object)null) || !((Object)(object)Chat.instance != (Object)null) || !((Object)(object)Console.instance != (Object)null))
			{
				return;
			}
			try
			{
				SkUtilities.Logz(new string[2] { "CONTROLLER", "AUTORUN" }, new string[1] { "Command List: " + SkConfigEntry.CAutoRunCommand.Value }, (LogType)3);
				SkCommandProcessor.PrintOut("==> AutoRun enabled! Command line: " + SkConfigEntry.CAutoRunCommand.Value, SkCommandProcessor.LogTo.Console);
				SkCommandProcessor.ProcessCommands(SkConfigEntry.CAutoRunCommand.Value, SkCommandProcessor.LogTo.Console);
				return;
			}
			catch (Exception)
			{
				SkUtilities.Logz(new string[1] { "CONTROLLER" }, new string[1] { "AutoRun Failed. Something went wrong. Command line: " + SkConfigEntry.CAutoRunCommand.Value }, (LogType)2);
				SkCommandProcessor.PrintOut("==> AutoRun Failed. Something went wrong. Command line: " + SkConfigEntry.CAutoRunCommand.Value, SkCommandProcessor.LogTo.Console);
				return;
			}
			finally
			{
				SkConfigEntry.CAutoRunComplete = true;
			}
		}
		SkConfigEntry.CAutoRunComplete = true;
	}

	private void BuildHotkeys()
	{
		hotkeys.Clear();
		ConfigEntry<string>[] array = new ConfigEntry<string>[15]
		{
			SkConfigEntry.CHotkey1,
			SkConfigEntry.CHotkey2,
			SkConfigEntry.CHotkey3,
			SkConfigEntry.CHotkey4,
			SkConfigEntry.CHotkey5,
			SkConfigEntry.CHotkey6,
			SkConfigEntry.CHotkey7,
			SkConfigEntry.CHotkey8,
			SkConfigEntry.CHotkey9,
			SkConfigEntry.CHotkey10,
			SkConfigEntry.CHotkey11,
			SkConfigEntry.CHotkey12,
			SkConfigEntry.CHotkey13,
			SkConfigEntry.CHotkey14,
			SkConfigEntry.CHotkey15
		};
		for (int i = 0; i < array.Length; i++)
		{
			int slot = i + 1;
			try
			{
				if (array[i] == null || string.IsNullOrEmpty(array[i].Value))
				{
					continue;
				}
				string value = array[i].Value;
				int num = value.IndexOf(':');
				if (num < 0)
				{
					RejectHotkey(slot, "needs a colon between the key and the command, as in \"l: /fly\".");
					continue;
				}
				string text = value.Substring(0, num).Trim();
				string text2 = value.Substring(num + 1).Trim();
				if (text.Length != 1)
				{
					RejectHotkey(slot, (text.Length == 0) ? "has no key before the colon." : ("the key must be a single character, not \"" + text + "\"."));
					continue;
				}
				char c = text[0];
				if (c < ' ' || c > '~')
				{
					RejectHotkey(slot, "'" + c + "' is not a printable ASCII character.");
				}
				else if (text2.Length == 0)
				{
					RejectHotkey(slot, "has no command after the colon.");
				}
				else if (hotkeys.ContainsKey(c))
				{
					RejectHotkey(slot, "'" + c + "' is already bound; this slot is ignored.");
				}
				else
				{
					hotkeys.Add(c, text2);
				}
			}
			catch (Exception ex)
			{
				RejectHotkey(slot, ex.Message);
			}
		}
		if (hotkeys.Count > 0)
		{
			SkUtilities.Logz(new string[2] { "CONTROLLER", "HOTKEYS" }, new string[1] { hotkeys.Count + " hotkey(s) bound." }, (LogType)3);
		}
	}

	private static void RejectHotkey(int slot, string why)
	{
		string text = "Hotkey " + slot + " ignored: " + why;
		SkUtilities.Logz(new string[2] { "CONTROLLER", "HOTKEYS" }, new string[1] { text }, (LogType)2);
		SkCommandProcessor.PrintOut("==> " + text, SkCommandProcessor.LogTo.Console);
	}

	private void BuildAliases()
	{
		try
		{
			if (SkConfigEntry.CAlias1 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias1.Value))
			{
				string[] array = SkConfigEntry.CAlias1.Value.Split(':');
				if (array.Length == 2)
				{
					AliasList.Add(array[0], array[1]);
				}
			}
			if (SkConfigEntry.CAlias2 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias2.Value))
			{
				string[] array2 = SkConfigEntry.CAlias2.Value.Split(':');
				if (array2.Length == 2)
				{
					AliasList.Add(array2[0], array2[1]);
				}
			}
			if (SkConfigEntry.CAlias3 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias3.Value))
			{
				string[] array3 = SkConfigEntry.CAlias3.Value.Split(':');
				if (array3.Length == 2)
				{
					AliasList.Add(array3[0], array3[1]);
				}
			}
			if (SkConfigEntry.CAlias4 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias4.Value))
			{
				string[] array4 = SkConfigEntry.CAlias4.Value.Split(':');
				if (array4.Length == 2)
				{
					AliasList.Add(array4[0], array4[1]);
				}
			}
			if (SkConfigEntry.CAlias5 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias5.Value))
			{
				string[] array5 = SkConfigEntry.CAlias5.Value.Split(':');
				if (array5.Length == 2)
				{
					AliasList.Add(array5[0], array5[1]);
				}
			}
			if (SkConfigEntry.CAlias6 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias6.Value))
			{
				string[] array6 = SkConfigEntry.CAlias6.Value.Split(':');
				if (array6.Length == 2)
				{
					AliasList.Add(array6[0], array6[1]);
				}
			}
			if (SkConfigEntry.CAlias7 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias7.Value))
			{
				string[] array7 = SkConfigEntry.CAlias7.Value.Split(':');
				if (array7.Length == 2)
				{
					AliasList.Add(array7[0], array7[1]);
				}
			}
			if (SkConfigEntry.CAlias8 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias8.Value))
			{
				string[] array8 = SkConfigEntry.CAlias8.Value.Split(':');
				if (array8.Length == 2)
				{
					AliasList.Add(array8[0], array8[1]);
				}
			}
			if (SkConfigEntry.CAlias9 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias9.Value))
			{
				string[] array9 = SkConfigEntry.CAlias9.Value.Split(':');
				if (array9.Length == 2)
				{
					AliasList.Add(array9[0], array9[1]);
				}
			}
			if (SkConfigEntry.CAlias10 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias10.Value))
			{
				string[] array10 = SkConfigEntry.CAlias10.Value.Split(':');
				if (array10.Length == 2)
				{
					AliasList.Add(array10[0], array10[1]);
				}
			}
			if (SkConfigEntry.CAlias11 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias11.Value))
			{
				string[] array11 = SkConfigEntry.CAlias11.Value.Split(':');
				if (array11.Length == 2)
				{
					AliasList.Add(array11[0], array11[1]);
				}
			}
			if (SkConfigEntry.CAlias12 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias12.Value))
			{
				string[] array12 = SkConfigEntry.CAlias12.Value.Split(':');
				if (array12.Length == 2)
				{
					AliasList.Add(array12[0], array12[1]);
				}
			}
			if (SkConfigEntry.CAlias13 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias13.Value))
			{
				string[] array13 = SkConfigEntry.CAlias13.Value.Split(':');
				if (array13.Length == 2)
				{
					AliasList.Add(array13[0], array13[1]);
				}
			}
			if (SkConfigEntry.CAlias14 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias14.Value))
			{
				string[] array14 = SkConfigEntry.CAlias14.Value.Split(':');
				if (array14.Length == 2)
				{
					AliasList.Add(array14[0], array14[1]);
				}
			}
			if (SkConfigEntry.CAlias15 != null && !string.IsNullOrEmpty(SkConfigEntry.CAlias15.Value))
			{
				string[] array15 = SkConfigEntry.CAlias15.Value.Split(':');
				if (array15.Length == 2)
				{
					AliasList.Add(array15[0], array15[1]);
				}
			}
			if (AliasList != null && AliasList.Count > 0)
			{
				SkUtilities.Logz(new string[2] { "CONTROLLER", "ALIASES" }, new string[1] { AliasList.Count + " alias(es) loaded." }, (LogType)3);
			}
		}
		catch (Exception ex)
		{
			SkCommandProcessor.PrintOut("Something failed while loading command aliases! Check your config file. " + ex.Message, SkCommandProcessor.LogTo.DebugConsole);
		}
	}

	private void ProcessEnemies(int WindowID)
	{
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)Player.m_localPlayer != (Object)null))
		{
			return;
		}
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		List<Character> list = nearbyCharacters;
		if (list != null && list.Count > 0)
		{
			Vector3 position = ((Component)Player.m_localPlayer).transform.position;
			foreach (Character nearbyCharacter in nearbyCharacters)
			{
				if ((Object)(object)nearbyCharacter != (Object)null)
				{
					float num = Vector3.Distance(position, ((Component)nearbyCharacter).transform.position);
					if (num > 15f)
					{
						GUI.color = Color.green;
					}
					else if (num > 10f && num < 15f)
					{
						GUI.color = Color.yellow;
					}
					else if (num > 5f && num < 10f)
					{
						GUI.color = Color.yellow + Color.red;
					}
					else if (num > 0f && num < 5f)
					{
						GUI.color = Color.red;
					}
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Name: " + nearbyCharacter.GetHoverName(), Array.Empty<GUILayoutOption>());
					GUILayout.Label("HP: " + Mathf.RoundToInt(nearbyCharacter.GetHealth()) + "/" + nearbyCharacter.GetMaxHealth() + " | Level: " + nearbyCharacter.GetLevel() + " | Dist: " + Mathf.RoundToInt(num), Array.Empty<GUILayoutOption>());
					GUILayout.EndHorizontal();
					GUI.color = Color.white;
				}
			}
		}
		GUILayout.EndVertical();
		GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
	}
}
