using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.U2D;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class ItemNetObj : NetworkBehaviour
{
    [Header("物品图标")]
    public SpriteRenderer icon;
    [Header("物品根部")]
    public SortingGroup root;

    [HideInInspector, Header("物品配置")]
    public ItemConfig config;
    [Networked, OnChangedRender(nameof(ResetItem))]
    public ItemData data { get; set; } = new ItemData();

    public override void Spawned()
    {
        base.Spawned();
        if (data.Item_ID != new ItemData().Item_ID)
        {
            ResetItem();
        }
    }
    public virtual void ResetItem()
    {
        icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID);
        PlayDropAnim();
    }
    private void PlayDropAnim()
    {
        root.enabled = true;
        icon.transform.DOKill();
        icon.transform.localScale = Vector3.zero;
        UnityEngine.Random.InitState(data.Item_Seed);
        Vector3 point = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(0.2f, 0.5f);
        icon.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        icon.transform.DOLocalJump(point, 1, 1, 0.5f).OnComplete(() => 
        {
            root.enabled = false;
            icon.transform.DOPunchScale(new Vector3(0.2f, -0.2f, 0), 0.1f).SetEase(Ease.OutBack);
        }).SetEase(Ease.InOutQuad);
    }
    public virtual void PickUp(out ItemData itemData)
    {
        itemData = data;
    }

}
