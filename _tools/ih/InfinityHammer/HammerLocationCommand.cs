using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;
using UnityEngine;

namespace InfinityHammer;

public class HammerLocationCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<int, int, List<string>> _003C_003E9__1_0;

		public static ConsoleEvent _003C_003E9__1_1;

		internal List<string> _003C_002Ector_003Eb__1_0(int index, int subIndex)
		{
			return index switch
			{
				0 => ParameterInfo.LocationIds, 
				1 => ParameterInfo.Create("Seed for the random output. 0 = random, all = enable all objects."), 
				2 => ParameterInfo.Create("Any value forces random damage on structures (disabled by default)."), 
				_ => ParameterInfo.None, 
			};
		}

		internal void _003C_002Ector_003Eb__1_1(ConsoleEventArgs args)
		{
			Helper.ArgsCheck(args, 2, "Missing the location id.");
			HammerHelper.Init();
			try
			{
				Hammer.AllLocationsObjects = args.Length > 2 && args[2] == "all";
				Hammer.RandomLocationDamage = args.Length > 3;
				Random random = new Random();
				int num = args.TryParameterInt(2, random.Next());
				if (num == 0)
				{
					num = random.Next();
				}
				GameObject val = Selection.CreateGhost(new LocationSelection(ZoneSystem.instance.GetLocation(StringExtensionMethods.GetStableHashCode(args[1])), num));
				Hammer.SelectEmpty();
				((Object)val).name = args[1];
				PrintSelected(args.Context, val);
			}
			catch (InvalidOperationException ex)
			{
				HammerHelper.Message(args.Context, ex.Message);
			}
		}
	}

	private static void PrintSelected(Terminal terminal, GameObject obj)
	{
		if (!Configuration.DisableSelectMessages)
		{
			string text = (Object.op_Implicit((Object)(object)obj) ? Utils.GetPrefabName(obj.gameObject) : "");
			HammerHelper.Message(terminal, "Selected " + text + ".");
		}
	}

	public HammerLocationCommand()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		AutoComplete.Register("hammer_location", (Func<int, int, List<string>>)((int index, int subIndex) => index switch
		{
			0 => ParameterInfo.LocationIds, 
			1 => ParameterInfo.Create("Seed for the random output. 0 = random, all = enable all objects."), 
			2 => ParameterInfo.Create("Any value forces random damage on structures (disabled by default)."), 
			_ => ParameterInfo.None, 
		}));
		object obj = _003C_003Ec._003C_003E9__1_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				Helper.ArgsCheck(args, 2, "Missing the location id.");
				HammerHelper.Init();
				try
				{
					Hammer.AllLocationsObjects = args.Length > 2 && args[2] == "all";
					Hammer.RandomLocationDamage = args.Length > 3;
					Random random = new Random();
					int num = args.TryParameterInt(2, random.Next());
					if (num == 0)
					{
						num = random.Next();
					}
					GameObject val2 = Selection.CreateGhost(new LocationSelection(ZoneSystem.instance.GetLocation(StringExtensionMethods.GetStableHashCode(args[1])), num));
					Hammer.SelectEmpty();
					((Object)val2).name = args[1];
					PrintSelected(args.Context, val2);
				}
				catch (InvalidOperationException ex)
				{
					HammerHelper.Message(args.Context, ex.Message);
				}
			};
			_003C_003Ec._003C_003E9__1_1 = val;
			obj = (object)val;
		}
		Helper.Command("hammer_location", "[location id] [seed=0] [random damage] - Selects the location to be placed.", (ConsoleEvent)obj);
	}
}
