using System.Collections.Generic;
using System.ComponentModel;

namespace ServerDevcommands;

public class PermissionEntry
{
	[DefaultValue("")]
	public string id = "";

	[DefaultValue("")]
	public string name = "";

	[DefaultValue("")]
	public string character = "";

	[DefaultValue("")]
	public string admin = "";

	public List<string>? groups;

	public Dictionary<string, List<string>>? features;

	public List<string>? commands;
}
