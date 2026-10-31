using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class HammerMeasureCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<Collider, bool> _003C_003E9__1_0;

		public static ConsoleEvent _003C_003E9__2_0;

		public static Func<int, List<string>> _003C_003E9__2_1;

		public static Func<GameObject, bool> _003C_003E9__2_3;

		public static Func<GameObject, string> _003C_003E9__2_5;

		public static ConsoleEvent _003C_003E9__2_2;

		internal bool _003CCheckWithCollider_003Eb__1_0(Collider c)
		{
			return !IgnoredLayers.Contains(((Component)c).gameObject.layer);
		}

		internal void _003C_002Ector_003Eb__2_0(ConsoleEventArgs args)
		{
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			ZNetView.m_forceDisableInit = true;
			ShieldGenerator val = default(ShieldGenerator);
			foreach (GameObject prefab in ZNetScene.instance.m_prefabs)
			{
				if (!(((Object)prefab).name == "Player") && !((Object)prefab).name.StartsWith("_", StringComparison.Ordinal) && !((Object)prefab).name.StartsWith("fx_", StringComparison.Ordinal) && !((Object)prefab).name.StartsWith("sfx_", StringComparison.Ordinal) && !Object.op_Implicit((Object)(object)prefab.GetComponentInChildren<Ragdoll>()) && (!Object.op_Implicit((Object)(object)prefab.GetComponentInChildren<ItemDrop>()) || Object.op_Implicit((Object)(object)prefab.GetComponentInChildren<Piece>())) && !Object.op_Implicit((Object)(object)prefab.GetComponentInChildren<RandomFlyingBird>()) && Object.op_Implicit((Object)(object)prefab.GetComponentInChildren<Collider>()))
				{
					GameObject obj = Object.Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity);
					if (obj.TryGetComponent<ShieldGenerator>(ref val))
					{
						val.m_isPlacementGhost = true;
					}
					CheckWithCollider(obj);
					Object.Destroy((Object)(object)obj);
				}
			}
			ZNetView.m_forceDisableInit = false;
			HammerHelper.Message(args.Context, "Objects measured.");
		}

		internal List<string> _003C_002Ector_003Eb__2_1(int index)
		{
			if (index == 0)
			{
				return ParameterInfo.Components;
			}
			return ParameterInfo.None;
		}

		internal void _003C_002Ector_003Eb__2_2(ConsoleEventArgs args)
		{
			_003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass2_0
			{
				args = args
			};
			string[] array = (from p in ZNetScene.instance.m_prefabs
				where p.GetComponent<ZNetView>().m_syncInitialScale
				where CS_0024_003C_003E8__locals0.args.Length == 1 || Object.op_Implicit((Object)(object)p.GetComponent(CS_0024_003C_003E8__locals0.args[1]))
				select ((Object)p).name).ToArray();
			ZLog.Log((object)string.Format("Scalable objects ({0}):\n{1}", array.Length, string.Join("\n", array)));
		}

		internal bool _003C_002Ector_003Eb__2_3(GameObject p)
		{
			return p.GetComponent<ZNetView>().m_syncInitialScale;
		}

		internal string _003C_002Ector_003Eb__2_5(GameObject p)
		{
			return ((Object)p).name;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public ConsoleEventArgs args;

		internal bool _003C_002Ector_003Eb__4(GameObject p)
		{
			if (args.Length != 1)
			{
				return Object.op_Implicit((Object)(object)p.GetComponent(args[1]));
			}
			return true;
		}
	}

	private static readonly int[] IgnoredLayers = new int[3]
	{
		LayerMask.NameToLayer("character_trigger"),
		LayerMask.NameToLayer("viewblock"),
		LayerMask.NameToLayer("pathblocker")
	};

	public static void CheckWithCollider(GameObject obj)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		if (obj.transform.rotation != Quaternion.identity)
		{
			return;
		}
		string prefabName = Utils.GetPrefabName(obj);
		if (Configuration.Dimensions.ContainsKey(prefabName.ToLower()) || !Object.op_Implicit((Object)(object)ZNetScene.instance.GetPrefab(prefabName)))
		{
			return;
		}
		Collider[] array = (from c in obj.GetComponentsInChildren<Collider>()
			where !IgnoredLayers.Contains(((Component)c).gameObject.layer)
			select c).ToArray();
		if (array.Length != 0)
		{
			Bounds bounds = array[0].bounds;
			Collider[] array2 = array;
			foreach (Collider val in array2)
			{
				((Bounds)(ref bounds)).Encapsulate(val.bounds);
			}
			Configuration.SetDimension(prefabName, ((Bounds)(ref bounds)).size);
		}
	}

	public HammerMeasureCommand()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		AutoComplete.RegisterEmpty("hammer_measure");
		object obj = _003C_003Ec._003C_003E9__2_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00be: Unknown result type (might be due to invalid IL or missing references)
				ZNetView.m_forceDisableInit = true;
				ShieldGenerator val3 = default(ShieldGenerator);
				foreach (GameObject prefab in ZNetScene.instance.m_prefabs)
				{
					if (!(((Object)prefab).name == "Player") && !((Object)prefab).name.StartsWith("_", StringComparison.Ordinal) && !((Object)prefab).name.StartsWith("fx_", StringComparison.Ordinal) && !((Object)prefab).name.StartsWith("sfx_", StringComparison.Ordinal) && !Object.op_Implicit((Object)(object)prefab.GetComponentInChildren<Ragdoll>()) && (!Object.op_Implicit((Object)(object)prefab.GetComponentInChildren<ItemDrop>()) || Object.op_Implicit((Object)(object)prefab.GetComponentInChildren<Piece>())) && !Object.op_Implicit((Object)(object)prefab.GetComponentInChildren<RandomFlyingBird>()) && Object.op_Implicit((Object)(object)prefab.GetComponentInChildren<Collider>()))
					{
						GameObject obj3 = Object.Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity);
						if (obj3.TryGetComponent<ShieldGenerator>(ref val3))
						{
							val3.m_isPlacementGhost = true;
						}
						CheckWithCollider(obj3);
						Object.Destroy((Object)(object)obj3);
					}
				}
				ZNetView.m_forceDisableInit = false;
				HammerHelper.Message(args.Context, "Objects measured.");
			};
			_003C_003Ec._003C_003E9__2_0 = val;
			obj = (object)val;
		}
		Helper.Command("hammer_measure", "Tries to measure all structures.", (ConsoleEvent)obj);
		AutoComplete.Register("hammer_print_scalable", (Func<int, List<string>>)((int index) => (index == 0) ? ParameterInfo.Components : ParameterInfo.None));
		object obj2 = _003C_003Ec._003C_003E9__2_2;
		if (obj2 == null)
		{
			ConsoleEvent val2 = delegate(ConsoleEventArgs args)
			{
				string[] array = (from p in ZNetScene.instance.m_prefabs
					where p.GetComponent<ZNetView>().m_syncInitialScale
					where args.Length == 1 || Object.op_Implicit((Object)(object)p.GetComponent(args[1]))
					select ((Object)p).name).ToArray();
				ZLog.Log((object)string.Format("Scalable objects ({0}):\n{1}", array.Length, string.Join("\n", array)));
			};
			_003C_003Ec._003C_003E9__2_2 = val2;
			obj2 = (object)val2;
		}
		Helper.Command("hammer_print_scalable", "[component] - Prints scalable objects, optionally filtered by component.", (ConsoleEvent)obj2);
	}
}
