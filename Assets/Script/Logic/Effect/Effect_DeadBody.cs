using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Effect_DeadBody : EffectBase
{
    [SerializeField, Header("刚体")]
    private Rigidbody2D rigidbody_Main;
    [SerializeField, Header("身体")]
    private Transform transform_Body;
    [SerializeField, Header("头")]
    private Transform transform_Head;
    [SerializeField, Header("头发")]
    private SpriteRenderer sprite_Hair;
    public void SetBodyForce(NetworkRigidbody2D rigidbody2D,bool faceRight,bool turnRight)
    {
        rigidbody_Main.velocity = rigidbody2D.Rigidbody.velocity * 2;
        Turn(turnRight);
        Face(faceRight);
    }
    public void SetBodyFace(Sprite hair, Color32 hairColor)
    {
        sprite_Hair.sprite = hair;
        sprite_Hair.color = hairColor;
    }
    public void Turn(bool right)
    {
        if (right)
        {
            transform_Body.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform_Body.localScale = new Vector3(-1, 1, 1);
        }
    }
    public void Face(bool right)
    {
        if (right)
        {
            if (transform_Head.lossyScale.x < 0)
            {
                transform_Head.localScale = new Vector3(-transform_Head.localScale.x, 1, 1);
            }
        }
        else
        {
            if (transform_Head.lossyScale.x > 0)
            {
                transform_Head.localScale = new Vector3(-transform_Head.localScale.x, 1, 1);
            }
        }
    }

}
