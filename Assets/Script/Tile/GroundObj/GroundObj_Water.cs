using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObj_Water : GroundObj
{
    public SpriteRenderer spriteRenderer_WaterSurface;
    public SpriteRenderer spriteRenderer_Water;

    public List<BuildingSpriteList> waterList;
    public List<BuildingSpriteList> waterSurfaceList;
    private System.Random random = new System.Random();

    public ParticleSystem particleSystem_Bubbles;
    public float float_Speed = 0.2f;

    public override void Draw()
    {
        int index = IndexCalculator.GetIndex
            (MapManager.Instance.CheckAround_Ground(groundTile.tilePos, (int id) => { return id == groundTile.tileID; }, DirectionType.Eight));
        spriteRenderer_Water.sprite = waterList[random.Next(0, waterList.Count)].sprites[index];
        DrawBubble(index);
        DrawSurface(index);
        base.Draw();

    }
    private void DrawSurface(int index)
    {
        spriteRenderer_WaterSurface.sprite = waterSurfaceList[random.Next(0, waterSurfaceList.Count)].sprites[index];
        spriteRenderer_WaterSurface.color = new Color(1, 1, 1, random.Next(5, 10) * 0.1f);
        spriteRenderer_WaterSurface.transform.localScale = new Vector3(random.Next(-1, 2), 1, 1);
    }
    private void DrawBubble(int index)
    {
        particleSystem_Bubbles.gameObject.SetActive((index != 8));
    }
    public override void All_ActorStandOn(ActorManager actor)
    {
        actor.bodyController.StandOnWater(true);
    }
    public override float All_SpeedOffset()
    {
        return float_Speed;
    }

}
