using System;

namespace SkToolbox.Configuration;

internal static class SkVersionChecker
{
	internal static System.Version currentVersion = new System.Version("1.1.7");

	internal static System.Version latestVersion = currentVersion;

	public static bool VersionCurrent()
	{
		return true;
	}
}
