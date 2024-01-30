using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ItemObj : MonoBehaviour
{
    [Header("ŒÔ∆∑Õº±Í")]
    public SpriteRenderer icon;
    public void Init(ItemConfig config)
    {
        icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + config.Item_ID);
    }
}
