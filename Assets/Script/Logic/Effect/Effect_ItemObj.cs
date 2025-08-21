using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_ItemObj : EffectBase
{
    public SpriteRenderer spriteRenderer_Icon;
    public void DrawSpriter(Sprite icon)
    {
        spriteRenderer_Icon.sprite = icon;
    }

}
