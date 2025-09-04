using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemLocalObj_Hoe : ItemLocalObj
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer_Hand;
    [SerializeField, Header("锄地速度")]
    private float config_HoeSpeed = 1;
    /// <summary>
    /// 劈砍磨损
    /// </summary>
    private float HoeAbrasion;
    private float HoeAbrasion_Temp;
    /// <summary>
    /// 锄地动画时长
    /// </summary>
    private float config_HoeDuraction = 1;
    /// <summary>
    /// 锄地CD
    /// </summary>
    private float config_HoeCD;
    /// <summary>
    /// 锄地CD倒数
    /// </summary>
    private float config_HoeCDRec;
    /// <summary>
    /// 下次锄地时间
    /// </summary>
    private float float_NextHoeTiming = 0;

    private InputData inputData = new InputData();
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            MapPreviewManager.Instance.Local_HideSingal();
        }
    }
    private void FixedUpdate()
    {
        if (inputData.leftPressTimer == 0 && float_NextHoeTiming > 0)
        {
            float_NextHoeTiming -= Time.fixedDeltaTime;
        }
    }
    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        config_HoeCD = config_HoeDuraction / config_HoeSpeed;
        config_HoeCDRec = config_HoeSpeed / config_HoeDuraction;

        spriteRenderer_Hand.color = body.transform_RightHand.GetComponent<SpriteRenderer>().color;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;

        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            MapPreviewManager.Instance.Local_ShowSingal(Vector3Int.zero);
        }
        base.HoldingStart(owner, body);
    }
    public void UpdateHoeData(float hoeExpend, ItemQuality itemQuality)
    {
        HoeAbrasion = hoeExpend;
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= float_NextHoeTiming)
        {
            float_NextHoeTiming += config_HoeCD + 0.1f;
            animator.SetTrigger("Hoe");
            animator.speed = config_HoeSpeed;
        }
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override void ReleaseLeftMouse()
    {
        if (inputData.leftPressTimer > 0)
        {
            float_NextHoeTiming -= inputData.leftPressTimer;
            inputData.leftPressTimer = 0;
        }
        base.ReleaseLeftMouse();
    }
    public void Hoe()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            Vector3Int pos = actorManager.pathManager.vector3Int_CurPos;

            if (!MapManager.Instance.GetBuilding(pos, out _))
            {
                if (MapManager.Instance.GetGround(pos, out GroundTile tile))
                {
                    if(tile.tileID == 1000 || tile.tileID == 1001)
                    {
                        MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_ChangeGround()
                        {
                            groundID = 2002,
                            groundPos = pos,
                        });
                        AudioManager.Instance.Play3DEffect(2006, transform.position);
                        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BombSmoke");
                        effect.transform.position = MapManager.Instance.grid_Ground.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0);
                        AddAbrasion(HoeAbrasion);
                        return;
                    }
                }
            }
            MapPreviewManager.Instance.Local_FailSingal(0.2f);
        }
    }
    /// <summary>
    /// 累计损耗
    /// </summary>
    /// <param name="val"></param>
    public void AddAbrasion(float val)
    {
        HoeAbrasion_Temp += val;
        if (HoeAbrasion_Temp >= 1)
        {
            int offset = (int)Math.Floor(HoeAbrasion_Temp);
            HoeAbrasion_Temp = HoeAbrasion_Temp - offset;
            if (val != 0 && actorManager.actorAuthority.isPlayer)
            {
                ItemData _oldItem = itemData;
                ItemData _newItem = itemData;
                if (_newItem.Item_Durability - offset <= 0)
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Sub()
                    {
                        item = itemData,
                    });
                }
                else
                {
                    _newItem.Item_Durability -= (sbyte)offset;
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Change()
                    {
                        oldItem = _oldItem,
                        newItem = _newItem,
                    });
                }
            }
        }
    }
}
