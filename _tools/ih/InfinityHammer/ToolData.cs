using System.ComponentModel;

namespace InfinityHammer;

public class ToolData
{
	[DefaultValue("")]
	public string name = "";

	[DefaultValue("")]
	public string description = "";

	[DefaultValue("")]
	public string icon = "";

	public string command = "";

	public CommandData[]? commands;

	[DefaultValue("")]
	public string continuous = "";

	[DefaultValue("")]
	public string initialHeight = "";

	[DefaultValue("")]
	public string initialSize = "";

	[DefaultValue("")]
	public string initialShape = "";

	[DefaultValue("false")]
	public string targetEdge = "false";

	[DefaultValue("false")]
	public string snapGround = "false";

	[DefaultValue("false")]
	public string snapPiece = "false";

	[DefaultValue("")]
	public string playerHeight = "";

	[DefaultValue("false")]
	public string highlight = "false";

	[DefaultValue("false")]
	public string terrainGrid = "false";

	[DefaultValue("")]
	public string finish = "";

	[DefaultValue(null)]
	public bool? instant;

	[DefaultValue(null)]
	public int? tabIndex;

	[DefaultValue(null)]
	public int? index;

	internal bool IsDefaultData;
}
