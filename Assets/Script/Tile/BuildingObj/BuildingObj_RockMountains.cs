using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_RockMountains : BuildingObj
{

    public List<BuildingSpriteList> bodyList;
    public List<BuildingSpriteList> topList;

    public SpriteRenderer sprite_Body;
    public SpriteRenderer sprite_Top;

    private Material material;
    private System.Random random = new System.Random();
    public override void Start()
    {
        material = new Material(sprite_Top.sharedMaterial);
        sprite_Top.material = material;
        base.Start();
    }
    public override void All_OnDraw()
    {
        int index = IndexCalculator.GetIndex
            (MapManager.Instance.CheckAround_Building(buildingTile.tilePos, (int id) => { return id >= 1110 && id < 1120; }, DirectionType.Eight));

        sprite_Body.sprite = bodyList[random.Next(0, bodyList.Count)].sprites[index];
        sprite_Top.sprite = topList[random.Next(0, topList.Count)].sprites[index];



        //if (new System.Random().Next(0, 2) == 0)
        //{
        //    sprite_Body.sprite = bodyList_0[index];
        //    sprite_Top.sprite = topList_0[index];
        //}
        //else
        //{
        //    sprite_Body.sprite = bodyList_1[index];
        //    sprite_Top.sprite = topList_0[index];
        //}

        base.All_OnDraw();
    }
    public override int Local_TakeDamage(int val, DamageState damageState, ActorNetManager from)
    {
        if (damageState == DamageState.AttackBludgeoningDamage)
        {
            return base.Local_TakeDamage(val, damageState, from);
        }
        else
        {
            Local_IneffectiveDamage(damageState, from);
            return 0;
        }
    }
    #region//岩石墙
    public override void All_OnHpDown(int offset)
    {
        if (offset < 0)
        {
            AudioManager.Instance.Play3DEffect(3002, transform.position);
            All_Flash();
        }
        All_Shake();
    }
    private void All_Shake()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_RockSplash");
        effect.transform.position = transform.position;
    }
    private Sequence sequence;
    private void All_Flash()
    {
        float light = 1;
        if (sequence != null) sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Insert(0,
            DOTween.To(() => light, x => light = x, 0, 0.2f).SetEase(Ease.InOutSine));
        sequence.OnUpdate(() =>
        { material.SetFloat("_White", light); });
    }

    public override void All_OnBroken()
    {
        AudioManager.Instance.Play3DEffect(3003, transform.position);
        base.All_OnBroken();
    }
    public override void All_Broken()
    {
        if (WorldManager.Instance.gameNetManager.Object.HasStateAuthority)
        {
            State_CreateLootItem(Tool_GetFixedItemList(LootItemConfigData.GetLootFixedConfig(buildingTile.tileID).Loot_List));
        }
        base.All_Broken();
    }
    #endregion
    #region//阴影
    [Header("阴影启用")]
    public bool bool_Shadow = false;
    public PolygonCollider2D polyCollider;
    public override void DrawShadow()
    {
        if (!bool_Shadow) return;
        ShadowManager.Instance.AddPolygons((Vector2Int)buildingTile.tilePos, polyCollider);
    }
    public override void RemoveShadow()
    {
        if (!bool_Shadow) return;
        ShadowManager.Instance.RemovePolygons((Vector2Int)buildingTile.tilePos);
    }
    #endregion
}
