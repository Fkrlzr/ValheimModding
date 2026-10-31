using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Splatform;
using UnityEngine;

namespace ServerDevcommands;

public abstract class Helper
{
	public static bool IsValid(ZoneLocation loc)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		if (loc != null)
		{
			_ = loc.m_prefab;
			if (!loc.m_prefab.IsValid)
			{
				return loc.m_prefab.m_name != null;
			}
			return true;
		}
		return false;
	}

	public static void AddMessage(Terminal context, string message, bool priority = false)
	{
		if ((Object)(object)context == (Object)(object)Console.instance || Settings.ChatOutput)
		{
			context.AddString(message);
		}
		MessageHud instance = MessageHud.instance;
		if (!Object.op_Implicit((Object)(object)instance) || !Object.op_Implicit((Object)(object)Player.m_localPlayer))
		{
			return;
		}
		if (priority)
		{
			MsgData[] array = instance.m_msgQeue.ToArray();
			instance.m_msgQeue.Clear();
			((Character)Player.m_localPlayer).Message((MessageType)1, message, 0, (Sprite)null, false);
			MsgData[] array2 = array;
			foreach (MsgData item in array2)
			{
				instance.m_msgQeue.Enqueue(item);
			}
			instance.m_msgQueueTimer = 10f;
		}
		else
		{
			((Character)Player.m_localPlayer).Message((MessageType)1, message, 0, (Sprite)null, false);
		}
	}

	public static GameObject? GetPrefab(string name)
	{
		string name2 = name;
		GameObject prefab = ZNetScene.instance.GetPrefab(name2);
		if (Object.op_Implicit((Object)(object)prefab))
		{
			return prefab;
		}
		string text = ParameterInfo.ObjectIds.Find((string id) => string.Equals(id, name2, StringComparison.OrdinalIgnoreCase));
		prefab = ((text == null) ? null : ZNetScene.instance.GetPrefab(text));
		if (!Object.op_Implicit((Object)(object)prefab))
		{
			((Character)Player.m_localPlayer).Message((MessageType)1, "Missing object " + name2, 0, (Sprite)null, false);
		}
		return prefab;
	}

	public static string GetPrefabName(int hash)
	{
		GameObject prefab = ZNetScene.instance.GetPrefab(hash);
		if (!Object.op_Implicit((Object)(object)prefab))
		{
			return "";
		}
		return Utils.GetPrefabName(prefab);
	}

	public static int Hash(string key)
	{
		if (int.TryParse(key, out var result))
		{
			return result;
		}
		if (key.StartsWith("$", StringComparison.InvariantCultureIgnoreCase))
		{
			int hash = ZSyncAnimation.GetHash(key.Substring(1));
			if (key == "$anim_speed")
			{
				return hash;
			}
			return 438569 + hash;
		}
		return StringExtensionMethods.GetStableHashCode(key);
	}

	public static bool Within(Range<float> range, float value)
	{
		if (range.Min == range.Max)
		{
			return value <= range.Max;
		}
		if (range.Min <= value)
		{
			return value <= range.Max;
		}
		return false;
	}

	public static bool Within(Range<float> range1, Range<float> range2, float value1, float value2)
	{
		if (value1 > range1.Max)
		{
			return false;
		}
		if (value2 > range2.Max)
		{
			return false;
		}
		if (range1.Min == range1.Max || value1 >= range1.Min)
		{
			return true;
		}
		if (range2.Min == range2.Max || value2 >= range2.Min)
		{
			return true;
		}
		return false;
	}

	public static float RandomValue(Range<float>? range)
	{
		if (range != null)
		{
			if (range.Min != range.Max)
			{
				return Random.Range(range.Min, range.Max);
			}
			return range.Min;
		}
		return 0f;
	}

	public static int RandomValue(Range<int>? range)
	{
		if (range != null)
		{
			if (range.Min != range.Max)
			{
				return Random.Range(range.Min, range.Max + 1);
			}
			return range.Min;
		}
		return 0;
	}

	public static Vector3 RandomValue(Range<Vector3>? range)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (range == null)
		{
			return Vector3.zero;
		}
		if (range.Uniform)
		{
			float num = Random.Range(0f, 1f);
			return Vector3.Lerp(range.Min, range.Max, num);
		}
		float num2 = Random.Range(range.Min.x, range.Max.x);
		float num3 = Random.Range(range.Min.y, range.Max.y);
		float num4 = Random.Range(range.Min.z, range.Max.z);
		return new Vector3(num2, num3, num4);
	}

	public static Quaternion RandomValue(Range<Quaternion>? range)
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		if (range == null)
		{
			return Quaternion.identity;
		}
		float num = Random.Range(range.Min.x, range.Max.x);
		float num2 = Random.Range(range.Min.y, range.Max.y);
		float num3 = Random.Range(range.Min.z, range.Max.z);
		float num4 = Random.Range(range.Min.w, range.Max.w);
		return new Quaternion(num, num2, num3, num4);
	}

	public static (string[] Parameters, Vector3 Position) AddPlayerPosXZ(string[] args, int count)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = (Object.op_Implicit((Object)(object)Player.m_localPlayer) ? ((Component)Player.m_localPlayer).transform.position : Vector3.zero);
		float num = ((args.Length > count) ? Parse.Float(args[count]) : val.x);
		float num2 = ((args.Length > count + 1) ? Parse.Float(args[count + 1]) : val.z);
		Vector3 item = default(Vector3);
		((Vector3)(ref item))._002Ector(num, 0f, num2);
		return (Parameters: args.Take(count).Append<string>(num.ToString("0.##", CultureInfo.InvariantCulture)).Append(num2.ToString("0.##", CultureInfo.InvariantCulture))
			.Concat<string>(args.Skip(count + 2))
			.ToArray(), Position: item);
	}

	public static (string[] Parameters, Vector3 Position) AddPlayerPosXZY(string[] args, int index)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Vector3 defaultValue = (Object.op_Implicit((Object)(object)Player.m_localPlayer) ? ((Component)Player.m_localPlayer).transform.position : Vector3.zero);
		Vector3 val = Parse.VectorXZY(Parse.Split(string.Join(",", args.Skip(index))), defaultValue);
		return (Parameters: args.Take(index).Append<string>(PrintVectorXZY(val)).ToArray(), Position: val);
	}

	public static PlayerInfo FindPlayer(string name, bool publicOnly = false)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		string name2 = name;
		List<PlayerInfo> players = ZNet.instance.m_players;
		PlayerInfo result = players.FirstOrDefault((PlayerInfo player) => player.m_name == name2 && (player.m_publicPosition || !publicOnly));
		if (!((ZDOID)(ref result.m_characterID)).IsNone())
		{
			return result;
		}
		result = players.FirstOrDefault((PlayerInfo player) => player.m_name.ToLower().StartsWith(name2.ToLower()) && (player.m_publicPosition || !publicOnly));
		if (!((ZDOID)(ref result.m_characterID)).IsNone())
		{
			return result;
		}
		result = players.FirstOrDefault((PlayerInfo player) => player.m_name.ToLower().Contains(name2.ToLower()) && (player.m_publicPosition || !publicOnly));
		if (!((ZDOID)(ref result.m_characterID)).IsNone())
		{
			return result;
		}
		throw new InvalidOperationException("Unable to find the player.");
	}

	public static float Round(float value)
	{
		return Mathf.Round(value * 1000f) / 1000f;
	}

	public static bool IsZero(float a)
	{
		return Mathf.Abs(a) < 0.001f;
	}

	public static bool Approx(float a, float b)
	{
		return Mathf.Abs(a - b) < 0.001f;
	}

	public static bool ApproxBetween(float a, float min, float max)
	{
		if (min - 0.001f <= a)
		{
			return a <= max + 0.001f;
		}
		return false;
	}

	public static string PrintVectorXZY(Vector3 vector)
	{
		return vector.x.ToString("0.##", CultureInfo.InvariantCulture) + ", " + vector.z.ToString("0.##", CultureInfo.InvariantCulture) + ", " + vector.y.ToString("0.##", CultureInfo.InvariantCulture);
	}

	public static string PrintVectorYXZ(Vector3 vector)
	{
		return vector.y.ToString("0.##", CultureInfo.InvariantCulture) + ", " + vector.x.ToString("0.##", CultureInfo.InvariantCulture) + ", " + vector.z.ToString("0.##", CultureInfo.InvariantCulture);
	}

	public static string PrintAngleYXZ(Quaternion quaternion)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return PrintVectorYXZ(((Quaternion)(ref quaternion)).eulerAngles);
	}

	public static void AddError(Terminal context, string message, bool priority = false)
	{
		AddMessage(context, "Error: " + message, priority);
	}

	public static bool? IsDown(string key)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (key.StartsWith("-", StringComparison.OrdinalIgnoreCase))
		{
			if (!Enum.TryParse<KeyCode>(key.Substring(1), ignoreCase: true, out KeyCode result))
			{
				return null;
			}
			return !Input.GetKey(result);
		}
		if (!Enum.TryParse<KeyCode>(key, ignoreCase: true, out KeyCode result2))
		{
			return null;
		}
		return Input.GetKey(result2);
	}

	public static Player GetPlayer()
	{
		Player localPlayer = Player.m_localPlayer;
		if (!Object.op_Implicit((Object)(object)localPlayer))
		{
			throw new InvalidOperationException("No player.");
		}
		return localPlayer;
	}

	public static void ArgsCheck(ConsoleEventArgs args, int amount, string message)
	{
		if (args.Length < amount)
		{
			throw new InvalidOperationException(message);
		}
	}

	public static void Command(string name, string description, ConsoleEvent action, bool remote = false)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		new ConsoleCommand(name, description, Catch(action), true, true, false, false, false, false, (ConsoleOptionsFetcher)null, false, remote, false);
	}

	public static void Command(string name, string description, ConsoleEvent action)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		new ConsoleCommand(name, description, Catch(action), true, true, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
	}

	public static ConsoleEvent Catch(ConsoleEvent action)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		ConsoleEvent action2 = action;
		return (ConsoleEvent)delegate(ConsoleEventArgs args)
		{
			try
			{
				action2.Invoke(args);
			}
			catch (InvalidOperationException ex)
			{
				AddError(args.Context, ex.Message);
			}
		};
	}

	public static void Command(string name, string description, ConsoleEventFailable action)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		new ConsoleCommand(name, description, Catch(action), true, true, false, false, false, false, (ConsoleOptionsFetcher)null, false, false, false);
	}

	public static ConsoleEventFailable Catch(ConsoleEventFailable action)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		ConsoleEventFailable action2 = action;
		return (ConsoleEventFailable)delegate(ConsoleEventArgs args)
		{
			try
			{
				return action2.Invoke(args);
			}
			catch (InvalidOperationException ex)
			{
				AddError(args.Context, ex.Message);
			}
			return (object)null;
		};
	}

	public static bool IsDedicated()
	{
		if (Object.op_Implicit((Object)(object)ZNet.instance))
		{
			return ZNet.instance.IsDedicated();
		}
		return true;
	}

	public static bool IsServer()
	{
		if (Object.op_Implicit((Object)(object)ZNet.instance))
		{
			return ZNet.instance.IsServer();
		}
		return false;
	}

	public static bool IsClient()
	{
		return !IsServer();
	}

	public static string GetPlayerID()
	{
		Player localPlayer = Player.m_localPlayer;
		return ((localPlayer != null) ? localPlayer.GetPlayerID().ToString() : null) ?? "";
	}

	public static string GetNetworkId()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		IDistributionPlatform distributionPlatform = PlatformManager.DistributionPlatform;
		object obj;
		if (distributionPlatform == null)
		{
			obj = null;
		}
		else
		{
			ILocalUser localUser = distributionPlatform.LocalUser;
			if (localUser == null)
			{
				obj = null;
			}
			else
			{
				PlatformUserID platformUserID = ((IUser)localUser).PlatformUserID;
				obj = ((object)(PlatformUserID)(ref platformUserID)).ToString();
			}
		}
		if (obj == null)
		{
			obj = "0";
		}
		return (string)obj;
	}
}
