using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ItemLocalObj_Food : ItemLocalObj
{
    public SpriteRenderer spriteRenderer_Food;
    public SpriteAtlas spriteAtlas_Item;
    public ParticleSystem bitsParticle;
    public override void HoldingByHand(ActorManager owner, BodyController_Human body, ItemData data)
    {
        actorManager = owner;
        itemData = data;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        spriteRenderer_Food.sprite = spriteAtlas_Item.GetSprite("Item_" + data.Item_ID.ToString());
        base.HoldingByHand(owner, body, data);
    }
    public void PlayParticle()
    {
        bitsParticle.Play();
    }
    public void StopParticle()
    {
        bitsParticle.Stop();
    }
}
