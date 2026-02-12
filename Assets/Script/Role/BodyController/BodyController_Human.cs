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
    public ParticleSystem particleSystem_Dust_Left;
    public ParticleSystem particleSystem_Dust_Right;
    public ParticleSystem particleSystem_Dust_Small;
    private bool bool_StepLeft = false;
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
                PlayStep();
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
    
    public override void PlayStep()
    {
        
        if (bool_StepLeft)
        {
            if (particleSystem_Dust_Left) particleSystem_Dust_Left.Play();
            bool_StepLeft = false;
        }
        else
        {
            if (particleSystem_Dust_Right) particleSystem_Dust_Right.Play();
            bool_StepLeft = true;
        }
        if (particleSystem_Dust_Small) particleSystem_Dust_Small.Play();
        base.PlayStep();
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
}
