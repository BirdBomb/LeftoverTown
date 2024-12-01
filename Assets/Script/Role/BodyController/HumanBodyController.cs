using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.U2D;
using UniRx;
/// <summary>
/// �������������
/// </summary>

public class HumanBodyController : BaseBodyController
{
    [Header("ͷ��")]
    public SpriteRenderer spriteRenderer_Hair;
    [Header("�۾�")]
    public SpriteRenderer spriteRenderer_Eye;
    [Header("ͷ")]
    public SpriteRenderer spriteRenderer_Head;
    [Header("����")]
    public SpriteRenderer spriteRenderer_Body;
    [Header("����")]
    public SpriteRenderer spriteRenderer_LeftHand;
    [Header("����")]
    public SpriteRenderer spriteRenderer_RightHand;
    [Header("���")]
    public SpriteRenderer spriteRenderer_LeftLeg;
    [Header("�ҽ�")]
    public SpriteRenderer spriteRenderer_RightLeg;

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
    public override void HideActor()         
    {
        spriteRenderer_Head.gameObject.SetActive(false);
        spriteRenderer_Body.gameObject.SetActive(false);
        spriteRenderer_LeftHand.gameObject.SetActive(false);
        spriteRenderer_RightHand.gameObject.SetActive(false);
        spriteRenderer_LeftLeg.gameObject.SetActive(false);
        spriteRenderer_RightLeg.gameObject.SetActive(false);
        base.HideActor();
    }
    public override void ShowActor()
    {
        spriteRenderer_Head.gameObject.SetActive(true);
        spriteRenderer_Body.gameObject.SetActive(true);
        spriteRenderer_LeftHand.gameObject.SetActive(true);
        spriteRenderer_RightHand.gameObject.SetActive(true);
        spriteRenderer_LeftLeg.gameObject.SetActive(true);
        spriteRenderer_RightLeg.gameObject.SetActive(true);
        base.ShowActor();
    }
}
