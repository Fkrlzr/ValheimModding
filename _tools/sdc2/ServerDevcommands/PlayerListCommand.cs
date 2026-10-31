using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ServerDevcommands;

public class PlayerListCommand
{
	private string Format(ZNetPeer player)
	{
		return string.Format(Settings.Format(Settings.PlayerListFormat), player.m_playerName, player.m_socket.GetHostName(), ((ZDOID)(ref player.m_characterID)).UserID, player.m_refPos.x, player.m_refPos.y, player.m_refPos.z);
	}

	private string Format()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 referencePosition = ZNet.instance.m_referencePosition;
		return string.Format(Settings.Format(Settings.PlayerListFormat), Game.instance.GetPlayerProfile().GetName(), UserInfo.GetLocalUser().UserId, ZNet.instance.m_characterID, referencePosition.x, referencePosition.y, referencePosition.z);
	}

	private void Execute(Terminal context)
	{
		List<string> list = ZNet.instance.GetPeers().Select(Format).ToList();
		if (Object.op_Implicit((Object)(object)ZNet.instance) && !ZNet.instance.IsDedicated())
		{
			list.Add(Format());
		}
		context.AddString(string.Join("\n", list));
	}

	public PlayerListCommand()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		Helper.Command("playerlist", "- Prints online players.", (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			if (ZNet.instance.IsServer())
			{
				Execute(args.Context);
			}
			else
			{
				ServerExecution.Send((IEnumerable<string>)args.Args);
			}
		});
		AutoComplete.RegisterEmpty("playerlist");
	}
}
