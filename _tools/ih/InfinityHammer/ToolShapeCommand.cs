using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ServerDevcommands;

namespace InfinityHammer;

public class ToolShapeCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__0_1;

		internal void _003C_002Ector_003Eb__0_1(ConsoleEventArgs args)
		{
			if (!(Selection.Get() is ToolSelection))
			{
				return;
			}
			if (args.Length > 1)
			{
				string text = args[1].ToLower();
				if (text == RulerShape.Circle.ToString().ToLower())
				{
					Ruler.Shape = RulerShape.Circle;
				}
				else if (text == RulerShape.Ring.ToString().ToLower())
				{
					Ruler.Shape = RulerShape.Ring;
				}
				else if (text == RulerShape.Rectangle.ToString().ToLower())
				{
					Ruler.Shape = RulerShape.Rectangle;
				}
				else if (text == RulerShape.Frame.ToString().ToLower())
				{
					Ruler.Shape = RulerShape.Frame;
				}
				else
				{
					if (!(text == RulerShape.Square.ToString().ToLower()))
					{
						return;
					}
					Ruler.Shape = RulerShape.Square;
				}
			}
			else
			{
				if (Ruler.Shape == RulerShape.Circle)
				{
					Ruler.Shape = RulerShape.Ring;
				}
				else if (Ruler.Shape == RulerShape.Ring)
				{
					Ruler.Shape = RulerShape.Square;
				}
				else if (Ruler.Shape == RulerShape.Square)
				{
					Ruler.Shape = RulerShape.Frame;
				}
				else if (Ruler.Shape == RulerShape.Frame)
				{
					Ruler.Shape = RulerShape.Rectangle;
				}
				else if (Ruler.Shape == RulerShape.Rectangle)
				{
					Ruler.Shape = RulerShape.Circle;
				}
				Ruler.SanityCheckShape();
			}
			HammerHelper.Message(args.Context, $"Selection shape set to {Ruler.Shape}.");
		}
	}

	public ToolShapeCommand()
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		List<string> shapes = new List<string>(3)
		{
			RulerShape.Circle.ToString(),
			RulerShape.Rectangle.ToString(),
			RulerShape.Square.ToString()
		};
		AutoComplete.Register("tool_shape", (Func<int, int, List<string>>)((int index, int subIndex) => (index != 0) ? ParameterInfo.None : shapes));
		object obj = _003C_003Ec._003C_003E9__0_1;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				if (Selection.Get() is ToolSelection)
				{
					if (args.Length > 1)
					{
						string text = args[1].ToLower();
						if (text == RulerShape.Circle.ToString().ToLower())
						{
							Ruler.Shape = RulerShape.Circle;
						}
						else if (text == RulerShape.Ring.ToString().ToLower())
						{
							Ruler.Shape = RulerShape.Ring;
						}
						else if (text == RulerShape.Rectangle.ToString().ToLower())
						{
							Ruler.Shape = RulerShape.Rectangle;
						}
						else if (text == RulerShape.Frame.ToString().ToLower())
						{
							Ruler.Shape = RulerShape.Frame;
						}
						else
						{
							if (!(text == RulerShape.Square.ToString().ToLower()))
							{
								return;
							}
							Ruler.Shape = RulerShape.Square;
						}
					}
					else
					{
						if (Ruler.Shape == RulerShape.Circle)
						{
							Ruler.Shape = RulerShape.Ring;
						}
						else if (Ruler.Shape == RulerShape.Ring)
						{
							Ruler.Shape = RulerShape.Square;
						}
						else if (Ruler.Shape == RulerShape.Square)
						{
							Ruler.Shape = RulerShape.Frame;
						}
						else if (Ruler.Shape == RulerShape.Frame)
						{
							Ruler.Shape = RulerShape.Rectangle;
						}
						else if (Ruler.Shape == RulerShape.Rectangle)
						{
							Ruler.Shape = RulerShape.Circle;
						}
						Ruler.SanityCheckShape();
					}
					HammerHelper.Message(args.Context, $"Selection shape set to {Ruler.Shape}.");
				}
			};
			_003C_003Ec._003C_003E9__0_1 = val;
			obj = (object)val;
		}
		Helper.Command("tool_shape", "[shape] - Toggles or sets the selection shape.", (ConsoleEvent)obj);
	}
}
