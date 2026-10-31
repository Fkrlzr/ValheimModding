using System.Collections.Generic;
using UnityEngine;

namespace ServerDevcommands;

public class CommandBind
{
	public List<KeyCode> Required = new List<KeyCode>();

	public List<KeyCode>? Banned;

	public List<string>? RequiredState;

	public List<string>? BannedState;

	public bool MouseWheel;

	public string Command = "";

	public string OffCommand = "";

	public bool Executed;

	public bool WasExecuted;

	public string? Keys;

	public bool Temporary;

	public CommandValidity Validity;

	public bool IsDefault;

	public string Source = "";
}
