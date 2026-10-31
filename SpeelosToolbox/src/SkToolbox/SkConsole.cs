using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SkToolbox;

internal class SkConsole : MonoBehaviour
{
	public static System.Version SkConsoleVersion = new System.Version(1, 1, 1);

	public KeyCode toggleKey = (KeyCode)285;

	public bool openOnStart;

	public float toggleThresholdSeconds = 0.5f;

	public bool restrictLogCount = true;

	public int maxLogCount = 3000;

	public int logFontSize = 18;

	private int logLineNumber;

	public float scaleFactor = 1f;

	public static bool writeToFile = false;

	private string logSavePath;

	public static bool cursorUnlock = true;

	private static string outputString = string.Empty;

	private static readonly GUIContent submitLabel = new GUIContent("Submit", "Submit the input.");

	private static readonly GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");

	private static readonly GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

	private const int margin = 350;

	private const string windowTitle = "Console [Speelo's Toolbox]";

	private static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>
	{
		{
			(LogType)1,
			Color.white
		},
		{
			(LogType)0,
			Color.red
		},
		{
			(LogType)4,
			Color.red
		},
		{
			(LogType)3,
			Color.white
		},
		{
			(LogType)2,
			Color.yellow
		}
	};

	private bool isCollapsed;

	private bool isVisible;

	private readonly List<Log> logs = new List<Log>();

	private readonly ConcurrentQueue<Log> queuedLogs = new ConcurrentQueue<Log>();

	private Vector2 scrollPosition;

	private readonly Rect titleBarRect = new Rect(0f, 0f, 10000f, 20f);

	private float windowX = 350f;

	private float windowY = 350f;

	private readonly Dictionary<LogType, bool> logTypeFilters = new Dictionary<LogType, bool>
	{
		{
			(LogType)1,
			false
		},
		{
			(LogType)0,
			false
		},
		{
			(LogType)4,
			false
		},
		{
			(LogType)3,
			true
		},
		{
			(LogType)2,
			false
		}
	};

	private void OnDisable()
	{
	}

	private void OnEnable()
	{
	}

	private void OnGUI()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		if (isVisible)
		{
			GUI.matrix = Matrix4x4.Scale(Vector3.one * scaleFactor);
			float num = (float)Screen.width / (scaleFactor * 2f) - 700f;
			float num2 = (float)Screen.height / scaleFactor - 700f;
			Rect val = default(Rect);
			val = new Rect(windowX, windowY, num, num2);
			Rect val2 = GUILayout.Window(8675309, val, new WindowFunction(DrawWindow), "Console [Speelo's Toolbox]", Array.Empty<GUILayoutOption>());
			windowX = val2.x;
			windowY = val2.y;
			if (cursorUnlock)
			{
				unlockCursor();
			}
		}
	}

	private void Start()
	{
		logSavePath = Application.persistentDataPath + "/!SpeelosToolbox Console Log.txt";
		if (openOnStart)
		{
			isVisible = true;
		}
	}

	private void Update()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		UpdateQueuedLogs();
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (Input.GetKeyDown(toggleKey))
		{
			isVisible = !isVisible;
		}
	}

	private void DrawLog(Log log, GUIStyle logStyle, GUIStyle badgeStyle)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		GUI.contentColor = logTypeColors[log.type];
		if (isCollapsed)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label(log.GetTruncatedMessage(), logStyle, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.Label(log.count.ToString(), GUI.skin.box, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
		}
		else
		{
			for (int i = 0; i < log.count; i++)
			{
				GUILayout.Label(log.GetTruncatedMessage(), logStyle, Array.Empty<GUILayoutOption>());
			}
		}
		GUI.contentColor = Color.white;
	}

	private void DrawLogList()
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Invalid comparison between Unknown and I4
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		GUIStyle box = GUI.skin.box;
		box.fontSize = logFontSize;
		try
		{
			box.font = Font.CreateDynamicFontFromOSFont("Consolas.ttf", 12);
		}
		catch (Exception)
		{
		}
		GUIStyle label = GUI.skin.label;
		label.fontSize = logFontSize;
		try
		{
			label.font = Font.CreateDynamicFontFromOSFont("Consolas.ttf", 12);
		}
		catch (Exception)
		{
		}
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, Array.Empty<GUILayoutOption>());
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		IEnumerable<Log> enumerable = logs.Where(IsLogVisible);
		foreach (Log item in enumerable)
		{
			DrawLog(item, label, box);
		}
		GUILayout.EndVertical();
		Rect lastRect = GUILayoutUtility.GetLastRect();
		GUILayout.EndScrollView();
		Rect lastRect2 = GUILayoutUtility.GetLastRect();
		if ((int)Event.current.type == 7 && IsScrolledToBottom(lastRect, lastRect2))
		{
			ScrollToBottom();
		}
	}

	private void DrawToolbar()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		if (GUILayout.Button(clearLabel, Array.Empty<GUILayoutOption>()))
		{
			logs.Clear();
		}
		foreach (LogType value in Enum.GetValues(typeof(LogType)))
		{
			LogType key = value;
			bool flag = logTypeFilters[key];
			string text = ((object)key).ToString();
			logTypeFilters[key] = GUILayout.Toggle(flag, text, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
			GUILayout.Space(20f);
		}
		isCollapsed = GUILayout.Toggle(isCollapsed, collapseLabel, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
		GUILayout.EndHorizontal();
	}

	private void DrawInput()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		outputString = GUILayout.TextField(outputString, 2000, Array.Empty<GUILayoutOption>());
		if (GUILayout.Button(submitLabel, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }))
		{
			SkCommandProcessor.ProcessCommands(outputString, SkCommandProcessor.LogTo.Console);
			outputString = string.Empty;
		}
		GUILayout.EndHorizontal();
	}

	private void DrawWindow(int windowID)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		DrawLogList();
		DrawInput();
		DrawToolbar();
		GUI.DragWindow(titleBarRect);
	}

	private Log? GetLastLog()
	{
		if (logs.Count == 0)
		{
			return null;
		}
		return logs.Last();
	}

	private void UpdateQueuedLogs()
	{
		Log result;
		while (queuedLogs.TryDequeue(out result))
		{
			ProcessLogItem(result);
		}
	}

	public void AddLogEntry(string message)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		Log log = default(Log);
		log.count = 1;
		log.message = message;
		log.stackTrace = "";
		log.type = (LogType)3;
		Log item = log;
		queuedLogs.Enqueue(item);
	}

	private void HandleLogThreaded(string message, string stackTrace, LogType type)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		Log log = default(Log);
		log.count = 1;
		log.message = message;
		log.stackTrace = stackTrace;
		log.type = type;
		Log item = log;
		if (writeToFile)
		{
			logLineNumber++;
			using StreamWriter streamWriter = new StreamWriter(logSavePath, append: true);
			streamWriter.WriteLine(logLineNumber + "\t:\t" + message);
		}
		queuedLogs.Enqueue(item);
	}

	private void ProcessLogItem(Log log)
	{
		Log? lastLog = GetLastLog();
		if (lastLog.HasValue && log.Equals(lastLog.Value))
		{
			log.count = lastLog.Value.count + 1;
			logs[logs.Count - 1] = log;
		}
		else
		{
			logs.Add(log);
			TrimExcessLogs();
		}
	}

	private bool IsLogVisible(Log log)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return logTypeFilters[log.type];
	}

	private bool IsScrolledToBottom(Rect innerScrollRect, Rect outerScrollRect)
	{
		float height = innerScrollRect.height;
		float num = outerScrollRect.height - (float)GUI.skin.box.padding.vertical;
		if (num > height)
		{
			return true;
		}
		return Mathf.Approximately(height, scrollPosition.y + num);
	}

	private void ScrollToBottom()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		scrollPosition = new Vector2(0f, 2.1474836E+09f);
	}

	private void TrimExcessLogs()
	{
		if (restrictLogCount)
		{
			int num = logs.Count - maxLogCount;
			if (num > 0)
			{
				logs.RemoveRange(0, num);
			}
		}
	}

	private void unlockCursor()
	{
		Cursor.lockState = (CursorLockMode)0;
		Cursor.visible = true;
	}
}
