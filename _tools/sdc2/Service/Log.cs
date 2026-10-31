using BepInEx.Logging;

namespace Service;

public class Log
{
	private static ManualLogSource Logger;

	public static void Init(ManualLogSource logger)
	{
		Logger = logger;
	}

	public static void Error(string message)
	{
		Logger.LogError((object)message);
	}

	public static void Warning(string message)
	{
		Logger.LogWarning((object)message);
	}

	public static void Info(string message)
	{
		Logger.LogInfo((object)message);
	}

	public static void Debug(string message)
	{
		Logger.LogDebug((object)message);
	}
}
