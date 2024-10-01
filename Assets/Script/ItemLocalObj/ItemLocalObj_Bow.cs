using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemLocalObj_Bow : ItemLocalObj
{
    public Transform sprite;
    public Transform leftHand;
    /// <summary>
    /// À­¹­
    /// </summary>
    public void DrawBow()
    {
        sprite.DOKill();
        sprite.transform.localScale = Vector3.one;
        sprite.transform.localPosition = Vector3.zero;
        leftHand.transform.localPosition = Vector3.zero;
        sprite.transform.DOScale(new Vector3(1.2f, 0.8f, 1), 1f);
        sprite.transform.DOLocalMoveX(0.2f, 1f);
        leftHand.transform.DOLocalMoveX(0.2f, 1f);
    }
    /// <summary>
    /// Éä¼ý
    /// </summary>
    public void ShotBow()
    {
        sprite.DOKill();
        sprite.transform.localScale = Vector3.one;
        sprite.transform.localPosition = new Vector3(0.2f, 0, 0);
        leftHand.transform.localPosition = new Vector3(0.2f, 0, 0);
        sprite.transform.DOPunchScale(new Vector3(-0.2f, 0.2f, 0), 0.2f);
        sprite.transform.DOPunchPosition(new Vector3(0.2f, 0, 0), 0.2f);
        leftHand.transform.DOPunchPosition(new Vector3(0.2f, 0, 0), 0.2f);
    }
    /// <summary>
    /// ÊÍ·Å
    /// </summary>
    public void ReleaseBow()
    {
        sprite.transform.DOScale(Vector3.one, 0.5f);
        sprite.transform.DOLocalMoveX(0f, 0.5f);
        leftHand.transform.DOLocalMoveX(0f, 0.5f);
    }
}
