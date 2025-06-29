using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_RockMountains : BuildingObj
{
    public Sprite[] sprite;
    public SpriteRenderer spriteRenderer;
    private Material material;
    private Sequence sequence;
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
    public override void All_Draw()
    {
        spriteRenderer.sprite = sprite[GetInde(MapManager.Instance.CheckBuilding_EightSide(buildingTile.tileID, buildingTile.tilePos))];
        base.All_Draw();
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
    #region//岩石墙
    public override void All_PlayHpDown()
    {
        All_RockShake();
        All_RockFlash();
    }
    private void All_RockShake()
    {
        bitsParticle.Play();
        AudioManager.Instance.Play3DEffect(3002, transform.position);
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    private void All_RockFlash()
    {
        float light = 1;
        if (sequence != null) sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Insert(0,
            DOTween.To(() => light, x => light = x, 0, 0.2f).SetEase(Ease.InOutSine));
        sequence.OnUpdate(() =>
        { material.SetFloat("_White", light); });
    }

    public override void All_PlayBroken()
    {
        AudioManager.Instance.Play3DEffect(3003, transform.position);
        base.All_PlayBroken();
    }
    public override void All_Broken()
    {
        if (MapManager.Instance.mapNetManager.Object.HasStateAuthority)
        {
            State_CreateLootItem(State_GetLootItem(baseLootInfos, extraLootInfos));
        }
        base.All_Broken();
    }
    #endregion
}
