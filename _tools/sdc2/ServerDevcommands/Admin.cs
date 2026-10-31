using UnityEngine;

namespace ServerDevcommands;

public static class Admin
{
	public static bool Checking { get; set; }

	private static void Check()
	{
		if (Object.op_Implicit((Object)(object)ZNet.instance))
		{
			Checking = true;
			PermissionManager.Instance.ResetToDefaults();
			if (ZNet.instance.IsServer())
			{
				OnSuccess();
			}
			else
			{
				ZNet.instance.Unban("admintest_" + GetPlayerId());
			}
		}
	}

	private static long GetPlayerId()
	{
		long playerID = Game.instance.GetPlayerProfile().GetPlayerID();
		if (playerID != 0L)
		{
			return playerID;
		}
		Player localPlayer = Player.m_localPlayer;
		return (localPlayer != null) ? localPlayer.GetPlayerID() : 0;
	}

	public static bool Verify(string text)
	{
		if (text == "Unbanning user admintest_" + GetPlayerId())
		{
			OnSuccess();
		}
		else
		{
			if (!(text == "You are not admin"))
			{
				return false;
			}
			OnFail();
		}
		return true;
	}

	public static void ReceivePermissions(ZRpc rpc, ZPackage pkg)
	{
		PermissionManager.Instance.Read(pkg);
		if (PermissionManager.Instance.IsAdmin)
		{
			OnSuccess();
		}
		else
		{
			OnFail();
		}
	}

	public static void AutomaticCheck()
	{
		if (Settings.AutoDevcommands)
		{
			Check();
		}
	}

	public static void OnSuccess()
	{
		Checking = false;
		DevcommandsCommand.SetCheats(value: true);
		PermissionManager.Instance.IsAdmin = true;
		DevcommandsCommand.Set(value: true);
		((Terminal)Console.instance).AddString("Authorized to use devcommands.");
		ServerExecution.RequestIds();
	}

	private static void OnFail()
	{
		Checking = false;
		((Terminal)Console.instance).AddString("Unauthorized to use devcommands.");
	}

	public static void ManualCheck()
	{
		Check();
	}

	public static void Reset()
	{
		Checking = false;
		DevcommandsCommand.Set(value: false);
	}
}
