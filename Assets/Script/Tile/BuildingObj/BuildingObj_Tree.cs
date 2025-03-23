using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_Tree : BuildingObj
{
    public SpriteRenderer spriteRenderer_Tree;
    public Sprite[] sprites_treeSprite;
    public override void Start()
    {
        spriteRenderer_Tree.sprite = sprites_treeSprite[new System.Random().Next(0, sprites_treeSprite.Length)];
        base.Start();
    }
}
