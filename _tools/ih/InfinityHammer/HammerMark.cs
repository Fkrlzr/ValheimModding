using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HarmonyLib;
using ServerDevcommands;
using Service;
using UnityEngine;

namespace InfinityHammer;

[HarmonyPatch]
public class HammerMark
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<WearNTear, bool> _003C_003E9__3_0;

		public static Func<WearNTear, ZNetView> _003C_003E9__3_1;

		public static Func<int, List<string>> _003C_003E9__4_3;

		public static Func<int, List<string>> _003C_003E9__4_4;

		public static Func<int, List<string>> _003C_003E9__4_5;

		public static Func<int, int, List<string>> _003C_003E9__4_1;

		public static Func<ZNetView, WearNTear> _003C_003E9__4_6;

		public static Func<WearNTear, bool> _003C_003E9__4_7;

		public static Func<WearNTear, bool> _003C_003E9__4_8;

		public static ConsoleEvent _003C_003E9__4_2;

		internal bool _003CGetMarkedAsViews_003Eb__3_0(WearNTear piece)
		{
			if (Object.op_Implicit((Object)(object)piece))
			{
				return Object.op_Implicit((Object)(object)((Component)piece).GetComponent<ZNetView>());
			}
			return false;
		}

		internal ZNetView _003CGetMarkedAsViews_003Eb__3_1(WearNTear piece)
		{
			return ((Component)piece).GetComponent<ZNetView>();
		}

		internal List<string> _003C_002Ector_003Eb__4_3(int index)
		{
			return ParameterInfo.Create("Selects whole building.");
		}

		internal List<string> _003C_002Ector_003Eb__4_4(int index)
		{
			return ParameterInfo.ObjectIds;
		}

		internal List<string> _003C_002Ector_003Eb__4_5(int index)
		{
			return ParameterInfo.ObjectIds;
		}

		internal List<string> _003C_002Ector_003Eb__4_1(int index, int subIndex)
		{
			return ParameterInfo.None;
		}

		internal void _003C_002Ector_003Eb__4_2(ConsoleEventArgs args)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			HammerParameters hammerParameters = new HammerParameters(args);
			ZNetView[] array = Array.Empty<ZNetView>();
			if (hammerParameters.Radius != null)
			{
				array = Selector.GetNearby(hammerParameters.Included, hammerParameters.Components, hammerParameters.Ignored, hammerParameters.Position, hammerParameters.Radius, hammerParameters.Height);
			}
			else if (hammerParameters.Width != null && hammerParameters.Depth != null)
			{
				array = Selector.GetNearby(hammerParameters.Included, hammerParameters.Components, hammerParameters.Ignored, hammerParameters.Position, hammerParameters.Angle, hammerParameters.Width, hammerParameters.Depth, hammerParameters.Height);
			}
			else if (args.Length > 1 && !args[1].Contains("=") && !hammerParameters.Connect && !hammerParameters.Pick && !hammerParameters.Freeze)
			{
				GameObject val = ZNetScene.instance.GetPrefab(args[1]) ?? throw new InvalidOperationException("Object not found.");
				array = (ZNetView[])(object)new ZNetView[1] { val.GetComponent<ZNetView>() };
			}
			else
			{
				ZNetView val2 = Selector.GetHovered(Configuration.Range, hammerParameters.Included, hammerParameters.Ignored) ?? throw new InvalidOperationException("Nothing is being hovered.");
				array = (ZNetView[])((!hammerParameters.Connect) ? ((Array)new ZNetView[1] { val2 }) : ((Array)Selector.GetConnected(val2, hammerParameters.Included, hammerParameters.Ignored)));
			}
			WearNTear[] array2 = (from view in array
				select ((Component)view).GetComponent<WearNTear>() into wtr
				where Object.op_Implicit((Object)(object)wtr)
				select wtr).ToArray();
			if (array2.Length == 0)
			{
				return;
			}
			if (array2.All((WearNTear wtr) => MarkedPieces.Contains(wtr)))
			{
				WearNTear[] array3 = array2;
				foreach (WearNTear val3 in array3)
				{
					val3.ResetHighlight();
					MarkedPieces.Remove(val3);
				}
				HammerHelper.Message(args.Context, $"Unmarked {array2.Length} pieces (total {MarkedPieces.Count})");
			}
			else
			{
				WearNTear[] array3 = array2;
				foreach (WearNTear val4 in array3)
				{
					MarkedPieces.Add(val4);
					val4.Highlight();
				}
				HammerHelper.Message(args.Context, $"Marked {array2.Length}  pieces (total {MarkedPieces.Count})");
			}
		}

		internal WearNTear _003C_002Ector_003Eb__4_6(ZNetView view)
		{
			return ((Component)view).GetComponent<WearNTear>();
		}

		internal bool _003C_002Ector_003Eb__4_7(WearNTear wtr)
		{
			return Object.op_Implicit((Object)(object)wtr);
		}

		internal bool _003C_002Ector_003Eb__4_8(WearNTear wtr)
		{
			return MarkedPieces.Contains(wtr);
		}
	}

	public static HashSet<WearNTear> MarkedPieces = new HashSet<WearNTear>();

	[HarmonyPatch(typeof(Player), "UpdateWearNTearHover")]
	[HarmonyPrefix]
	private static bool HighlightSelected()
	{
		foreach (WearNTear markedPiece in MarkedPieces)
		{
			markedPiece.Highlight();
		}
		return MarkedPieces.Count == 0;
	}

	public static void ClearMarked()
	{
		MarkedPieces.Clear();
	}

	public static ZNetView[] GetMarkedAsViews()
	{
		List<ZNetView> list = new List<ZNetView>();
		list.AddRange(from piece in MarkedPieces
			where Object.op_Implicit((Object)(object)piece) && Object.op_Implicit((Object)(object)((Component)piece).GetComponent<ZNetView>())
			select ((Component)piece).GetComponent<ZNetView>());
		return list.ToArray();
	}

	public HammerMark()
	{
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		List<string> named = new List<string>(3) { "connect", "include", "ignore" };
		named.Sort();
		AutoComplete.Register("hammer_mark", (Func<int, int, List<string>>)((int index, int subIndex) => (index == 0) ? ParameterInfo.ObjectIds : named), new Dictionary<string, Func<int, List<string>>>
		{
			{
				"connect",
				(int index) => ParameterInfo.Create("Selects whole building.")
			},
			{
				"include",
				(int index) => ParameterInfo.ObjectIds
			},
			{
				"ignore",
				(int index) => ParameterInfo.ObjectIds
			}
		});
		AutoComplete.Register("hammer_mark", (Func<int, int, List<string>>)((int index, int subIndex) => ParameterInfo.None));
		object obj = _003C_003Ec._003C_003E9__4_2;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				HammerParameters hammerParameters = new HammerParameters(args);
				ZNetView[] array = Array.Empty<ZNetView>();
				if (hammerParameters.Radius != null)
				{
					array = Selector.GetNearby(hammerParameters.Included, hammerParameters.Components, hammerParameters.Ignored, hammerParameters.Position, hammerParameters.Radius, hammerParameters.Height);
				}
				else if (hammerParameters.Width != null && hammerParameters.Depth != null)
				{
					array = Selector.GetNearby(hammerParameters.Included, hammerParameters.Components, hammerParameters.Ignored, hammerParameters.Position, hammerParameters.Angle, hammerParameters.Width, hammerParameters.Depth, hammerParameters.Height);
				}
				else if (args.Length > 1 && !args[1].Contains("=") && !hammerParameters.Connect && !hammerParameters.Pick && !hammerParameters.Freeze)
				{
					GameObject val2 = ZNetScene.instance.GetPrefab(args[1]) ?? throw new InvalidOperationException("Object not found.");
					array = (ZNetView[])(object)new ZNetView[1] { val2.GetComponent<ZNetView>() };
				}
				else
				{
					ZNetView val3 = Selector.GetHovered(Configuration.Range, hammerParameters.Included, hammerParameters.Ignored) ?? throw new InvalidOperationException("Nothing is being hovered.");
					array = (ZNetView[])((!hammerParameters.Connect) ? ((Array)new ZNetView[1] { val3 }) : ((Array)Selector.GetConnected(val3, hammerParameters.Included, hammerParameters.Ignored)));
				}
				WearNTear[] array2 = (from view in array
					select ((Component)view).GetComponent<WearNTear>() into wtr
					where Object.op_Implicit((Object)(object)wtr)
					select wtr).ToArray();
				if (array2.Length != 0)
				{
					if (array2.All((WearNTear wtr) => MarkedPieces.Contains(wtr)))
					{
						WearNTear[] array3 = array2;
						foreach (WearNTear val4 in array3)
						{
							val4.ResetHighlight();
							MarkedPieces.Remove(val4);
						}
						HammerHelper.Message(args.Context, $"Unmarked {array2.Length} pieces (total {MarkedPieces.Count})");
					}
					else
					{
						WearNTear[] array3 = array2;
						foreach (WearNTear val5 in array3)
						{
							MarkedPieces.Add(val5);
							val5.Highlight();
						}
						HammerHelper.Message(args.Context, $"Marked {array2.Length}  pieces (total {MarkedPieces.Count})");
					}
				}
			};
			_003C_003Ec._003C_003E9__4_2 = val;
			obj = (object)val;
		}
		Helper.Command("hammer_mark", "- Marks the selected object for later selection.", (ConsoleEvent)obj);
	}
}
