using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ItemLocalObj_Seed : ItemLocalObj
{
    [SerializeField]
    private short plantID;
    [SerializeField, Header("²¥ÖÖËÙ¶È")]
    private float config_SowSpeed = 1;
    private float config_SowCD = 0.5f;
    private float float_NextSowTiming = 0;

    private InputData inputData = new InputData();
    private void OnEnable()
    {
        MapPreviewManager.Instance.ShowSingal(Vector3Int.zero);
    }
    private void OnDisable()
    {
        MapPreviewManager.Instance.HideSingal();
    }
    private void FixedUpdate()
    {
        if (inputData.leftPressTimer == 0 && float_NextSowTiming > 0)
        {
            float_NextSowTiming -= Time.fixedDeltaTime;
        }
    }
    public override void HoldingByHand(ActorManager owner, BodyController_Human body, ItemData data)
    {
        itemData = data;
        actorManager = owner;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        base.HoldingByHand(owner, body, data);
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= float_NextSowTiming)
        {
            float_NextSowTiming += config_SowCD + 0.1f;
            actorManager.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Pick");
            actorManager.bodyController.SetAnimatorTrigger(BodyPart.Head, "Pick");
            actorManager.bodyController.SetAnimatorAction(BodyPart.Hand, (str) =>
            {
                if (str.Equals("Pick"))
                {
                    Sow();
                }
            });
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
                        MessageBroker.Default.Publish(new MapEvent.MapEvent_State_ChangeBuildingArea()
                        {
                            buildingID = plantID,
                            buildingPos = pos,
                            areaSize = AreaSize._1X1
                        });
                        Expend(1);
                        return;
                    }
                }
            }
            MapPreviewManager.Instance.FailSingal(0.2f);
        }
    }
    private void Expend(short val)
    {
        if (itemData.Item_Count > val)
        {
            ItemData _oldItem = itemData;
            ItemData _newItem = itemData;
            _newItem.Item_Count = (short)(_newItem.Item_Count - val);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemOnHand()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySubItemOnHand()
            {
                item = itemData,
            });
        }
    }
}
