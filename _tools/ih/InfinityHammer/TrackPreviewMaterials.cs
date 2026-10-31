using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;

namespace InfinityHammer;

[HarmonyPatch]
public static class TrackPreviewMaterials
{
	private static readonly Dictionary<Material, Material> Cache = new Dictionary<Material, Material>();

	private static readonly HashSet<Material> Cleaned = new HashSet<Material>();

	public static void Clear()
	{
		foreach (Material value in Cache.Values)
		{
			if (Object.op_Implicit((Object)(object)value))
			{
				Object.Destroy((Object)(object)value);
			}
		}
		Cache.Clear();
		Cleaned.Clear();
	}

	private static IEnumerable<MethodBase> TargetMethods()
	{
		MethodInfo method = AccessTools.Method(typeof(Player), "CleanupGhostMaterials", (Type[])null, (Type[])null);
		yield return method.MakeGenericMethod(typeof(MeshRenderer));
		yield return method.MakeGenericMethod(typeof(SkinnedMeshRenderer));
	}

	private static bool Prefix(Player __instance, GameObject ghost)
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		if (!Object.op_Implicit((Object)(object)ghost))
		{
			return false;
		}
		Renderer[] componentsInChildren = ghost.GetComponentsInChildren<Renderer>();
		foreach (Renderer val in componentsInChildren)
		{
			if ((!(val is MeshRenderer) && !(val is SkinnedMeshRenderer)) || !Object.op_Implicit((Object)(object)val) || (Object)(object)val.sharedMaterial == (Object)null)
			{
				continue;
			}
			Material[] sharedMaterials = val.sharedMaterials;
			for (int j = 0; j < sharedMaterials.Length; j++)
			{
				Material val2 = sharedMaterials[j];
				if (!Object.op_Implicit((Object)(object)val2) || Cleaned.Contains(val2))
				{
					continue;
				}
				if (!Cache.TryGetValue(val2, out Material value) || !Object.op_Implicit((Object)(object)value))
				{
					value = new Material(val2);
					if (value.HasProperty("_RippleDistance"))
					{
						__instance.m_ghostRippleDistance[value] = value.GetFloat("_RippleDistance");
					}
					value.SetFloat("_ValueNoise", 0f);
					value.SetFloat("_TriplanarLocalPos", 1f);
					Cache[val2] = value;
					Cleaned.Add(value);
				}
				else if (value.HasProperty("_RippleDistance") && !__instance.m_ghostRippleDistance.ContainsKey(value))
				{
					__instance.m_ghostRippleDistance[value] = val2.GetFloat("_RippleDistance");
				}
				sharedMaterials[j] = value;
			}
			if (sharedMaterials.Length != 0)
			{
				val.sharedMaterials = sharedMaterials;
			}
			val.shadowCastingMode = (ShadowCastingMode)0;
		}
		return false;
	}
}
