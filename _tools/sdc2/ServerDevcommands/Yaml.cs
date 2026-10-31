using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using Service;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ServerDevcommands;

public class Yaml
{
	public static void SetupWatcher(string pattern, Action action)
	{
		SetupWatcher(Paths.ConfigPath, pattern, action);
	}

	public static void SetupWatcher(string path, string pattern, string folder, Action action)
	{
		SetupWatcher(path, pattern, action);
		if (!string.IsNullOrEmpty(folder))
		{
			string path2 = Path.Combine(path, folder);
			if (Directory.Exists(path2))
			{
				SetupWatcher(path2, "*", action);
			}
		}
	}

	public static void SetupWatcher(string path, string pattern, Action action)
	{
		Action action2 = action;
		FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(path, pattern);
		fileSystemWatcher.Created += delegate
		{
			action2();
		};
		fileSystemWatcher.Changed += delegate
		{
			action2();
		};
		fileSystemWatcher.Renamed += delegate
		{
			action2();
		};
		fileSystemWatcher.Deleted += delegate
		{
			action2();
		};
		fileSystemWatcher.IncludeSubdirectories = true;
		fileSystemWatcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
		fileSystemWatcher.EnableRaisingEvents = true;
	}

	public static IDeserializer Deserializer()
	{
		return new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
	}

	public static IDeserializer DeserializerUnSafe()
	{
		return new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).IgnoreUnmatchedProperties().Build();
	}

	public static ISerializer Serializer()
	{
		return new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).DisableAliases().ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
			.WithTypeConverter(new FloatConverter())
			.WithEventEmitter((IEventEmitter nextEmitter) => new MultilineScalarFlowStyleEmitter(nextEmitter))
			.Build();
	}

	public static T Deserialize<T>(string raw, string fileName) where T : new()
	{
		try
		{
			return Deserializer().Deserialize<T>(raw);
		}
		catch (Exception ex)
		{
			Log.Error(fileName + ": " + ex.Message);
			try
			{
				return DeserializerUnSafe().Deserialize<T>(raw);
			}
			catch (Exception)
			{
				return new T();
			}
		}
	}

	private static IEnumerable<string> GetPaths(string directory, string pattern, string folder)
	{
		if (!Directory.Exists(directory))
		{
			return Array.Empty<string>();
		}
		SearchOption searchOption = ((!(directory == Paths.ConfigPath)) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
		IEnumerable<string> enumerable = Directory.GetFiles(directory, pattern, searchOption).AsEnumerable();
		if (!string.IsNullOrEmpty(folder))
		{
			string path = Path.Combine(directory, folder);
			if (Directory.Exists(path))
			{
				enumerable = enumerable.Concat(Directory.GetFiles(path, "*", SearchOption.AllDirectories)).Distinct();
			}
		}
		return enumerable;
	}

	private static string GetRelativePath(string directory, string path)
	{
		if (!path.StartsWith(directory, StringComparison.OrdinalIgnoreCase))
		{
			return Path.GetFileName(path);
		}
		return path.Substring(directory.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
	}

	public static bool AnyFileExists(string directory, string pattern, string folder)
	{
		return GetPaths(directory, pattern, folder).Any((string path) => File.ReadAllText(path).Trim().Length > 0);
	}

	public static void LoadDictFromDirectory<T>(string directory, string pattern, Action<string, string, T> action)
	{
		LoadDictFromDirectory(directory, pattern, "", action);
	}

	public static void LoadDictFromDirectory<T>(string directory, string pattern, string folder, Action<string, string, T> action)
	{
		foreach (string path in GetPaths(directory, pattern, folder))
		{
			string relativePath = GetRelativePath(directory, path);
			Dictionary<string, T> dictionary = Deserialize<Dictionary<string, T>>(File.ReadAllText(path), Path.GetFileName(path));
			if (dictionary == null)
			{
				continue;
			}
			foreach (KeyValuePair<string, T> item in dictionary)
			{
				if (item.Key != null && item.Value != null)
				{
					action(relativePath, item.Key, item.Value);
				}
			}
		}
	}

	public static void LoadListsFromDirectory<T>(string directory, string pattern, Action<string, T> action) where T : new()
	{
		LoadListsFromDirectory(directory, pattern, "", action);
	}

	public static void LoadListsFromDirectory<T>(string directory, string pattern, string folder, Action<string, T> action) where T : new()
	{
		foreach (string path in GetPaths(directory, pattern, folder))
		{
			string relativePath = GetRelativePath(directory, path);
			List<T> list = Deserialize<List<T>>(File.ReadAllText(path), Path.GetFileName(path));
			if (list == null)
			{
				continue;
			}
			foreach (T item in list)
			{
				if (item != null)
				{
					action(relativePath, item);
				}
			}
		}
	}

	public static bool IsDefaultFile(string file, string folder, string fileName)
	{
		if (!(file == fileName))
		{
			return file == Path.Combine(folder, fileName);
		}
		return true;
	}

	public static void ConsolidateDefaultFile(string directory, string folder, string fileName)
	{
		string text = Path.Combine(directory, folder);
		bool flag = Directory.Exists(text);
		string text2 = Path.Combine(text, fileName);
		string text3 = Path.Combine(directory, fileName);
		if (!flag)
		{
			if (!File.Exists(text3))
			{
				File.WriteAllText(text3, "");
			}
		}
		else if (!File.Exists(text3))
		{
			if (!File.Exists(text2))
			{
				File.WriteAllText(text2, "");
			}
		}
		else if (File.Exists(text2))
		{
			Log.Warning("Both " + text3 + " and " + text2 + " exist, keeping the one in the folder.");
			File.Delete(text3);
		}
		else
		{
			File.Move(text3, text2);
		}
	}
}
