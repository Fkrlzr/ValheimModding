using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ServerDevcommands;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InfinityHammer;

public class Tool
{
	public string Name;

	private List<CommandValue> Commands;

	private readonly string description;

	private readonly string iconName;

	private Sprite? icon;

	private readonly string continuous;

	public float? InitialHeight;

	public float? InitialSize;

	public string InitialShape;

	private readonly string snapGround;

	private readonly string playerHeight;

	private readonly string highlight;

	private readonly string terrainGrid;

	private readonly string snapPiece;

	public bool Instant;

	public int? TabIndex;

	public int? Index;

	public bool Radius;

	public bool Ring;

	public bool Width;

	public bool Frame;

	public bool Depth;

	public bool Height;

	public bool RotateWithPlayer;

	private readonly string targetEdge;

	public bool IsId;

	public int PointCount;

	public bool Variadic;

	private readonly string finish;

	public static readonly Regex PointRegex = new Regex("<([xyz])(\\d+)>");

	public const string RepeatPlaceholder = "<p*>";

	public string Description => DisplayKeys(description);

	public Sprite? Icon => icon ?? (icon = HammerHelper.FindSprite(iconName));

	public bool Continuous
	{
		get
		{
			if (!(continuous == "true"))
			{
				return HammerHelper.IsDown(continuous);
			}
			return true;
		}
	}

	public bool SnapGround
	{
		get
		{
			if (!(snapGround == "true"))
			{
				return HammerHelper.IsDown(snapGround);
			}
			return true;
		}
	}

	public bool PlayerHeight
	{
		get
		{
			if (!(playerHeight == "true"))
			{
				return HammerHelper.IsDown(playerHeight);
			}
			return true;
		}
	}

	public bool Highlight
	{
		get
		{
			if (!(highlight == "true"))
			{
				return HammerHelper.IsDown(highlight);
			}
			return true;
		}
	}

	public bool TerrainGrid
	{
		get
		{
			if (!(terrainGrid == "true"))
			{
				return HammerHelper.IsDown(terrainGrid);
			}
			return true;
		}
	}

	public bool SnapPiece
	{
		get
		{
			if (!(snapPiece == "true"))
			{
				return HammerHelper.IsDown(snapPiece);
			}
			return true;
		}
	}

	public bool IsTargetEdge
	{
		get
		{
			if (!(targetEdge == "true"))
			{
				return HammerHelper.IsDown(targetEdge);
			}
			return true;
		}
	}

	public bool UsesPoints
	{
		get
		{
			if (PointCount <= 0)
			{
				return Variadic;
			}
			return true;
		}
	}

	public bool Finish
	{
		get
		{
			if (!(finish == "true"))
			{
				return HammerHelper.IsDown(finish);
			}
			return true;
		}
	}

	public Tool(ToolData data)
	{
		Name = data.name;
		finish = data.finish;
		description = data.description.Replace("\\n", "\n");
		iconName = data.icon;
		continuous = data.continuous;
		InitialHeight = ((data.initialHeight == "") ? null : new float?(Parse.Float(data.initialHeight, 0f)));
		InitialSize = ((data.initialSize == "") ? null : new float?(Parse.Float(data.initialSize, 0f)));
		InitialShape = data.initialShape;
		snapGround = data.snapGround;
		playerHeight = data.playerHeight;
		highlight = data.highlight;
		terrainGrid = data.terrainGrid;
		snapPiece = data.snapPiece;
		TabIndex = data.tabIndex;
		targetEdge = data.targetEdge;
		Index = data.index;
		List<CommandValue> list;
		if (data.commands != null)
		{
			list = new List<CommandValue>();
			list.AddRange(data.commands.Select((CommandData c) => new CommandValue(c.command, c.keys)));
		}
		else
		{
			list = GetCommands(data.command);
		}
		Commands = list;
		Instant = Commands.All((CommandValue c) => !c.Command.Contains("<") && !c.Command.Contains("hammer "));
		Instant = ((!data.instant.HasValue) ? Instant : data.instant.Value);
		RotateWithPlayer = true;
		foreach (CommandValue command in Commands)
		{
			ParseParameters(command.Command);
			if (command.Command.Contains("<p*>"))
			{
				Variadic = true;
			}
			foreach (Match item in PointRegex.Matches(command.Command))
			{
				PointCount = Math.Max(PointCount, int.Parse(item.Groups[2].Value, CultureInfo.InvariantCulture));
			}
		}
	}

	public string GetCommand()
	{
		IEnumerable<CommandValue> source = Commands.Where((CommandValue c) => c.IsDown());
		return string.Join("; ", source.Select((CommandValue c) => c.Command));
	}

	private void ParseParameters(string command)
	{
		string[] array = command.Split(' ').ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Contains("<id>"))
			{
				IsId = true;
			}
			if (array[i].Contains("<a>"))
			{
				RotateWithPlayer = false;
			}
			if (array[i].Contains("<r>"))
			{
				Radius = true;
			}
			if (array[i].Contains("<r2>"))
			{
				Ring = true;
			}
			if (array[i].Contains("<w>"))
			{
				Width = true;
			}
			if (array[i].Contains("<w2>"))
			{
				Frame = true;
			}
			if (array[i].Contains("<d>"))
			{
				Depth = true;
			}
			if (array[i].Contains("<h>"))
			{
				Height = true;
			}
		}
	}

	private List<CommandValue> GetCommands(string rawCommand)
	{
		string[] array = rawCommand.Split(';');
		Commands = new List<CommandValue>();
		string[] array2 = array;
		foreach (string text in array2)
		{
			int num = text.IndexOf("keys=", StringComparison.Ordinal);
			if (num == -1)
			{
				Commands.Add(new CommandValue(text, ""));
				continue;
			}
			int num2 = text.IndexOf(" ", num + 5, StringComparison.Ordinal);
			if (num2 == -1)
			{
				num2 = text.Length;
			}
			string keys = text.Substring(num + 5, num2 - num - 5);
			string command = text.Substring(0, num - 1) + text.Substring(num2);
			Commands.Add(new CommandValue(command, keys));
		}
		return Commands;
	}

	private static string DisplayKeys(string text)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		InputBinding val = ((IEnumerable<InputBinding>)(object)ZInput.instance.GetButtonDef("AltPlace").ButtonAction.bindings).FirstOrDefault();
		string text2 = "";
		text2 = ZInput.instance.MapKeyFromPath(((InputBinding)(ref val)).effectivePath).ToLowerInvariant();
		return text.Replace("<mod1>", Configuration.ModifierKey1()).Replace("<mod2>", Configuration.ModifierKey2()).Replace("<alt>", text2);
	}
}
