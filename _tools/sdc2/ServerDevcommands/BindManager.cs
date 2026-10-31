using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using HarmonyLib;
using Service;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ServerDevcommands;

[HarmonyPatch]
public class BindManager
{
	public static string Pattern = "binds*.yaml";

	public static string Folder = "binds";

	public const string DefaultFile = "binds.yaml";

	private static readonly List<CommandBind> Binds = new List<CommandBind>();

	private static readonly List<CommandBind> WheelBinds = new List<CommandBind>();

	private static readonly List<CommandBind> TemporaryBinds = new List<CommandBind>();

	public static bool ToBeSaved = false;

	private static string Mode = "";

	private static string GetFolderPath()
	{
		string text = Path.Combine(Paths.ConfigPath, Folder);
		if (!Directory.Exists(text))
		{
			return Paths.ConfigPath;
		}
		return text;
	}

	public static void AddBind(string keys, string command)
	{
		CommandBind commandBind = FromData(new BindData
		{
			key = keys,
			command = command
		}, temporary: false);
		commandBind.IsDefault = true;
		if (commandBind.MouseWheel)
		{
			WheelBinds.Add(commandBind);
		}
		else
		{
			Binds.Add(commandBind);
		}
		ToBeSaved = true;
	}

	public static void RemoveBind(string key)
	{
		KeyCode keyCode;
		if (key == "wheel")
		{
			WheelBinds.RemoveAll((CommandBind bind) => bind.IsDefault);
		}
		else if (TryParse(key, out keyCode))
		{
			Binds.RemoveAll((CommandBind bind) => bind.Required != null && bind.Required.Contains(keyCode) && bind.IsDefault);
			WheelBinds.RemoveAll((CommandBind bind) => bind.Required != null && bind.Required.Contains(keyCode) && bind.IsDefault);
		}
		ToBeSaved = true;
	}

	public static void SetTemporaryBind(string keys, string mode, string command, string offCommand)
	{
		string command2 = command;
		BindData data = new BindData
		{
			key = keys,
			modifiers = mode,
			command = command2,
			offCommand = offCommand
		};
		TemporaryBinds.RemoveAll((CommandBind b) => b.Command == command2);
		WheelBinds.RemoveAll((CommandBind b) => b.Command == command2);
		Binds.RemoveAll((CommandBind b) => b.Command == command2);
		CommandBind commandBind = FromData(data, temporary: true);
		commandBind.IsDefault = true;
		if (commandBind.MouseWheel || commandBind.Required.Count != 0)
		{
			TemporaryBinds.Add(commandBind);
			if (commandBind.MouseWheel)
			{
				WheelBinds.Add(commandBind);
			}
			else if (commandBind.Required.Count > 0)
			{
				Binds.Add(commandBind);
			}
		}
	}

	public static void ClearBinds()
	{
		Binds.Clear();
		WheelBinds.Clear();
		ToBeSaved = true;
	}

	public static CommandBind FromData(BindData data, bool temporary)
	{
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		CommandBind commandBind = new CommandBind
		{
			Command = data.command,
			OffCommand = data.offCommand,
			Keys = data.key,
			Temporary = temporary
		};
		if (data.keys != null)
		{
			string[] array = Parse.Split(data.keys);
			foreach (string text in array)
			{
				if (text == "wheel")
				{
					commandBind.MouseWheel = true;
				}
				else if (text == "-wheel")
				{
					commandBind.MouseWheel = false;
				}
				else if (text.StartsWith("-"))
				{
					CommandBind commandBind2 = commandBind;
					if (commandBind2.Banned == null)
					{
						commandBind2.Banned = new List<KeyCode>();
					}
					if (TryParse(text.Substring(1), out var keyCode))
					{
						commandBind.Banned.Add(keyCode);
					}
				}
				else
				{
					CommandBind commandBind2 = commandBind;
					if (commandBind2.Required == null)
					{
						commandBind2.Required = new List<KeyCode>();
					}
					if (TryParse(text, out var keyCode2))
					{
						commandBind.Required.Add(keyCode2);
					}
				}
			}
		}
		if (data.state != null)
		{
			string[] array = Parse.Split(data.state);
			foreach (string text2 in array)
			{
				if (text2 == "wheel")
				{
					commandBind.MouseWheel = true;
				}
				else if (text2 == "-wheel")
				{
					commandBind.MouseWheel = false;
				}
				else if (text2.StartsWith("-"))
				{
					CommandBind commandBind2 = commandBind;
					if (commandBind2.BannedState == null)
					{
						commandBind2.BannedState = new List<string>();
					}
					commandBind.BannedState.Add(text2.Substring(1));
				}
				else
				{
					CommandBind commandBind2 = commandBind;
					if (commandBind2.RequiredState == null)
					{
						commandBind2.RequiredState = new List<string>();
					}
					commandBind.RequiredState.Add(text2);
				}
			}
		}
		if (data.key != null)
		{
			string[] array = Parse.Split(data.key);
			foreach (string text3 in array)
			{
				if (text3 == "wheel")
				{
					commandBind.MouseWheel = true;
				}
				else if (text3 == "-wheel")
				{
					commandBind.MouseWheel = false;
				}
				else if (text3.StartsWith("-"))
				{
					CommandBind commandBind2 = commandBind;
					if (commandBind2.Banned == null)
					{
						commandBind2.Banned = new List<KeyCode>();
					}
					if (Enum.TryParse<KeyCode>(text3.Substring(1), ignoreCase: true, out KeyCode result))
					{
						commandBind.Banned.Add(result);
					}
				}
				else
				{
					CommandBind commandBind2 = commandBind;
					if (commandBind2.Required == null)
					{
						commandBind2.Required = new List<KeyCode>();
					}
					if (Enum.TryParse<KeyCode>(text3, ignoreCase: true, out KeyCode result2))
					{
						commandBind.Required.Add(result2);
					}
				}
			}
		}
		if (data.modifiers != null)
		{
			string[] array = Parse.Split(data.modifiers);
			foreach (string text4 in array)
			{
				if (text4 == "wheel")
				{
					commandBind.MouseWheel = true;
					continue;
				}
				if (text4 == "-wheel")
				{
					commandBind.MouseWheel = false;
					continue;
				}
				CommandBind commandBind2;
				if (text4.StartsWith("-"))
				{
					commandBind2 = commandBind;
					if (commandBind2.Banned == null)
					{
						commandBind2.Banned = new List<KeyCode>();
					}
					if (Enum.TryParse<KeyCode>(text4.Substring(1), ignoreCase: true, out KeyCode result3))
					{
						commandBind.Banned.Add(result3);
						continue;
					}
					commandBind2 = commandBind;
					if (commandBind2.BannedState == null)
					{
						commandBind2.BannedState = new List<string>();
					}
					commandBind.BannedState.Add(text4.Substring(1));
					continue;
				}
				commandBind2 = commandBind;
				if (commandBind2.Required == null)
				{
					commandBind2.Required = new List<KeyCode>();
				}
				if (Enum.TryParse<KeyCode>(text4, ignoreCase: true, out KeyCode result4))
				{
					commandBind.Required.Add(result4);
					continue;
				}
				commandBind2 = commandBind;
				if (commandBind2.RequiredState == null)
				{
					commandBind2.RequiredState = new List<string>();
				}
				commandBind.RequiredState.Add(text4);
			}
		}
		if (!temporary && commandBind.Command.StartsWith("_"))
		{
			commandBind.Temporary = true;
		}
		List<string>? requiredState = commandBind.RequiredState;
		if (requiredState != null && requiredState.Contains("unbound"))
		{
			commandBind.MouseWheel = false;
			commandBind.Required = new List<KeyCode>();
			ToBeSaved = true;
		}
		return commandBind;
	}

	private static bool TryParse(string str, out KeyCode keyCode)
	{
		if (Enum.TryParse<KeyCode>(str, ignoreCase: true, out keyCode))
		{
			return true;
		}
		Log.Warning("Failed to parse " + str + " as KeyCode.");
		return false;
	}

	public static BindData ToData(string command)
	{
		string[] array = command.Split(' ');
		BindData bindData = new BindData
		{
			key = array[0]
		};
		if (bindData.key.ToLower() == "none")
		{
			bindData.key = "wheel";
		}
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string[] array3 = array2[i].Split('=');
			if (array3.Length != 1 && array3[0] == "keys")
			{
				bindData.modifiers = array3[1];
			}
		}
		IEnumerable<string> values = from arg in array.Skip(1)
			where !arg.StartsWith("keys=", StringComparison.OrdinalIgnoreCase)
			select arg;
		bindData.command = string.Join(" ", values);
		return bindData;
	}

	public static BindData ToData(CommandBind commandBind)
	{
		List<string> list = new List<string>();
		if (commandBind.Required != null)
		{
			list.AddRange(commandBind.Required.Select((KeyCode key) => ((object)(KeyCode)(ref key)).ToString().ToLower()));
		}
		if (commandBind.Banned != null)
		{
			list.AddRange(commandBind.Banned.Select((KeyCode key) => "-" + ((object)(KeyCode)(ref key)).ToString().ToLower()));
		}
		if (commandBind.MouseWheel)
		{
			list.Add("wheel");
		}
		List<string> list2 = new List<string>();
		if (commandBind.RequiredState != null)
		{
			list2.AddRange(commandBind.RequiredState.Select((string state) => state.ToString().ToLower()));
		}
		if (commandBind.BannedState != null)
		{
			list2.AddRange(commandBind.BannedState.Select((string state) => "-" + state.ToString().ToLower()));
		}
		return new BindData
		{
			keys = string.Join(", ", list),
			state = string.Join(", ", list2),
			command = commandBind.Command,
			offCommand = commandBind.OffCommand
		};
	}

	private static void ImportBinds()
	{
		BindData[] array = (from bind in Terminal.m_bindList.Select(ToData)
			where !bind.command.StartsWith("_", StringComparison.OrdinalIgnoreCase)
			select bind).ToArray();
		if (array.Length != 0)
		{
			string contents = Yaml.Serializer().Serialize(array);
			File.WriteAllText(Path.Combine(GetFolderPath(), "binds.yaml"), contents);
			Log.Info($"Importing {array.Length} bind data.");
		}
	}

	public static void Init()
	{
		ZInput.s_keyCodeToKeyMap[(KeyCode)294] = (Key)112;
		ZInput.s_keyCodeToKeyMap[(KeyCode)295] = (Key)113;
		ZInput.s_keyCodeToKeyMap[(KeyCode)296] = (Key)114;
		ZInput.s_keyCodeToKeyMap[(KeyCode)670] = (Key)115;
		ZInput.s_keyCodeToKeyMap[(KeyCode)671] = (Key)116;
		ZInput.s_keyCodeToKeyMap[(KeyCode)672] = (Key)117;
		ZInput.s_keyCodeToKeyMap[(KeyCode)673] = (Key)118;
		ZInput.s_keyCodeToKeyMap[(KeyCode)674] = (Key)119;
		ZInput.s_keyCodeToKeyMap[(KeyCode)675] = (Key)120;
		ZInput.s_keyCodeToKeyMap[(KeyCode)676] = (Key)121;
		ZInput.s_keyCodeToKeyMap[(KeyCode)677] = (Key)122;
		ZInput.s_keyCodeToKeyMap[(KeyCode)678] = (Key)123;
		Yaml.ConsolidateDefaultFile(Paths.ConfigPath, Folder, "binds.yaml");
		FromFiles();
		Yaml.SetupWatcher(Paths.ConfigPath, Pattern, Folder, FromFiles);
	}

	public static void Load()
	{
		FromFiles();
		if (Binds.Count == 0 && WheelBinds.Count == 0 && !Yaml.AnyFileExists(Paths.ConfigPath, Pattern, Folder))
		{
			ImportBinds();
		}
		Terminal.m_bindList.Clear();
		Terminal.m_binds.Clear();
	}

	public static void ToFile()
	{
		ToBeSaved = false;
		List<BindData> list = new List<BindData>();
		list.AddRange(Binds.Where((CommandBind b) => !b.Temporary && b.IsDefault).Select(ToData));
		list.AddRange(WheelBinds.Where((CommandBind b) => !b.Temporary && b.IsDefault).Select(ToData));
		List<BindData> graph = list;
		string contents = Yaml.Serializer().Serialize(graph);
		File.WriteAllText(Path.Combine(GetFolderPath(), "binds.yaml"), contents);
	}

	public static void FromFiles()
	{
		Binds.Clear();
		WheelBinds.Clear();
		Yaml.LoadListsFromDirectory<BindData>(Paths.ConfigPath, Pattern, Folder, LoadBind);
		Log.Info($"Reloading {WheelBinds.Count + Binds.Count} bind data.");
		Binds.AddRange(TemporaryBinds.Where((CommandBind b) => !b.MouseWheel && b.Required.Count > 0));
		WheelBinds.AddRange(TemporaryBinds.Where((CommandBind b) => b.MouseWheel));
	}

	private static void LoadBind(string file, BindData data)
	{
		CommandBind commandBind = FromData(data, temporary: false);
		commandBind.IsDefault = Yaml.IsDefaultFile(file, Folder, "binds.yaml");
		commandBind.Source = file;
		if (commandBind.MouseWheel || commandBind.Required.Count != 0)
		{
			if (commandBind.MouseWheel)
			{
				WheelBinds.Add(commandBind);
			}
			else
			{
				Binds.Add(commandBind);
			}
		}
	}

	public static List<CommandBind> GetBestKeyCommands()
	{
		return GetBestCommands(Binds);
	}

	public static List<CommandBind> GetBestWheelCommands()
	{
		return GetBestCommands(WheelBinds);
	}

	private static List<CommandBind> GetBestCommands(List<CommandBind> binds)
	{
		int num = 0;
		List<CommandBind> list = new List<CommandBind>();
		foreach (CommandBind bind in binds)
		{
			if (!IsValid(bind))
			{
				continue;
			}
			int count = bind.Required.Count;
			if (count >= num)
			{
				if (count > num)
				{
					list.Clear();
				}
				num = count;
				list.Add(bind);
			}
		}
		return list;
	}

	public static int GetBestKeyCount()
	{
		return GetKeyCount(Binds);
	}

	public static int GetBestWheelCount()
	{
		return GetKeyCount(WheelBinds);
	}

	private static int GetKeyCount(List<CommandBind> binds)
	{
		int num = 0;
		foreach (CommandBind bind in binds)
		{
			if (IsValid(bind))
			{
				int num2 = ((bind.Required != null) ? bind.Required.Count : 0);
				if (num2 > num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	public static bool CouldKeyExecute()
	{
		return CouldExecute(Binds);
	}

	public static bool CouldWheelExecute()
	{
		return CouldExecute(WheelBinds);
	}

	private static bool CouldExecute(List<CommandBind> binds)
	{
		foreach (CommandBind bind in binds)
		{
			if (IsValid(bind))
			{
				return true;
			}
		}
		return false;
	}

	public static void SetMode(string mode)
	{
		Mode = mode;
	}

	private static bool IsValid(CommandBind bind)
	{
		object obj;
		if (!(Mode == ""))
		{
			obj = Mode;
		}
		else
		{
			Player localPlayer = Player.m_localPlayer;
			obj = ((localPlayer != null && ((Character)localPlayer).InPlaceMode()) ? "build" : "");
		}
		string mode = (string)obj;
		if (!bind.MouseWheel && (bind.Required == null || bind.Required.Count == 0))
		{
			return false;
		}
		if (bind.Required.Any((KeyCode key) => !GetKey(key)))
		{
			return false;
		}
		if (bind.Banned != null && bind.Banned.Any(GetKey))
		{
			return false;
		}
		if (bind.RequiredState != null && bind.RequiredState.All((string state) => state != mode))
		{
			return false;
		}
		if (bind.BannedState != null && bind.BannedState.Any((string state) => state == mode))
		{
			return false;
		}
		if (bind.Validity == CommandValidity.Invalid)
		{
			return false;
		}
		if (bind.Validity == CommandValidity.Unknown)
		{
			bind.Validity = (HasRegisteredCommand(bind.Command) ? CommandValidity.Valid : CommandValidity.Invalid);
			if (bind.Validity == CommandValidity.Invalid)
			{
				_ = bind.Source + "\n" + bind.Command;
				Log.Warning("Tried to execute unknown bind command from '" + bind.Source + "': " + bind.Command);
				return false;
			}
		}
		return true;
	}

	private static bool HasRegisteredCommand(string command)
	{
		return (from text in MultiCommands.Split(command).Select(Aliasing.Plain).SelectMany(MultiCommands.Split)
			select text.Split(' ')[0]).Any(Terminal.commands.ContainsKey);
	}

	public static bool GetKey(KeyCode key)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if ((int)key == 326)
		{
			return ZInput.GetKey((KeyCode)327, true);
		}
		if ((int)key == 327)
		{
			return ZInput.GetKey((KeyCode)326, true);
		}
		if ((int)key == 328)
		{
			return Input.GetKey((KeyCode)328);
		}
		if ((int)key == 329)
		{
			return Input.GetKey((KeyCode)329);
		}
		return ZInput.GetKey(key, true);
	}

	public static void PrintBinds(Terminal terminal)
	{
		if (Binds.Count == 0 && WheelBinds.Count == 0)
		{
			terminal.AddString("No binds found.");
			return;
		}
		if (Binds.Count > 0)
		{
			terminal.AddString("Binds:");
		}
		foreach (CommandBind bind in Binds)
		{
			terminal.AddString(PrintBind(bind));
		}
		if (WheelBinds.Count > 0)
		{
			terminal.AddString("Wheel binds:");
		}
		foreach (CommandBind wheelBind in WheelBinds)
		{
			terminal.AddString(PrintBind(wheelBind));
		}
	}

	public static Tuple<string, string>[] GetBinds()
	{
		List<Tuple<string, string>> list = new List<Tuple<string, string>>();
		list.AddRange(Binds.Select((CommandBind bind) => new Tuple<string, string>(GetInput(bind), bind.Command)));
		return list.ToArray();
	}

	public static Tuple<string, string>[] GetWheelBinds()
	{
		List<Tuple<string, string>> list = new List<Tuple<string, string>>();
		list.AddRange(WheelBinds.Select((CommandBind bind) => new Tuple<string, string>(GetInput(bind), bind.Command)));
		return list.ToArray();
	}

	private static string PrintBind(CommandBind bind)
	{
		return GetInput(bind) + ": " + bind.Command;
	}

	private static string GetInput(CommandBind bind)
	{
		List<string> list = new List<string>();
		if (bind.Required != null)
		{
			list.AddRange(bind.Required.Select((KeyCode key) => ((object)(KeyCode)(ref key)).ToString()));
		}
		if (bind.Banned != null)
		{
			list.AddRange(bind.Banned.Select((KeyCode key) => "-" + ((object)(KeyCode)(ref key)).ToString()));
		}
		if (bind.RequiredState != null)
		{
			list.AddRange(bind.RequiredState.Select((string state) => state.ToString()));
		}
		if (bind.BannedState != null)
		{
			list.AddRange(bind.BannedState.Select((string state) => "-" + state.ToString()));
		}
		return string.Join(",", list);
	}

	public static List<CommandBind> GetOffBinds()
	{
		List<CommandBind> list = new List<CommandBind>();
		foreach (CommandBind bind in Binds)
		{
			if (bind.Executed)
			{
				bind.WasExecuted = true;
				bind.Executed = false;
			}
			else if (bind.WasExecuted)
			{
				list.Add(bind);
			}
		}
		return list;
	}
}
