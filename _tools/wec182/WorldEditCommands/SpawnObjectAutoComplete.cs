using System;
using System.Collections.Generic;
using Data;
using ServerDevcommands;

namespace WorldEditCommands;

public class SpawnObjectAutoComplete : SharedObjectAutoComplete
{
	public List<string> NamedParameters;

	public SpawnObjectAutoComplete()
	{
		NamedParameters = SharedObjectAutoComplete.WithSharedParameters(new List<string>(14)
		{
			"hunt", "durability", "name", "crafter", "variant", "amount", "pos", "rot", "refPlayer", "from",
			"refRot", "to", "data", "crafterId"
		});
		AutoComplete.Register("spawn_object", (Func<int, List<string>>)((int index) => (index != 0) ? NamedParameters : ParameterInfo.ObjectIds), SharedObjectAutoComplete.WithSharedFetchers(new Dictionary<string, Func<int, List<string>>>
		{
			{
				"data",
				(int index) => DataLoading.DataKeys
			},
			{
				"crafterId",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("crafterId=<color=yellow>number</color>", "Sets the crafter player ID.")
			},
			{
				"hunt",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("hunt=<color=yellow>true/false</color> or no value for true.", "Sets is the creature in aggressive mode.")
			},
			{
				"name",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("name", "string", "Name for tameable creatures.")
			},
			{
				"crafter",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("name", "string", "Crafter for items.")
			},
			{
				"variant",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.CreateWithMinMax("variant", "integer", "Variant for items.")
			},
			{
				"amount",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.CreateWithMinMax("amount", "integer", "Amount of spawned objects.")
			},
			{
				"pos",
				(int index) => ParameterInfo.FRU("pos", "Offset from the player position", index)
			},
			{
				"to",
				(int index) => ParameterInfo.XZY("to", "End position for multiple objects", index)
			},
			{
				"from",
				(int index) => ParameterInfo.XZY("from", "Overrides the player position", index)
			},
			{
				"refPlayer",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.PlayerNames
			},
			{
				"rot",
				(int index) => ParameterInfo.YawRollPitch("rot", "Rotation from the player rotation", index)
			},
			{
				"refRot",
				(int index) => ParameterInfo.YawRollPitch("refRot", "Overrides the player rotation", index)
			},
			{
				"circle",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("circle=<color=yellow>number</color>", "Maximum spawn distance when spawning multiple objects. Default is 0.5 meters.")
			},
			{
				"radius",
				(int index) => (index != 0) ? ParameterInfo.None : ParameterInfo.Create("radius=<color=yellow>number</color>", "Maximum spawn distance when spawning multiple objects. Default is 0.5 meters.")
			}
		}));
	}
}
