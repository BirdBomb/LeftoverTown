using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_Door : BuildingObj_Manmade
{
    [SerializeField]
    private GameObject obj_Door;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite sprite_Door_H_Close;
    [SerializeField]
    private Sprite sprite_Door_H_Open;
    [SerializeField]
    private Sprite sprite_Door_V_Close;
    [SerializeField]
    private Sprite sprite_Door_V_Open;
    private DoorDir doorDir;
    private DoorState doorState = DoorState.Close;
    public override void All_Draw()
    {
        ChangeDoorDir();
        ChangeDoorState(doorState);
        base.All_Draw();
    }
    private void ChangeDoorDir()
    {
        Around around = MapManager.Instance.CheckBuilding_FourSide((id) => { return id > 0; }, buildingTile.tilePos);
        if (around.U && around.D)
        {
            doorDir = DoorDir.V;
        }
        else
        {
            doorDir = DoorDir.H;
        }
    }
    private void ChangeDoorState(DoorState state)
    {
        if (doorState != state)
        {
            doorState = state;
            obj_Door.transform.DOKill();
            obj_Door.transform.localScale = Vector3.one;
            obj_Door.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            if (doorState == DoorState.Open)
            {
                AudioManager.Instance.Play3DEffect(3004, transform.position);
            }
            else
            {
                AudioManager.Instance.Play3DEffect(3005, transform.position);
            }
        }
        if (doorDir == DoorDir.V)
        {
            if (doorState == DoorState.Open)
            {
                spriteRenderer.sprite = sprite_Door_V_Open;
            }
            else
            {
                spriteRenderer.sprite = sprite_Door_V_Close;
            }
        }
        if (doorDir == DoorDir.H)
        {
            if (doorState == DoorState.Open)
            {
                spriteRenderer.sprite = sprite_Door_H_Open;
            }
            else
            {
                spriteRenderer.sprite = sprite_Door_H_Close;
            }
        }
    }
    public override bool All_ActorNearby(ActorManager actor)
    {
        ChangeDoorState(DoorState.Open);
        return true;
    }
    public override bool All_ActorFaraway(ActorManager actor)
    {
        ChangeDoorState(DoorState.Close);
        return true;
    }
    public enum DoorDir
    {
        H, V
    }
    public enum DoorState
    {
        Open, Close
    }
}
