using System.Collections.Generic;
using UnityEngine;

namespace InfinityHammer;

internal static class ToolMenuPieces
{
	private class Table
	{
		private readonly Dictionary<Tool, BuildMenuTool> Pieces = new Dictionary<Tool, BuildMenuTool>();

		private bool Dirty;

		public void Invalidate()
		{
			Dirty = true;
		}

		public BuildMenuTool Get(Tool tool)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			if (Dirty)
			{
				Purge();
			}
			if (!Pieces.TryGetValue(tool, out BuildMenuTool value) || !Object.op_Implicit((Object)(object)value))
			{
				GameObject val = new GameObject("_IH_Tool_" + tool.Name);
				val.SetActive(false);
				value = val.AddComponent<BuildMenuTool>();
				value.tool = tool;
				Pieces[tool] = value;
			}
			((Piece)value).m_description = tool.Description;
			((Piece)value).m_name = tool.Name;
			((Piece)value).m_icon = tool.Icon;
			return value;
		}

		private void Purge()
		{
			foreach (BuildMenuTool value in Pieces.Values)
			{
				if (Object.op_Implicit((Object)(object)value))
				{
					Object.Destroy((Object)(object)((Component)value).gameObject);
				}
			}
			Pieces.Clear();
			Dirty = false;
		}
	}

	private static readonly Dictionary<PieceTable, Table> Tables = new Dictionary<PieceTable, Table>();

	internal static BuildMenuTool Get(PieceTable pt, Tool tool)
	{
		if (!Tables.TryGetValue(pt, out Table value))
		{
			value = (Tables[pt] = new Table());
		}
		return value.Get(tool);
	}

	public static void Invalidate()
	{
		foreach (Table value in Tables.Values)
		{
			value.Invalidate();
		}
	}
}
