using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SkToolbox.Utility;
using UnityEngine;

namespace SkToolbox;

public class SkLoader : MonoBehaviour
{
	private static GameObject _SkGameObject;

	private static SkBepInExLoader BepLoader;

	private static bool FirstLoad = true;

	private static bool InitLogging = false;

	public static GameObject Load
	{
		get
		{
			return _SkGameObject;
		}
		set
		{
			_SkGameObject = value;
		}
	}

	public static void Unload()
	{
		Object.Destroy((Object)(object)_SkGameObject, 0f);
		_SkGameObject = null;
	}

	public static void Reload()
	{
		Unload();
		if ((Object)(object)BepLoader != (Object)null)
		{
			BepLoader.InitConfig();
		}
		Init();
	}

	private void Start()
	{
		InitThreading();
	}

	public static void Main(string[] args)
	{
		InitThreading();
	}

	public static void InitThreading()
	{
		new Thread((ThreadStart)delegate
		{
			Thread.Sleep(2000);
			Init();
		}).Start();
	}

	public static void InitBepThreading(SkBepInExLoader bepLoader)
	{
		BepLoader = bepLoader;
		new Thread((ThreadStart)delegate
		{
			Thread.Sleep(2000);
			Init();
		}).Start();
	}

	public static void InitWithLog()
	{
		InitLogging = true;
		Init();
	}

	public static void Init()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		_SkGameObject = new GameObject("SpeelosToolbox");
		if (InitLogging)
		{
			InitLogging = false;
		}
		if (FirstLoad)
		{
			SkUtilities.Logz(new string[2] { "LOADER", "STARTUP" }, new string[1] { "SUCCESS!" }, (LogType)3);
		}
		CheckForUnknownInstance();
		Load.transform.parent = null;
		Transform parent = Load.transform.parent;
		if ((Object)(object)parent != (Object)null && (Object)(object)((Component)parent).gameObject != (Object)(object)Load)
		{
			parent.parent = Load.transform;
		}
		_SkGameObject.AddComponent<SkMenuController>();
		Object.DontDestroyOnLoad((Object)(object)_SkGameObject);
		FirstLoad = false;
	}

	public static void CheckForUnknownInstance()
	{
		IEnumerable<GameObject> enumerable = from obj in Resources.FindObjectsOfTypeAll<GameObject>()
			where ((Object)obj).name == "SpeelosToolbox"
			select obj;
		foreach (GameObject item in enumerable)
		{
			if ((Object)(object)item != (Object)(object)_SkGameObject)
			{
				Object.Destroy((Object)(object)item);
				SkUtilities.Logz(new string[2] { "LOADER", "DETECT" }, new string[1] { "Other Speelo's Toolbox instance destroyed." }, (LogType)3);
			}
		}
	}

	private void OnDestroy()
	{
		Unload();
	}
}
