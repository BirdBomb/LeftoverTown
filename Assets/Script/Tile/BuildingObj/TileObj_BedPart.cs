using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class TileObj_BedPart : TileObj
{
    //private ActorManager owner = null;
    //#region//瓦片生命周期
    //public override void Init()
    //{
    //    MessageBroker.Default.Receive<GameEvent.GameEvent_Local_TimeChange>().Subscribe(_ =>
    //    {

    //    }).AddTo(this);
    //    MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneFindTargetTile>().Subscribe(_ =>
    //    {
    //        if (_.id == 1005)
    //        {
    //            if (!owner)
    //            {
    //                if (Vector3.Distance(_.pos, transform.position) < _.distance)
    //                {
    //                    owner = _.actor;
    //                    _.action(this);
    //                }
    //            }
    //        }
    //    }).AddTo(this);
    //    base.Init();
    //}
    //public override void Draw(int seed)
    //{
    //    if (!linkAlready)
    //    {
    //        CheckAroundBuilding_FourSide("WoodBed_Part");
    //    }
    //    base.Draw(seed);
    //}

    //#endregion
    
    //#region//绘制
    //[SerializeField]
    //private SpriteRenderer spriteRenderer;
    //[SerializeField]
    //private Sprite Bed_Part;
    //[SerializeField]
    //private Sprite Bed_V;
    //[SerializeField]
    //private Sprite Bed_H;

    //public override void LinkAround(LinkState linkState, TileObj linkToUp, TileObj linkToDown, TileObj linkToLeft, TileObj linkToRight)
    //{
    //    if (!linkAlready && linkState != LinkState.NoneSide)
    //    {
    //        if (linkToRight && linkToRight.LinkFrom(Vector2Int.left))
    //        {
    //            linkAlready = true;
    //            spriteRenderer.sprite = Bed_H;
    //        }
    //        else if (linkToLeft && linkToLeft.LinkFrom(Vector2Int.right))
    //        {
    //            linkAlready = true;
    //            spriteRenderer.sprite = null;
    //        }
    //        else if (linkToUp && linkToUp.LinkFrom(Vector2Int.down))
    //        {
    //            linkAlready = true;
    //            spriteRenderer.sprite = Bed_V;
    //        }
    //        else if (linkToDown && linkToDown.LinkFrom(Vector2Int.up))
    //        {
    //            linkAlready = true;
    //            spriteRenderer.sprite = null;
    //        }
    //    }
    //    base.LinkAround(linkState, linkToUp, linkToDown, linkToLeft, linkToRight);
    //}
    //public override bool LinkFrom(Vector2Int fromPos)
    //{
    //    if (!linkAlready)
    //    {
    //        if (fromPos == Vector2Int.up)
    //        {
    //            linkAlready = true;
    //            spriteRenderer.sprite = Bed_V;
    //            return true;
    //        }
    //        if (fromPos == Vector2Int.down)
    //        {
    //            linkAlready = true;
    //            spriteRenderer.sprite = null;
    //            return true;
    //        }
    //        if (fromPos == Vector2Int.left)
    //        {
    //            linkAlready = true;
    //            spriteRenderer.sprite = null;
    //            return true;
    //        }
    //        if (fromPos == Vector2Int.right)
    //        {
    //            linkAlready = true;
    //            spriteRenderer.sprite = Bed_H;
    //            return true;
    //        }
    //    }
    //    return base.LinkFrom(fromPos);
    //}
    //#endregion
}
