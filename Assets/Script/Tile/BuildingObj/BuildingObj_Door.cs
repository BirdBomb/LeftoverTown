using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_Door : BuildingObj_Manmade
{
    public Transform trans_Root;
    public GameObject gameObject_Door_H_Close;
    public GameObject gameObject_Door_H_Open;
    public GameObject gameObject_Door_V_Close;
    public GameObject gameObject_Door_V_Open;
    [Header("“ı”∞∆Ù”√")]
    public bool bool_Shadow = false;
    public PolygonCollider2D polyCollider_H;
    public PolygonCollider2D polyCollider_V;

    public enum DoorDir
    {
        H, V
    }
    public enum DoorState
    {
        Open, Close
    }

    private DoorDir doorDir;
    private DoorState doorState = DoorState.Close;
    public override void All_OnDraw()
    {
        ChangeDoorDir();
        ChangeDoorState(doorState);
        base.All_OnDraw();
    }
    private void ChangeDoorDir()
    {
        Around around = MapManager.Instance.CheckAround_Building(buildingTile.tilePos, (int id) => { return id > 0; }, DirectionType.Four);
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
        gameObject_Door_V_Open.SetActive(false);
        gameObject_Door_V_Close.SetActive(false);
        gameObject_Door_H_Open.SetActive(false);
        gameObject_Door_H_Close.SetActive(false);
        if (doorState != state)
        {
            doorState = state;
            trans_Root.transform.DOKill();
            trans_Root.transform.localScale = Vector3.one;
            trans_Root.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            if (doorState == DoorState.Open)
            {
                AudioManager.Instance.Play3DEffect(3004, transform.position);
                RemoveShadow();
            }
            else
            {
                AudioManager.Instance.Play3DEffect(3005, transform.position);
                DrawShadow();
            }
        }
        if (doorDir == DoorDir.V)
        {
            if (doorState == DoorState.Open)
            {
                gameObject_Door_V_Open.SetActive(true);
                polyCollider_V.enabled = false;
                polyCollider_H.enabled = false;
            }
            else
            {
                gameObject_Door_V_Close.SetActive(true);
                polyCollider_V.enabled = true;
                polyCollider_H.enabled = false;
            }
        }
        if (doorDir == DoorDir.H)
        {
            if (doorState == DoorState.Open)
            {
                gameObject_Door_H_Open.SetActive(true);
                polyCollider_H.enabled = false;
                polyCollider_V.enabled = false;
            }
            else
            {
                gameObject_Door_H_Close.SetActive(true);
                polyCollider_H.enabled = true;
                polyCollider_V.enabled = false;
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
    #region “ı”∞
    public override void DrawShadow()
    {
        if (!bool_Shadow) return;
        if (doorDir == DoorDir.H) ShadowManager.Instance.AddPolygons((Vector2Int)buildingTile.tilePos, polyCollider_H);
        if (doorDir == DoorDir.V) ShadowManager.Instance.AddPolygons((Vector2Int)buildingTile.tilePos, polyCollider_V);
    }
    public override void RemoveShadow()
    {
        if (!bool_Shadow) return;
        ShadowManager.Instance.RemovePolygons((Vector2Int)buildingTile.tilePos);
    }
    #endregion

}
