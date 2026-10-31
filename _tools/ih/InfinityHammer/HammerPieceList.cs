using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace InfinityHammer;

internal sealed class HammerPieceList : IPieceList
{
	[CompilerGenerated]
	private IPieceList _003Coriginal_003EP;

	private PieceTable? table;

	private bool Custom
	{
		get
		{
			if (Object.op_Implicit((Object)(object)table))
			{
				return Hammer.IsInfinityHammer(table);
			}
			return false;
		}
	}

	public string DisplayName => _003Coriginal_003EP.DisplayName;

	public bool ShowTags
	{
		get
		{
			if (!Custom)
			{
				return _003Coriginal_003EP.ShowTags;
			}
			return true;
		}
	}

	public bool CanCustomizeTags
	{
		get
		{
			if (!Custom)
			{
				return _003Coriginal_003EP.CanCustomizeTags;
			}
			return false;
		}
	}

	public int TagCount
	{
		get
		{
			if (!Custom)
			{
				return _003Coriginal_003EP.TagCount;
			}
			return table.m_categories.Count;
		}
	}

	public int TagSeparatorIndex
	{
		get
		{
			if (!Custom)
			{
				return _003Coriginal_003EP.TagSeparatorIndex;
			}
			return -1;
		}
	}

	public HammerPieceList(IPieceList original)
	{
		_003Coriginal_003EP = original;
		base._002Ector();
	}

	public string GetTagDisplayName(int index)
	{
		if (!Custom)
		{
			return _003Coriginal_003EP.GetTagDisplayName(index);
		}
		return table.m_categoryLabels[index];
	}

	public int GetTagIdByIndex(int index)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected I4, but got Unknown
		if (!Custom)
		{
			return _003Coriginal_003EP.GetTagIdByIndex(index);
		}
		return (int)table.m_categories[index];
	}

	public void UpdateAvailableTags(PieceTable buildTool)
	{
		table = buildTool;
		if (!Custom)
		{
			_003Coriginal_003EP.UpdateAvailableTags(buildTool);
		}
	}

	public void GetAvailablePiecesWithTag(int tag, PieceTable buildTool, IList<Piece> pieces)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected I4, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between I4 and Unknown
		if (!Hammer.IsInfinityHammer(buildTool))
		{
			_003Coriginal_003EP.GetAvailablePiecesWithTag(tag, buildTool, pieces);
			return;
		}
		HashSet<Piece> hashSet = new HashSet<Piece>();
		foreach (PieceCategory category in buildTool.m_categories)
		{
			if (tag != -1 && tag != (int)category)
			{
				continue;
			}
			foreach (Piece item in buildTool.m_availablePiecesByCategory[(int)category])
			{
				if (Object.op_Implicit((Object)(object)item) && hashSet.Add(item))
				{
					pieces.Add(item);
				}
			}
		}
	}
}
