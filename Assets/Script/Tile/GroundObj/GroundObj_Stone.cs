using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObj_Stone : GroundObj
{
    public Sprite[] sprite;
    public SpriteRenderer spriteRenderer;
    public override void Draw()
    {
        int index = IndexCalculator.GetIndex
            (MapManager.Instance.CheckAround_Ground(groundTile.tilePos, (int id) => { return id == groundTile.tileID; }, DirectionType.Eight));
        spriteRenderer.sprite = sprite[index];
        base.Draw();
    }
    public override void All_ActorStandOn(ActorManager actor)
    {
        actor.bodyController.SetStep(40030);
        base.All_ActorStandOn(actor);
    }
}
