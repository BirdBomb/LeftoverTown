using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ItemLocalObj_Food : ItemLocalObj
{
    public SpriteRenderer spriteRenderer_Food;
    public SpriteAtlas spriteAtlas_Item;
    public ParticleSystem bitsParticle;
    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        spriteRenderer_Food.sprite = spriteAtlas_Item.GetSprite("Item_" + itemData.I.ToString());
        base.HoldingStart(owner, body);
    }
    public void PlayParticle()
    {
        if(bitsParticle)bitsParticle.Play();
    }
    public void StopParticle()
    {
        if (bitsParticle)bitsParticle.Stop();
    }
}
