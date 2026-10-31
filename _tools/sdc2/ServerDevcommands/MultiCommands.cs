using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerDevcommands;

public class MultiCommands
{
	private static readonly List<MultiCommands> Groups = new List<MultiCommands>();

	private readonly Queue<string> Commands = new Queue<string>(commands);

	private readonly Terminal Terminal;

	private float WaitTimer;

	public MultiCommands(Terminal terminal, string[] commands)
	{
		Terminal = terminal;
		base._002Ector();
	}

	public static string[] Split(string text)
	{
		if (!Settings.MultiCommand)
		{
			return new string[1] { text };
		}
		return (from s in text.Split(';')
			select s.Trim()).ToArray();
	}

	public static void Handle(Terminal terminal, string[] commands)
	{
		MultiCommands multiCommands = new MultiCommands(terminal, commands);
		multiCommands.Run(0f);
		if (!multiCommands.IsDone())
		{
			Groups.Add(multiCommands);
		}
	}

	public static void Execute(float dt)
	{
		for (int i = 0; i < Groups.Count; i++)
		{
			Groups[i].Run(dt);
			if (Groups[i].IsDone())
			{
				Groups.RemoveAt(i);
				i--;
			}
		}
	}

	public bool IsDone()
	{
		return Commands.Count() == 0;
	}

	public void Run(float dt)
	{
		if ((double)WaitTimer > -0.01)
		{
			WaitTimer -= dt;
		}
		if ((double)WaitTimer > -0.01)
		{
			return;
		}
		while (Commands.Count() > 0)
		{
			string text = Commands.Dequeue();
			if (text.StartsWith("wait ", StringComparison.InvariantCultureIgnoreCase))
			{
				string[] array = text.Split(' ');
				if (array.Length >= 2)
				{
					WaitTimer = float.Parse(array[1]) / 1000f;
					break;
				}
			}
			else
			{
				Terminal.TryRunCommand(text, false, false);
			}
		}
	}

	public static void ClearGroups()
	{
		Groups.Clear();
	}
}
