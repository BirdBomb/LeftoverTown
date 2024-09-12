using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj_Floor : TileObj
{
    public SpriteRenderer tileSprite;
    public Sprite[] randomSprites;
    public override void Init(MyTile tile)
    {
        if (randomSprites.Length > 0)
        {
            tileSprite.sprite = randomSprites[new System.Random().Next(0, randomSprites.Length)];
        }
        base.Init(tile);
    }
}
