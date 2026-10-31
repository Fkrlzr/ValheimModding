using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ServerDevcommands;

public class InventoryCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<ItemData, bool> _003C_003E9__5_2;

		public static Func<ItemData, bool> _003C_003E9__5_3;

		public static Func<ItemData, bool> _003C_003E9__5_4;

		public static ConsoleEvent _003C_003E9__5_0;

		public static Func<int, List<string>> _003C_003E9__5_1;

		internal void _003C_002Ector_003Eb__5_0(ConsoleEventArgs args)
		{
			_003C_003Ec__DisplayClass5_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass5_0();
			Helper.ArgsCheck(args, 2, "Missing clear, level, refill, repair or upgrade.");
			CS_0024_003C_003E8__locals0.amount = Parse.IntNull(args.Args, 2);
			string text = Parse.String(args.Args, 3, "all");
			if (!CS_0024_003C_003E8__locals0.amount.HasValue)
			{
				text = Parse.String(args.Args, 2, "all");
				CS_0024_003C_003E8__locals0.amount = Parse.IntNull(args.Args, 3);
			}
			Player player = Helper.GetPlayer();
			Inventory inventory = ((Humanoid)player).GetInventory();
			List<ItemData> list = inventory.GetAllItems();
			if (text == "hand" || text == "worn")
			{
				list = inventory.GetEquippedItems();
			}
			if (text == "hand")
			{
				list = list.Where((ItemData item) => HandTypes.Contains(item.m_shared.m_itemType)).ToList();
			}
			switch (args[1])
			{
			case "repair":
			{
				ItemData[] array5 = list.Where((ItemData item) => item.m_durability < item.GetMaxDurability()).ToArray();
				ItemData[] array2 = array5;
				foreach (ItemData obj3 in array2)
				{
					obj3.m_durability = obj3.GetMaxDurability();
				}
				Helper.AddMessage(args.Context, $"{array5.Length} items repaired.");
				break;
			}
			case "refill":
			{
				ItemData[] array3 = list.Where((ItemData item) => item.m_stack < item.m_shared.m_maxStackSize).ToArray();
				ItemData[] array2 = array3;
				foreach (ItemData obj in array2)
				{
					obj.m_stack = obj.m_shared.m_maxStackSize;
				}
				Helper.AddMessage(args.Context, $"{array3.Length} items filled.");
				break;
			}
			case "clear":
				((Humanoid)player).UnequipAllItems();
				Helper.AddMessage(args.Context, $"{list.Count} items cleared.");
				inventory.RemoveAll();
				break;
			case "level":
			{
				if (!CS_0024_003C_003E8__locals0.amount.HasValue)
				{
					throw new InvalidOperationException("Missing the level.");
				}
				ItemData[] array4 = list.Where(Valid).ToArray();
				ItemData[] array2 = array4;
				foreach (ItemData obj2 in array2)
				{
					obj2.m_quality = CS_0024_003C_003E8__locals0.amount.Value;
					obj2.m_durability = obj2.GetMaxDurability();
				}
				Helper.AddMessage(args.Context, $"{array4.Length} items upgraded.");
				break;
			}
			case "upgrade":
			{
				ItemData[] array = list.Where((ItemData item) => Valid(item) && (CS_0024_003C_003E8__locals0.amount.HasValue || item.m_quality != item.m_shared.m_maxQuality)).ToArray();
				ItemData[] array2 = array;
				foreach (ItemData val in array2)
				{
					if (CS_0024_003C_003E8__locals0.amount.HasValue)
					{
						val.m_quality = CS_0024_003C_003E8__locals0.amount.Value;
					}
					else
					{
						val.m_quality = val.m_shared.m_maxQuality;
					}
					val.m_durability = val.GetMaxDurability();
				}
				Helper.AddMessage(args.Context, $"{array.Length} items upgraded.");
				break;
			}
			default:
				throw new InvalidOperationException("Invalid operation. Use level, repair or upgrade.");
			}
			inventory.Changed(false, false);
		}

		internal bool _003C_002Ector_003Eb__5_2(ItemData item)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return HandTypes.Contains(item.m_shared.m_itemType);
		}

		internal bool _003C_002Ector_003Eb__5_3(ItemData item)
		{
			return item.m_durability < item.GetMaxDurability();
		}

		internal bool _003C_002Ector_003Eb__5_4(ItemData item)
		{
			return item.m_stack < item.m_shared.m_maxStackSize;
		}

		internal List<string> _003C_002Ector_003Eb__5_1(int index)
		{
			return index switch
			{
				0 => Operations, 
				1 => Targets, 
				2 => ParameterInfo.Create("Amount", "How many levels to upgrade."), 
				_ => ParameterInfo.None, 
			};
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public int? amount;

		internal bool _003C_002Ector_003Eb__5(ItemData item)
		{
			if (Valid(item))
			{
				if (!amount.HasValue)
				{
					return item.m_quality != item.m_shared.m_maxQuality;
				}
				return true;
			}
			return false;
		}
	}

	private static readonly List<string> Operations = new List<string>(5) { "clear", "level", "refill", "repair", "upgrade" };

	private static readonly List<string> Targets = new List<string>(3) { "all", "hand", "worn" };

	private static readonly HashSet<ItemType> HandTypes = new HashSet<ItemType>
	{
		(ItemType)4,
		(ItemType)3,
		(ItemType)5,
		(ItemType)15,
		(ItemType)19,
		(ItemType)14
	};

	private static readonly HashSet<ItemType> ArmorTypes = new HashSet<ItemType>
	{
		(ItemType)7,
		(ItemType)12,
		(ItemType)6,
		(ItemType)11,
		(ItemType)17
	};

	private static bool Valid(ItemData item)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (item.m_shared.m_maxQuality <= 1 && !HandTypes.Contains(item.m_shared.m_itemType))
		{
			return ArmorTypes.Contains(item.m_shared.m_itemType);
		}
		return true;
	}

	public InventoryCommand()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		object obj = _003C_003Ec._003C_003E9__5_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				Helper.ArgsCheck(args, 2, "Missing clear, level, refill, repair or upgrade.");
				int? amount = Parse.IntNull(args.Args, 2);
				string text = Parse.String(args.Args, 3, "all");
				if (!amount.HasValue)
				{
					text = Parse.String(args.Args, 2, "all");
					amount = Parse.IntNull(args.Args, 3);
				}
				Player player = Helper.GetPlayer();
				Inventory inventory = ((Humanoid)player).GetInventory();
				List<ItemData> list = inventory.GetAllItems();
				if (text == "hand" || text == "worn")
				{
					list = inventory.GetEquippedItems();
				}
				if (text == "hand")
				{
					list = list.Where((ItemData item) => HandTypes.Contains(item.m_shared.m_itemType)).ToList();
				}
				switch (args[1])
				{
				case "repair":
				{
					ItemData[] array5 = list.Where((ItemData item) => item.m_durability < item.GetMaxDurability()).ToArray();
					ItemData[] array2 = array5;
					foreach (ItemData obj4 in array2)
					{
						obj4.m_durability = obj4.GetMaxDurability();
					}
					Helper.AddMessage(args.Context, $"{array5.Length} items repaired.");
					break;
				}
				case "refill":
				{
					ItemData[] array3 = list.Where((ItemData item) => item.m_stack < item.m_shared.m_maxStackSize).ToArray();
					ItemData[] array2 = array3;
					foreach (ItemData obj2 in array2)
					{
						obj2.m_stack = obj2.m_shared.m_maxStackSize;
					}
					Helper.AddMessage(args.Context, $"{array3.Length} items filled.");
					break;
				}
				case "clear":
					((Humanoid)player).UnequipAllItems();
					Helper.AddMessage(args.Context, $"{list.Count} items cleared.");
					inventory.RemoveAll();
					break;
				case "level":
				{
					if (!amount.HasValue)
					{
						throw new InvalidOperationException("Missing the level.");
					}
					ItemData[] array4 = list.Where(Valid).ToArray();
					ItemData[] array2 = array4;
					foreach (ItemData obj3 in array2)
					{
						obj3.m_quality = amount.Value;
						obj3.m_durability = obj3.GetMaxDurability();
					}
					Helper.AddMessage(args.Context, $"{array4.Length} items upgraded.");
					break;
				}
				case "upgrade":
				{
					ItemData[] array = list.Where((ItemData item) => Valid(item) && (amount.HasValue || item.m_quality != item.m_shared.m_maxQuality)).ToArray();
					ItemData[] array2 = array;
					foreach (ItemData val2 in array2)
					{
						if (amount.HasValue)
						{
							val2.m_quality = amount.Value;
						}
						else
						{
							val2.m_quality = val2.m_shared.m_maxQuality;
						}
						val2.m_durability = val2.GetMaxDurability();
					}
					Helper.AddMessage(args.Context, $"{array.Length} items upgraded.");
					break;
				}
				default:
					throw new InvalidOperationException("Invalid operation. Use level, repair or upgrade.");
				}
				inventory.Changed(false, false);
			};
			_003C_003Ec._003C_003E9__5_0 = val;
			obj = (object)val;
		}
		Helper.Command("inventory", "[level/repair/upgrade] [hand/worn/all] [amount or max level if not given] - Modifies items in the inventory.", (ConsoleEvent)obj);
		AutoComplete.Register("inventory", (int index) => index switch
		{
			0 => Operations, 
			1 => Targets, 
			2 => ParameterInfo.Create("Amount", "How many levels to upgrade."), 
			_ => ParameterInfo.None, 
		});
	}
}
