using System;
using SkToolbox.Configuration;
using UnityEngine;

namespace SkToolbox;

// Tools that act on OTHER players, taken from ValheimTooler (Yohann MARTIN / Astropilot, MIT licence): damage or kill
// players, give or drain their stamina, and chat under a name of your choice. They exist for a private server whose
// players have agreed to PvP and cheats. They are client side: any player who has switched on EnablePlayerTools in their
// own config can use them, with no admin rights needed, and it is off by default. They use the game's own messages to
// the other client, the same way the game's damage and teleport already travel.
internal class SkPlayerTools : MonoBehaviour
{
	internal static bool InfiniteStaminaOthers;

	internal static bool NoStaminaOthers;

	private float nextTick;

	internal static bool Enabled
	{
		get
		{
			if (SkConfigEntry.CPlayerTools != null)
			{
				return SkConfigEntry.CPlayerTools.Value;
			}
			return false;
		}
	}

	// Returns true when the tools may be used, otherwise tells the player why not.
	internal static bool Allowed()
	{
		if (!Enabled)
		{
			SkCommandProcessor.Notify("Player tools are off. Set EnablePlayerTools to true in the toolbox config and restart.");
			return false;
		}
		if (ZNet.instance == null || Player.m_localPlayer == null)
		{
			SkCommandProcessor.Notify("No world yet.");
			return false;
		}
		return true;
	}

	private void Update()
	{
		if ((!InfiniteStaminaOthers && !NoStaminaOthers) || Time.time < nextTick)
		{
			return;
		}
		nextTick = Time.time + 0.5f;
		if (!Enabled || ZNet.instance == null)
		{
			InfiniteStaminaOthers = false;
			NoStaminaOthers = false;
			return;
		}
		Player me = Player.m_localPlayer;
		if (me == null)
		{
			return;
		}
		foreach (Player player in Player.GetAllPlayers())
		{
			if (player == null || player.GetPlayerID() == me.GetPlayerID())
			{
				continue;
			}
			if (InfiniteStaminaOthers)
			{
				player.UseStamina(0f - player.GetMaxStamina());
			}
			else if (NoStaminaOthers)
			{
				player.UseStamina(99999f);
			}
		}
	}

	internal static void Damage(Player target, float amount)
	{
		HitData hit = new HitData();
		hit.m_damage.m_damage = amount;
		((Character)target).Damage(hit);
	}

	// Chat line under the given name. A blank name uses yours. The line carries your own account id.
	internal static void ChatAs(string username, string message, bool shout)
	{
		Player me = Player.m_localPlayer;
		if (me == null)
		{
			return;
		}
		Player speaker = me;
		if (!string.IsNullOrWhiteSpace(username))
		{
			foreach (Player player in Player.GetAllPlayers())
			{
				if (player != null && string.Equals(player.GetPlayerName(), username, StringComparison.OrdinalIgnoreCase))
				{
					speaker = player;
					break;
				}
			}
		}
		UserInfo info = UserInfo.GetLocalUser();
		info.Name = string.IsNullOrWhiteSpace(username) ? me.GetPlayerName() : username.Trim();
		if (shout)
		{
			if (ZRoutedRpc.instance != null)
			{
				ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, "ChatMessage", ((Character)speaker).GetHeadPoint(), 2, info, message);
			}
			return;
		}
		Talker talker = ((Component)speaker).GetComponent<Talker>();
		ZNetView view = (talker != null) ? ((Component)talker).GetComponent<ZNetView>() : null;
		if (view != null)
		{
			view.InvokeRPC(ZNetView.Everybody, "Say", 1, info, message);
		}
	}
}
