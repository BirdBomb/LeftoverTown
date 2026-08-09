using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ItemLocalObj_Seed : ItemLocalObj
{
    [SerializeField]
    private short plantID;
    [SerializeField, Header("播种速度")]
    private float config_SowSpeed = 1;
    private float config_SowCD = 0.5f;
    private float float_NextSowTiming = 0;

    private InputData inputData = new InputData();
    private void OnDisable()
    {
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            MapPreviewManager.Instance.Local_HideSingal();
        }
    }
    private void FixedUpdate()
    {
        if (inputData.leftPressTimer == 0 && float_NextSowTiming > 0)
        {
            float_NextSowTiming -= Time.fixedDeltaTime;
        }
    }
    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;
        body.AddItemOnRightHand(gameObject, Vector3.zero, Quaternion.identity, Vector3.one);
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            MapPreviewManager.Instance.Local_ShowSingal(Vector3Int.zero);
        }
        base.HoldingStart(owner, body);
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= float_NextSowTiming)
        {
            float_NextSowTiming += config_SowCD + 0.1f;
            actorManager.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Pick");
            actorManager.bodyController.SetAnimatorTrigger(BodyPart.Head, "Pick");
            actorManager.bodyController.SetAnimatorFunc(BodyPart.Hand, TryToSow);
        }
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override void ReleaseLeftMouse()
    {
        if (inputData.leftPressTimer > 0)
        {
            float_NextSowTiming -= inputData.leftPressTimer;
            inputData.leftPressTimer = 0;
        }
        base.ReleaseLeftMouse();
    }
    public bool TryToSow(string str)
    {
        if (str.Equals("Pick"))
        {
            Sow();
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Sow()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            Vector3Int pos = actorManager.pathManager.vector3Int_CurPos;
            if (!MapManager.Instance.GetBuilding(pos, out _))
            {
                if (MapManager.Instance.GetGround(pos, out GroundTile tile))
                {
                    if (tile.tileID == 2002)
                    {
                        MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_CreateBuildingArea()
                        {
                            buildingID = plantID,
                            buildingPos = pos,
                            areaSize = AreaSize._1X1
                        });
                        Expend(1);
                        return;
                    }
                    else
                    {
                        actorManager.actionManager.AllClient_SendText("我需要一个正确的地块", (int)Emoji.Yell, 1, false, 1);
                    }
                }
            }
            MapPreviewManager.Instance.Local_FailSingal(0.2f);
        }
    }
    private void Expend(short val)
    {
        if (itemData.C > val)
        {
            ItemData _oldItem = itemData;
            ItemData _newItem = itemData;
            _newItem.C = (short)(_newItem.C - val);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Change()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Sub()
            {
                item = itemData,
            });
        }
    }
}
