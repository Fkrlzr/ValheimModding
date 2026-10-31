using UnityEngine;

namespace SkToolbox;

internal struct Log
{
	public int count;

	public string message;

	public string stackTrace;

	public LogType type;

	private const int maxMessageLength = 16382;

	public bool Equals(Log log)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		if (message == log.message && stackTrace == log.stackTrace)
		{
			return type == log.type;
		}
		return false;
	}

	public string GetTruncatedMessage()
	{
		if (string.IsNullOrEmpty(message))
		{
			return message;
		}
		if (message.Length > 16382)
		{
			return message.Substring(0, 16382);
		}
		return message;
	}
}
