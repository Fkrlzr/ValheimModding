using System;
using System.Runtime.CompilerServices;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class HammerAddPieceComponentsCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__0_0;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			foreach (GameObject prefab in ZNetScene.instance.m_prefabs)
			{
				if (!(((Object)prefab).name == "Player") && !((Object)prefab).name.StartsWith("_", StringComparison.Ordinal) && !((Object)prefab).name.StartsWith("fx_", StringComparison.Ordinal) && !((Object)prefab).name.StartsWith("sfx_", StringComparison.Ordinal) && !Object.op_Implicit((Object)(object)prefab.GetComponent<Piece>()))
				{
					Piece obj = prefab.AddComponent<Piece>();
					obj.m_name = Utils.GetPrefabName(((Component)obj).gameObject);
					obj.m_clipEverything = true;
				}
			}
			foreach (ZNetView value in ZNetScene.instance.m_instances.Values)
			{
				if (!(((Object)((Component)value).gameObject).name == "Player(Clone)") && !((Object)((Component)value).gameObject).name.StartsWith("_", StringComparison.Ordinal) && !((Object)((Component)value).gameObject).name.StartsWith("fx_", StringComparison.Ordinal) && !((Object)((Component)value).gameObject).name.StartsWith("sfx_", StringComparison.Ordinal) && !Object.op_Implicit((Object)(object)((Component)value).gameObject.GetComponent<Piece>()))
				{
					Piece obj2 = ((Component)value).gameObject.AddComponent<Piece>();
					obj2.m_name = Utils.GetPrefabName(((Component)obj2).gameObject);
					obj2.m_clipEverything = true;
				}
			}
			HammerHelper.Message(args.Context, "Piece component added to every prefab.");
		}
	}

	public HammerAddPieceComponentsCommand()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		AutoComplete.RegisterEmpty("hammer_add_piece_components");
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				foreach (GameObject prefab in ZNetScene.instance.m_prefabs)
				{
					if (!(((Object)prefab).name == "Player") && !((Object)prefab).name.StartsWith("_", StringComparison.Ordinal) && !((Object)prefab).name.StartsWith("fx_", StringComparison.Ordinal) && !((Object)prefab).name.StartsWith("sfx_", StringComparison.Ordinal) && !Object.op_Implicit((Object)(object)prefab.GetComponent<Piece>()))
					{
						Piece obj2 = prefab.AddComponent<Piece>();
						obj2.m_name = Utils.GetPrefabName(((Component)obj2).gameObject);
						obj2.m_clipEverything = true;
					}
				}
				foreach (ZNetView value in ZNetScene.instance.m_instances.Values)
				{
					if (!(((Object)((Component)value).gameObject).name == "Player(Clone)") && !((Object)((Component)value).gameObject).name.StartsWith("_", StringComparison.Ordinal) && !((Object)((Component)value).gameObject).name.StartsWith("fx_", StringComparison.Ordinal) && !((Object)((Component)value).gameObject).name.StartsWith("sfx_", StringComparison.Ordinal) && !Object.op_Implicit((Object)(object)((Component)value).gameObject.GetComponent<Piece>()))
					{
						Piece obj3 = ((Component)value).gameObject.AddComponent<Piece>();
						obj3.m_name = Utils.GetPrefabName(((Component)obj3).gameObject);
						obj3.m_clipEverything = true;
					}
				}
				HammerHelper.Message(args.Context, "Piece component added to every prefab.");
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		Helper.Command("hammer_add_piece_components", "Adds the Piece component to every prefab to allow copying them with PlanBuild.", (ConsoleEvent)obj);
	}
}
