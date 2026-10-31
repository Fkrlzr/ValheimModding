using System.Collections.Generic;
using System.ComponentModel;

namespace Data;

public class ItemYaml
{
	[DefaultValue("")]
	public string pos = "";

	[DefaultValue(1f)]
	public float chance = 1f;

	[DefaultValue("")]
	public string prefab = "";

	[DefaultValue(null)]
	public string? stack;

	[DefaultValue(null)]
	public string? quality;

	[DefaultValue(null)]
	public string? variant;

	[DefaultValue(null)]
	public string? durability;

	[DefaultValue(null)]
	public string? crafterID;

	[DefaultValue(null)]
	public string? crafterName;

	[DefaultValue(null)]
	public string? worldLevel;

	[DefaultValue(null)]
	public string? equipped;

	[DefaultValue(null)]
	public string? pickedUp;

	[DefaultValue(null)]
	public string? cheated;

	[DefaultValue(null)]
	public Dictionary<string, string>? customData;
}
