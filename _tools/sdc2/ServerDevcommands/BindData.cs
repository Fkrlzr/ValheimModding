using System.ComponentModel;

namespace ServerDevcommands;

public class BindData
{
	[DefaultValue("")]
	public string keys = "";

	[DefaultValue("")]
	public string state = "";

	public string? key;

	public string? modifiers;

	[DefaultValue("")]
	public string command = "";

	[DefaultValue("")]
	public string offCommand = "";
}
