using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Data;
using ServerDevcommands;
using Service;
using UnityEngine;

namespace WorldEditCommands;

public class ObjectCommand
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ConsoleEvent _003C_003E9__4_0;

		internal void _003C_002Ector_003Eb__4_0(ConsoleEventArgs args)
		{
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			ObjectParameters objectParameters = new ObjectParameters(args);
			ZNetView[] views;
			if (objectParameters.Connect)
			{
				ZNetView hovered = Selector.GetHovered(50f, objectParameters.IncludedIds, objectParameters.Components, objectParameters.ExcludedIds);
				if ((Object)(object)hovered == (Object)null)
				{
					return;
				}
				views = Selector.GetConnected(hovered, objectParameters.ConnectionIds, objectParameters.IncludedIds, objectParameters.ExcludedIds);
			}
			else if (objectParameters.Radius != null)
			{
				views = Selector.GetNearby(objectParameters.IncludedIds, objectParameters.Components, objectParameters.ExcludedIds, (Vector3)(((_003F?)objectParameters.Center) ?? objectParameters.From), objectParameters.Radius, objectParameters.Height);
			}
			else if (objectParameters.Width != null && objectParameters.Depth != null)
			{
				views = Selector.GetNearby(objectParameters.IncludedIds, objectParameters.Components, objectParameters.ExcludedIds, (Vector3)(((_003F?)objectParameters.Center) ?? objectParameters.From), objectParameters.Angle, objectParameters.Width, objectParameters.Depth, objectParameters.Height);
			}
			else
			{
				ZNetView hovered2 = Selector.GetHovered(50f, objectParameters.IncludedIds, objectParameters.ExcludedIds);
				if ((Object)(object)hovered2 == (Object)null)
				{
					return;
				}
				if (!Selector.GetPrefabs(objectParameters.IncludedIds).Contains(hovered2.GetZDO().GetPrefab()))
				{
					Helper.AddMessage(args.Context, "Skipped: " + ((Object)hovered2).name + " has invalid id.", false);
					return;
				}
				views = (ZNetView[])(object)new ZNetView[1] { hovered2 };
			}
			Execute(args.Context, objectParameters, objectParameters.Operations, views);
		}
	}

	public static Random Random = new Random();

	public const string Name = "object";

	public static bool Roll(float value)
	{
		if (value >= 1f)
		{
			return true;
		}
		return Random.NextDouble() < (double)value;
	}

	private static void Execute(Terminal context, ObjectParameters pars, IEnumerable<string> operations, ZNetView[] views)
	{
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		Terminal context2 = context;
		ObjectParameters pars2 = pars;
		_ = ZNetScene.instance;
		DataEntry matchData = ((pars2.Match == "") ? null : DataHelper.Get(pars2.Match));
		DataEntry unmatchData = ((pars2.Unmatch == "") ? null : DataHelper.Get(pars2.Unmatch));
		views = views.Where(delegate(ZNetView view)
		{
			if (!Object.op_Implicit((Object)(object)view))
			{
				return false;
			}
			if (!view.GetZDO().IsValid())
			{
				context2.AddString("Skipped: " + ((Object)view).name + " is not loaded.");
				return false;
			}
			ZDO zDO = view.GetZDO();
			if (matchData != null && !matchData.Match(new ParFunctions(pars2.DataParameters, zDO), zDO))
			{
				context2.AddString("Skipped: " + ((Object)view).name + " not matching filter.");
				return false;
			}
			if (unmatchData != null && !unmatchData.MatchNone(new ParFunctions(pars2.DataParameters, zDO), zDO))
			{
				context2.AddString("Skipped: " + ((Object)view).name + " matching filter.");
				return false;
			}
			if (!Roll(pars2.Chance))
			{
				context2.AddString("Skipped: " + ((Object)view).name + " (chance).");
				return false;
			}
			return true;
		}).ToArray();
		UndoHelper.BeginAction();
		ZNetView[] array = views;
		foreach (ZNetView val in array)
		{
			if (val.GetZDO().GetPrefab() != Hash.Player)
			{
				val.ClaimOwnership();
			}
		}
		int num = views.Count();
		foreach (string operation in operations)
		{
			array = views;
			foreach (ZNetView val2 in array)
			{
				if (!Object.op_Implicit((Object)(object)val2))
				{
					continue;
				}
				string text = "";
				string prefabName = Utils.GetPrefabName(((Component)val2).gameObject);
				if (operation == "durability" || operation == "health")
				{
					text = ChangeHealth(val2, Helper.RandomValue(pars2.Health), pars2.isHealthPercentage);
				}
				if (operation == "damage")
				{
					text = Actions.Damage(val2, Helper.RandomValue(pars2.Damage));
				}
				if (operation == "ammo")
				{
					text = Actions.Ammo(val2, Helper.RandomValue(pars2.Ammo));
				}
				if (operation == "ammotype")
				{
					text = Actions.AmmoType(val2, pars2.AmmoType);
				}
				if (operation == "stars" || operation == "level")
				{
					text = SetStars(val2, Helper.RandomValue(pars2.Level) - 1);
				}
				if (operation == "fuel" && pars2.Fuel != null)
				{
					text = SetFuel(val2, Helper.RandomValue(pars2.Fuel));
				}
				if (operation == "creator")
				{
					text = SetCreator(val2, pars2.Creator);
				}
				if (operation == "fuel" && pars2.Fuel == null)
				{
					text = PrintFuel(val2);
				}
				if (operation == "tame")
				{
					text = MakeTame(val2);
				}
				if (operation == "wild")
				{
					text = MakeWild(val2);
				}
				if (operation == "baby")
				{
					text = SetBaby(val2);
				}
				if (operation == "respawn")
				{
					text = Respawn(val2);
				}
				if (operation == "info")
				{
					text = GetInfo(val2);
				}
				if (operation == "components")
				{
					text = GetComponents(val2);
				}
				if (operation == "sleep")
				{
					text = MakeSleep(val2);
				}
				if (operation == "visual")
				{
					text = SetVisual(val2, pars2.Visual);
				}
				if (operation == "model")
				{
					text = SetModel(val2, Helper.RandomValue(pars2.Model));
				}
				if (operation == "helmet")
				{
					text = SetHelmet(val2, pars2.Helmet);
				}
				if (operation == "field" || operation == "f")
				{
					SetFields(val2, pars2.Fields);
				}
				if (operation == "left_hand")
				{
					text = SetLeftHand(val2, pars2.LeftHand);
				}
				if (operation == "right_hand")
				{
					text = SetRightHand(val2, pars2.RightHand);
				}
				if (operation == "chest")
				{
					text = SetChest(val2, pars2.Chest);
				}
				if (operation == "shoulders")
				{
					text = SetShoulder(val2, pars2.Shoulders);
				}
				if (operation == "legs")
				{
					text = SetLegs(val2, pars2.Legs);
				}
				if (operation == "utility")
				{
					text = SetUtility(val2, pars2.Utility);
				}
				if (operation == "prefab")
				{
					text = SetPrefab(val2, pars2.Prefab);
				}
				if (operation == "copy")
				{
					text = CopyId(val2);
				}
				if (operation == "status" && pars2.StatusName != null)
				{
					text = SetStatus(val2, pars2.StatusName, Helper.RandomValue(pars2.StatusDuration), Helper.RandomValue(pars2.StatusIntensity));
				}
				if (operation == "move")
				{
					text = Move(val2, Helper.RandomValue(pars2.Offset), pars2.Origin);
				}
				if (operation == "mirror" && pars2.Center.HasValue)
				{
					text = Mirror(val2, pars2.Center.Value);
				}
				if (operation == "distant" && pars2.Distant.HasValue)
				{
					text = SetDistant(val2, pars2.Distant.Value);
				}
				if (operation == "persist" && pars2.Persist.HasValue)
				{
					text = SetPersistent(val2, pars2.Persist.Value);
				}
				if (operation == "rotate")
				{
					text = ((!pars2.ResetRotation) ? Rotate(val2, Helper.RandomValue(pars2.Rotation), pars2.Origin, pars2.Center) : ResetRotation(val2));
				}
				if (operation == "scale")
				{
					text = Scale(val2, Helper.RandomValue(pars2.Scale));
				}
				if (operation == "remove")
				{
					UndoHelper.AddRemoveAction(val2.GetZDO());
					Actions.RemoveZDO(val2.GetZDO());
					text = "Entity ¤ destroyed.";
				}
				if (!(text == ""))
				{
					string text2 = text.Replace("¤", prefabName);
					if (num == 1)
					{
						Helper.AddMessage(context2, text2, false);
					}
					else
					{
						context2.AddString(text2);
					}
				}
			}
		}
		if (operations.Contains<string>("move") || operations.Contains<string>("scale") || operations.Contains<string>("rotate") || operations.Contains<string>("mirror"))
		{
			array = views;
			WearNTear val4 = default(WearNTear);
			foreach (ZNetView val3 in array)
			{
				if (Object.op_Implicit((Object)(object)val3) && ((Component)val3).TryGetComponent<WearNTear>(ref val4))
				{
					val4.m_colliders = null;
				}
			}
		}
		UndoHelper.EndAction();
	}

	public ObjectCommand()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		new ObjectAutoComplete();
		object obj = _003C_003Ec._003C_003E9__4_0;
		if (obj == null)
		{
			ConsoleEvent val = delegate(ConsoleEventArgs args)
			{
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
				ObjectParameters objectParameters = new ObjectParameters(args);
				ZNetView[] views;
				if (objectParameters.Connect)
				{
					ZNetView hovered = Selector.GetHovered(50f, objectParameters.IncludedIds, objectParameters.Components, objectParameters.ExcludedIds);
					if ((Object)(object)hovered == (Object)null)
					{
						return;
					}
					views = Selector.GetConnected(hovered, objectParameters.ConnectionIds, objectParameters.IncludedIds, objectParameters.ExcludedIds);
				}
				else if (objectParameters.Radius != null)
				{
					views = Selector.GetNearby(objectParameters.IncludedIds, objectParameters.Components, objectParameters.ExcludedIds, (Vector3)(((_003F?)objectParameters.Center) ?? objectParameters.From), objectParameters.Radius, objectParameters.Height);
				}
				else if (objectParameters.Width != null && objectParameters.Depth != null)
				{
					views = Selector.GetNearby(objectParameters.IncludedIds, objectParameters.Components, objectParameters.ExcludedIds, (Vector3)(((_003F?)objectParameters.Center) ?? objectParameters.From), objectParameters.Angle, objectParameters.Width, objectParameters.Depth, objectParameters.Height);
				}
				else
				{
					ZNetView hovered2 = Selector.GetHovered(50f, objectParameters.IncludedIds, objectParameters.ExcludedIds);
					if ((Object)(object)hovered2 == (Object)null)
					{
						return;
					}
					if (!Selector.GetPrefabs(objectParameters.IncludedIds).Contains(hovered2.GetZDO().GetPrefab()))
					{
						Helper.AddMessage(args.Context, "Skipped: " + ((Object)hovered2).name + " has invalid id.", false);
						return;
					}
					views = (ZNetView[])(object)new ZNetView[1] { hovered2 };
				}
				Execute(args.Context, objectParameters, objectParameters.Operations, views);
			};
			_003C_003Ec._003C_003E9__4_0 = val;
			obj = (object)val;
		}
		Helper.Command("object", "Modifies objects.", (ConsoleEvent)obj);
	}

	private static string ChangeHealth(ZNetView obj, float amount, bool isPercentage)
	{
		if (!Object.op_Implicit((Object)(object)((Component)obj).GetComponent<Character>()) && !Object.op_Implicit((Object)(object)((Component)obj).GetComponent<WearNTear>()) && !Object.op_Implicit((Object)(object)((Component)obj).GetComponent<TreeLog>()) && !Object.op_Implicit((Object)(object)((Component)obj).GetComponent<Destructible>()) && !Object.op_Implicit((Object)(object)((Component)obj).GetComponent<TreeBase>()))
		{
			return "Skipped: ¤ is not a creature or a destructible.";
		}
		UndoHelper.AddEditAction(obj);
		float num = Actions.SetHealth(((Component)obj).gameObject, amount, isPercentage);
		string arg = ((amount == 0f) ? "default" : (isPercentage ? $"{100f * amount:0.##} %" : amount.ToString("F0")));
		return $"¤ health changed from {num:F0} to {arg}.";
	}

	private static string SetStars(ZNetView view, int amount)
	{
		Character component = ((Component)view).GetComponent<Character>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not a creature.";
		}
		UndoHelper.AddEditAction(view);
		int num = component.GetLevel() - 1;
		Actions.SetLevel(component, amount + 1);
		return $"¤ stars changed from {num} to {amount}.";
	}

	private static string PrintFuel(ZNetView view)
	{
		Smelter val = default(Smelter);
		Fireplace val2 = default(Fireplace);
		CookingStation val3 = default(CookingStation);
		if (!((((Component)view).TryGetComponent<Smelter>(ref val) && (Object)(object)val.m_fuelItem != (Object)null) | (((Component)view).TryGetComponent<Fireplace>(ref val2) && (Object)(object)val2.m_fuelItem != (Object)null) | (((Component)view).TryGetComponent<CookingStation>(ref val3) && (Object)(object)val3.m_fuelItem != (Object)null)))
		{
			return "Skipped: ¤ doesn't use fuel.";
		}
		float @float = view.GetZDO().GetFloat("fuel", 0f);
		return $"¤ has {@float} fuel.";
	}

	private static string SetFields(ZNetView view, Dictionary<string, object> fields)
	{
		UndoHelper.AddEditAction(view);
		int num = Actions.SetFields(view, fields);
		if (num == 0)
		{
			return "Skipped: ¤ doesn't have valid components.";
		}
		if (num == fields.Count)
		{
			return $"¤ {fields.Count} fields set.";
		}
		return $"¤ {num} of {fields.Count} fields set.";
	}

	private static string SetFuel(ZNetView view, float amount)
	{
		Smelter val = default(Smelter);
		Fireplace val2 = default(Fireplace);
		CookingStation val3 = default(CookingStation);
		if (!((((Component)view).TryGetComponent<Smelter>(ref val) && (Object)(object)val.m_fuelItem != (Object)null) | (((Component)view).TryGetComponent<Fireplace>(ref val2) && (Object)(object)val2.m_fuelItem != (Object)null) | (((Component)view).TryGetComponent<CookingStation>(ref val3) && (Object)(object)val3.m_fuelItem != (Object)null)))
		{
			return "Skipped: ¤ doesn't use fuel.";
		}
		UndoHelper.AddEditAction(view);
		float @float = view.GetZDO().GetFloat("fuel", 0f);
		Actions.SetFuel(view, amount);
		return $"¤ fuel changed from {@float} to {amount}.";
	}

	private static string Move(ZNetView view, Vector3 offset, string origin)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		UndoHelper.AddEditAction(view);
		Actions.Move(view, offset, origin);
		return $"¤ moved {offset:F1} from the {origin}.";
	}

	private static string ResetRotation(ZNetView view)
	{
		UndoHelper.AddEditAction(view);
		Actions.ResetRotation(view);
		return "¤ rotation reseted.";
	}

	private static string Mirror(ZNetView view, Vector3 center)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		UndoHelper.AddEditAction(view);
		Actions.Mirror(view, center);
		return "¤ mirrored.";
	}

	private static string Rotate(ZNetView view, Vector3 rotation, string origin, Vector3? center = null)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		UndoHelper.AddEditAction(view);
		Actions.Rotate(view, rotation, origin, center);
		return $"¤ rotated {rotation:F1} from the {origin}.";
	}

	private static string Scale(ZNetView view, Vector3 scale)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		UndoHelper.AddEditAction(view);
		string arg = (Actions.Scale(view, scale) ? " (scaling enabled)" : "");
		return $"¤ scaled to {scale:F1}{arg}.";
	}

	private static string SetDistant(ZNetView view, bool distant)
	{
		UndoHelper.AddEditAction(view);
		Actions.SetDistant(((Component)view).gameObject, distant);
		if (!distant)
		{
			return "¤ made normally loaded.";
		}
		return "¤ made distant loaded.";
	}

	private static string SetPersistent(ZNetView view, bool persist)
	{
		UndoHelper.AddEditAction(view);
		Actions.SetPersistent(((Component)view).gameObject, persist);
		if (!persist)
		{
			return "¤ made non-persistent.";
		}
		return "¤ made persistent.";
	}

	private static string SetBaby(ZNetView view)
	{
		Growup component = ((Component)view).GetComponent<Growup>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not an offspring.";
		}
		UndoHelper.AddEditAction(view);
		Actions.SetBaby(component);
		return "¤ growth disabled.";
	}

	private static string Respawn(ZNetView view)
	{
		if (!Actions.CanRespawn(((Component)view).gameObject))
		{
			return "Skipped: ¤ is not a loot container, pickable or spawn point.";
		}
		UndoHelper.AddEditAction(view);
		Actions.Respawn(((Component)view).gameObject);
		return "¤ respawned.";
	}

	private static string MakeTame(ZNetView view)
	{
		Character component = ((Component)view).GetComponent<Character>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not a creature.";
		}
		UndoHelper.AddEditAction(view);
		Actions.SetTame(component, tame: true);
		return "¤ made tame.";
	}

	private static string MakeWild(ZNetView view)
	{
		Character component = ((Component)view).GetComponent<Character>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not a creature.";
		}
		UndoHelper.AddEditAction(view);
		Actions.SetTame(component, tame: false);
		return "¤ made wild.";
	}

	private static string MakeSleep(ZNetView view)
	{
		MonsterAI component = ((Component)view).GetComponent<MonsterAI>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not a creature.";
		}
		UndoHelper.AddEditAction(view);
		Actions.SetSleeping(component, sleep: true);
		return "¤ made to sleep.";
	}

	private static string SetCreator(ZNetView view, long creator)
	{
		Piece component = ((Component)view).GetComponent<Piece>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not a piece.";
		}
		UndoHelper.AddEditAction(view);
		long num = Actions.SetCreator(component, creator);
		return $"Creator of ¤ set from {num} to {creator}.";
	}

	private static string SetPrefab(ZNetView view, string prefab)
	{
		UndoHelper.AddEditAction(view);
		if (Actions.SetPrefab(view, prefab))
		{
			return "Prefab of ¤ set to " + prefab + ".";
		}
		return "Error: Prefab of ¤ was not set to " + prefab + ". Probably invalid prefab name.";
	}

	private static string CopyId(ZNetView view)
	{
		GUIUtility.systemCopyBuffer = ((Object)ZNetScene.instance.GetPrefab(view.GetZDO().GetPrefab())).name;
		return "Copied id of ¤.";
	}

	private static string SetVisual(ZNetView view, Item? item)
	{
		if (item == null)
		{
			return "Skipped: Invalid item.";
		}
		ItemStand component = ((Component)view).GetComponent<ItemStand>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not an item stand.";
		}
		UndoHelper.AddEditAction(view);
		Actions.SetVisual(component, item);
		return $"Visual of ¤ set to {item.Name} with variant {item.Variant}.";
	}

	private static string SetHelmet(ZNetView view, Item? item)
	{
		if (item == null)
		{
			return "Skipped: Invalid item.";
		}
		Character component = ((Component)view).GetComponent<Character>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not a creature.";
		}
		UndoHelper.AddEditAction(view);
		Actions.SetVisual(component, (VisSlot)6, item);
		return $"Helmet of ¤ set to {item.Name} with variant {item.Variant}.";
	}

	private static string SetLeftHand(ZNetView view, Item? item)
	{
		if (item == null)
		{
			return "Skipped: Invalid item.";
		}
		Character component = ((Component)view).GetComponent<Character>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not a creature.";
		}
		UndoHelper.AddEditAction(view);
		Actions.SetVisual(component, (VisSlot)0, item);
		return $"Left hand of ¤ set to {item.Name} with variant {item.Variant}.";
	}

	private static string SetRightHand(ZNetView view, Item? item)
	{
		if (item == null)
		{
			return "Skipped: Invalid item.";
		}
		Character component = ((Component)view).GetComponent<Character>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not a creature.";
		}
		UndoHelper.AddEditAction(view);
		Actions.SetVisual(component, (VisSlot)1, item);
		return $"Right hand of ¤ set to {item.Name} with variant {item.Variant}.";
	}

	private static string SetChest(ZNetView view, Item? item)
	{
		if (item == null)
		{
			return "Skipped: Invalid item.";
		}
		Character component = ((Component)view).GetComponent<Character>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not a creature.";
		}
		UndoHelper.AddEditAction(view);
		Actions.SetVisual(component, (VisSlot)4, item);
		return $"Chest of ¤ set to {item.Name} with variant {item.Variant}.";
	}

	private static string SetShoulder(ZNetView view, Item? item)
	{
		if (item == null)
		{
			return "Skipped: Invalid item.";
		}
		Character component = ((Component)view).GetComponent<Character>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not a creature.";
		}
		UndoHelper.AddEditAction(view);
		Actions.SetVisual(component, (VisSlot)7, item);
		return $"Shoulder of ¤ set to {item.Name} with variant {item.Variant}.";
	}

	private static string SetLegs(ZNetView view, Item? item)
	{
		if (item == null)
		{
			return "Skipped: Invalid item.";
		}
		Character component = ((Component)view).GetComponent<Character>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not a creature.";
		}
		UndoHelper.AddEditAction(view);
		Actions.SetVisual(component, (VisSlot)5, item);
		return $"Legs of ¤ set to {item.Name} with variant {item.Variant}.";
	}

	private static string SetModel(ZNetView view, int index)
	{
		Character component = ((Component)view).GetComponent<Character>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not a creature.";
		}
		Actions.SetModel(component, index);
		return $"Model of ¤ set to {index}.";
	}

	private static string SetUtility(ZNetView view, Item? item)
	{
		if (item == null)
		{
			return "Skipped: Invalid item.";
		}
		Character component = ((Component)view).GetComponent<Character>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return "Skipped: ¤ is not a creature.";
		}
		UndoHelper.AddEditAction(view);
		Actions.SetVisual(component, (VisSlot)8, item);
		return $"Utility item of ¤ set to {item.Name} with variant {item.Variant}.";
	}

	private static string SetStatus(ZNetView obj, string name, float duration, float intensity)
	{
		Character val = default(Character);
		if (!((Component)obj).TryGetComponent<Character>(ref val))
		{
			return "Skipped: ¤ is not a creature.";
		}
		obj.ClaimOwnership();
		int hashCode = name.GetHashCode();
		SEMan sEMan = val.GetSEMan();
		if (sEMan != null)
		{
			sEMan.AddStatusEffect(hashCode, true, 0, 0f, (short)(-1));
		}
		SEMan sEMan2 = val.GetSEMan();
		StatusEffect val2 = ((sEMan2 != null) ? sEMan2.GetStatusEffect(hashCode) : null);
		if ((Object)(object)val2 == (Object)null)
		{
			return "Failed to set status of ¤ to " + name;
		}
		val2.m_ttl = duration;
		SE_Shield val3 = (SE_Shield)(object)((val2 is SE_Shield) ? val2 : null);
		if (val3 != null)
		{
			val3.m_absorbDamage = intensity;
		}
		SE_Burning val4 = (SE_Burning)(object)((val2 is SE_Burning) ? val2 : null);
		if (val4 != null)
		{
			if (name == "Burning")
			{
				val4.m_fireDamageLeft = 0f;
				val4.AddFireDamage(intensity);
			}
			else
			{
				val4.m_spiritDamageLeft = 0f;
				val4.AddSpiritDamage(intensity);
			}
		}
		SE_Poison val5 = (SE_Poison)(object)((val2 is SE_Poison) ? val2 : null);
		if (val5 != null)
		{
			val5.m_damageLeft = intensity;
			val5.m_damagePerHit = intensity / val2.m_ttl * val5.m_damageInterval;
		}
		return "Status of ¤ set to " + name;
	}

	private static string GetInfo(ZNetView obj)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		List<string> list = new List<string>(3)
		{
			"Id: ¤",
			"Pos: " + Helper.PrintVectorXZY(((Component)obj).transform.position),
			"Rot: " + Helper.PrintAngleYXZ(((Component)obj).transform.rotation)
		};
		if (obj.m_syncInitialScale)
		{
			list.Add("Scale: " + Helper.PrintVectorXZY(((Component)obj).transform.localScale));
		}
		Character component = ((Component)obj).GetComponent<Character>();
		if (Object.op_Implicit((Object)(object)component))
		{
			float health = component.GetHealth();
			if (health < 0f || health > 1E+17f)
			{
				list.Add("Health: Infinite");
			}
			else
			{
				list.Add("Health: " + health.ToString("F0") + " / " + component.GetMaxHealth());
			}
			list.Add("Faction: " + ((object)(Faction)(ref component.m_faction)).ToString());
			list.Add("Stars: " + (component.GetLevel() - 1));
			list.Add("Tamed: " + (component.IsTamed() ? "Yes" : "No"));
			Growup component2 = ((Component)obj).GetComponent<Growup>();
			if (Object.op_Implicit((Object)(object)component2))
			{
				list.Add("Baby: " + ((component2.m_baseAI.GetTimeSinceSpawned().TotalSeconds < 0.0) ? "Yes" : "No"));
			}
		}
		else
		{
			float health2 = Actions.GetHealth(obj);
			if (health2 < 0f || health2 > 1E+17f)
			{
				list.Add("Health: Infinite");
			}
			else
			{
				list.Add("Health: " + health2.ToString("F0"));
			}
		}
		VisEquipment component3 = ((Component)obj).GetComponent<VisEquipment>();
		if (Object.op_Implicit((Object)(object)component3))
		{
			if (component3.m_rightItem != 0)
			{
				list.Add("Right hand: " + DataHelper.Name(component3.m_rightItem));
			}
			if (component3.m_leftItem != 0)
			{
				list.Add("Left hand: " + DataHelper.Name(component3.m_leftItem));
			}
			if (component3.m_helmetItem != 0)
			{
				list.Add("Helmet: " + DataHelper.Name(component3.m_helmetItem));
			}
			if (component3.m_shoulderItem != 0)
			{
				list.Add("Shoulders: " + DataHelper.Name(component3.m_shoulderItem));
			}
			if (component3.m_chestItem != 0)
			{
				list.Add("Chest: " + DataHelper.Name(component3.m_chestItem));
			}
			if (component3.m_legItem != 0)
			{
				list.Add("Legs: " + DataHelper.Name(component3.m_legItem));
			}
			if (component3.m_utilityItem != 0)
			{
				list.Add("Utility: " + DataHelper.Name(component3.m_utilityItem));
			}
		}
		Piece component4 = ((Component)obj).GetComponent<Piece>();
		if (Object.op_Implicit((Object)(object)component4))
		{
			list.Add("Creator ID: " + component4.GetCreator());
		}
		return string.Join(", ", list);
	}

	private static string GetComponents(ZNetView obj)
	{
		return string.Join(", ", ComponentInfo.Get(obj));
	}
}
