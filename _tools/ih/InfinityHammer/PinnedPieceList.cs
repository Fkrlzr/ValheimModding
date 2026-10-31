using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace InfinityHammer;

internal sealed class PinnedPieceList : IPieceList
{
	[CompilerGenerated]
	private IPieceList _003Coriginal_003EP;

	public string DisplayName => _003Coriginal_003EP.DisplayName;

	public bool ShowTags => _003Coriginal_003EP.ShowTags;

	public bool CanCustomizeTags => _003Coriginal_003EP.CanCustomizeTags;

	public int TagCount => _003Coriginal_003EP.TagCount;

	public int TagSeparatorIndex => _003Coriginal_003EP.TagSeparatorIndex;

	public PinnedPieceList(IPieceList original)
	{
		_003Coriginal_003EP = original;
		base._002Ector();
	}

	public string GetTagDisplayName(int index)
	{
		return _003Coriginal_003EP.GetTagDisplayName(index);
	}

	public int GetTagIdByIndex(int index)
	{
		return _003Coriginal_003EP.GetTagIdByIndex(index);
	}

	public void UpdateAvailableTags(PieceTable buildTool)
	{
		_003Coriginal_003EP.UpdateAvailableTags(buildTool);
	}

	internal static int PinIndex(Piece piece)
	{
		BuildMenuTool buildMenuTool = default(BuildMenuTool);
		if (!Object.op_Implicit((Object)(object)piece) || !((Component)piece).TryGetComponent<BuildMenuTool>(ref buildMenuTool))
		{
			return -1;
		}
		return buildMenuTool.tool?.Name switch
		{
			"Pipette" => 0, 
			"Building pipette" => 1, 
			"Area pipette" => 2, 
			_ => -1, 
		};
	}

	public void GetAvailablePiecesWithTag(int tag, PieceTable buildTool, IList<Piece> pieces)
	{
		_003Coriginal_003EP.GetAvailablePiecesWithTag(tag, buildTool, pieces);
		Piece val = null;
		Piece val2 = null;
		Piece val3 = null;
		foreach (Piece availablePiece in buildTool.m_availablePieces)
		{
			switch (PinIndex(availablePiece))
			{
			case 0:
				val = availablePiece;
				break;
			case 1:
				val2 = availablePiece;
				break;
			case 2:
				val3 = availablePiece;
				break;
			}
		}
		for (int num = pieces.Count - 1; num >= 0; num--)
		{
			if (PinIndex(pieces[num]) >= 0)
			{
				pieces.RemoveAt(num);
			}
		}
		if (Object.op_Implicit((Object)(object)val3))
		{
			pieces.Insert(0, val3);
		}
		if (Object.op_Implicit((Object)(object)val2))
		{
			pieces.Insert(0, val2);
		}
		if (Object.op_Implicit((Object)(object)val))
		{
			pieces.Insert(0, val);
		}
	}
}
