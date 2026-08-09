using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObj_Snowland : GroundObj
{
    public List<BuildingSpriteList> topList;
    public List<BuildingSpriteList> bodyList;
    public SpriteRenderer sprite_Top;
    public SpriteRenderer sprite_Body;
    private System.Random random = new System.Random();
    public override void Draw()
    {
        int index = IndexCalculator.GetIndex
            (MapManager.Instance.CheckAround_Ground(groundTile.tilePos, (int id) => { return id == groundTile.tileID; }, DirectionType.Eight));
        sprite_Body.sprite = bodyList[random.Next(0, bodyList.Count)].sprites[index];
        sprite_Top.sprite = topList[random.Next(0, topList.Count)].sprites[index];
        base.Draw();
    }
    public override void All_ActorStandOn(ActorManager actor)
    {
        actor.bodyController.SetStep(40020);
        base.All_ActorStandOn(actor);
    }
}
