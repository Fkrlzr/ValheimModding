using Data;

namespace InfinityHammer;

public class SelectedObject
{
	public int Prefab;

	public DataEntry? Data;

	public bool Scalable;

	public SelectedObject(int prefab, bool scalable, DataEntry? data)
	{
		Prefab = prefab;
		Data = data;
		Scalable = scalable;
		base._002Ector();
	}
}
