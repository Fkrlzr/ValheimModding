using Splatform;
using UnityEngine;

namespace InfinityHammer;

public class NoCreator
{
	public static void Set(ZNetView view, Piece piece)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		ZDO zDO = view.GetZDO();
		if (Configuration.NoCreator)
		{
			zDO.RemoveLong(ZDOVars.s_creator);
			zDO.RemoveInt(ZDOVars.s_creatorIndex);
			piece.m_creator = 0L;
			piece.m_creatorPlatformUserIDIndex = -1;
			ZDOHelper.Remove<string>(ZDOExtraData.s_strings, zDO.m_uid, ZDOVars.s_creatorName);
			ZDOHelper.Remove<string>(ZDOExtraData.s_strings, zDO.m_uid, Hashes.XRaySteamName);
			ZDOHelper.Remove<string>(ZDOExtraData.s_strings, zDO.m_uid, Hashes.XRaySteamID);
			zDO.RemoveLong(Hashes.XRayCreatedID);
		}
		else
		{
			piece.m_creator = 0L;
			piece.SetCreator(Game.instance.GetPlayerProfile().GetPlayerID(), ((IUser)PlatformManager.DistributionPlatform.LocalUser).PlatformUserID);
		}
		if (Configuration.NoTarget && (((StaticTarget)piece).m_primaryTarget || ((StaticTarget)piece).m_randomTarget))
		{
			zDO.Set(Hashes.HasFields, true);
			zDO.Set(Hashes.HasFieldsStaticTarget, true);
			if (((StaticTarget)piece).m_primaryTarget)
			{
				((StaticTarget)piece).m_primaryTarget = false;
				zDO.Set(Hashes.StaticTargetPrimaryTarget, false);
			}
			if (((StaticTarget)piece).m_randomTarget)
			{
				((StaticTarget)piece).m_randomTarget = false;
				zDO.Set(Hashes.StaticTargetRandomTarget, false);
			}
			view.LoadFields();
		}
		StaticPhysics val = default(StaticPhysics);
		if (Configuration.NoPhysics && ((Component)piece).TryGetComponent<StaticPhysics>(ref val) && (val.m_fall || val.m_pushUp))
		{
			zDO.Set(Hashes.HasFields, true);
			zDO.Set(Hashes.HasFieldsStaticPhysics, true);
			if (val.m_fall)
			{
				val.m_fall = false;
				zDO.Set(Hashes.StaticPhysicsFall, false);
			}
			if (val.m_pushUp)
			{
				val.m_pushUp = false;
				zDO.Set(Hashes.StaticPhysicsPushUp, false);
			}
			view.LoadFields();
		}
		if (Configuration.NoRemove)
		{
			zDO.Set(Hashes.HasFields, true);
			zDO.Set(Hashes.HasFieldsPiece, true);
			if (((StaticTarget)piece).m_primaryTarget)
			{
				zDO.Set(Hashes.PieceCanBeRemoved, false);
			}
			piece.m_canBeRemoved = false;
			view.LoadFields();
		}
	}
}
