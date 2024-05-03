using DG.Tweening;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using static Fusion.Allocator;

public class ItemObj : MonoBehaviour
{
    [Header("��Ʒͼ��")]
    public SpriteRenderer icon;
    [HideInInspector, Header("��Ʒ��Ϣ")]
    public ItemConfig config;
    #region//�����߼�
    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="config"></param>
    public virtual void Init(ItemConfig itemConfig)
    {
        icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemConfig.Item_ID);
        config = itemConfig;
    }
    #endregion//���Ŷ���
    /// <summary>
    /// ���䶯��
    /// </summary>
    public virtual void PlayDropAnim(Vector3 dropFrom)
    {
        Vector2 offset = Random.insideUnitSphere;
        transform.DOJump(dropFrom + new Vector3(offset.x,offset.y,0), 1, 1, 0.5f);
    }


}
