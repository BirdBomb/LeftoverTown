using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ItemLocalObj_Clothes : ItemLocalObj
{
    public SpriteRenderer spriteRenderer_Clothes;
    public SpriteAtlas spriteAtlas_Clothes;
    public override void InitData(ItemData data)
    {
        base.InitData(data);
        spriteRenderer_Clothes.sprite = spriteAtlas_Clothes.GetSprite("Item_" + itemData.I.ToString());
    }
}
