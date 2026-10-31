using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BepInEx.Configuration;
using InfinityHammer;
using ServerDevcommands;
using UnityEngine;

namespace Service;

public class ConfigWrapper
{
	private readonly ConfigFile ConfigFile;

	private readonly Dictionary<string, Action<Terminal, string>> SettingHandlers = new Dictionary<string, Action<Terminal, string>>();

	private readonly List<Action> Binders = new List<Action>();

	private static readonly HashSet<string> Truthies = new HashSet<string> { "1", "t", "true", "yes", "on" };

	private static readonly HashSet<string> Falsies = new HashSet<string> { "0", "f", "false", "no", "off" };

	public ConfigWrapper(string command, ConfigFile configFile)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_004f: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		ConfigFile = configFile;
		new ConsoleCommand(command, "[key] [value] - Toggles or sets a config value.", (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			if (args.Length >= 2 && SettingHandlers.TryGetValue(args[1].ToLower(), out Action<Terminal, string> value))
			{
				if (args.Length == 2)
				{
					value(args.Context, "");
				}
				else
				{
					value(args.Context, string.Join(" ", args.Args, 2, args.Length - 2));
				}
			}
		}, false, false, false, false, false, false, (ConsoleOptionsFetcher)(() => SettingHandlers.Keys.ToList()), false, false, false);
	}

	private ConfigEntry<T> Create<T>(string group, string name, T value, ConfigDescription description)
	{
		return ConfigFile.Bind<T>(group, name, value, description);
	}

	private ConfigEntry<T> Create<T>(string group, string name, T value, string description)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		return Create(group, name, value, new ConfigDescription(description, (AcceptableValueBase)null, Array.Empty<object>()));
	}

	public ConfigEntry<T> Bind<T>(string group, string name, T value, ConfigDescription description)
	{
		ConfigEntry<T> val = Create(group, name, value, description);
		if (val is ConfigEntry<bool> setting)
		{
			Register(setting);
		}
		else if (val is ConfigEntry<KeyboardShortcut> setting2)
		{
			Register(setting2);
		}
		else
		{
			Register<T>(val);
		}
		return val;
	}

	public ConfigEntry<string> BindList(string group, string name, string value, string description)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		return Bind(group, name, value, new ConfigDescription(description, (AcceptableValueBase)null, Array.Empty<object>()));
	}

	public ConfigEntry<string> BindList(string group, string name, string value, ConfigDescription description)
	{
		ConfigEntry<string> val = Create(group, name, value, description);
		RegisterList(val);
		return val;
	}

	public ConfigEntry<KeyboardShortcut> BindCommand(string command, string group, string name, KeyboardShortcut value, string description, string mode = "")
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		string command2 = command;
		return BindCommand(() => command2, group, name, value, description, mode);
	}

	public ConfigEntry<KeyboardShortcut> BindCommand(Func<string> command, string group, string name, KeyboardShortcut value, string description, string mode)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return BindCommand(command, group, name, value, description, "", mode);
	}

	public ConfigEntry<KeyboardShortcut> BindCommand(Func<string> command, string group, string name, KeyboardShortcut value, string description, string offCommand, string mode)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		Func<string> command2 = command;
		string text = ToKey(name);
		string text2 = "_bind_" + text;
		Helper.Command(text2, description, (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			string text3 = command2();
			if (args.Length == 1)
			{
				args.Context.TryRunCommand(text3, false, false);
			}
			else
			{
				string args2 = HammerHelper.GetArgs(args[0], args);
				args.Context.TryRunCommand(text3 + " " + args2, false, false);
			}
		});
		ConfigEntry<KeyboardShortcut> val = Create<KeyboardShortcut>(group, name, value, description);
		RegisterCommand(val, text2, offCommand, mode);
		return val;
	}

	public ConfigEntry<KeyboardShortcut> BindWheelCommand(string command, string group, string name, KeyboardShortcut value, string description, string mode = "")
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		string command2 = command;
		return BindWheelCommand(() => command2, group, name, value, description, mode);
	}

	public ConfigEntry<KeyboardShortcut> BindWheelCommand(Func<string> command, string group, string name, KeyboardShortcut value, string description, string mode = "")
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		Func<string> command2 = command;
		string text = ToKey(name);
		string text2 = "_bind_" + text;
		Helper.Command(text2, description, (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			string text3 = command2();
			if (args.Length == 1)
			{
				args.Context.TryRunCommand(text3, false, false);
			}
			else
			{
				string args2 = HammerHelper.GetArgs(args[0], args);
				args.Context.TryRunCommand(text3 + " " + args2, false, false);
			}
		});
		ConfigEntry<KeyboardShortcut> val = Create<KeyboardShortcut>(group, name, value, description);
		RegisterWheelCommand(val, text2, "", mode);
		return val;
	}

	public ConfigEntry<T> Bind<T>(string group, string name, T value, string description)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		return Bind(group, name, value, new ConfigDescription(description, (AcceptableValueBase)null, Array.Empty<object>()));
	}

	private static void AddMessage(Terminal context, string message)
	{
		context.AddString(message);
		Player localPlayer = Player.m_localPlayer;
		if (localPlayer != null)
		{
			((Character)localPlayer).Message((MessageType)1, message, 0, (Sprite)null, false);
		}
	}

	public void Bind()
	{
		foreach (Action binder in Binders)
		{
			binder();
		}
	}

	private void Register(ConfigEntry<bool> setting)
	{
		ConfigEntry<bool> setting2 = setting;
		string name = ((ConfigEntryBase)setting2).Definition.Key;
		string key = ToKey(name);
		SettingHandlers.Add(key, delegate(Terminal terminal, string value)
		{
			Toggle(terminal, setting2, name, value);
		});
	}

	public static string GetKeys(KeyboardShortcut key)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if ((int)((KeyboardShortcut)(ref key)).MainKey == 0)
		{
			return "unbound";
		}
		KeyCode mainKey = ((KeyboardShortcut)(ref key)).MainKey;
		string text = ((object)(KeyCode)(ref mainKey)).ToString().ToLower();
		if (((KeyboardShortcut)(ref key)).Modifiers.Count() > 0)
		{
			text = text + "," + string.Join(",", ((KeyboardShortcut)(ref key)).Modifiers);
		}
		return text;
	}

	private void UpdateKey(KeyboardShortcut key, string command, string offCommand, string mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		BindManager.SetTemporaryBind(GetKeys(key), mode, command, offCommand);
	}

	private string ToKey(string name)
	{
		return name.ToLower().Replace(' ', '_').Replace("(", "")
			.Replace(")", "");
	}

	private void RegisterCommand(ConfigEntry<KeyboardShortcut> setting, string command, string offCommand, string mode)
	{
		ConfigEntry<KeyboardShortcut> setting2 = setting;
		string command2 = command;
		string offCommand2 = offCommand;
		string mode2 = mode;
		string name = ((ConfigEntryBase)setting2).Definition.Key;
		string key = ToKey(name);
		setting2.SettingChanged += delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			UpdateKey(setting2.Value, command2, offCommand2, mode2);
		};
		Binders.Add(delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			UpdateKey(setting2.Value, command2, offCommand2, mode2);
		});
		SettingHandlers.Add(key, delegate(Terminal terminal, string value)
		{
			SetKey(terminal, setting2, name, value);
		});
	}

	private void UpdateWheelKey(KeyboardShortcut key, string command, string offCommand, string mode)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		List<string> list = new List<string>();
		if ((int)((KeyboardShortcut)(ref key)).MainKey != 0 || mode == "command")
		{
			list.Add("wheel");
			list.Add(GetKeys(key));
		}
		BindManager.SetTemporaryBind(string.Join(",", list), mode, command, offCommand);
	}

	private void RegisterWheelCommand(ConfigEntry<KeyboardShortcut> setting, string command, string offCommand, string mode)
	{
		ConfigEntry<KeyboardShortcut> setting2 = setting;
		string command2 = command;
		string offCommand2 = offCommand;
		string mode2 = mode;
		string name = ((ConfigEntryBase)setting2).Definition.Key;
		string key = ToKey(name);
		setting2.SettingChanged += delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			UpdateWheelKey(setting2.Value, command2, offCommand2, mode2);
		};
		Binders.Add(delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			UpdateWheelKey(setting2.Value, command2, offCommand2, mode2);
		});
		SettingHandlers.Add(key, delegate(Terminal terminal, string value)
		{
			SetKey(terminal, setting2, name, value);
		});
	}

	private void Register(ConfigEntry<KeyboardShortcut> setting)
	{
		ConfigEntry<KeyboardShortcut> setting2 = setting;
		string name = ((ConfigEntryBase)setting2).Definition.Key;
		string key = ToKey(name);
		SettingHandlers.Add(key, delegate(Terminal terminal, string value)
		{
			SetKey(terminal, setting2, name, value);
		});
	}

	private void RegisterList(ConfigEntry<string> setting)
	{
		ConfigEntry<string> setting2 = setting;
		string name = ((ConfigEntryBase)setting2).Definition.Key;
		string key = ToKey(name);
		SettingHandlers.Add(key, delegate(Terminal terminal, string value)
		{
			ToggleFlag(terminal, setting2, name, value);
		});
	}

	private void Register<T>(ConfigEntry<T> setting)
	{
		ConfigEntry<T> setting2 = setting;
		string name = ((ConfigEntryBase)setting2).Definition.Key;
		string key = ToKey(name);
		SettingHandlers.Add(key, delegate(Terminal terminal, string value)
		{
			SetValue<T>(terminal, setting2, name, value);
		});
	}

	private static string State(bool value)
	{
		if (!value)
		{
			return "disabled";
		}
		return "enabled";
	}

	private static string Flag(bool value)
	{
		if (!value)
		{
			return "Added";
		}
		return "Removed";
	}

	private static bool IsTruthy(string value)
	{
		return Truthies.Contains(value);
	}

	private static bool IsFalsy(string value)
	{
		return Falsies.Contains(value);
	}

	private static void Toggle(Terminal context, ConfigEntry<bool> setting, string name, string value)
	{
		if (value == "")
		{
			setting.Value = !setting.Value;
		}
		else if (IsTruthy(value))
		{
			setting.Value = true;
		}
		else if (IsFalsy(value))
		{
			setting.Value = false;
		}
		AddMessage(context, name + " " + State(setting.Value) + ".");
	}

	private static void SetKey(Terminal context, ConfigEntry<KeyboardShortcut> setting, string name, string value)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		if (value == "")
		{
			AddMessage(context, $"{name}: {setting.Value}.");
			return;
		}
		if (!Enum.TryParse<KeyCode>(value, ignoreCase: true, out KeyCode result))
		{
			throw new InvalidOperationException("'" + value + "' is not a valid UnityEngine.KeyCode.");
		}
		setting.Value = new KeyboardShortcut(result, Array.Empty<KeyCode>());
		AddMessage(context, name + " set to " + value + ".");
	}

	public static int TryParseInt(string value, int defaultValue)
	{
		if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			return result;
		}
		return defaultValue;
	}

	public static int TryParseInt(ConfigEntry<string> setting)
	{
		if (int.TryParse(setting.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			return result;
		}
		return TryParseInt((string)((ConfigEntryBase)setting).DefaultValue, 0);
	}

	private static float TryParseFloat(string value, float defaultValue)
	{
		if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
		{
			return result;
		}
		return defaultValue;
	}

	public static float TryParseFloat(ConfigEntry<string> setting)
	{
		if (float.TryParse(setting.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
		{
			return result;
		}
		return TryParseFloat((string)((ConfigEntryBase)setting).DefaultValue, 0f);
	}

	private static HashSet<string> ParseList(string value)
	{
		return (from s in value.Split(',')
			select s.Trim().ToLower()).ToHashSet();
	}

	private static void ToggleFlag(Terminal context, ConfigEntry<string> setting, string name, string value)
	{
		if (value == "")
		{
			AddMessage(context, name + ": " + setting.Value + "\".");
			return;
		}
		HashSet<string> hashSet = ParseList(setting.Value);
		foreach (string item in ParseList(value))
		{
			bool flag = hashSet.Contains(item);
			if (flag)
			{
				hashSet.Remove(item);
			}
			else
			{
				hashSet.Add(item);
			}
			setting.Value = string.Join(",", hashSet);
			AddMessage(context, name + ": " + Flag(flag) + " \"" + item + "\".");
		}
	}

	private static void SetValue<T>(Terminal context, ConfigEntry<T> setting, string name, string value)
	{
		if (value == "")
		{
			AddMessage(context, $"{name}: {setting.Value}.");
			return;
		}
		if (value.Contains("/"))
		{
			string[] array = value.Split('/');
			value = array[0];
			for (int i = 0; i < array.Length - 1; i++)
			{
				T value2 = setting.Value;
				if (string.Equals((value2 != null) ? value2.ToString() : null, array[i], StringComparison.OrdinalIgnoreCase))
				{
					value = array[i + 1];
					break;
				}
			}
		}
		setting.Value = (T)(object)value;
		AddMessage(context, name + " set to " + value + ".");
	}
}
