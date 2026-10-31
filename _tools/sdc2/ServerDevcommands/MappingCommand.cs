using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class MappingCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<int, List<string>> _003C_003E9__2_0;

		public static Func<int, int, string[], List<string>> _003C_003E9__5_0;

		public static ConsoleEvent _003C_003E9__5_1;

		public static ConsoleEvent _003C_003E9__5_2;

		public static ConsoleEvent _003C_003E9__5_3;

		internal List<string> _003CRegister_003Eb__2_0(int index)
		{
			return index switch
			{
				0 => ParameterInfo.Create("X coordinate."), 
				1 => ParameterInfo.Create("Z coordinate."), 
				2 => ParameterInfo.Create("Radius (default 0)."), 
				_ => ParameterInfo.None, 
			};
		}

		internal List<string> _003C_002Ector_003Eb__5_0(int index, int subIndex, string[] args)
		{
			if (index == 0)
			{
				string[] names = Enum.GetNames(typeof(PinType));
				List<string> list = new List<string>(names.Length);
				list.AddRange(names);
				return list;
			}
			if (args.Length != 0 && TryParsePinTypes(args[0], out List<PinType> _))
			{
				index--;
			}
			return index switch
			{
				0 => ParameterInfo.Create("X coordinate."), 
				1 => ParameterInfo.Create("Z coordinate."), 
				2 => ParameterInfo.Create("Radius (default 0)."), 
				_ => ParameterInfo.None, 
			};
		}

		internal void _003C_002Ector_003Eb__5_1(ConsoleEventArgs args)
		{
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			if (args.Length < 2)
			{
				args.Context.AddString("Error: Missing the types or the position.");
				return;
			}
			List<PinType> types = new List<PinType>();
			int num = (TryParsePinTypes(args[1], out types) ? 1 : 0);
			bool flag = args.Length > 1 + num;
			float x = 0f;
			float z = 0f;
			float radius = 0f;
			if (flag && !ParseArgs(args, out x, out z, out radius, num))
			{
				return;
			}
			Vector3 val = default(Vector3);
			((Vector3)(ref val))._002Ector(x, 0f, z);
			int num2 = 0;
			foreach (PinData item in Minimap.instance.m_pins.ToList())
			{
				if (item.m_save && (num <= 0 || types.Contains(item.m_type)) && (!flag || !(Utils.DistanceXZ(val, item.m_pos) > radius)))
				{
					Minimap.instance.RemovePin(item);
					num2++;
				}
			}
			Helper.AddMessage(args.Context, num2 + " pins removed.");
		}

		internal void _003C_002Ector_003Eb__5_2(ConsoleEventArgs args)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			float x;
			float z;
			float radius;
			if (args.Length == 1)
			{
				Minimap.instance.ExploreAll();
			}
			else if (ParseArgs(args, out x, out z, out radius))
			{
				Vector3 p = default(Vector3);
				((Vector3)(ref p))._002Ector(x, 0f, z);
				ExploreRadius(args.Context, p, radius, explore: true);
			}
		}

		internal void _003C_002Ector_003Eb__5_3(ConsoleEventArgs args)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			float x;
			float z;
			float radius;
			if (args.Length == 1)
			{
				Minimap.instance.Reset();
			}
			else if (ParseArgs(args, out x, out z, out radius))
			{
				Vector3 p = default(Vector3);
				((Vector3)(ref p))._002Ector(x, 0f, z);
				ExploreRadius(args.Context, p, radius, explore: false);
			}
		}
	}

	private static bool ParseArgs(ConsoleEventArgs args, out float x, out float z, out float radius, int offset = 0)
	{
		x = 0f;
		z = 0f;
		radius = 0f;
		if (args.Length < 2 + offset)
		{
			args.Context.AddString("Error: Missing coordinate X");
			return false;
		}
		if (args.Length < 3 + offset)
		{
			args.Context.AddString("Error: Missing coordinate Z");
			return false;
		}
		x = Parse.Float(args[1 + offset], float.MinValue);
		if (x == float.MinValue)
		{
			args.Context.AddString("Error: Invalid format for X coordinate.");
			return false;
		}
		z = Parse.Float(args[2 + offset], float.MinValue);
		if (z == float.MinValue)
		{
			args.Context.AddString("Error: Invalid format for Z coordinate.");
			return false;
		}
		if (args.Length > 3 + offset)
		{
			radius = Parse.Float(args[3 + offset], float.MinValue);
			if (radius == float.MinValue)
			{
				args.Context.AddString("Error: Invalid format for radius.");
				return false;
			}
		}
		return true;
	}

	private static bool TryParsePinTypes(string arg, out List<PinType> types)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		types = new List<PinType>();
		string[] array = Parse.Split(arg);
		foreach (string text in array)
		{
			if (Parse.TryFloat(text, out var _) || !Enum.TryParse<PinType>(text, ignoreCase: true, out PinType result))
			{
				return false;
			}
			types.Add(result);
		}
		return types.Count > 0;
	}

	private static void Register(string command)
	{
		AutoComplete.Register(command, (int index) => index switch
		{
			0 => ParameterInfo.Create("X coordinate."), 
			1 => ParameterInfo.Create("Z coordinate."), 
			2 => ParameterInfo.Create("Radius (default 0)."), 
			_ => ParameterInfo.None, 
		});
	}

	private static void ExploreRadius(Terminal terminal, Vector3 p, float radius, bool explore)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		Minimap instance = Minimap.instance;
		int num = (int)Mathf.Ceil(radius / instance.m_pixelSize);
		int num2 = default(int);
		int num3 = default(int);
		instance.WorldToPixel(p, ref num2, ref num3);
		int num4 = 0;
		for (int i = num3 - num; i <= num3 + num; i++)
		{
			if (i < 0 || i >= instance.m_textureSize)
			{
				continue;
			}
			for (int j = num2 - num; j <= num2 + num; j++)
			{
				if (j >= 0 && j < instance.m_textureSize)
				{
					Vector2 val = new Vector2((float)(j - num2), (float)(i - num3));
					if (!(((Vector2)(ref val)).magnitude > (float)num) && ExploreSpot(j, i, explore))
					{
						num4++;
					}
				}
			}
		}
		if (num4 > 0)
		{
			instance.m_fogTexture.Apply();
		}
		Helper.AddMessage(terminal, num4 + " spots " + (explore ? "explored" : "unexplored") + ".");
	}

	private static bool ExploreSpot(int x, int y, bool explore)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		Minimap instance = Minimap.instance;
		if (explore == instance.m_explored[y * instance.m_textureSize + x])
		{
			return false;
		}
		Color pixel = instance.m_fogTexture.GetPixel(x, y);
		pixel.r = ((!explore) ? 255 : 0);
		instance.m_fogTexture.SetPixel(x, y, pixel);
		instance.m_explored[y * instance.m_textureSize + x] = explore;
		return true;
	}

	public MappingCommand()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		AutoComplete.Register("resetpins", delegate(int index, int subIndex, string[] args)
		{
			if (index == 0)
			{
				string[] names = Enum.GetNames(typeof(PinType));
				List<string> list = new List<string>(names.Length);
				list.AddRange(names);
				return list;
			}
			if (args.Length != 0 && TryParsePinTypes(args[0], out List<PinType> _))
			{
				index--;
			}
			return index switch
			{
				0 => ParameterInfo.Create("X coordinate."), 
				1 => ParameterInfo.Create("Z coordinate."), 
				2 => ParameterInfo.Create("Radius (default 0)."), 
				_ => ParameterInfo.None, 
			};
		});
		object obj = _003C_003Ec._003C_003E9__5_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				if (args.Length < 2)
				{
					args.Context.AddString("Error: Missing the types or the position.");
				}
				else
				{
					List<PinType> types = new List<PinType>();
					int num = (TryParsePinTypes(args[1], out types) ? 1 : 0);
					bool flag = args.Length > 1 + num;
					float x3 = 0f;
					float z3 = 0f;
					float radius3 = 0f;
					if (!flag || ParseArgs(args, out x3, out z3, out radius3, num))
					{
						Vector3 val4 = default(Vector3);
						((Vector3)(ref val4))._002Ector(x3, 0f, z3);
						int num2 = 0;
						foreach (PinData item in Minimap.instance.m_pins.ToList())
						{
							if (item.m_save && (num <= 0 || types.Contains(item.m_type)) && (!flag || !(Utils.DistanceXZ(val4, item.m_pos) > radius3)))
							{
								Minimap.instance.RemovePin(item);
								num2++;
							}
						}
						Helper.AddMessage(args.Context, num2 + " pins removed.");
					}
				}
			};
			_003C_003Ec._003C_003E9__5_1 = val;
			obj = (object)val;
		}
		new ConsoleCommand("resetpins", "[types] [x] [z] [radius=0] - Removes pins from the map. Needs the types (comma separated), the position or both.", (ConsoleEvent)obj, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		Register("exploremap");
		object obj2 = _003C_003Ec._003C_003E9__5_2;
		if (obj2 == null)
		{
			ConsoleEvent val2 = delegate(ConsoleEventArgs args)
			{
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				float x2;
				float z2;
				float radius2;
				if (args.Length == 1)
				{
					Minimap.instance.ExploreAll();
				}
				else if (ParseArgs(args, out x2, out z2, out radius2))
				{
					Vector3 p2 = default(Vector3);
					((Vector3)(ref p2))._002Ector(x2, 0f, z2);
					ExploreRadius(args.Context, p2, radius2, explore: true);
				}
			};
			_003C_003Ec._003C_003E9__5_2 = val2;
			obj2 = (object)val2;
		}
		new ConsoleCommand("exploremap", "[x] [z] [radius=0] - Reveals part of the map. Without parameters, reveals the whole map.", (ConsoleEvent)obj2, true, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
		Register("resetmap");
		object obj3 = _003C_003Ec._003C_003E9__5_3;
		if (obj3 == null)
		{
			ConsoleEvent val3 = delegate(ConsoleEventArgs args)
			{
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				float x;
				float z;
				float radius;
				if (args.Length == 1)
				{
					Minimap.instance.Reset();
				}
				else if (ParseArgs(args, out x, out z, out radius))
				{
					Vector3 p = default(Vector3);
					((Vector3)(ref p))._002Ector(x, 0f, z);
					ExploreRadius(args.Context, p, radius, explore: false);
				}
			};
			_003C_003Ec._003C_003E9__5_3 = val3;
			obj3 = (object)val3;
		}
		new ConsoleCommand("resetmap", "[x] [z] [radius=0] - Hides part of the map. Without parameters, hides the whole map.", (ConsoleEvent)obj3, false, false, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
	}
}
