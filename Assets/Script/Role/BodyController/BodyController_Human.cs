using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.U2D;
using UniRx;
using Fusion.Addons.Physics;
using DG.Tweening;
/// <summary>
/// 人型身体控制器
/// </summary>

public class BodyController_Human : BodyController_Base
{
    [SerializeField, Header("身体节点")]
    public Transform transform_Body;
    [SerializeField, Header("头部节点")]
    public Transform transform_Head;
    [SerializeField, Header("双手节点")]
    public Transform transform_Hand;
    [SerializeField, Header("右手节点")]
    public Transform transform_RightHand;
    [SerializeField, Header("左手节点")]
    public Transform transform_LeftHand;
    [Header("身体")]
    public SpriteRenderer spriteRenderer_Body;
    private Material material_Body;
    [Header("头发")]
    public SpriteRenderer spriteRenderer_Hair;
    [Header("眼睛")]
    public SpriteRenderer spriteRenderer_Eye;

    [SerializeField, Header("右手物品位置")]
    public Transform transform_ItemInRightHand;
    [SerializeField, Header("左手物品位置")]
    public Transform transform_ItemInLeftHand;
    [HideInInspector]
    public List<GameObject> gameObjects_ItemInHand = new List<GameObject>();
    [SerializeField, Header("头部物品位置")]
    public Transform transform_ItemOnHead;
    [HideInInspector]
    public List<GameObject> gameObjects_ItemOnHead = new List<GameObject>();
    [SerializeField, Header("身体物品位置")]
    public Transform transform_ItemOnBody;
    [HideInInspector]
    public List<GameObject> gameObjects_ItemOnBody = new List<GameObject>();


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

    public Animator animator_Body;
    public AnimaEventListen animaEventListen_Body;

    public Animator animator_Head;
    public AnimaEventListen animaEventListen_Head;

    public Animator animator_Hand;
    public AnimaEventListen animaEventListen_Hand;

    public void Start()
    {
        animaEventListen_Body.BindCommonEvent((x) => 
        {
            if (x.Equals("Step"))
            {
                AudioManager.Instance.Play3DEffect(4000,transform.position);
            }
        });
        material_Body = new Material(spriteRenderer_Body.sharedMaterial);
        spriteRenderer_Body.material = material_Body;
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
    public override void SetAnimatorAction(BodyPart bodyPart, Action<string> action)
    {
        if (bodyPart == BodyPart.Body && animator_Body != null)
        {
            animaEventListen_Body.BindTempEvent(action);
        }
        else if (bodyPart == BodyPart.Hand && animator_Hand != null)
        {
            animaEventListen_Hand.BindTempEvent(action);
        }
        else if (bodyPart == BodyPart.Head && animator_Head != null)
        {
            animaEventListen_Head.BindTempEvent(action);
        }
        base.SetAnimatorAction(bodyPart, action);
    }
    public override void Turn(bool right)
    {
        if (right)
        {
            transform_Body.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform_Body.localScale = new Vector3(-1, 1, 1);
        }
        base.Turn(right);
    }
    public override void Face(bool right)
    {
        if (right)
        {
            if (transform_Head.lossyScale.x < 0)
            {
                transform_Head.localScale = new Vector3(-transform_Head.localScale.x, 1, 1);
            }
            if (transform_Hand.lossyScale.x < 0)
            {
                transform_Hand.localScale = new Vector3(-transform_Hand.localScale.x, 1, 1);
            }
        }
        else
        {
            if (transform_Head.lossyScale.x > 0)
            {
                transform_Head.localScale = new Vector3(-transform_Head.localScale.x, 1, 1);
            }
            if (transform_Hand.lossyScale.x > 0)
            {
                transform_Hand.localScale = new Vector3(-transform_Hand.localScale.x, 1, 1);
            }
        }
        base.Face(right);
    }
    public override void Dead()
    {
        Effect_DeadBody deadBody = PoolManager.Instance.GetObject("Effect/Effect_DeadBody").GetComponent<Effect_DeadBody>();
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
        transform_Body.localScale = Vector3.one;
        transform_Body.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.2f);
    }
}
