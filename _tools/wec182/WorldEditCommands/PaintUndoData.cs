using UnityEngine;

namespace WorldEditCommands;

public class PaintUndoData
{
	public int Index;

	public bool PaintModified;

	public Color Paint = Color.black;
}
