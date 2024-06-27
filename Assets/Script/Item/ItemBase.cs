using DG.Tweening;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class ItemBase
{
    public ItemData data;
    public ItemConfig config;
    public ActorManager owner;
    public virtual void UpdateData(ItemData itemData)
    {
        data = itemData;
        config = ItemConfigData.GetItemConfig(itemData.Item_ID);
    }
    /// <summary>
    /// �����ʼ��
    /// </summary>
    /// <returns></returns>
    public virtual ItemData GetRandomInitData(int id,int count,int val)
    {
        ItemData itemData = new ItemData();
        itemData.Item_ID = id;
        itemData.Item_Count = count;
        itemData.Item_Val = val;
        return itemData;
    }
    /// <summary>
    /// ���Ƹ���
    /// </summary>
    public virtual void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + config.Item_ID.ToString());
        gridCell.text_Info.text = data.Item_Count.ToString();
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    public virtual void LeftClickGridCell(UI_GridCell gridCell,ItemData itemData)
    {

    }
    /// <summary>
    /// �һ�����
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    public virtual void RightClickGridCell(UI_GridCell gridCell,ItemData itemData)
    {

    }
    /// <summary>
    /// ��������
    /// </summary>
    public virtual void DrawItemObj(ItemNetObj obj)
    {
        obj.icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID);
    }
    /// <summary>
    /// ���ŵ��䶯��
    /// </summary>
    /// <param name="obj"></param>
    public virtual void PlayDropAnim(ItemNetObj obj)
    {
        obj.root.enabled = true;
        obj.icon.transform.DOKill();
        obj.icon.transform.localScale = Vector3.zero;
        UnityEngine.Random.InitState(data.Item_Seed);
        Vector3 point = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(0.2f, 0.5f);
        obj.icon.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        obj.icon.transform.DOLocalJump(point, 1, 1, 0.5f).OnComplete(() =>
        {
            obj.root.enabled = false;
            obj.icon.transform.DOPunchScale(new Vector3(0.2f, -0.2f, 0), 0.1f).SetEase(Ease.OutBack);
        }).SetEase(Ease.InOutQuad);
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="item"></param>
    public virtual void BeHolding(ActorManager owner,BaseBodyController body)
    {
        this.owner = owner;
        body.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID);
    }
    /// <summary>
    /// ����(ͷ��)
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void BeWearingOnHead(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        body.Head_Item.GetComponent<SpriteRenderer>().sprite
           = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID);
    }
    /// <summary>
    /// ����(����)
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void BeWearingOnBody(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        body.Body_Item.GetComponent<SpriteRenderer>().sprite
           = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID);
    }
    /// <summary>
    /// ʳ��
    /// </summary>
    public virtual void BeEating(ActorManager who)
    {

    }
    /// <summary>
    /// �ͷ�
    /// </summary>
    /// <param name="who"></param>
    public virtual void BeRelease(ActorManager who)
    {

    }
    /// <summary>
    /// �Ҽ����
    /// </summary>
    public virtual void ClickRightClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// �Ҽ���ѹ
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="state"></param>
    /// <param name="input"></param>
    /// <param name="showSI"></param>
    /// <returns>�ﵽ���ֵ</returns>
    public virtual bool PressRightClick(float dt, bool state, bool input, bool showSI)
    {
        return true;
    }
    /// <summary>
    /// �Ҽ��ͷ�
    /// </summary>
    public virtual void ReleaseRightClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// ������
    /// </summary>
    public virtual void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// �����ѹ
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="state"></param>
    /// <param name="input"></param>
    /// <param name="showSI"></param>
    /// <returns>�ﵽ���ֵ</returns>
    public virtual bool PressLeftClick(float dt, bool state, bool input, bool showSI)
    {
        return true;
    }
    /// <summary>
    /// ����ͷ�
    /// </summary>
    public virtual void ReleaseLeftClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// ���λ��
    /// </summary>
    public virtual void FaceTo(Vector3 mouse, float time)
    {

    }
}
[Serializable]
public struct ItemData : INetworkStruct
{
    public int Item_ID;
    public int Item_Seed;
    public int Item_Count;
    public int Item_Val;
}
