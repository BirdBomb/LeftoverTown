using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ActorManager_Robot_SpiderThrow : ActorManager_Monster
{
    public Transform trans_Muzzle;

    #region//¼àÌý
    public override void ForState_Listen_MyselfInjured(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null && networkObject.TryGetComponent(out ActorManager who) && who.actorAuthority.isPlayer)
        {
            State_InAttack(who);
        }
    }
    public override void State_SecondUpdate()
    {
        State_UpdateSkillTimer();
        base.State_SecondUpdate();
    }
    public override void AllClient_UpdateHpBar(float val)
    {
        UI_BossInfo.Instance.Show(actorNetManager.Net_HpCur, actorNetManager.Local_HpMax, "ÖÀµ¯¾®", actorNetManager.Object.Id);
        base.AllClient_UpdateHpBar(val);
    }
    #endregion
    #region//¼ì²é
    public override bool State_CheckNearbyActor()
    {
        if (brainManager.globalTime_Now != GlobalTime.Evening) { return true; }
        List<ActorManager> temp = brainManager.State_GetNearbyActors();
        foreach (ActorManager actor in temp)
        {
            if (actionManager.LookAt(actor, State_CalculateView()))
            {
                if (actor.statusManager.statusType != StatusType.Animal_Common && actor.statusManager.statusType != StatusType.Monster_Common)
                {
                    State_InAttack(actor);
                    return true;
                }
            }
        }
        return false;
    }
    #endregion
    #region//Ë¼¿¼
    public override void State_ThinkByTimeUpdate(int date, int hour, GlobalTime time)
    {
        State_Think_GoToStroll_Long(2, 5);
    }
    #endregion
    #region//¼¼ÄÜ
    private enum Skill
    {
        Throw
    }
    public override void State_AttackLoop()
    {
        if (brainManager.ForAll_GetAttackTarget(out ActorManager target))
        {
            float realDistance = (target.transform.position - transform.position).sqrMagnitude;
            if (realDistance > 1)
            {
                State_Follow(target.pathManager.vector3Int_CurPos);
            }
        }
        base.State_AttackLoop();
    }
    public override bool State_Attack()
    {
        if (brainManager.ForAll_GetAttackTarget(out ActorManager target))
        {
            if (int_ThrowTimer <= 0)
            {
                actorNetManager.RPC_State_NpcUseSkill((int)Skill.Throw, pathManager.vector3Int_CurPos, target.actorNetManager.Object.Id);
                int_ThrowTimer = int_ThrowCD;
                return true;
            }
        }
        return false;
    }
    public override void ForAll_Listen_NpcAction(int id, Vector3Int vector3, NetworkId networkId)
    {
        if (id == (int)Skill.Throw)
        {
            AllClient_ThrowBullet(vector3, networkId);
        }
        base.ForAll_Listen_NpcAction(id, vector3, networkId);
    }
    public void State_UpdateSkillTimer()
    {
        int_ThrowTimer--;
    }
    #endregion
    #region//¼¼ÄÜ_Í¶ÖÀ·Éµ¯
    [Header("-------Í¶ÖÀ·Éµ¯--------")]
    [Header("·Éµ¯±¬Õ¨ÉËº¦")]
    public int int_BludgeoningDamage;
    [Header("·Éµ¯±¬Õ¨·¶Î§")]
    public float float_ExplodeRange;
    private const int int_ThrowCD = 10;
    private int int_ThrowTimer = 0;
    private void AllClient_ThrowBullet(Vector3Int pos,NetworkId target)
    {
        bodyController.SetAnimatorTrigger(BodyPart.Head, "Throw");
        bodyController.SetAnimatorFunc(BodyPart.Head, AllClient_AnimaEvent_Throw_0);
        bodyController.SetAnimatorFunc(BodyPart.Head, AllClient_AnimaEvent_Throw_1);
        bodyController.SetAnimatorFunc(BodyPart.Head, AllClient_AnimaEvent_Throw_2);
    }
    private bool AllClient_AnimaEvent_Throw_0(string str)
    {
        if (str.Equals("Throw_0"))
        {
            UnityEngine.Random.InitState(pathManager.vector3Int_CurPos.x * pathManager.vector3Int_CurPos.x + pathManager.vector3Int_CurPos.y);
            GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_SpiderThrowBomb");
            effect.transform.position = trans_Muzzle.position;

            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_Grenade");
            if (obj.TryGetComponent(out Bullet_Bomb bulletBase) && brainManager.ForAll_GetAttackTarget(out ActorManager target))
            {
                Vector3 offset = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), 0);
                bulletBase.SetPath(trans_Muzzle.position, target.transform.position + offset, 9);
                bulletBase.SetBomb(int_BludgeoningDamage, float_ExplodeRange);
                bulletBase.SetOwner(this);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool AllClient_AnimaEvent_Throw_1(string str)
    {
        if (str.Equals("Throw_1"))
        {
            UnityEngine.Random.InitState(pathManager.vector3Int_CurPos.x * pathManager.vector3Int_CurPos.x + pathManager.vector3Int_CurPos.y + 1);
            GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_SpiderThrowBomb");
            effect.transform.position = trans_Muzzle.position;

            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_Grenade");
            if (obj.TryGetComponent(out Bullet_Bomb bulletBase) && brainManager.ForAll_GetAttackTarget(out ActorManager target))
            {
                Vector3 offset = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), 0);
                bulletBase.SetPath(trans_Muzzle.position, target.transform.position + offset, 9);
                bulletBase.SetBomb(int_BludgeoningDamage, float_ExplodeRange);
                bulletBase.SetOwner(this);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool AllClient_AnimaEvent_Throw_2(string str)
    {
        if (str.Equals("Throw_2"))
        {
            UnityEngine.Random.InitState(pathManager.vector3Int_CurPos.x * pathManager.vector3Int_CurPos.x + pathManager.vector3Int_CurPos.y + 2);
            GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_SpiderThrowBomb");
            effect.transform.position = trans_Muzzle.position;

            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_Grenade");
            if (obj.TryGetComponent(out Bullet_Bomb bulletBase) && brainManager.ForAll_GetAttackTarget(out ActorManager target))
            {
                Vector3 offset = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), 0);
                bulletBase.SetPath(trans_Muzzle.position, target.transform.position + offset, 9);
                bulletBase.SetBomb(int_BludgeoningDamage, float_ExplodeRange);
                bulletBase.SetOwner(this);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
