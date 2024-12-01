using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.U2D;
using UniRx;
/// <summary>
/// 人型身体控制器
/// </summary>

public class HumanBodyController : BaseBodyController
{
    [Header("头发")]
    public SpriteRenderer spriteRenderer_Hair;
    [Header("眼镜")]
    public SpriteRenderer spriteRenderer_Eye;
    [Header("头")]
    public SpriteRenderer spriteRenderer_Head;
    [Header("身体")]
    public SpriteRenderer spriteRenderer_Body;
    [Header("左手")]
    public SpriteRenderer spriteRenderer_LeftHand;
    [Header("右手")]
    public SpriteRenderer spriteRenderer_RightHand;
    [Header("左脚")]
    public SpriteRenderer spriteRenderer_LeftLeg;
    [Header("右脚")]
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
