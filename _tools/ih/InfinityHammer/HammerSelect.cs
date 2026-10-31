using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Data;
using ServerDevcommands;
using Service;
using UnityEngine;
using WorldEditCommands;

namespace InfinityHammer;

public class HammerSelect
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<int, List<string>> _003C_003E9__4_2;

		public static Func<int, List<string>> _003C_003E9__4_3;

		public static Func<int, List<string>> _003C_003E9__4_4;

		public static Func<int, List<string>> _003C_003E9__4_5;

		public static Func<int, List<string>> _003C_003E9__4_6;

		public static Func<int, List<string>> _003C_003E9__4_7;

		public static Func<int, List<string>> _003C_003E9__4_8;

		public static Func<int, List<string>> _003C_003E9__4_9;

		public static Func<int, List<string>> _003C_003E9__4_17;

		public static Func<int, List<string>> _003C_003E9__4_18;

		public static Func<int, List<string>> _003C_003E9__4_19;

		public static Func<int, List<string>> _003C_003E9__4_20;

		public static Func<int, List<string>> _003C_003E9__4_21;

		public static Func<int, List<string>> _003C_003E9__4_22;

		public static Func<int, List<string>> _003C_003E9__4_23;

		public static Func<int, List<string>> _003C_003E9__4_24;

		public static ConsoleEvent _003C_003E9__4_1;

		internal List<string> _003C_002Ector_003Eb__4_2(int index)
		{
			return ParameterInfo.Scale("scale", "Size of the object (if the object can be scaled).", index);
		}

		internal List<string> _003C_002Ector_003Eb__4_3(int index)
		{
			return ParameterInfo.Create("Level.");
		}

		internal List<string> _003C_002Ector_003Eb__4_4(int index)
		{
			return ParameterInfo.Create("Stars.");
		}

		internal List<string> _003C_002Ector_003Eb__4_5(int index)
		{
			return ParameterInfo.Create("Text.");
		}

		internal List<string> _003C_002Ector_003Eb__4_6(int index)
		{
			return ParameterInfo.Create("Health.");
		}

		internal List<string> _003C_002Ector_003Eb__4_7(int index)
		{
			return ParameterInfo.Create("Freezes in the place.");
		}

		internal List<string> _003C_002Ector_003Eb__4_8(int index)
		{
			return ParameterInfo.Create("Picks up the selection.");
		}

		internal List<string> _003C_002Ector_003Eb__4_9(int index)
		{
			return ParameterInfo.Create("Selects whole building.");
		}

		internal List<string> _003C_002Ector_003Eb__4_17(int index)
		{
			return ParameterInfo.ObjectIds;
		}

		internal List<string> _003C_002Ector_003Eb__4_18(int index)
		{
			return ParameterInfo.ObjectIds;
		}

		internal List<string> _003C_002Ector_003Eb__4_19(int index)
		{
			return ParameterInfo.ObjectIds;
		}

		internal List<string> _003C_002Ector_003Eb__4_20(int index)
		{
			return ParameterInfo.Components;
		}

		internal List<string> _003C_002Ector_003Eb__4_21(int index)
		{
			return DataLoading.DataKeys;
		}

		internal List<string> _003C_002Ector_003Eb__4_22(int index)
		{
			return ParameterInfo.Create("Terrain radius offset for including terrain height and paint in selection.");
		}

		internal List<string> _003C_002Ector_003Eb__4_23(int index)
		{
			return ParameterInfo.Create("Paint radius offset for including terrain paint in selection.");
		}

		internal List<string> _003C_002Ector_003Eb__4_24(int index)
		{
			return ParameterInfo.Create("Terrain smooth (0.0-1.0) for height or height,paint.");
		}

		internal void _003C_002Ector_003Eb__4_1(ConsoleEventArgs args)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Expected O, but got Unknown
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Expected O, but got Unknown
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Expected O, but got Unknown
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Expected O, but got Unknown
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Expected O, but got Unknown
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Expected O, but got Unknown
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Expected O, but got Unknown
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Expected O, but got Unknown
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Expected O, but got Unknown
			//IL_035f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Expected O, but got Unknown
			//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_041b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0444: Unknown result type (might be due to invalid IL or missing references)
			//IL_079d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0789: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0594: Unknown result type (might be due to invalid IL or missing references)
			//IL_0596: Unknown result type (might be due to invalid IL or missing references)
			//IL_0598: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
			_003C_003Ec__DisplayClass4_1 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass4_1();
			HammerParameters hammerParameters = new HammerParameters(args);
			CS_0024_003C_003E8__locals0.views = Array.Empty<ZNetView>();
			if (hammerParameters.Radius != null)
			{
				CS_0024_003C_003E8__locals0.views = Selector.GetNearby(hammerParameters.Included, hammerParameters.Components, hammerParameters.Ignored, hammerParameters.Position, hammerParameters.Radius, hammerParameters.Height);
			}
			else if (hammerParameters.Width != null && hammerParameters.Depth != null)
			{
				CS_0024_003C_003E8__locals0.views = Selector.GetNearby(hammerParameters.Included, hammerParameters.Components, hammerParameters.Ignored, hammerParameters.Position, hammerParameters.Angle, hammerParameters.Width, hammerParameters.Depth, hammerParameters.Height);
			}
			else if (args.Length > 1 && !args[1].Contains("=") && !hammerParameters.Connect && !hammerParameters.Pick && !hammerParameters.Freeze)
			{
				GameObject val = ZNetScene.instance.GetPrefab(args[1]) ?? throw new InvalidOperationException("Object not found.");
				CS_0024_003C_003E8__locals0.views = (ZNetView[])(object)new ZNetView[1] { val.GetComponent<ZNetView>() };
			}
			else
			{
				ZNetView val2 = Selector.GetHovered(Configuration.Range, hammerParameters.Included, hammerParameters.Ignored) ?? throw new InvalidOperationException("Nothing is being hovered.");
				if (hammerParameters.Connect)
				{
					CS_0024_003C_003E8__locals0.views = Selector.GetConnected(val2, hammerParameters.Included, hammerParameters.Ignored);
				}
				else
				{
					CS_0024_003C_003E8__locals0.views = (ZNetView[])(object)new ZNetView[1] { val2 };
				}
			}
			if (CS_0024_003C_003E8__locals0.views.Length == 0)
			{
				return;
			}
			ZNetView[] array = (from view in HammerMark.GetMarkedAsViews()
				where !CS_0024_003C_003E8__locals0.views.Contains(view)
				select view).ToArray();
			if (array.Length != 0)
			{
				List<ZNetView> list = new List<ZNetView>(CS_0024_003C_003E8__locals0.views);
				list.AddRange(array);
				CS_0024_003C_003E8__locals0.views = list.ToArray();
				HammerMark.ClearMarked();
				HammerHelper.Message(args.Context, $"Including {array.Length} marked pieces in selection.");
			}
			HammerHelper.Init();
			DataEntry val3 = ((hammerParameters.Data == null) ? null : DataHelper.Get(hammerParameters.Data));
			if (hammerParameters.Health.HasValue)
			{
				if (val3 == null)
				{
					val3 = new DataEntry();
				}
				DataEntryExtensions.Set(val3, ZDOVars.s_health, hammerParameters.Health.Value);
			}
			if (hammerParameters.Level.HasValue)
			{
				if (val3 == null)
				{
					val3 = new DataEntry();
				}
				DataEntryExtensions.Set(val3, ZDOVars.s_level, hammerParameters.Level.Value);
			}
			if (hammerParameters.Growth != 0)
			{
				if (val3 == null)
				{
					val3 = new DataEntry();
				}
				DataEntryExtensions.Set(val3, Hashes.Growth, GrowthNumber(hammerParameters.Growth));
				DataEntryExtensions.Set(val3, ZDOVars.s_plantTime, DateTime.MaxValue.Ticks / 2);
			}
			if (hammerParameters.Wear != 0)
			{
				if (val3 == null)
				{
					val3 = new DataEntry();
				}
				DataEntryExtensions.Set(val3, Hashes.Wear, WearNumber(hammerParameters.Wear));
			}
			if (hammerParameters.Fall != 0)
			{
				if (val3 == null)
				{
					val3 = new DataEntry();
				}
				DataEntryExtensions.Set(val3, Hashes.Fall, FallNumber(hammerParameters.Fall));
			}
			if (!hammerParameters.Collision)
			{
				if (val3 == null)
				{
					val3 = new DataEntry();
				}
				DataEntryExtensions.Set(val3, Hashes.Collision, false);
			}
			if (!hammerParameters.Show)
			{
				if (val3 == null)
				{
					val3 = new DataEntry();
				}
				DataEntryExtensions.Set(val3, Hashes.Render, false);
			}
			if (!hammerParameters.Restrict)
			{
				if (val3 == null)
				{
					val3 = new DataEntry();
				}
				DataEntryExtensions.Set(val3, Hashes.Restrict, false);
			}
			if (!hammerParameters.Interact)
			{
				if (val3 == null)
				{
					val3 = new DataEntry();
				}
				DataEntryExtensions.Set(val3, Hashes.Interact, false);
			}
			if (hammerParameters.Text != null)
			{
				if (val3 == null)
				{
					val3 = new DataEntry();
				}
				DataEntryExtensions.Set(val3, Hashes.Text, hammerParameters.Text);
			}
			TerrainHeight terrainHeight = null;
			TerrainPaint terrainPaint = null;
			bool flag = hammerParameters.Terrain.HasValue || Configuration.IncludeTerrainHeight;
			bool flag2 = hammerParameters.Paint.HasValue || Configuration.IncludeTerrainPaint;
			if (flag || flag2)
			{
				float num = hammerParameters.Terrain.GetValueOrDefault() + Configuration.BlueprintTerrainHeightOffset;
				float num2 = hammerParameters.Paint.GetValueOrDefault() + Configuration.BlueprintTerrainPaintOffset;
				float blueprintTerrainNodeSpacing = Configuration.BlueprintTerrainNodeSpacing;
				float blueprintPaintNodeSpacing = Configuration.BlueprintPaintNodeSpacing;
				Vector3 position = ((Component)CS_0024_003C_003E8__locals0.views[0]).transform.position;
				Quaternion centerRot = ((CS_0024_003C_003E8__locals0.views.Length == 1) ? ((Component)CS_0024_003C_003E8__locals0.views[0]).transform.rotation : Quaternion.identity);
				Vector3 searchPos = position;
				if (hammerParameters.Radius != null || (hammerParameters.Width != null && hammerParameters.Depth != null))
				{
					searchPos = hammerParameters.Position;
				}
				if (hammerParameters.Width != null && hammerParameters.Depth != null)
				{
					Range<float> width = ((hammerParameters.Width.Min == hammerParameters.Width.Max) ? new Range<float>(hammerParameters.Width.Max + num) : new Range<float>(hammerParameters.Width.Min, hammerParameters.Width.Max + num));
					Range<float> depth = ((hammerParameters.Depth.Min == hammerParameters.Depth.Max) ? new Range<float>(hammerParameters.Depth.Max + num) : new Range<float>(hammerParameters.Depth.Min, hammerParameters.Depth.Max + num));
					Range<float> width2 = ((hammerParameters.Width.Min == hammerParameters.Width.Max) ? new Range<float>(hammerParameters.Width.Max + num2) : new Range<float>(hammerParameters.Width.Min, hammerParameters.Width.Max + num2));
					Range<float> depth2 = ((hammerParameters.Depth.Min == hammerParameters.Depth.Max) ? new Range<float>(hammerParameters.Depth.Max + num2) : new Range<float>(hammerParameters.Depth.Min, hammerParameters.Depth.Max + num2));
					if (flag)
					{
						terrainHeight = TerrainInfo.CollectTerrainHeightInRect(position, centerRot, searchPos, width, depth, hammerParameters.Angle, blueprintTerrainNodeSpacing);
					}
					if (flag2)
					{
						terrainPaint = TerrainInfo.CollectTerrainPaintInRect(position, centerRot, searchPos, width2, depth2, hammerParameters.Angle, blueprintPaintNodeSpacing);
					}
				}
				else
				{
					Range<float> radius = new Range<float>(Mathf.Max(1f, num));
					Range<float> radius2 = new Range<float>(Mathf.Max(1f, num2));
					if (hammerParameters.Radius != null)
					{
						radius = ((hammerParameters.Radius.Min == hammerParameters.Radius.Max) ? new Range<float>(hammerParameters.Radius.Max + num) : new Range<float>(hammerParameters.Radius.Min, hammerParameters.Radius.Max + num));
						radius2 = ((hammerParameters.Radius.Min == hammerParameters.Radius.Max) ? new Range<float>(hammerParameters.Radius.Max + num2) : new Range<float>(hammerParameters.Radius.Min, hammerParameters.Radius.Max + num2));
					}
					if (flag)
					{
						terrainHeight = TerrainInfo.CollectTerrainHeightInRadius(position, centerRot, searchPos, radius, blueprintTerrainNodeSpacing);
					}
					if (flag2)
					{
						terrainPaint = TerrainInfo.CollectTerrainPaintInRadius(position, centerRot, searchPos, radius2, blueprintPaintNodeSpacing);
					}
				}
				if (terrainHeight != null)
				{
					terrainHeight.Smooth = hammerParameters.HeightSmooth ?? Configuration.BlueprintTerrainHeightSmooth;
				}
				if (terrainPaint != null)
				{
					terrainPaint.Smooth = hammerParameters.PaintSmooth ?? Configuration.BlueprintTerrainPaintSmooth;
				}
			}
			GameObject obj = Selection.CreateGhost((CS_0024_003C_003E8__locals0.views.Length == 1) ? new ObjectSelection(CS_0024_003C_003E8__locals0.views[0], hammerParameters.Pick, hammerParameters.Scale, val3, terrainHeight, terrainPaint) : new ObjectSelection((IEnumerable<ZNetView>)CS_0024_003C_003E8__locals0.views, hammerParameters.Pick, hammerParameters.Scale, val3, terrainHeight, terrainPaint));
			Hammer.SelectEmpty();
			if (hammerParameters.Freeze)
			{
				Position.Freeze((CS_0024_003C_003E8__locals0.views.Length != 0) ? ((Component)CS_0024_003C_003E8__locals0.views[0]).transform.position : ((Component)Helper.GetPlayer()).transform.position);
			}
			if (hammerParameters.Pick)
			{
				UndoHelper.BeginAction();
				ZNetView[] views = CS_0024_003C_003E8__locals0.views;
				for (int i = 0; i < views.Length; i++)
				{
					UndoHelper.AddRemoveAction(views[i].GetZDO());
				}
				views = CS_0024_003C_003E8__locals0.views;
				for (int i = 0; i < views.Length; i++)
				{
					HammerHelper.RemoveZDO(views[i].GetZDO());
				}
				UndoHelper.EndAction();
			}
			PrintSelected(args.Context, obj);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass4_1
	{
		public ZNetView[] views;

		internal bool _003C_002Ector_003Eb__25(ZNetView view)
		{
			return !views.Contains(view);
		}
	}

	private static int WearNumber(Wear wear)
	{
		return wear switch
		{
			Wear.Broken => 0, 
			Wear.Damaged => 1, 
			Wear.Healthy => 2, 
			_ => -1, 
		};
	}

	private static int GrowthNumber(Growth growth)
	{
		return growth switch
		{
			Growth.Healthy => 0, 
			Growth.Unhealthy => 1, 
			Growth.HealthyGrown => 2, 
			Growth.UnhealthyGrown => 3, 
			_ => -1, 
		};
	}

	private static int FallNumber(Fall fall)
	{
		return fall switch
		{
			Fall.Off => 0, 
			Fall.Terrain => 1, 
			Fall.Solid => 2, 
			_ => -1, 
		};
	}

	private static void PrintSelected(Terminal terminal, GameObject obj)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		if (Configuration.DisableSelectMessages)
		{
			return;
		}
		Vector3 localScale = obj.transform.localScale;
		ZNetView component = obj.GetComponent<ZNetView>();
		string prefabName = Utils.GetPrefabName(obj.gameObject);
		if (Object.op_Implicit((Object)(object)component) && component.m_syncInitialScale)
		{
			if (localScale.x == localScale.y && localScale.y == localScale.z)
			{
				HammerHelper.Message(terminal, $"Selected {prefabName} (size {localScale.y:P0}).");
			}
			else
			{
				HammerHelper.Message(terminal, $"Selected {prefabName} (scale {localScale:F2}).");
			}
		}
		else
		{
			HammerHelper.Message(terminal, "Selected " + prefabName + ".");
		}
	}

	public HammerSelect()
	{
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		List<string> named = new List<string>(15)
		{
			"freeze", "pick", "scale", "level", "stars", "connect", "health", "type", "include", "ignore",
			"id", "data", "terrain", "paint", "smooth"
		};
		if (InfinityHammer.StructureTweaks)
		{
			List<string> collection = new List<string>(7) { "growth", "wear", "show", "collision", "interact", "fall", "restrict" };
			named.AddRange(collection);
		}
		List<string> Wears = new List<string>(4) { "default", "broken", "damaged", "healthy" };
		List<string> Growths = new List<string>(5) { "big", "big_bad", "default", "small", "small_bad" };
		List<string> Falls = new List<string>(3) { "off", "solid", "terrain" };
		List<string> False = new List<string>(1) { "false" };
		named.Sort();
		AutoComplete.Register("hammer", (Func<int, int, List<string>>)((int index, int subIndex) => (index == 0) ? ParameterInfo.ObjectIds : named), new Dictionary<string, Func<int, List<string>>>
		{
			{
				"scale",
				(int index) => ParameterInfo.Scale("scale", "Size of the object (if the object can be scaled).", index)
			},
			{
				"level",
				(int index) => ParameterInfo.Create("Level.")
			},
			{
				"stars",
				(int index) => ParameterInfo.Create("Stars.")
			},
			{
				"text",
				(int index) => ParameterInfo.Create("Text.")
			},
			{
				"health",
				(int index) => ParameterInfo.Create("Health.")
			},
			{
				"freeze",
				(int index) => ParameterInfo.Create("Freezes in the place.")
			},
			{
				"pick",
				(int index) => ParameterInfo.Create("Picks up the selection.")
			},
			{
				"connect",
				(int index) => ParameterInfo.Create("Selects whole building.")
			},
			{
				"wear",
				(int index) => Wears
			},
			{
				"fall",
				(int index) => Falls
			},
			{
				"growth",
				(int index) => Growths
			},
			{
				"show",
				(int index) => False
			},
			{
				"collision",
				(int index) => False
			},
			{
				"interact",
				(int index) => False
			},
			{
				"restrict",
				(int index) => False
			},
			{
				"include",
				(int index) => ParameterInfo.ObjectIds
			},
			{
				"ignore",
				(int index) => ParameterInfo.ObjectIds
			},
			{
				"id",
				(int index) => ParameterInfo.ObjectIds
			},
			{
				"type",
				(int index) => ParameterInfo.Components
			},
			{
				"data",
				(int index) => DataLoading.DataKeys
			},
			{
				"terrain",
				(int index) => ParameterInfo.Create("Terrain radius offset for including terrain height and paint in selection.")
			},
			{
				"paint",
				(int index) => ParameterInfo.Create("Paint radius offset for including terrain paint in selection.")
			},
			{
				"smooth",
				(int index) => ParameterInfo.Create("Terrain smooth (0.0-1.0) for height or height,paint.")
			}
		});
		object obj = _003C_003Ec._003C_003E9__4_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_0207: Unknown result type (might be due to invalid IL or missing references)
				//IL_020d: Expected O, but got Unknown
				//IL_0233: Unknown result type (might be due to invalid IL or missing references)
				//IL_0239: Expected O, but got Unknown
				//IL_025a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0260: Expected O, but got Unknown
				//IL_029d: Unknown result type (might be due to invalid IL or missing references)
				//IL_02a3: Expected O, but got Unknown
				//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ca: Expected O, but got Unknown
				//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f1: Expected O, but got Unknown
				//IL_0308: Unknown result type (might be due to invalid IL or missing references)
				//IL_030e: Expected O, but got Unknown
				//IL_0325: Unknown result type (might be due to invalid IL or missing references)
				//IL_032b: Expected O, but got Unknown
				//IL_0342: Unknown result type (might be due to invalid IL or missing references)
				//IL_0348: Expected O, but got Unknown
				//IL_035f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0365: Expected O, but got Unknown
				//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
				//IL_041b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0407: Unknown result type (might be due to invalid IL or missing references)
				//IL_0420: Unknown result type (might be due to invalid IL or missing references)
				//IL_0422: Unknown result type (might be due to invalid IL or missing references)
				//IL_0424: Unknown result type (might be due to invalid IL or missing references)
				//IL_043f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0444: Unknown result type (might be due to invalid IL or missing references)
				//IL_079d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0789: Unknown result type (might be due to invalid IL or missing references)
				//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_0594: Unknown result type (might be due to invalid IL or missing references)
				//IL_0596: Unknown result type (might be due to invalid IL or missing references)
				//IL_0598: Unknown result type (might be due to invalid IL or missing references)
				//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
				HammerParameters hammerParameters = new HammerParameters(args);
				ZNetView[] views = Array.Empty<ZNetView>();
				if (hammerParameters.Radius != null)
				{
					views = Selector.GetNearby(hammerParameters.Included, hammerParameters.Components, hammerParameters.Ignored, hammerParameters.Position, hammerParameters.Radius, hammerParameters.Height);
				}
				else if (hammerParameters.Width != null && hammerParameters.Depth != null)
				{
					views = Selector.GetNearby(hammerParameters.Included, hammerParameters.Components, hammerParameters.Ignored, hammerParameters.Position, hammerParameters.Angle, hammerParameters.Width, hammerParameters.Depth, hammerParameters.Height);
				}
				else if (args.Length > 1 && !args[1].Contains("=") && !hammerParameters.Connect && !hammerParameters.Pick && !hammerParameters.Freeze)
				{
					GameObject val2 = ZNetScene.instance.GetPrefab(args[1]) ?? throw new InvalidOperationException("Object not found.");
					views = (ZNetView[])(object)new ZNetView[1] { val2.GetComponent<ZNetView>() };
				}
				else
				{
					ZNetView val3 = Selector.GetHovered(Configuration.Range, hammerParameters.Included, hammerParameters.Ignored) ?? throw new InvalidOperationException("Nothing is being hovered.");
					if (hammerParameters.Connect)
					{
						views = Selector.GetConnected(val3, hammerParameters.Included, hammerParameters.Ignored);
					}
					else
					{
						views = (ZNetView[])(object)new ZNetView[1] { val3 };
					}
				}
				if (views.Length != 0)
				{
					ZNetView[] array = (from view in HammerMark.GetMarkedAsViews()
						where !views.Contains(view)
						select view).ToArray();
					if (array.Length != 0)
					{
						List<ZNetView> list = new List<ZNetView>(views);
						list.AddRange(array);
						views = list.ToArray();
						HammerMark.ClearMarked();
						HammerHelper.Message(args.Context, $"Including {array.Length} marked pieces in selection.");
					}
					HammerHelper.Init();
					DataEntry val4 = ((hammerParameters.Data == null) ? null : DataHelper.Get(hammerParameters.Data));
					if (hammerParameters.Health.HasValue)
					{
						if (val4 == null)
						{
							val4 = new DataEntry();
						}
						DataEntryExtensions.Set(val4, ZDOVars.s_health, hammerParameters.Health.Value);
					}
					if (hammerParameters.Level.HasValue)
					{
						if (val4 == null)
						{
							val4 = new DataEntry();
						}
						DataEntryExtensions.Set(val4, ZDOVars.s_level, hammerParameters.Level.Value);
					}
					if (hammerParameters.Growth != 0)
					{
						if (val4 == null)
						{
							val4 = new DataEntry();
						}
						DataEntryExtensions.Set(val4, Hashes.Growth, GrowthNumber(hammerParameters.Growth));
						DataEntryExtensions.Set(val4, ZDOVars.s_plantTime, DateTime.MaxValue.Ticks / 2);
					}
					if (hammerParameters.Wear != 0)
					{
						if (val4 == null)
						{
							val4 = new DataEntry();
						}
						DataEntryExtensions.Set(val4, Hashes.Wear, WearNumber(hammerParameters.Wear));
					}
					if (hammerParameters.Fall != 0)
					{
						if (val4 == null)
						{
							val4 = new DataEntry();
						}
						DataEntryExtensions.Set(val4, Hashes.Fall, FallNumber(hammerParameters.Fall));
					}
					if (!hammerParameters.Collision)
					{
						if (val4 == null)
						{
							val4 = new DataEntry();
						}
						DataEntryExtensions.Set(val4, Hashes.Collision, false);
					}
					if (!hammerParameters.Show)
					{
						if (val4 == null)
						{
							val4 = new DataEntry();
						}
						DataEntryExtensions.Set(val4, Hashes.Render, false);
					}
					if (!hammerParameters.Restrict)
					{
						if (val4 == null)
						{
							val4 = new DataEntry();
						}
						DataEntryExtensions.Set(val4, Hashes.Restrict, false);
					}
					if (!hammerParameters.Interact)
					{
						if (val4 == null)
						{
							val4 = new DataEntry();
						}
						DataEntryExtensions.Set(val4, Hashes.Interact, false);
					}
					if (hammerParameters.Text != null)
					{
						if (val4 == null)
						{
							val4 = new DataEntry();
						}
						DataEntryExtensions.Set(val4, Hashes.Text, hammerParameters.Text);
					}
					TerrainHeight terrainHeight = null;
					TerrainPaint terrainPaint = null;
					bool flag = hammerParameters.Terrain.HasValue || Configuration.IncludeTerrainHeight;
					bool flag2 = hammerParameters.Paint.HasValue || Configuration.IncludeTerrainPaint;
					if (flag || flag2)
					{
						float num = hammerParameters.Terrain.GetValueOrDefault() + Configuration.BlueprintTerrainHeightOffset;
						float num2 = hammerParameters.Paint.GetValueOrDefault() + Configuration.BlueprintTerrainPaintOffset;
						float blueprintTerrainNodeSpacing = Configuration.BlueprintTerrainNodeSpacing;
						float blueprintPaintNodeSpacing = Configuration.BlueprintPaintNodeSpacing;
						Vector3 position = ((Component)views[0]).transform.position;
						Quaternion centerRot = ((views.Length == 1) ? ((Component)views[0]).transform.rotation : Quaternion.identity);
						Vector3 searchPos = position;
						if (hammerParameters.Radius != null || (hammerParameters.Width != null && hammerParameters.Depth != null))
						{
							searchPos = hammerParameters.Position;
						}
						if (hammerParameters.Width != null && hammerParameters.Depth != null)
						{
							Range<float> width = ((hammerParameters.Width.Min == hammerParameters.Width.Max) ? new Range<float>(hammerParameters.Width.Max + num) : new Range<float>(hammerParameters.Width.Min, hammerParameters.Width.Max + num));
							Range<float> depth = ((hammerParameters.Depth.Min == hammerParameters.Depth.Max) ? new Range<float>(hammerParameters.Depth.Max + num) : new Range<float>(hammerParameters.Depth.Min, hammerParameters.Depth.Max + num));
							Range<float> width2 = ((hammerParameters.Width.Min == hammerParameters.Width.Max) ? new Range<float>(hammerParameters.Width.Max + num2) : new Range<float>(hammerParameters.Width.Min, hammerParameters.Width.Max + num2));
							Range<float> depth2 = ((hammerParameters.Depth.Min == hammerParameters.Depth.Max) ? new Range<float>(hammerParameters.Depth.Max + num2) : new Range<float>(hammerParameters.Depth.Min, hammerParameters.Depth.Max + num2));
							if (flag)
							{
								terrainHeight = TerrainInfo.CollectTerrainHeightInRect(position, centerRot, searchPos, width, depth, hammerParameters.Angle, blueprintTerrainNodeSpacing);
							}
							if (flag2)
							{
								terrainPaint = TerrainInfo.CollectTerrainPaintInRect(position, centerRot, searchPos, width2, depth2, hammerParameters.Angle, blueprintPaintNodeSpacing);
							}
						}
						else
						{
							Range<float> radius = new Range<float>(Mathf.Max(1f, num));
							Range<float> radius2 = new Range<float>(Mathf.Max(1f, num2));
							if (hammerParameters.Radius != null)
							{
								radius = ((hammerParameters.Radius.Min == hammerParameters.Radius.Max) ? new Range<float>(hammerParameters.Radius.Max + num) : new Range<float>(hammerParameters.Radius.Min, hammerParameters.Radius.Max + num));
								radius2 = ((hammerParameters.Radius.Min == hammerParameters.Radius.Max) ? new Range<float>(hammerParameters.Radius.Max + num2) : new Range<float>(hammerParameters.Radius.Min, hammerParameters.Radius.Max + num2));
							}
							if (flag)
							{
								terrainHeight = TerrainInfo.CollectTerrainHeightInRadius(position, centerRot, searchPos, radius, blueprintTerrainNodeSpacing);
							}
							if (flag2)
							{
								terrainPaint = TerrainInfo.CollectTerrainPaintInRadius(position, centerRot, searchPos, radius2, blueprintPaintNodeSpacing);
							}
						}
						if (terrainHeight != null)
						{
							terrainHeight.Smooth = hammerParameters.HeightSmooth ?? Configuration.BlueprintTerrainHeightSmooth;
						}
						if (terrainPaint != null)
						{
							terrainPaint.Smooth = hammerParameters.PaintSmooth ?? Configuration.BlueprintTerrainPaintSmooth;
						}
					}
					GameObject obj2 = Selection.CreateGhost((views.Length == 1) ? new ObjectSelection(views[0], hammerParameters.Pick, hammerParameters.Scale, val4, terrainHeight, terrainPaint) : new ObjectSelection((IEnumerable<ZNetView>)views, hammerParameters.Pick, hammerParameters.Scale, val4, terrainHeight, terrainPaint));
					Hammer.SelectEmpty();
					if (hammerParameters.Freeze)
					{
						Position.Freeze((views.Length != 0) ? ((Component)views[0]).transform.position : ((Component)Helper.GetPlayer()).transform.position);
					}
					if (hammerParameters.Pick)
					{
						UndoHelper.BeginAction();
						ZNetView[] array2 = views;
						for (int i = 0; i < array2.Length; i++)
						{
							UndoHelper.AddRemoveAction(array2[i].GetZDO());
						}
						array2 = views;
						for (int i = 0; i < array2.Length; i++)
						{
							HammerHelper.RemoveZDO(array2[i].GetZDO());
						}
						UndoHelper.EndAction();
					}
					PrintSelected(args.Context, obj2);
				}
			};
			_003C_003Ec._003C_003E9__4_1 = val;
			obj = (object)val;
		}
		Helper.Command("hammer", "[object id] - Selects the object to be placed (the hovered object by default).", (ConsoleEvent)obj);
	}
}
