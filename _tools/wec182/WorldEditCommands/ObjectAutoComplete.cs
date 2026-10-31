using System;
using System.Collections.Generic;
using Data;
using ServerDevcommands;

namespace WorldEditCommands;

public class ObjectAutoComplete : SharedObjectAutoComplete
{
	public List<string> NamedParameters;

	public ObjectAutoComplete()
	{
		NamedParameters = SharedObjectAutoComplete.WithSharedParameters(new List<string>(26)
		{
			"wild", "info", "sleep", "id", "ignore", "move", "rotate", "remove", "origin", "visual",
			"fuel", "prefab", "center", "respawn", "from", "rect", "angle", "creator", "chance", "type",
			"connect", "status", "components", "match", "unmatch", "copy"
		});
		AutoComplete.Register("object", (Func<int, List<string>>)((int index) => NamedParameters), SharedObjectAutoComplete.WithSharedFetchers(new Dictionary<string, Func<int, List<string>>>
		{
			{
				"status",
				(int index) => index switch
				{
					0 => ParameterInfo.StatusEffects, 
					1 => ParameterInfo.Create("status=name,<color=yellow>duration</color>,intensity", "Duration in seconds."), 
					2 => ParameterInfo.Create("status=name,duration,<color=yellow>intensity</color>", "Strength of the effect."), 
					_ => ParameterInfo.None, 
				}
			},
			{
				"type",
				(int index) => ParameterInfo.Components
			},
			{
				"connect",
				(int index) => ParameterInfo.Flag("Connect")
			},
			{
				"mirror",
				(int index) => ParameterInfo.Flag("Mirror")
			},
			{
				"components",
				(int index) => ParameterInfo.Flag("Components")
			},
			{
				"respawn",
				(int index) => ParameterInfo.Flag("Respawn")
			},
			{
				"wild",
				(int index) => ParameterInfo.Flag("Wild")
			},
			{
				"remove",
				(int index) => ParameterInfo.Flag("Remove")
			},
			{
				"sleep",
				(int index) => ParameterInfo.Flag("Sleep")
			},
			{
				"info",
				(int index) => ParameterInfo.Flag("info")
			},
			{
				"id",
				(int index) => ParameterInfo.Ids
			},
			{
				"ignore",
				(int index) => ParameterInfo.Ids
			},
			{
				"prefab",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.ObjectIds
			},
			{
				"fuel",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("fuel", "number", "Sets or gets the fuel amount.")
			},
			{
				"move",
				(int index) => ParameterInfo.FRU("move", "Movement offset based on the player rotation (unless origin is given)", index)
			},
			{
				"rotate",
				delegate(int index)
				{
					string text = "Rotation based on the player rotation (unless origin is given)";
					return (index == 0) ? ParameterInfo.Create("rotate=<color=yellow>reset</color> or " + ParameterInfo.YawRollPitch("rotate", text, index)[0].Substring(1)) : ParameterInfo.YawRollPitch("rotate", text, index);
				}
			},
			{
				"center",
				(int index) => ParameterInfo.XZY("center", "Overrides the player position. For <color=yellow>rotate</color> sets also the rotation center point.", index)
			},
			{
				"from",
				(int index) => ParameterInfo.XZY("center", "Overrides the player position. For <color=yellow>rotate</color> sets also the rotation center point.", index)
			},
			{
				"visual",
				(int index) => SharedObjectAutoComplete.VisualAutoComplete("visual", index)
			},
			{
				"origin",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Origin
			},
			{
				"rect",
				(int index) => index switch
				{
					0 => ParameterInfo.Create("rect=<color=yellow>size</color> or rect=<color=yellow>width</color>,depth", "Area of affected objects."), 
					1 => ParameterInfo.Create("rect=width,<color=yellow>depth</color>", "Area of affected objects."), 
					_ => ParameterInfo.None, 
				}
			},
			{
				"angle",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("angle=<color=yellow>degrees</color>", "Direction of the rectangle when used with <color=yellow>rect</color>.")
			},
			{
				"circle",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("circle=<color=yellow>number</color>", "Radius of affected objects.")
			},
			{
				"radius",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("radius=<color=yellow>number</color>", "Radius of affected objects.")
			},
			{
				"creator",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("creator=<color=yellow>player ID</color>", "Sets creator of objects (0 for no creator).")
			},
			{
				"chance",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("chance=<color=yellow>number</color>", "Chance to affect the object (from 0.0 to 1.0).")
			},
			{
				"match",
				(int index) => (index != 0) ? ParameterInfo.None : DataLoading.DataKeys
			},
			{
				"unmatch",
				(int index) => (index != 0) ? ParameterInfo.None : DataLoading.DataKeys
			},
			{
				"copy",
				(int index) => ParameterInfo.Flag("copy")
			}
		}));
	}
}
