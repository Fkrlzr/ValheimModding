using System.Collections.Generic;
using UnityEngine;

namespace SkToolbox.Utility;

internal static class SkRun
{
	internal static void Cmd(string command)
	{
		if (!string.IsNullOrEmpty(command))
		{
			if ((Object)(object)Console.instance == (Object)null)
			{
				SkCommandProcessor.Notify("The game console is not ready yet.");
			}
			else
			{
				RunRaw(command);
			}
		}
	}

	private static void RunRaw(string command)
	{
		bool bCheat = SkCommandPatcher.BCheat;
		SkCommandPatcher.BCheat = true;
		try
		{
			((Terminal)Console.instance).TryRunCommand(command, false, false);
		}
		finally
		{
			SkCommandPatcher.BCheat = bCheat;
		}
	}

	internal static void CmdNotify(string command, string fallback)
	{
		List<string> list = Capture(command);
		SkCommandProcessor.Notify((list.Count > 0) ? list[0] : fallback);
	}

	internal static void Show(string title, string command, string emptyMessage = null)
	{
		List<string> list = Capture(command);
		if (list.Count == 0)
		{
			SkCommandProcessor.Notify(emptyMessage ?? "Nothing to show.");
		}
		else
		{
			Text(title, string.Join("\n", list.ToArray()));
		}
	}

	internal static void Text(string title, string body)
	{
		SkMenuController instance = SkMenuController.Instance;
		if (!((Object)(object)instance == (Object)null))
		{
			SkMenuController.SkForm skForm = new SkMenuController.SkForm
			{
				Title = title
			};
			skForm.Fields.Add(new SkMenuController.SkFormField
			{
				Id = "text",
				Label = "Output",
				Kind = SkMenuController.SkFieldKind.Info,
				TextValue = body,
				Height = 300
			});
			instance.ShowForm(skForm);
		}
	}

	internal static List<string> Capture(string command)
	{
		List<string> list = new List<string>();
		if (string.IsNullOrEmpty(command) || (Object)(object)Console.instance == (Object)null)
		{
			SkCommandProcessor.Notify("The game console is not ready yet.");
			return list;
		}
		List<string> privateField = Console.instance.GetPrivateField<List<string>>("m_chatBuffer");
		if (privateField == null)
		{
			RunRaw(command);
			return list;
		}
		privateField.Add("\u200b");
		RunRaw(command);
		int num = privateField.LastIndexOf("\u200b");
		if (num < 0)
		{
			return list;
		}
		for (int i = num + 1; i < privateField.Count; i++)
		{
			if (!string.IsNullOrEmpty(privateField[i]))
			{
				list.Add(privateField[i]);
			}
		}
		privateField.RemoveAt(num);
		Console.instance.InvokePrivateMethod("UpdateChat", null);
		return list;
	}
}
