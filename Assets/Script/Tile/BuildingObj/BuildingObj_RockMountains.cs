using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_RockMountains : BuildingObj
{
    public Sprite[] spriteList_0;
    public Sprite[] spriteList_1;
    public SpriteRenderer spriteRenderer;
    private Material material;
    public ParticleSystem bitsParticle;
    [SerializeField, Header("常态基本掉落物")]
    private List<BaseLootInfo> baseLootInfos = new List<BaseLootInfo>();
    [SerializeField, Header("常态额外掉落物")]
    private List<ExtraLootInfo> extraLootInfos = new List<ExtraLootInfo>();
    public override void Start()
    {
        material = new Material(spriteRenderer.sharedMaterial);
        spriteRenderer.material = material;
    }
    public override void Init(int id)
    {
        DrawShadow();
        base.Init(id);
    }
    public void OnDisable()
    {
        RemoveShadow();
    }
    public override void All_OnDraw()
    {
        int index = GetInde(MapManager.Instance.CheckBuilding_EightSide(1110, 1120, buildingTile.tilePos));
        if (new System.Random().Next(0, 2) == 0)
        {
            spriteRenderer.sprite = spriteList_0[index];
        }
        else
        {
            spriteRenderer.sprite = spriteList_1[index];
        }

        base.All_OnDraw();
    }
    private int GetInde(Around aroundState)
    {
        int val = -1;
        if (aroundState.U)
        {
            if (aroundState.D)
            {
                if (aroundState.L)
                {
                    if (aroundState.R)
                    {
                        if (aroundState.UL)
                        {
                            if (aroundState.UR)
                            {
                                if (aroundState.DL)
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 8;
                                    }
                                    else
                                    {
                                        val = 11;
                                    }
                                }
                                else
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 12;
                                    }
                                    else
                                    {
                                        val = 37;
                                    }
                                }
                            }
                            else
                            {
                                if (aroundState.DL)
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 18;
                                    }
                                    else
                                    {
                                        val = 44;
                                    }
                                }
                                else
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 34;
                                    }
                                    else
                                    {
                                        val = 46;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (aroundState.UR)
                            {
                                if (aroundState.DL)
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 19;
                                    }
                                    else
                                    {
                                        val = 27;
                                    }
                                }
                                else
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 38;
                                    }
                                    else
                                    {
                                        val = 39;
                                    }
                                }
                            }
                            else
                            {
                                if (aroundState.DL)
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 45;
                                    }
                                    else
                                    {
                                        val = 47;
                                    }
                                }
                                else
                                {
                                    if (aroundState.DR)
                                    {
                                        val = 40;
                                    }
                                    else
                                    {
                                        val = 20;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (aroundState.UL)
                        {
                            if (aroundState.DL)
                            {
                                val = 9;
                            }
                            else
                            {
                                val = 24;
                            }
                        }
                        else
                        {
                            if (aroundState.DL)
                            {
                                val = 29;
                            }
                            else
                            {
                                val = 33;
                            }
                        }
                    }
                }
                else
                {
                    if (aroundState.R)
                    {
                        if (aroundState.UR)
                        {
                            if (aroundState.DR)
                            {
                                val = 7;
                            }
                            else
                            {
                                val = 21;
                            }
                        }
                        else
                        {
                            if (aroundState.DR)
                            {
                                val = 30;
                            }
                            else
                            {
                                val = 25;
                            }
                        }
                    }
                    else
                    {
                        val = 10;
                    }
                }
            }
            else
            {
                if (aroundState.L)
                {
                    if (aroundState.R)
                    {
                        if (aroundState.UL)
                        {
                            if (aroundState.UR)
                            {
                                val = 15;
                            }
                            else
                            {
                                val = 28;
                            }
                        }
                        else
                        {
                            if (aroundState.UR)
                            {
                                val = 31;
                            }
                            else
                            {
                                val = 32;
                            }
                        }

                    }
                    else
                    {
                        if (aroundState.UL)
                        {
                            val = 16;
                        }
                        else
                        {
                            val = 43;
                        }
                    }
                }
                else
                {
                    if (aroundState.R)
                    {
                        if (aroundState.UR)
                        {
                            val = 14;
                        }
                        else
                        {
                            val = 42;
                        }
                    }
                    else
                    {
                        val = 17;
                    }
                }
            }
        }
        else
        {
            if (aroundState.D)
            {
                if (aroundState.L)
                {
                    if (aroundState.R)
                    {
                        if (aroundState.DL)
                        {
                            if (aroundState.DR)
                            {
                                val = 1;
                            }
                            else
                            {
                                val = 23;
                            }
                        }
                        else
                        {
                            if (aroundState.DR)
                            {
                                val = 22;
                            }
                            else
                            {
                                val = 26;
                            }
                        }
                    }
                    else
                    {
                        if (aroundState.DL)
                        {
                            val = 2;
                        }
                        else
                        {
                            val = 36;
                        }
                    }
                }
                else
                {
                    if (aroundState.R)
                    {
                        if (aroundState.DR)
                        {
                            val = 0;
                        }
                        else
                        {
                            val = 35;
                        }
                    }
                    else
                    {
                        val = 3;
                    }
                }
            }
            else
            {
                if (aroundState.L)
                {
                    if (aroundState.R)
                    {
                        val = 5;
                    }
                    else
                    {
                        val = 6;
                    }
                }
                else
                {
                    if (aroundState.R)
                    {
                        val = 4;
                    }
                    else
                    {
                        val = 13;
                    }
                }
            }
        }
        return val;
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
        bitsParticle.Play();
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
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
            State_CreateLootItem(State_GetLootItem(baseLootInfos, extraLootInfos));
        }
        base.All_Broken();
    }
    #endregion
    #region//阴影
    public PolygonCollider2D polyCollider;
    private void DrawShadow()
    {
        ShadowManager.Instance.AddCollider((Vector2Int)buildingTile.tilePos, polyCollider);
    }
    private void RemoveShadow()
    {
        ShadowManager.Instance.RemoveCollider((Vector2Int)buildingTile.tilePos);
    }
    #endregion
}
