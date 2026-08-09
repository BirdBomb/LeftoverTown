using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingObj_BoxCommon : BuildingObj_Box
{
    #region ÐòÁÐ»¯×Ö¶Î
    public Transform trans_Root;
    public SpriteRenderer spriteRenderer_Top;
    public Sprite sprite_TopClose;
    public Sprite sprite_TopOpen;
    #endregion
    #region Ïä×Ó
    public override void All_ChangeBoxState(BoxState state)
    {
        if (state == boxState) return;
        boxState = state;
        trans_Root.DOKill();
        trans_Root.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        switch (boxState)
        {
            case BoxState.Close:
                {
                    AudioManager.Instance?.Play3DEffect(3007, transform.position);
                    spriteRenderer_Top.sprite = sprite_TopClose;
                }
                break;
            case BoxState.Open:
                {
                    AudioManager.Instance?.Play3DEffect(3006, transform.position);
                    spriteRenderer_Top.sprite = sprite_TopOpen;
                }
                break;
        }
    }
    #endregion
}
