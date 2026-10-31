using System;
using System.Collections.Generic;
using System.Linq;
using Service;

namespace ServerDevcommands;

public static class PermissionYaml
{
	private static readonly HashSet<string> ReservedKeys = new HashSet<string> { "id", "name", "character", "admin", "groups", "commands" };

	public static string SerializeEntries(List<PermissionEntry> entries)
	{
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		foreach (PermissionEntry entry in entries)
		{
			list.Add(SerializeEntry(entry));
		}
		return Yaml.Serializer().Serialize(list);
	}

	public static List<PermissionEntry> DeserializeEntries(string value)
	{
		try
		{
			List<Dictionary<string, object>> list = Yaml.Deserializer().Deserialize<List<Dictionary<string, object>>>(value);
			if (list == null)
			{
				return new List<PermissionEntry>();
			}
			return list.Select(ParseEntry).ToList();
		}
		catch (Exception ex)
		{
			Log.Error("permissions: " + ex.Message);
			return new List<PermissionEntry>();
		}
	}

	private static Dictionary<string, object> SerializeEntry(PermissionEntry entry)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (!string.IsNullOrWhiteSpace(entry.id))
		{
			dictionary["id"] = entry.id;
		}
		if (!string.IsNullOrWhiteSpace(entry.name))
		{
			dictionary["name"] = entry.name;
		}
		if (!string.IsNullOrWhiteSpace(entry.character))
		{
			dictionary["character"] = entry.character;
		}
		string text = NormalizeAdmin(entry.admin);
		if (text != "")
		{
			dictionary["admin"] = text;
		}
		if (entry.groups != null && entry.groups.Count > 0)
		{
			dictionary["groups"] = entry.groups;
		}
		if (entry.features != null)
		{
			foreach (KeyValuePair<string, List<string>> item in entry.features.OrderBy<KeyValuePair<string, List<string>>, string>((KeyValuePair<string, List<string>> kvp) => kvp.Key, StringComparer.OrdinalIgnoreCase))
			{
				if (item.Value.Count != 0)
				{
					Dictionary<string, string> dictionary2 = ToPermissionMap(item.Value);
					if (dictionary2.Count > 0)
					{
						dictionary[item.Key] = dictionary2;
					}
				}
			}
		}
		if (entry.commands != null && entry.commands.Count > 0)
		{
			Dictionary<string, string> dictionary3 = ToPermissionMap(entry.commands);
			if (dictionary3.Count > 0)
			{
				dictionary["commands"] = dictionary3;
			}
		}
		return dictionary;
	}

	private static PermissionEntry ParseEntry(Dictionary<string, object> raw)
	{
		PermissionEntry permissionEntry = new PermissionEntry
		{
			id = ReadScalar(raw, "id"),
			name = ReadScalar(raw, "name"),
			character = ReadScalar(raw, "character"),
			admin = ReadAdmin(raw, "admin"),
			groups = ReadStringList(raw, "groups"),
			commands = ReadPermissionList(raw, "commands")
		};
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		foreach (KeyValuePair<string, object> item in raw)
		{
			string text = item.Key?.Trim() ?? "";
			if (!(text == "") && !ReservedKeys.Contains(text.ToLowerInvariant()))
			{
				List<string> list = ToPermissionList(item.Value);
				if (list.Count != 0)
				{
					dictionary[text.ToLowerInvariant()] = list;
				}
			}
		}
		if (dictionary.Count > 0)
		{
			permissionEntry.features = dictionary;
		}
		if (permissionEntry.commands != null && permissionEntry.commands.Count == 0)
		{
			permissionEntry.commands = null;
		}
		if (permissionEntry.groups != null && permissionEntry.groups.Count == 0)
		{
			permissionEntry.groups = null;
		}
		return permissionEntry;
	}

	private static string ReadScalar(Dictionary<string, object> raw, string key)
	{
		if (!raw.TryGetValue(key, out object value))
		{
			return "";
		}
		return value?.ToString()?.Trim() ?? "";
	}

	private static List<string>? ReadPermissionList(Dictionary<string, object> raw, string key)
	{
		if (!raw.TryGetValue(key, out object value))
		{
			return null;
		}
		return ToPermissionList(value);
	}

	private static List<string>? ReadStringList(Dictionary<string, object> raw, string key)
	{
		if (!raw.TryGetValue(key, out object value))
		{
			return null;
		}
		return ToStringList(value);
	}

	private static string ReadAdmin(Dictionary<string, object> raw, string key)
	{
		if (!raw.TryGetValue(key, out object value))
		{
			return "";
		}
		return NormalizeAdmin(value?.ToString() ?? "");
	}

	private static string NormalizeAdmin(string value)
	{
		return (value?.Trim().ToLowerInvariant() ?? "") switch
		{
			"yes" => "yes", 
			"true" => "yes", 
			"1" => "yes", 
			"no" => "no", 
			"false" => "no", 
			"0" => "no", 
			_ => "", 
		};
	}

	private static List<string> ToStringList(object? value)
	{
		List<string> list = new List<string>();
		if (value is IList<object> list2)
		{
			{
				foreach (object item in list2)
				{
					string text = item?.ToString()?.Trim() ?? "";
					if (text != "")
					{
						list.Add(text);
					}
				}
				return list;
			}
		}
		if (value is IList<string> list3)
		{
			{
				foreach (string item2 in list3)
				{
					string text2 = item2?.Trim() ?? "";
					if (text2 != "")
					{
						list.Add(text2);
					}
				}
				return list;
			}
		}
		string text3 = value?.ToString()?.Trim() ?? "";
		if (text3 != "")
		{
			list.Add(text3);
		}
		return list;
	}

	private static List<string> ToPermissionList(object? value)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (value is IDictionary<object, object> dictionary2)
		{
			foreach (KeyValuePair<object, object> item in dictionary2)
			{
				string text = item.Key?.ToString()?.Trim() ?? "";
				if (!(text == ""))
				{
					string value2 = item.Value?.ToString()?.Trim() ?? "";
					dictionary[text] = value2;
				}
			}
		}
		else if (value is IDictionary<string, object> dictionary3)
		{
			foreach (KeyValuePair<string, object> item2 in dictionary3)
			{
				string text2 = item2.Key?.Trim() ?? "";
				if (!(text2 == ""))
				{
					string value3 = item2.Value?.ToString()?.Trim() ?? "";
					dictionary[text2] = value3;
				}
			}
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, string> item3 in dictionary)
		{
			string text3 = item3.Key.Trim();
			if (!(text3 == ""))
			{
				string text4 = item3.Value.Trim();
				if (text4 == "" || text4.Equals("yes", StringComparison.OrdinalIgnoreCase))
				{
					list.Add(text3);
				}
				else
				{
					list.Add(text3 + ": " + text4);
				}
			}
		}
		return list;
	}

	private static Dictionary<string, string> ToPermissionMap(List<string> values)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (string value in values)
		{
			KeyValuePair<string, string> keyValuePair = Parse.Kvp(value, ':');
			string text = keyValuePair.Key.Trim();
			if (!(text == ""))
			{
				string text2 = keyValuePair.Value.Trim().ToLowerInvariant();
				if (text2 == "")
				{
					text2 = "yes";
				}
				dictionary[text] = text2;
			}
		}
		return dictionary;
	}
}
