using System;
using System.Collections.Generic;
using System.Linq;
using ServerDevcommands;

namespace WorldEditCommands;

public class TerrainAutoComplete
{
	public List<string> NamedParameters = new List<string>(24)
	{
		"lower", "level", "raise", "reset", "paint", "delta", "blockcheck", "rect", "circle", "smooth",
		"slope", "angle", "offset", "step", "from", "to", "min", "max", "within", "chance",
		"path", "arc", "curve", "point"
	};

	public TerrainAutoComplete()
	{
		NamedParameters.Sort();
		List<string> paints = TerrainCommand.Paints.Keys.ToList();
		paints.Sort();
		AutoComplete.Register("terrain", (Func<int, List<string>>)((int index) => NamedParameters), new Dictionary<string, Func<int, List<string>>>
		{
			{
				"chance",
				(int index) => ParameterInfo.Create("chance=<color=yellow>number</color> (from 0.0 to 1.0)", "Chance of the operation being applied to a node.")
			},
			{
				"reset",
				(int index) => ParameterInfo.Flag("Reset")
			},
			{
				"void",
				(int index) => ParameterInfo.Flag("Void", "Removes all terrain.")
			},
			{
				"blockcheck",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("blockcheck=<color=yellow>inverse</color>/<color=yellow>off</color>/<color=yellow>on</color>", "When <color=yellow>on</color>, excludes terrain under structures. When <color=yellow>inverse</color>, only includes terrain under structures.")
			},
			{
				"rect",
				(int index) => index switch
				{
					0 => ParameterInfo.Create("rect=<color=yellow>size</color> or rect=<color=yellow>width</color>,depth", "Size of the included terrain (rectangle)."), 
					1 => ParameterInfo.Create("rect=width,<color=yellow>depth</color>", "Size of the included terrain (rectangle)."), 
					_ => ParameterInfo.None, 
				}
			},
			{
				"min",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("min", "y", "Raises terrain below the given y coordinate to the y coordinate.")
			},
			{
				"max",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("max", "y", "Lowers terrain above the given y coordinate to the y coordinate.")
			},
			{
				"angle",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("angle=<color=yellow>number</color>/<color=yellow>n</color>/<color=yellow>ne</color>/<color=yellow>e</color>/<color=yellow>se</color>/<color=yellow>s</color>/<color=yellow>sw</color>/<color=yellow>w</color>/<color=yellow>nw</color> (from 0 to 360)", "Direction of the shape.")
			},
			{
				"circle",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("circle=<color=yellow>number</color>", "Radius of the included terrain.")
			},
			{
				"offset",
				(int index) => ParameterInfo.FRU("offset", "Position of the center", index)
			},
			{
				"from",
				(int index) => ParameterInfo.XZY("from", "Overrides the player position", index)
			},
			{
				"to",
				(int index) => ParameterInfo.XZY("to", "Determines the angle and circle/rect distance. With <color=yellow>path</color>, adds a point.", index)
			},
			{
				"point",
				(int index) => ParameterInfo.XZY("point", "Adds a point to the path. Can be used multiple times (shorthand <color=yellow>p</color>).", index)
			},
			{
				"p",
				(int index) => ParameterInfo.XZY("p", "Adds a point to the path. Can be used multiple times.", index)
			},
			{
				"path",
				(int index) => ParameterInfo.Flag("Path", "Affects terrain along straight lines between the points. Width is set with <color=yellow>circle</color> (round ends) or <color=yellow>rect</color> (flat ends).")
			},
			{
				"arc",
				(int index) => ParameterInfo.Flag("Arc", "Like <color=yellow>path</color> but connects the points with circular arcs.")
			},
			{
				"curve",
				(int index) => ParameterInfo.Flag("Curve", "Like <color=yellow>path</color> but connects the points with a smooth curve.")
			},
			{
				"step",
				(int index) => ParameterInfo.FRU("step", "Offset based on radius", index)
			},
			{
				"smooth",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("smooth=<color=yellow>number</color> (from 0.0 to 1.0)", "Higher values smoothen the effect near edges.")
			},
			{
				"slope",
				(int index) => index switch
				{
					0 => ParameterInfo.Create("slope=<color=yellow>meters</color>,angle=0", "Creates a slope with height difference of a given amount."), 
					1 => ParameterInfo.Create("slope=meters,<color=yellow>angle</color>", "Changes the slope direction (added to the general angle)."), 
					_ => ParameterInfo.None, 
				}
			},
			{
				"delta",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("delta", "meters", "Sets the terrain elevation difference from the original.")
			},
			{
				"raise",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("raise", "meters", "Raises the terrain by a given amount.")
			},
			{
				"lower",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("lower", "meters", "Lowers the terrain by a given amount.")
			},
			{
				"level",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("level or level=<color=yellow>y</color>", "Levels the terrain to a given y coordinate. Without parameters, levels to the terrain height below the player (or to the path height with <color=yellow>path</color>).")
			},
			{
				"within",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("within", "min-max", "Only includes terrain within the given y coordinate range.")
			},
			{
				"paint",
				(int index) => index switch
				{
					0 => paints, 
					1 => ParameterInfo.Create("paint=dirt,<color=yellow>cultivated,vegetation</color>,paved", "Custom color (values from 0.0 to 1.0)."), 
					2 => ParameterInfo.Create("paint=dirt,cultivated,<color=yellow>paved</color>,vegetation", "Custom color (values from 0.0 to 1.0)."), 
					3 => ParameterInfo.Create("paint=dirt,cultivated,paved,<color=yellow>vegetation</color>", "Custom color (values from 0.0 to 1.0)."), 
					_ => ParameterInfo.None, 
				}
			},
			{
				"id",
				(int index) => ParameterInfo.Ids
			},
			{
				"ignore",
				(int index) => ParameterInfo.Ids
			}
		});
	}
}
