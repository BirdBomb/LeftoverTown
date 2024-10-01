using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TileObj_BedPart : TileObj
{
    public GameObject Bed_Part;
    public GameObject Bed_V;
    public GameObject Bed_H;
    private ActorManager owner = null;
    public void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_TimeChange>().Subscribe(_ =>
        {

        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneFindTargetTile>().Subscribe(_ =>
        {
            if (_.id == 1005)
            {
                if(!owner) 
                {
                    if (Vector3.Distance(_.pos, transform.position) < _.distance)
                    {
                        BindOwner(_.actor);
                        _.action(this);
                    }
                }
            }
        }).AddTo(this);
    }

    #region//信息更新与上传
    public override void TryToChangeInfo(string info)
    {
        base.TryToChangeInfo(info);
    }
    public override void TryToUpdateInfo(string ownerName)
    {
        base.TryToUpdateInfo(ownerName);
    }
    #endregion
    #region//床铺
    /// <summary>
    /// 绑定所有者
    /// </summary>
    /// <param name="actor"></param>
    public void BindOwner(ActorManager actor)
    {
        owner = actor;
    }
    #endregion
    #region//绘制
    public override void Draw()
    {
        if (!linkAlready)
        {
            CheckAround("WoodBed_Part", true);
        }
        base.Draw();
    }
    public override void LinkAround(LinkState linkState, TileObj linkToUp, TileObj linkToDown, TileObj linkToLeft, TileObj linkToRight)
    {
        if (!linkAlready && linkState != LinkState.NoneSide)
        {
            if (linkToRight && linkToRight.BeLink(Vector2Int.left))
            {
                linkAlready = true;
                Bed_Part.SetActive(false);
                Bed_H.SetActive(true);
            }
            else if (linkToLeft && linkToLeft.BeLink(Vector2Int.right))
            {
                linkAlready = true;
                Bed_Part.SetActive(false);
            }
            else if (linkToUp && linkToUp.BeLink(Vector2Int.down))
            {
                linkAlready = true;
                Bed_Part.SetActive(false);
                Bed_V.SetActive(true);
            }
            else if (linkToDown && linkToDown.BeLink(Vector2Int.up))
            {
                linkAlready = true;
                Bed_Part.SetActive(false);
            }
        }
        base.LinkAround(linkState, linkToUp, linkToDown, linkToLeft, linkToRight);
    }
    public override bool BeLink(Vector2Int fromPos)
    {
        if (!linkAlready)
        {
            if (fromPos == Vector2Int.up)
            {
                linkAlready = true;
                Bed_Part.SetActive(false);
                Bed_V.SetActive(true);
                return true;
            }
            if (fromPos == Vector2Int.down)
            {
                linkAlready = true;
                Bed_Part.SetActive(false);
                return true;
            }
            if (fromPos == Vector2Int.left)
            {
                linkAlready = true;
                Bed_Part.SetActive(false);
                return true;
            }
            if (fromPos == Vector2Int.right)
            {
                linkAlready = true;
                Bed_Part.SetActive(false);
                Bed_H.SetActive(true);
                return true;
            }
        }
        return base.BeLink(fromPos);
    }
    #endregion
}
