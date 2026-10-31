using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerDevcommands;

public class PermissionManager
{
	public enum FeaturePermission
	{
		Unknown = -1,
		Yes,
		No,
		Force
	}

	private static PermissionManager _instance;

	private Dictionary<string, Dictionary<int, FeaturePermission>> _featurePermissions = new Dictionary<string, Dictionary<int, FeaturePermission>>();

	private bool _isAdmin;

	private static readonly int WildCardHash = StringExtensionMethods.GetStableHashCode("*");

	private List<string> _allowedCommands;

	private List<string> _bannedCommands;

	public static PermissionManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PermissionManager(isAdmin: false);
			}
			return _instance;
		}
	}

	public bool CanCheat
	{
		get
		{
			if (!Terminal.m_cheat || !_isAdmin)
			{
				return Helper.IsDedicated();
			}
			return true;
		}
	}

	public bool IsAdmin
	{
		get
		{
			return _isAdmin;
		}
		set
		{
			if (_isAdmin != value)
			{
				_isAdmin = value;
				PermissionApi.Notify();
			}
		}
	}

	public PermissionManager(bool isAdmin)
	{
		_isAdmin = isAdmin;
		_allowedCommands = new List<string>();
		_bannedCommands = new List<string>();
		base._002Ector();
	}

	public void AddEntry(PermissionEntry entry)
	{
		if (entry == null)
		{
			return;
		}
		string text = entry.admin?.Trim().ToLowerInvariant() ?? "";
		if (text == "yes")
		{
			_isAdmin = true;
		}
		else if (text == "no")
		{
			_isAdmin = false;
		}
		if (entry.features != null)
		{
			foreach (KeyValuePair<string, List<string>> feature in entry.features)
			{
				string key = feature.Key.ToLower();
				if (!_featurePermissions.ContainsKey(key))
				{
					_featurePermissions[key] = new Dictionary<int, FeaturePermission>();
				}
				foreach (string item2 in feature.Value)
				{
					ParseFeature(item2, out string featureName, out FeaturePermission permission);
					if (!(featureName == ""))
					{
						int stableHashCode = StringExtensionMethods.GetStableHashCode(featureName.ToLower());
						_featurePermissions[key][stableHashCode] = permission;
					}
				}
			}
		}
		if (entry.commands == null)
		{
			return;
		}
		foreach (string command in entry.commands)
		{
			ParseFeature(command, out string featureName2, out FeaturePermission permission2);
			if (!(featureName2 == ""))
			{
				string item = NormalizeCommand(featureName2);
				if (permission2 == FeaturePermission.No)
				{
					_bannedCommands.Add(item);
				}
				else
				{
					_allowedCommands.Add(item);
				}
			}
		}
	}

	public bool IsFeatureEnabledByHash(string section, int featureHash, bool localConfigValue)
	{
		return GetFeaturePermissionByHash(section, featureHash) switch
		{
			FeaturePermission.No => false, 
			FeaturePermission.Force => true, 
			FeaturePermission.Yes => localConfigValue, 
			_ => (featureHash != WildCardHash) ? IsFeatureEnabledByHash(section, WildCardHash, localConfigValue) : (localConfigValue && CanCheat), 
		};
	}

	public FeaturePermission GetFeaturePermissionByHash(string section, int featureHash)
	{
		section = section.ToLower();
		if (!_featurePermissions.TryGetValue(section, out Dictionary<int, FeaturePermission> value))
		{
			return FeaturePermission.Unknown;
		}
		if (!value.TryGetValue(featureHash, out var value2))
		{
			return FeaturePermission.Unknown;
		}
		return value2;
	}

	private static string NormalizeCommand(string commandName)
	{
		return commandName?.Trim().ToLowerInvariant() ?? "";
	}

	private static bool StartsWithAny(List<string> commands, string cmd)
	{
		string cmd2 = cmd;
		return commands.Any((string check) => cmd2.StartsWith(check, StringComparison.Ordinal));
	}

	public bool IsCommandAllowed(ConsoleCommand cmd, string commandName, bool remote)
	{
		string text = NormalizeCommand(commandName);
		if (text == "")
		{
			return false;
		}
		if (StartsWithAny(_bannedCommands, text))
		{
			return false;
		}
		if (remote && !IsAdmin)
		{
			return false;
		}
		if (!remote && cmd.IsValid((Terminal)(object)Console.instance, false))
		{
			return true;
		}
		if (StartsWithAny(_allowedCommands, text))
		{
			return true;
		}
		return CanCheat;
	}

	public void Write(ZPackage pkg)
	{
		pkg.Write(_isAdmin);
		pkg.Write(_featurePermissions.Count);
		foreach (KeyValuePair<string, Dictionary<int, FeaturePermission>> featurePermission in _featurePermissions)
		{
			pkg.Write(featurePermission.Key);
			pkg.Write(featurePermission.Value.Count);
			foreach (KeyValuePair<int, FeaturePermission> item in featurePermission.Value)
			{
				pkg.Write(item.Key);
				pkg.Write((int)item.Value);
			}
		}
		pkg.Write(_allowedCommands.Count);
		foreach (string allowedCommand in _allowedCommands)
		{
			pkg.Write(allowedCommand);
		}
		pkg.Write(_bannedCommands.Count);
		foreach (string bannedCommand in _bannedCommands)
		{
			pkg.Write(bannedCommand);
		}
	}

	public void Read(ZPackage pkg)
	{
		ResetToDefaults();
		_isAdmin = pkg.ReadBool();
		int num = pkg.ReadInt();
		for (int i = 0; i < num; i++)
		{
			string key = pkg.ReadString();
			int num2 = pkg.ReadInt();
			if (!_featurePermissions.ContainsKey(key))
			{
				_featurePermissions[key] = new Dictionary<int, FeaturePermission>();
			}
			for (int j = 0; j < num2; j++)
			{
				int key2 = pkg.ReadInt();
				FeaturePermission value = (FeaturePermission)pkg.ReadInt();
				_featurePermissions[key][key2] = value;
			}
		}
		int num3 = pkg.ReadInt();
		_allowedCommands.Clear();
		for (int k = 0; k < num3; k++)
		{
			_allowedCommands.Add(NormalizeCommand(pkg.ReadString()));
		}
		int num4 = pkg.ReadInt();
		_bannedCommands.Clear();
		for (int l = 0; l < num4; l++)
		{
			_bannedCommands.Add(NormalizeCommand(pkg.ReadString()));
		}
		HandleFeatureCommands();
		PermissionApi.Notify();
	}

	public void HandleFeatureCommands()
	{
		Dictionary<int, string> dictionary = Terminal.commands.Select((KeyValuePair<string, ConsoleCommand> c) => c.Key.ToLower()).ToDictionary((string h) => StringExtensionMethods.GetStableHashCode(h), (string h) => h);
		foreach (KeyValuePair<string, Dictionary<int, FeaturePermission>> featurePermission in _featurePermissions)
		{
			foreach (KeyValuePair<int, FeaturePermission> item2 in featurePermission.Value)
			{
				if (dictionary.ContainsKey(item2.Key))
				{
					string item = NormalizeCommand(dictionary[item2.Key]);
					if (item2.Value == FeaturePermission.No)
					{
						_allowedCommands.Remove(item);
						_bannedCommands.Add(item);
					}
					else
					{
						_allowedCommands.Add(item);
						_bannedCommands.Remove(item);
					}
				}
			}
		}
	}

	public void ResetToDefaults()
	{
		_isAdmin = false;
		_featurePermissions.Clear();
		_allowedCommands.Clear();
		_bannedCommands.Clear();
	}

	private static void ParseFeature(string rawFeature, out string featureName, out FeaturePermission permission)
	{
		KeyValuePair<string, string> keyValuePair = Parse.Kvp(rawFeature, ':');
		featureName = keyValuePair.Key.Trim();
		permission = ParsePermission(keyValuePair.Value);
	}

	private static FeaturePermission ParsePermission(string value)
	{
		return value.ToLowerInvariant() switch
		{
			"yes" => FeaturePermission.Yes, 
			"no" => FeaturePermission.No, 
			"force" => FeaturePermission.Force, 
			_ => FeaturePermission.Yes, 
		};
	}
}
