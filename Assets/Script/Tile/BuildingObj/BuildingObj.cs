using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingObj : MonoBehaviour
{
    [HideInInspector]
    public BuildingTile buildingTile;
    [HideInInspector]
    public string info;
    /// <summary>
    /// ��
    /// </summary>
    public virtual void Bind(BuildingTile tile, out BuildingObj obj)
    {
        buildingTile = tile;
        obj = this;
    }
    public virtual void Start()
    {
        
    }
    #region//����
    /// <summary>
    /// ����
    /// </summary>
    public virtual void ActorInputKeycode(ActorManager actor, KeyCode code)
    {

    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public virtual bool PlayerHolding(PlayerCoreLocal player)
    {
        return false;
    }
    /// <summary>
    /// �ͷ�
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public virtual bool PlayerRelease(PlayerCoreLocal player)
    {
        return false;
    }
    /// <summary>
    /// ��ɫ����
    /// </summary>
    /// <returns></returns>
    public virtual bool ActorNearby(ActorManager actor)
    {
        return false;
    }
    /// <summary>
    /// ��ɫվ��
    /// </summary>
    public virtual void ActorStandOn(ActorManager actor)
    {

    }
    /// <summary>
    /// ��ɫԶ��
    /// </summary>
    /// <returns></returns>
    public virtual bool ActorFaraway(ActorManager actor)
    {
        return false;
    }
    #endregion
    #region//�߼�
    /// <summary>
    /// ����
    /// </summary>
    public virtual void Draw()
    {

    }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void TakeDamage(int val)
    {

    }
    /// <summary>
    /// �ƻ�
    /// </summary>
    public virtual void Broken()
    {

    }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void Loot()
    {

    }
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public virtual void ChangeInfo(string info)
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_UpdateBuildingInfo
        {
            pos = buildingTile.tilePos,
            info = info
        });
    }
    /// <summary>
    /// ����HP
    /// </summary>
    public virtual void ChangeHp()
    {

    }
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public virtual void UpdateInfo(string info)
    {
        this.info = info;
    }
    /// <summary>
    /// ����HP
    /// </summary>
    public virtual void UpdateHP(int hp)
    {

    }
    #endregion
}
