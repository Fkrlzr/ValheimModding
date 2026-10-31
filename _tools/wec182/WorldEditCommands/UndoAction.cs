using System;
using System.Collections.Generic;
using System.Linq;
using ServerDevcommands;
using UnityEngine;

namespace WorldEditCommands;

public class UndoAction : MonoBehaviour, IUndoAction
{
	public readonly UndoData[] Data = data.ToArray();

	public readonly UndoTerrain[] TerrainData = (terrainData ?? Array.Empty<UndoTerrain>()).ToArray();

	public UndoAction(IEnumerable<UndoData> data, IEnumerable<UndoTerrain>? terrainData = null)
	{
	}

	private string CreateMessage()
	{
		bool flag = Data.Length != 0;
		bool flag2 = TerrainData.Length != 0;
		if (flag && flag2)
		{
			return "Changed " + UndoHelper.Print(Data.Select((UndoData data) => data.Current)) + " + terrain";
		}
		if (flag)
		{
			return "Changed " + UndoHelper.Print(Data.Select((UndoData data) => data.Current));
		}
		if (flag2)
		{
			return "Changed terrain";
		}
		return "Changed";
	}

	public string Undo()
	{
		string result = CreateMessage();
		for (int num = TerrainData.Length - 1; num >= 0; num--)
		{
			TerrainData[num].Undo();
		}
		UndoData[] data = Data;
		for (int i = 0; i < data.Length; i++)
		{
			data[i].Undo();
		}
		return result;
	}

	public string Redo()
	{
		string result = CreateMessage();
		UndoData[] data = Data;
		for (int i = 0; i < data.Length; i++)
		{
			data[i].Redo();
		}
		UndoTerrain[] terrainData = TerrainData;
		for (int i = 0; i < terrainData.Length; i++)
		{
			terrainData[i].Redo();
		}
		return result;
	}
}
