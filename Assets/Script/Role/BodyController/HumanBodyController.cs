using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.U2D;
/// <summary>
/// �������������
/// </summary>

public class HumanBodyController : BaseBodyController
{
    [Header("ͷ��")]
    public SpriteRenderer Hair;
    [Header("�۾�")]
    public SpriteRenderer Eye;
     private SpriteAtlas atlasHair;
    private SpriteAtlas atlasEye;

    public override void InitFace(int hairID, int eyeID, Color32 hairColor)
    {
        if (atlasEye == null || atlasHair == null)
        {
            atlasHair = Resources.Load<SpriteAtlas>("Atlas/HairSprite");
            atlasEye = Resources.Load<SpriteAtlas>("Atlas/EyeSprite");
        }
        Hair.sprite = atlasHair.GetSprite("Hair_" + hairID.ToString());
        Hair.color = hairColor;
        Eye.sprite = atlasEye.GetSprite("Eye_" + eyeID.ToString());
        base.InitFace(hairID, eyeID, hairColor);
    }
}
