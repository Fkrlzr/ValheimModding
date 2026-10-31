using HarmonyLib;

namespace InfinityHammer;

[HarmonyPatch(typeof(Terminal), "InitTerminal")]
[HarmonyPriority(500)]
public class Initialize
{
	private static bool Initialized;

	private static void Postfix()
	{
		if (!Initialized)
		{
			Initialized = true;
			new HammerAddPieceComponentsCommand();
			new HammerSelect();
			new HammerLocationCommand();
			new HammerBlueprintCommand();
			new HammerMoveCommand();
			new HammerOffsetCommand();
			new HammerPlaceCommand();
			new HammerRepairCommand();
			new HammerRotateCommand();
			new HammerScaleCommand();
			new HammerZoomCommand();
			new HammerStackCommand();
			new HammerFreezeCommand();
			new HammerGridCommand();
			new HammerSaveCommand();
			new HammerMirrorCommand();
			new HammerZoopCommand();
			new HammerMeasureCommand();
			new HammerPosCommand();
			new HammerMenuCommand();
			new ToolShapeCommand();
			new ToolCommand();
			new ToolImportCommand();
			new ToolExportCommand();
			new ToolCmdCommand();
			new HammerRoomCommand();
			new HammerMark();
			ToolManager.Initialize();
			InfinityHammer.Wrapper.Bind();
		}
	}
}
