using UnityEngine;

namespace InfinityHammer;

public class PlacedCommand : MonoBehaviour
{
	public string Command = "";

	public void Start()
	{
		if (Command != "")
		{
			((Terminal)Console.instance).TryRunCommand(Command, false, false);
		}
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}
}
