using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.U2D;
using UniRx;
using Fusion.Addons.Physics;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.Rendering;
/// <summary>
/// 人型身体控制器
/// </summary>

public class BodyController_Human : BodyController_Base
{

    [Header("———全身———")]
    [Header("全身节点")]
    public Transform transform_Body;
    public Animator animator_Body;
    public AnimaEventListen animaEventListen_Body;
    [Header("上半部分层级")]
    public SortingGroup sortingGroup_Top;
    [Header("下半部分层级")]
    public SortingGroup sortingGroup_Down;
    [Header("手持部分层级")]
    public SortingGroup sortingGroup_HoldingItem;

    [Header("———躯体———")]
    [Header("躯体物品位置")]
    public Transform transform_ItemOnTorso;
    [HideInInspector]
    public List<GameObject> gameObjects_ItemOnTorso = new List<GameObject>();
    [Header("———双手———")]
    [Header("双手节点")]
    public Transform transform_Hand;
    [Header("右手节点")]
    public Transform transform_RightHand;
    [Header("左手节点")]
    public Transform transform_LeftHand;
    [Header("右手物品位置")]
    public Transform transform_ItemInRightHand;
    [Header("左手物品位置")]
    public Transform transform_ItemInLeftHand;
    [HideInInspector]
    public List<GameObject> gameObjects_ItemInHand = new List<GameObject>();
    public Animator animator_Hand;
    public AnimaEventListen animaEventListen_Hand;

    [Header("———头部———")]
    [Header("头部节点")]
    public Transform transform_Head;
    [Header("头部物品位置")]
    public Transform transform_ItemOnHead;
    [Header("头发")]
    public SpriteRenderer spriteRenderer_Hair;
    [Header("眼睛")]
    public SpriteRenderer spriteRenderer_Eye;
    [HideInInspector]
    public List<GameObject> gameObjects_ItemOnHead = new List<GameObject>();
    public Animator animator_Head;
    public AnimaEventListen animaEventListen_Head;

    private SpriteAtlas atlasHair;
    private SpriteAtlas atlasEye;

    public override void InitFace(int hairID, int eyeID, Color32 hairColor)
    {
        if (atlasEye == null || atlasHair == null)
        {
            atlasHair = Resources.Load<SpriteAtlas>("Atlas/HairSprite");
            atlasEye = Resources.Load<SpriteAtlas>("Atlas/EyeSprite");
        }
        spriteRenderer_Hair.sprite = atlasHair.GetSprite("Hair_" + hairID.ToString());
        spriteRenderer_Hair.color = hairColor;
        spriteRenderer_Eye.sprite = atlasEye.GetSprite("Eye_" + eyeID.ToString());
        base.InitFace(hairID, eyeID, hairColor);
    }




    public override void Start()
    {
        base.Start();
        animaEventListen_Body.BindCommonEvent((x) =>
        {
            if (x.Equals("Step"))
            {
                PlayStep();
            }
        });
        animaEventListen_Body.BindCommonEvent((x) =>
        {
            if (x.Equals("EnterWater"))
            {
                EnterWater();
            }
        });
        animaEventListen_Body.BindCommonEvent((x) =>
        {
            if (x.Equals("ExitWater"))
            {
                ExitWater();
            }
        });
    }
    public override void SetAnimatorTrigger(BodyPart bodyPart, string name)
    {
        if (bodyPart == BodyPart.Body && animator_Body != null)
        {
            animator_Body.SetTrigger(name);
        }
        else if (bodyPart == BodyPart.Hand && animator_Hand != null)
        {
            animator_Hand.SetTrigger(name);
        }
        else if (bodyPart == BodyPart.Head && animator_Head != null)
        {
            animator_Head.SetTrigger(name);
        }
        base.SetAnimatorTrigger(bodyPart, name);
    }
    public override void SetAnimatorBool(BodyPart bodyPart, string name, bool val)
    {
        if (bodyPart == BodyPart.Body && animator_Body != null)
        {
            animator_Body.SetBool(name, val);
        }
        else if (bodyPart == BodyPart.Hand && animator_Hand != null)
        {
            animator_Hand.SetBool(name, val);
        }
        else if (bodyPart == BodyPart.Head && animator_Head != null)
        {
            animator_Head.SetBool(name, val);
        }
        base.SetAnimatorBool(bodyPart, name, val);
    }
    public override void SetAnimatorFloat(BodyPart bodyPart, string name, float val)
    {
        if (bodyPart == BodyPart.Body && animator_Body != null)
        {
            animator_Body.SetFloat(name, val);
        }
        else if (bodyPart == BodyPart.Hand && animator_Hand != null)
        {
            animator_Hand.SetFloat(name, val);
        }
        base.SetAnimatorFloat(bodyPart, name, val);
    }
    public override void SetAnimatorFunc(BodyPart bodyPart, Func<string, bool> func)
    {
        if (bodyPart == BodyPart.Body && animator_Body != null)
        {
            animaEventListen_Body.BindTempFunc(func);
        }
        else if (bodyPart == BodyPart.Hand && animator_Hand != null)
        {
            animaEventListen_Hand.BindTempFunc(func);
        }
        else if (bodyPart == BodyPart.Head && animator_Head != null)
        {
            animaEventListen_Head.BindTempFunc(func);
        }
        base.SetAnimatorFunc(bodyPart, func);
    }
    public override void TurnRight()
    {
        transform_Body.localScale = new Vector3(1, 1, 1);
        base.TurnRight();
    }
    public override void TurnLeft()
    {
        transform_Body.localScale = new Vector3(-1, 1, 1);
        base.TurnLeft();
    }
    public override void FaceRight()
    {
        if (transform_Head.lossyScale.x < 0)
        {
            transform_Head.localScale = new Vector3(-transform_Head.localScale.x, 1, 1);
        }
        if (transform_Hand.lossyScale.x < 0)
        {
            transform_Hand.localScale = new Vector3(-transform_Hand.localScale.x, 1, 1);
        }
        base.FaceRight();
    }
    public override void FaceLeft()
    {
        if (transform_Head.lossyScale.x > 0)
        {
            transform_Head.localScale = new Vector3(-transform_Head.localScale.x, 1, 1);
        }
        if (transform_Hand.lossyScale.x > 0)
        {
            transform_Hand.localScale = new Vector3(-transform_Hand.localScale.x, 1, 1);
        }
        base.FaceLeft();
    }
    public override void Dead()
    {
        Effect_DeadBody deadBody = PoolManager.Instance.GetEffectObj("Effect/Effect_DeadBody").GetComponent<Effect_DeadBody>();
        deadBody.SetBodyForce(GetComponentInParent<NetworkRigidbody2D>(), faceRight, turnRight);
        deadBody.SetBodyFace(spriteRenderer_Hair.sprite,spriteRenderer_Hair.color);
        deadBody.transform.position = transform.position;
        base.Dead();
    }
    public override void Flash()
    {
        base.Flash();
    }
    public override void Shake()
    {
        transform_Body.DOKill();
        transform_Body.transform.localScale = new Vector3(transform_Body.transform.localScale.x, 1, 1);
        transform_Body.DOPunchScale(new Vector3(0, -0.1f, 0), 0.2f);
    }

    #region//手
    public void AddItemOnBothHand(GameObject itemObj, Vector3 pos, Quaternion quaternion, Vector3 scale)
    {
        itemObj.transform.SetParent(transform_Hand);
        gameObjects_ItemInHand.Add(itemObj);
        itemObj.transform.localPosition = pos;
        itemObj.transform.localRotation = quaternion;
        itemObj.transform.localScale = scale;
    }
    public void AddItemOnRightHand(GameObject itemObj, Vector3 pos, Quaternion quaternion, Vector3 scale)
    {
        itemObj.transform.SetParent(transform_ItemInRightHand);
        gameObjects_ItemInHand.Add(itemObj);
        itemObj.transform.localPosition = pos;
        itemObj.transform.localRotation = quaternion;
        itemObj.transform.localScale = scale;
    }
    public void AddItemOnLeftHand(GameObject itemObj, Vector3 pos, Quaternion quaternion, Vector3 scale)
    {
        itemObj.transform.SetParent(transform_ItemInLeftHand);
        gameObjects_ItemInHand.Add(itemObj);
        itemObj.transform.localPosition = pos;
        itemObj.transform.localRotation = quaternion;
        itemObj.transform.localScale = scale;
    }
    public void CleanItemInHand()
    {
        if (gameObjects_ItemInHand.Count > 0)
        {
            for (int i = 0; i < gameObjects_ItemInHand.Count; i++)
            {
                Destroy(gameObjects_ItemInHand[i]);
            }
        }
        transform_ItemInRightHand.localScale = Vector3.one;
        transform_ItemInRightHand.localPosition = Vector3.zero;
        transform_ItemInRightHand.localRotation = Quaternion.identity;
        transform_ItemInLeftHand.localScale = Vector3.one;
        transform_ItemInLeftHand.localPosition = Vector3.zero;
        transform_ItemInLeftHand.localRotation = Quaternion.identity;
    }

    public SpriteRenderer ShowRightHand(bool show)
    {
        SpriteRenderer spriteRenderer = transform_RightHand.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = show;
        if (show)
        {

        }
        return spriteRenderer;
    }
    public void ShowRightHandItem(short id)
    {
        if (id == 0)
        {
            transform_ItemInRightHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
            transform_ItemInRightHand.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
        else
        {
            transform_ItemInRightHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + id);
        }
    }
    public SpriteRenderer ShowLeftHand(bool show)
    {
        SpriteRenderer spriteRenderer = transform_LeftHand.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = show;
        return spriteRenderer;
    }
    public void ShowLeftHandItem(short id)
    {
        if (id == 0)
        {
            transform_ItemInLeftHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
            transform_ItemInLeftHand.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else
        {
            transform_ItemInLeftHand.GetComponent<SpriteRenderer>().sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + id);
        }
    }

    #endregion
    #region//身
    public void AddItemOnBody(GameObject itemObj, Vector3 pos, Quaternion quaternion, Vector3 scale)
    {
        itemObj.transform.SetParent(transform_ItemOnTorso);
        gameObjects_ItemOnTorso.Add(itemObj);
        itemObj.transform.localPosition = pos;
        itemObj.transform.localRotation = quaternion;
        itemObj.transform.localScale = scale;
    }
    public void ShowBodyItem(short id)
    {
        if (id == 0)
        {
            transform_ItemOnTorso.GetComponent<SpriteRenderer>().sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        }
        else
        {
            transform_ItemOnTorso.GetComponent<SpriteRenderer>().sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + id);
        }
    }
    public void CleanItemOnBody()
    {
        if (gameObjects_ItemOnTorso.Count > 0)
        {
            for (int i = 0; i < gameObjects_ItemOnTorso.Count; i++)
            {
                Destroy(gameObjects_ItemOnTorso[i]);
            }
        }
        transform_ItemOnTorso.GetComponent<SpriteRenderer>().sprite = null;

    }

    #endregion
    #region//头
    public void AddItemOnHead(GameObject itemObj, Vector3 pos, Quaternion quaternion, Vector3 scale)
    {
        itemObj.transform.SetParent(transform_ItemOnHead);
        gameObjects_ItemOnHead.Add(itemObj);
        itemObj.transform.localPosition = pos;
        itemObj.transform.localRotation = quaternion;
        itemObj.transform.localScale = scale;
    }
    public void ShowHeadItem(short id)
    {
        if (id == 0)
        {
            transform_ItemOnHead.GetComponent<SpriteRenderer>().sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        }
        else
        {
            transform_ItemOnHead.GetComponent<SpriteRenderer>().sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + id);
        }
    }
    public void CleanItemOnHead()
    {
        if (gameObjects_ItemOnHead.Count > 0)
        {
            for (int i = 0; i < gameObjects_ItemOnHead.Count; i++)
            {
                Destroy(gameObjects_ItemOnHead[i]);
            }
        }
        transform_ItemOnHead.GetComponent<SpriteRenderer>().sprite = null;

    }

    #endregion

    public override void StandOnWater(bool on)
    {
        if (bool_StandOnWater != on)
        {
            bool_StandOnWater = on;
        }
    }

    public override void EnterWater()
    {
        bool_EnterWater = true;
        sortingGroup_Root.enabled = !bool_EnterWater;
        sortingGroup_Top.enabled = bool_EnterWater;
        sortingGroup_Down.enabled = bool_EnterWater;
        sortingGroup_HoldingItem.enabled = bool_EnterWater;
        sortingGroup_HoldingItem.sortingOrder = 0;
        //trans_Offset.localPosition = bool_Swim ? new Vector3(0, -1f, 0) : Vector3.zero;
        LiquidManager.Instance.AddWave(transform.position + vector_WaveOffset, null, Vector3.one * 10);
        LiquidManager.Instance.AddWave(transform.position + vector_WaveOffset, null, Vector3.one * 5);

        GameObject waterEnter = PoolManager.Instance?.GetEffectObj("Effect/Effect_WaterEnter");
        if (waterEnter != null)
        {
            // 随机翻转
            waterEnter.transform.localScale = Vector3.one;
            waterEnter.transform.position = transform.position + new Vector3(0, 0.2f, 0);
            waterEnter.transform.localRotation = Quaternion.identity;
        }

        SetAnimatorTrigger(BodyPart.Hand, "Swim");
        SetAnimatorTrigger(BodyPart.Head, "Swim");
    }
    public override void ExitWater()
    {
        bool_EnterWater = false;
        sortingGroup_Root.enabled = !bool_EnterWater;
        sortingGroup_Top.enabled = bool_EnterWater;
        sortingGroup_Down.enabled = bool_EnterWater;
        sortingGroup_HoldingItem.enabled = bool_EnterWater;
        sortingGroup_HoldingItem.sortingOrder = 0;
        SetAnimatorTrigger(BodyPart.Hand, "ExitWater");
        SetAnimatorTrigger(BodyPart.Head, "ExitWater");
        //trans_Offset.localPosition = bool_Swim ? new Vector3(0, -1f, 0) : Vector3.zero;
    }
    public override void UpdateWave(float dt)
    {
        if (bool_EnterWater)
        {
            LiquidManager.Instance.AddWave(transform_RightHand.position);
            LiquidManager.Instance.AddWave(transform_LeftHand.position);
        }
        base.UpdateWave(dt);
    }
    public override void ShowAsRider(bool on, ActorManager vehicle)
    {
        //sortingGroup_HoldingItem.enabled = on;
        //sortingGroup_HoldingItem.sortingOrder = on ? 4 : 0;
        base.ShowAsRider(on, vehicle);
    }
}
