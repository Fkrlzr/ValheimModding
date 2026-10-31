using System;
using System.Collections.Generic;
using ServerDevcommands;

namespace WorldEditCommands;

public class SpawnLocationAutoComplete
{
	public List<string> NamedParameters = new List<string>(6) { "seed", "dungeonSeed", "pos", "rot", "from", "refRot" };

	public SpawnLocationAutoComplete()
	{
		NamedParameters.Sort();
		AutoComplete.Register("spawn_location", (Func<int, List<string>>)((int index) => (index == 0) ? ParameterInfo.LocationIds : NamedParameters), new Dictionary<string, Func<int, List<string>>>
		{
			{
				"seed",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("seed", "integer", "Sets the location appearance (if randomized).")
			},
			{
				"dungeonSeed",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("dungeonSeed", "integer", "Sets the room layout for the next dungeon.")
			},
			{
				"pos",
				(int index) => ParameterInfo.XZY("pos", "Offset from the player / reference position", index)
			},
			{
				"from",
				(int index) => ParameterInfo.XZY("from", "Overrides the reference position (player's position)", index)
			},
			{
				"rot",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("rot", "degrees", "Sets the location rotation. Randomized by default.")
			},
			{
				"refRot",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("refRot", "degrees", "Overrides the reference rotation (player's rotation).")
			}
		});
	}
}
