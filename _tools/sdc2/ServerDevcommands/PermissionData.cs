using System;
using System.Collections.Generic;

namespace ServerDevcommands;

public class PermissionData
{
	private const string AnyCharacter = "*";

	private readonly List<PermissionEntry> _entries;

	private readonly Dictionary<string, PermissionEntry> _entriesByKey;

	private readonly Dictionary<string, HashSet<string>> _groupsCache;

	private readonly Dictionary<string, bool?> _adminOverrideCache;

	public List<PermissionEntry> Entries => _entries;

	public int Count => _entriesByKey.Count;

	public static string PeerKey(string hostname, string characterId)
	{
		if (string.IsNullOrWhiteSpace(hostname) || string.IsNullOrWhiteSpace(characterId))
		{
			return "";
		}
		return hostname + "_" + characterId;
	}

	public static string PeerWildcardKey(string hostname)
	{
		return PeerKey(hostname, "*");
	}

	public PermissionData()
	{
		_entries = new List<PermissionEntry>();
		_entriesByKey = new Dictionary<string, PermissionEntry>();
		_groupsCache = new Dictionary<string, HashSet<string>>();
		_adminOverrideCache = new Dictionary<string, bool?>();
	}

	public PermissionData(List<PermissionEntry>? entries)
	{
		_entries = entries ?? new List<PermissionEntry>();
		_entriesByKey = ToDictionary(_entries);
		_groupsCache = new Dictionary<string, HashSet<string>>();
		_adminOverrideCache = new Dictionary<string, bool?>();
		RebuildGroupCache();
	}

	public bool TryGetValue(string key, out PermissionEntry entry)
	{
		return _entriesByKey.TryGetValue(key, out entry);
	}

	public PermissionEntry GetOrCreate(string key)
	{
		if (_entriesByKey.TryGetValue(key, out PermissionEntry value))
		{
			return value;
		}
		value = new PermissionEntry
		{
			name = key
		};
		_entries.Add(value);
		_entriesByKey[key] = value;
		_groupsCache[key] = new HashSet<string>();
		return value;
	}

	public bool AddGroup(PermissionEntry entry, string group)
	{
		string group2 = group;
		if (!TryGetTrackedEntry(entry, out string key, out PermissionEntry tracked))
		{
			return false;
		}
		group2 = group2?.Trim() ?? "";
		if (group2 == "")
		{
			return false;
		}
		PermissionEntry permissionEntry = tracked;
		if (permissionEntry.groups == null)
		{
			permissionEntry.groups = new List<string>();
		}
		if (tracked.groups.Exists((string raw) => raw.Trim().Equals(group2, StringComparison.OrdinalIgnoreCase)))
		{
			return false;
		}
		tracked.groups.Add(group2);
		RefreshGroupCache(key);
		_adminOverrideCache.Remove(key);
		return true;
	}

	public bool RemoveGroup(PermissionEntry entry, string group)
	{
		string group2 = group;
		if (!TryGetTrackedEntry(entry, out string key, out PermissionEntry tracked))
		{
			return false;
		}
		group2 = group2?.Trim() ?? "";
		if (group2 == "")
		{
			return false;
		}
		if (tracked.groups == null)
		{
			return false;
		}
		int count = tracked.groups.Count;
		tracked.groups.RemoveAll((string raw) => raw.Trim().Equals(group2, StringComparison.OrdinalIgnoreCase));
		if (tracked.groups.Count == 0)
		{
			tracked.groups = null;
		}
		bool num = count != (tracked.groups?.Count ?? 0);
		if (num)
		{
			RefreshGroupCache(key);
			_adminOverrideCache.Remove(key);
		}
		return num;
	}

	public bool ClearGroups(PermissionEntry entry)
	{
		if (!TryGetTrackedEntry(entry, out string key, out PermissionEntry tracked))
		{
			return false;
		}
		if (tracked.groups == null || tracked.groups.Count == 0)
		{
			return false;
		}
		tracked.groups = null;
		RefreshGroupCache(key);
		_adminOverrideCache.Remove(key);
		return true;
	}

	public bool SetFeaturePermission(PermissionEntry entry, string section, string feature, PermissionManager.FeaturePermission permission)
	{
		string feature2 = feature;
		if (!TryGetTrackedEntry(entry, out string _, out PermissionEntry tracked))
		{
			return false;
		}
		section = section?.Trim().ToLowerInvariant() ?? "";
		feature2 = feature2?.Trim() ?? "";
		if (section == "" || feature2 == "")
		{
			throw new InvalidOperationException("Missing section or feature name.");
		}
		PermissionEntry permissionEntry = tracked;
		if (permissionEntry.features == null)
		{
			permissionEntry.features = new Dictionary<string, List<string>>();
		}
		if (!tracked.features.TryGetValue(section, out List<string> value))
		{
			value = new List<string>();
			tracked.features[section] = value;
		}
		string text = PermissionToSuffix(permission);
		string replacement = ((text == "yes") ? feature2 : (feature2 + ": " + text));
		bool num = value.Exists((string raw) => raw.Equals(replacement, StringComparison.OrdinalIgnoreCase));
		int count = value.Count;
		value.RemoveAll((string raw) => IsMatchingFeature(raw, feature2));
		value.Add(replacement);
		if (num)
		{
			return count != value.Count;
		}
		return true;
	}

	public bool ClearFeature(PermissionEntry entry, string section, string feature)
	{
		string feature2 = feature;
		if (!TryGetTrackedEntry(entry, out string _, out PermissionEntry tracked))
		{
			return false;
		}
		section = section?.Trim().ToLowerInvariant() ?? "";
		feature2 = feature2?.Trim() ?? "";
		if (section == "" || feature2 == "")
		{
			throw new InvalidOperationException("Missing section or feature name.");
		}
		if (tracked.features == null)
		{
			return false;
		}
		if (!tracked.features.TryGetValue(section, out List<string> value))
		{
			return false;
		}
		int count = value.Count;
		value.RemoveAll((string raw) => IsMatchingFeature(raw, feature2));
		if (value.Count == 0)
		{
			tracked.features.Remove(section);
		}
		if (tracked.features.Count == 0)
		{
			tracked.features = null;
		}
		return count != value.Count;
	}

	public bool SetCommandPermission(PermissionEntry entry, string command, PermissionManager.FeaturePermission permission)
	{
		string command2 = command;
		if (!TryGetTrackedEntry(entry, out string _, out PermissionEntry tracked))
		{
			return false;
		}
		command2 = command2?.Trim() ?? "";
		if (command2 == "")
		{
			throw new InvalidOperationException("Missing command name.");
		}
		PermissionEntry permissionEntry = tracked;
		if (permissionEntry.commands == null)
		{
			permissionEntry.commands = new List<string>();
		}
		string text = PermissionToSuffix(permission);
		string replacement = ((text == "yes") ? command2 : (command2 + ": " + text));
		bool num = tracked.commands.Exists((string raw) => raw.Equals(replacement, StringComparison.OrdinalIgnoreCase));
		int count = tracked.commands.Count;
		tracked.commands.RemoveAll((string raw) => IsMatchingCommand(raw, command2));
		tracked.commands.Add(replacement);
		if (num)
		{
			return count != tracked.commands.Count;
		}
		return true;
	}

	public bool ClearCommand(PermissionEntry entry, string command)
	{
		string command2 = command;
		if (!TryGetTrackedEntry(entry, out string _, out PermissionEntry tracked))
		{
			return false;
		}
		command2 = command2?.Trim() ?? "";
		if (command2 == "")
		{
			throw new InvalidOperationException("Missing command name.");
		}
		if (tracked.commands == null)
		{
			return false;
		}
		int count = tracked.commands.Count;
		tracked.commands.RemoveAll((string raw) => IsMatchingCommand(raw, command2));
		int count2 = tracked.commands.Count;
		if (tracked.commands.Count == 0)
		{
			tracked.commands = null;
		}
		return count != count2;
	}

	public bool ClearAll(PermissionEntry entry)
	{
		if (!TryGetTrackedEntry(entry, out string _, out PermissionEntry tracked))
		{
			return false;
		}
		bool result = ClearGroups(tracked);
		if (tracked.commands != null)
		{
			tracked.commands = null;
			result = true;
		}
		if (tracked.features != null)
		{
			tracked.features = null;
			result = true;
		}
		if (!string.IsNullOrWhiteSpace(tracked.admin))
		{
			tracked.admin = "";
			_adminOverrideCache[EntryKey(tracked)] = null;
			result = true;
		}
		return result;
	}

	public bool HasGroup(string key, string group)
	{
		if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(group))
		{
			return false;
		}
		if (!_groupsCache.TryGetValue(key, out HashSet<string> value))
		{
			return false;
		}
		return value.Contains(group.Trim().ToLowerInvariant());
	}

	public bool HasGroup(string hostname, string characterId, string group)
	{
		string text = PeerKey(hostname, characterId);
		if (text == "")
		{
			return false;
		}
		if (HasGroup(text, group))
		{
			return true;
		}
		string key = PeerWildcardKey(hostname);
		return HasGroup(key, group);
	}

	private static bool IsMatchingFeature(string raw, string feature)
	{
		return Parse.Kvp(raw, ':').Key.Trim().Equals(feature.Trim(), StringComparison.OrdinalIgnoreCase);
	}

	private static bool IsMatchingCommand(string raw, string command)
	{
		return Parse.Kvp(raw, ':').Key.Trim().Equals(command.Trim(), StringComparison.OrdinalIgnoreCase);
	}

	private static string PermissionToSuffix(PermissionManager.FeaturePermission permission)
	{
		return permission switch
		{
			PermissionManager.FeaturePermission.No => "no", 
			PermissionManager.FeaturePermission.Force => "force", 
			_ => "yes", 
		};
	}

	public void RebuildGroupCache()
	{
		_groupsCache.Clear();
		foreach (string key in _entriesByKey.Keys)
		{
			RefreshGroupCache(key);
		}
	}

	public void RefreshGroupCache(string key)
	{
		if (!string.IsNullOrWhiteSpace(key))
		{
			if (!_entriesByKey.ContainsKey(key))
			{
				_groupsCache.Remove(key);
				return;
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			HashSet<string> visitedPath = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			AddResolvedGroups(key, hashSet, visitedPath);
			_groupsCache[key] = hashSet;
		}
	}

	private void AddResolvedGroups(string key, HashSet<string> resolved, HashSet<string> visitedPath)
	{
		if (!visitedPath.Add(key))
		{
			return;
		}
		if (!_entriesByKey.TryGetValue(key, out PermissionEntry value) || value.groups == null)
		{
			visitedPath.Remove(key);
			return;
		}
		foreach (string group in value.groups)
		{
			string text = group.Trim().ToLowerInvariant();
			if (!(text == ""))
			{
				resolved.Add(text);
				AddResolvedGroups(text, resolved, visitedPath);
			}
		}
		visitedPath.Remove(key);
	}

	public bool UpdatePeer(string hostname, string characterId, string playerName)
	{
		string text = PeerKey(hostname, characterId);
		if (text == "")
		{
			return false;
		}
		PermissionEntry orCreate = GetOrCreate(text);
		return false | UpdateString(orCreate, (PermissionEntry e) => e.name, delegate(PermissionEntry e, string value)
		{
			e.name = value;
		}, playerName) | UpdateString(orCreate, (PermissionEntry e) => e.id, delegate(PermissionEntry e, string value)
		{
			e.id = value;
		}, hostname) | UpdateString(orCreate, (PermissionEntry e) => e.character, delegate(PermissionEntry e, string value)
		{
			e.character = value;
		}, characterId);
	}

	public PermissionManager Resolve(string hostname, string characterId)
	{
		PermissionManager permissionManager = new PermissionManager(ZNet.instance.IsAdmin(hostname));
		List<PermissionEntry> list = BuildResolutionChain(hostname, characterId);
		for (int i = 0; i < list.Count; i++)
		{
			permissionManager.AddEntry(list[i]);
		}
		return permissionManager;
	}

	public bool? ResolveAdminOverride(string hostname, string characterId)
	{
		string text = PeerKey(hostname, characterId);
		if (text == "")
		{
			return null;
		}
		if (_adminOverrideCache.TryGetValue(text, out var value))
		{
			return value;
		}
		List<PermissionEntry> list = BuildResolutionChain(hostname, characterId);
		bool? flag = null;
		for (int i = 0; i < list.Count; i++)
		{
			string text2 = list[i].admin?.Trim().ToLowerInvariant() ?? "";
			if (text2 == "yes")
			{
				flag = true;
			}
			else if (text2 == "no")
			{
				flag = false;
			}
		}
		_adminOverrideCache[text] = flag;
		return flag;
	}

	public bool? ResolveAdminOverride(string hostname)
	{
		return ResolveAdminOverride(hostname, "*");
	}

	private List<PermissionEntry> BuildResolutionChain(string hostname, string characterId)
	{
		List<PermissionEntry> result = new List<PermissionEntry>();
		HashSet<string> visitedPath = new HashSet<string>();
		HashSet<string> addedEntries = new HashSet<string>();
		AddEntryWithParents("Everyone", result, visitedPath, addedEntries);
		string text = PeerWildcardKey(hostname);
		if (text != "")
		{
			AddEntryWithParents(text, result, visitedPath, addedEntries);
		}
		string key = PeerKey(hostname, characterId);
		AddEntryWithParents(key, result, visitedPath, addedEntries);
		return result;
	}

	private void AddEntryWithParents(string key, List<PermissionEntry> result, HashSet<string> visitedPath, HashSet<string> addedEntries)
	{
		if (_entriesByKey.TryGetValue(key, out PermissionEntry value))
		{
			AddGroupChains(value.groups, result, visitedPath, addedEntries);
			if (!addedEntries.Contains(key))
			{
				addedEntries.Add(key);
				result.Add(value);
			}
		}
	}

	private void AddGroupChains(List<string>? groupNames, List<PermissionEntry> result, HashSet<string> visitedPath, HashSet<string> addedEntries)
	{
		if (groupNames == null)
		{
			return;
		}
		foreach (string groupName in groupNames)
		{
			AddGroupChain(groupName, result, visitedPath, addedEntries);
		}
	}

	private void AddGroupChain(string groupName, List<PermissionEntry> result, HashSet<string> visitedPath, HashSet<string> addedEntries)
	{
		groupName = groupName.Trim();
		if (!string.IsNullOrWhiteSpace(groupName) && !visitedPath.Contains(groupName) && _entriesByKey.TryGetValue(groupName, out PermissionEntry value))
		{
			visitedPath.Add(groupName);
			AddGroupChains(value.groups, result, visitedPath, addedEntries);
			if (!addedEntries.Contains(groupName))
			{
				addedEntries.Add(groupName);
				result.Add(value);
			}
			visitedPath.Remove(groupName);
		}
	}

	private static bool UpdateString(PermissionEntry entry, Func<PermissionEntry, string> getter, Action<PermissionEntry, string> setter, string value)
	{
		if (getter(entry) == value)
		{
			return false;
		}
		setter(entry, value);
		return true;
	}

	private static string EntryKey(PermissionEntry entry)
	{
		string text = PeerKey(entry.id, entry.character);
		if (text != "")
		{
			return text;
		}
		if (!string.IsNullOrWhiteSpace(entry.id))
		{
			return PeerWildcardKey(entry.id);
		}
		return entry.name;
	}

	public static bool IsPeerChanged(HashSet<string> changedKeys, string hostname, string characterId)
	{
		string text = PeerKey(hostname, characterId);
		if (text == "")
		{
			return false;
		}
		if (changedKeys.Contains(text))
		{
			return true;
		}
		string item = PeerWildcardKey(hostname);
		return changedKeys.Contains(item);
	}

	private bool TryGetTrackedEntry(PermissionEntry? entry, out string key, out PermissionEntry tracked)
	{
		key = "";
		tracked = null;
		if (entry == null)
		{
			return false;
		}
		key = EntryKey(entry);
		if (string.IsNullOrWhiteSpace(key))
		{
			return false;
		}
		if (!_entriesByKey.TryGetValue(key, out tracked))
		{
			return false;
		}
		return true;
	}

	private static Dictionary<string, PermissionEntry> ToDictionary(List<PermissionEntry> entries)
	{
		Dictionary<string, PermissionEntry> dictionary = new Dictionary<string, PermissionEntry>();
		foreach (PermissionEntry entry in entries)
		{
			string text = EntryKey(entry);
			if (!string.IsNullOrWhiteSpace(text))
			{
				dictionary[text] = entry;
			}
		}
		return dictionary;
	}

	private static bool IsNullOrEmpty(List<string>? value)
	{
		if (value != null)
		{
			return value.Count == 0;
		}
		return true;
	}

	private static bool IsNullOrEmpty(Dictionary<string, List<string>>? value)
	{
		if (value != null)
		{
			return value.Count == 0;
		}
		return true;
	}

	private static bool EqualStringList(List<string>? a, List<string>? b)
	{
		if (IsNullOrEmpty(a) && IsNullOrEmpty(b))
		{
			return true;
		}
		if (a == null || b == null)
		{
			return false;
		}
		if (a.Count != b.Count)
		{
			return false;
		}
		for (int i = 0; i < a.Count; i++)
		{
			if (a[i] != b[i])
			{
				return false;
			}
		}
		return true;
	}

	private static bool EqualFeatures(Dictionary<string, List<string>>? a, Dictionary<string, List<string>>? b)
	{
		if (IsNullOrEmpty(a) && IsNullOrEmpty(b))
		{
			return true;
		}
		if (a == null || b == null)
		{
			return false;
		}
		if (a.Count != b.Count)
		{
			return false;
		}
		foreach (KeyValuePair<string, List<string>> item in a)
		{
			if (!b.TryGetValue(item.Key, out List<string> value))
			{
				return false;
			}
			if (!EqualStringList(item.Value, value))
			{
				return false;
			}
		}
		return true;
	}

	private static bool EqualEntry(PermissionEntry? a, PermissionEntry? b)
	{
		if (a == b)
		{
			return true;
		}
		if (a == null || b == null)
		{
			return false;
		}
		if (a.id != b.id)
		{
			return false;
		}
		if (a.name != b.name)
		{
			return false;
		}
		if (a.character != b.character)
		{
			return false;
		}
		if (!EqualStringList(a.groups, b.groups))
		{
			return false;
		}
		if (!EqualFeatures(a.features, b.features))
		{
			return false;
		}
		if (!EqualStringList(a.commands, b.commands))
		{
			return false;
		}
		return true;
	}

	public static HashSet<string> ChangedKeys(Dictionary<string, PermissionEntry> oldData, Dictionary<string, PermissionEntry> newData)
	{
		HashSet<string> hashSet = new HashSet<string>();
		foreach (KeyValuePair<string, PermissionEntry> oldDatum in oldData)
		{
			if (!newData.TryGetValue(oldDatum.Key, out PermissionEntry value) || !EqualEntry(oldDatum.Value, value))
			{
				hashSet.Add(oldDatum.Key);
			}
		}
		foreach (string key in newData.Keys)
		{
			if (!oldData.ContainsKey(key))
			{
				hashSet.Add(key);
			}
		}
		return hashSet;
	}

	public static HashSet<string> ChangedKeys(PermissionData oldData, PermissionData newData)
	{
		return ChangedKeys(oldData._entriesByKey, newData._entriesByKey);
	}

	public static bool HasGroupChanges(HashSet<string> changedKeys, Dictionary<string, PermissionEntry> oldData, Dictionary<string, PermissionEntry> newData)
	{
		foreach (string changedKey in changedKeys)
		{
			if (IsGroupEntry(SelectEntry(changedKey, oldData, newData)))
			{
				return true;
			}
		}
		return false;
	}

	public static bool HasGroupChanges(HashSet<string> changedKeys, PermissionData oldData, PermissionData newData)
	{
		return HasGroupChanges(changedKeys, oldData._entriesByKey, newData._entriesByKey);
	}

	private static PermissionEntry? SelectEntry(string key, Dictionary<string, PermissionEntry> oldData, Dictionary<string, PermissionEntry> newData)
	{
		if (newData.TryGetValue(key, out PermissionEntry value))
		{
			return value;
		}
		if (oldData.TryGetValue(key, out PermissionEntry value2))
		{
			return value2;
		}
		return null;
	}

	private static bool IsGroupEntry(PermissionEntry? entry)
	{
		if (entry == null)
		{
			return false;
		}
		if (!string.IsNullOrWhiteSpace(entry.id))
		{
			return false;
		}
		return !string.IsNullOrWhiteSpace(entry.name);
	}
}
