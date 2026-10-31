using System.Collections.Generic;
using UnityEngine;

namespace ServerDevcommands;

public class ResolutionCommand
{
	private string GetMode()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		string result = "window";
		if ((int)Screen.fullScreenMode == 0)
		{
			result = "exclusive";
		}
		if ((int)Screen.fullScreenMode == 2)
		{
			result = "max";
		}
		if ((int)Screen.fullScreenMode == 1)
		{
			result = "full";
		}
		return result;
	}

	private void PrintResolution(Terminal terminal)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Resolution currentResolution = Screen.currentResolution;
		Helper.AddMessage(terminal, $"Resolution is {Screen.width}x{Screen.height} ({((Resolution)(ref currentResolution)).width}x{((Resolution)(ref currentResolution)).height}) with {((Resolution)(ref currentResolution)).refreshRateRatio.numerator} hz and {GetMode()} mode.");
	}

	private void SetResolution(Terminal terminal, string[] args)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		Resolution currentResolution = Screen.currentResolution;
		string text = Parse.String(args, 1, GetMode());
		int num = Parse.Int(args, 2, ((Resolution)(ref currentResolution)).width);
		int num2 = Parse.Int(args, 3, ((Resolution)(ref currentResolution)).height);
		int num3 = Parse.Int(args, 4, (int)((Resolution)(ref currentResolution)).refreshRateRatio.numerator);
		FullScreenMode val = (FullScreenMode)3;
		if (text == "exclusive")
		{
			val = (FullScreenMode)0;
		}
		if (text == "max")
		{
			val = (FullScreenMode)2;
		}
		if (text == "full")
		{
			val = (FullScreenMode)1;
		}
		RefreshRate val2 = default(RefreshRate);
		val2.numerator = (uint)num3;
		val2.denominator = 1u;
		RefreshRate val3 = val2;
		Screen.SetResolution(num, num2, val, val3);
		string text2 = ((num3 == 0) ? "max" : num3.ToString());
		Helper.AddMessage(terminal, $"Resolution set to {num}x{num2} with {text2} hz and {text} mode.");
	}

	public ResolutionCommand()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		new ConsoleCommand("resolution", "[mode] [width] [height] [refresh] - Prints or sets the resolution.", (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			if (args.Length == 1)
			{
				PrintResolution(args.Context);
			}
			else
			{
				SetResolution(args.Context, args.Args);
			}
		}, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		AutoComplete.Register("resolution", (int index) => index switch
		{
			0 => new List<string>(4) { "exclusive", "full", "max", "window" }, 
			1 => ParameterInfo.Create("Width", "a positive integer (max supported if not given)"), 
			2 => ParameterInfo.Create("Height", "a positive integer (max supported if not given)"), 
			3 => ParameterInfo.Create("Refresh rate", "a positive integer (max supported if not given)"), 
			_ => ParameterInfo.None, 
		});
	}
}
