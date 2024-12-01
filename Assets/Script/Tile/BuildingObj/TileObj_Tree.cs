using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileObj_Tree : TileObj
{
    #region
    public SpriteRenderer Tree;
    public Sprite[] treeSprite;
    public override void Draw(int seed)
    {
        Tree.sprite = treeSprite[new System.Random().Next(0, treeSprite.Length)];
        base.Draw(seed);
    }
    #endregion
}
