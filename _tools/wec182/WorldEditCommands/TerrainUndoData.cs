using System.Collections.Generic;

namespace WorldEditCommands;

public class TerrainUndoData
{
	public List<HeightUndoData> Heights = new List<HeightUndoData>();

	public List<PaintUndoData> Paints = new List<PaintUndoData>();
}
