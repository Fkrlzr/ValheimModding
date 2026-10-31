using System.Collections.Generic;
using System.Linq;
using Service;
using UnityEngine;

namespace Data;

public class ResolvedDataEntry
{
	public Dictionary<int, string>? Strings;

	public Dictionary<int, float>? Floats;

	public Dictionary<int, int>? Ints;

	public Dictionary<int, long>? Longs;

	public Dictionary<int, Vector3>? Vecs;

	public Dictionary<int, Quaternion>? Quats;

	public Dictionary<int, byte[]>? ByteArrays;

	public ConnectionType? ConnectionType;

	public int ConnectionHash;

	public ZDOID? OriginalId;

	public ZDOID? TargetConnectionId;

	public Vector3? Position;

	public Vector3? Rotation;

	public bool? Persistent;

	public bool? Distant;

	public ObjectType? Priority;

	public void Load(DataEntry data, Functions f, ZDO? source)
	{
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<int, IFloatValue>? floats = data.Floats;
		if (floats != null && floats.Count > 0)
		{
			foreach (KeyValuePair<int, IFloatValue> @float in data.Floats)
			{
				float? num = @float.Value.Get(f);
				if (num.HasValue)
				{
					AddFloat(@float.Key, num.Value);
				}
			}
		}
		Dictionary<int, IIntValue>? ints = data.Ints;
		if (ints != null && ints.Count > 0)
		{
			foreach (KeyValuePair<int, IIntValue> int2 in data.Ints)
			{
				int? num2 = int2.Value.Get(f);
				if (num2.HasValue)
				{
					AddInt(int2.Key, num2.Value);
				}
			}
		}
		Dictionary<int, ILongValue>? longs = data.Longs;
		if (longs != null && longs.Count > 0)
		{
			foreach (KeyValuePair<int, ILongValue> @long in data.Longs)
			{
				long? num3 = @long.Value.Get(f);
				if (num3.HasValue)
				{
					AddLong(@long.Key, num3.Value);
				}
			}
		}
		Dictionary<int, IStringValue>? strings = data.Strings;
		if (strings != null && strings.Count > 0)
		{
			foreach (KeyValuePair<int, IStringValue> @string in data.Strings)
			{
				string text = @string.Value.Get(f);
				if (text != null)
				{
					AddString(@string.Key, text);
				}
			}
		}
		Dictionary<int, IVector3Value>? vecs = data.Vecs;
		if (vecs != null && vecs.Count > 0)
		{
			foreach (KeyValuePair<int, IVector3Value> vec in data.Vecs)
			{
				Vector3? val = vec.Value.Get(f);
				if (val.HasValue)
				{
					AddVec(vec.Key, val.Value);
				}
			}
		}
		Dictionary<int, IQuaternionValue>? quats = data.Quats;
		if (quats != null && quats.Count > 0)
		{
			foreach (KeyValuePair<int, IQuaternionValue> quat in data.Quats)
			{
				Quaternion? val2 = quat.Value.Get(f);
				if (val2.HasValue)
				{
					AddQuat(quat.Key, val2.Value);
				}
			}
		}
		Dictionary<int, IBytesValue>? byteArrays = data.ByteArrays;
		if (byteArrays != null && byteArrays.Count > 0)
		{
			foreach (KeyValuePair<int, IBytesValue> byteArray in data.ByteArrays)
			{
				byte[] array = byteArray.Value.Get(f);
				if (array != null)
				{
					AddByteArray(byteArray.Key, array);
				}
			}
		}
		byte[] array2 = data.CreateItemData(f, source);
		if (array2 != null)
		{
			AddByteArray(ZDOVars.s_itemData, array2);
		}
		byte[] array3 = data.CreateInventory(f, source);
		if (array3 != null)
		{
			AddByteArray(ZDOVars.s_items, array3);
		}
		Dictionary<int, IBoolValue>? bools = data.Bools;
		if (bools != null && bools.Count > 0)
		{
			foreach (KeyValuePair<int, IBoolValue> @bool in data.Bools)
			{
				int? @int = @bool.Value.GetInt(f);
				if (@int.HasValue)
				{
					AddInt(@bool.Key, @int.Value);
				}
			}
		}
		Dictionary<int, IHashValue>? hashes = data.Hashes;
		if (hashes != null && hashes.Count > 0)
		{
			foreach (KeyValuePair<int, IHashValue> hash in data.Hashes)
			{
				int? num4 = hash.Value.Get(f);
				if (num4.HasValue)
				{
					AddInt(hash.Key, num4.Value);
				}
			}
		}
		if (data.Components != null)
		{
			foreach (KeyValuePair<int, IIntValue> component in data.Components)
			{
				int? num5 = component.Value.Get(f);
				if (num5.HasValue)
				{
					AddInt(component.Key, num5.Value);
				}
			}
		}
		ConnectionHash = data.ConnectionHash;
		ConnectionType = data.ConnectionType;
		if (data.OriginalId != null)
		{
			OriginalId = data.OriginalId.Get(f);
		}
		if (data.TargetConnectionId != null)
		{
			TargetConnectionId = data.TargetConnectionId.Get(f);
		}
		Distant = data.Distant?.GetBool(f);
		Persistent = data.Persistent?.GetBool(f);
		Priority = data.Priority;
		Position = data.Position?.Get(f) ?? Position;
		IQuaternionValue? rotation = data.Rotation;
		Vector3? obj;
		if (rotation == null)
		{
			obj = null;
		}
		else
		{
			Quaternion? val3 = rotation.Get(f);
			if (!val3.HasValue)
			{
				obj = null;
			}
			else
			{
				Quaternion valueOrDefault = val3.GetValueOrDefault();
				obj = ((Quaternion)(ref valueOrDefault)).eulerAngles;
			}
		}
		Rotation = obj ?? Rotation;
		ItemDataHelper.ConvertAll(this);
	}

	public virtual void Write(ZDO zdo)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		ZDOID uid = zdo.m_uid;
		Log.Warning("Writing ZDO with ID: " + ((object)(ZDOID)(ref uid)).ToString());
		ZDOID uid2 = zdo.m_uid;
		Dictionary<int, float>? floats = Floats;
		if (floats != null && floats.Count > 0)
		{
			ZDOHelper.Init<float>(ZDOExtraData.s_floats, uid2);
			foreach (KeyValuePair<int, float> @float in Floats)
			{
				ZDOExtraData.s_floats[uid2].SetValue(@float.Key, @float.Value);
			}
		}
		Dictionary<int, Vector3>? vecs = Vecs;
		if (vecs != null && vecs.Count > 0)
		{
			ZDOHelper.Init<Vector3>(ZDOExtraData.s_vec3, uid2);
			foreach (KeyValuePair<int, Vector3> vec in Vecs)
			{
				ZDOExtraData.s_vec3[uid2].SetValue(vec.Key, vec.Value);
			}
		}
		Dictionary<int, Quaternion>? quats = Quats;
		if (quats != null && quats.Count > 0)
		{
			ZDOHelper.Init<Quaternion>(ZDOExtraData.s_quats, uid2);
			foreach (KeyValuePair<int, Quaternion> quat in Quats)
			{
				ZDOExtraData.s_quats[uid2].SetValue(quat.Key, quat.Value);
			}
		}
		Dictionary<int, int>? ints = Ints;
		if (ints != null && ints.Count > 0)
		{
			ZDOHelper.Init<int>(ZDOExtraData.s_ints, uid2);
			foreach (KeyValuePair<int, int> @int in Ints)
			{
				ZDOExtraData.s_ints[uid2].SetValue(@int.Key, @int.Value);
			}
		}
		Dictionary<int, long>? longs = Longs;
		if (longs != null && longs.Count > 0)
		{
			ZDOHelper.Init<long>(ZDOExtraData.s_longs, uid2);
			foreach (KeyValuePair<int, long> @long in Longs)
			{
				ZDOExtraData.s_longs[uid2].SetValue(@long.Key, @long.Value);
			}
		}
		Dictionary<int, string>? strings = Strings;
		if (strings != null && strings.Count > 0)
		{
			ZDOHelper.Init<string>(ZDOExtraData.s_strings, uid2);
			foreach (KeyValuePair<int, string> @string in Strings)
			{
				ZDOExtraData.s_strings[uid2].SetValue(@string.Key, @string.Value);
			}
		}
		Dictionary<int, byte[]>? byteArrays = ByteArrays;
		if (byteArrays != null && byteArrays.Count > 0)
		{
			uid = zdo.m_uid;
			Log.Warning("Writing bytes to ZDO with ID: " + ((object)(ZDOID)(ref uid)).ToString());
			ZDOHelper.Init<byte[]>(ZDOExtraData.s_byteArrays, uid2);
			foreach (KeyValuePair<int, byte[]> byteArray in ByteArrays)
			{
				ZDOExtraData.s_byteArrays[uid2].SetValue(byteArray.Key, byteArray.Value);
			}
		}
		HandleConnection(zdo);
		HandleHashConnection(zdo);
		if (Distant.HasValue)
		{
			zdo.Distant = Distant.Value;
		}
		if (Persistent.HasValue)
		{
			zdo.Persistent = Persistent.Value;
		}
		if (Priority.HasValue)
		{
			zdo.Type = Priority.Value;
		}
		if (Position.HasValue)
		{
			zdo.SetPosition(Position.Value);
		}
		if (Rotation.HasValue)
		{
			zdo.m_rotation = Rotation.Value;
		}
	}

	public virtual bool HasSyncedChanges()
	{
		Dictionary<int, float>? floats = Floats;
		if (floats != null && floats.Count > 0)
		{
			return true;
		}
		Dictionary<int, int>? ints = Ints;
		if (ints != null && ints.Count > 0)
		{
			return true;
		}
		Dictionary<int, long>? longs = Longs;
		if (longs != null && longs.Count > 0)
		{
			return true;
		}
		Dictionary<int, string>? strings = Strings;
		if (strings != null && strings.Count > 0)
		{
			return true;
		}
		Dictionary<int, Vector3>? vecs = Vecs;
		if (vecs != null && vecs.Count > 0)
		{
			return true;
		}
		Dictionary<int, Quaternion>? quats = Quats;
		if (quats != null && quats.Count > 0)
		{
			return true;
		}
		Dictionary<int, byte[]>? byteArrays = ByteArrays;
		if (byteArrays != null && byteArrays.Count > 0)
		{
			return true;
		}
		if (ConnectionType.HasValue)
		{
			return true;
		}
		if (ConnectionHash != 0)
		{
			return true;
		}
		if (OriginalId.HasValue)
		{
			return true;
		}
		if (TargetConnectionId.HasValue)
		{
			return true;
		}
		if (Persistent.HasValue)
		{
			return true;
		}
		if (Distant.HasValue)
		{
			return true;
		}
		if (Priority.HasValue)
		{
			return true;
		}
		return false;
	}

	private void HandleConnection(ZDO ownZdo)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		ConnectionType valueOrDefault = ConnectionType.GetValueOrDefault();
		ZDOID val = (ZDOID)(((_003F?)TargetConnectionId) ?? ZDOID.None);
		ZDOID uid = ownZdo.m_uid;
		if (val != ZDOID.None)
		{
			ZDO zDO = ZDOMan.instance.GetZDO(val);
			if (zDO != null)
			{
				ownZdo.SetConnection(valueOrDefault, val);
				if ((int)valueOrDefault == 1)
				{
					zDO.SetConnection((ConnectionType)1, uid);
				}
			}
			return;
		}
		ZDOID originalId = (ZDOID)(((_003F?)OriginalId) ?? ZDOID.None);
		if (originalId == ZDOID.None)
		{
			return;
		}
		KeyValuePair<ZDOID, ZDOConnection> keyValuePair = ZDOExtraData.s_connections.FirstOrDefault((KeyValuePair<ZDOID, ZDOConnection> kvp) => kvp.Value.m_target == originalId);
		if (keyValuePair.Value != null)
		{
			ZDO zDO2 = ZDOMan.instance.GetZDO(keyValuePair.Key);
			if (zDO2 != null)
			{
				zDO2.SetConnection(keyValuePair.Value.m_type, uid);
			}
		}
	}

	private void HandleHashConnection(ZDO ownZdo)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Invalid comparison between Unknown and I4
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Invalid comparison between Unknown and I4
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Invalid comparison between Unknown and I4
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		ConnectionType type = ConnectionType.GetValueOrDefault();
		if (ConnectionHash == 0 || (int)type == 0)
		{
			return;
		}
		ZDOID uid = ownZdo.m_uid;
		ZDOExtraData.SetConnectionData(uid, type, ConnectionHash);
		_003F val = type ^ 0x10;
		bool flag = (type & 0x10) == 0;
		ZDOID val2 = ZDOExtraData.GetAllConnectionZDOIDs((ConnectionType)val).FirstOrDefault((ZDOID z) => ZDOExtraData.GetConnectionHashData(z, type)?.m_hash == ConnectionHash);
		if (val2 == ZDOID.None)
		{
			return;
		}
		ZDO zDO = ZDOMan.instance.GetZDO(val2);
		if (zDO != null)
		{
			if ((type & 3) > 0)
			{
				ZDO val3 = (ZDO)(flag ? ((object)ownZdo) : ((object)zDO));
				ZDOID val4 = (flag ? val2 : uid);
				val3.SetConnection((ConnectionType)3, val4);
			}
			if ((type & 2) > 0)
			{
				ZDO val5 = (ZDO)(flag ? ((object)ownZdo) : ((object)zDO));
				ZDOID val6 = (flag ? val2 : uid);
				val5.SetConnection((ConnectionType)2, val6);
			}
			if ((type & 1) > 0)
			{
				zDO.SetConnection((ConnectionType)1, uid);
				ownZdo.SetConnection((ConnectionType)1, val2);
			}
		}
	}

	protected virtual void AddString(int key, string value)
	{
		if (Strings == null)
		{
			Strings = new Dictionary<int, string>();
		}
		Strings[key] = value;
	}

	protected virtual void AddFloat(int key, float value)
	{
		if (Floats == null)
		{
			Floats = new Dictionary<int, float>();
		}
		Floats[key] = value;
	}

	protected virtual void AddInt(int key, int value)
	{
		if (Ints == null)
		{
			Ints = new Dictionary<int, int>();
		}
		Ints[key] = value;
	}

	protected virtual void AddLong(int key, long value)
	{
		if (Longs == null)
		{
			Longs = new Dictionary<int, long>();
		}
		Longs[key] = value;
	}

	protected virtual void AddVec(int key, Vector3 value)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (Vecs == null)
		{
			Vecs = new Dictionary<int, Vector3>();
		}
		Vecs[key] = value;
	}

	protected virtual void AddQuat(int key, Quaternion value)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (Quats == null)
		{
			Quats = new Dictionary<int, Quaternion>();
		}
		Quats[key] = value;
	}

	protected virtual void AddByteArray(int key, byte[] value)
	{
		if (ByteArrays == null)
		{
			ByteArrays = new Dictionary<int, byte[]>();
		}
		ByteArrays[key] = value;
	}
}
