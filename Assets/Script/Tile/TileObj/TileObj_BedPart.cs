using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TileObj_BedPart : TileObj
{
    public GameObject Bed_Part;
    public GameObject Bed_V;
    public GameObject Bed_H;
    [HideInInspector]
    public TileObj_BedPart link;
    private ActorManager onlyState_owner = null;
    private string onlyState_ownerName = "";
    public void Start()
    {
        LinkAround();
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_TimeChange>().Subscribe(_ =>
        {
            ListenTimeUpdate();
        }).AddTo(this);
    }
    public override void TryToUpdateInfo(string ownerName)
    {
        onlyState_ownerName = ownerName;
        if (onlyState_owner == null)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
            {
                name = onlyState_ownerName,
                pos = transform.position - Vector3.up * 0.1f,
                callBack = BindOwner
            });
        }
        base.TryToUpdateInfo(ownerName);
    }
    public void BindOwner(ActorManager actor)
    {
        onlyState_owner = actor;
        onlyState_owner.onlyState_myBed = bindTile;
    }
    public void LinkAround()
    {
        if (link == null)
        {
            if (MapManager.Instance.GetTileObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObjUp))
            {
                Debug.Log(tileObjUp.name);
                if (tileObjUp.name == "WoodBed_Part(Clone)")
                {
                    if (tileObjUp.GetComponent<TileObj_BedPart>().link == null)
                    {
                        tileObjUp.GetComponent<TileObj_BedPart>().link = this;
                        this.link = tileObjUp.GetComponent<TileObj_BedPart>();
                        tileObjUp.GetComponent<TileObj_BedPart>().Bed_Part.SetActive(false);
                        Bed_V.SetActive(true);
                        Bed_Part.SetActive(false);
                    }
                }
            }
            if (MapManager.Instance.GetTileObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObjDown))
            {
                if (tileObjDown.name == "WoodBed_Part(Clone)")
                {
                    if (tileObjDown.GetComponent<TileObj_BedPart>().link == null)
                    {
                        tileObjDown.GetComponent<TileObj_BedPart>().link = this;
                        this.link = tileObjDown.GetComponent<TileObj_BedPart>();
                        tileObjDown.GetComponent<TileObj_BedPart>().Bed_Part.SetActive(false);
                        tileObjDown.GetComponent<TileObj_BedPart>().Bed_V.SetActive(true);
                        Bed_Part.SetActive(false);
                    }
                }
            }
            if (MapManager.Instance.GetTileObj(bindTile._posInCell + Vector3Int.right, out TileObj tileObjRight))
            {
                if (tileObjRight.name == "WoodBed_Part(Clone)")
                {
                    if (tileObjRight.GetComponent<TileObj_BedPart>().link == null)
                    {
                        tileObjRight.GetComponent<TileObj_BedPart>().link = this;
                        this.link = tileObjRight.GetComponent<TileObj_BedPart>();
                        tileObjRight.GetComponent<TileObj_BedPart>().Bed_Part.SetActive(false);
                        Bed_H.SetActive(true);
                        Bed_Part.SetActive(false);
                    }
                }
                Debug.Log(tileObjRight.name);
            }
            if (MapManager.Instance.GetTileObj(bindTile._posInCell + Vector3Int.left, out TileObj tileObjLeft))
            {
                if (tileObjLeft.name == "WoodBed_Part(Clone)")
                {
                    if (tileObjLeft.GetComponent<TileObj_BedPart>().link == null)
                    {
                        tileObjLeft.GetComponent<TileObj_BedPart>().link = this;
                        this.link = tileObjLeft.GetComponent<TileObj_BedPart>();
                        tileObjLeft.GetComponent<TileObj_BedPart>().Bed_Part.SetActive(false);
                        tileObjLeft.GetComponent<TileObj_BedPart>().Bed_H.SetActive(true);
                        Bed_Part.SetActive(false);
                    }
                }
            }
        }

    }
}
