using System;
using BepInEx.Configuration;
using Service;
using UnityEngine;

namespace InfinityHammer;

public class ConfigurationLegacy
{
	public static ConfigEntry<bool> configShapeCircle;

	public static ConfigEntry<bool> configShapeRing;

	public static ConfigEntry<bool> configShapeSquare;

	public static ConfigEntry<bool> configShapeRectangle;

	public static ConfigEntry<bool> configShapeFrame;

	public static ConfigEntry<bool> configShowCommandValues;

	public static ConfigEntry<KeyboardShortcut> shapeKey;

	public static ConfigEntry<KeyboardShortcut> commandRadius;

	public static ConfigEntry<KeyboardShortcut> commandRotate;

	public static ConfigEntry<KeyboardShortcut> commandDepth;

	public static ConfigEntry<KeyboardShortcut> commandHeight;

	public static ConfigEntry<KeyboardShortcut> commandModifier1;

	public static ConfigEntry<KeyboardShortcut> commandModifier2;

	public static ConfigEntry<string> commandHeightAmount;

	public static ConfigWrapper Wrapper;

	public static bool ShapeCircle => configShapeCircle.Value;

	public static bool ShapeRing => configShapeRing.Value;

	public static bool ShapeSquare => configShapeSquare.Value;

	public static bool ShapeRectangle => configShapeRectangle.Value;

	public static bool ShapeFrame => configShapeFrame.Value;

	public static bool AlwaysShowCommand => configShowCommandValues.Value;

	public static string ModifierKey1()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (commandModifier1 == null)
		{
			return "leftalt";
		}
		return ConfigWrapper.GetKeys(commandModifier1.Value);
	}

	public static string ModifierKey2()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (commandModifier2 == null)
		{
			return "leftcontrol";
		}
		return ConfigWrapper.GetKeys(commandModifier2.Value);
	}

	public static void Init(ConfigWrapper wrapper)
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		Wrapper = wrapper;
		string group = "1. General";
		configShowCommandValues = wrapper.Bind(group, "Show command values", value: false, "Always shows the command in the tool descriptions.");
		configShapeCircle = wrapper.Bind(group, "Shape circle", value: true, "Enables circle shape for commands.");
		configShapeRing = wrapper.Bind(group, "Shape ring", value: false, "Enables ring shape for commands.");
		configShapeRectangle = wrapper.Bind(group, "Shape rectangle", value: true, "Enables rectangle shape for commands.");
		configShapeSquare = wrapper.Bind(group, "Shape square", value: true, "Enables square shape for commands.");
		configShapeFrame = wrapper.Bind(group, "Shape frame", value: false, "Enables frame shape for commands.");
		commandHeightAmount = wrapper.Bind(group, "Command height amount", "0.1", "Meters to move.");
		group = "2. Binds";
		commandModifier1 = wrapper.Bind<KeyboardShortcut>(group, "Command modifier 1", new KeyboardShortcut((KeyCode)308, Array.Empty<KeyCode>()), "");
		commandModifier2 = wrapper.Bind<KeyboardShortcut>(group, "Command modifier 2", new KeyboardShortcut((KeyCode)306, Array.Empty<KeyCode>()), "");
		shapeKey = wrapper.BindCommand("tool_shape", group, "Change shape", new KeyboardShortcut((KeyCode)113, Array.Empty<KeyCode>()), "Changes the selection shape.", "command");
		commandRadius = wrapper.BindWheelCommand(() => "hammer_zoom_x " + (Selection.Get().TerrainGrid ? "0.5" : "1"), group, "Command radius (mouse wheel)", new KeyboardShortcut((KeyCode)0, Array.Empty<KeyCode>()), "Changes the command radius.", "command");
		commandDepth = wrapper.BindWheelCommand(() => "hammer_zoom_z " + (Selection.Get().TerrainGrid ? "0.5" : "1"), group, "Command depth (mouse wheel)", new KeyboardShortcut((KeyCode)304, (KeyCode[])(object)new KeyCode[1] { (KeyCode)308 }), "Changes the command rectangle depth.", "command");
		commandHeight = wrapper.BindWheelCommand(() => "hammer_zoom_y " + commandHeightAmount.Value, group, "Command height (mouse wheel)", new KeyboardShortcut((KeyCode)304, Array.Empty<KeyCode>()), "Changes the command height.", "command");
		commandRotate = wrapper.BindWheelCommand("hammer_rotate", group, "Command rotation (mouse wheel)", new KeyboardShortcut((KeyCode)304, (KeyCode[])(object)new KeyCode[1] { (KeyCode)306 }), "Changes the command rotation.", "command");
	}
}
