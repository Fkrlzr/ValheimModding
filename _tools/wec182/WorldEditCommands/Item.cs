using ServerDevcommands;

namespace WorldEditCommands;

public class Item
{
	public string Name;

	public int Variant;

	public Item(string value)
	{
		string[] array = Parse.Split(value, ',');
		Name = Parse.String(array, 0, "");
		Variant = Parse.Int(array, 1, 0);
	}
}
