using System.Collections.Generic;

namespace Data;

public class ItemRecord
{
	public int PrefabHash;

	public string PrefabName = "";

	public int Stack;

	public float Durability;

	public Vector2i GridPos;

	public bool Equipped;

	public int Quality = 1;

	public int Variant;

	public long CrafterID;

	public string CrafterName = "";

	public Dictionary<string, string> CustomData = new Dictionary<string, string>();

	public int WorldLevel;

	public bool PickedUp;

	public bool Cheated;
}
