using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ServerDevcommands;

public class SearchIdCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__0_0;

		public static Func<int, List<string>> _003C_003E9__0_1;

		internal void _003C_002Ector_003Eb__0_0(ConsoleEventArgs args)
		{
			_003C_003Ec__DisplayClass0_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass0_0();
			if (args.Length < 2)
			{
				ZLog.Log((object)("\n" + string.Join("\n", ParameterInfo.ObjectIds)));
				return;
			}
			CS_0024_003C_003E8__locals0.term = ((args.Length < 2) ? "" : args[1].ToLower());
			int num = args.TryParameterInt(2, 5);
			string[] array = ((CS_0024_003C_003E8__locals0.term == "") ? ParameterInfo.ObjectIds.ToArray() : ParameterInfo.ObjectIds.Where((string id) => id.ToLower().Contains(CS_0024_003C_003E8__locals0.term)).ToArray());
			if (array.Length > 100)
			{
				args.Context.AddString("Over 100 results, printing to the log file.");
				ZLog.Log((object)("\n" + string.Join("\n", array)));
				return;
			}
			int num2 = (int)Math.Ceiling((float)array.Length / (float)num);
			string[] array2 = new string[num2];
			for (int i = 0; i < array.Length; i += num2)
			{
				if (array.Length - i < num2)
				{
					num2 = array.Length - i;
					array2 = new string[num2];
				}
				Array.Copy(array, i, array2, 0, num2);
				args.Context.AddString(string.Join(", ", array2));
			}
		}

		internal List<string> _003C_002Ector_003Eb__0_1(int index)
		{
			return index switch
			{
				0 => ParameterInfo.Create("Search term"), 
				1 => ParameterInfo.Create("Max lines", "number (default 5)"), 
				_ => ParameterInfo.None, 
			};
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_0
	{
		public string term;

		internal bool _003C_002Ector_003Eb__2(string id)
		{
			return id.ToLower().Contains(term);
		}
	}

	public SearchIdCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__0_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				if (args.Length < 2)
				{
					ZLog.Log((object)("\n" + string.Join("\n", ParameterInfo.ObjectIds)));
				}
				else
				{
					string term = ((args.Length < 2) ? "" : args[1].ToLower());
					int num = args.TryParameterInt(2, 5);
					string[] array = ((term == "") ? ParameterInfo.ObjectIds.ToArray() : ParameterInfo.ObjectIds.Where((string id) => id.ToLower().Contains(term)).ToArray());
					if (array.Length > 100)
					{
						args.Context.AddString("Over 100 results, printing to the log file.");
						ZLog.Log((object)("\n" + string.Join("\n", array)));
					}
					else
					{
						int num2 = (int)Math.Ceiling((float)array.Length / (float)num);
						string[] array2 = new string[num2];
						for (int i = 0; i < array.Length; i += num2)
						{
							if (array.Length - i < num2)
							{
								num2 = array.Length - i;
								array2 = new string[num2];
							}
							Array.Copy(array, i, array2, 0, num2);
							args.Context.AddString(string.Join(", ", array2));
						}
					}
				}
			};
			_003C_003Ec._003C_003E9__0_0 = val;
			obj = (object)val;
		}
		Helper.Command("search_id", "[term] [max_lines=5] - Prints object ids matching the search term. Without term, prints all of them to the log file.", (ConsoleEvent)obj);
		AutoComplete.Register("search_id", (int index) => index switch
		{
			0 => ParameterInfo.Create("Search term"), 
			1 => ParameterInfo.Create("Max lines", "number (default 5)"), 
			_ => ParameterInfo.None, 
		});
	}
}
