using System.Linq;
using ServerDevcommands;

namespace InfinityHammer;

public class HammerSavePars
{
	public string CenterPiece = Configuration.BlueprintCenterPiece;

	public string SnapPiece = Configuration.BlueprintSnapPiece;

	public bool SaveData = Configuration.SaveBlueprintData;

	public bool Profile = Configuration.SaveBlueprintsToProfile;

	public string Name = "";

	public HammerSavePars(ConsoleEventArgs args)
	{
		string[] array = args.Args.Skip(1).ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i];
			string[] array2 = text.Split('=');
			if (array2.Length < 2)
			{
				if (i == 0)
				{
					Name = text;
				}
				if (i == 1)
				{
					CenterPiece = text;
				}
				if (i == 2)
				{
					SnapPiece = text;
				}
				continue;
			}
			if (array2[0] == "center" || array2[0] == "c")
			{
				CenterPiece = array2[1];
			}
			if (array2[0] == "snap" || array2[0] == "s")
			{
				SnapPiece = array2[1];
			}
			if (array2[0] == "data" || array2[0] == "d")
			{
				SaveData = Parse.BoolNull(array2[1]) ?? Configuration.SaveBlueprintData;
			}
			if (array2[0] == "profile" || array2[0] == "p")
			{
				Profile = Parse.BoolNull(array2[1]) ?? Configuration.SaveBlueprintsToProfile;
			}
		}
	}
}
