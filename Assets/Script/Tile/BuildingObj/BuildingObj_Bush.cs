using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
public class BuildingObj_Bush : BuildingObj_Growth_TwoState
{
    public override void Start()
    {
        InitializeMaterial();
        base.Start();
    }
    public override void OnDisable()
    {
        KillAllAnimations();
        base.OnDisable();
    }
    #region 序列化字段
    [Header("Sprite渲染器")]
    public SpriteRenderer spriteRenderer;
    [Header("状态精灵")]
    public Sprite[] sprites_Growing;      
    public Sprite[] sprites_Mature;      
    #endregion
    #region 私有字段
    private Material material;
    private System.Random random = new System.Random();
    // DOTween动画相关
    private Sequence sequence_ExtraShake;
    private Sequence sequence_ExtraScale;
    private Sequence sequence_Light;
    #endregion
    #region 初始化
    private void InitializeMaterial()
    {
        if (spriteRenderer != null)
        {
            material = new Material(spriteRenderer.sharedMaterial);
            spriteRenderer.material = material;
        }
    }
    #endregion
    #region 基类方法
    public override int Local_TakeDamage(int val, DamageState damageState, ActorNetManager from)
    {
        if (damageState == DamageState.AttackSlashingDamage)
        {
            return base.Local_TakeDamage(val, damageState, from);
        }
        Local_IneffectiveDamage(damageState, from);
        return 0;
    }
    public override void All_OnHpDown(int offset)
    {
        if (offset < 0)
        {
            PlayHurtEffects();
        }
        PlaySwingEffect();
    }
    #endregion
    #region 状态切换
    public override void All_ConfigureGrowingState()
    {
        Local_SetHp(int_GrowingHp);
        PlayStateChangeAnimation();
        All_SetRandomSprite(sprites_Growing);
    }

    public override void All_ConfigureMatureState()
    {
        Local_SetHp(int_MatureHp);
        PlayStateChangeAnimation();
        All_SetRandomSprite(sprites_Mature);
    }
    private void All_SetRandomSprite(Sprite[] sprites)
    {
        if (sprites != null && sprites.Length > 0 && spriteRenderer != null)
        {
            int index = random.Next(0, sprites.Length);
            spriteRenderer.sprite = sprites[index];
        }
    }

    #endregion
    #region 视效
    /// <summary>
    /// 受击
    /// </summary>
    private void PlayHurtEffects()
    {
        AudioManager.Instance?.Play3DEffect(3000, transform.position);
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f)
            .SetEase(Ease.InOutBack);

        float light = 1;
        if (sequence_Light != null) sequence_Light.Kill();

        sequence_Light = DOTween.Sequence();
        sequence_Light.Append(DOTween.To(() => light, x => light = x, 0, 0.2f).SetEase(Ease.InOutSine));
        sequence_Light.OnUpdate(() =>
        {
            material?.SetFloat("_White", light);
        });

    }
    /// <summary>
    /// 摇摆
    /// </summary>
    private void PlaySwingEffect()
    {
        const float duration = 2f;

        // 清理旧动画
        sequence_ExtraShake?.Kill();
        sequence_ExtraScale?.Kill();

        // 摇晃动画
        float shakeForce = 0f;
        sequence_ExtraShake = DOTween.Sequence();
        sequence_ExtraShake.Append(
            DOTween.To(() => shakeForce, x => shakeForce = x, 3 * Mathf.PI, duration)
                .SetEase(Ease.OutCubic)
        );
        sequence_ExtraShake.OnUpdate(() =>
        {
            material?.SetFloat("_ExtraShake", shakeForce);
        });

        // 缩放动画
        float scaleForce = 0;
        sequence_ExtraScale = DOTween.Sequence();
        sequence_ExtraScale.Append(
            DOTween.To(() => scaleForce, x => scaleForce = x, 0.1f, duration / 2f)
                .SetEase(Ease.OutCubic)
                .SetLoops(2, LoopType.Yoyo)
        );
        sequence_ExtraScale.OnUpdate(() =>
        {
            material?.SetFloat("_ExtraScale", scaleForce);
        });
    }
    /// <summary>
    /// 切换
    /// </summary>
    private void PlayStateChangeAnimation()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.1f);
    }

    #endregion
    #region 工具方法
    private void KillAllAnimations()
    {
        sequence_ExtraShake?.Kill();
        sequence_ExtraScale?.Kill();
        sequence_Light?.Kill();
        transform.DOKill();
    }
    #endregion
}

