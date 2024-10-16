using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj_Painting : TileObj
{
    public SpriteRenderer tileSprite;
    public Sprite[] randomSprites;
    #region//»æÖÆ
    public override void Draw()
    {
        if (randomSprites.Length > 0)
        {
            tileSprite.sprite = randomSprites[new System.Random().Next(0, randomSprites.Length)];
        }
        base.Draw();
    }
    #endregion
}
